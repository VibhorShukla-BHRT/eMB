<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report01.aspx.cs" Inherits="PHEDChhattisgarh.Report01" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Division eMB Report</title>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f5f5f5;
        }
        .container {
            max-width: 1000px;
            margin: 0 auto;
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        .header {
            text-align: center;
            color: #333;
            margin-bottom: 30px;
            border-bottom: 2px solid #007acc;
            padding-bottom: 10px;
        }
        .grid-container {
            margin-top: 20px;
        }
        .data-grid {
            width: 100%;
            border-collapse: collapse;
            margin: 20px 0;
            font-size: 14px;
        }
        .data-grid th {
            background-color: #007acc;
            color: white;
            padding: 12px 8px;
            text-align: center;
            font-weight: bold;
            border: 1px solid #ddd;
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header">
                <h1>Division eMB Report</h1>
                <p>Division-wise eMB Issued and Initiated Summary</p>
            </div>
            
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh Data" 
                CssClass="refresh-btn" OnClick="btnRefresh_Click" />
            
            <asp:Label ID="lblError" runat="server" CssClass="error-message" 
                Visible="false"></asp:Label>
            
            <div class="grid-container">
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
    </form>
</body>
</html>