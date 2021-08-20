<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Inventory.aspx.vb" Inherits="Tooling.Inventory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="row my-3">
            <div class="col-sm-auto">
                <asp:Label ID="LblMonth" runat="server" Text="กรุณาเลือกเดือน"></asp:Label>
            </div>
            <div class="col-sm-auto">
                <asp:TextBox ID="TxtMonth" runat="server" TextMode="Month" class="form-control"></asp:TextBox>
            </div>
            <div class="col-sm-auto">
                <asp:Button ID="BtnDownloadInven" runat="server" Text="Download Excel" class="btn btn-info" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:GridView ID="GrdInventory" runat="server" CssClass="col-sm-auto text-center" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
