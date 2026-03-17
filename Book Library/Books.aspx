<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Books.aspx.cs" Inherits="Book_Library.Books" %>

<!DOCTYPE html>
<html lang="az">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Kitablar – Kitabxana</title>
    <link rel="stylesheet" href="Styles/Site.css" />
</head>
<body>
    <form id="form1" runat="server">

        <header class="site-header">
            <div class="header-inner">
                <span class="logo">📚 Kitabxana İdarəetmə Sistemi</span>
                <div class="header-user">
                    Salam, <strong><asp:Label ID="lblUsername" runat="server" /></strong>
                    &nbsp;|&nbsp;
                    <asp:Button ID="btnLogout" runat="server" Text="Çıxış"
                                CssClass="btn btn-sm btn-logout" OnClick="btnLogout_Click"
                                CausesValidation="false" />
                </div>
            </div>
        </header>

        <main class="main-container">

            <asp:Label ID="lblMessage" runat="server" CssClass="msg" Visible="false" />

            <div class="card form-card">
                <h3 class="card-title">
                    <asp:Label ID="lblFormTitle" runat="server" Text="➕ Yeni Kitab Əlavə Et" />
                </h3>

                <asp:HiddenField ID="hfBookId" runat="server" Value="0" />

                <div class="form-row">
                    <div class="form-group">
                        <label>Kitabın adı *</label>
                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"
                                     placeholder="Kitabın adını daxil edin" MaxLength="200" />
                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server"
                            ControlToValidate="txtTitle"
                            ErrorMessage="Kitab adı boş ola bilməz"
                            CssClass="val-msg" Display="Dynamic"
                            ValidationGroup="BookForm" />
                    </div>

                    <div class="form-group">
                        <label>Müəllif *</label>
                        <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control"
                                     placeholder="Müəllif adını daxil edin" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvAuthor" runat="server"
                            ControlToValidate="txtAuthor"
                            ErrorMessage="Müəllif boş ola bilməz"
                            CssClass="val-msg" Display="Dynamic"
                            ValidationGroup="BookForm" />
                    </div>

                    <div class="form-group">
                        <label>Nəşr ili *</label>
                        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control"
                                     placeholder="Məs: 2020" MaxLength="4" />
                        <asp:RequiredFieldValidator ID="rfvYear" runat="server"
                            ControlToValidate="txtYear"
                            ErrorMessage="İl boş ola bilməz"
                            CssClass="val-msg" Display="Dynamic"
                            ValidationGroup="BookForm" />
                        <asp:RangeValidator ID="rvYear" runat="server"
                            ControlToValidate="txtYear"
                            MinimumValue="1000" MaximumValue="2100"
                            Type="Integer"
                            ErrorMessage="1000-2100 arasında il daxil edin"
                            CssClass="val-msg" Display="Dynamic"
                            ValidationGroup="BookForm" />
                    </div>

                    <div class="form-group">
                        <label>Janr *</label>
                        <asp:DropDownList ID="ddlGenre" runat="server" CssClass="form-control">
                            <asp:ListItem Value="">-- Janr seçin --</asp:ListItem>
                            <asp:ListItem>Roman</asp:ListItem>
                            <asp:ListItem>Hekayə</asp:ListItem>
                            <asp:ListItem>Poema</asp:ListItem>
                            <asp:ListItem>Dram</asp:ListItem>
                            <asp:ListItem>Elmi</asp:ListItem>
                            <asp:ListItem>Tarix</asp:ListItem>
                            <asp:ListItem>Fəlsəfə</asp:ListItem>
                            <asp:ListItem>Uşaq ədəbiyyatı</asp:ListItem>
                            <asp:ListItem>Digər</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvGenre" runat="server"
                            ControlToValidate="ddlGenre"
                            InitialValue=""
                            ErrorMessage="Janr seçin"
                            CssClass="val-msg" Display="Dynamic"
                            ValidationGroup="BookForm" />
                    </div>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnSave" runat="server" Text="💾 Saxla"
                                CssClass="btn btn-primary" OnClick="btnSave_Click"
                                ValidationGroup="BookForm" />
                    <asp:Button ID="btnCancel" runat="server" Text="İptal"
                                CssClass="btn btn-secondary" OnClick="btnCancel_Click"
                                CausesValidation="false" Visible="false" />
                </div>
            </div>

            <div class="card">
                <h3 class="card-title">📋 Kitablar Siyahısı</h3>

                <asp:GridView ID="gvBooks" runat="server"
                              AutoGenerateColumns="false"
                              CssClass="grid"
                              EmptyDataText="Heç bir kitab tapılmadı."
                              OnRowCommand="gvBooks_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id"     HeaderText="#"           ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Title"  HeaderText="Kitabın adı" />
                        <asp:BoundField DataField="Author" HeaderText="Müəllif" />
                        <asp:BoundField DataField="Year"   HeaderText="Nəşr ili"    ItemStyle-Width="90px" />
                        <asp:BoundField DataField="Genre"  HeaderText="Janr" />

                        <asp:TemplateField HeaderText="Əməliyyatlar" ItemStyle-Width="160px">
                            <ItemTemplate>
                                <asp:Button runat="server" Text="✏️ Dəyiş"
                                            CommandName="EditBook"
                                            CommandArgument='<%# Eval("Id") %>'
                                            CssClass="btn btn-sm btn-edit"
                                            CausesValidation="false" />
                                &nbsp;
                                <asp:Button runat="server" Text="🗑️ Sil"
                                            CommandName="DeleteBook"
                                            CommandArgument='<%# Eval("Id") %>'
                                            CssClass="btn btn-sm btn-delete"
                                            CausesValidation="false"
                                            OnClientClick="return confirm('Bu kitabı silmək istədiyinizə əminsiniz?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </main>
    </form>
</body>
</html>