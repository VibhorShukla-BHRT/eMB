using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;

namespace PHEDChhattisgarh
{
    public partial class ProgressReport : System.Web.UI.Page
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["eMB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { }
        }

        [WebMethod]
        public static List<string> GetEmbBooks()
        {
            List<string> embBooks = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT eMBBookNo FROM eMBBookNoIssued ORDER BY eMBBookNo";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                embBooks.Add(reader["eMBBookNo"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching eMB books: " + ex.Message);
            }

            return embBooks;
        }

        [WebMethod]
        public static List<string> GetWorkCodes(string bookNo)
        {
            List<string> workCodes = new List<string>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT DISTINCT TOP 100 WorkCode 
                                   FROM eMBBookNoIssued 
                                   WHERE eMBBookNo = @BookNo 
                                   ORDER BY WorkCode";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@BookNo", bookNo);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                workCodes.Add(reader["WorkCode"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching work codes: " + ex.Message);
            }

            return workCodes;
        }

        [WebMethod]
        public static ProgressReportData GetProgressReport(string workCode)
        {
            ProgressReportData reportData = new ProgressReportData();

            try
            {
                // Get Agreement Details
                reportData.AgreementDetails = GetAgreementDetails(workCode);

                // Get Components
                reportData.Components = GetComponents(reportData.AgreementDetails.Year_Of_Agreement,
                    reportData.AgreementDetails.AgreementBy, reportData.AgreementDetails.Agreement_No, workCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching progress report: " + ex.Message);
            }

            return reportData;
        }

        private static AgreementDetails GetAgreementDetails(string workCode)
        {
            AgreementDetails details = new AgreementDetails();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                p.Year_Of_Agreement, 
                                p.AgreementBy, 
                                r.distname AS AgreementByName, 
                                p.Agreement_No, 
                                p.Work_Code
                            FROM eMB_ProgressOfScheme p
                            LEFT JOIN [JJM].[dbo].Retrofiting_dist r 
                                ON p.AgreementBy = r.divid
                            WHERE p.Work_Code = @WorkCode";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@WorkCode", workCode);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            details.Year_Of_Agreement = reader["Year_Of_Agreement"].ToString();
                            details.AgreementBy = reader["AgreementBy"].ToString();
                            details.AgreementByName = reader["AgreementByName"].ToString();
                            details.Agreement_No = reader["Agreement_No"].ToString();
                            details.Work_Code = reader["Work_Code"].ToString();
                        }
                    }
                }
            }

            return details;
        }

        private static List<ComponentData> GetComponents(string year, string agreementBy, string agreementNo, string workCode)
        {
            List<ComponentData> components = new List<ComponentData>();

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
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@By", agreementBy);
                    cmd.Parameters.AddWithValue("@No", agreementNo);
                    cmd.Parameters.AddWithValue("@Work", workCode);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ComponentData component = new ComponentData
                            {
                                Work_Code = reader["Work_Code"].ToString(),
                                AgreementBy = reader["AgreementBy"].ToString(),
                                Agreement_No = reader["Agreement_No"].ToString(),
                                AA_Amount = Convert.ToDecimal(reader["AA_Amount"]),
                                Year_Of_Agreement = reader["Year_Of_Agreement"].ToString(),
                                ComponentName = reader["ComponentName"].ToString(),
                                ComponentID = Convert.ToInt32(reader["ComponentID"]),
                                AA_Quantity = Convert.ToDecimal(reader["AA_Quantity"]),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                ComponentUnit = reader["ComponentUnit"].ToString(),
                                CompletedQty = Convert.ToDecimal(reader["CompletedQty"]),
                                RemainingQty = Convert.ToDecimal(reader["RemainingQty"])
                            };
                            components.Add(component);
                        }
                    }
                }
            }

            return components;
        }

        [WebMethod]
        public static List<SubComponentData> GetSubComponents(string workCode, string yearOfAgreement,
            string agreementNo, int componentID, string agreementBy)
        {
            List<SubComponentData> subComponents = new List<SubComponentData>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                                    entry.SORItem,
                                    entry.SORFrom,
                                    entry.BasicorAmendment,
                                    entry.SORItemNo,
                                    entry.SORSubItem,
                                    entry.Qty,
                                    entry.ActualUnit,
                                    entry.AmountWithGST,
                                    entry.UnitCost
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
                        cmd.Parameters.AddWithValue("@ComponentID", componentID);
                        cmd.Parameters.AddWithValue("@AgreementBy", agreementBy);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SubComponentData subComponent = new SubComponentData
                                {
                                    SORItem = reader["SORItem"].ToString(),
                                    SORFrom = reader["SORFrom"].ToString(),
                                    BasicorAmendment = reader["BasicorAmendment"].ToString(),
                                    SORItemNo = reader["SORItemNo"].ToString(),
                                    SORSubItem = reader["SORSubItem"].ToString(),
                                    Qty = Convert.ToDecimal(reader["Qty"]),
                                    ActualUnit = reader["ActualUnit"].ToString(),
                                    AmountWithGST = Convert.ToDecimal(reader["AmountWithGST"]),
                                    UnitCost = Convert.ToDecimal(reader["UnitCost"])
                                };
                                subComponents.Add(subComponent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching sub-components: " + ex.Message);
            }

            return subComponents;
        }
        [WebMethod]
        public static List<SubComponentWithProgress> GetSubComponentsWithProgress(string workCode, string yearOfAgreement,
    string agreementNo, int componentID, string agreementBy)
        {
            List<SubComponentWithProgress> subComponents = new List<SubComponentWithProgress>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
                            entry.SORItem,
                            entry.SORFrom,
                            entry.BasicorAmendment,
                            entry.SORItemNo,
                            entry.SORSubItem,
                            entry.Qty,
                            entry.ActualUnit,
                            entry.AmountWithGST,
                            entry.UnitCost,
                            ISNULL((SELECT SUM(ResultValue) 
                                   FROM [JJM].[dbo].[eMB_Entry] e
                                   WHERE e.WorkCode = @WorkCode
                                   AND e.ComponentID = @ComponentID
                                   AND e.SORItemNo = entry.SORItemNo
                                   AND e.Deleted = 0
                                   AND e.IsCurrent = 1), 0) AS CompletedQty
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
                        cmd.Parameters.AddWithValue("@ComponentID", componentID);
                        cmd.Parameters.AddWithValue("@AgreementBy", agreementBy);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SubComponentWithProgress subComponent = new SubComponentWithProgress
                                {
                                    SORItem = reader["SORItem"].ToString(),
                                    SORFrom = reader["SORFrom"].ToString(),
                                    BasicorAmendment = reader["BasicorAmendment"].ToString(),
                                    SORItemNo = reader["SORItemNo"].ToString(),
                                    SORSubItem = reader["SORSubItem"].ToString(),
                                    Qty = Convert.ToDecimal(reader["Qty"]),
                                    ActualUnit = reader["ActualUnit"].ToString(),
                                    AmountWithGST = Convert.ToDecimal(reader["AmountWithGST"]),
                                    UnitCost = Convert.ToDecimal(reader["UnitCost"]),
                                    CompletedQty = Convert.ToDecimal(reader["CompletedQty"])
                                };
                                subComponents.Add(subComponent);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching sub-components with progress: " + ex.Message);
            }

            return subComponents;
        }
        [WebMethod]
        public static List<EmbEntryData> GetEmbEntries(string workCode, string agreementBy,
            string yearOfAgreement, string agreementNo, int componentID, string sorItemNo)
        {
            List<EmbEntryData> embEntries = new List<EmbEntryData>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"SELECT 
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

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@WorkCode", workCode);
                        cmd.Parameters.AddWithValue("@AgreementBy", agreementBy);
                        cmd.Parameters.AddWithValue("@YearOfAgreement", yearOfAgreement);
                        cmd.Parameters.AddWithValue("@AgreementNo", agreementNo);
                        cmd.Parameters.AddWithValue("@ComponentID", componentID);
                        cmd.Parameters.AddWithValue("@SORItemNo", sorItemNo);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmbEntryData entry = new EmbEntryData
                                {
                                    EmbId = Convert.ToInt32(reader["EmbId"]),
                                    SORItemNo = reader["SORItemNo"].ToString(),
                                    Remark = reader["Remark"].ToString(),
                                    UniqueEmbID = reader["UniqueEmbID"].ToString(),
                                    Inputs = reader["Inputs"].ToString(),
                                    ActualUnit = reader["ActualUnit"].ToString(),
                                    ResultValue = Convert.ToDecimal(reader["ResultValue"]),
                                    IsCurrent = Convert.ToBoolean(reader["IsCurrent"])
                                };
                                embEntries.Add(entry);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching eMB entries: " + ex.Message);
            }

            return embEntries;
        }
    }

    // Data Models
    public class ProgressReportData
    {
        public AgreementDetails AgreementDetails { get; set; }
        public List<ComponentData> Components { get; set; }
    }

    public class AgreementDetails
    {
        public string Year_Of_Agreement { get; set; }
        public string AgreementBy { get; set; }
        public string AgreementByName { get; set; }
        public string Agreement_No { get; set; }
        public string Work_Code { get; set; }
    }

    public class ComponentData
    {
        public string Work_Code { get; set; }
        public string AgreementBy { get; set; }
        public string Agreement_No { get; set; }
        public decimal AA_Amount { get; set; }
        public string Year_Of_Agreement { get; set; }
        public string ComponentName { get; set; }
        public int ComponentID { get; set; }
        public decimal AA_Quantity { get; set; }
        public decimal Amount { get; set; }
        public string ComponentUnit { get; set; }
        public decimal CompletedQty { get; set; }
        public decimal RemainingQty { get; set; }
    }
    public class SubComponentWithProgress : SubComponentData
    {
        public decimal CompletedQty { get; set; }
    }
    public class SubComponentData
    {
        public string SORItem { get; set; }
        public string SORFrom { get; set; }
        public string BasicorAmendment { get; set; }
        public string SORItemNo { get; set; }
        public string SORSubItem { get; set; }
        public decimal Qty { get; set; }
        public string ActualUnit { get; set; }
        public decimal AmountWithGST { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class EmbEntryData
    {
        public int EmbId { get; set; }
        public string SORItemNo { get; set; }
        public string Remark { get; set; }
        public string UniqueEmbID { get; set; }
        public string Inputs { get; set; }
        public string ActualUnit { get; set; }
        public decimal ResultValue { get; set; }
        public bool IsCurrent { get; set; }
    }
}