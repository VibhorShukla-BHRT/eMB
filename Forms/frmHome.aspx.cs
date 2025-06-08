using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace PHEDChhattisgarh
{
public partial class Forms_Home : System.Web.UI.Page
{
    Dashboard obj = new Dashboard();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", "alert('Login Expired');window.location.href='../logout.aspx';", true);
            return;
        }
        if (!Page.IsPostBack)
        {
            Div_SubEngineer.Visible = false;
            Div_eMB_Pending.Visible = false;
            Div_eMB_Approved.Visible = false;
            Div_eMB_Rejected.Visible = false;
            Div_Scheme_Assigned.Visible = false;

            Dashboard.DashboardModel model = obj.fillDashboard(Session["UserId"].ToString());
            Div_TotalSB.Text = model._SubEngineer.TotalSB;
            Div_Pending.Text = model._SubEngineer.Pending;
            Div_Approved.Text = model._SubEngineer.Approved;
            Div_Rejected.Text = model._SubEngineer.Rejected;

            Div_SBTotal1.Text = model._eMBRequestSB.SBTotal;
            //Div_SBTotal2.Text = model._eMBRequestSB.SBTotal;
            //Div_SBTotal3.Text = model._eMBRequestSB.SBTotal;

            Div_SBPending.Text = model._eMBRequestSB.SBPending;
            Div_SBApproved.Text = model._eMBRequestSB.SBApproved;
            Div_SBRejected.Text = model._eMBRequestSB.SBRejected;

            Div_AEPending.Text = model._eMBRequestAE.AEPending;
            Div_AEApproved.Text = model._eMBRequestAE.AEApproved;
            Div_AERejected.Text = model._eMBRequestAE.AERejected;

            Div_EEPending.Text = model._eMBRequestEE.EEPending;
            Div_EEApproved.Text = model._eMBRequestEE.EEApproved;
            Div_EERejected.Text = model._eMBRequestEE.EERejected;

            Div_TotalAssignedScheme.Text = model.TotalAssignedScheme;
            if (Session["UserId"].ToString().Trim().StartsWith("6"))
            {
                Div_SubEngineer.Visible =false;
                Div_eMB_Pending.Visible = true;
                Div_eMB_Approved.Visible = true;
                Div_eMB_Rejected.Visible = true;
                Div_Scheme_Assigned.Visible = true;
            }
            else
            {
                Div_SubEngineer.Visible = true;
                Div_eMB_Pending.Visible = true;
                Div_eMB_Approved.Visible = true;
                Div_eMB_Rejected.Visible = true;
                Div_Scheme_Assigned.Visible = true;
            }
        }
    }
}

}