<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ComponentList.aspx.cs" Inherits="PHEDChhattisgarh.ComponentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .phed-cg-progress-container {
            padding: 10px 0px !important;
            max-width: 100% !important;
            margin: 0 auto !important;
        }

        .phed-cg-stepper-wrapper {
            display: flex !important;
            justify-content: space-between !important;
            position: relative !important;
        }

        .phed-cg-stepper-item {
            position: relative !important;
            z-index: 1 !important;
            text-align: center !important;
            flex: 1 !important;
        }

        .phed-cg-step-indicator {
            width: 30px !important;
            height: 30px !important;
            border: 3px solid #dee2e6 !important;
            background: white !important;
            border-radius: 25% !important;
            display: flex !important;
            align-items: center !important;
            justify-content: center !important;
            margin: 0 auto 5px !important;
            font-weight: bold !important;
            transition: all 0.3s ease !important;
        }

        .phed-cg-stepper-item.phed-cg-completed .phed-cg-step-indicator {
            background: #0066cc !important;
            border-color: #0066cc !important;
            color: white !important;
        }

        .phed-cg-stepper-item.phed-cg-completed .phed-cg-step-indicator::after {
            content: '' !important;
            margin-left: 2px !important;
        }

        .phed-cg-stepper-wrapper::before {
            content: '' !important;
            position: absolute !important;
            top: 15px !important;
            left: 10% !important;
            right: 10% !important;
            height: 3px !important;
            background: #dee2e6 !important;
            z-index: 0 !important;
        }

        .phed-cg-step-text {
            font-size: 14px !important;
            color: #6c757d !important;
            margin-top: 0px !important;
        }
        
        .phed-cg-body-container {
            height: 100vh !important;
            overflow: hidden !important;
            font-family: Arial, sans-serif !important;
            margin: 0 !important;
            padding: 0 !important;
        }

        .phed-cg-main-wrapper {
            padding: 20px !important;
            height: calc(100vh - 200px) !important;
            overflow-y: auto !important;
        }

        .phed-cg-header-section {
            position: sticky !important;
            top: 0 !important;
            z-index: 1000 !important;
            background-color: #0066cc !important;
            color: white !important;
            padding: 10px 20px !important;
        }
        
        .phed-cg-form-group {
            margin-bottom: 15px !important;
        }
        
        .phed-cg-table-base th, .phed-cg-table-base td {
            padding: 8px !important;
        }
        
        .phed-cg-btn-main {
            background-color: #0066cc !important;
        }
        
        .phed-cg-field-mandatory {
            background-color: #e6f2ff !important;
        }
        
        .phed-cg-error-message {
            color: #ff0000 !important;
            margin-bottom: 15px !important;
            padding: 10px !important;
            background-color: #ffebee !important;
            border-radius: 4px !important;
            border: 1px solid #ffcdd2 !important;
        }
        
        .phed-cg-details-grid {
            border: 2px solid #000 !important;
            width: 100% !important;
            table-layout: auto !important;
            border-collapse: collapse !important;
        }
        
        .phed-cg-details-grid th {
            background-color: #f8f9fa !important; 
            border: 2px solid #000 !important;
            padding: 12px !important;
            font-weight: bold !important;
            word-wrap: break-word !important;
            white-space: normal !important;
            overflow: visible !important;
            vertical-align: top !important;
        }
        
        .phed-cg-details-grid td {
            border: 2px solid #dee2e6 !important;
            padding: 10px !important;
            word-wrap: break-word !important;
            white-space: normal !important;
            overflow: visible !important;
            vertical-align: top !important;
        }
        
        .phed-cg-floating-action {
            position: fixed !important;
            bottom: 20px !important;
            right: 20px !important;
            z-index: 1000 !important;
        }

        .phed-cg-float-button {
            background-color: #0066cc !important;
            color: white !important;
            border: none !important;
            border-radius: 50px !important;
            padding: 12px 24px !important;
            box-shadow: 0 4px 8px rgba(0,0,0,0.2) !important;
            cursor: pointer !important;
            font-weight: bold !important;
            transition: all 0.3s ease !important;
            display: flex !important;
            align-items: center !important;
            gap: 8px !important;
        }

        .phed-cg-float-button:hover {
            background-color: #004d99 !important;
            transform: translateY(-2px) !important;
            box-shadow: 0 6px 12px rgba(0,0,0,0.3) !important;
        }

        .phed-cg-float-button::before {
            content: "←" !important;
            font-size: 18px !important;
        }
        
        /* Table responsive wrapper */
        .phed-cg-table-responsive {
            width: 100% !important;
            overflow-x: auto !important;
            overflow-y: hidden !important;
            -webkit-overflow-scrolling: touch !important;
            border: 1px solid #dee2e6 !important;
            border-radius: 4px !important;
        }
        
        /* Sub-components table specific styles */
        .phed-cg-subcomp-table {
            width: 100% !important;
            min-width: 1400px !important;
            table-layout: auto !important;
            border-collapse: collapse !important;
            margin: 0 !important;
        }
        
        .phed-cg-subcomp-table th,
        .phed-cg-subcomp-table td {
            border: 1px solid #dee2e6 !important;
            padding: 8px 12px !important;
            vertical-align: top !important;
            word-wrap: break-word !important;
            white-space: normal !important;
        }
        
        .phed-cg-subcomp-table th {
            background-color: #f8f9fa !important;
            font-weight: bold !important;
            position: sticky !important;
            top: 0 !important;
            z-index: 10 !important;
        }
        
        /* Column width adjustments */
        .phed-cg-col-soritem { width: 60px !important; } /* Reduced for short content */
        .phed-cg-col-sorfrom { width: 80px !important; }
        .phed-cg-col-basicamend { width: 70px !important; } /* Reduced for short content */
        .phed-cg-col-soritemname { 
            min-width: 300px !important; 
            max-width: 400px !important; 
            white-space: normal !important;
            word-wrap: break-word !important;
        }
        .phed-cg-col-sorsubitem { 
            min-width: 350px !important; 
            max-width: 500px !important; 
            white-space: normal !important;
            word-wrap: break-word !important;
        }
        .phed-cg-col-qty { width: 60px !important; }
        .phed-cg-col-unit { width: 60px !important; }
        .phed-cg-col-amount { width: 80px !important; } /* Reduced for short content */
        .phed-cg-col-actions { width: 100px !important; }
        
        /* Card container constraints */
        .phed-cg-card-container {
            width: 100% !important;
            max-width: 100% !important;
            overflow: hidden !important;
            margin-bottom: 20px !important;
        }
        
        .phed-cg-card-body {
            padding: 15px !important;
            overflow: hidden !important;
        }
        
        /* Tooltip styles for full text display */
        .phed-cg-tooltip-container {
            position: relative !important;
            display: inline-block !important;
        }
        
        .phed-cg-tooltip {
            visibility: hidden !important;
            width: auto !important;
            min-width: 200px !important;
            max-width: 400px !important;
            background-color: #333 !important;
            color: #fff !important;
            text-align: center !important;
            border-radius: 4px !important;
            padding: 8px !important;
            position: absolute !important;
            z-index: 1000 !important;
            bottom: 125% !important;
            left: 50% !important;
            transform: translateX(-50%) !important;
            opacity: 0 !important;
            transition: opacity 0.3s !important;
            font-size: 14px !important;
            white-space: normal !important;
        }
        
        .phed-cg-tooltip-container:hover .phed-cg-tooltip {
            visibility: visible !important;
            opacity: 1 !important;
        }
        
        @keyframes phed-cg-animation-ticker {
            0% {
                transform: translateX(0) !important;
            }
            100% {
                transform: translateX(-100%) !important;
            }
        }
        
        /* Override any bootstrap or external styles specifically for this page */
        .phed-cg-card-container .card {
            overflow: hidden !important;
        }
        
        .phed-cg-card-container .card-body {
            overflow: hidden !important;
        }
        
        .phed-cg-subcomp-table.table {
            margin-bottom: 0 !important;
        }
        
        /* Adjust header spacing */
        .phed-cg-card-container .card-header h3 {
            margin: 0 !important;
            padding: 5px 0 !important;
        }

        /* Additional specificity for master page conflict resolution */
        .phed-cg-main-wrapper .phed-cg-card-container {
            border: 1px solid #dee2e6 !important;
            border-radius: 0.375rem !important;
            background-color: #fff !important;
        }

        .phed-cg-main-wrapper .phed-cg-card-container .card-header {
            background-color: #f8f9fa !important;
            border-bottom: 1px solid #dee2e6 !important;
            padding: 0.75rem 1.25rem !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" Runat="Server">
eMB Entry Form
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
     <asp:HiddenField ID="hdnTotalQuantity" runat="server" Value="0" />
    <div class="phed-cg-progress-container">
        <div class="phed-cg-stepper-wrapper">
            <div class="phed-cg-stepper-item phed-cg-completed">
                <div class="phed-cg-step-indicator">1</div>
                <div class="phed-cg-step-text">General-Abstract</div>
            </div>
            <div class="phed-cg-stepper-item phed-cg-completed">
                <div class="phed-cg-step-indicator">2</div>
                <div class="phed-cg-step-text">Sub-Estimate</div>
            </div>
            <div class="phed-cg-stepper-item phed-cg-completed">
                <div class="phed-cg-step-indicator">3</div>
                <div class="phed-cg-step-text">Component of Sub-Estimate</div>
            </div>
            <div class="phed-cg-stepper-item">
                <div class="phed-cg-step-indicator">4</div>
                <div class="phed-cg-step-text">eMB Entry</div>
            </div>
        </div>
    </div>

    <div class="phed-cg-main-wrapper">
        <asp:Label ID="lblError" runat="server" CssClass="phed-cg-error-message" Visible="false"></asp:Label>

        <div class="card mb-4 phed-cg-card-container">
            <div class="card-header">
                <h3><b>Details of Work</b></h3>
            </div>
            <div class="card-body phed-cg-card-body">
                <div class="phed-cg-table-responsive">
                    <asp:GridView ID="gvWorkDetails" runat="server" 
                        CssClass="table table-bordered phed-cg-details-grid" 
                        AutoGenerateColumns="false"
                        GridLines="Both">
                        <Columns>
                            <asp:BoundField DataField="Work_Code" HeaderText="Work Code" 
                                ItemStyle-HorizontalAlign="Center" 
                                HeaderStyle-HorizontalAlign="Center" 
                                HeaderStyle-CssClass="text-center"/>
                            <asp:BoundField DataField="WorkName" HeaderText="Name Of Work" 
                                ItemStyle-HorizontalAlign="Center" 
                                HeaderStyle-HorizontalAlign="Center" 
                                HeaderStyle-CssClass="text-center"/>
                            <asp:BoundField DataField="ComponentName" HeaderText="Component Name" 
                                ItemStyle-HorizontalAlign="Center" 
                                HeaderStyle-HorizontalAlign="Center" 
                                HeaderStyle-CssClass="text-center"/>
                            <asp:BoundField DataField="ComponentAmount" HeaderText="Component Amount (In Lakhs)" 
                                DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center" 
                                HeaderStyle-HorizontalAlign="Center" 
                                HeaderStyle-CssClass="text-center"/>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        
        <div id="divSubComponents" runat="server" class="card mt-4 phed-cg-card-container">
            <div class="card-header">
                <h3>
                    <asp:Label ID="lblSORItem" runat="server" Text=""></asp:Label> 
                    <b>SOR Sub-Item Grid</b>
                </h3>
            </div>
            <div class="card-body phed-cg-card-body">
                <div class="phed-cg-table-responsive">
                    <asp:GridView ID="gvSubComponents" runat="server" 
                        AutoGenerateColumns="False" 
                        DataKeyNames="SORItemNo, SORItem, SORFrom, SORSubItem, ActualUnit, Qty, AmountWithGST, BasicorAmendment"
                        OnRowCommand="gvSubComponents_RowCommand" 
                        CssClass="phed-cg-subcomp-table">
                        <Columns>
                            <asp:TemplateField HeaderText="Actions" 
                                HeaderStyle-CssClass="phed-cg-col-actions" ItemStyle-CssClass="phed-cg-col-actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEnter" runat="server"
                                      CommandName="EntereMB"
                                      CommandArgument='<%# Container.DataItemIndex %>'
                                      CssClass="btn btn-sm btn-primary">
                                      Enter eMB
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SORItemNo" HeaderText="SOR Sub-Item No." 
                                HeaderStyle-CssClass="phed-cg-col-soritem" ItemStyle-CssClass="phed-cg-col-soritem" />
                            
                            <asp:BoundField DataField="SORFrom" HeaderText="SOR From" 
                                HeaderStyle-CssClass="phed-cg-col-sorfrom" ItemStyle-CssClass="phed-cg-col-sorfrom" />
                            
                            <asp:BoundField DataField="BasicorAmendment"
                                HeaderText="Basic or Amendment"
                                NullDisplayText="Basic" 
                                HeaderStyle-CssClass="phed-cg-col-basicamend" ItemStyle-CssClass="phed-cg-col-basicamend" />
                            
                            <asp:TemplateField HeaderText="SOR Item" 
                                HeaderStyle-CssClass="phed-cg-col-soritemname" ItemStyle-CssClass="phed-cg-col-soritemname">
                                <ItemTemplate>
                                    <div class="phed-cg-tooltip-container">
                                        <span><%# Eval("SORItem") %></span>
                                        <div class="phed-cg-tooltip"><%# Eval("SORItem") %></div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="SOR Sub-Item" 
                                HeaderStyle-CssClass="phed-cg-col-sorsubitem" ItemStyle-CssClass="phed-cg-col-sorsubitem">
                                <ItemTemplate>
                                    <div class="phed-cg-tooltip-container">
                                        <span><%# Eval("SORSubItem") %></span>
                                        <div class="phed-cg-tooltip"><%# Eval("SORSubItem") %></div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="Qty" HeaderText="Qty" 
                                HeaderStyle-CssClass="phed-cg-col-qty" ItemStyle-CssClass="phed-cg-col-qty" />
                            
                            <asp:BoundField DataField="ActualUnit" HeaderText="Unit" 
                                HeaderStyle-CssClass="phed-cg-col-unit" ItemStyle-CssClass="phed-cg-col-unit" />
                            
                            <asp:BoundField DataField="AmountWithGST" HeaderText="Amount (with GST)" 
                                HeaderStyle-CssClass="phed-cg-col-amount" ItemStyle-CssClass="phed-cg-col-amount" />
                            <asp:BoundField DataField="UnitCost" HeaderText="Cost/Unit(₹)" 
                                HeaderStyle-CssClass="phed-cg-col-sorfrom" ItemStyle-CssClass="phed-cg-col-sorfrom" />
                            
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

        <div class="phed-cg-floating-action">
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-secondary phed-cg-float-button" OnClick="btnPrevious_Click" />
        </div>
    </div>
    <script>
        // Collapse sidebar on page load
        $(document).ready(function () {
            $('body').addClass('navbar-collapsed');
            $('#pcoded').addClass('navbar-collapsed');
        });
    </script>
</asp:Content>