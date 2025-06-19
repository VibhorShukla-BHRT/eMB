<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="Report01.aspx.cs" Inherits="PHEDChhattisgarh.Report01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .container01 {
            max-width: 1000px;
            margin: 0 auto;
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        .grid-container01 {
            margin-top: 20px;
            max-height: 400px; /* Set maximum height for scrolling */
            overflow-y: auto;
            border: 1px solid #ddd;
            border-radius: 4px;
            position: relative;
        }
        .data-grid {
            width: 100%;
            border-collapse: collapse;
            margin: 0;
            font-size: 14px;
        }
        .data-grid th {
            background-color: #007acc;
            color: white;
            padding: 12px 8px;
            text-align: center;
            font-weight: bold;
            border: 1px solid #ddd;
            position: sticky;
            top: 0;
            z-index: 10;
            box-shadow: 0 2px 2px -1px rgba(0, 0, 0, 0.4);
        }
        .data-grid td {
            padding: 10px 8px;
            border: 1px solid #ddd;
            text-align: center;
        }
        .data-grid tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .data-grid tr:hover {
            background-color: #e6f3ff;
        }
        .refresh-btn {
            background-color: #007acc;
            color: white;
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
            margin-bottom: 20px;
        }
        .refresh-btn:hover {
            background-color: #005999;
        }
        .error-message {
            color: #d32f2f;
            background-color: #ffebee;
            padding: 10px;
            border-radius: 4px;
            margin: 10px 0;
            border-left: 4px solid #d32f2f;
        }
        .no-data {
            text-align: center;
            color: #666;
            font-style: italic;
            padding: 20px;
        }
        
        /* Additional styling for better fixed header appearance */
        .data-grid thead th {
            background-color: #007acc !important;
        }
        
        /* Ensure proper scrollbar styling */
        .grid-container01::-webkit-scrollbar {
            width: 8px;
        }
        
        .grid-container01::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 4px;
        }
        
        .grid-container01::-webkit-scrollbar-thumb {
            background: #888;
            border-radius: 4px;
        }
        
        .grid-container01::-webkit-scrollbar-thumb:hover {
            background: #555;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" Runat="Server">
    Division eMB Report
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
        <div class="container01">
            <div class="header">
                <h1>Division eMB Report</h1>
                <p>Division-wise eMB Issued and Initiated Summary</p>
            </div>
            
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh Data" 
                CssClass="refresh-btn" OnClick="btnRefresh_Click" />
            
            <asp:Label ID="lblError" runat="server" CssClass="error-message" 
                Visible="false"></asp:Label>
            
            <div class="grid-container01">
                <asp:GridView ID="gvDivisionReport" runat="server" 
                    CssClass="data-grid"
                    AutoGenerateColumns="false"
                    EmptyDataText="No data found."
                    EmptyDataRowStyle-CssClass="no-data">
                    <Columns>
                        <asp:BoundField DataField="DivisionName" HeaderText="Division" 
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="50%" />
                        <asp:BoundField DataField="IssuedCount" HeaderText="eMB Issued" 
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" />
                        <asp:BoundField DataField="EmbInitiated" HeaderText="eMB Initiated" 
                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="25%" />
                        <asp:TemplateField HeaderText="Percentage %" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# GetPercentage(Eval("EmbInitiated"), Eval("IssuedCount")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>