<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ErrorPage.aspx.cs" Inherits="Mini_Student_Management_System.ErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center mt-5">
        <div class="col-md-6 text-center">
            <div class="card shadow-lg border-0 p-5">
                <div class="card-body">
                    <div class="display-1 text-danger mb-4">
                        <i class="bi bi-exclamation-triangle-fill"></i> ⚠️
                    </div>
                    
                    <h2 class="fw-bold text-dark">Ups! Xəta baş verdi</h2>
                    <p class="text-muted fs-5 mt-3">
                        Bağışlayın, sistemdə gözlənilməz bir texniki problem yarandı. 
                        Narahat olmayın, mühəndislərimiz artıq xəbərdar edilib.
                    </p>

                    <hr class="my-4" />

                    <div class="d-grid gap-2 d-sm-flex justify-content-sm-center">
                        <a href="Login.aspx" class="btn btn-primary btn-lg px-4 gap-3 shadow-sm">
                            🏠 Ana səhifəyə qayıt
                        </a>
                        <button type="button" onclick="history.back()" class="btn btn-outline-secondary btn-lg px-4">
                            ⬅️ Geri qayıt
                        </button>
                    </div>
                </div>
                <div class="card-footer bg-transparent border-0 text-muted small">
                    Xəta kodu: 500 | Daxili Server Xətası
                </div>
            </div>
        </div>
    </div>
</asp:Content>