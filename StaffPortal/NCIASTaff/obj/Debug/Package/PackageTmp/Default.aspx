<%@ Page Title="Login" Language="C#" MasterPageFile="~/Layout/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NCIASTaff.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="limiter">
        <div class="container-login100">
            <div class="wrap-login100">
                <!-- Logo Section -->
                <div class="login100-form-title img-rounded text-center">
                    <img src="images/logo.png" alt="Logo" class="img-fluid" style="max-width: 200px;" />
                    <hr class="custom-hr" />
                </div>

                <!-- Page Title -->
                <div class="login100-form-title text-center">
                    <h5 class="portal-title">NCIA STAFF PORTAL</h5>
                </div>

                <!-- Login Panel -->
                <asp:Panel DefaultButton="LbtnLogin" ID="LoginPanel" runat="server">
                    <asp:Label ID="lblError" runat="server" CssClass="error-label"></asp:Label>

                    <!-- Username Input -->
                    <div class="wrap-input100 validate-input" data-validate="Enter PF Number">
                        <asp:TextBox CssClass="input100" runat="server" ID="txtusername" Placeholder="PF Number"></asp:TextBox>
                        <span class="focus-input100"></span>
                    </div>

                    <!-- Password Input -->
                    <div class="wrap-input100 validate-input" data-validate="Enter password">
                        <span class="btn-show-pass">
                            <i class="zmdi zmdi-eye"></i>
                        </span>
                        <asp:TextBox CssClass="input100" runat="server" ID="txtpassword" TextMode="Password" Placeholder="Password"></asp:TextBox>
                        <span class="focus-input100"></span>
                    </div>

                    <!-- Login Button -->
                    <div class="container-login100-form-btn">
                        <asp:LinkButton ID="lbtnLogin" runat="server" OnClick="lbtnLogin_Click" CssClass="login-btn"><strong>LOGIN</strong></asp:LinkButton>
                    </div>

                    <!-- Forgot Password -->
                    <div class="text-center p-t-15">
                        <asp:LinkButton ID="lbtnForgot" runat="server" OnClick="lbtnForgot_Click" CssClass="forgot-link">Forgot Password? <i class="fa fa-arrow-circle-right"></i></asp:LinkButton>
                    </div>
                </asp:Panel>

                <!-- Footer -->
                <div class="text-center p-t-25">
                    <span class="txt1">
                        <strong>All Rights Reserved NCIA &copy; <%=DateTime.Now.Year %>. Powered by 
                            <a href="https://appkings.co.ke/" target="_blank">AppKings Solutions Ltd</a>.</strong>
                    </span>
                </div>
            </div>
        </div>
    </div>

    <style>
        .container-login100 {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background-color: #d9e1ef;
}

.wrap-login100 {
    background: #ffffff;
    padding: 30px;
    border-radius: 10px;
    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
    max-width: 400px;
    width: 100%;
}

.login100-form-title {
    margin-bottom: 20px;
}

.portal-title {
    font-family: 'Jazz Let', sans-serif;
    color: #244767;
    text-transform: uppercase;
}

.custom-hr {
    border: 5px solid #244767;
    border-radius: 2px;
    width: 100%;
}

.input100 {
    width: 100%;
    padding: 10px;
    margin-bottom: 15px;
    border: 1px solid #ddd;
    border-radius: 5px;
    box-sizing: border-box;
}

.login-btn {
    background-color: #244767;
    color: white;
    padding: 10px;
    border-radius: 5px;
    display: block;
    width: 100%;
    text-align: center;
    font-size: 16px;
    cursor: pointer;
}

.forgot-link {
    color: #244767;
    text-decoration: none;
}

.error-label {
    color: red;
    font-size: 14px;
    text-align: center;
    display: block;
    margin-bottom: 10px;
}

</style>
</asp:Content>

