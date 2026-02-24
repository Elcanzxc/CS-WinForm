<%@ Page Title="Calculator" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true" CodeBehind="Calculator.aspx.cs" Inherits="HomeWorkASP.Calculator" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Simple Calculator</h2>
    
   
    <asp:TextBox ID="num1" runat="server" type="number"></asp:TextBox>
    
 
    <span> + </span>
    <asp:Button ID="btnAdd" runat="server" Text="+" OnClick="btnOperation_Click" />
    <span> - </span>
    <asp:Button ID="btnSubtract" runat="server" Text="-" OnClick="btnOperation_Click" />
    <span> * </span>
    <asp:Button ID="btnMultiply" runat="server" Text="*" OnClick="btnOperation_Click" />
    <span> / </span>
    <asp:Button ID="btnDivide" runat="server" Text="/" OnClick="btnOperation_Click" />
    
    
    <asp:TextBox ID="num2" runat="server" type="number"></asp:TextBox>
    
  
    <asp:Button ID="btnCalc" runat="server" Text="=" OnClick="btnCalc_Click" />
    
  
    <asp:Label ID="lblResult" runat="server" Font-Bold="true"></asp:Label>
</asp:Content>