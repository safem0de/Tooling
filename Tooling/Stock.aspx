<%@ Page Title="Stock" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Stock.aspx.vb" Inherits="Tooling.Stock" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Stock Purchase</h3>
    <div class="row">
        <div>
            <a class="btn btn-danger" href="#" role="button">
                <i class="fa fa-fire" aria-hidden="true"></i>
                Urgent to Order
            <span class="badge badge-light">
            <asp:Label ID="LblUrgent" runat="server" Text="Label"></asp:Label></span>
            <span class="sr-only">unread messages</span> items
        </a>
        <a class="btn btn-warning" href="#" role="button">
            <i class="fa fa-truck" aria-hidden="true"></i>
            Need to Order
        <span class="badge badge-light">
            <asp:Label ID="LblNeedtoOrder" runat="server" Text="Label"></asp:Label></span>
            <span class="sr-only">unread messages</span> items
        </a>
        <a class="btn btn-success" href="#" role="button">
            <i class="fa fa-check-circle" aria-hidden="true"></i>
            Regular
            <span class="badge badge-light">
            <asp:Label ID="LblRegular" runat="server" Text="Label"></asp:Label></span>
            <span class="sr-only">unread messages</span> items
        </a>
        </div>
        
        <div class="ml-md-auto">
            <asp:Button ID="BtnDowloadExcel" runat="server" Text="Dowload Excel" CssClass="btn btn-info" />
        </div>
    </div>
    
    <br />
    <asp:GridView ID="GridViewStockReport" runat="server" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CellSpacing="2" ForeColor="Black">
        <FooterStyle BackColor="#CCCCCC" />
        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
        <RowStyle BackColor="White" />
        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#808080" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#383838" />
    </asp:GridView>
</asp:Content>
