using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YourNamespace
{
    public partial class Report01 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDivisionData();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDivisionData();
        }
        protected string GetPercentage(object initiated, object issued)
        {
            try
            {
                int initiatedCount = Convert.ToInt32(initiated);
                int issuedCount = Convert.ToInt32(issued);

                if (issuedCount == 0)
                    return "0.00%";

                decimal percentage = (decimal)initiatedCount / issuedCount * 100;
                return percentage.ToString("F2") + "%";
            }
            catch
            {
                return "0.00%";
            }
        }
        private void LoadDivisionData()
        {
            try
            {
                lblError.Visible = false;

                string connectionString = ConfigurationManager.ConnectionStrings["eMB"].ConnectionString;

                string query = @"
                    SELECT 
                        dm.division_name AS DivisionName,
                        COUNT(e.DivisionId) AS IssuedCount,
                        (
                            SELECT COUNT(DISTINCT ee.WorkCode)
                            FROM [JJM].[dbo].[eMB_Entry] ee
                            JOIN [JJM].[dbo].[SubEngineerMaster] sem ON sem.SubEngineerId = ee.userId
                            WHERE sem.DivisionId = dm.division_code
                        ) AS EmbInitiated
                    FROM 
                        [JJM].[dbo].[division_master] dm
                    LEFT JOIN 
                        [JJM].[dbo].[eMBBookNoIssued] e ON dm.division_code = e.DivisionId
                    WHERE 
                        dm.division_code LIKE '4%' AND LEN(dm.division_code) = 8
                    GROUP BY 
                        dm.division_code, dm.division_name
                    ORDER BY 
                        dm.division_name";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            gvDivisionReport.DataSource = dataTable;
                            gvDivisionReport.DataBind();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                ShowError("Database Error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                ShowError("An error occurred while loading data: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
            gvDivisionReport.DataSource = null;
            gvDivisionReport.DataBind();
        }

        // Optional: Method to export data to Excel (requires additional references)
        protected void ExportToExcel()
        {
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DivisionReport.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                using (System.IO.StringWriter sw = new System.IO.StringWriter())
                {
                    using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                    {
                        // Create a new GridView for export (to avoid control issues)
                        GridView gvExport = new GridView();
                        gvExport.DataSource = gvDivisionReport.DataSource;
                        gvExport.DataBind();

                        gvExport.RenderControl(hw);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Export failed: " + ex.Message);
            }
        }
    }
}