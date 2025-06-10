using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PHEDChhattisgarh
{
    public class ParamInfo
    {
        public string code { get; set; }
        public string label { get; set; }
    }

    public partial class eMBEntry : System.Web.UI.Page
    {
        // Get connection string from web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["eMB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            if (ddlFormula.SelectedIndex > 0)
                RebuildParameterInputs();

            if (!IsPostBack)
            {
                if (!ValidateQueryParams())
                {
                    Response.Redirect("ComponentList.aspx");
                    return;
                }

                string workCode = Request.QueryString["WorkCode"];
                string componentID = Request.QueryString["ComponentID"];
                var sorItemNo = Request.QueryString["SORItemNo"];

                string sorItemName = "";
                using (var cn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    @"SELECT SORSubItem 
                    FROM [JJM].[dbo].[eMB_ComponentMaterialsEntry]
                    WHERE Work_Code = @WorkCode
                    AND ComponentID = @ComponentID
                    AND SORItemNo = @sorno", cn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", workCode);
                    cmd.Parameters.AddWithValue("@ComponentID", componentID);
                    cmd.Parameters.AddWithValue("@sorno", sorItemNo);
                    cn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        sorItemName = result.ToString();
                }

                lblSORItem.Text = sorItemName;
                GenerateUniqueEmbID();

                PopulateFormulas();
                LoadWorkDetailsGrid();
                LoadExistingEntries();
            }
        }

        private void RebuildParameterInputs()
        {
            tblParams.Controls.Clear();

            int formulaId;
            if (!int.TryParse(ddlFormula.SelectedValue, out formulaId) || formulaId == 0)
                return;

            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT parameters, units, expression FROM [JJM].[dbo].[Formula] WHERE formula_id=@id", cn))
            {
                cmd.Parameters.AddWithValue("@id", formulaId);
                cn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        var rawParams = rdr["parameters"].ToString();
                        var serializer = new JavaScriptSerializer();
                        var paramInfos = serializer.Deserialize<List<ParamInfo>>(rawParams);

                        string[] unitArray = rdr["units"].ToString().Trim('[', ']').Replace("\"", "").Split(',');
                         var unitList = new List<string>();
                                    foreach (string u in unitArray)
                                    {
                                        unitList.Add(u.Trim());
                                    }

                        string formulaExpression = rdr["expression"].ToString();
                        int currentFormulaId = int.Parse(ddlFormula.SelectedValue);
                        int num = 0;
                        foreach (var p in paramInfos)
                        {
                            var tdLabel = new HtmlTableCell
                            {
                                InnerText = p.label
                            };
                            tdLabel.Style["vertical-align"] = "middle";

                            var tdInput = new HtmlTableCell();
                            if (currentFormulaId == 11 && p.code == "P")
                            {
                                var ddl = new DropDownList
                                {
                                    ID = "ddl_" + p.code + num,
                                    CssClass = "form-control"
                                };
                                ddl.Attributes["data-param"] = p.code;
                                ddl.Attributes["onchange"] = "recalculateFormula();";

                                // Add predefined options
                                ddl.Items.AddRange(new[]
                                {
                                    new ListItem("0", "0"),
                                    new ListItem("5", "5"),
                                    new ListItem("15", "15"),
                                    new ListItem("25", "25"),
                                    new ListItem("35", "35"),
                                    new ListItem("60", "60"),
                                    new ListItem("75", "75"),
                                    new ListItem("90", "90"),
                                    new ListItem("100", "100")
                                });

                                tdInput.Controls.Add(ddl);
                            }
                            else // Regular textbox for other parameters
                            {
                                var tb = new TextBox
                                {
                                    ID = "tb_" + p.code + num,
                                    CssClass = "form-control"
                                };
                                tb.Attributes["data-param"] = p.code;
                                tb.Attributes["oninput"] = "recalculateFormula();";
                                tdInput.Controls.Add(tb);
                                tdInput.Style["background"] = "#f8f9fa";
                            }
                            var tr = new HtmlTableRow();
                            tr.Cells.Add(tdLabel);
                            tr.Cells.Add(tdInput);

                            tblParams.Controls.Add(tr);
                            num++;
                        }

                        string selected = ddlUnit.SelectedValue;
                        ddlUnit.Items.Clear();
                        ddlUnit.Items.Add(new ListItem("-- Select Unit --", ""));
                        foreach (var u in unitList)
                            ddlUnit.Items.Add(new ListItem(u, u));

                        if (!string.IsNullOrEmpty(selected))
                        {
                            var item = ddlUnit.Items.FindByValue(selected);
                            if (item != null)
                            {
                                ddlUnit.ClearSelection();
                                item.Selected = true;
                            }
                        }

                        ScriptManager.RegisterStartupScript(
                            this,
                            this.GetType(),
                            "storeFormulaExpr",
                            "window.formulaExpr = '" + formulaExpression.Replace("'", "\\'") + "';",
                            true
                        );
                    }
                }
            }
        }

        private void PopulateFormulas()
        {
            var dt = new DataTable();
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT formula_id, name, parameters, units FROM [JJM].[dbo].[Formula]", cn))
            using (var da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }

            // Clear DataSourceID to avoid conflict
            ddlFormula.DataSourceID = null;

            ddlFormula.DataSource = dt;
            ddlFormula.DataTextField = "name";
            ddlFormula.DataValueField = "formula_id";
            ddlFormula.DataBind();

            ddlFormula.Items.Insert(0, new ListItem("-- Select Formula --", "0"));
        }

        protected void ddlFormula_SelectedIndexChanged(object sender, EventArgs e)
        {
            RebuildParameterInputs();
        }

        private bool ValidateQueryParams()
        {
            return !string.IsNullOrEmpty(Request.QueryString["WorkCode"]) &&
                   !string.IsNullOrEmpty(Request.QueryString["AgreementBy"]) &&
                   !string.IsNullOrEmpty(Request.QueryString["YearOfAgreement"]) &&
                   !string.IsNullOrEmpty(Request.QueryString["AgreementNo"]) &&
                   !string.IsNullOrEmpty(Request.QueryString["ComponentID"]);
        }

        private void GenerateUniqueEmbID()
        {
            string workCode = Request.QueryString["WorkCode"] ?? "";
            string componentID = Request.QueryString["ComponentID"];
            string sorItemNo = Request.QueryString["SORItemNo"]
                             ?? hdnSORItemNo.Value
                             ?? "";

            var now = DateTime.Now;
            string dateStr = now.ToString("yyyyMMdd");
            string timeStr = now.ToString("HHmmss");

            // 3) Compose the UniqueEmbID
            //    Format: <WorkCode>.<componentID>.(<SORItemNo>).<YYYYMMDD>.<HHMMSS>
            string uniqueEmbID = string.Format("{0}-{1}-({2})-{3}-{4}", workCode, componentID, sorItemNo, dateStr, timeStr);

            lblUniqueEmbID.Text = uniqueEmbID;
            hdnUniqueEmbID.Value = uniqueEmbID;
        }

        private void LoadWorkDetailsGrid()
        {
            string workCode = Request.QueryString["WorkCode"];
            string componentId = Request.QueryString["ComponentID"];
            string yoa = Request.QueryString["YearOfAgreement"];
            string ab = Request.QueryString["AgreementBy"];
            string SORItemNo = Request.QueryString["SORItemNo"];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT DISTINCT
                                p.Work_Code, 
                                w.name_of_work_Eng AS WorkName,
                                cm.ComponentName,
                                cme.SORItem,
                                cme.SORSubItem,
                                cme.Qty
                            FROM [JJM].[dbo].[eMB_ProgressOfScheme] p
                            LEFT JOIN[JJM].[dbo].[Work_Master] w ON p.Work_Code = w.PKWorkCode
                            LEFT JOIN eMB_ComponentMaster cm
                                ON cm.ComponentID = @ComponentID
                            INNER JOIN [JJM].[dbo].[eMB_ComponentMaterialsEntry] cme
                               ON p.Work_Code = cme.Work_Code
                                AND p.Year_of_Agreement = cme.Year_of_Agreement
                                AND p.AgreementBy = cme.AgreementBy
                            WHERE
                                cme.Work_Code = @WorkCode
                                AND cme.Year_of_Agreement = @yoa
                                AND cme.AgreementBy = @ab
                                AND cme.ComponentID = @ComponentID
                                AND cme.SORItemNo = @SORItemNo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", workCode);
                    cmd.Parameters.AddWithValue("@ComponentID", componentId);
                    cmd.Parameters.AddWithValue("@yoa", yoa);
                    cmd.Parameters.AddWithValue("@ab", ab);
                    cmd.Parameters.AddWithValue("@SORItemNo", SORItemNo);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvWorkDetails.DataSource = dt;
                    gvWorkDetails.DataBind();
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        // Log error
                        System.Diagnostics.Debug.WriteLine("Error loading particulars and unit: " + ex.Message);
                    }
                }
            }
        }

        private void LoadExistingEntries()
        {
            // 1) Read all the IDs from query-string / hidden fields
            string workCode = Request.QueryString["WorkCode"];
            string agreementBy = Request.QueryString["AgreementBy"];
            string yearOfAgreement = Request.QueryString["YearOfAgreement"];
            string agreementNo = Request.QueryString["AgreementNo"];
            string componentId = Request.QueryString["ComponentID"];
            string sorItemNo = Request.QueryString["SORItemNo"];

            if (string.IsNullOrEmpty(workCode)
             || string.IsNullOrEmpty(sorItemNo))
            {
                gvEntries.DataSource = null;
                gvEntries.DataBind();
                return;
            }

            var dt = new DataTable();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT 
                        EmbId,
                        SORItemNo,
                        Remark,
                        UniqueEmbID,
                        Inputs,
                        ActualUnit,
                        ResultValue,
                        IsCurrent
                    FROM [JJM].[dbo].[eMB_Entry]
                    WHERE 
                        WorkCode             = @WorkCode
                      AND AgreementBy          = @AgreementBy
                      AND YearOfAgreement      = @YearOfAgreement
                      AND AgreementNo          = @AgreementNo
                      AND ComponentID          = @ComponentID
                      AND SORItemNo            = @SORItemNo
                      AND Deleted              = 0
                      AND IsCurrent            = 1
                    ORDER BY UniqueEmbID DESC";

                cmd.Parameters.AddWithValue("@WorkCode", workCode);
                cmd.Parameters.AddWithValue("@AgreementBy", agreementBy);
                cmd.Parameters.AddWithValue("@YearOfAgreement", yearOfAgreement);
                cmd.Parameters.AddWithValue("@AgreementNo", agreementNo);
                cmd.Parameters.AddWithValue("@ComponentID", componentId);
                cmd.Parameters.AddWithValue("@SORItemNo", sorItemNo);

                conn.Open();
                new SqlDataAdapter(cmd).Fill(dt);
            }

            gvEntries.DataSource = dt;
            gvEntries.DataBind();
        }

        protected string FormatInputs(string inputsJson)
        {
            if (string.IsNullOrEmpty(inputsJson))
                return "";

            try
            {
                var serializer = new JavaScriptSerializer();
                var dict = serializer.Deserialize<Dictionary<string, decimal>>(inputsJson);
                var formattedPairs = new List<string>();
                foreach (var kv in dict)
                {
                    formattedPairs.Add(string.Format("{0}={1}", kv.Key, kv.Value.ToString(CultureInfo.InvariantCulture)));
                }
                return string.Join(", ", formattedPairs.ToArray());
            }
            catch
            {
                return inputsJson;
            }
        }

        protected string ExtractDateFromEmbID(string uniqueEmbID)
        {
            if (string.IsNullOrEmpty(uniqueEmbID))
                return "";

            try
            {
                // Split by dots and get the second-to-last part (date part)
                string[] parts = uniqueEmbID.Split('-');
                if (parts.Length >= 2)
                {
                    string datePart = parts[parts.Length - 2]; // 20250521
                    if (datePart.Length == 8)
                    {
                        // Convert YYYYMMDD to DD/MM/YYYY
                        string year = datePart.Substring(0, 4);
                        string month = datePart.Substring(4, 2);
                        string day = datePart.Substring(6, 2);
                        return string.Format("{0}/{1}/{2}", day, month, year);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error extracting date from " + uniqueEmbID + " :" + ex.Message);
            }

            return "";
        }

        protected string ExtractTimeFromEmbID(string uniqueEmbID)
        {
            if (string.IsNullOrEmpty(uniqueEmbID))
                return "";

            try
            {
                // Split by dots and get the last part (time part)
                string[] parts = uniqueEmbID.Split('-');
                if (parts.Length >= 1)
                {
                    string timePart = parts[parts.Length - 1]; // 164738
                    if (timePart.Length == 6)
                    {
                        // Convert HHMMSS to HH:MM:SS
                        string hour = timePart.Substring(0, 2);
                        string minute = timePart.Substring(2, 2);
                        string second = timePart.Substring(4, 2);
                        return hour + ":" + minute + ":" + second;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error extracting time from " + uniqueEmbID + ":" + ex.Message);
            }

            return "";
        }
        private static readonly object _syncLock = new object();

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // 1) Validation
            if (ddlFormula.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Please select a formula.');", true);
                return;
            }
            if (string.IsNullOrEmpty(ddlUnit.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "alert('Please select a unit.');", true);
                return;
            }

            // 2) Read fixed fields
            bool isEditMode = !string.IsNullOrEmpty(hdnEditEmbId.Value);
            string workCode = Request.QueryString["WorkCode"];
            string agreementBy = Request.QueryString["AgreementBy"];
            string yearOfAgreement = Request.QueryString["YearOfAgreement"];
            string agreementNo = Request.QueryString["AgreementNo"];
            string componentId = Request.QueryString["ComponentID"];
            string sorSubItem = Request.QueryString["SORSubItem"];
            string sorItemNo = Request.QueryString["SORItemNo"];
            string sorItem = lblSORItem.Text;
            string uniqueEmbID = hdnUniqueEmbID.Value;
            string remarks = txtRemarks.Text;
            string userId = Session["UserId"] != null ? Session["UserId"].ToString() : null;

            int formulaId = int.Parse(ddlFormula.SelectedValue);
            string actualUnit = ddlUnit.SelectedValue;

            // 3) Gather dynamic parameter inputs from tblParams
            var inputs = new Dictionary<string, decimal>();
            foreach (HtmlTableRow row in tblParams.Controls)
            {
                Control control = row.Cells[1].Controls[0]; // First control in the cell
                string param = null;
                string stringValue = null;

                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    param = textBox.Attributes["data-param"];
                    stringValue = textBox.Text;
                }
                else if (control is DropDownList) // Handle dropdown for percentage
                {
                    DropDownList dropDown = (DropDownList)control;
                    param = dropDown.Attributes["data-param"];
                    stringValue = dropDown.SelectedValue;
                }

                if (param != null)
                {
                    decimal val;
                    // Parse value and add to inputs dictionary
                    if (!decimal.TryParse(stringValue, NumberStyles.Any,
                        CultureInfo.InvariantCulture, out val))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                            "alert('Invalid value for parameter " + param + ". Please enter a valid number.');", true);
                        return;
                    }
                    inputs[param] = val;
                }
            }

            // 4) Load and substitute expression
            string expression;
            using (var cn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                "SELECT expression FROM [JJM].[dbo].[Formula] WHERE formula_id = @fid", cn))
            {
                cmd.Parameters.AddWithValue("@fid", formulaId);
                cn.Open();
                string resp = (string)cmd.ExecuteScalar();
                expression = resp;
            }

            // Replace pi with its value
            string evalExpr = expression.Replace("pi", Math.PI.ToString(CultureInfo.InvariantCulture));

            // Replace each parameter with its value
            foreach (var kv in inputs)
            {
                string pat = @"\b" + Regex.Escape(kv.Key) + @"\b";
                evalExpr = Regex.Replace(
                    evalExpr,
                    pat,
                    kv.Value.ToString(CultureInfo.InvariantCulture),
                    RegexOptions.CultureInvariant);
            }

            // 5) Calculate result
            decimal resultValue;
            decimal tempValue;

            // Check if expression is just a number after substitution
            if (decimal.TryParse(evalExpr, NumberStyles.Any, CultureInfo.InvariantCulture, out tempValue))
            {
                // Simple case: expression became just a number (like "L" -> "5.0")
                resultValue = tempValue;
            }
            else
            {
                // Complex expression: needs mathematical evaluation
                // Handle ^2 patterns first
                while (evalExpr.Contains("^2"))
                {
                    Match match = Regex.Match(evalExpr, @"(?<num>\d+(\.\d+)?)\s*\^\s*2");
                    if (match.Success)
                    {
                        string num = match.Groups["num"].Value;
                        string replacement = "(" + num + "*" + num + ")";
                        evalExpr = evalExpr.Replace(match.Value, replacement);
                    }
                    else
                    {
                        break;
                    }
                }

                try
                {
                    // Evaluate using DataColumn
                    var table = new DataTable();
                    var calcCol = new DataColumn("Calc", typeof(double), evalExpr);
                    table.Columns.Add(calcCol);
                    var rowEval = table.NewRow();
                    table.Rows.Add(rowEval);
                    double raw = (double)rowEval["Calc"];
                    resultValue = Convert.ToDecimal(raw, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                        "alert('Error calculating formula: " + ex.Message.Replace("'", "\\'") + "');", true);
                    return;
                }
            }

            // Show result in textbox
            txtResult.Text = resultValue.ToString("0.######", CultureInfo.InvariantCulture);

            // 6) Serialize inputs to JSON (compatible way)
            var jsonParts = new List<string>();
            foreach (var kv in inputs)
            {
                jsonParts.Add("\"" + kv.Key + "\":" + kv.Value.ToString(CultureInfo.InvariantCulture));
            }
            string inputsJson = "{" + string.Join(",", jsonParts.ToArray()) + "}";

            // 7) Database transaction
            lock (_syncLock)
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var tx = conn.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (var cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = tx;

                                if (isEditMode)
                                {
                                    // Edit mode: optimistic concurrency check
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = @"
                                SELECT EntryGroupId, Revision, Version 
                                FROM [JJM].[dbo].[eMB_Entry] 
                                WHERE EmbId = @EmbId AND IsCurrent = 1 AND Deleted = 0";
                                    cmd.Parameters.AddWithValue("@EmbId", hdnEditEmbId.Value);

                                    int entryGroupId, oldRevision;
                                    byte[] versionBytes;

                                    using (var rdr = cmd.ExecuteReader())
                                    {
                                        if (rdr.Read())
                                        {
                                            entryGroupId = rdr["EntryGroupId"] != DBNull.Value
                                                ? Convert.ToInt32(rdr["EntryGroupId"])
                                                : Convert.ToInt32(hdnEditEmbId.Value);
                                            oldRevision = Convert.ToInt32(rdr["Revision"]);
                                            versionBytes = (byte[])rdr["Version"];
                                        }
                                        else
                                        {
                                            tx.Rollback();
                                            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                                "alert('The entry to edit could not be found.');", true);
                                            return;
                                        }
                                    }

                                    // Update old record (use version for concurrency check)
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = @"
                                UPDATE [JJM].[dbo].[eMB_Entry] 
                                SET IsCurrent = 0 
                                WHERE EmbId = @EmbId AND Version = @Version";
                                    cmd.Parameters.AddWithValue("@EmbId", hdnEditEmbId.Value);
                                    cmd.Parameters.AddWithValue("@Version", versionBytes);

                                    int rowsUpdated = cmd.ExecuteNonQuery();
                                    if (rowsUpdated == 0)
                                    {
                                        tx.Rollback();
                                        ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                            "alert('The entry was modified by another user. Please reload and try again.');", true);
                                        return;
                                    }

                                    // Insert new version
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = @"
                                INSERT INTO eMB_Entry (WorkCode, AgreementBy, YearOfAgreement, AgreementNo,
                                    ComponentID, SORItemNo, SORSubItem,
                                    FormulaID, Inputs, ActualUnit, ResultValue, Remark, UniqueEmbID, Units, [Date], 
                                    Deleted, EntryGroupId, Revision, IsCurrent, userId)
                                VALUES (@WorkCode, @AgreementBy, @YearOfAgreement, @AgreementNo,
                                    @ComponentID, @SORItemNo, @SORSubItem,
                                    @FormulaID, @Inputs, @ActualUnit, @ResultValue, @Remark, @UniqueEmbID, @Units, 
                                    GETDATE(), 0, @EntryGroupId, @Revision, 1, @userId)";

                                    cmd.Parameters.AddWithValue("@WorkCode", workCode);
                                    cmd.Parameters.AddWithValue("@AgreementBy", agreementBy);
                                    cmd.Parameters.AddWithValue("@YearOfAgreement", yearOfAgreement);
                                    cmd.Parameters.AddWithValue("@AgreementNo", agreementNo);
                                    cmd.Parameters.AddWithValue("@ComponentID", componentId);
                                    cmd.Parameters.AddWithValue("@SORItemNo", sorItemNo);
                                    cmd.Parameters.AddWithValue("@SORSubItem", sorSubItem ?? "");
                                    cmd.Parameters.AddWithValue("@FormulaID", formulaId);
                                    cmd.Parameters.AddWithValue("@Inputs", inputsJson);
                                    cmd.Parameters.AddWithValue("@ActualUnit", actualUnit);
                                    cmd.Parameters.AddWithValue("@ResultValue", resultValue);
                                    cmd.Parameters.AddWithValue("@Remark", remarks);
                                    cmd.Parameters.AddWithValue("@UniqueEmbID", uniqueEmbID);
                                    cmd.Parameters.AddWithValue("@Units", actualUnit);
                                    cmd.Parameters.AddWithValue("@EntryGroupId", entryGroupId);
                                    cmd.Parameters.AddWithValue("@userId", userId);
                                    cmd.Parameters.AddWithValue("@Revision", oldRevision + 1);

                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    // New entry mode: uniqueness check
                                    cmd.CommandText = "SELECT COUNT(*) FROM eMB_Entry WHERE UniqueEmbID=@UniqueEmbID";
                                    cmd.Parameters.AddWithValue("@UniqueEmbID", uniqueEmbID);
                                    if ((int)cmd.ExecuteScalar() > 0)
                                    {
                                        tx.Rollback();
                                        ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                            "alert('An entry with this ID already exists.');", true);
                                        return;
                                    }

                                    // Insert new entry
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = @"
                                INSERT INTO eMB_Entry
                                    (WorkCode,AgreementBy,YearOfAgreement,AgreementNo,
                                    ComponentID,SORItemNo,SORSubItem,
                                    FormulaID,Inputs,ActualUnit,ResultValue,
                                    Remark,UniqueEmbID,Units,[Date],Deleted,userId)
                                VALUES
                                    (@WorkCode,@AgreementBy,@YearOfAgreement,@AgreementNo,
                                    @ComponentID,@SORItemNo,@SORSubItem,
                                    @FormulaID,@Inputs,@ActualUnit,@ResultValue,
                                    @Remark,@UniqueEmbID,@Units,GETDATE(),0,@userId);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                                    cmd.Parameters.AddWithValue("@WorkCode", workCode);
                                    cmd.Parameters.AddWithValue("@AgreementBy", agreementBy);
                                    cmd.Parameters.AddWithValue("@YearOfAgreement", yearOfAgreement);
                                    cmd.Parameters.AddWithValue("@AgreementNo", agreementNo);
                                    cmd.Parameters.AddWithValue("@ComponentID", componentId);
                                    cmd.Parameters.AddWithValue("@SORItemNo", sorItemNo);
                                    cmd.Parameters.AddWithValue("@SORSubItem", sorSubItem ?? "");
                                    cmd.Parameters.AddWithValue("@FormulaID", formulaId);
                                    cmd.Parameters.AddWithValue("@Inputs", inputsJson);
                                    cmd.Parameters.AddWithValue("@ActualUnit", actualUnit);
                                    cmd.Parameters.AddWithValue("@ResultValue", resultValue);
                                    cmd.Parameters.AddWithValue("@Remark", remarks);
                                    cmd.Parameters.AddWithValue("@UniqueEmbID", uniqueEmbID);
                                    cmd.Parameters.AddWithValue("@Units", actualUnit);
                                    cmd.Parameters.AddWithValue("@userId", userId);

                                    int newEmbId = (int)cmd.ExecuteScalar();

                                    // Update EntryGroupId
                                    cmd.Parameters.Clear();
                                    cmd.CommandText = @"
                                UPDATE eMB_Entry
                                SET EntryGroupId = @GroupId
                                WHERE EmbId = @GroupId";
                                    cmd.Parameters.AddWithValue("@GroupId", newEmbId);
                                    cmd.ExecuteNonQuery();
                                }

                                tx.Commit();
                                string msg = isEditMode
                                    ? "Entry updated successfully."
                                    : "Entry saved successfully.";
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                    "alert('" + msg + "');", true);
                            }
                        }
                        catch (SqlException sx)
                        {
                            tx.Rollback();
                            var text = sx.Number == 1205
                                ? "The system is busy. Please try again."
                                : "Error saving entry: " + sx.Message.Replace("'", "\\'");
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                "alert('" + text + "');", true);
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                                "alert('Error saving entry: " + ex.Message.Replace("'", "\\'") + "');", true);
                        }
                    }

                    LoadExistingEntries();
                    hdnEditEmbId.Value = "";
                }
            }

            hdnUniqueEmbID.Value = "";
            GenerateUniqueEmbID();
        }



        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlFormula.ClearSelection();
            ddlUnit.Items.Clear();                   // also clears unit values

            tblParams.Controls.Clear();
            txtResult.Text = "";
            txtRemarks.Text = "";

            hdnEditEmbId.Value = "";
            hdnUniqueEmbID.Value = "";
            GenerateUniqueEmbID();
        }

        protected void btnBackToComponentList_Click(object sender, EventArgs e)
        {
            var qs = HttpUtility.ParseQueryString(string.Empty);
            qs["WorkCode"] = Request.QueryString["WorkCode"];
            qs["AgreementBy"] = Request.QueryString["AgreementBy"];
            qs["YearOfAgreement"] = Request.QueryString["YearOfAgreement"];
            qs["AgreementNo"] = Request.QueryString["AgreementNo"];
            qs["ComponentID"] = Request.QueryString["ComponentID"];
            qs["AAAmount"] = Request.QueryString["AAAmount"];


            Response.Redirect("ComponentList.aspx?" + qs.ToString());
        }

        protected void gvEntries_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int embId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditItem")
            {
                // Load the entry for editing
                LoadEntryForEdit(embId);
            }
            else if (e.CommandName == "DeleteItem")
            {
                // Delete the entry
                DeleteEntry(embId);
            }
        }

        private void LoadEntryForEdit(int embId)
        {
            lock (_syncLock)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = transaction;
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "SELECT * FROM eMB_Entry WHERE EmbId = @EmbId AND Deleted = 0 AND IsCurrent = 1";
                                cmd.Parameters.AddWithValue("@EmbId", embId);

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        hdnEditEmbId.Value = embId.ToString();
                                        txtRemarks.Text = reader["Remark"].ToString();

                                        GenerateUniqueEmbID();
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('The entry could not be found or has been deleted.');", true);
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Error loading entry for edit: " + ex.Message.Replace("'", "\\'") + "');", true);
                        }
                    }
                }
            }
        }

        private void DeleteEntry(int embId)
        {
            lock (_syncLock)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.Connection = conn;
                                cmd.Transaction = transaction;
                                cmd.CommandType = CommandType.Text;

                                // First check if the record exists and get its version
                                cmd.CommandText = "SELECT Version FROM eMB_Entry WHERE EmbId = @EmbId AND Deleted = 0";
                                cmd.Parameters.AddWithValue("@EmbId", embId);
                                object currentVersion = cmd.ExecuteScalar();

                                if (currentVersion == null)
                                {
                                    transaction.Rollback();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('The entry has already been deleted.');", true);
                                    return;
                                }

                                cmd.Parameters.Clear();
                                cmd.CommandText = "UPDATE eMB_Entry SET Deleted=1 WHERE EmbId = @EmbId AND Version = @Version";
                                cmd.Parameters.AddWithValue("@EmbId", embId);
                                cmd.Parameters.AddWithValue("@Version", currentVersion);

                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected == 0)
                                {
                                    transaction.Rollback();
                                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('The entry was modified by another user. Please refresh and try again.');", true);
                                    return;
                                }

                                transaction.Commit();
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Entry deleted successfully.');", true);
                                LoadExistingEntries();
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            transaction.Rollback();
                            if (sqlEx.Number == 1205) // Deadlock victim
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('The system is busy. Please try again.');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Error deleting entry: " + sqlEx.Message.Replace("'", "\\'") + "');", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Error deleting entry: " + ex.Message.Replace("'", "\\'") + "');", true);
                        }
                    }
                }
            }
        }
    }
}