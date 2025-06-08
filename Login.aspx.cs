using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;

namespace PHEDChhattisgarh
{
public partial class AppLogin : System.Web.UI.Page
{
    SqlFunction db = new SqlFunction();
    Function obj = new Function();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillCapctha(); 
            SetSalt();
        }
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["appcaptcha"] == null)
            {
                FillCapctha();
                SetSalt();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "Swal.fire('Invalid Captcha','Required','warning');", true);
            }
            else if (Session["appcaptcha"].ToString() != txtCaptcha.Text)
            { 
                FillCapctha();
                SetSalt();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "Swal.fire('Invalid Captcha','Required','warning');", true);
            }
            else
            {
                FillCapctha();
                string sql = @"SELECT UserId,Password,UserName,IsActive FROM LoginMaster where  UserId=@UserId COLLATE Latin1_General_CS_AS";//IsAppUser is null and
                SqlParameter[] para = {
                    new SqlParameter() { ParameterName = "@UserId", Value =txtUserID.Text.Trim() }, 
                };
                DataTable dt = db.select_data(sql, para);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["isActive"].ToString() =="False")
                    {
                        FillCapctha();
                        SetSalt();
                        txtPassword.Text = ""; 
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "Swal.fire('Your Id is inactive','Required','warning');", true);
                        return;
                    }
                }
                else
                {
                    SetSalt(); 
                    loginAttempt("N");
                    txtPassword.Focus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "Swal.fire('Invalid User Id Or Password','Required','warning');", true);
                    return;
                }
                string UserId = dt.Rows[0]["UserId"].ToString();
                string Password = dt.Rows[0]["Password"].ToString();
                string pwdCrypt = obj.sha256_hash(Password.ToUpper().Trim() + Session["SesValue"].ToString()).ToUpper();
                if (txtUserID.Text.ToUpper().Trim() == UserId.Trim().ToUpper().Trim() && (txtPassword.Text.Trim().ToUpper() == pwdCrypt))
                {
                    Session["login_attempt"] = null;
                    Session["UserName"] = dt.Rows[0]["UserName"].ToString().Trim();
                    Session["UserId"] = dt.Rows[0]["UserId"].ToString().Trim();
                    obj.LoginActivityLog(Session["UserId"].ToString().Trim(), "1");
                    Response.Redirect("~/Forms/frmHome.aspx", false);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "Swal.fire('Invalid User Id Or Password','Required','warning');", true);
                    obj.LoginActivityLog(Session["UserId"].ToString().Trim(), "0");
                    FillCapctha();
                    SetSalt();
                    txtPassword.Focus();
                    return;
                }
            }
        }
        catch (Exception ee)
        {
            /*Logs obj = new Logs();
            obj.errorLogs(obj.GetCurrentPageName(), ee.Message.ToString(), txtUserID.Text.Trim(), System.Reflection.MethodInfo.GetCurrentMethod().ToString(), ee.StackTrace.Substring(ee.StackTrace.LastIndexOf(' ')).ToString());
            */
            string errorMessage = ee.Message.Replace("'", "\\'");
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "alert",
                "Swal.fire('" + errorMessage + "','Required','warning');",
                true
            );
        }
    }
    private void SetSalt()
    {
        Session["SesValue"] = Guid.NewGuid().ToString("N").Trim();
        txtSesValue.Text = Session["SesValue"].ToString().Trim();
    }
    private void loginAttempt(string suceess)
    {
        /*Count Invalid Attempt*/
        if (suceess == "N")
        {
            if (Session["login_attempt"] != null)
            {
                Session["login_attempt"] = (Convert.ToInt32(Session["login_attempt"]) + 1).ToString();
            }
            if (Session["login_attempt"] == null)
            {
                Session["login_attempt"] = "1";
            }
            if (Session["login_attempt"].ToString() == "1")
            {
                FillCapctha();
                txtCaptcha.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({ title: 'You have only 2 attempts left to login, after that your login Id will be blocked', text: '', type: 'warning', allowOutsideClick: false, allowEscapeKey: false });", true);
            }
            else if (Session["login_attempt"].ToString() == "2")
            {
                FillCapctha();
                txtCaptcha.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({ title: 'You have only 1 attempts left to login, after that your login Id will be blocked', text: '', type: 'warning', allowOutsideClick: false, allowEscapeKey: false });", true);
            }
            else if (Convert.ToInt32(Session["login_attempt"].ToString()) >= 3)
            {
                SqlParameter[] para = {
                    new SqlParameter() { ParameterName = "@UserId", Value =txtUserID.Text.Trim() }, 
                };
                int res=db.update("update LoginMaster set IsActive='0' where UserId=@UserId",para);
                Session["login_attempt"] = null;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "swal({ title: 'Your ID is not activated. Contact E-In-C office to reactivate your Id', text: '', type: 'warning', allowOutsideClick: false, allowEscapeKey: false });", true);
            }
        }
        /*End of Count Invalid Attempt*/
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        txtPassword.Text = "";
        txtUserID.Text = "";
        txtCaptcha.Text = "";
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        FillCapctha();
    }  
    void FillCapctha()
    {
        try
        {
            txtCaptcha.Text = "";
            Random random = new Random();
            //string combination = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string combination = "0123456789ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder captcha = new StringBuilder();
            for (int i = 0; i < 6; i++)
                captcha.Append(combination[random.Next(combination.Length)]);
            Session["appcaptcha"] = captcha.ToString();
            imgCaptcha.ImageUrl = "GenerateCaptcha.aspx?id=LOGIN&" + DateTime.Now.Ticks.ToString();
        }
        catch
        {
            throw;
        }
    }
}

}