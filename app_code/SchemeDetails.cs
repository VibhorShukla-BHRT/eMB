using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PHEDChhattisgarh
{
public class SchemeDetails
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eMB"].ConnectionString);
    SqlFunction db = new SqlFunction();
    public SchemeDetails()
    {
    }
    public class SchemesDetail
    {
        public string WorkCode { get; set; }
        public string FinYear { get; set; }
        public string DistrictID { get; set; }
        public string DistrictName { get; set; }
        //public string BlockID { get; set; }
        //public string PanchayatID { get; set; }
        //public string VillageID { get; set; }
        //public string HabitationID { get; set; }
        //public string VidhanSabhaID { get; set; }
        public string SchemeType { get; set; }
        public string SchemeName { get; set; }
        public string AABy { get; set; }
        public string AAAmount { get; set; }
        public string AAReference { get; set; }
        public string DateOfAA { get; set; }
        public string TSBy { get; set; }
        public string TSAmount { get; set; }
        public string TSReference { get; set; }
        public string DateOfTS { get; set; }
        public List<SchemesBlock> _SchemesBlock { get; set; }
        public List<SchemesPanchayat> _SchemesPanchayat { get; set; }
        public List<SchemesVillage> _SchemesVillage { get; set; }
        public List<SchemesHabitation> _SchemesHabitation { get; set; }
    }
    public class SchemesBlock
    {
        public string BlockId { get; set; }
        public string BlockName { get; set; }
    }
    public class SchemesPanchayat
    {
        public string PanchayatId { get; set; }
        public string PanchayatName { get; set; }
    }
    public class SchemesVillage
    {
        public string VillageId { get; set; }
        public string VillageName { get; set; }
    }
    public class SchemesHabitation
    {
        public string HabitationId { get; set; }
        public string HabitationName { get; set; }
    }
    public SchemesDetail getSchemesDetail(string WorkCode)
    {
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@WorkCode", Value =WorkCode.Trim() },  
        };
        string sql = @"SELECT RetrofittingOf
                      ,WorkCode
                      ,FinYear
	                  ,DistrictId
                      ,(select distinct DistrictName from HabitationDirectory as b where b.DistrictId=a.DistrictID) as DistrictName
                      ,BlockID
                      ,PanchayatID
                      ,VillageID
                      ,HabitationID
                      ,VidhanSabhaID
                      ,Schcme_Type
                      ,Schcme_Name
                      ,AA_By
                      ,AA_Amount
                      ,AA_Refrence
                      ,DateOf_AA
                      ,TS_By
                      ,TS_Amount
                      ,TS_Reference
                      ,Date_Of_TS 
                  FROM Retrofiting_EntryForm  as a where WorkCode=@WorkCode 
                  union 
                  SELECT RetrofittingOf
                      ,WorkCode
                      ,FinYear
                      ,DistrictID
                      ,(select distinct DistrictName from HabitationDirectory as b where b.DistrictId=a.DistrictID) as DistrictName
                      ,BlockID
                      ,PanchayatID
                      ,VillageID
                      ,HabitationID
                      ,VidhanSabhaID
                      ,Schcme_Type
                      ,Schcme_Name
                      ,AA_By
                      ,AA_Amount
                      ,AA_Refrence
                      ,DateOf_AA
                      ,TS_By
                      ,TS_Amount
                      ,TS_Reference
                      ,Date_Of_TS 
                  FROM Retrofiting_EntryForm_Eworks as a where WorkCode=@WorkCode";
        DataTable dt = db.select_data(sql, para);
        SchemesDetail _SchemesDetail = new SchemesDetail();
        if (dt.Rows.Count > 0)
        {
            _SchemesDetail.WorkCode = dt.Rows[0]["WorkCode"].ToString().Trim();
            _SchemesDetail.FinYear = dt.Rows[0]["FinYear"].ToString().Trim();
            _SchemesDetail.DistrictID = dt.Rows[0]["DistrictID"].ToString().Trim();
            _SchemesDetail.DistrictName = dt.Rows[0]["DistrictName"].ToString().Trim();
            //_SchemesDetail.BlockID = dt.Rows[0]["BlockID"].ToString().Trim();
            //_SchemesDetail.PanchayatID = dt.Rows[0]["PanchayatID"].ToString().Trim();
            //_SchemesDetail.VillageID = dt.Rows[0]["VillageID"].ToString().Trim();
            //_SchemesDetail.HabitationID = dt.Rows[0]["HabitationID"].ToString().Trim();
            //_SchemesDetail.VidhanSabhaID = dt.Rows[0]["VidhanSabhaID"].ToString().Trim();
            _SchemesDetail.SchemeType = dt.Rows[0]["Schcme_Type"].ToString().Trim();
            _SchemesDetail.SchemeName = dt.Rows[0]["Schcme_Name"].ToString().Trim();
            _SchemesDetail.AABy = dt.Rows[0]["AA_By"].ToString().Trim();
            _SchemesDetail.AAAmount = dt.Rows[0]["AA_Amount"].ToString().Trim();
            _SchemesDetail.AAReference = dt.Rows[0]["AA_Refrence"].ToString().Trim();
            _SchemesDetail.DateOfAA = dt.Rows[0]["DateOf_AA"].ToString().Trim();
            _SchemesDetail.TSBy = dt.Rows[0]["TS_By"].ToString().Trim();
            _SchemesDetail.TSAmount = dt.Rows[0]["TS_Amount"].ToString().Trim();
            _SchemesDetail.TSReference = dt.Rows[0]["TS_Reference"].ToString().Trim();
            _SchemesDetail.DateOfTS = dt.Rows[0]["Date_Of_TS"].ToString().Trim();
            _SchemesDetail._SchemesBlock = (dt.Rows[0]["BlockID"].ToString().Trim() == "") ? getBlocks(dt.Rows[0]["AA_Refrence"].ToString().Trim()) : getBlock(dt.Rows[0]["BlockID"].ToString().Trim());
            _SchemesDetail._SchemesPanchayat = (dt.Rows[0]["PanchayatID"].ToString().Trim() == "") ? getPanchayats(dt.Rows[0]["AA_Refrence"].ToString().Trim()) : getPanchayat(dt.Rows[0]["AA_Refrence"].ToString().Trim());
            _SchemesDetail._SchemesVillage = (dt.Rows[0]["VillageID"].ToString().Trim() == "") ? getVillages(dt.Rows[0]["AA_Refrence"].ToString().Trim()) : getVillage(dt.Rows[0]["AA_Refrence"].ToString().Trim());
            _SchemesDetail._SchemesHabitation = (dt.Rows[0]["VillageID"].ToString().Trim() == "") ? getHabitations(dt.Rows[0]["AA_Refrence"].ToString().Trim()) : getHabitation(dt.Rows[0]["AA_Refrence"].ToString().Trim());
        }
        return _SchemesDetail;
    }
    public List<SchemesBlock> getBlocks(string AAReference)
    {
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@AAReference", Value =AAReference.Trim() }, 
        };
        string sql = @"select distinct b.BlockId,b.BlockName from Retrofiting_EntryForm_Block as a inner join HabitationDirectory as b 
        on a.BlockId=b.BlockId  where AA_Refrence=@AAReference order by b.BlockName";
        DataTable dt = db.select_data("SELECT  ID ,AA_Refrence ,BlockId FROM  Retrofiting_EntryForm_Block ", para);
        List<SchemesBlock> _blocks = new List<SchemesBlock>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SchemesBlock _block = new SchemesBlock
            {
                BlockId = dt.Rows[i]["BlockId"].ToString().Trim(),
                BlockName = dt.Rows[i]["BlockName"].ToString().Trim(),
            };
            _blocks.Add(_block);
        }
        return _blocks;
    }
    public List<SchemesPanchayat> getPanchayats(string AAReference)
    {
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@AAReference", Value =AAReference.Trim() }, 
        };
        string sql = @"select distinct b.PanchayatId,b.PanchayatName from Retrofiting_EntryForm_Panchayat as a inner join HabitationDirectory as b 
        on a.PanchayatId=b.PanchayatId  where AA_Refrence=@AAReference order by b.PanchayatName";
        DataTable dt = db.select_data(sql, para);
        List<SchemesPanchayat> _Panchayats = new List<SchemesPanchayat>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SchemesPanchayat _Panchayat = new SchemesPanchayat
            {
                PanchayatId = dt.Rows[i]["PanchayatId"].ToString().Trim(),
                PanchayatName = dt.Rows[i]["PanchayatName"].ToString().Trim(),
            };
            _Panchayats.Add(_Panchayat);
        }
        return _Panchayats;
    }
    public List<SchemesVillage> getVillages(string AAReference)
    {
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@AAReference", Value =AAReference.Trim() }, 
        };
        string sql = @"select distinct b.VillageId,b.VillageName from Retrofiting_EntryForm_Village as a inner join HabitationDirectory as b 
        on a.VillageId=b.VillageId  where AA_Refrence=@AAReference order by b.VillageName";
        DataTable dt = db.select_data(sql, para);
        List<SchemesVillage> _Villages = new List<SchemesVillage>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SchemesVillage _Village = new SchemesVillage
            {
                VillageId = dt.Rows[i]["VillageId"].ToString().Trim(),
                VillageName = dt.Rows[i]["VillageName"].ToString().Trim(),
            };
            _Villages.Add(_Village);
        }
        return _Villages;
    }
    public List<SchemesHabitation> getHabitations(string AAReference)
    {
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@AAReference", Value =AAReference.Trim() }, 
        };
        string sql = @"select distinct b.HabitationId,b.HabitationName from Retrofiting_EntryForm_Child as a inner join HabitationDirectory as b 
        on a.HabitationId=b.HabitationId  where AA_Refrence=@AAReference order by b.HabitationName";
        DataTable dt = db.select_data(sql, para);
        List<SchemesHabitation> _Habitations = new List<SchemesHabitation>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SchemesHabitation _Habitation = new SchemesHabitation
            {
                HabitationId = dt.Rows[i]["HabitationId"].ToString().Trim(),
                HabitationName = dt.Rows[i]["HabitationName"].ToString().Trim(),
            };
            _Habitations.Add(_Habitation);
        }
        return _Habitations;
    }
    public List<SchemesBlock> getBlock(string BlockId)
    {
        List<SchemesBlock> _blocks = new List<SchemesBlock>();
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@BlockId", Value =BlockId.Trim() }, 
        };
        DataTable dt = db.select_data("select distinct BlockId,BlockName from HabitationDirectory where BlockId=@BlockId", para);
        if (dt.Rows.Count > 0)
        {
            SchemesBlock _block = new SchemesBlock
            {
                BlockId = dt.Rows[0]["BlockId"].ToString().Trim(),
                BlockName = dt.Rows[0]["BlockName"].ToString().Trim()
            };
            _blocks.Add(_block);
        }
        return _blocks;
    }
    public List<SchemesPanchayat> getPanchayat(string PanchayatId)
    {
        List<SchemesPanchayat> _Panchayats = new List<SchemesPanchayat>();
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@PanchayatId", Value =PanchayatId.Trim() }, 
        };
        DataTable dt = db.select_data("select distinct Panchayatid,PanchayatName from HabitationDirectory where PanchayatId=@PanchayatId", para);
        if (dt.Rows.Count > 0)
        {
            SchemesPanchayat _Panchayat = new SchemesPanchayat
            {
                PanchayatId = dt.Rows[0]["Panchayatid"].ToString().Trim(),
                PanchayatName = dt.Rows[0]["PanchayatName"].ToString().Trim()
            };
        }
        return _Panchayats;
    }
    public List<SchemesVillage> getVillage(string VillageId)
    {
        List<SchemesVillage> _Villages = new List<SchemesVillage>();
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@VillageId", Value =VillageId.Trim() }, 
        };
        DataTable dt = db.select_data("select distinct VillageId,VillageName from HabitationDirectory where VillageId=@VillageId", para); if (dt.Rows.Count > 0)
        {
            SchemesVillage _Village = new SchemesVillage
            {
                VillageId = dt.Rows[0]["VillageId"].ToString().Trim(),
                VillageName = dt.Rows[0]["VillageName"].ToString().Trim()
            };
            _Villages.Add(_Village);
        }
        return _Villages;
    }
    public List<SchemesHabitation> getHabitation(string HabitationId)
    {
        List<SchemesHabitation> _Habitations = new List<SchemesHabitation>();  
        SqlParameter[] para = { 
            new SqlParameter() { ParameterName = "@HabitationId", Value =HabitationId.Trim() }, 
        };
        DataTable dt = db.select_data("select distinct Habitationid,HabitationName from HabitationDirectory where Habitationid=@HabitationId", para); 
        if(dt.Rows.Count > 0)
        {
            SchemesHabitation _Habitation = new SchemesHabitation
            {
                HabitationId = dt.Rows[0]["Habitationid"].ToString().Trim(),
                HabitationName = dt.Rows[0]["HabitationName"].ToString().Trim()
            };
        }
        return _Habitations;
    }
    public DataTable getSchemeData(string WorkCode)
    {
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@WorkCode", Value =WorkCode.Trim() }, 
            };
        string sql = @"select * from (
             select   distinct FinYear,WorkCode,Schcme_Name as SchemeName,Schcme_Type,a.AA_Refrence as AAReference,AA_Amount as AmountOfAA,convert(char,DateOf_AA,103) as DateOf_AA,AA_By as AAIssuedBy
                        ,a.TS_Reference as TSReference,ts_Amount as AmountOfTS,convert(char,Date_Of_TS,103) as DateOfTS,TS_By as TSIssuedBy,IsMVS,DistrictID,a.BlockID 
                        from Retrofiting_EntryForm_Eworks  as a where WorkCode=@WorkCode
			            union 
             select   distinct FinYear,WorkCode,Schcme_Name as SchemeName,Schcme_Type,a.AA_Refrence as AAReference,AA_Amount as AmountOfAA,convert(char,DateOf_AA,103) as DateOf_AA,AA_By as AAIssuedBy
            ,a.TS_Reference as TSReference,ts_Amount as AmountOfTS,convert(char,Date_Of_TS,103) as DateOfTS,TS_By as TSIssuedBy,IsMVS,DistrictID,a.BlockID 
            from Retrofiting_EntryForm as a where WorkCode=@WorkCode) as t order by finyear,workcode asc";
        DataTable dt = db.select_data(sql,para);
        return dt;
    }
}

}