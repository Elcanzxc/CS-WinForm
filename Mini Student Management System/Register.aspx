<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.cs" Inherits="Mini_Student_Management_System.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center mt-5">
        <div class="col-md-5">
            <div class="card shadow-lg p-4">
                <div class="card-body">
                    <h2 class="text-center mb-4 text-primary">Yeni Hesab</h2>
                    <hr />

                    <div class="mb-3">
                        <label class="form-label">İstifadəçi adı</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Məsələn: aliyev77"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUser" runat="server" ControlToValidate="txtUsername" 
                            ErrorMessage="Username boş ola bilməz!" ForeColor="Red" Display="Dynamic" CssClass="small" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Email ünvanı</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="nümunə@mail.com"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                            ErrorMessage="Email boş ola bilməz!" ForeColor="Red" Display="Dynamic" CssClass="small" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ErrorMessage="Email formatı düzgün deyil!" ForeColor="Red" Display="Dynamic" CssClass="small" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Şifrə</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtPassword" 
                            ErrorMessage="Şifrə boş ola bilməz!" ForeColor="Red" Display="Dynamic" CssClass="small" />
                    </div>

                    <div class="d-grid gap-2 mt-4">
                        <asp:Button ID="btnRegister" runat="server" Text="Qeydiyyatdan keç" CssClass="btn btn-primary btn-lg" OnClick="btnRegister_Click" />
                    </div>

                    <div class="text-center mt-3">
                        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-2"></asp:Label>
                        <p class="text-muted">Artıq hesabınız var? <a href="Login.aspx" class="text-primary text-decoration-none fw-bold">Daxil ol</a></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>