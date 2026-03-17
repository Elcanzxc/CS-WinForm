<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Book_Library.Login" %>

<!DOCTYPE html>
<html lang="az">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Daxil ol – Kitabxana</title>
    <link rel="stylesheet" href="Styles/Site.css" />
</head>
<body>
    <div class="auth-wrapper">
        <form id="form1" runat="server" method="post">
            <div class="auth-card">
                <h2 class="auth-title">📚 Kitabxana Sistemi</h2>

                <asp:Label ID="lblSuccess" runat="server" CssClass="msg msg-success" Visible="false" />
                <asp:Label ID="lblError"   runat="server" CssClass="msg msg-error"   Visible="false" />

                <div class="form-group">
                    <label>İstifadəçi adı</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"
                                 placeholder="İstifadəçi adınızı daxil edin" />
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                        ControlToValidate="txtUsername"
                        ErrorMessage="İstifadəçi adı boş ola bilməz"
                        CssClass="val-msg" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label>Şifrə</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                 CssClass="form-control" placeholder="Şifrənizi daxil edin" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="Şifrə boş ola bilməz"
                        CssClass="val-msg" Display="Dynamic" />
                </div>

                <asp:Button ID="btnLogin" runat="server" Text="Daxil ol"
                            CssClass="btn btn-primary" OnClick="btnLogin_Click" />

                <p class="auth-link">
                    Hesabınız yoxdur?
                    <a href="Register.aspx">Qeydiyyatdan keçin</a>
                </p>
            </div>
        </form>
    </div>
</body>
</html>