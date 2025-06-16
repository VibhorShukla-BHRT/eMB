<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="eMBEntry.aspx.cs" Inherits="PHEDChhattisgarh.eMBEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <style>
        .progress-container {
            padding: 10px 0px;
            max-width: 100%;
            margin: 0 auto;
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

        .work-occupied {
            background-color: #eaeaea;
        }

        .work-occupied thead {
            background-color: #ffcccc;
        }

        body {
            height: 100vh;
            overflow: hidden;
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
        }

/* Make the container scrollable */
.container {
    padding: 20px;
    height: calc(100vh - 200px); /* Adjust based on header height */
    overflow-y: auto;
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
        }
        .form-group {
            margin-bottom: 15px;
        }
        .btn-primary {
            background-color: #0066cc;
        }
        .input-error {
            border: 1px solid red;
        }
        .auto-size-label {
            min-height: 38px; /* Standard form-control height */
            height: auto; /* Will expand as needed */
            width: 100%; /* Full width of parent */
            white-space: normal; /* Allows text wrapping */
            word-wrap: break-word; /* Breaks long words */
            padding: 8px 12px; /* Consistent padding */
            line-height: 1.5; /* Better readability */
            overflow: visible; /* No scrollbars */
        }
                /* Add to your existing styles */
        #gvMaterialDetails {
            width: 100%;
            border: none !important;
        }

        #gvMaterialDetails td {
            border: none !important;
            padding: 8px 12px;
        }

        #gvMaterialDetails td:first-child,
        #gvMaterialDetails td:nth-child(3) {
            font-weight: bold;
            background-color: #f8f9fa;
        }
                .table th, .table td {
                    padding: 8px;
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
        .details-grid td {
            white-space: normal !important;
            word-wrap: break-word;
        }

        /* Specific style for wide columns */
        .wide-column {
            max-width: 200px; 
            white-space: nowrap;
        }
        .inputs-col  {
            max-width: 150px;
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
        @media (min-width: 992px) {
            .col-md-4 .auto-size-label {
                width: 150%; /* Extends beyond the column boundaries */
                position: relative;
                z-index: 1; /* Ensures it appears above other content */
            }
        }

        /* Slim down the card’s margins */
        .card.mb-4 {
            margin-bottom: 1rem; /* was 1.5rem */
        }
        /* Very tight table cells */
        .details-grid td,
        .details-grid th {
            padding: 0.3rem 0.5rem;
            vertical-align: middle;
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
                <div class="stepper-item completed">
                    <div class="step-counter">3</div>
                    <div class="step-name">Component of Sub-Estimate</div>
                </div>
                <div class="stepper-item completed">
                    <div class="step-counter">4</div>
                    <div class="step-name">eMB Entry</div>
                </div>
            </div>
        </div>

        <div class="container">
            <!-- Work Details Grid -->
            <div class="card mb-4">

                <div class="card-header py-2">
                    <h3><strong>Details of Work</strong></h3>
                </div>
                <div class="card-body p-2">
                    <asp:Panel ID="pnlWorkDetails" runat="server" CssClass="work-details work-occupied">
                        <asp:GridView ID="gvWorkDetails" runat="server" 
                            CssClass="table table-bordered table-sm details-grid" 
                            AutoGenerateColumns="false"
                            GridLines="Both">
                            <Columns>
                                <asp:BoundField 
                                    DataField="Work_Code" 
                                    HeaderText="Work Code" 
                                    ItemStyle-HorizontalAlign="Center" 
                                    HeaderStyle-HorizontalAlign="Center" 
                                    HeaderStyle-CssClass="text-center" />

                                <asp:BoundField 
                                    DataField="WorkName" 
                                    HeaderText="Name Of Work" 
                                    ItemStyle-HorizontalAlign="Center" 
                                    HeaderStyle-HorizontalAlign="Center" 
                                    HeaderStyle-CssClass="text-center" />

                                <asp:BoundField 
                                    DataField="ComponentName" 
                                    HeaderText="Component" 
                                    ItemStyle-HorizontalAlign="Center" 
                                    HeaderStyle-HorizontalAlign="Center" 
                                    HeaderStyle-CssClass="text-center" />

                                <asp:BoundField 
                                    DataField="SORItem" 
                                    HeaderText="SOR Item" 
                                    ItemStyle-HorizontalAlign="Center" 
                                    HeaderStyle-HorizontalAlign="Center" 
                                    HeaderStyle-CssClass="text-center" />

                                <asp:BoundField 
                                    DataField="Qty" 
                                    HeaderText="Quantity" 
                                    ItemStyle-HorizontalAlign="Center" 
                                    HeaderStyle-HorizontalAlign="Center" 
                                    HeaderStyle-CssClass="text-center" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <div class="form-group row">
                      <label class="col-sm-2 col-form-label">
                          SOR Sub-Item:
                      </label>
                      <div class="col-sm-12">
                        <asp:Label ID="lblSORItem" runat="server" CssClass="form-control-plaintext" />
                      </div>
                    </div>
                </div>
            </div>
            <!-- Measurement Entry Section -->
            <div class="card mb-4">
                <div class="card-header">
                    <h3><b>Measurement Entry</b></h3>
                </div>
               
                     
                <div class="card-body">
                    <asp:HiddenField ID="hdnEditEmbId" runat="server" Value="" />
                    <asp:HiddenField ID="hdnSORItemNo" runat="server" />
                    <asp:HiddenField ID="hdnSORItem" runat="server" />
                    <asp:HiddenField ID="hdnParticulars" runat="server" />
                    <asp:HiddenField ID="hdnUnits" runat="server" />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="emb-id">
                                <label><strong>Unique EMB ID:</strong></label>
                                <asp:Label ID="lblUniqueEmbID" runat="server" Text="" CssClass="form-control-plaintext" />
                                <asp:HiddenField ID="hdnUniqueEmbID" runat="server" />
                            </div>
                        </div>
                    </div>

                    <asp:UpdatePanel ID="updFormulaPanel" runat="server" UpdateMode="Conditional">
                      <ContentTemplate>

                        <asp:SqlDataSource
                          ID="dsFormulas"
                          runat="server"
                          ConnectionString="<%$ ConnectionStrings:eMB %>"
                          SelectCommand="SELECT formula_id, name FROM Formula ORDER BY name">
                        </asp:SqlDataSource>

                        <div class="form-group">
                          <asp:Label runat="server" AssociatedControlID="ddlFormula" Text="Choose Formula:" CssClass="control-label" />
                          <asp:DropDownList
                            ID="ddlFormula"
                            runat="server"
                            DataSourceID="dsFormulas"
                            DataTextField="name"
                            DataValueField="formula_id"
                            AppendDataBoundItems="true"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlFormula_SelectedIndexChanged"
                            CssClass="form-control">
                          </asp:DropDownList>
                        </div>

                        <table class="table table-bordered" style="max-width:600px;">
                          <thead class="thead-light">
                            <tr>
                              <th style="width:40%;">Parameter</th>
                              <th style="width:60%;">Value</th>
                            </tr>
                          </thead>
                          <tbody id="tblParams" runat="server">
                          </tbody>
                          <tfoot>
                            <tr class="table-secondary">
                              <td><strong>Calculated Value</strong></td>
                              <td>
                                <asp:TextBox ID="txtResult" runat="server"
                                             CssClass="form-control"
                                             ReadOnly="true"
                                             Style="background:#e9ecef;" />
                              </td>
                            </tr>
                          </tfoot>
                        </table>

                        <div class="form-group">
                          <asp:Label runat="server" AssociatedControlID="ddlUnit" Text="Unit:" CssClass="control-label" />
                          <asp:DropDownList
                            ID="ddlUnit"
                            runat="server"
                            CssClass="form-control">
                            <asp:ListItem Text="-- Select Unit --" Value="" />
                          </asp:DropDownList>
                        </div>

                      </ContentTemplate>
                      <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlFormula" EventName="SelectedIndexChanged" />
                      </Triggers>
                    </asp:UpdatePanel>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Remarks:</label>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row mt-2">
                        <div class="col-md-12">
                            <h6>Current Entries</h6>
                            <asp:GridView ID="gvEntries" runat="server" CssClass="table table-bordered details-grid" AutoGenerateColumns="false" 
                                OnRowCommand="gvEntries_RowCommand" DataKeyNames="EmbId, IsCurrent"
                                Width="100%" CellPadding="5" HeaderStyle-CssClass="table-header">
                                <Columns>
                                    <asp:BoundField DataField="EmbId" HeaderText="ID" 
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" 
                                        HeaderStyle-CssClass="text-center" />

                                    <asp:BoundField DataField="SORItemNo" HeaderText="Sub-Item No" 
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" 
                                        HeaderStyle-CssClass="text-center"/>

                                    <asp:BoundField DataField="UniqueEmbID" HeaderText="Unique EMB ID" 
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Left" 
                                        HeaderStyle-CssClass="text-center" />
            

                                    <asp:TemplateField HeaderText="Inputs">
                                      <ItemTemplate>
                                        <%# FormatInputs(Eval("Inputs").ToString()) %>
                                      </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="ActualUnit" HeaderText="Unit" 
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" 
                                        HeaderStyle-CssClass="text-center" />

                                    <asp:BoundField DataField="ResultValue" HeaderText="Value"
                                        ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" 
                                        HeaderStyle-CssClass="text-center" />


                                    <asp:BoundField DataField="Remark" HeaderText="Remarks" ItemStyle-HorizontalAlign="Center" 
                                        HeaderStyle-HorizontalAlign="Center" 
                                        HeaderStyle-CssClass="text-center"/>
                                    
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <%# ExtractDateFromEmbID(Eval("UniqueEmbID").ToString()) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Time">
                                        <ItemTemplate>
                                            <%# ExtractTimeFromEmbID(Eval("UniqueEmbID").ToString()) %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditItem" CommandArgument='<%# Eval("EmbId") %>' 
                                                CssClass="btn btn-sm btn-primary" Text="Edit" />
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("EmbId") %>' 
                                                CssClass="btn btn-sm btn-danger" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                   <div class="row mt-3">
                        <div class="col-md-12">
                            <asp:Button ID="btnSave" runat="server" Text="Save Entry" CssClass="btn btn-primary" OnClick="btnSave_Click" OnClientClick="return validateForm();" />
                            <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
                            <div class="floating-button-container">
    <asp:Button ID="btnBackToComponentList" runat="server" Text="Back to Sub-Components" CssClass="floating-button" OnClick="btnBackToComponentList_Click" />
</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <script src="Scripts/jquery-3.6.0.min.js"></script>
        <script src="Scripts/bootstrap.min.js"></script>

        <script type="text/javascript">
            // Function to validate numeric input (allows numbers and one decimal point)
            function validateNumericInput(event) {
                // Allow: backspace, delete, tab, escape, enter and decimal point
                var key = event.keyCode || event.which;

                // Allow: backspace, delete, tab, escape, enter
                if (key == 8 || key == 46 || key == 9 || key == 27 || key == 13 ||
                    // Allow: Ctrl+A, Ctrl+C, Ctrl+V, Ctrl+X
                    ((key == 65 || key == 67 || key == 86 || key == 88) && (event.ctrlKey === true || event.metaKey === true)) ||
                    // Allow: home, end, left, right
                    (key >= 35 && key <= 39)) {
                    // let it happen, don't do anything
                    return true;
                }

                // Allow decimal point (.)
                if (key == 46 || key == 190) {
                    // Check if there's already a decimal point in the input
                    if (event.target.value.indexOf('.') !== -1) {
                        return false;
                    }
                    return true;
                }

                // Allow minus sign (-)
                if (key == 45 || key == 189) {
                    // Allow minus sign only at the beginning
                    if (event.target.value.length === 0) {
                        return true;
                    }
                    return false;
                }

                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (key < 48 || key > 57) && (key < 96 || key > 105)) {
                    event.preventDefault();
                    return false;
                }
                return true;
            }

            // Function to validate pasted content
            function validatePaste(event) {
                var clipboardData = event.clipboardData || window.clipboardData;
                var pastedData = clipboardData.getData('Text');

                // Check if pasted data is a valid number with up to one decimal point
                var regex = /^-?\d*\.?\d*$/;
                if (!regex.test(pastedData)) {
                    event.preventDefault();
                    return false;
                }
                return true;
            }

            // Function to validate input in real-time
            function validateInput(element) {
                var value = element.value;
                var regex = /^-?\d*\.?\d*$/;

                if (!regex.test(value)) {
                    // Remove invalid characters
                    element.value = value.replace(/[^\d.-]/g, '');

                    // Ensure only one decimal point
                    var parts = element.value.split('.');
                    if (parts.length > 2) {
                        element.value = parts[0] + '.' + parts.slice(1).join('');
                    }

                    // Ensure minus sign is only at the beginning
                    if (element.value.indexOf('-') > 0) {
                        element.value = element.value.replace(/-/g, '');
                        if (element.value.charAt(0) !== '-') {
                            element.value = '-' + element.value;
                        }
                    }
                }
            }
        </script>
        <script type="text/javascript">
            function recalculateFormula() {
                const expr = window.formulaExpr;
                if (!expr) return;

                let evalExpr = expr;
                let allValid = true;

                // Replace variables with values from textboxes
                document.querySelectorAll('[data-param]').forEach(input => {
                    const param = input.getAttribute('data-param');
                    let val;

                    if (input.tagName === 'SELECT') {
                        val = input.value;
                    } else {
                        val = input.value.trim();
                    }

                    if (!/^-?\d*\.?\d*$/.test(val)) {
                        allValid = false;
                        return;
                    }

                    const regex = new RegExp("\\b" + param + "\\b", "g");
                    evalExpr = evalExpr.replace(regex, val);
                });

                if (!allValid) {
                    document.getElementById('<%= txtResult.ClientID %>').value = "";
            return;
        }

        evalExpr = evalExpr.replace(/\bpi\b/g, Math.PI.toString());
        evalExpr = evalExpr.replace(/(\d+(\.\d+)?)\s*\^\s*2/g, (m, x) => `(${x}*${x})`);

        try {
            const result = eval(evalExpr);
            if (!isNaN(result)) {
                const resultElement = document.getElementById('<%= txtResult.ClientID %>');
                resultElement.value = parseFloat(result).toFixed(6).replace(/\.?0+$/, '');
            }
        } catch (e) {
            document.getElementById('<%= txtResult.ClientID %>').value = "";
                }
            }

            // Initialize event listeners when page loads
            document.addEventListener('DOMContentLoaded', function () {
                // Listen for changes on all parameter inputs
                document.querySelectorAll('[data-param]').forEach(input => {
                    if (input.tagName === 'SELECT') {
                        input.addEventListener('change', recalculateFormula);
                    } else {
                        input.addEventListener('input', recalculateFormula);
                    }
                });
            });
</script>
    </asp:Content>