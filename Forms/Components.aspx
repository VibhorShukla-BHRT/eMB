<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="Components.aspx.cs" Inherits="PHEDChhattisgarh.Components" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style>
        .components-container {
            padding: 10px;
            height: calc(100vh - 160px);
            overflow-y: auto;
        }

        /* Mobile adjustments */
        @media (max-width: 768px) {
            .components-container {
                padding: 5px;
                height: calc(100vh - 140px);
            }
        }

        /* Keep header and progress bar fixed */
        .components-header {
            position: sticky;
            top: 0;
            z-index: 1000;
            background-color: #0066cc;
            color: white;
            padding: 10px 20px;
        }

        .components-progress-container {
            position: sticky;
            top: 80px;
            z-index: 999;
            background: white;
            padding: 10px 0;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }

        .components-stepper-wrapper {
            display: flex;
            justify-content: space-between;
            position: relative;
        }

        .components-stepper-item {
            position: relative;
            z-index: 1;
            text-align: center;
            flex: 1;
        }

        .components-step-counter {
            width: 30px;
            height: 30px;
            border: 3px solid #dee2e6;
            background: white;
            border-radius: 25%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 5px;
            font-weight: bold;
            transition: all 0.3s ease;
        }

        .components-stepper-item.completed .components-step-counter {
            background: #0066cc;
            border-color: #0066cc;
            color: white;
        }

        .components-stepper-item.completed .components-step-counter::after {
            content: '';
            margin-left: 2px;
        }

        .components-stepper-wrapper::before {
            content: '';
            position: absolute;
            top: 15px;
            left: 10%;
            right: 10%;
            height: 3px;
            background: #dee2e6;
            z-index: 0;
        }

        .components-step-name {
            font-size: 14px;
            color: #6c757d;
            margin-top: 0px;
        }

        .components-form-group {
            margin-bottom: 15px;
        }

        /* Custom Grid Styles - These override master page grid styles */
        .components-grid {
            width: 100% !important;
            border-collapse: collapse !important;
            margin: 0 !important;
            background-color: white !important;
            font-size: 14px !important;
        }

        .components-grid th {
            background-color: #f8f9fa !important;
            border: 2px solid #000 !important;
            padding: 12px 8px !important;
            font-weight: bold !important;
            text-align: center !important;
            vertical-align: middle !important;
            color: #333 !important;
            font-size: 13px !important;
            white-space: normal !important;
            word-wrap: break-word !important;
        }

        .components-grid td {
            border: 1px solid #dee2e6 !important;
            padding: 10px 8px !important;
            text-align: center !important;
            vertical-align: middle !important;
            background-color: white !important;
            color: #333 !important;
            font-size: 13px !important;
            white-space: normal !important;
            word-wrap: break-word !important;
        }

        .components-grid tr:nth-child(even) td {
            background-color: #f9f9f9 !important;
        }

        .components-grid tr:hover td {
            background-color: #e3f2fd !important;
        }

        /* Card Styles - Renamed to avoid conflicts */
        .components-card {
            background: #fff !important;
            border: 1px solid #dee2e6 !important;
            border-radius: 8px !important;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1) !important;
            margin-bottom: 20px !important;
            overflow: hidden !important;
        }

        .components-card-header {
            background-color: #f8f9fa !important;
            border-bottom: 1px solid #dee2e6 !important;
            padding: 15px 20px !important;
            margin: 0 !important;
        }

        .components-card-header h3 {
            margin: 0 !important;
            padding: 0 !important;
            color: #333 !important;
            font-size: 18px !important;
            font-weight: bold !important;
        }

        .components-card-body {
            padding: 20px !important;
            background: white !important;
        }

        /* Button Styles */
        .components-btn-primary {
            background-color: #0066cc !important;
            border-color: #0066cc !important;
            color: white !important;
            padding: 6px 6px !important;
            font-size: 12px !important;
            border-radius: 4px !important;
            text-decoration: none !important;
            display: inline-block !important;
            margin: 2px !important;
            min-width: 200px !important;
            text-align: center !important;
        }

        .components-btn-primary:hover {
            background-color: #0052a3 !important;
            border-color: #0052a3 !important;
            color: white !important;
            text-decoration: none !important;
        }

        .components-btn-primary:disabled {
            background-color: #0066cc !important;
            border-color: #0066cc !important;
            opacity: 0.65 !important;
            color: white !important;
            cursor: not-allowed !important;
        }

        .components-btn-success {
            background-color: #28a745 !important;
            border-color: #28a745 !important;
            color: white !important;
            padding: 6px 6px !important;
            font-size: 12px !important;
            border-radius: 4px !important;
            text-decoration: none !important;
            display: inline-block !important;
            margin: 2px !important;
            min-width: 200px !important;
            text-align: center !important;
        }

        .components-btn-success:hover {
            background-color: #218838 !important;
            border-color: #1e7e34 !important;
            color: white !important;
            text-decoration: none !important;
        }

        .components-btn-secondary {
            background-color: #6c757d !important;
            border-color: #6c757d !important;
            color: white !important;
            padding: 6px 12px !important;
            font-size: 12px !important;
            border-radius: 4px !important;
            text-decoration: none !important;
            display: inline-block !important;
            margin: 2px !important;
            min-width: 140px !important;
            text-align: center !important;
        }

        .components-btn-secondary:hover {
            background-color: #5a6268 !important;
            border-color: #545b62 !important;
            color: white !important;
            text-decoration: none !important;
        }

        .components-btn-warning {
            background-color: #ffc107 !important;
            border-color: #ffc107 !important;
            color: #212529 !important;
            padding: 6px 12px !important;
            font-size: 12px !important;
            border-radius: 4px !important;
            text-decoration: none !important;
            display: inline-block !important;
            margin: 2px !important;
            min-width: 140px !important;
            text-align: center !important;
        }

        .components-btn-info {
            background-color: #17a2b8 !important;
            border-color: #17a2b8 !important;
            color: white !important;
            padding: 6px 12px !important;
            font-size: 12px !important;
            border-radius: 4px !important;
            text-decoration: none !important;
            display: inline-block !important;
            margin: 2px !important;
            min-width: 140px !important;
            text-align: center !important;
        }

        .components-action-buttons {
            display: flex !important;
            flex-direction: column !important;
            gap: 5px !important;
            align-items: center !important;
            justify-content: center !important;
        }

        .components-actions-column {
            text-align: center !important;
            vertical-align: middle !important;
            width: 140px !important;
            min-width: 140px !important;
        }

        /* Floating Button */
        .components-floating-button-container {
            position: fixed;
            bottom: 20px;
            right: 20px;
            z-index: 1000;
        }

        .components-floating-button {
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
            text-decoration: none !important;
        }

        .components-floating-button:hover {
            background-color: #004d99 !important;
            transform: translateY(-2px) !important;
            box-shadow: 0 6px 12px rgba(0,0,0,0.3) !important;
            color: white !important;
            text-decoration: none !important;
        }

        .components-floating-button::before {
            content: "←";
            font-size: 18px;
        }

        /* Progress Entry Styles */
        .components-progress-entry-panel {
            display: none;
            background: #f8f9fa;
            border: 2px solid #0066cc;
            border-radius: 8px;
            padding: 20px;
            margin: 20px 0;
        }

        .components-progress-form {
            background: white !important;
            padding: 20px !important;
            border-radius: 5px !important;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1) !important;
            margin-bottom: 20px !important;
        }

        .components-component-info {
            background: #e3f2fd !important;
            padding: 15px !important;
            border-radius: 5px !important;
            margin-bottom: 20px !important;
            border-left: 4px solid #0066cc !important;
        }

        /* Form Controls */
        .components-form-control {
            width: 100% !important;
            padding: 8px 12px !important;
            border: 1px solid #ced4da !important;
            border-radius: 4px !important;
            font-size: 14px !important;
            background-color: white !important;
        }

        .components-form-control:focus {
            border-color: #0066cc !important;
            box-shadow: 0 0 0 0.2rem rgba(0, 102, 204, 0.25) !important;
            outline: 0 !important;
        }

        .components-form-label {
            font-weight: bold !important;
            margin-bottom: 5px !important;
            display: block !important;
            color: #333 !important;
        }
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

        .components-form-control-plaintext {
            padding: 8px 0 !important;
            font-weight: bold !important;
            color: #0066cc !important;
        }
        .validator-container {
    display: flex;
    flex-wrap: wrap;
    gap: 5px;
    margin-top: 5px;
}
        .components-grid td.text-truncate {
    max-width: 300px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.components-grid td.text-truncate:hover {
    white-space: normal;
    overflow: visible;
    text-overflow: clip;
    background-color: #e3f2fd;
    z-index: 100;
    position: relative;
}

@media (max-width: 768px) {
    .components-grid td.text-truncate {
        max-width: 100px;
    }
}

.validator-item {
    display: inline-block;
    white-space: nowrap;
}
        /* Row and Column Fixes */
        .components-row {
            display: flex !important;
            flex-wrap: wrap !important;
            margin: 0 -7.5px !important;
        }

        .components-col-md-2,
        .components-col-md-3,
        .components-col-md-4,
        .components-col-md-6 {
            padding: 0 7.5px !important;
            margin-bottom: 15px !important;
        }

        .components-col-md-2 { flex: 0 0 16.666667% !important; max-width: 16.666667% !important; }
        .components-col-md-3 { flex: 0 0 25% !important; max-width: 25% !important; }
        .components-col-md-4 { flex: 0 0 33.333333% !important; max-width: 33.333333% !important; }
        .components-col-md-6 { flex: 0 0 50% !important; max-width: 50% !important; }

        /* Survey Panel Styles */
        .components-survey-panel {
            margin-bottom: 20px !important;
        }

        .components-survey-header {
            padding: 10px 15px !important;
            margin: 0 !important;
            border-radius: 4px 4px 0 0 !important;
        }

        .components-survey-body {
            padding: 15px !important;
            border-radius: 0 0 4px 4px !important;
        }

        /* Text utilities */
        .components-text-danger {
            color: #dc3545 !important;
            font-size: 12px !important;
            display: block !important;
            margin-top: 5px !important;
        }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .components-col-md-2,
            .components-col-md-3,
            .components-col-md-4,
            .components-col-md-6 {
                flex: 0 0 100% !important;
                max-width: 100% !important;
            }

            .components-grid th,
            .components-grid td {
                padding: 8px 4px !important;
                font-size: 12px !important;
            }

            .components-actions-column {
                width: 140px !important;
                min-width: 140px !important;
            }

            .components-btn-primary,
            .components-btn-success,
            .components-btn-secondary {
                min-width: 120px !important;
                font-size: 11px !important;
                padding: 5px 8px !important;
            }
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
            <div class="phed-cg-stepper-item">
                <div class="phed-cg-step-indicator">3</div>
                <div class="phed-cg-step-text">Component of Sub-Estimate</div>
            </div>
            <div class="phed-cg-stepper-item">
                <div class="phed-cg-step-indicator">4</div>
                <div class="phed-cg-step-text">eMB Entry</div>
            </div>
        </div>
    </div>


    <div class="components-container">

        <!-- Work Details Panel -->
        <div class="components-card mt-4" id="workDetailsPanel" runat="server">
            <div class="components-card-header">
                <h3><b>Details of Work</b></h3>
            </div>
            <div class="components-card-body">
                <asp:GridView ID="gvWorkDetails" runat="server" 
                    CssClass="components-grid" 
                    AutoGenerateColumns="false"
                    GridLines="Both">
                    <Columns>
                        <asp:BoundField DataField="Work_Code" HeaderText="Work Code" ItemStyle-HorizontalAlign="Center" 
                            HeaderStyle-HorizontalAlign="Center" 
                            HeaderStyle-CssClass="text-center"/>
                        <asp:TemplateField HeaderText="Name Of Work" ItemStyle-HorizontalAlign="Center" 
                            HeaderStyle-HorizontalAlign="Center" 
                            HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <%# Eval("WorkName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="AA_Amount" HeaderText="AA Amount (In Lakhs)" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center" 
                            HeaderStyle-HorizontalAlign="Center" 
                            HeaderStyle-CssClass="text-center"/>
                        <asp:BoundField DataField="AgreementType" HeaderText="Agreement Type" ItemStyle-HorizontalAlign="Center" 
                            HeaderStyle-HorizontalAlign="Center" 
                            HeaderStyle-CssClass="text-center"/>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <!-- Component List Panel -->
        <div class="components-card" id="componentListPanel" runat="server">
            <div class="components-card-header">
                <h3><b>Component List </b></h3>
            </div>
            <div class="components-card components-card-body">
                <asp:GridView ID="gvComponents" runat="server" CssClass="components-grid" AutoGenerateColumns="false"
                OnRowCommand="gvComponents_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ComponentID" HeaderText="CompId" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
            HeaderStyle-HorizontalAlign="Center" 
            HeaderStyle-CssClass="text-center"/>
            
        <asp:BoundField DataField="ComponentName" HeaderText="ComponentName" 
            ItemStyle-HorizontalAlign="Center" 
            HeaderStyle-HorizontalAlign="Center" 
            HeaderStyle-CssClass="text-center"
            ItemStyle-Width="500px" 
            ItemStyle-CssClass="text-truncate"/>
            
        <asp:BoundField DataField="AA_Quantity" HeaderText="AA_Quantity" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"
            HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="text-truncate"
            HeaderStyle-CssClass="text-center" DataFormatString="{0:N2}"/>
                        <asp:BoundField DataField="RemainingQty" HeaderText="Remaining Qty" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="50px"
                            HeaderStyle-HorizontalAlign="Center" 
                            HeaderStyle-CssClass="text-center" DataFormatString="{0:N2}"/>
                        <asp:BoundField DataField="ComponentUnit" HeaderText="Unit" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="text-truncate" ItemStyle-Width="50px"
                            HeaderStyle-HorizontalAlign="Center" 
                            HeaderStyle-CssClass="text-center"/>
                        <asp:BoundField DataField="Amount" HeaderText="Amount (in lakhs)" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" ItemStyle-CssClass="text-truncate"
                            HeaderStyle-HorizontalAlign="Center" 
                            HeaderStyle-CssClass="text-center"/>

                        <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="components-actions-column text-truncate" HeaderStyle-CssClass="text-center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <div class="components-action-buttons">
                                    <asp:LinkButton ID="btnViewComponents" runat="server" CommandName="ViewComponents" 
                                        CommandArgument='<%# Eval("Work_Code") + "," + Eval("AgreementBy") + "," + 
                                                            Eval("Year_Of_Agreement") + "," + Eval("Agreement_No") + "," + 
                                                            Eval("ComponentId") + "," + Eval("Amount") %>' 
                                        CssClass="components-btn-primary">
                                        View Sub-Components
                                    </asp:LinkButton>
            
                                    <asp:LinkButton ID="btnEnterProgress" runat="server" CommandName="EnterProgress" 
                                        CommandArgument='<%# Eval("ComponentId") + "#" + Eval("ComponentName") + "#" + 
                                                            Eval("AA_Quantity") + "#" + Eval("ComponentUnit") %>' 
                                        CssClass="components-btn-success">
                                        Enter Progress
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
            </div>
        </div>

            <!-- Progress Entry Panel -->
            <div class="components-card" id="progressEntryPanel" runat="server" style="display: none;">
                <div class="components-card-header">
                    <h3><b>Enter Physical Progress</b></h3>
                </div>
                <div class="components-card-body">
                    <!-- Component Information -->
                    <div class="components-component-info">
                        <div class="components-row">
                            <div class="components-col-md-3">
                                <strong>Component ID:</strong> 
                                <asp:Label ID="lblComponentId" runat="server"></asp:Label>
                            </div>
                            <div class="components-col-md-6">
                                <strong>Component Name:</strong> 
                                <asp:Label ID="lblComponentName" runat="server"></asp:Label>
                            </div>
                            <div class="components-col-md-3">
                                <strong>Quantity:</strong> 
                                <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                                <asp:Label ID="lblUnit" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- Progress Entry Form -->
                    <div class="components-progress-form">
                        <div class="components-row">
                            <div class="components-col-md-2">
                                <label class="components-form-label"><strong>Entry Type:</strong></label>
                                <asp:DropDownList ID="ddlEntryType" runat="server" CssClass="components-form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEntryType_SelectedIndexChanged">
                                    <asp:ListItem Value="Percentage" Text="Percentage" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="Quantity" Text="Quantity"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="components-col-md-3">
                        <label id="lblInputType" runat="server" class="components-form-label"></label>
                        <asp:TextBox ID="txtProgressValue" runat="server" CssClass="components-form-control" 
                            placeholder="Enter percentage (0-100)" type="number" min="0" max="100" step="0.01"></asp:TextBox>
    
                        <div class="validator-container">
                            <asp:RequiredFieldValidator ID="rfvProgressValue" runat="server" 
                                ControlToValidate="txtProgressValue" 
                                ErrorMessage="Value required" 
                                CssClass="validator-item components-text-danger" 
                                ValidationGroup="ProgressEntry"></asp:RequiredFieldValidator>
        
                            <asp:RangeValidator ID="rvProgressPercentage" runat="server" 
                                ControlToValidate="txtProgressValue" 
                                ErrorMessage="" 
                                CssClass="validator-item components-text-danger" 
                                ValidationGroup="ProgressEntry"
                                MinimumValue="0" 
                                MaximumValue="100" 
                                Type="Double"
                                Display="Dynamic"></asp:RangeValidator>
        
                            <asp:CustomValidator ID="cvProgressValue" runat="server" 
                                ControlToValidate="txtProgressValue" 
                                ErrorMessage="" 
                                CssClass="validator-item components-text-danger" 
                                ValidationGroup="ProgressEntry"
                                OnServerValidate="cvProgressValue_ServerValidate"
                                Display="Dynamic"></asp:CustomValidator>
                            <asp:CustomValidator 
                                ID="cvProgressIncrement" 
                                runat="server" 
                                ControlToValidate="txtProgressValue"
                                ClientValidationFunction="validateProgressIncrement"
                                OnServerValidate="cvProgressIncrement_ServerValidate"
                                ValidationGroup="ProgressEntry"
                                CssClass="validator-item components-text-danger"
                                Display="Dynamic"
                                ErrorMessage="">
                            </asp:CustomValidator>
                        </div>
</div>
                            <div class="components-col-md-3">
                                <label class="components-form-label"><strong>Calculated Value:</strong></label>
                                <asp:Label ID="lblCalculatedValue" runat="server" CssClass="components-form-control-plaintext text-primary font-weight-bold"></asp:Label>
                            </div>
                            <div class="components-col-md-4">
                                <label class="components-form-label"><strong>Entry Date:</strong></label>
                                <asp:Label ID="lblEntryDate" runat="server" CssClass="components-form-control-plaintext"></asp:Label>
                            </div>
                            <div class="components-col-md-4" style="padding-top: 32px;">
                                <asp:Button ID="btnSaveProgress" runat="server" Text="Save Progress" 
                                    CssClass="btn btn-success" OnClick="btnSaveProgress_Click" 
                                    ValidationGroup="ProgressEntry" />
                            </div>
                        </div>
                    </div>
                    <!-- CSEB Survey Panel - Only for Component ID 26 -->
                    <div class="components-card" id="csebSurveyPanel" runat="server" style="display: none; margin-bottom: 20px;">
                        <div class="components-card-header" style="background-color: #fff3cd; border-color: #ffeaa7;">
                            <h5><b>🔌 CSEB Connection Survey</b></h5>
                        </div>
                        <div class="components-card-body" style="background-color: #fffbf0;">
                            <div class="components-row">
                                <div class="components-col-md-6">
                                    <label class="components-form-label"><strong>Status of CSEB Connection:</strong></label>
                                    <asp:DropDownList ID="ddlCSEBStatus" runat="server" CssClass="components-form-control">
                                        <asp:ListItem Value="" Text="-- Select Status --" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Provided" Text="1. Provided"></asp:ListItem>
                                        <asp:ListItem Value="Pending with Contractor" Text="2. Pending with Contractor"></asp:ListItem>
                                        <asp:ListItem Value="Pending with CSEB" Text="3. Pending with CSEB"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCSEBStatus" runat="server" 
                                        ControlToValidate="ddlCSEBStatus" 
                                        ErrorMessage="Please select CSEB connection status" 
                                        CssClass="components-text-danger" 
                                        ValidationGroup="CSEBSurvey"></asp:RequiredFieldValidator>
                                </div>
                                <div class="components-col-md-3" style="padding-top: 32px;">
                                    <asp:Button ID="btnSaveCSEBSurvey" runat="server" Text="Save Survey" 
                                        CssClass="components-btn-warning" OnClick="btnSaveCSEBSurvey_Click" 
                                        ValidationGroup="CSEBSurvey" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Source Availability Survey Panel - Only for Component ID 2 and 3 -->
                    <div class="components-card" id="sourceSurveyPanel" runat="server" style="display: none; margin-bottom: 20px;">
                        <div class="components-card-header" style="background-color: #d1ecf1; border-color: #bee5eb;">
                            <h5><b>💧 Source Availability Survey</b></h5>
                        </div>
                        <div class="components-card-body" style="background-color: #f0f8ff;">
                            <div class="components-row">
                                <div class="components-col-md-6">
                                    <label class="components-form-label"><strong>The source with sufficient yield is available for scheme:</strong></label>
                                    <asp:DropDownList ID="ddlSourceAvailable" runat="server" CssClass="components-form-control">
                                        <asp:ListItem Value="" Text="-- Select --" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvSourceAvailable" runat="server" 
                                        ControlToValidate="ddlSourceAvailable" 
                                        ErrorMessage="Please select source availability status" 
                                        CssClass="components-text-danger" 
                                        ValidationGroup="SourceSurvey"></asp:RequiredFieldValidator>
                                </div>
                                <div class="components-col-md-3" style="padding-top: 32px;">
                                    <asp:Button ID="btnSaveSourceSurvey" runat="server" Text="Save Survey" 
                                        CssClass="components-btn-info" OnClick="btnSaveSourceSurvey_Click" 
                                        ValidationGroup="SourceSurvey" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Previous Progress Entries -->
                    <div class="components-card">
                        <div class="components-card-header">
                            <h5><b>Previous Progress Entries</b></h5>
                        </div>
                        <div class="components-card-body">
                            <asp:GridView ID="gvProgressHistory" runat="server" 
                                CssClass="table table-bordered table-striped" 
                                AutoGenerateColumns="false"
                                EmptyDataText="No previous progress entries found.">
                                <Columns>
                                    <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" 
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" 
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Percentage" HeaderText="Cumulative Progress (%)" 
                                        DataFormatString="{0:N2}" 
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Qty" HeaderText="Quantity Completed" 
                                        DataFormatString="{0:N2}" 
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="components-floating-button-container">
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-primary components-floating-button" OnClick="btnPrevious_Click" />
        </div>
     <script>
         // Collapse sidebar on page load
         $(document).ready(function () {
             $('body').addClass('navbar-collapsed');
             $('#pcoded').addClass('navbar-collapsed');
         });
     </script>
    <script type="text/javascript">
        function updateCalculatedValue() {
            const entryType = document.getElementById('<%= ddlEntryType.ClientID %>').value;
        const inputValue = parseFloat(document.getElementById('<%= txtProgressValue.ClientID %>').value) || 0;
        const totalQty = parseFloat(document.getElementById('<%= lblQuantity.ClientID %>').innerText.trim()) || 0;
        const unit = document.getElementById('<%= lblUnit.ClientID %>').innerText;
        const calculatedLabel = document.getElementById('<%= lblCalculatedValue.ClientID %>');

            if (entryType === "Percentage") {
                if (inputValue > 100) {
                    calculatedLabel.innerText = "❌ Percentage cannot exceed 100%";
                    calculatedLabel.className = "components-form-control-plaintext text-danger font-weight-bold";
                } else {
                    const calculatedQty = (totalQty * inputValue) / 100;
                    calculatedLabel.innerText = `Quantity: ${calculatedQty.toFixed(2)} ${unit}`;
                    calculatedLabel.className = "components-form-control-plaintext text-primary font-weight-bold";
                }
            } else {
                if (inputValue > totalQty) {
                    calculatedLabel.innerText = `❌ Quantity cannot exceed ${totalQty.toFixed(2)}`;
                    calculatedLabel.className = "components-form-control-plaintext text-danger font-weight-bold";
                } else {
                    const calculatedPercentage = totalQty > 0 ? (inputValue / totalQty) * 100 : 0;
                    calculatedLabel.innerText = `Percentage: ${calculatedPercentage.toFixed(2)}%`;
                    calculatedLabel.className = "components-form-control-plaintext text-primary font-weight-bold";
                }
            }
        }

        // Attach events
        document.getElementById('<%= txtProgressValue.ClientID %>').addEventListener('input', updateCalculatedValue);
        document.getElementById('<%= ddlEntryType.ClientID %>').addEventListener('change', function() {
        const inputLabel = document.getElementById('<%= lblInputType.ClientID %>');
        const progressInput = document.getElementById('<%= txtProgressValue.ClientID %>');
        
        if (this.value === "Percentage") {
            inputLabel.innerHTML = "<strong>Progress Percentage (%):</strong>";
            progressInput.placeholder = "Enter percentage (0-100)";
            progressInput.setAttribute("max", "100");
        } else {
            inputLabel.innerHTML = "<strong>Completed Quantity:</strong>";
            progressInput.placeholder = "Enter completed quantity";
            progressInput.removeAttribute("max");
        }
        
        progressInput.value = "";
        document.getElementById('<%= lblCalculatedValue.ClientID %>').innerText = "";
    });
        function validateProgressIncrement(source, args) {
            const entryType = document.getElementById('<%= ddlEntryType.ClientID %>').value;
            const inputValue = parseFloat(args.Value) || 0;
            const latestProgress = parseFloat('<%= GetLatestProgressForClient() %>') || 0;
    
    if (entryType === "Percentage") {
        args.IsValid = inputValue >= latestProgress;
        if (!args.IsValid) {
            source.errormessage = `Progress cannot be less than ${latestProgress.toFixed(2)}%`;
        }
    } else {
                const totalQty = parseFloat('<%= lblQuantity.Text %>') || 0;
                const currentQty = (latestProgress * totalQty) / 100;
                args.IsValid = inputValue >= currentQty;
                if (!args.IsValid) {
                    source.errormessage = `Quantity cannot be less than ${currentQty.toFixed(2)}`;
                }
            }
        }
    // Initialize on page load
    document.addEventListener('DOMContentLoaded', function() {
        const entryType = document.getElementById('<%= ddlEntryType.ClientID %>');
        if (entryType.value === "Percentage") {
            document.getElementById('<%= lblInputType.ClientID %>').innerHTML = "<strong>Progress Percentage (%):</strong>";
        } else {
            document.getElementById('<%= lblInputType.ClientID %>').innerHTML = "<strong>Completed Quantity:</strong>";
        }
    });
</script>
    </asp:Content>