<%@ Page Title="Registed Users" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UserList.aspx.vb" Inherits="Tooling.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>รายชื่อผู้ลงทะเบียน (Registered)</h4>

    <asp:GridView ID="GrdNameList" runat="server"></asp:GridView>
</asp:Content>
