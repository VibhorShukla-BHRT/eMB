using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace PHEDChhattisgarh
{
    public partial class Components : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["eMB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("../Login.aspx");
            }
            if (!IsPostBack)
            {
                if (!ValidateQueryParams())
                {
                    Response.Redirect("AgreementList.aspx");
                    return;
                }

                LoadWorkDetails();
                LoadComponents();

                // Set current date for progress entry
                lblEntryDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
        }

        private bool ValidateQueryParams()
        {
            return !string.IsNullOrEmpty(Request.QueryString["Year"]) &&
                   !string.IsNullOrEmpty(Request.QueryString["By"]) &&
                   !string.IsNullOrEmpty(Request.QueryString["No"]) &&
                   !string.IsNullOrEmpty(Request.QueryString["WorkCode"]);
        }

        private void LoadWorkDetails()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT p.Work_Code, p.Year_Of_Agreement, p.AgreementBy, 
                        p.Agreement_No, p.AA_Amount, p.AgreementType,
                        w.name_of_work_Eng AS WorkName
                        FROM eMB_ProgressOfScheme p
                        LEFT JOIN [JJM].[dbo].[Work_Master] w ON p.Work_Code = w.PKWorkCode
                        WHERE p.Year_Of_Agreement = @Year
                        AND p.AgreementBy = @By
                        AND p.Agreement_No = @No
                        AND p.Work_Code = @Work";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Year", Request.QueryString["Year"]);
                    cmd.Parameters.AddWithValue("@By", Request.QueryString["By"]);
                    cmd.Parameters.AddWithValue("@No", Request.QueryString["No"]);
                    cmd.Parameters.AddWithValue("@Work", Request.QueryString["WorkCode"]);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvWorkDetails.DataSource = dt;
                    gvWorkDetails.DataBind();
                }
            }
        }

        private void LoadComponents()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT DISTINCT ps.Work_Code, ps.AgreementBy, ps.Agreement_No, ps.AA_Amount, 
                ps.Year_Of_Agreement, cm.ComponentName, psc.ComponentID, 
                psc.qty AS AA_Quantity, psc.Amount, cm.unit AS ComponentUnit,
                COALESCE(latest_progress.LatestQty, 0) AS CompletedQty,
                (psc.qty - COALESCE(latest_progress.LatestQty, 0)) AS RemainingQty
                FROM eMB_ProgressOfScheme ps
                INNER JOIN eMB_ProgressOfScheme_Child psc 
                    ON ps.GroupId = psc.GroupId
                INNER JOIN eMB_ComponentMaster cm 
                    ON psc.ComponentID = cm.ComponentID
                LEFT JOIN (
                    SELECT 
                        WorkCode, AgreementBy, YearOfAgreement, AgreementNo, ComponentID,
                        Qty AS LatestQty,
                        ROW_NUMBER() OVER (PARTITION BY WorkCode, AgreementBy, YearOfAgreement, AgreementNo, ComponentID 
                                         ORDER BY EntryDate DESC) as rn
                    FROM [JJM].[dbo].[componentPhysicalProgress]
                ) latest_progress ON ps.Work_Code = latest_progress.WorkCode 
                    AND ps.AgreementBy = latest_progress.AgreementBy 
                    AND ps.Year_Of_Agreement = latest_progress.YearOfAgreement 
                    AND ps.Agreement_No = latest_progress.AgreementNo 
                    AND psc.ComponentID = latest_progress.ComponentID
                    AND latest_progress.rn = 1
                WHERE ps.Year_Of_Agreement = @Year
                AND ps.AgreementBy = @By
                AND ps.Agreement_No = @No
                AND ps.Work_Code = @Work AND psc.qty > 0 
                ORDER BY psc.ComponentID ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Year", Request.QueryString["Year"]);
                    cmd.Parameters.AddWithValue("@By", Request.QueryString["By"]);
                    cmd.Parameters.AddWithValue("@No", Request.QueryString["No"]);
                    cmd.Parameters.AddWithValue("@Work", Request.QueryString["WorkCode"]);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gvComponents.DataSource = dt;
                    gvComponents.DataBind();
                }
            }
        }

        private void LoadProgressHistory(int componentId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT EntryDate, Percentage, Qty 
                        FROM [JJM].[dbo].[componentPhysicalProgress]
                        WHERE WorkCode = @WorkCode 
                        AND AgreementBy = @AgreementBy 
                        AND YearOfAgreement = @YearOfAgreement 
                        AND AgreementNo = @AgreementNo 
                        AND ComponentID = @ComponentID
                        ORDER BY EntryDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", Request.QueryString["WorkCode"]);
                    cmd.Parameters.AddWithValue("@AgreementBy", Request.QueryString["By"]);
                    cmd.Parameters.AddWithValue("@YearOfAgreement", Request.QueryString["Year"]);
                    cmd.Parameters.AddWithValue("@AgreementNo", Request.QueryString["No"]);
                    cmd.Parameters.AddWithValue("@ComponentID", componentId);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvProgressHistory.DataSource = dt;
                    gvProgressHistory.DataBind();
                }
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgreementList.aspx");
        }

        protected void gvComponents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewComponents")
            {
                string[] args = e.CommandArgument.ToString().Split(',');

                if (args.Length == 6)
                {
                    string workCode = args[0];
                    string agreementBy = args[1];
                    string yearOfAgreement = args[2];
                    string agreementNo = args[3];
                    string componentId = args[4];
                    string aaAmount = args[5];

                    Response.Redirect(string.Format(
                        "ComponentList.aspx?WorkCode={0}&AgreementBy={1}&YearOfAgreement={2}&AgreementNo={3}&ComponentId={4}&AAAmount={5}",
                        workCode, agreementBy, yearOfAgreement, agreementNo, componentId, aaAmount
                    ));
                }
                else
                {
                    throw new ArgumentException("Invalid number of parameters in CommandArgument.");
                }
            }
            else if (e.CommandName == "EnterProgress")
            {
                string[] args = e.CommandArgument.ToString().Split('#');

                if (args.Length == 4)
                {
                    string componentId = args[0];
                    string componentName = args[1];
                    string quantity = args[2];
                    string unit = args[3];

                    // Hide component list and show progress entry panel
                    componentListPanel.Style.Add("display", "none");
                    progressEntryPanel.Style.Add("display", "block");

                    // Populate component information
                    lblComponentId.Text = componentId;
                    lblComponentName.Text = componentName;
                    lblQuantity.Text = quantity + " ";
                    lblUnit.Text = unit;

                    int compId = Convert.ToInt32(componentId);

                    // Check if CSEB survey should be shown for component ID 26
                    if (ShouldShowCSEBSurvey(compId))
                    {
                        csebSurveyPanel.Style.Add("display", "block");
                    }
                    else
                    {
                        csebSurveyPanel.Style.Add("display", "none");
                    }

                    // Check if Source survey should be shown for component ID 2 and 3
                    if (ShouldShowSourceSurvey(compId))
                    {
                        sourceSurveyPanel.Style.Add("display", "block");
                    }
                    else
                    {
                        sourceSurveyPanel.Style.Add("display", "none");
                    }

                    // Load progress history for this component
                    LoadProgressHistory(compId);
                }
            }
        }
        protected void btnSaveSourceSurvey_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string userId = Session["UserId"] != null ? Session["UserId"].ToString() : null;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO [JJM].[dbo].[SourceAvailabilitySurvey] 
                        (WorkCode, AgreementBy, YearOfAgreement, AgreementNo, ComponentID, SourceAvailable, EntryDate, userId)
                        VALUES (@WorkCode, @AgreementBy, @YearOfAgreement, @AgreementNo, @ComponentID, @SourceAvailable, @EntryDate, @userId)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@WorkCode", Request.QueryString["WorkCode"]);
                            cmd.Parameters.AddWithValue("@AgreementBy", Request.QueryString["By"]);
                            cmd.Parameters.AddWithValue("@YearOfAgreement", Request.QueryString["Year"]);
                            cmd.Parameters.AddWithValue("@AgreementNo", Request.QueryString["No"]);
                            cmd.Parameters.AddWithValue("@ComponentID", Convert.ToInt32(lblComponentId.Text));
                            cmd.Parameters.AddWithValue("@SourceAvailable", ddlSourceAvailable.SelectedValue);
                            cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@userId", userId);

                            conn.Open();
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                // Hide the survey panel after successful save
                                sourceSurveyPanel.Style.Add("display", "none");

                                string message = "";
                                if (ddlSourceAvailable.SelectedValue == "Yes")
                                {
                                    message = "यह सुनिश्चित करते है कि इस योजना में सोर्स की उपलब्धता है";
                                }
                                else
                                {
                                    message = "यह सुनिश्चित करते है कि इस योजना में सोर्स की आवश्यकता है";
                                }

                                ClientScript.RegisterStartupScript(this.GetType(), "sourceAlert",
                                    "alert('"+message+"');", true);
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                    "alert('Error saving survey. Please try again.');", true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Error: "+ex.Message+"');", true);
                }
            }
        }
        protected void btnSaveCSEBSurvey_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string userId = Session["UserId"] != null ? Session["UserId"].ToString() : null;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO [JJM].[dbo].[CSEBSurvey] 
                        (WorkCode, AgreementBy, YearOfAgreement, AgreementNo, ComponentID, CSEBStatus, EntryDate,userId)
                        VALUES (@WorkCode, @AgreementBy, @YearOfAgreement, @AgreementNo, @ComponentID, @CSEBStatus, @EntryDate, @userId)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@WorkCode", Request.QueryString["WorkCode"]);
                            cmd.Parameters.AddWithValue("@AgreementBy", Request.QueryString["By"]);
                            cmd.Parameters.AddWithValue("@YearOfAgreement", Request.QueryString["Year"]);
                            cmd.Parameters.AddWithValue("@AgreementNo", Convert.ToInt32(Request.QueryString["No"]));
                            cmd.Parameters.AddWithValue("@ComponentID", Convert.ToInt32(lblComponentId.Text));
                            cmd.Parameters.AddWithValue("@CSEBStatus", ddlCSEBStatus.SelectedValue);
                            cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@userId",userId);

                            conn.Open();
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                // Hide the survey panel after successful save
                                csebSurveyPanel.Style.Add("display", "none");

                                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                    "alert('CSEB Survey saved successfully!');", true);

                                // If status is "Provided", they won't see this survey again
                                if (ddlCSEBStatus.SelectedValue == "Provided")
                                {
                                    ClientScript.RegisterStartupScript(this.GetType(), "alert2",
                                        "alert('CSEB connection marked as provided. Survey complete!');", true);
                                }
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                    "alert('Error saving survey. Please try again.');", true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Error: "+ex.Message+"');", true);
                }
            }
        }

        // Add this validation method to your Components.aspx.cs class

        protected void btnSaveProgress_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    decimal inputValue = Convert.ToDecimal(txtProgressValue.Text);
                    decimal totalQty = Convert.ToDecimal(lblQuantity.Text.Trim());

                    decimal percentage, completedQty;

                    if (ddlEntryType.SelectedValue == "Percentage")
                    {
                        // Validate percentage is not greater than 100
                        if (inputValue > 100)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                "alert('Progress percentage cannot exceed 100%. Please enter a valid percentage.');", true);
                            return;
                        }

                        percentage = inputValue;
                        completedQty = (totalQty * percentage) / 100;
                    }
                    else
                    {
                        // Validate quantity is not greater than total quantity
                        if (inputValue > totalQty)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                "alert('Completed quantity cannot exceed total quantity (" + totalQty + "). Please enter a valid quantity.');", true);
                            return;
                        }

                        completedQty = inputValue;
                        percentage = totalQty > 0 ? (completedQty / totalQty) * 100 : 0;
                    }

                    // Additional check to ensure percentage never exceeds 100%
                    if (percentage > 100)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert",
                            "alert('Calculated progress exceeds 100%. Please check your input values.');", true);
                        return;
                    }
                    string userId = Session["UserId"] != null ? Session["UserId"].ToString() : null;
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = @"INSERT INTO [JJM].[dbo].[componentPhysicalProgress] 
                (WorkCode, AgreementBy, YearOfAgreement, AgreementNo, ComponentID, Percentage, Qty, EntryDate,userId)
                VALUES (@WorkCode, @AgreementBy, @YearOfAgreement, @AgreementNo, @ComponentID, @Percentage, @Qty, @EntryDate,@userId)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@WorkCode", Request.QueryString["WorkCode"]);
                            cmd.Parameters.AddWithValue("@AgreementBy", Request.QueryString["By"]);
                            cmd.Parameters.AddWithValue("@YearOfAgreement", Request.QueryString["Year"]);
                            cmd.Parameters.AddWithValue("@AgreementNo", Convert.ToInt32(Request.QueryString["No"]));
                            cmd.Parameters.AddWithValue("@ComponentID", Convert.ToInt32(lblComponentId.Text));
                            cmd.Parameters.AddWithValue("@Percentage", percentage);
                            cmd.Parameters.AddWithValue("@Qty", completedQty);
                            cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@userId", userId);

                            conn.Open();
                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                txtProgressValue.Text = "";
                                lblCalculatedValue.Text = "";

                                // Reload progress history to show the new entry
                                LoadProgressHistory(Convert.ToInt32(lblComponentId.Text));

                                // Update the remaining quantity in the background GridView
                                UpdateRemainingQuantityForComponent(Convert.ToInt32(lblComponentId.Text));

                                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                    "alert('Progress saved successfully!');", true);
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                                    "alert('Error saving progress. Please try again.');", true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Error: "+ex.Message+"');", true);
                }
            }
        }
        protected void txtProgressValue_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProgressValue.Text) && !string.IsNullOrEmpty(lblQuantity.Text))
            {
                try
                {
                    decimal inputValue = Convert.ToDecimal(txtProgressValue.Text);
                    decimal totalQty = Convert.ToDecimal(lblQuantity.Text.Trim());

                    if (ddlEntryType.SelectedValue == "Percentage")
                    {
                        if (inputValue > 100)
                        {
                            lblCalculatedValue.Text = "❌ Percentage cannot exceed 100%";
                            lblCalculatedValue.CssClass = "form-control-plaintext text-danger font-weight-bold";
                            return;
                        }

                        // Calculate quantity from percentage
                        decimal calculatedQty = (totalQty * inputValue) / 100;
                        lblCalculatedValue.Text = string.Format("Quantity: {0:N2} {1}", calculatedQty, lblUnit.Text);
                        lblCalculatedValue.CssClass = "form-control-plaintext text-primary font-weight-bold";
                    }
                    else
                    {
                        if (inputValue > totalQty)
                        {
                            lblCalculatedValue.Text = string.Format("❌ Quantity cannot exceed {0:N2}", totalQty);
                            lblCalculatedValue.CssClass = "form-control-plaintext text-danger font-weight-bold";
                            return;
                        }

                        // Calculate percentage from quantity
                        if (totalQty > 0)
                        {
                            decimal calculatedPercentage = (inputValue / totalQty) * 100;
                            lblCalculatedValue.Text = string.Format("Percentage: {0:N2}", calculatedPercentage);
                            lblCalculatedValue.CssClass = "form-control-plaintext text-primary font-weight-bold";
                        }
                    }
                }
                catch
                {
                    lblCalculatedValue.Text = "Invalid input";
                    lblCalculatedValue.CssClass = "form-control-plaintext text-danger font-weight-bold";
                }
            }
        }

        protected void ddlEntryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEntryType.SelectedValue == "Percentage")
            {
                lblInputType.InnerHtml = "<strong>Progress Percentage (%):</strong>";
                txtProgressValue.Attributes["placeholder"] = "Enter percentage (0-100)";
                txtProgressValue.Attributes["max"] = "100";
                rfvProgressValue.ErrorMessage = "Percentage is required";

                // Enable range validator, disable custom validator
                rvProgressPercentage.Enabled = true;
                cvProgressValue.Enabled = false;
            }
            else
            {
                lblInputType.InnerHtml = "<strong>Completed Quantity:</strong>";
                txtProgressValue.Attributes["placeholder"] = "Enter completed quantity";
                txtProgressValue.Attributes["max"] = lblQuantity.Text.Trim();
                rfvProgressValue.ErrorMessage = "Quantity is required";

                // Disable range validator, enable custom validator
                rvProgressPercentage.Enabled = false;
                cvProgressValue.Enabled = true;
            }

            // Clear values when switching
            txtProgressValue.Text = "";
            lblCalculatedValue.Text = "";
            lblCalculatedValue.CssClass = "form-control-plaintext text-primary font-weight-bold";
        }


        protected void btnBackToList_Click(object sender, EventArgs e)
        {
            // Get the component ID before clearing the form
            int componentId = Convert.ToInt32(lblComponentId.Text);

            // Update the remaining quantity for this specific component
            UpdateRemainingQuantityForComponent(componentId);

            // Show component list and hide progress entry panel
            componentListPanel.Style.Add("display", "block");
            progressEntryPanel.Style.Add("display", "none");

            csebSurveyPanel.Style.Add("display", "none");
            ddlCSEBStatus.SelectedIndex = 0;

            sourceSurveyPanel.Style.Add("display", "none");
            ddlSourceAvailable.SelectedIndex = 0;

            // Clear form data
            txtProgressValue.Text = "";
            lblCalculatedValue.Text = "";
            ddlEntryType.SelectedIndex = 0; // Reset to Percentage
            lblInputType.InnerHtml = "<strong>Progress Percentage (%):</strong>";
            lblComponentId.Text = "";
            lblComponentName.Text = "";
            lblQuantity.Text = "";
            lblUnit.Text = "";

            // Clear progress history
            gvProgressHistory.DataSource = null;
            gvProgressHistory.DataBind();
        }

        private void UpdateRemainingQuantityForComponent(int componentId)
        {
            try
            {
                // Get the latest completed quantity for this component
                decimal latestCompletedQty = GetLatestCompletedQuantity(componentId);

                // Find the row in the GridView that corresponds to this component
                foreach (GridViewRow row in gvComponents.Rows)
                {
                    // Find the ComponentID in the row (it's in the first column - index 0)
                    string rowComponentId = row.Cells[0].Text;

                    if (rowComponentId == componentId.ToString())
                    {
                        // Get the AA_Quantity from the row (it's in the third column - index 2)
                        decimal aaQuantity = Convert.ToDecimal(row.Cells[2].Text);

                        // Calculate and update the remaining quantity (it's in the fourth column - index 3)
                        decimal remainingQty = aaQuantity - latestCompletedQty;
                        row.Cells[3].Text = remainingQty.ToString("N2");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error or handle gracefully
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Error updating remaining quantity: "+ex.Message+"');", true);
            }
        }

        private decimal GetLatestCompletedQuantity(int componentId)
        {
            decimal latestQty = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT TOP 1 Qty 
                FROM [JJM].[dbo].[componentPhysicalProgress]
                WHERE WorkCode = @WorkCode 
                AND AgreementBy = @AgreementBy 
                AND YearOfAgreement = @YearOfAgreement 
                AND AgreementNo = @AgreementNo 
                AND ComponentID = @ComponentID
                ORDER BY EntryDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", Request.QueryString["WorkCode"]);
                    cmd.Parameters.AddWithValue("@AgreementBy", Request.QueryString["By"]);
                    cmd.Parameters.AddWithValue("@YearOfAgreement", Request.QueryString["Year"]);
                    cmd.Parameters.AddWithValue("@AgreementNo", Request.QueryString["No"]);
                    cmd.Parameters.AddWithValue("@ComponentID", componentId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        latestQty = Convert.ToDecimal(result);
                    }
                }
            }

            return latestQty;
        }
        private bool ShouldShowSourceSurvey(int componentId)
        {
            if (componentId != 2 && componentId != 3) return false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM [JJM].[dbo].[SourceAvailabilitySurvey] 
                        WHERE WorkCode = @WorkCode 
                        AND AgreementBy = @AgreementBy 
                        AND YearOfAgreement = @YearOfAgreement 
                        AND AgreementNo = @AgreementNo 
                        AND ComponentID = @ComponentID 
                        AND SourceAvailable = 'Yes'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", Request.QueryString["WorkCode"]);
                    cmd.Parameters.AddWithValue("@AgreementBy", Request.QueryString["By"]);
                    cmd.Parameters.AddWithValue("@YearOfAgreement", Request.QueryString["Year"]);
                    cmd.Parameters.AddWithValue("@AgreementNo", Request.QueryString["No"]);
                    cmd.Parameters.AddWithValue("@ComponentID", componentId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    int count = result != null ? Convert.ToInt32(result) : 0;
                    return count == 0; // Show survey if no "Yes" status exists
                }
            }
        }
        private bool ShouldShowCSEBSurvey(int componentId)
        {
            if (componentId != 26) return false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT COUNT(*) FROM [JJM].[dbo].[CSEBSurvey]
                        WHERE WorkCode = @WorkCode 
                        AND AgreementBy = @AgreementBy 
                        AND YearOfAgreement = @YearOfAgreement 
                        AND AgreementNo = @AgreementNo 
                        AND ComponentID = @ComponentID 
                        AND CSEBStatus = 'Provided'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", Request.QueryString["WorkCode"]);
                    cmd.Parameters.AddWithValue("@AgreementBy", Request.QueryString["By"]);
                    // Change this line - pass as string instead of converting to int
                    cmd.Parameters.AddWithValue("@YearOfAgreement", Request.QueryString["Year"]);
                    cmd.Parameters.AddWithValue("@AgreementNo", Request.QueryString["No"]);
                    cmd.Parameters.AddWithValue("@ComponentID", componentId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    int count = result != null ? Convert.ToInt32(result) : 0;
                    return count == 0; // Show survey if no "Provided" status exists
                }
            }
        }
        // Add this custom validator method to your Components.yaspx.cs class

        protected void cvProgressValue_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                decimal inputValue = Convert.ToDecimal(args.Value);

                if (ddlEntryType.SelectedValue == "Percentage")
                {
                    // For percentage, check if it's within 0-100 range
                    args.IsValid = (inputValue >= 0 && inputValue <= 100);
                }
                else
                {
                    // For quantity, check if it doesn't exceed total quantity
                    decimal totalQty = Convert.ToDecimal(lblQuantity.Text.Trim());
                    args.IsValid = (inputValue >= 0 && inputValue <= totalQty);
                }
            }
            catch
            {
                args.IsValid = false;
            }
        }
    }
}