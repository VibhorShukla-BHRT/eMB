<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComponentList.aspx.cs" Inherits="PHEDChhattisgarh.ComponentList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component List - PHED Chhattisgarh</title>
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
        .form-group {
            margin-bottom: 15px;
        }
        .table th, .table td {
            padding: 8px;
        }
        .btn-primary {
            background-color: #0066cc;
        }
        .required {
            background-color: #e6f2ff;
        }
        .error-message {
            color: #ff0000;
            margin-bottom: 15px;
            padding: 10px;
            background-color: #ffebee;
            border-radius: 4px;
            border: 1px solid #ffcdd2;
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
        @keyframes ticker {
            0% {
                transform: translateX(0);
            }
            100% {
                transform: translateX(-100%);
            }
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
                <div class="stepper-item completed">
                    <div class="step-counter">3</div>
                    <div class="step-name">Component of Sub-Estimate</div>
                </div>
                <div class="stepper-item">
                    <div class="step-counter">4</div>
                    <div class="step-name">eMB Entry</div>
                </div>
            </div>
        </div>

        <div class="container">
            <!-- Error Message (Hidden by default) -->
            <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>


            <!-- Agreement Details Grid -->
            <div class="card mb-4">
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
                    <asp:BoundField DataField="WorkName" HeaderText="Name Of Work" ItemStyle-HorizontalAlign="Center" 
                HeaderStyle-HorizontalAlign="Center" 
                HeaderStyle-CssClass="text-center"/>
                    <asp:BoundField DataField="ComponentName" HeaderText="Component Name" ItemStyle-HorizontalAlign="Center" 
                HeaderStyle-HorizontalAlign="Center" 
                HeaderStyle-CssClass="text-center"/>
                    <asp:BoundField DataField="ComponentAmount" HeaderText="Component Amount (In Lakhs)" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Center" 
                HeaderStyle-HorizontalAlign="Center" 
                HeaderStyle-CssClass="text-center"/>
                </Columns>
            </asp:GridView>
                </div>
            </div>
            
            <!-- Sub-Components Section (Hidden by default, shown when a component is selected) -->
            <div id="divSubComponents" runat="server" class="card mt-4">
                <div class="card-header">
                    <h3>
                        <asp:Label ID="lblSORItem" runat="server" Text="">
                        </asp:Label> <b>SOR Sub-Item Grid</b>
                    </h3>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvSubComponents" runat="server" 
                        AutoGenerateColumns="False" 
                        DataKeyNames="SORItemNo, SORItem, SORFrom, SORSubItem, ActualUnit, Qty, AmountWithGST, BasicorAmendment"
                        OnRowCommand="gvSubComponents_RowCommand" 
                        CssClass="table table-bordered">
                        <Columns>
                            <asp:BoundField DataField="SORItemNo" HeaderText="SOR Sub-Item No." />
                            <asp:BoundField DataField="SORFrom" HeaderText="SOR From" />
                            <asp:BoundField DataField="BasicorAmendment"
                                HeaderText="Basic or Amendment"
                                NullDisplayText="Basic" />
                            <asp:BoundField DataField="SORItem" HeaderText="SOR Item" />
                            <asp:BoundField DataField="SORSubItem" HeaderText="SOR Sub-Item" />
                            <asp:BoundField DataField="Qty" HeaderText="Qty" />
                            <asp:BoundField DataField="ActualUnit" HeaderText="Unit" />
                            <asp:BoundField DataField="AmountWithGST" HeaderText="Amount (with GST)" />
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEnter" runat="server"
                                      CommandName="EntereMB"
                                      CommandArgument='<%# Container.DataItemIndex %>'
                                      CssClass="btn btn-sm btn-primary">
                                      Enter eMB
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="floating-button-container">
                <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-secondary floating-button" OnClick="btnPrevious_Click" />
            </div>
        </div>
    </form>

    <script src="Scripts/jquery-3.6.0.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>