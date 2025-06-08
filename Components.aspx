<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Components.aspx.cs" Inherits="PHEDChhattisgarh.Components" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component List - PHED Chhattisgarh</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <style>
        /* Lock the body height and hide overflow */
body {
    height: 100vh;
    overflow: hidden;
    font-family: Arial, sans-serif;
    margin: 0;
    padding: 0;
}

.container {
    padding: 10px; /* Reduced from 20px */
    height: calc(100vh - 160px); /* Adjusted for mobile */
    overflow-y: auto;
}

/* Mobile adjustments */
@media (max-width: 768px) {
    .container {
        padding: 5px;
        height: calc(100vh - 140px);
    }
}

/* Keep header and progress bar fixed */
.header {
    position: sticky;
    top: 0;
    z-index: 1000;
    background-color: #0066cc;
    color: white;
    padding: 10px 20px;
}

.progress-container {
    position: sticky;
    top: 80px; /* Height of header */
    z-index: 999;
    background: white;
    padding: 10px 0;
    box-shadow: 0 2px 5px rgba(0,0,0,0.1);
}
        .stepper-wrapper {
            display: flex;
            justify-content: space-between;
            position: relative;
        }

        .stepper-item {
            position: relative;
            z-index: 1;
            text-align: center;
            flex: 1;
        }

        .step-counter {
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

        .stepper-item.completed .step-counter {
            background: #0066cc;
            border-color: #0066cc;
            color: white;
        }

        .stepper-item.completed .step-counter::after {
            content: '';
            margin-left: 2px;
        }

        .stepper-wrapper::before {
            content: '';
            position: absolute;
            top: 15px;
            left: 10%;
            right: 10%;
            height: 3px;
            background: #dee2e6;
            z-index: 0;
        }

        .step-name {
            font-size: 14px;
            color: #6c757d;
            margin-top: 0px;
        }

        .form-group {
            margin-bottom: 15px;
        }
        .table th, .table td {
            padding: 8px;
        }
        .btn-primary {
            background-color: #0066cc;
        }
        .btn-primary:disabled {
            background-color: #0066cc !important; /* Original blue color */
            border-color: #0066cc !important;     /* Original border color */
            opacity: 0.65;                        /* Slight transparency to indicate disabled state */
            color: white !important;               /* Ensure text remains visible */
            cursor: not-allowed;                   /* Show "blocked" cursor */
        }

        .ticker-container {
            width: 100%;
            background-color: #f8f9fa;
            padding: 8px 0;
            border-bottom: 1px solid #dee2e6;
            overflow: hidden;
            position: relative;
            box-shadow: none;
             outline: none !important;
        }

        .ticker-text {
            white-space: nowrap;
            animation: ticker 20s linear infinite;
            color: #dc3545;
            font-weight: bold;
            position: relative;
            display: inline-block;
            padding-left: 100%;
        }

        .details-grid {
            border: 2px solid #000 !important;
        }
        .details-grid th {
            background-color: #f8f9fa; 
            border: 2px solid #000 !important;
            padding: 12px !important;
            font-weight: bold;
        }
        .details-grid td {
            border: 2px solid #dee2e6 !important;
            padding: 10px !important;
        }
.floating-button-container {
    position: fixed;
    bottom: 20px;
    right: 20px; /* Changed from left to right for better placement */
    z-index: 1000;
}

.floating-button {
    background-color: #0066cc; /* Using your primary color instead of secondary */
    color: white;
    border: none;
    border-radius: 50px; /* More rounded for a modern look */
    padding: 12px 24px;
    box-shadow: 0 4px 8px rgba(0,0,0,0.2);
    cursor: pointer;
    font-weight: bold;
    transition: all 0.3s ease;
    display: flex;
    align-items: center;
    gap: 8px;
}

.floating-button:hover {
    background-color: #004d99; /* Darker shade of your primary color */
    transform: translateY(-2px); /* Slight lift effect on hover */
    box-shadow: 0 6px 12px rgba(0,0,0,0.3);
}

/* Optional: Add an icon before the text */
.floating-button::before {
    content: "←";
    font-size: 18px;
}

/* Progress Entry Styles */
.progress-entry-panel {
    display: none;
    background: #f8f9fa;
    border: 2px solid #0066cc;
    border-radius: 8px;
    padding: 20px;
    margin: 20px 0;
}

.progress-form {
    background: white;
    padding: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 5px rgba(0,0,0,0.1);
    margin-bottom: 20px;
}

.btn-success {
    background-color: #28a745;
    border-color: #28a745;
}

.btn-success:hover {
    background-color: #218838;
    border-color: #1e7e34;
}

.btn-secondary {
    background-color: #6c757d;
    border-color: #6c757d;
}

.btn-secondary:hover {
    background-color: #5a6268;
    border-color: #545b62;
}

.component-info {
    background: #e3f2fd;
    padding: 15px;
    border-radius: 5px;
    margin-bottom: 20px;
    border-left: 4px solid #0066cc;
}
.btn-uniform {
    width: 150px !important; /* Adjust width as needed */
    text-align: center;
}
.action-buttons {
    display: flex;
    flex-direction: column;
    gap: 5px;
    align-items: center; /* Centers the buttons horizontally */
    justify-content: center; /* Centers the buttons vertically */
}

.action-buttons .btn {
    width: 140px; /* Fixed width for uniform buttons */
    min-width: 140px;
}

.actions-column {
    text-align: center !important;
    vertical-align: middle !important;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <h2>Public Health Engineering Department Govt. Of Chhattisgarh</h2>
        </div>

        <div class="progress-container">
            <div class="stepper-wrapper">
                <div class="stepper-item completed">
                    <div class="step-counter">1</div>
                    <div class="step-name">General-Abstract</div>
                </div>
                <div class="stepper-item completed">
                    <div class="step-counter">2</div>
                    <div class="step-name">Sub-Estimate</div>
                </div>
                <div class="stepper-item">
                    <div class="step-counter">3</div>
                    <div class="step-name">Components of Sub-Estimate</div>
                </div>
                <div class="stepper-item">
                    <div class="step-counter">4</div>
                    <div class="step-name">eMB Entry</div>
                </div>
            </div>
        </div>

        <div class="container">

            <!-- Work Details Panel -->
            <div class="card mt-4" id="workDetailsPanel" runat="server">
                <div class="card-header">
                    <h3><b>Details of Work</b></h3>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvWorkDetails" runat="server" 
                        CssClass="table table-bordered details-grid" 
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
            <div class="card" id="componentListPanel" runat="server">
                <div class="card-header">
                    <h3><b>Component List </b></h3>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvComponents" runat="server" CssClass="table table-bordered details-grid" AutoGenerateColumns="false"
    OnRowCommand="gvComponents_RowCommand">
    <Columns>
        <asp:BoundField DataField="ComponentID" HeaderText="CompId" ItemStyle-HorizontalAlign="Center" 
            HeaderStyle-HorizontalAlign="Center" 
            HeaderStyle-CssClass="text-center"/>
        <asp:BoundField DataField="ComponentName" HeaderText="ComponentName" ItemStyle-HorizontalAlign="Center" 
            HeaderStyle-HorizontalAlign="Center" 
            HeaderStyle-CssClass="text-center"/>
        <asp:BoundField DataField="AA_Quantity" HeaderText="AA_Quantity" ItemStyle-HorizontalAlign="Center" 
            HeaderStyle-HorizontalAlign="Center" 
            HeaderStyle-CssClass="text-center" DataFormatString="{0:N2}"/>
        <asp:BoundField DataField="RemainingQty" HeaderText="Remaining Qty" ItemStyle-HorizontalAlign="Center" 
            HeaderStyle-HorizontalAlign="Center" 
            HeaderStyle-CssClass="text-center" DataFormatString="{0:N2}"/>
        <asp:BoundField DataField="ComponentUnit" HeaderText="Unit" ItemStyle-HorizontalAlign="Center" 
            HeaderStyle-HorizontalAlign="Center" 
            HeaderStyle-CssClass="text-center"/>
        <asp:BoundField DataField="Amount" HeaderText="Amount (in lakhs)" ItemStyle-HorizontalAlign="Center" 
            HeaderStyle-HorizontalAlign="Center" 
            HeaderStyle-CssClass="text-center"/>

        <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="actions-column" HeaderStyle-CssClass="text-center">
            <ItemTemplate>
                <div class="action-buttons">
                    <asp:LinkButton ID="btnViewComponents" runat="server" CommandName="ViewComponents" 
                        CommandArgument='<%# Eval("Work_Code") + "," + Eval("AgreementBy") + "," + 
                                            Eval("Year_Of_Agreement") + "," + Eval("Agreement_No") + "," + 
                                            Eval("ComponentId") + "," + Eval("Amount") %>' 
                        CssClass="btn btn-sm btn-primary">
                        View Sub-Components
                    </asp:LinkButton>
            
                    <asp:LinkButton ID="btnEnterProgress" runat="server" CommandName="EnterProgress" 
                        CommandArgument='<%# Eval("ComponentId") + "#" + Eval("ComponentName") + "#" + 
                                            Eval("AA_Quantity") + "#" + Eval("ComponentUnit") %>' 
                        CssClass="btn btn-sm btn-success">
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
            <div class="card" id="progressEntryPanel" runat="server" style="display: none;">
                <div class="card-header">
                    <h3><b>Enter Physical Progress</b></h3>
                </div>
                <div class="card-body">
                    <!-- Component Information -->
                    <div class="component-info">
                        <div class="row">
                            <div class="col-md-3">
                                <strong>Component ID:</strong> 
                                <asp:Label ID="lblComponentId" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <strong>Component Name:</strong> 
                                <asp:Label ID="lblComponentName" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-3">
                                <strong>Quantity:</strong> 
                                <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                                <asp:Label ID="lblUnit" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- Progress Entry Form -->
                    <div class="progress-form">
                        <div class="row">
                            <div class="col-md-2">
                                <label class="form-label"><strong>Entry Type:</strong></label>
                                <asp:DropDownList ID="ddlEntryType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEntryType_SelectedIndexChanged">
                                    <asp:ListItem Value="Percentage" Text="Percentage" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="Quantity" Text="Quantity"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label id="lblInputType" runat="server" class="form-label"><strong>Progress Percentage (%):</strong></label>
                                <asp:TextBox ID="txtProgressValue" runat="server" CssClass="form-control" 
                                    placeholder="Enter percentage (0-100)" type="number" min="0" max="100" step="0.01" 
                                    AutoPostBack="true" OnTextChanged="txtProgressValue_TextChanged"></asp:TextBox>
            
                                <!-- Required Field Validator -->
                                <asp:RequiredFieldValidator ID="rfvProgressValue" runat="server" 
                                    ControlToValidate="txtProgressValue" 
                                    ErrorMessage="Value is required" 
                                    CssClass="text-danger" 
                                    ValidationGroup="ProgressEntry"></asp:RequiredFieldValidator>
            
                                <!-- Range Validator for Percentage -->
                                <asp:RangeValidator ID="rvProgressPercentage" runat="server" 
                                    ControlToValidate="txtProgressValue" 
                                    ErrorMessage="Percentage must be between 0 and 100" 
                                    CssClass="text-danger" 
                                    ValidationGroup="ProgressEntry"
                                    MinimumValue="0" 
                                    MaximumValue="100" 
                                    Type="Double"
                                    Display="Dynamic"></asp:RangeValidator>
            
                                <!-- Custom Validator for Quantity -->
                                <asp:CustomValidator ID="cvProgressValue" runat="server" 
                                    ControlToValidate="txtProgressValue" 
                                    ErrorMessage="Quantity cannot exceed total quantity" 
                                    CssClass="text-danger" 
                                    ValidationGroup="ProgressEntry"
                                    OnServerValidate="cvProgressValue_ServerValidate"
                                    Display="Dynamic"></asp:CustomValidator>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label"><strong>Calculated Value:</strong></label>
                                <asp:Label ID="lblCalculatedValue" runat="server" CssClass="form-control-plaintext text-primary font-weight-bold"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                <label class="form-label"><strong>Entry Date:</strong></label>
                                <asp:Label ID="lblEntryDate" runat="server" CssClass="form-control-plaintext"></asp:Label>
                            </div>
                            <div class="col-md-4" style="padding-top: 32px;">
                                <asp:Button ID="btnSaveProgress" runat="server" Text="Save Progress" 
                                    CssClass="btn btn-success" OnClick="btnSaveProgress_Click" 
                                    ValidationGroup="ProgressEntry" />
                                <asp:Button ID="btnBackToList" runat="server" Text="Back" 
                                    CssClass="btn btn-secondary" OnClick="btnBackToList_Click" 
                                    style="margin-left: 10px;" />
                            </div>
                        </div>
                    </div>
                    <!-- CSEB Survey Panel - Only for Component ID 26 -->
                    <div class="card" id="csebSurveyPanel" runat="server" style="display: none; margin-bottom: 20px;">
                        <div class="card-header" style="background-color: #fff3cd; border-color: #ffeaa7;">
                            <h5><b>🔌 CSEB Connection Survey</b></h5>
                        </div>
                        <div class="card-body" style="background-color: #fffbf0;">
                            <div class="row">
                                <div class="col-md-6">
                                    <label class="form-label"><strong>Status of CSEB Connection:</strong></label>
                                    <asp:DropDownList ID="ddlCSEBStatus" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="-- Select Status --" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Provided" Text="1. Provided"></asp:ListItem>
                                        <asp:ListItem Value="Pending with Contractor" Text="2. Pending with Contractor"></asp:ListItem>
                                        <asp:ListItem Value="Pending with CSEB" Text="3. Pending with CSEB"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCSEBStatus" runat="server" 
                                        ControlToValidate="ddlCSEBStatus" 
                                        ErrorMessage="Please select CSEB connection status" 
                                        CssClass="text-danger" 
                                        ValidationGroup="CSEBSurvey"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3" style="padding-top: 32px;">
                                    <asp:Button ID="btnSaveCSEBSurvey" runat="server" Text="Save Survey" 
                                        CssClass="btn btn-warning" OnClick="btnSaveCSEBSurvey_Click" 
                                        ValidationGroup="CSEBSurvey" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Source Availability Survey Panel - Only for Component ID 2 and 3 -->
                    <div class="card" id="sourceSurveyPanel" runat="server" style="display: none; margin-bottom: 20px;">
                        <div class="card-header" style="background-color: #d1ecf1; border-color: #bee5eb;">
                            <h5><b>💧 Source Availability Survey</b></h5>
                        </div>
                        <div class="card-body" style="background-color: #f0f8ff;">
                            <div class="row">
                                <div class="col-md-6">
                                    <label class="form-label"><strong>The source with sufficient yield is available for scheme:</strong></label>
                                    <asp:DropDownList ID="ddlSourceAvailable" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="-- Select --" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvSourceAvailable" runat="server" 
                                        ControlToValidate="ddlSourceAvailable" 
                                        ErrorMessage="Please select source availability status" 
                                        CssClass="text-danger" 
                                        ValidationGroup="SourceSurvey"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3" style="padding-top: 32px;">
                                    <asp:Button ID="btnSaveSourceSurvey" runat="server" Text="Save Survey" 
                                        CssClass="btn btn-info" OnClick="btnSaveSourceSurvey_Click" 
                                        ValidationGroup="SourceSurvey" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Previous Progress Entries -->
                    <div class="card">
                        <div class="card-header">
                            <h5><b>Previous Progress Entries</b></h5>
                        </div>
                        <div class="card-body">
                            <asp:GridView ID="gvProgressHistory" runat="server" 
                                CssClass="table table-bordered table-striped" 
                                AutoGenerateColumns="false"
                                EmptyDataText="No previous progress entries found.">
                                <Columns>
                                    <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" 
                                        DataFormatString="{0:dd/MM/yyyy HH:mm}" 
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Percentage" HeaderText="Progress (%)" 
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
        
        <div class="floating-button-container">
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-secondary floating-button" OnClick="btnPrevious_Click" />
        </div>
    </form>
</body>
</html>