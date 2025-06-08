using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PHEDChhattisgarh
{
public partial class MasterPages_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null || Session["UserName"] == null)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "window.location.href='../logout.aspx';", true);
            return;
        }
        else
        {
            UserName2.InnerHtml = Session["UserName"].ToString().Trim();
            UserName3.InnerHtml = "<span id='more-details'>" + Session["UserName"].ToString().Trim() + "<i class='fa fa-caret-down'></i></span>";
        }
        if (!Page.IsPostBack)
        {
            setMenu(Session["UserId"].ToString().Trim());
        }
    }
    private void setMenu(string UserId)
    {
        //forms
        frmCreateSubEngIdAE.Visible = false;
        frmCreateSubEngIdEE.Visible = false;
        frmAssignSchemes.Visible = false;
        frmeMBBookRequestSB.Visible = false;
        frmeMBBookRequestAE.Visible = false;
        frmeMBBookRequestEE.Visible = false;
        frmeMBEntry.Visible = false;
        frmeMBTransfer.Visible = false;
        frmAssignedSchemesRollBack.Visible = false;
        //reports
        rptListOfSubEngineers.Visible = false;
        AssignSchemesRpt.Visible = false;
        if (UserId.StartsWith("1"))
        {
            //reports
            rptListOfSubEngineers.Visible = true;
            AssignSchemesRpt.Visible = true;
        }
        else if (UserId.StartsWith("2"))
        {
            //reports
            rptListOfSubEngineers.Visible = true;
            AssignSchemesRpt.Visible = true;
        }
        else if (UserId.StartsWith("3"))
        {
            //reports
            rptListOfSubEngineers.Visible = true;
            AssignSchemesRpt.Visible = true;
        }
        else if (UserId.StartsWith("4"))
        {
            //forms
            frmCreateSubEngIdEE.Visible = true;
            frmeMBBookRequestEE.Visible = true;
            frmeMBTransfer.Visible = true;
            //reports
            rptListOfSubEngineers.Visible = true;
            AssignSchemesRpt.Visible = true;
        }
        else if (UserId.StartsWith("5"))
        {
            //forms
            frmCreateSubEngIdAE.Visible = true;
            frmAssignSchemes.Visible = true;
            frmeMBBookRequestAE.Visible = true;
            frmAssignedSchemesRollBack.Visible = true;
            //reports
            rptListOfSubEngineers.Visible = true;
            AssignSchemesRpt.Visible = true;
        }
        else if (UserId.StartsWith("6"))
        {
            //forms
            frmeMBBookRequestSB.Visible = true;
            frmeMBEntry.Visible = true;
            //reports
            rptListOfSubEngineers.Visible = true;
            AssignSchemesRpt.Visible = true;
        }
    }
}

}
