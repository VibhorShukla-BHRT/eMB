using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

namespace PHEDChhattisgarh
{
public class Function
{
    SqlFunction db = new SqlFunction();
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["eMB"].ConnectionString);
    public Function()
    {
    }
    public void fillDistrict(string UserId, DropDownList ddl)
    {
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value =UserId.ToString().Trim() }, 
            };
        string sql = "";
        string where = "";
        string select = "All District";
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
            select = "NOSELECT";
        }
        if (UserId.ToString().Trim().StartsWith("5"))
        {
            sql = @"select distinct DistrictId,DistrictName from [JJM].[dbo].[Subdivision_master] as a inner join DistrictMaster as b on a.division_code=b.DivisionId 
            where subdivision_code=@UserId order by DistrictName asc";
            select = "NOSELECT";
        }
        else
        {
            sql = @"select  distinct b.DistrictName,b.DistrictId  from [JJM].[dbo].[division_master] as a inner join DistrictMaster as b
            on a.division_code=b.DivisionId where 1=1 " + where + " order by b.DistrictName asc";
        }
        db.fill_Dropdown_join(sql, "DistrictId", "DistrictName", select, ddl, para);
    }
    public void fillDivision(string UserId, DropDownList ddl)
    {
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value =UserId.ToString().Trim() }, 
            };
        string sql2 = "";
        string where = "";
        string select = "All Division";
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
            select = "NOSELECT";
        }
        if (UserId.ToString().Trim().StartsWith("5"))
        {
            sql2 = @"select a.division_code,a.division_name from [JJM].[dbo].[division_master] as a inner join [JJM].[dbo].[Subdivision_master] as b
                on a.division_code=b.division_code where subdivision_code=@UserId";
            select = "NOSELECT";
        }
        else
        {
            sql2 = @"select a.division_code,a.division_name,circle_code,zone_code from [JJM].[dbo].[division_master] as a inner join DistrictMaster as b
                on a.division_code=b.DivisionId where 1=1 " + where + "";
        }
        db.fill_Dropdown_join(sql2, "division_code", "division_name", select, ddl, para);
    }
    public void fillSubEngineer(string UserId, DropDownList ddl)
    {
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value =UserId.ToString().Trim() }, 
            };
        string sql = @"select distinct SubEngineerId, SubEngineerName from [JJM].[dbo].[Subdivision_master] as a1 inner join SubEngineerMaster as a2
        on a1.subdivision_code=a2.SubDivisionId where a1.subdivision_code=@UserId order by SubEngineerName asc";
        db.fill_Dropdown_join(sql, "SubEngineerId", "SubEngineerName", "All Sub-Engineer", ddl, para);
    }
     

    public bool getFinyear2()
    {
        bool flag = false;
        string sql = @"SELECT  FinYear  FROM  FinancialYear order by FinYear desc";
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value ="".Trim() }, 
            };
        DataTable dt = db.select_data(sql, para);
        if (dt.Rows.Count > 0)
        {
            flag = true;
        }
        return flag;
    }

    public string encryption_key()
    {
        return "Key@PhedTollFree";
    }
    public string GetLocalIPAddress()
    {
        string strIpAddress;
        strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (strIpAddress == null)
        {
            strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }
        return strIpAddress;
    }
    public string GetMACAddress()
    {
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        String sMacAddress = string.Empty;
        foreach (NetworkInterface adapter in nics)
        {
            if (sMacAddress == String.Empty)// only return MAC Address from first card
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                sMacAddress = adapter.GetPhysicalAddress().ToString();
            }
        } return sMacAddress;
    }
    public void LoginActivityLog(string UserId, string SuccessFlag)
    {
        con.Open();
        string sql = @"INSERT INTO LoginActivityLog
           (UserId
           ,EntryDate
           ,IPAddress
           ,SuccessFlag)
             VALUES
           (@UserId 
           ,getdate() 
           ,@IPAddress 
           ,@SuccessFlag )";
        SqlCommand cmd1 = new SqlCommand(sql, con);
        cmd1.Parameters.AddWithValue("@UserId", UserId);
        cmd1.Parameters.AddWithValue("@IPAddress", GetLocalIPAddress());
        cmd1.Parameters.AddWithValue("@SuccessFlag", SuccessFlag); 
        int res = cmd1.ExecuteNonQuery();
        con.Close();
    }
    public string sha256_hash(String value)
    {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
    public string Encrypt(string clearText)
    {
        string EncryptionKey = "CgPhEd@TollFree";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText.Replace(" ", "+");
    }
    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "CgPhEd@TollFree";
        byte[] cipherBytes = Convert.FromBase64String(cipherText.Replace(" ", "+"));
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    public void WriteCookie(string UserId)
    {
        //Create a Cookie with a suitable Key.
        HttpCookie nameCookie = new HttpCookie("UserId");
        //Set the Cookie value.
        nameCookie.Values["UserId"] = UserId.Trim(); 
        //Set the Expiry date.
        nameCookie.Expires = DateTime.Now.AddDays(30);
        //Add the Cookie to Browser.
        HttpContext.Current.Response.Cookies.Add(nameCookie);
    }
    public string ReadCookie()
    {
        //Fetch the Cookie using its Key.
        HttpCookie nameCookie = HttpContext.Current.Request.Cookies["UserId"];
        //If Cookie exists fetch its value.
        return  nameCookie != null ? nameCookie.Value.Split('=')[1] : "N/A";
    }
    public void RemoveCookie()
    {
        //Fetch the Cookie using its Key.
        HttpCookie nameCookie = HttpContext.Current.Request.Cookies["UserId"];
        if (nameCookie != null)
        {
            //Set the Expiry date to past date.
            nameCookie.Expires = DateTime.Now.AddDays(-1);
            //Update the Cookie in Browser.
            HttpContext.Current.Response.Cookies.Add(nameCookie);
        }
    }
    public bool IsAlreadyLoggedIn(string UserId)
    {
        bool flag = false;
        string sql = @"select UserId from User_Master where  UserId=@UserId and DATEDIFF(MINUTE, LastActiveDT, GETDATE())<=10 and IsLoggedIn='Y'";
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value =UserId.Trim() }, 
            };
        DataTable dt = db.select_data(sql, para);
        if (dt.Rows.Count > 0)
        {
            flag = true;
        }
        return flag;
    }
    public void updateLog(string UserId, string IsLoggedIn)
    {
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value =UserId.Trim() }, 
                new SqlParameter() { ParameterName = "@IsLoggedIn", Value =IsLoggedIn.Trim() },  
            };
        int res = db.insert("update User_Master set LastActiveDT=getdate(),IsLoggedIn=@IsLoggedIn where UserId=@UserId", para, con);
    }
    public bool UpdateLoginSession(string UserId)//, string Token
    {
        bool flag = false;
        string sql = @"select DATEDIFF(MINUTE, LastActiveDT, GETDATE()) as Minutes from User_Master where  UserId=@UserId and IsLoggedIn='Y'";
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value =UserId.Trim() }, 
               // new SqlParameter() { ParameterName = "@Token", Value =Token.Trim() }  and Token=@Token   and Token=@Token 
            };
        DataTable dt = db.select_data(sql, para);
        if (dt.Rows.Count > 0)
        {
            if (db.insert("update User_Master set LastActiveDT=GETDATE() where  UserId=@UserId and IsLoggedIn='Y'", para, con) > 0)
            {
                flag = true;
            }
        }
        return flag;
    }
    public bool UserActivityLog(string UserId, string ActionPerformed, string ActionStatus, string PageName)
    {
        bool flag = false;
        string sql = @"INSERT INTO UserActivityLog
                                   (UserId
                                   ,ActionPerformed
                                   ,ActionStatus
                                   ,PageName
                                   ,EntryDate
                                   ,IPAddress)
                             VALUES
                                   (@UserId
                                   ,@ActionPerformed
                                   ,@ActionStatus
                                   ,@PageName
                                   ,GETDATE()
                                   ,@IPAddress)";
        SqlParameter[] para = { 
                new SqlParameter() { ParameterName = "@UserId", Value =UserId.Trim() }, 
                new SqlParameter() { ParameterName = "@ActionPerformed", Value =ActionPerformed.Trim() }, 
                new SqlParameter() { ParameterName = "@ActionStatus", Value =ActionStatus.Trim() }, 
                new SqlParameter() { ParameterName = "@PageName", Value =PageName.Trim() }, 
                new SqlParameter() { ParameterName = "@IPAddress", Value = GetLocalIPAddress() }, 
            };
        if (db.insert(sql, para,con) > 0)
        {
            flag = true;
        }
        return flag;
    }
    public bool ActivityLog(string UserId, string ActionPerformed, string ActionStatus, SqlConnection con1, SqlTransaction tra1)
    {
        bool status = false;
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            string sql = @"INSERT INTO ActivityLog
                           (UserId
                           ,Page
                           ,ActionPerformed
                           ,ActionStatus 
                           ,IPAddress
                           ,EntryDate)
                     VALUES
                           (@UserId
                           ,@Page
                           ,@ActionPerformed
                           ,@ActionStatus 
                           ,@IPAddress
                           ,GETDATE())";
            SqlCommand cmd1 = new SqlCommand(sql, con1,tra1);
            cmd1.Parameters.AddWithValue("@UserId", UserId.Trim());
            cmd1.Parameters.AddWithValue("@ActionPerformed", ActionPerformed.Trim());
            cmd1.Parameters.AddWithValue("@ActionStatus", ActionStatus.Trim()); 
            cmd1.Parameters.AddWithValue("@IPAddress", GetLocalIPAddress().Trim());
            cmd1.Parameters.AddWithValue("@Page", GetCurrentPageName().Trim());
            int res = cmd1.ExecuteNonQuery();
            status = (res > 0) ? true : false;
        }
        catch (Exception ee)
        {
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        return status;
    }
    public int ActivityLog(string userid, string pageName, string actionPerformed, string actionStatus, string success,SqlConnection con1,SqlTransaction tra1)
    {
        int res = 0;
        try
        {
            string sql = @"INSERT INTO [ActivityLog](
           [UserId]
           ,[IPAddress]
           ,[Page]
           ,[ActionPerformed]
           ,[ActionStatus]
           ,[successflag],EntryDate
            )
            VALUES(
            @userid 
            ,@ip_address 
            ,@page           
            ,@ActionPerformed
            ,@ActionStatus
            ,@success_flag,GETDATE()
            )";
            SqlCommand cmd1 = new SqlCommand(sql, con1,tra1);
            cmd1.Parameters.AddWithValue("@userid", userid);
            cmd1.Parameters.AddWithValue("@ip_address", GetLocalIPAddress());
            cmd1.Parameters.AddWithValue("@page", pageName);
            cmd1.Parameters.AddWithValue("@ActionPerformed", actionPerformed);
            cmd1.Parameters.AddWithValue("@ActionStatus", actionStatus);
            cmd1.Parameters.AddWithValue("@success_flag", success);
            res = cmd1.ExecuteNonQuery(); 
        }
        catch (Exception ee) { }
        return res;
    }
    public bool chkParameterPollution(ContentPlaceHolder cph)
    {
        TextBox tb;
        DropDownList tb2;
        bool flag = true;
        foreach (Control c in cph.Controls)
        {
            if (c.GetType() == typeof(TextBox))
            {
                tb = (TextBox)c;
                if (tb.Text.Contains(","))
                {
                    (tb).Text = String.Empty;
                    tb.Focus();
                    flag = false;
                }
            }
            if (c.GetType() == typeof(DropDownList))
            {
                tb2 = (DropDownList)c;
                if (tb2.SelectedValue.Contains(","))
                {
                    tb2.SelectedIndex = 0;
                    tb2.Focus();
                    flag = false;
                }
            }
        }
        return true;// flag;
    }
    public bool chkParameter(UpdatePanel cph)
    {
        bool flag = true;
        foreach (Control c in cph.Controls)
        {
            foreach (Control ctrl in c.Controls)
            {
                if (ctrl is TextBox)
                {
                    string data = ((TextBox)ctrl).Text.Trim();
                    Regex regex = new Regex("^[a-zA-Z0-9_-]*$");
                    var res = regex.IsMatch(data);
                    if (res == false && data.Trim()!="")
                    {
                        flag = false;
                    }
                    /*if (((TextBox)ctrl).Text.Contains(","))
                    {
                        ((TextBox)ctrl).Focus();
                        flag = false;
                    }*/
                }
                else if (ctrl is DropDownList)
                {
                    string data = ((DropDownList)ctrl).SelectedValue.Trim();
                    Regex regex = new Regex("^[a-zA-Z0-9_-]*$");
                    var res = regex.IsMatch(data);
                    if (res == false && data.Trim() != "")
                    {
                        flag = false;
                    }
                    /*if (((TextBox)ctrl).Text.Contains(","))
                    {
                        ((TextBox)ctrl).Focus();
                        flag = false;
                    }*/
                }
            }
        }
        return flag;
    }
    public bool chkParameterPollutionDiv(UpdatePanel cph)
    {
        TextBox tb;
        DropDownList tb2;
        bool flag = true;
        foreach (Control c in cph.Controls)
        {
            if (c.GetType() == typeof(TextBox))
            {
                tb = (TextBox)c;
                if (tb.Text.Contains(","))
                {
                    (tb).Text = String.Empty;
                    tb.Focus();
                    flag = false;
                }
            }
            if (c.GetType() == typeof(DropDownList))
            {
                tb2 = (DropDownList)c;
                if (tb2.SelectedValue.Contains(","))
                {
                    tb2.SelectedIndex = 0;
                    tb2.Focus();
                    flag = false;
                }
            }
        }
        return flag;
    }
    public string preventConcurrentLoginGuid(string Userid, string UserType)
    {
        string data = "";
        string sql = "";
        if (UserType.ToUpper() == "OPERATOR")
        {
            sql = @"select PrevConnLogin from Call_Login where UPPER(UserID)=@Userid";
        }
        else if (UserType.ToUpper() == "ADMIN")
        {
            sql = @"select PrevConnLogin from user_master where UPPER(UName)=@Userid";
        }
        SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Userid", Userid.Trim().ToUpper());
        SqlDataAdapter adapt = new SqlDataAdapter(cmd);
        DataTable dt1 = new DataTable();
        adapt.Fill(dt1);
        if (dt1.Rows.Count > 0)
        {
            data = dt1.Rows[0][0].ToString();
        }
        return data;
    } 
    public void ErrorLogs(ErrorInfo data)
    {
        try
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            string sql = @"INSERT INTO ErroLog
                           (UserId
                           ,PageName
                           ,ErrorName
                           ,FunctionName
                           ,ErrorLineNo
                           ,EntryDate
                           ,IPAddrss)
                     VALUES
                           (@UserId
                           ,@PageName
                           ,@ErrorName
                           ,@FunctionName
                           ,@ErrorLineNo
                           ,GETDATE()
                           ,@IPAddrss)";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@PageName",data.PageName.Trim());
            cmd.Parameters.AddWithValue("@ErrorName",data.ErrorName.Trim());
            cmd.Parameters.AddWithValue("@IPAddrss", GetLocalIPAddress());
            cmd.Parameters.AddWithValue("@UserId",data.UserId.Trim());
            cmd.Parameters.AddWithValue("@FunctionName",data.FunctionName.Trim());
            cmd.Parameters.AddWithValue("@ErrorLineNo",data.ErrorLineNo.ToString().Trim());
            int res = cmd.ExecuteNonQuery();
            con.Close();
        }
        catch (Exception ex)
        {
        }
    }
    public void errorLogs(string userid, string page_name, string error_name, string method_name, string error_line_no, SqlConnection con1, SqlTransaction tra1)
    {
        try
        {
            string ip = GetLocalIPAddress();
            string sql = @"INSERT INTO error_logs (userid,page_name,error_name,entryDt,ip,method_name,error_line_no)values(@userid,@page_name,@error_name,GETDATE(),@ip,@method_name,@error_line_no)";
            SqlCommand cmd = new SqlCommand(sql, con1, tra1);
            cmd.Parameters.AddWithValue("@page_name", page_name);
            cmd.Parameters.AddWithValue("@error_name", error_name);
            cmd.Parameters.AddWithValue("@ip", ip);
            cmd.Parameters.AddWithValue("@userid", userid);
            cmd.Parameters.AddWithValue("@method_name", method_name);
            cmd.Parameters.AddWithValue("@error_line_no", error_line_no);
            int res = cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
        }
    }
    public string GetCurrentPageName()
    {
        string sPath = HttpContext.Current.Request.Url.AbsolutePath;
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath);
        string sRet = oInfo.Name;
        return sRet;
    }
}

}