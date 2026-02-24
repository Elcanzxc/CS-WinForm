<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HomeWorkASP.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f7fc;
            margin: 0;
            padding: 0;
        }

        .login-box {
            width: 350px;
            margin: 100px auto;
            padding: 30px;
            background-color: #fff;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        }

        .login-box h3 {
            text-align: center;
            color: #333;
            font-size: 28px;
            margin-bottom: 30px;
            font-weight: bold;
        }

        .login-box input {
            width: 100%;
            padding: 12px;
            margin: 10px 0;
            border: 1px solid #ccc;
            border-radius: 5px;
            font-size: 16px;
            box-sizing: border-box;
        }

        .btn-login {
            background-color: #007bff;
            color: white;
            border: none;
            cursor: pointer;
            padding: 14px;
            width: 100%;
            border-radius: 5px;
            font-size: 16px;
            transition: background-color 0.3s ease;
        }

        .btn-login:hover {
            background-color: #0056b3;
        }

        .login-box input:focus {
            border-color: #007bff;
            outline: none;
        }

        .login-box .message {
            font-size: 14px;
            text-align: center;
            color: #999;
        }

        .login-box .message a {
            color: #007bff;
            text-decoration: none;
        }

        .login-box .message a:hover {
            text-decoration: underline;
        }

        .error-message {
            color: #ff4d4d;
            font-size: 14px;
            text-align: center;
        }
    </style>

    <div class="login-box">
        <h3>Login to Your Account</h3>
        <asp:TextBox ID="txtUser" runat="server" placeholder="Username" CssClass="input-field"></asp:TextBox>
        <asp:TextBox ID="txtPass" runat="server" TextMode="Password" placeholder="Password" CssClass="input-field"></asp:TextBox>
        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-login" OnClick="btnLogin_Click" />
        
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message"></asp:Label>
        
        <div class="message">
            <p>Forgot your password? <a href="#">Click here</a></p>
        </div>
    </div>
</asp:Content>