<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Students.aspx.cs" Inherits="Mini_Student_Management_System.Students" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="text-primary fw-bold">🎓 Tələbə İdarəetmə Paneli</h2>
            <div class="badge bg-info text-dark p-2 px-3 shadow-sm">
                Sessiya: <asp:Label ID="lblUser" runat="server" Font-Bold="true"></asp:Label>
            </div>
        </div>

        <div class="card shadow-sm border-0 mb-5">
            <div class="card-header bg-white py-3">
                <h5 class="card-title mb-0"><i class="bi bi-person-plus"></i> Tələbə Məlumatları</h5>
            </div>
            <div class="card-body bg-light">
                <div class="row g-3 align-items-end">
                    <div class="col-md-3">
                        <label class="form-label small fw-bold">Tələbə No</label>
                        <asp:TextBox ID="txtID" runat="server" CssClass="form-control" placeholder="Məs: S101"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSID" runat="server" ControlToValidate="txtID" ErrorMessage="Mütləqdir!" ForeColor="Red" Display="Dynamic" ValidationGroup="StudentGroup" CssClass="small" />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label small fw-bold">Ad</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Ad daxil edin"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSName" runat="server" ControlToValidate="txtName" ErrorMessage="Mütləqdir!" ForeColor="Red" Display="Dynamic" ValidationGroup="StudentGroup" CssClass="small" />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label small fw-bold">Soyad</label>
                        <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control" placeholder="Soyad daxil edin"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSSurname" runat="server" ControlToValidate="txtSurname" ErrorMessage="Mütləqdir!" ForeColor="Red" Display="Dynamic" ValidationGroup="StudentGroup" CssClass="small" />
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnAdd" runat="server" Text="💾 Yadda Saxla" OnClick="btnAdd_Click" ValidationGroup="StudentGroup" CssClass="btn btn-success w-100 fw-bold shadow-sm" />
                    </div>
                </div>
            </div>
        </div>

        <div class="card shadow-sm border-0">
            <div class="card-header bg-dark text-white py-3">
                <h5 class="card-title mb-0">Сiyahı</h5>
            </div>
            <div class="card-body p-0">
                <div class="table-responsive">
                    <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-hover mb-0 border-0" GridLines="None"
                        OnRowCommand="gvStudents_RowCommand" DataKeyNames="ID">
                        <HeaderStyle CssClass="table-light border-bottom" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="No" ItemStyle-CssClass="fw-bold text-secondary" />
                            <asp:BoundField DataField="Name" HeaderText="Ad" />
                            <asp:BoundField DataField="Surname" HeaderText="Soyad" />
                            <asp:TemplateField HeaderText="Əməliyyatlar" ItemStyle-Width="180px">
                                <ItemTemplate>
                                    <div class="btn-group" role="group">
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditStudent" CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-sm btn-outline-warning" CausesValidation="false">
                                            ✏️ Düzəliş
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteStudent" CommandArgument='<%# Eval("ID") %>' CssClass="btn btn-sm btn-outline-danger" OnClientClick="return confirm('Silmək istədiyinizə əminsiniz?');" CausesValidation="false">
                                            🗑️ Sil
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>