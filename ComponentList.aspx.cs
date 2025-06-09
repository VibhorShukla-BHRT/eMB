using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace PHEDChhattisgarh
{
    public partial class ComponentList : System.Web.UI.Page
    {
        // Get connection string from web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["eMB"].ConnectionString;
        private string selectedComponentID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!IsPostBack)
            {
                LoadWorkDetails();
                //LoadAgreementGrid();
                // Check if work details are passed in the query string
                if (Request.QueryString["WorkCode"] != null &&
                    Request.QueryString["AgreementBy"] != null &&
                    Request.QueryString["YearOfAgreement"] != null &&
                    Request.QueryString["AgreementNo"] != null &&
                    Request.QueryString["ComponentId"] != null &&
                    Request.QueryString["AAAmount"] != null)
                {
                    // Load work details and components
                    string compId = Request.QueryString["ComponentId"];
                    LoadSubComponents(compId);
                }
                else
                {
                    // Redirect to agreement selection page if required parameters are missing
                    Response.Redirect("AgreementList.aspx");
                }
            }
        }

        private void LoadWorkDetails()
        {
            string workCode = Request.QueryString["WorkCode"];
            string agreementBy = Request.QueryString["AgreementBy"];
            string yearOfAgreement = Request.QueryString["YearOfAgreement"];
            string agreementNo = Request.QueryString["AgreementNo"];
            string componentID = Request.QueryString["ComponentId"];
            string aa_amount = Request.QueryString["AAAmount"];

            // Get the details for the grid
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                        p.Work_Code, 
                        w.name_of_work_Eng AS WorkName,
                        cm.ComponentName
                    FROM eMB_ProgressOfScheme p
                    LEFT JOIN [JJM].[dbo].[Work_Master] w ON p.Work_Code = w.PKWorkCode
                    LEFT JOIN eMB_ComponentMaster cm ON cm.ComponentID = @ComponentID
                    WHERE p.Work_Code = @WorkCode
                    AND p.AgreementBy = @AgreementBy
                    AND p.Year_Of_Agreement = @YearOfAgreement
                    AND p.Agreement_No = @AgreementNo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", workCode);
                    cmd.Parameters.AddWithValue("@AgreementBy", agreementBy);
                    cmd.Parameters.AddWithValue("@YearOfAgreement", yearOfAgreement);
                    cmd.Parameters.AddWithValue("@AgreementNo", agreementNo);
                    cmd.Parameters.AddWithValue("@ComponentID", componentID);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // If we got data, bind it to the grid and set the labels
                    if (dt.Rows.Count > 0)
                    {
                        // Add the component amount from URL parameter to the DataTable
                        dt.Columns.Add("ComponentAmount", typeof(decimal));
                        dt.Rows[0]["ComponentAmount"] = decimal.Parse(aa_amount);

                        gvWorkDetails.DataSource = dt;
                        gvWorkDetails.DataBind();

                    }
                }
            }
        }

        protected void gvComponents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Highlight required components
                bool isRequired = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsRequired"));
                if (isRequired)
                {
                    e.Row.CssClass = "required";
                }
            }
        }

        private void LoadSubComponents(string componentID)
        {
            // Get parameters from the query string
            string workCode = Request.QueryString["WorkCode"];
            string agreementBy = Request.QueryString["AgreementBy"];
            string yearOfAgreement = Request.QueryString["YearOfAgreement"];
            string agreementNo = Request.QueryString["AgreementNo"];
            string componentIdFromQS = componentID;

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT 
                        entry.SORItem,
                        entry.SORFrom,
                        entry.BasicorAmendment,
                        entry.SORItemNo,
                        entry.SORSubItem,
                        entry.Qty,
                        entry.ActualUnit,
                        entry.AmountWithGST
                    FROM [JJM].[dbo].[eMB_ComponentMaterialsEntry] entry
                     WHERE 
                        entry.Work_Code = @WorkCode
                        AND entry.Year_of_Agreement = @YearOfAgreement
                        AND entry.Agreement_No = @AgreementNo
                        AND entry.ComponentID = @ComponentID
                        AND entry.AgreementBy = @AgreementBy
                        AND entry.Qty > 0";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", workCode);
                    cmd.Parameters.AddWithValue("@YearOfAgreement", yearOfAgreement);
                    cmd.Parameters.AddWithValue("@AgreementNo", agreementNo);
                    cmd.Parameters.AddWithValue("@ComponentID", componentIdFromQS);
                    cmd.Parameters.AddWithValue("@AgreementBy", agreementBy);

                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);

                        // Bind to GridView
                        gvSubComponents.DataSource = dt;
                        gvSubComponents.DataBind();
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Error loading SOR Sub-Item grid: " + ex.Message;
                        lblError.Visible = true;
                    }
                }
            }
        }

        protected void gvSubComponents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EntereMB")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                var keys = gvSubComponents.DataKeys[rowIndex];

                string sorItemNo = keys["SORItemNo"].ToString();
                string sorSubItem = keys["SORSubItem"].ToString();
                string units = keys["ActualUnit"].ToString();
                string aa = Request.QueryString["AAAmount"];

                string qs = "WorkCode=" + Request.QueryString["WorkCode"] +
                            "&AgreementBy=" + Request.QueryString["AgreementBy"] +
                            "&YearOfAgreement=" + Request.QueryString["YearOfAgreement"] +
                            "&AgreementNo=" + Request.QueryString["AgreementNo"] +
                            "&ComponentID=" + Request.QueryString["ComponentID"] +
                            "&SORItemNo=" + sorItemNo +
                            "&AAAmount=" + aa;
                Response.Redirect("eMBEntry.aspx?" + qs);
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            // Redirect to Components.aspx with the correct parameters
            string workCode = Request.QueryString["WorkCode"];
            string agreementBy = Request.QueryString["AgreementBy"];
            string yearOfAgreement = Request.QueryString["YearOfAgreement"];
            string agreementNo = Request.QueryString["AgreementNo"];

            string url = "Components.aspx?Year=" + Server.UrlEncode(yearOfAgreement) +
                         "&By=" + Server.UrlEncode(agreementBy) +
                         "&No=" + Server.UrlEncode(agreementNo) +
                         "&WorkCode=" + Server.UrlEncode(workCode);

            Response.Redirect(url);
        }
    }
}