using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PHEDChhattisgarh
{
public partial class CompaintMonitor_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            try
            {
                if (Session["UserId"] != null)
                {
                    Function fun = new Function();
                    /*string UserId = fun.ReadCookie();//get cookie
                    fun.RemoveCookie();//remove cookie
                    if (UserId.Trim() != "N/A")
                    {
                        fun.updateLog(UserId.Trim(), "N");
                    }*/
                    fun.updateLog(Session["UserId"].ToString().Trim(), "0");
                }
            }
            catch (Exception ex) { } 
            Response.Cookies["SessionToken"].Expires = DateTime.Now.AddSeconds(-1);
            // Expire authentication and session cookies
            if (Request.Cookies["ASP.NET_SessionId"] != null || Request.Cookies["eMB"] != null)
            {
                try
                {
                    HttpCookie sessionCookie = new HttpCookie("ASP.NET_SessionId", "");
                    sessionCookie.Expires = DateTime.Now.AddSeconds(-1);
                    Response.Cookies.Add(sessionCookie);
                }
                catch (Exception ex) { }
                try
                {
                    HttpCookie sessionCookie1 = new HttpCookie("eMB", "");
                    sessionCookie1.Expires = DateTime.Now.AddSeconds(-1);
                    Response.Cookies.Add(sessionCookie1);
                }
                catch (Exception ex) { }
            }

        }
        catch (Exception ee)
        {
        }
        finally
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}

}