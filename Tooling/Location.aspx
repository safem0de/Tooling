<%@ Page Title="Location" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Location.aspx.vb" Inherits="Tooling.Location" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="alert alert-danger" role="alert" id="alertfalse" style="display:none;">
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        This is a danger alert—check it out!
        <%--<asp:Label ID="Lblalert" runat="server" Text="Label"></asp:Label>--%>
    </div>

    <h3>ตำแหน่งของ Tooling (Location)</h3>
    <div class="col-4">
    <div class="form-group">
        <asp:Label ID="CabinetName" runat="server" Text="Cabinet Name"></asp:Label>
        <asp:DropDownList ID="DrpCabinetName" runat="server" CssClass="form-control">
            <asp:ListItem Value="A"></asp:ListItem>
            <asp:ListItem Value="B"></asp:ListItem>
            <asp:ListItem Value="C"></asp:ListItem>
            <asp:ListItem Value="D"></asp:ListItem>
            <asp:ListItem Value="E"></asp:ListItem>
            <asp:ListItem Value="F"></asp:ListItem>
            <asp:ListItem Value="G"></asp:ListItem>
            <asp:ListItem Value="H"></asp:ListItem>
            <asp:ListItem Value="I"></asp:ListItem>
            <asp:ListItem Value="X"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="CabinetLevel" runat="server" Text="Cabinet Level"></asp:Label>
        <asp:DropDownList ID="DrpCabinetLevel" runat="server" CssClass="form-control">
            <asp:ListItem Value="1"></asp:ListItem>
            <asp:ListItem Value="2"></asp:ListItem>
            <asp:ListItem Value="3"></asp:ListItem>
            <asp:ListItem Value="4"></asp:ListItem>
            <asp:ListItem Value="5"></asp:ListItem>
            <asp:ListItem Value="6"></asp:ListItem>
            <asp:ListItem Value="7"></asp:ListItem>
            <asp:ListItem Value="8"></asp:ListItem>
            <asp:ListItem Value="9"></asp:ListItem>
            <asp:ListItem Value="10"></asp:ListItem>
            <asp:ListItem Value="11"></asp:ListItem>
            <asp:ListItem Value="12"></asp:ListItem>
            <asp:ListItem Value="13"></asp:ListItem>
            <asp:ListItem Value="14"></asp:ListItem>
            <asp:ListItem Value="15"></asp:ListItem>
            <asp:ListItem Value="16"></asp:ListItem>
            <asp:ListItem Value="17"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="form-group">
        <asp:Label ID="CabinetNo" runat="server" Text="Cabinet No."></asp:Label>
        <asp:TextBox ID="TxtCabinetNo" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
    </div>
    <asp:Button ID="Add" runat="server" Text="เพิ่ม Location" CssClass="btn btn-success"/>
    </div>
    
    <div class="col-8">

    </div>
    
</asp:Content>
