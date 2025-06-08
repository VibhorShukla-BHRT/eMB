using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Services;
using System.Collections.Generic;
using System.Linq;

namespace PHEDChhattisgarh
{
    public partial class AgreementList : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["eMB"].ConnectionString;

        public string CurrentPage { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Don't load dropdown on page load - use search instead
            }
        }

        // AJAX method for autocomplete search
        [WebMethod]
        public static List<string> SearchEMBBookNo(string searchTerm)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["eMB"].ConnectionString;
            var results = new List<string>();

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT DISTINCT TOP 20 eMBBookNo 
                FROM eMBBookNoIssued 
                WHERE eMBBookNo LIKE @SearchTerm + '%'
                ORDER BY eMBBookNo", conn))
            {
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(reader["eMBBookNo"].ToString());
                    }
                }
            }
            return results;
        }


        // Method to validate if EMB Book No exists
        private bool ValidateEMBBookNo(string embBookNo)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM eMBBookNoIssued WHERE eMBBookNo = @BookNo", conn))
            {
                cmd.Parameters.AddWithValue("@BookNo", embBookNo);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        protected void ddlEMBBookNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadWorkCodeDropDown(ddlEMBBookNo.SelectedValue);
        }

        // Optimized work code loading with TOP clause
        private void LoadWorkCodeDropDown(string embBookNo)
        {
            ddlWorkCode.Items.Clear();
            if (string.IsNullOrEmpty(embBookNo))
            {
                ddlWorkCode.Items.Add(new ListItem("-- Select Book No First --", ""));
                ddlWorkCode.Enabled = false;
                return;
            }

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT DISTINCT TOP 100 WorkCode 
                FROM eMBBookNoIssued 
                WHERE eMBBookNo = @BookNo 
                ORDER BY WorkCode", conn))
            {
                cmd.Parameters.AddWithValue("@BookNo", embBookNo);
                var dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                ddlWorkCode.DataSource = dt;
                ddlWorkCode.DataTextField = "WorkCode";
                ddlWorkCode.DataValueField = "WorkCode";
                ddlWorkCode.DataBind();

                ddlWorkCode.Items.Insert(0, new ListItem("-- Select Work Code --", ""));
                ddlWorkCode.Enabled = dt.Rows.Count > 0;
            }
        }

        // Method for direct search without dropdown
        protected void btnDirectSearch_Click(object sender, EventArgs e)
        {
            var bookNo = txtEMBBookNo.Text.Trim();
            var workCode = txtWorkCode.Text.Trim();

            if (string.IsNullOrEmpty(bookNo))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Validation",
                    "alert('Please enter Book No.');", true);
                return;
            }

            // Validate if Book No exists
            if (!ValidateEMBBookNo(bookNo))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Validation",
                    "alert('Invalid Book No. Please check and try again.');", true);
                return;
            }

            // If work code is provided, validate it
            if (!string.IsNullOrEmpty(workCode))
            {
                if (!ValidateWorkCode(bookNo, workCode))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Validation",
                        "alert('Invalid Work Code for selected Book No.');", true);
                    return;
                }

                // Search with specific work code
                SearchWithWorkCode(workCode);
            }
            else
            {
                // Load all work codes for this book
                LoadWorkCodesForBook(bookNo);
            }
        }

        private void SearchWithWorkCode(string workCode)
        {
            // **CERTIFICATION CHECK**
            if (!CheckIfWorkCodeIsCertified(workCode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "NotCertified",
                    "alert('Work Code is not certified.');", true);
                return;
            }

            // Fetch the original agreement details for this WorkCode
            var dt = new DataTable();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
        SELECT 
            p.Year_Of_Agreement, 
            p.AgreementBy, 
            r.distname AS AgreementByName, 
            p.Agreement_No, 
            p.Work_Code
        FROM eMB_ProgressOfScheme p
        LEFT JOIN Retrofiting_dist r 
            ON p.AgreementBy = r.divid
        WHERE p.Work_Code = @WorkCode", conn))
            {
                cmd.Parameters.AddWithValue("@WorkCode", workCode);
                new SqlDataAdapter(cmd).Fill(dt);
            }

            gvAgreements.DataSource = dt;
            gvAgreements.DataBind();

            if (dt.Rows.Count > 0)
            {
                ViewState["Year"] = dt.Rows[0]["Year_Of_Agreement"].ToString();
                ViewState["By"] = dt.Rows[0]["AgreementBy"].ToString();
                ViewState["No"] = dt.Rows[0]["Agreement_No"].ToString();
                ViewState["Work"] = workCode;
                btnViewComponents.Visible = true;
            }
            else
            {
                btnViewComponents.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "NoData",
                    "alert('No agreement found for this Work Code.');", true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var bookNo = ddlEMBBookNo.SelectedValue;
            var workCode = ddlWorkCode.SelectedValue;

            if (string.IsNullOrEmpty(bookNo) || string.IsNullOrEmpty(workCode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Validation",
                    "alert('Please select both Book No and Work Code.');", true);
                return;
            }

            SearchWithWorkCode(workCode);
        }

        private bool ValidateWorkCode(string bookNo, string workCode)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT COUNT(1) FROM eMBBookNoIssued 
                WHERE eMBBookNo = @BookNo AND WorkCode = @WorkCode", conn))
            {
                cmd.Parameters.AddWithValue("@BookNo", bookNo);
                cmd.Parameters.AddWithValue("@WorkCode", workCode);
                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private void LoadWorkCodesForBook(string bookNo)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT DISTINCT WorkCode 
                FROM eMBBookNoIssued 
                WHERE eMBBookNo = @BookNo 
                ORDER BY WorkCode", conn))
            {
                cmd.Parameters.AddWithValue("@BookNo", bookNo);
                var dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);

                // Bind to a GridView showing all work codes for selection
                gvWorkCodes.DataSource = dt;
                gvWorkCodes.DataBind();
                pnlWorkCodes.Visible = true;
            }
        }

        protected void btnViewComponents_Click(object sender, EventArgs e)
        {
            var year = (string)ViewState["Year"];
            var by = (string)ViewState["By"];
            var no = (string)ViewState["No"];
            var work = (string)ViewState["Work"];

            Response.Redirect("Components.aspx?" +
              "Year="+Server.UrlEncode(year)+
              "&By="+Server.UrlEncode(by)+
              "&No="+Server.UrlEncode(no) +
              "&WorkCode="+Server.UrlEncode(work));
        }

        // Helper method to check certification status
        private bool CheckIfWorkCodeIsCertified(string workCode)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM eMB_FinalSubmitofSubEstimate WHERE Work_Code = @WorkCode";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", workCode);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        protected void gvAgreements_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewComponents")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                var keys = gvAgreements.DataKeys[rowIndex];

                string workCode = keys["Work_Code"].ToString();
                string byId = keys["AgreementBy"].ToString();
                string year = keys["Year_Of_Agreement"].ToString();
                string no = keys["Agreement_No"].ToString();

                Response.Redirect("Components.aspx?" +
                    "YearOfAgreement="+Server.UrlEncode(year)+
                    "&AgreementBy="+Server.UrlEncode(byId) +
                    "&AgreementNo="+Server.UrlEncode(no)+
                    "&WorkCode="+Server.UrlEncode(workCode));
            }
        }

        protected void gvWorkCodes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                string workCode = e.CommandArgument.ToString();
                SearchWithWorkCode(workCode);
                pnlWorkCodes.Visible = false;
            }
        }
    }
}