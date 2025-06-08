using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace PHEDChhattisgarh
{
public class Dashboard
{
    SqlFunction db = new SqlFunction();
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eMB"].ConnectionString);
    public DashboardModel fillDashboard(string UserId)
    {
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value =UserId.ToString().Trim() }, 
            };
        string sql2 = "";
        string where = "";
        if (UserId.ToString().Trim().StartsWith("1"))
        {
            where = " and state_code=@UserId";
        }
        else if (UserId.ToString().Trim().StartsWith("2"))
        {
            where = " and zone_code=@UserId";
        }
        else if (UserId.ToString().Trim().StartsWith("3"))
        {
            where = " and circle_code=@UserId";
        }
        else if (UserId.ToString().Trim().StartsWith("4"))
        {
            where = " and division_code=@UserId";
        }
        if (UserId.ToString().Trim().StartsWith("5"))
        {
            sql2 = @"select b.isActive,isnull(cast(b.IsApproved as char),2) as IsApproved from SubDivisionMaster as a inner join SubEngineerMaster as b on a.subdivision_code=b.SubDivisionId where b.SubDivisionId=@UserId";
        }
        else
        {
            sql2 = @"select b.isActive,isnull(cast(b.IsApproved as char),2) as IsApproved  from DivisionMaster as a inner join SubEngineerMaster as b on a.division_code=b.DivisionId where 1=1 " + where + "";
        }
        DashboardModel dashbaord = new DashboardModel();
        SubEngineer _SubEngineer = new SubEngineer();
        DataTable dt = db.select_data(sql2, para);
        if (dt.Rows.Count > 0)
        {
            _SubEngineer.TotalSB = dt.Rows.Count.ToString();
            _SubEngineer.Pending = dt.Select("IsApproved = 2").Length.ToString();//2 for blank which indicate to pending
            _SubEngineer.Approved = dt.Select("IsApproved = 1").Length.ToString();
            _SubEngineer.Rejected = dt.Select("IsApproved = 0").Length.ToString();
        }
        else
        {
            _SubEngineer.TotalSB = "0";
            _SubEngineer.Pending = "0";
            _SubEngineer.Approved = "0";
            _SubEngineer.Rejected = "0";
        }
        dashbaord._SubEngineer = _SubEngineer;
        string sql3 = "";
        if (UserId.ToString().Trim().StartsWith("5"))
        {
            sql3 = @"select distinct WorkCode from AssignedSchemes as a inner join SubDivisionMaster as b on a.AssignFrom=b.subdivision_code 
            where IsRollBack=0 and b.subdivision_code=@UserId";
        }
        else
        {
            sql3 = @"select distinct WorkCode from AssignedSchemes as a inner join SubDivisionMaster as b on a.AssignFrom=b.subdivision_code 
            where IsRollBack=0 "+where+"";
        }
        DataTable dt3 = db.select_data(sql3, para);
        if (dt3.Rows.Count > 0)
        {
            dashbaord.TotalAssignedScheme = dt3.Rows.Count.ToString();
        }
        else
        {
            dashbaord.TotalAssignedScheme = "0";
        }
        string sql4 = "";
        if (UserId.ToString().Trim().StartsWith("5"))
        {
            sql4 = @"select distinct a.RequestNo,isnull(cast(StatusOfAE as char),2) as StatusOfAE,isnull(cast(StatusOfEE as char),2)  as StatusOfEE,isnull(Status,2) as Status  from eMBBookNoRequestMaster as a inner join
            eMBBookNoRequestChild as b on a.RequestNo=b.RequestNo where a.IsRollBack=0 and (b.RequestFrom=@UserId or b.RequestTo=@UserId)";
        }
        else if (UserId.ToString().Trim().StartsWith("6"))
        {
            sql4 = @"select distinct a.RequestNo,isnull(cast(StatusOfAE as char),2) as StatusOfAE,isnull(cast(StatusOfEE as char),2)  as StatusOfEE,isnull(Status,2) as Status  from eMBBookNoRequestMaster as a inner join
            eMBBookNoRequestChild as b on a.RequestNo=b.RequestNo where a.IsRollBack=0  and b.RequestFrom=@UserId";
        }
        else
        {
            sql4 = @"select distinct a.RequestNo,isnull(cast(StatusOfAE as char),2) as StatusOfAE,isnull(cast(StatusOfEE as char),2)  as StatusOfEE,isnull(Status,2) as Status from eMBBookNoRequestMaster as a inner join
            eMBBookNoRequestChild as b on a.RequestNo=b.RequestNo where a.IsRollBack=0 and (b.RequestFrom in(select division_code from DivisionMaster where 1=1 " + where + @")
            or b.RequestTo in(select division_code from DivisionMaster where  1=1 " + where + "))";
        }
        eMBRequestSB _eMBRequestSB = new eMBRequestSB();
        eMBRequestAE _eMBRequestAE = new eMBRequestAE();
        eMBRequestEE _eMBRequestEE = new eMBRequestEE();

        _eMBRequestSB.SBTotal = "0";
        _eMBRequestSB.SBPending = "0";
        _eMBRequestSB.SBApproved = "0";
        _eMBRequestSB.SBRejected = "0";

        _eMBRequestEE.EEPending = "0";
        _eMBRequestEE.EEApproved = "0";
        _eMBRequestEE.EERejected = "0";

        _eMBRequestAE.AEPending = "0";
        _eMBRequestAE.AEApproved = "0";
        _eMBRequestAE.AERejected = "0";
        
        DataTable dt4 = db.select_data(sql4, para);
        if (dt4.Rows.Count > 0)
        {
            _eMBRequestSB.SBTotal = dt4.Rows.Count.ToString();
            _eMBRequestSB.SBPending = dt4.Select("StatusOfAE = 2 or (StatusOfEE = 2 and StatusOfAE = 1)").Length.ToString();//2 for blank which indicate to pending
            _eMBRequestSB.SBApproved = dt4.Select("StatusOfAE = 1 and StatusOfEE = 1").Length.ToString();
            _eMBRequestSB.SBRejected = dt4.Select("StatusOfAE = 0 or StatusOfEE = 0").Length.ToString();

            _eMBRequestAE.AEPending = dt4.Select("StatusOfAE = 2").Length.ToString();//2 for blank which indicate to pending
            _eMBRequestAE.AEApproved = dt4.Select("StatusOfAE = 1").Length.ToString();
            _eMBRequestAE.AERejected = dt4.Select("StatusOfAE = 0").Length.ToString();

            _eMBRequestEE.EEPending = dt4.Select("StatusOfEE = 2 and StatusOfAE=1").Length.ToString();//2 for blank which indicate to pending
            _eMBRequestEE.EEApproved = dt4.Select("StatusOfEE = 1").Length.ToString();
            _eMBRequestEE.EERejected = dt4.Select("StatusOfEE = 0").Length.ToString();
        }
        dashbaord._eMBRequestSB = _eMBRequestSB;
        dashbaord._eMBRequestAE = _eMBRequestAE;
        dashbaord._eMBRequestEE = _eMBRequestEE;
        return dashbaord;
    }
    public class DashboardModel
    {
        public string TotalAssignedScheme { get; set; }
        public SubEngineer _SubEngineer { get; set; }
        public eMBRequestSB _eMBRequestSB { get; set; }
        public eMBRequestAE _eMBRequestAE { get; set; }
        public eMBRequestEE _eMBRequestEE { get; set; }
    }
    public class SubEngineer
    {
        public string TotalSB { get; set; }
        public string Pending { get; set; }
        public string Approved { get; set; }
        public string Rejected { get; set; }
    }
    public class eMBRequestSB
    {
        public string SBTotal { get; set; }
        public string SBPending { get; set; }
        public string SBApproved { get; set; }
        public string SBRejected { get; set; }
    }
    public class eMBRequestAE
    {
        public string AEPending { get; set; }
        public string AEApproved { get; set; }
        public string AERejected { get; set; }
    }
    public class eMBRequestEE
    {
        public string EEPending { get; set; }
        public string EEApproved { get; set; }
        public string EERejected { get; set; }
    }
}

}