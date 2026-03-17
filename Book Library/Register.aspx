<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Book_Library.Register" %>

<!DOCTYPE html>
<html lang="az">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Qeydiyyat – Kitabxana</title>
    <link rel="stylesheet" href="Styles/Site.css" />
</head>
<body>
    <div class="auth-wrapper">
        <form id="form1" runat="server">
            <div class="auth-card">
                <h2 class="auth-title">📚 Qeydiyyat</h2>

                <asp:Label ID="lblMessage" runat="server" CssClass="msg" Visible="false" />

                <div class="form-group">
                    <label>İstifadəçi adı</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"
                                 placeholder="İstifadəçi adı daxil edin" MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                        ControlToValidate="txtUsername"
                        ErrorMessage="İstifadəçi adı boş ola bilməz"
                        CssClass="val-msg" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label>Şifrə</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                 CssClass="form-control" placeholder="Şifrə daxil edin" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="Şifrə boş ola bilməz"
                        CssClass="val-msg" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label>E-poçt</label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email"
                                 CssClass="form-control" placeholder="email@numune.az" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="E-poçt boş ola bilməz"
                        CssClass="val-msg" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="Düzgün e-poçt ünvanı daxil edin"
                        CssClass="val-msg" Display="Dynamic" />
                </div>

                <asp:Button ID="btnRegister" runat="server" Text="Qeydiyyat"
                            CssClass="btn btn-primary" OnClick="btnRegister_Click" />

                <p class="auth-link">
                    Artıq hesabınız var?
                    <a href="Login.aspx">Daxil olun</a>
                </p>
            </div>
        </form>
    </div>
</body>
</html>
