<%@ Page Title="Recieve" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Recieve.aspx.vb" Inherits="Tooling.Recieve" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Tooling Recieve (รับ)</h3>
    <div class="form-inline">
    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="ItemCode" runat="server" Text="ItemCode" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtItemCode" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="ItemName" runat="server" Text="ItemName" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtItemName" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="Spec" runat="server" Text="Spec" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtSpec" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="Qty" runat="server" Text="จำนวน" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtQty" runat="server" CssClass="form-control" TextMode="Number" Text="1"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="LotNo" runat="server" Text="LotNo" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtLotNo" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="Status" runat="server" Text="Status" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:DropDownList ID="DrpStatus" runat="server" CssClass="form-control">
            <asp:ListItem Value="InActive">In-Active(มี stock)</asp:ListItem>
            <asp:ListItem Value="Active">Active(พร้อมจ่าย)</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
        <asp:Button ID="BtnAdd" runat="server" Text="เพิ่มรายการ" cssclass="btn btn-primary"/>
    </div>

</div>
    
  <div class="card">
  <div class="card-header">
    รายการรับ
  </div>
  <div class="card-body text-center">
        <asp:GridView ID="GridViewRecieve" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" cssClass="col-12 mx-auto mb-3">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:ButtonField CommandName="Delete" HeaderText="Delete" ShowHeader="True" Text="ลบ" ControlStyle-CssClass="btn btn-danger">
                <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                </asp:ButtonField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
        <asp:Button ID="Confirm" runat="server" Text="ยืนยันการรับ" cssclass="btn btn-success"/>
      </div>
  </div>

    <%--<asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>--%>
</asp:Content>
