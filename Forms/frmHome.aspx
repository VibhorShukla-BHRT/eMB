<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="frmHome.aspx.cs" Inherits="PHEDChhattisgarh.Forms_Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" Runat="Server">
Dashboard
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadcrumbContent" Runat="Server">

<ul class="breadcrumb-title">
    <li class="breadcrumb-item">
        <a href="frmHome.aspx"> <i class="fa fa-home"></i> </a>
    </li>
    <li class="breadcrumb-item">
        <a href="frmHome.aspx">Dashboard</a>
    </li> 
</ul>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="page-body">
    <div class="row">
        <div class="col-xl-3 col-md-6" id="Div_SubEngineer" runat="server">
            <div class="card">
                <div class="card-footer bg-c-brown" style="background-color:#ca61ff">
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-user text-white f-16"></i>&nbsp;Total Sub-Engineers</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_TotalSB" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-clock-o text-white f-16"></i>&nbsp;Pending By EE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_Pending" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-check text-white f-16"></i>&nbsp;Approved By EE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_Approved" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-times text-white f-16"></i>&nbsp;Rejected By EE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_Rejected" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                </div>
            </div>
        </div>
       

        <div class="col-xl-3 col-md-6" id="Div_eMB_Pending" runat="server">
            <div class="card">
                <div class="card-footer bg-c-blue">    
                    <div class="row align-items-center">
                        <div class="col-12">
                            <p class="text-white m-b-0 title_dashboard"><i class="fa fa-book text-white f-16"></i>&nbsp;Status Of e-MB Pending</p>
                        </div>
                        <%--<div class="col-3 text-right">
                            <asp:Label ID="Div_SBTotal1" runat="server" Text="0"></asp:Label>
                        </div>--%>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-clock-o text-white f-16"></i>&nbsp;Pending By AE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_AEPending" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-clock-o text-white f-16"></i>&nbsp;Pending By EE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_EEPending" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-clock-o text-white f-16"></i>&nbsp;Total Pending</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_SBPending" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                </div>
            </div>
        </div>
        <div class="col-xl-3 col-md-6" id="Div_eMB_Approved" runat="server">
            <div class="card">
                <div class="card-footer bg-c-green">
                    <div class="row align-items-center">
                        <div class="col-12">
                            <p class="text-white m-b-0 title_dashboard"><i class="fa fa-book text-white f-16"></i>&nbsp;Status Of e-MB Approved</p>
                        </div>
                       <%-- <div class="col-3 text-right">
                            <asp:Label ID="Div_SBTotal2" runat="server" Text="0"></asp:Label>
                        </div>--%>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-check text-white f-16"></i>&nbsp;Approved By AE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_AEApproved" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-check text-white f-16"></i>&nbsp;Approved By EE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_EEApproved" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-check text-white f-16"></i>&nbsp;Total Approved</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_SBApproved" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xl-3 col-md-6" id="Div_eMB_Rejected" runat="server">
            <div class="card">
                <div class="card-footer bg-c-red">
                    <div class="row align-items-center">
                        <div class="col-12">
                            <p class="text-white m-b-0 title_dashboard"><i class="fa fa-book text-white f-16"></i>&nbsp;Status Of e-MB Rejected</p>
                        </div>
                        <%--<div class="col-3 text-right">
                            <asp:Label ID="Div_SBTotal3" runat="server" Text="0"></asp:Label>
                        </div>--%>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-times text-white f-16"></i>&nbsp;Rejected By AE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_AERejected" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-times text-white f-16"></i>&nbsp;Rejected By EE</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_EERejected" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>      
                    <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-times text-white f-16"></i>&nbsp;Total Rejected</p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_SBRejected" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xl-3 col-md-6" id="Div_Scheme_Assigned" runat="server">
            <div class="card">
                <div class="card-footer bg-c-red" style="background-color:#aa7a71">
                   <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-envelope text-white f-16"></i>&nbsp;Schemes Assigned From Sub-Division To Sub-Engineer </p>
                        </div>
                        <div class="col-3 text-right">
                            <asp:Label ID="Div_TotalAssignedScheme" runat="server" Text="0" style="font-size:25px" ></asp:Label>
                        </div>
                    </div>     
                </div>
            </div>
        </div>
        
        <div class="col-xl-3 col-md-6" id="Div_eMBRequest" runat="server">
            <div class="card">
                <div class="card-footer bg-c-red" style="background-color:#ec4b94">
                   <div class="row align-items-center">
                        <div class="col-9">
                            <p class="text-white m-b-0"><i class="fa fa-book text-white f-16"></i>&nbsp;Total eMB Request From  Sub-Engineer To Sub-Division </p>
                        </div>
                        <div class="col-3 text-right"> 
                            <asp:Label ID="Div_SBTotal1" runat="server" Text="0" style="font-size:25px" ></asp:Label>
                        </div>
                    </div>     
                </div>
            </div>
        </div>
    </div>
</div> 
<link href="../assets/css/PageCSS/frmHome.css" rel="stylesheet" type="text/css" />
</asp:Content>

