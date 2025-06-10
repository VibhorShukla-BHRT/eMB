<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="AgreementList.aspx.cs" Inherits="PHEDChhattisgarh.AgreementList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style>

/* Main Container */
.agreement-main-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

/* Header Section */
.agreement-page-header {
    text-align: center;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    padding: 20px;
    border-radius: 10px;
    margin-bottom: 30px;
    box-shadow: 0 4px 15px rgba(0,0,0,0.1);
}

.agreement-page-header h2 {
    margin: 0;
    font-size: 1.8rem;
    font-weight: 600;
    text-shadow: 0 2px 4px rgba(0,0,0,0.3);
}

/* Progress Stepper Container */
.agreement-progress-container {
    margin-bottom: 30px;
    padding: 20px;
    background: #f8f9fa;
    border-radius: 10px;
    border: 1px solid #e9ecef;
}

.agreement-stepper-wrapper {
    display: flex;
    justify-content: space-between;
    align-items: center;
    position: relative;
}

.agreement-stepper-wrapper::before {
    content: '';
    position: absolute;
    top: 25px;
    left: 50px;
    right: 50px;
    height: 2px;
    background: #dee2e6;
    z-index: 1;
}

.agreement-stepper-item {
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    z-index: 2;
    background: #f8f9fa;
    padding: 0 15px;
}

.agreement-step-counter {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    background: #6c757d;
    color: white;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
    font-size: 1.1rem;
    margin-bottom: 10px;
    border: 3px solid #f8f9fa;
    transition: all 0.3s ease;
}

.agreement-stepper-item.agreement-completed .agreement-step-counter {
    background: #28a745;
    box-shadow: 0 0 0 3px rgba(40, 167, 69, 0.2);
}

.agreement-step-name {
    font-size: 0.9rem;
    text-align: center;
    font-weight: 500;
    color: #495057;
    max-width: 120px;
    line-height: 1.3;
}

.agreement-stepper-item.agreement-completed .agreement-step-name {
    color: #28a745;
    font-weight: 600;
}

/* Ticker/Alert Container */
.agreement-ticker-container {
    background: #fff3cd;
    border: 1px solid #ffeaa7;
    border-radius: 8px;
    padding: 15px;
    margin-bottom: 25px;
    overflow: hidden;
    position: relative;
}

.agreement-ticker-text {
    animation: agreementTickerScroll 30s linear infinite;
    white-space: nowrap;
    color: #856404;
    font-weight: 500;
    font-size: 1rem;
}

@keyframes agreementTickerScroll {
    0% { transform: translateX(100%); }
    100% { transform: translateX(-100%); }
}

/* Search Content Sections */
.agreement-search-content {
    display: none;
}

.agreement-search-content.agreement-active {
    display: block;
}

/* Card Styles */
.agreement-card {
    background: white;
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.08);
    border: 1px solid #e9ecef;
    margin-bottom: 25px;
    overflow: hidden;
}

.agreement-card-header {
    background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
    padding: 20px;
    border-bottom: 1px solid #e9ecef;
}

.agreement-card-header h3 {
    margin: 0;
    color: #495057;
    font-size: 1.3rem;
    font-weight: 600;
}

.agreement-card-header small {
    display: block;
    margin-top: 5px;
    color: #6c757d;
    font-size: 0.9rem;
}

.agreement-card-body {
    padding: 25px;
}

/* Form Groups and Controls */
.agreement-form-group {
    margin-bottom: 20px;
}

.agreement-form-group label {
    display: block;
    margin-bottom: 8px;
    font-weight: 600;
    color: #495057;
    font-size: 0.95rem;
}

.agreement-form-control {
    width: 100%;
    padding: 12px 15px;
    border: 2px solid #e9ecef;
    border-radius: 8px;
    font-size: 1rem;
    transition: all 0.3s ease;
    background: white;
}

.agreement-form-control:focus {
    outline: none;
    border-color: #667eea;
    box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.agreement-form-control::placeholder {
    color: #adb5bd;
    font-style: italic;
}

/* Autocomplete Container */
.agreement-autocomplete-container {
    position: relative;
}

.agreement-autocomplete-suggestions {
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    background: white;
    border: 2px solid #e9ecef;
    border-top: none;
    border-radius: 0 0 8px 8px;
    max-height: 200px;
    overflow-y: auto;
    z-index: 1000;
    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
}

.agreement-autocomplete-suggestions div {
    padding: 12px 15px;
    cursor: pointer;
    transition: background-color 0.2s ease;
}

.agreement-autocomplete-suggestions div:hover {
    background: #f8f9fa;
}

/* Button Styles */
.agreement-btn {
    padding: 12px 25px;
    border: none;
    border-radius: 8px;
    font-size: 1rem;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s ease;
    text-decoration: none;
    display: inline-block;
    text-align: center;
    line-height: 1;
}

.agreement-btn-primary {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3);
}

.agreement-btn-primary:hover {
    transform: translateY(-2px);
    box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
}

.agreement-btn-sm {
    padding: 8px 16px;
    font-size: 0.9rem;
}

/* Grid/Table Styles */
.agreement-table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 15px;
    background: white;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 2px 8px rgba(0,0,0,0.05);
}

.agreement-table th {
    background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
    padding: 15px;
    text-align: left;
    font-weight: 600;
    color: #495057;
    border-bottom: 2px solid #dee2e6;
}

.agreement-table td {
    padding: 15px;
    border-bottom: 1px solid #f1f3f4;
    vertical-align: middle;
}

.agreement-table tr:hover {
    background: #f8f9fa;
}

.agreement-table-striped tr:nth-child(even) {
    background: #f8f9fa;
}

.agreement-table-bordered {
    border: 1px solid #dee2e6;
}

.agreement-table-bordered th,
.agreement-table-bordered td {
    border: 1px solid #dee2e6;
}

.agreement-details-grid {
    font-size: 0.95rem;
}

/* Text Utilities */
.agreement-text-muted {
    color: #6c757d !important;
    font-size: 0.9rem;
}

/* Spacing Utilities */
.agreement-mb-4 {
    margin-bottom: 2rem !important;
}

.agreement-mt-3 {
    margin-top: 1.5rem !important;
}

/* Floating Button */
.agreement-floating-button-container {
    position: fixed;
    bottom: 30px;
    right: 30px;
    z-index: 1000;
}

.agreement-floating-button {
    background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
    color: white;
    padding: 15px 25px;
    border-radius: 50px;
    border: none;
    font-weight: 600;
    box-shadow: 0 4px 20px rgba(40, 167, 69, 0.3);
    transition: all 0.3s ease;
    cursor: pointer;
}

.agreement-floating-button:hover {
    transform: translateY(-3px);
    box-shadow: 0 6px 25px rgba(40, 167, 69, 0.4);
}

/* Responsive Grid System */
.agreement-row {
    display: flex;
    flex-wrap: wrap;
    margin: 0 -10px;
}

.agreement-col-md-2 {
    flex: 0 0 16.666667%;
    max-width: 16.666667%;
    padding: 0 10px;
}

.agreement-col-md-3 {
    flex: 0 0 25%;
    max-width: 25%;
    padding: 0 10px;
}

.agreement-col-md-4 {
    flex: 0 0 33.333333%;
    max-width: 33.333333%;
    padding: 0 10px;
}

/* Responsive Design */
@media (max-width: 768px) {
    .agreement-main-container {
        padding: 15px;
    }
    
    .agreement-stepper-wrapper {
        flex-direction: column;
        gap: 20px;
    }
    
    .agreement-stepper-wrapper::before {
        display: none;
    }
    
    .agreement-row {
        flex-direction: column;
    }
    
    .agreement-col-md-2,
    .agreement-col-md-3,
    .agreement-col-md-4 {
        flex: 0 0 100%;
        max-width: 100%;
        margin-bottom: 15px;
    }
    
    .agreement-page-header h2 {
        font-size: 1.5rem;
    }
    
    .agreement-floating-button-container {
        bottom: 20px;
        right: 20px;
    }
    
    .agreement-table {
        font-size: 0.85rem;
    }
    
    .agreement-table th,
    .agreement-table td {
        padding: 10px 8px;
    }
}

@media (max-width: 480px) {
    .agreement-card-body {
        padding: 15px;
    }
    
    .agreement-card-header {
        padding: 15px;
    }
    
    .agreement-ticker-text {
        font-size: 0.9rem;
    }
    
    .agreement-form-control {
        padding: 10px 12px;
    }
}
    </style>
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
    <div class="agreement-main-container">
        
        <div class="agreement-page-header">
            <h2>Public Health Engineering Department Govt. Of Chhattisgarh</h2>
        </div>
        
        <div class="agreement-progress-container">
            <div class="agreement-stepper-wrapper">
                <div class="agreement-stepper-item agreement-completed" id="step1">
                    <div class="agreement-step-counter">1</div>
                    <div class="agreement-step-name">General-Abstract</div>
                </div>
                <div class="agreement-stepper-item" id="step2">
                    <div class="agreement-step-counter">2</div>
                    <div class="agreement-step-name">Sub-Estimate</div>
                </div>
                <div class="agreement-stepper-item" id="step3">
                    <div class="agreement-step-counter">3</div>
                    <div class="agreement-step-name">Component of Sub-Estimate</div>
                </div>
                <div class="agreement-stepper-item">
                    <div class="agreement-step-counter">4</div>
                    <div class="agreement-step-name">eMB Entry</div>
                </div>
            </div>
        </div>

        <div class="agreement-ticker-container">
            <div class="agreement-ticker-text">
                ⚠️ Only certified Work Codes are available for Search. ⚠️&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                ⚠️ केवल प्रमाणित Work कोड ही चयन के लिए उपलब्ध होंगे। ⚠️
            </div>
        </div>

        <!-- Direct Search Form (Faster Method) -->
        <div id="directSearch" class="agreement-search-content agreement-active">
            <div class="agreement-card agreement-mb-4">
                <div class="agreement-card-header">
                    <h3><b>Direct Search (Type Book No & Work Code)</b></h3>
                </div>
                <div class="agreement-card-body">
                    <div class="agreement-row">
                        <div class="agreement-col-md-4">
                            <div class="agreement-form-group agreement-autocomplete-container">
                                <label>eMB Book No:</label>
                                <asp:TextBox 
                                    ID="txtEMBBookNo" 
                                    runat="server" 
                                    CssClass="agreement-form-control" 
                                    placeholder="Type to search..."
                                    onkeyup="searchEMBBookNo(this.value)" />
                                <div id="bookNoSuggestions" class="agreement-autocomplete-suggestions"></div>
                            </div>
                        </div>
                        <div class="agreement-col-md-4">
                            <div class="agreement-form-group">
                                <label>Work Code (Optional):</label>
                                <asp:TextBox 
                                    ID="txtWorkCode" 
                                    runat="server" 
                                    CssClass="agreement-form-control" 
                                    placeholder="Enter Work Code or leave empty" />
                                <small class="agreement-text-muted">Leave empty to see all work codes for the book</small>
                            </div>
                        </div>
                        <div class="agreement-col-md-2">
                            <div class="agreement-form-group">
                                <label>&nbsp;</label>
                                <asp:Button
                                    ID="btnDirectSearch"
                                    runat="server"
                                    Text="Search"
                                    CssClass="agreement-btn agreement-btn-primary agreement-form-control"
                                    OnClick="btnDirectSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Dropdown Search Form (Original Method) -->
        <div id="dropdownSearch" class="agreement-search-content" visible="false">
            <div class="agreement-card agreement-mb-4">
                <div class="agreement-card-header">
                    <h3><b>Dropdown Search</b></h3>
                    <small class="agreement-text-muted">Traditional method - may be slower for large datasets</small>
                </div>
                <div class="agreement-card-body">
                    <div class="agreement-row">
                        <div class="agreement-col-md-3">
                            <div class="agreement-form-group">
                                <label>eMB Book No:</label>
                                <asp:DropDownList
                                    ID="ddlEMBBookNo"
                                    runat="server"
                                    CssClass="agreement-form-control"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlEMBBookNo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="agreement-col-md-3">
                            <div class="agreement-form-group">
                                <label>Work Code:</label>
                                <asp:DropDownList
                                    ID="ddlWorkCode"
                                    runat="server"
                                    CssClass="agreement-form-control"
                                    Enabled="false">
                                    <asp:ListItem Text="-- Select Book No First --" Value="" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="agreement-col-md-2">
                            <div class="agreement-form-group">
                                <label>&nbsp;</label>
                                <asp:Button
                                    ID="btnSearch"
                                    runat="server"
                                    Text="Search"
                                    CssClass="agreement-btn agreement-btn-primary agreement-form-control"
                                    OnClick="btnSearch_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Work Codes List Panel (for when book is entered but work code is not) -->
        <asp:Panel ID="pnlWorkCodes" runat="server" Visible="false" CssClass="agreement-card agreement-mb-4">
            <div class="agreement-card-header">
                <h3><b>Select Work Code</b></h3>
            </div>
            <div class="agreement-card-body">
                <asp:GridView
                    ID="gvWorkCodes"
                    runat="server"
                    AutoGenerateColumns="false"
                    CssClass="agreement-table agreement-table-striped"
                    OnRowCommand="gvWorkCodes_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="WorkCode" HeaderText="Work Code" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button
                                    runat="server"
                                    Text="Select"
                                    CssClass="agreement-btn agreement-btn-sm agreement-btn-primary"
                                    CommandName="Select"
                                    CommandArgument='<%# Eval("WorkCode") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>

        <!-- Results -->
        <asp:Panel ID="pnlResults" runat="server" Visible="false">
            <div class="agreement-card agreement-mb-4">
                <div class="agreement-card-header">
                    <h3><b>Details of Work</b></h3>
                </div>
                <div class="agreement-card-body">
                    <asp:GridView
                        ID="gvAgreements"
                        runat="server"
                        AutoGenerateColumns="false"
                        DataKeyNames="Work_Code,AgreementBy,Year_Of_Agreement,Agreement_No"
                        OnRowCommand="gvAgreements_RowCommand"
                        CssClass="agreement-table agreement-table-bordered agreement-details-grid"
                        GridLines="Both"
                        EmptyDataText="No records found. Please search first.">
                        <Columns>
                            <asp:BoundField DataField="Year_Of_Agreement" HeaderText="Year" />
                            <asp:BoundField DataField="AgreementByName" HeaderText="Agreement By" />
                            <asp:BoundField DataField="Agreement_No" HeaderText="Agreement No" />
                            <asp:BoundField DataField="Work_Code" HeaderText="Work Code" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="agreement-row agreement-mt-3">
                    <div class="agreement-col-md-3">
                        <asp:Button
                            ID="btnViewComponents"
                            runat="server"
                            Text="View Components"
                            OnClick="btnViewComponents_Click"
                            CssClass="agreement-btn agreement-btn-primary agreement-form-control" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        
    </div>
    
    <div class="agreement-floating-button-container">
        <asp:Button ID="btnPrevious" runat="server" Text="Home" CssClass="agreement-floating-button" OnClick="btnPrevious_Click" />
    </div>
</asp:Content>