<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgreementList.aspx.cs" Inherits="PHEDChhattisgarh.AgreementList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agreement List - PHED Chhattisgarh</title>
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
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
        }
        .header {
            background-color: #0066cc;
            color: white;
            padding: 10px 20px;
        }
        .container {
            padding-left: 20px;
            padding-right: 20px;
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
            background-color: #0066cc !important;
            border-color: #0066cc !important;
            opacity: 0.65;
            color: white !important;
            cursor: not-allowed;
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

        /* Autocomplete styles */
        .autocomplete-container {
            position: relative;
        }
        .autocomplete-suggestions {
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            background: white;
            border: 1px solid #ccc;
            border-top: none;
            max-height: 200px;
            overflow-y: auto;
            z-index: 1000;
            display: none;
        }
        .autocomplete-suggestion {
            padding: 10px;
            cursor: pointer;
            border-bottom: 1px solid #eee;
        }
        .autocomplete-suggestion:hover,
        .autocomplete-suggestion.selected {
            background-color: #f0f0f0;
        }

        /* Search method tabs */
        .search-tabs {
            margin-bottom: 20px;
        }
        .search-tab {
            display: inline-block;
            padding: 10px 20px;
            background: #f8f9fa;
            border: 1px solid #dee2e6;
            cursor: pointer;
            margin-right: 5px;
        }
        .search-tab.active {
            background: #0066cc;
            color: white;
        }
        .search-content {
            display: none;
        }
        .search-content.active {
            display: block;
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
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        
        <div class="header">
            <h2>Public Health Engineering Department Govt. Of Chhattisgarh</h2>
        </div>

        <div class="progress-container">
            <div class="stepper-wrapper">
                <div class="stepper-item completed" id="step1">
                    <div class="step-counter">1</div>
                    <div class="step-name">General-Abstract</div>
                </div>
                <div class="stepper-item" id="step2">
                    <div class="step-counter">2</div>
                    <div class="step-name">Sub-Estimate</div>
                </div>
                <div class="stepper-item" id="step3">
                    <div class="step-counter">3</div>
                    <div class="step-name">Component of Sub-Estimate</div>
                </div>
                <div class="stepper-item">
                    <div class="step-counter">4</div>
                    <div class="step-name">eMB Entry</div>
                </div>
            </div>
        </div>

        <div class="ticker-container">
            <div class="ticker-text">
                ⚠️ Only certified Work Codes are available for Search. ⚠️&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                ⚠️ केवल प्रमाणित Work कोड ही चयन के लिए उपलब्ध होंगे। ⚠️
            </div>
        </div>

        <div class="container">
            <!-- Search Method Selection -->
            <div class="search-tabs">
                <div class="search-tab active" onclick="showSearchMethod('direct')" visible="false">Direct Search (Recommended)</div>
            </div>

            <!-- Direct Search Form (Faster Method) -->
            <div id="directSearch" class="search-content active">
                <div class="card mb-4">
                    <div class="card-header">
                        <h3><b>Direct Search (Type Book No & Work Code)</b></h3>
                        <small class="text-muted">Fastest method - directly type the values you know</small>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group autocomplete-container">
                                    <label>eMB Book No:</label>
                                    <asp:TextBox 
                                        ID="txtEMBBookNo" 
                                        runat="server" 
                                        CssClass="form-control" 
                                        placeholder="Type to search..."
                                        onkeyup="searchEMBBookNo(this.value)" />
                                    <div id="bookNoSuggestions" class="autocomplete-suggestions"></div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Work Code (Optional):</label>
                                    <asp:TextBox 
                                        ID="txtWorkCode" 
                                        runat="server" 
                                        CssClass="form-control" 
                                        placeholder="Enter Work Code or leave empty" />
                                    <small class="text-muted">Leave empty to see all work codes for the book</small>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <asp:Button
                                        ID="btnDirectSearch"
                                        runat="server"
                                        Text="Search"
                                        CssClass="btn btn-primary form-control"
                                        OnClick="btnDirectSearch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Dropdown Search Form (Original Method) -->
            <div id="dropdownSearch" class="search-content" visible="false">
                <div class="card mb-4">
                    <div class="card-header">
                        <h3><b>Dropdown Search</b></h3>
                        <small class="text-muted">Traditional method - may be slower for large datasets</small>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>eMB Book No:</label>
                                    <asp:DropDownList
                                        ID="ddlEMBBookNo"
                                        runat="server"
                                        CssClass="form-control"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlEMBBookNo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>Work Code:</label>
                                    <asp:DropDownList
                                        ID="ddlWorkCode"
                                        runat="server"
                                        CssClass="form-control"
                                        Enabled="false">
                                        <asp:ListItem Text="-- Select Book No First --" Value="" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <asp:Button
                                        ID="btnSearch"
                                        runat="server"
                                        Text="Search"
                                        CssClass="btn btn-primary form-control"
                                        OnClick="btnSearch_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Work Codes List Panel (for when book is entered but work code is not) -->
            <asp:Panel ID="pnlWorkCodes" runat="server" Visible="false" CssClass="card mb-4">
                <div class="card-header">
                    <h4>Select Work Code</h4>
                </div>
                <div class="card-body">
                    <asp:GridView
                        ID="gvWorkCodes"
                        runat="server"
                        AutoGenerateColumns="false"
                        CssClass="table table-striped"
                        OnRowCommand="gvWorkCodes_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="WorkCode" HeaderText="Work Code" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button
                                        runat="server"
                                        Text="Select"
                                        CssClass="btn btn-sm btn-primary"
                                        CommandName="Select"
                                        CommandArgument='<%# Eval("WorkCode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- Results -->
            <div class="card mb-4">
                <div class="card-header">
                    <h3><b>Details of Work</b></h3>
                </div>
                <div class="card-body">
                    <asp:GridView
                        ID="gvAgreements"
                        runat="server"
                        AutoGenerateColumns="false"
                        DataKeyNames="Work_Code,AgreementBy,Year_Of_Agreement,Agreement_No"
                        OnRowCommand="gvAgreements_RowCommand"
                        CssClass="table table-bordered details-grid"
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
                <div class="row mt-3">
                    <div class="col-md-3">
                        <asp:Button
                            ID="btnViewComponents"
                            runat="server"
                            Text="View Components"
                            OnClick="btnViewComponents_Click"
                            Visible="false"
                            CssClass="btn btn-primary form-control" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        // Use noConflict mode to avoid $ conflicts
        var $j = jQuery.noConflict();

        // Now use $j instead of $
        $j(document).ready(function () {
            console.log('jQuery loaded with noConflict');

            // Example usage
            $j('#myButton').click(function () {
                alert('Button clicked!');
            });
        });

        // Alternative: Create your own $ function
        (function ($) {
            $(document).ready(function () {
                // Now you can use $ within this scope
                $('.my-element').fadeIn();
            });
        })(jQuery);
</script>
    <script>
        // Search method switching
        function showSearchMethod(method) {
            // Hide all search contents
            document.querySelectorAll('.search-content').forEach(content => {
                content.classList.remove('active');
            });

            // Remove active class from all tabs
            document.querySelectorAll('.search-tab').forEach(tab => {
                tab.classList.remove('active');
            });

            // Show selected method
            if (method === 'direct') {
                document.getElementById('directSearch').classList.add('active');
                event.target.classList.add('active');
            } else {
                document.getElementById('dropdownSearch').classList.add('active');
                event.target.classList.add('active');
            }
        }

        // Autocomplete functionality
        let searchTimeout;
        let selectedIndex = -1;

        function searchEMBBookNo(searchTerm) {
            clearTimeout(searchTimeout);

            if (searchTerm.length < 2) {
                hideBookNoSuggestions();
                return;
            }

            searchTimeout = setTimeout(() => {
                PageMethods.SearchEMBBookNo(searchTerm, onBookNoSearchSuccess, onBookNoSearchError);
            }, 300); // Debounce for 300ms
        }

        function onBookNoSearchSuccess(results) {
            const suggestionsDiv = document.getElementById('bookNoSuggestions');

            if (results.length === 0) {
                hideBookNoSuggestions();
                return;
            }

            let html = '';
            results.forEach((item, index) => {
                html += `<div class="autocomplete-suggestion" onclick="selectBookNo('${item}')" data-index="${index}">${item}</div>`;
            });

            suggestionsDiv.innerHTML = html;
            suggestionsDiv.style.display = 'block';
            selectedIndex = -1;
        }

        function onBookNoSearchError(error) {
            console.error('Search error:', error);
            hideBookNoSuggestions();
        }

        function selectBookNo(bookNo) {
            document.getElementById('<%= txtEMBBookNo.ClientID %>').value = bookNo;
            hideBookNoSuggestions();
        }

        function hideBookNoSuggestions() {
            document.getElementById('bookNoSuggestions').style.display = 'none';
            selectedIndex = -1;
        }

        // Keyboard navigation for autocomplete
        document.getElementById('<%= txtEMBBookNo.ClientID %>').addEventListener('keydown', function (e) {
            const suggestions = document.querySelectorAll('#bookNoSuggestions .autocomplete-suggestion');

            if (e.key === 'ArrowDown') {
                e.preventDefault();
                selectedIndex = Math.min(selectedIndex + 1, suggestions.length - 1);
                updateSelection(suggestions);
            } else if (e.key === 'ArrowUp') {
                e.preventDefault();
                selectedIndex = Math.max(selectedIndex - 1, -1);
                updateSelection(suggestions);
            } else if (e.key === 'Enter' && selectedIndex >= 0) {
                e.preventDefault();
                suggestions[selectedIndex].click();
            } else if (e.key === 'Escape') {
                hideBookNoSuggestions();
            }
        });

        function updateSelection(suggestions) {
            suggestions.forEach((suggestion, index) => {
                suggestion.classList.toggle('selected', index === selectedIndex);
            });
        }

        // Hide suggestions when clicking outside
        document.addEventListener('click', function (e) {
            if (!e.target.closest('.autocomplete-container')) {
                hideBookNoSuggestions();
            }
        });

        // Initialize page
        document.addEventListener("DOMContentLoaded", function () {
            document.getElementById('step1').classList.add('completed');
            document.getElementById('step2').classList.remove('completed');
            document.getElementById('step3').classList.remove('completed');
        });
    </script>
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>