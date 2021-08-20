<%@ Page Title="History" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="History.aspx.vb" Inherits="Tooling.History" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>History (ประวัติการรับ-จ่าย)</h3>
    <div class="form-inline">
    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="ItemCode" runat="server" Text="ItemCode" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtItemCode" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="StartDate" runat="server" Text="StartDate" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="EndDate" runat="server" Text="EndDate" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="Status" runat="server" Text="สถานะ" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:DropDownList ID="DrpStatus" runat="server" CssClass="form-control">
            <asp:ListItem Value="">All (ทั้งหมด)</asp:ListItem>
            <asp:ListItem Value="Recieve">Recieve (รับ)</asp:ListItem>
            <asp:ListItem Value="Issue">Issue (จ่าย)</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="input-group my-3 col-12 justify-content-end">
        <asp:Button ID="BtnSearch" runat="server" Text="ค้นหา" class="btn btn-primary"/>&nbsp;
        <asp:Button ID="BtnClear" runat="server" Text="ล้างฟอร์ม" class="btn btn-danger"/>&nbsp;
        <asp:Button ID="BtnDownload" runat="server" Text="ดาวน์โหลด" class="btn btn-success"/>
    </div>
</div>
    
  <div class="card">
  <div class="card-header">
    ประวัติการรับ-จ่าย
  </div>
  <div class="card-body text-center">
        <asp:GridView ID="GridViewHistory" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" cssClass="col-12 mx-auto mb-3" AllowPaging="True" PageSize="20">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" ForeColor="White" />
            <PagerSettings PageButtonCount="20" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Left" Font-Underline="True" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
      </div>
  </div>
</asp:Content>
