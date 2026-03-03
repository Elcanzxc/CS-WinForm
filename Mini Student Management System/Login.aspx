<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="Login.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Mini_Student_Management_System.Login" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center mt-5">
        <div class="col-md-4">
            <div class="card p-4">
                <h3 class="text-center mb-4">Sistemə Giriş</h3>
                <div class="mb-3">
                    <label class="form-label">İstifadəçi adı</label>
                    <asp:TextBox ID="txtLoginUsername" runat="server" CssClass="form-control" placeholder="username"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label class="form-label">Şifrə</label>
                    <asp:TextBox ID="txtLoginPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••"></asp:TextBox>
                </div>
                <div class="mb-3 form-check">
                    <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="form-check-input" />
                    <label class="form-check-label">Məni xatırla</label>
                </div>
                <asp:Button ID="btnLogin" runat="server" Text="Daxil ol" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
                <div class="text-center mt-3">
                    <a href="Register.aspx" class="text-decoration-none">Hesabınız yoxdur? Qeydiyyat</a>
                </div>
                <asp:Label ID="lblLoginMessage" runat="server" CssClass="text-danger mt-2 d-block text-center"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>