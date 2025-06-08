<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoginMaster.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="PHEDChhattisgarh.AppLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="SesValue" runat="server" /> 
        <div class="form-group">
          <div class="form-label-group">
             <asp:TextBox ID="txtSesValue" runat="server" Width="120px" style="display:none;color:white;border:0px solid white" Enabled="false"></asp:TextBox>
             <asp:TextBox ID="txtUserID" runat="server" class="form-control" placeholder="User ID" autofocus=""></asp:TextBox><label for="inputEmail">User Id</label>
          </div>
        </div> 
        <div class="form-group">
          <div class="form-label-group">
             <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password" placeholder="User ID" autofocus=""></asp:TextBox><label for="inputEmail">Password</label>
          </div>
        </div> 
        <asp:TextBox ID="TextBox1" style="display:none" runat="server" class="form-control" TextMode="Password" placeholder="Password" autofocus=""></asp:TextBox>
        <!-- .form-group -->
        <div class="form-group">
          <div class="form-label-group">
                <asp:TextBox ID="txtCaptcha"  style="width:40%;float:left;color:black"  class="form-control"  onkeypress="checkcapslockon(event)" runat="server"   placeholder="Enter Captcha" autocomplete="off"></asp:TextBox> 
                <label for="inputPassword">Captcha</label>
                      <table  style="width:60%;float:left;padding:0px">
                        <tr>
                        <td>
                            <asp:Image ID="imgCaptcha" runat="server"  style="float:left;margin:1px 0px 5px 5px" alt="Loding..." /> 
                        </td>
                        <td valign="middle"> 
                             <asp:ImageButton  style="padding-top:0px;width:35px;float:right;margin:5px 0px 0px 5px" ID="btnRefresh" runat="server" ImageUrl="assets/images/reload.png"  OnClick="btnRefresh_Click" OnClientClick="resetCaptcha();" ></asp:ImageButton>
                        </td>
                    </tr>
                </table>                
            </div>
        </div>        
        <!-- /.form-group -->
        <!-- .form-group -->
        <br />
        <br />
        <center style="margin-top:20px">
            <asp:Label ID="msg" runat="server" Text=""></asp:Label>
           <asp:Button ID="Button1" runat="server" Text="Login"  OnClick="btnLogin_Click" OnClientClick="return validate();" class="btn btn-lg btn-primary btn-block" />
        </center>  
        <br> 
        <asp:UpdateProgress ID="PageUpdateProgress" runat="server" 
            AssociatedUpdatePanelID="UpdatePanel1">
            <progresstemplate>
                <div ID="modal">
                    <div class="loading">
                        <center>
                            <br />
                            <img src="assets/images/loader.gif" />
                        </center>
                    </div>
                </div>
            </progresstemplate>
        </asp:UpdateProgress>
        <script type="text/javascript"> 
              function resetCaptcha() {
                  $("#<%= txtPassword.ClientID %>").val("");
                  $("#<%= txtCaptcha.ClientID %>").val("");
              }
              function validate() {
                  txtUsername = $("#<%= txtUserID.ClientID %>").val();
                  txtPassword = $("#<%= txtPassword.ClientID %>").val();
                  txtCaptcha = $("#<%= txtCaptcha.ClientID %>").val(); 
                  if (txtUsername.trim()=="") {
                      Swal.fire('Enter User ID', 'Required', 'warning');   
                      $("#<%= txtUserID.ClientID %>").focus();
                      return false;
                  }
                  else if (txtPassword.trim() == "") {
                      Swal.fire('Enter Password', 'Required', 'warning');    
                      $("#<%= txtPassword.ClientID %>").focus();
                      return false;
                  }
                  else if (txtCaptcha.trim() == "") {
                      Swal.fire('Enter Captcha', 'Required', 'warning');   
                      $("#<%= txtCaptcha.ClientID %>").focus();
                      return false;
                  }
                  else {
                      $("#<%= txtPassword.ClientID %>").val("");
                      $("#<%= txtPassword.ClientID %>").val(SHA256(SHA256(txtPassword).toUpperCase() + $("#<%= txtSesValue.ClientID %>").val()).toUpperCase()); 
                      return true;
                  }
              }
                </script>
       
    </ContentTemplate> 
</asp:UpdatePanel>

</asp:Content>

