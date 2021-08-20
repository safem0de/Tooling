<%@ Page Title="Login" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="Tooling.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>เข้าสู่ระบบ (Login)</h3>

    <div class="container">
        <div class="row">
            <div class="col-6">
                <div class="form-group">
                    <asp:Label ID="Login_EmpNo" runat="server" Text="รหัสพนักงาน"></asp:Label>
                    <asp:TextBox ID="TxtEmpNo" runat="server" CssClass="form-control" placeholder="Employee No"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="Login_Password" runat="server" Text="รหัสผ่าน"></asp:Label>
                    <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password"></asp:TextBox>
                </div>
                <asp:Button ID="BtnLogin" runat="server" Text="เข้าสู่ระบบ | Login" CssClass="btn btn-success" />
            </div>
        </div>
    </div>

</asp:Content>
