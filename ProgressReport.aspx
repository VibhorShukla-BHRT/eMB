<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProgressReport.aspx.cs" Inherits="PHEDChhattisgarh.ProgressReport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Progress Report - eMB System</title>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.9.1/chart.min.js"></script>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f5f5f5;
        }
        
        .container {
            max-width: 1200px;
            margin: 0 auto;
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        
        .header {
            background-color: #2c3e50;
            color: white;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }
        
        .emb-selector {
            margin-bottom: 20px;
            padding: 15px;
            background-color: #ecf0f1;
            border-radius: 5px;
        }
        
        .emb-selector select {
            padding: 8px;
            margin-right: 10px;
            border: 1px solid #bdc3c7;
            border-radius: 3px;
        }
        
        .emb-selector button {
            padding: 8px 15px;
            background-color: #3498db;
            color: white;
            border: none;
            border-radius: 3px;
            cursor: pointer;
        }
        
        .emb-selector button:hover {
            background-color: #2980b9;
        }
        
        .agreement-details {
            background-color: #f8f9fa;
            padding: 15px;
            border-left: 4px solid #3498db;
            margin-bottom: 20px;
            display: none;
        }
        
        .details-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 15px;
        }
        
        .detail-item {
            background-color: white;
            padding: 10px;
            border-radius: 4px;
            border: 1px solid #e0e0e0;
        }
        
        .detail-label {
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 5px;
        }
        
        .detail-value {
            color: #555;
        }
        
        .components-section {
            margin-top: 20px;
            display: none;
        }
        
        .component-item {
            border: 1px solid #ddd;
            margin-bottom: 10px;
            border-radius: 5px;
            overflow: hidden;
        }
        
        .component-header {
            background-color: #34495e;
            color: white;
            padding: 12px 15px;
            cursor: pointer;
            position: relative;
        }
        
        .component-header:hover {
            background-color: #2c3e50;
        }
        
        .component-header .expand-icon {
            float: right;
            font-size: 14px;
            transition: transform 0.3s;
        }
        
        .component-header.expanded .expand-icon {
            transform: rotate(90deg);
        }
        
        .component-content {
            display: none;
            padding: 15px;
            background-color: #f8f9fa;
        }
        
        .component-info {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 10px;
            margin-bottom: 15px;
            padding: 10px;
            background-color: white;
            border-radius: 4px;
        }
        .progress-percentage {
            background-color: #3498db;
            color: white;
            padding: 4px 8px;
            border-radius: 12px;
            font-size: 12px;
            font-weight: bold;
            margin-left: 10px;
        }
        .subcomponents-section {
            margin-top: 15px;
        }
        
        .subcomponent-item {
            border: 1px solid #ccc;
            margin-bottom: 8px;
            border-radius: 3px;
        }
        
        .subcomponent-header {
            background-color: #7f8c8d;
            color: white;
            padding: 10px 12px;
            cursor: pointer;
            font-size: 14px;
        }
        
        .subcomponent-header:hover {
            background-color: #6c7b7d;
        }
        
        .subcomponent-content {
            display: none;
            padding: 12px;
            background-color: #ffffff;
        }
        
        .subcomponent-info {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
            gap: 8px;
            margin-bottom: 10px;
            font-size: 13px;
        }
        
        .emb-entries {
            margin-top: 10px;
        }
        
        .emb-entry {
            background-color: #f1f2f6;
            border: 1px solid #ddd;
            border-radius: 3px;
            padding: 8px;
            margin-bottom: 5px;
            font-size: 12px;
        }
        
        .emb-entry-header {
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 5px;
        }
        
        .emb-entry-details {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
            gap: 5px;
        }
        
        .loading {
            text-align: center;
            padding: 20px;
            color: #7f8c8d;
        }
        
        .no-data {
            text-align: center;
            padding: 20px;
            color: #7f8c8d;
            font-style: italic;
        }
        
        .status-badge {
            display: inline-block;
            padding: 3px 8px;
            border-radius: 12px;
            font-size: 11px;
            font-weight: bold;
        }
        
        .status-current {
            background-color: #27ae60;
            color: white;
        }
        
        .status-inactive {
            background-color: #95a5a6;
            color: white;
        }
        .subcomponent-progress {
    background-color: #27ae60;
    color: white;
    padding: 3px 6px;
    border-radius: 10px;
    font-size: 11px;
    font-weight: bold;
    margin-left: 8px;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        
        <div class="container">
            <div class="header">
                <h1>eMB Progress Report</h1>
            </div>
            
            <div class="emb-selector">
                <label for="ddlEmbBooks">Select eMB Book Number:</label>
                <asp:DropDownList ID="ddlEmbBooks" runat="server" CssClass="emb-dropdown">
                    <asp:ListItem Text="-- Select eMB Book --" Value="" />
                </asp:DropDownList>
                <asp:DropDownList ID="ddlWorkCodes" runat="server" CssClass="emb-dropdown" style="display:none;">
                    <asp:ListItem Text="-- Select Work Code --" Value="" />
                </asp:DropDownList>
                <button type="button" id="btnLoadReport" onclick="loadProgressReport()">Load Report</button>
                <button type="button" id="btnRefresh" onclick="refreshReport()" style="margin-left: 10px; background-color: #27ae60;">Refresh
</button>
            </div>
            
            <div id="agreementDetails" class="agreement-details">
                <h3>Agreement Details</h3>
                <div id="agreementContent" class="details-grid"></div>
            </div>
            
            <div id="componentsSection" class="components-section">
                <h3>Components</h3>
                <div id="componentsContent"></div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function loadEmbBooks() {
            PageMethods.GetEmbBooks(onEmbBooksSuccess, onFailure);
        }

        function onEmbBooksSuccess(result) {
            var ddl = document.getElementById('<%= ddlEmbBooks.ClientID %>');
            ddl.innerHTML = '<option value="">-- Select eMB Book --</option>';

            for (var i = 0; i < result.length; i++) {
                var option = document.createElement('option');
                option.value = result[i];
                option.text = result[i];
                ddl.appendChild(option);
            }

            ddl.onchange = function () {
                if (this.value) {
                    loadWorkCodes(this.value);
                } else {
                    document.getElementById('<%= ddlWorkCodes.ClientID %>').style.display = 'none';
                    hideReportSections();
                }
            };
        }

        function loadWorkCodes(bookNo) {
            PageMethods.GetWorkCodes(bookNo, onWorkCodesSuccess, onFailure);
        }

        function onWorkCodesSuccess(result) {
            var ddl = document.getElementById('<%= ddlWorkCodes.ClientID %>');
            ddl.innerHTML = '<option value="">-- Select Work Code --</option>';
            
            for (var i = 0; i < result.length; i++) {
                var option = document.createElement('option');
                option.value = result[i];
                option.text = result[i];
                ddl.appendChild(option);
            }
            
            ddl.style.display = 'inline-block';
        }
        
        function loadProgressReport() {
            var embBook = document.getElementById('<%= ddlEmbBooks.ClientID %>').value;
            var workCode = document.getElementById('<%= ddlWorkCodes.ClientID %>').value;
            
            console.log('Loading progress report for:', embBook, workCode); // Debug log
            
            if (!embBook || !workCode) {
                alert('Please select both eMB Book and Work Code');
                return;
            }
            
            showLoading();
            console.log('Calling GetProgressReport with workCode:', workCode); // Debug log
            PageMethods.GetProgressReport(workCode, onProgressReportSuccess, onFailure);
        }
        
        function onProgressReportSuccess(result) {
            console.log('Progress Report Data:', result); // Debug log
            hideLoading();
            
            if (result && result.AgreementDetails) {
                displayAgreementDetails(result.AgreementDetails);
            } else {
                console.error('No agreement details found');
                document.getElementById('agreementDetails').innerHTML = '<div class="no-data">No agreement details found</div>';
                document.getElementById('agreementDetails').style.display = 'block';
            }
            
            if (result && result.Components && result.Components.length > 0) {
                displayComponents(result.Components);
                // Store components data for later use
                document.getElementById('componentsSection').dataset.components = JSON.stringify(result.Components);
            } else {
                console.error('No components found');
                document.getElementById('componentsContent').innerHTML = '<div class="no-data">No components found for this work code</div>';
                document.getElementById('componentsSection').style.display = 'block';
            }
        }
        
        function displayAgreementDetails(details) {
            console.log('Agreement Details:', details); // Debug log
            
            if (!details) {
                document.getElementById('agreementContent').innerHTML = '<div class="no-data">No agreement details available</div>';
                return;
            }
            
            var content = document.getElementById('agreementContent');
            content.innerHTML = `
                <div class="detail-item">
                    <div class="detail-label">Year of Agreement</div>
                    <div class="detail-value">${details.Year_Of_Agreement || 'N/A'}</div>
                </div>
                <div class="detail-item">
                    <div class="detail-label">Agreement By</div>
                    <div class="detail-value">${details.AgreementByName || details.AgreementBy || 'N/A'}</div>
                </div>
                <div class="detail-item">
                    <div class="detail-label">Agreement No</div>
                    <div class="detail-value">${details.Agreement_No || 'N/A'}</div>
                </div>
                <div class="detail-item">
                    <div class="detail-label">Work Code</div>
                    <div class="detail-value">${details.Work_Code || 'N/A'}</div>
                </div>
            `;
            
            document.getElementById('agreementDetails').style.display = 'block';
        }
        function calculateProgressPercentage(completedQty, aaQuantity) {
            if (!aaQuantity || aaQuantity == 0) return 0;
            var percentage = (completedQty / aaQuantity) * 100;
            return Math.round(percentage * 100) / 100; // Round to 2 decimal places
        }
        
        function displayComponents(components) {
            console.log('Components Data:', components); // Debug log
            
            var content = document.getElementById('componentsContent');
            content.innerHTML = '';
            
            if (!components || components.length === 0) {
                content.innerHTML = '<div class="no-data">No components found</div>';
                document.getElementById('componentsSection').style.display = 'block';
                return;
            }
            
            components.forEach(function(component, index) {
                console.log('Processing component:', component); // Debug log
                
                var componentDiv = document.createElement('div');
                componentDiv.className = 'component-item';
                componentDiv.innerHTML = `
                    <div class="component-header" onclick="toggleComponent(${index})">
                        <strong>${component.ComponentName || 'Unknown Component'}</strong>
<span class="progress-percentage">${calculateProgressPercentage(component.CompletedQty, component.AA_Quantity)}% Complete</span>
                        <span class="expand-icon">▶</span>
                    </div>
                    <div class="component-content" id="component-${index}">
                        <div class="component-info">
                            <div><strong>Component ID:</strong> ${component.ComponentID || 'N/A'}</div>
                            <div><strong>AA Amount:</strong> ${component.AA_Amount || '0'}</div>
                            <div><strong>AA Quantity:</strong> ${component.AA_Quantity || '0'}</div>
                            <div><strong>Unit:</strong> ${component.ComponentUnit || 'N/A'}</div>
                            <div><strong>Completed Qty:</strong> ${component.CompletedQty || '0'}</div>
                            <div><strong>Remaining Qty:</strong> ${component.RemainingQty || '0'}</div>
                        </div>
                        <div class="subcomponents-section">
                            <h4>Sub-components</h4>
                            <div id="subcomponents-${index}">
                                <div class="loading">Click to load sub-components...</div>
                            </div>
                        </div>
                    </div>
                `;
                content.appendChild(componentDiv);
            });
            
            document.getElementById('componentsSection').style.display = 'block';
        }
        function refreshReport() {
            var embBook = document.getElementById('<%= ddlEmbBooks.ClientID %>').value;
            var workCode = document.getElementById('<%= ddlWorkCodes.ClientID %>').value;

            if (!embBook || !workCode) {
                alert('Please select both eMB Book and Work Code first');
                return;
            }

            // Clear existing data and reload
            hideReportSections();
            loadProgressReport();
        }
        
        function toggleComponent(index) {
            var content = document.getElementById(`component-${index}`);
            var header = content.previousElementSibling;
            
            if (content.style.display === 'none' || content.style.display === '') {
                content.style.display = 'block';
                header.classList.add('expanded');
                loadSubComponents(index);
            } else {
                content.style.display = 'none';
                header.classList.remove('expanded');
            }
        }
        
        function loadSubComponents(componentIndex) {
            var workCode = document.getElementById('<%= ddlWorkCodes.ClientID %>').value;
            var subcomponentsDiv = document.getElementById(`subcomponents-${componentIndex}`);

            if (subcomponentsDiv.innerHTML.includes('Click to load')) {
                subcomponentsDiv.innerHTML = '<div class="loading">Loading sub-components...</div>';

                // Get component details for the API call
                var components = JSON.parse(document.getElementById('componentsSection').dataset.components || '[]');
                var component = components[componentIndex];

                PageMethods.GetSubComponentsWithProgress(workCode, component.Year_Of_Agreement, component.Agreement_No,
                    component.ComponentID, component.AgreementBy,
                    function (result) { onSubComponentsSuccess(result, componentIndex); },
                    onFailure);
            }
        }

        function onSubComponentsSuccess(result, componentIndex) {
            var subcomponentsDiv = document.getElementById(`subcomponents-${componentIndex}`);
            subcomponentsDiv.innerHTML = '';

            if (result.length === 0) {
                subcomponentsDiv.innerHTML = '<div class="no-data">No sub-components found</div>';
                return;
            }

            // Store subcomponents data for later use
            subcomponentsDiv.dataset.subcomponents = JSON.stringify(result);

            result.forEach(function (subComponent, subIndex) {
                var subDiv = document.createElement('div');
                subDiv.className = 'subcomponent-item';
                subDiv.innerHTML = `
                    <div class="subcomponent-header" onclick="toggleSubComponent(${componentIndex}, ${subIndex})">
                        ${subComponent.SORItemNo} - ${subComponent.SORItem}
<span class="subcomponent-progress">${calculateProgressPercentage(subComponent.CompletedQty, subComponent.Qty)}% Complete</span>
                        <span class="expand-icon">▶</span>
                    </div>
                    <div class="subcomponent-content" id="subcomponent-${componentIndex}-${subIndex}">
                        <div class="subcomponent-info">
                            <div><strong>SOR From:</strong> ${subComponent.SORFrom}</div>
                            <div><strong>Basic/Amendment:</strong> ${subComponent.BasicorAmendment}</div>
                            <div><strong>Sub Item:</strong> ${subComponent.SORSubItem}</div>
                            <div><strong>Quantity:</strong> ${subComponent.Qty}</div>
                            <div><strong>Unit:</strong> ${subComponent.ActualUnit}</div>
                            <div><strong>Unit Cost:</strong> ${subComponent.UnitCost}</div>
                            <div><strong>Amount (GST):</strong> ${subComponent.AmountWithGST}</div>
                        </div>
                        <div class="emb-entries">
                            <h5>eMB Entries</h5>
                            <div id="emb-entries-${componentIndex}-${subIndex}">
                                <div class="loading">Click to load eMB entries...</div>
                            </div>
                        </div>
                    </div>
                `;
                subcomponentsDiv.appendChild(subDiv);
            });
        }

        function toggleSubComponent(componentIndex, subIndex) {
            var content = document.getElementById(`subcomponent-${componentIndex}-${subIndex}`);
            var header = content.previousElementSibling;

            if (content.style.display === 'none' || content.style.display === '') {
                content.style.display = 'block';
                header.classList.add('expanded');
                loadEmbEntries(componentIndex, subIndex);
            } else {
                content.style.display = 'none';
                header.classList.remove('expanded');
            }
        }

        function loadEmbEntries(componentIndex, subIndex) {
            var entriesDiv = document.getElementById(`emb-entries-${componentIndex}-${subIndex}`);

            if (entriesDiv.innerHTML.includes('Click to load')) {
                entriesDiv.innerHTML = '<div class="loading">Loading eMB entries...</div>';

                // Get the required data from stored component and subcomponent data
                var components = JSON.parse(document.getElementById('componentsSection').dataset.components || '[]');
                var component = components[componentIndex];

                var subcomponentsDiv = document.getElementById(`subcomponents-${componentIndex}`);
                var subComponents = JSON.parse(subcomponentsDiv.dataset.subcomponents || '[]');
                var subComponent = subComponents[subIndex];

                PageMethods.GetEmbEntries(component.Work_Code, component.AgreementBy,
                    component.Year_Of_Agreement, component.Agreement_No, component.ComponentID,
                    subComponent.SORItemNo,
                    function (result) { onEmbEntriesSuccess(result, componentIndex, subIndex); },
                    onFailure);
            }
        }

        function onEmbEntriesSuccess(result, componentIndex, subIndex) {
            var entriesDiv = document.getElementById(`emb-entries-${componentIndex}-${subIndex}`);
            entriesDiv.innerHTML = '';

            if (result.length === 0) {
                entriesDiv.innerHTML = '<div class="no-data">No eMB entries found</div>';
                return;
            }

            result.forEach(function (entry) {
                var entryDiv = document.createElement('div');
                entryDiv.className = 'emb-entry';
                entryDiv.innerHTML = `
                    <div class="emb-entry-header">
                        Entry ID: ${entry.UniqueEmbID}
                        <span class="status-badge ${entry.IsCurrent ? 'status-current' : 'status-inactive'}">
                            ${entry.IsCurrent ? 'Current' : 'Inactive'}
                        </span>
                    </div>
                    <div class="emb-entry-details">
                        <div><strong>EMB ID:</strong> ${entry.EmbId}</div>
                        <div><strong>SOR Item:</strong> ${entry.SORItemNo}</div>
                        <div><strong>Result Value:</strong> ${entry.ResultValue}</div>
                        <div><strong>Unit:</strong> ${entry.ActualUnit}</div>
                        <div><strong>Inputs:</strong> ${entry.Inputs || 'N/A'}</div>
                        <div><strong>Remark:</strong> ${entry.Remark || 'N/A'}</div>
                    </div>
                `;
                entriesDiv.appendChild(entryDiv);
            });
        }

        function showLoading() {
            var agreementDiv = document.getElementById('agreementDetails');
            var componentsDiv = document.getElementById('componentsSection');

            agreementDiv.innerHTML = '<div class="loading">Loading agreement details...</div>';
            agreementDiv.style.display = 'block';

            componentsDiv.innerHTML = '<div class="loading">Loading components...</div>';
            componentsDiv.style.display = 'block';
        }

        function hideLoading() {
            // Loading content will be replaced by actual content
            // Reset the sections to their original structure
            document.getElementById('agreementDetails').innerHTML = `
                <h3>Agreement Details</h3>
                <div id="agreementContent" class="details-grid"></div>
            `;

            document.getElementById('componentsSection').innerHTML = `
                <h3>Components</h3>
                <div id="componentsContent"></div>
            `;
        }

        function hideReportSections() {
            document.getElementById('agreementDetails').style.display = 'none';
            document.getElementById('componentsSection').style.display = 'none';
        }

        function onFailure(error) {
            console.error('AJAX Error:', error);
            alert('Error: ' + (error.get_message ? error.get_message() : error));
            hideLoading();
        }

        // Initialize on page load
        window.onload = function () {
            loadEmbBooks();
        };
    </script>
</body>
</html>