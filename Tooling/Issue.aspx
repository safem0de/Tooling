<%@ Page Title="Issue" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Issue.aspx.vb" Inherits="Tooling.Issue" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Tooling Issue (เบิกจ่าย)</h3>
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
        <asp:Label ID="Reason" runat="server" Text="เหตุผล" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:DropDownList ID="DrpReason" runat="server" CssClass="form-control">
            <asp:ListItem Value="Set Up">Set Up (เซทอัพ)</asp:ListItem>
            <asp:ListItem Value="Adjust">Adjust (ปรับสเปค)</asp:ListItem>
            <asp:ListItem Value="Tool broken">Broken (ชำรุด)</asp:ListItem>
            <asp:ListItem Value="Usage">Cover Usage (ครบกำหนดการใช้)</asp:ListItem>
        </asp:DropDownList>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
        <asp:Button ID="BtnAdd" runat="server" Text="เพิ่มรายการ" cssclass="btn btn-primary"/>
    </div>

</div>
    
  <div class="card">
  <div class="card-header">
    รายการเบิกจ่าย
  </div>
  <div class="card-body text-center">
        <asp:GridView ID="GridViewIssue" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" cssClass="col-12 mx-auto mb-3" >
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:ButtonField CommandName="Delete" HeaderText="Delete" ShowHeader="True" Text="ลบ" ControlStyle-CssClass="btn btn-danger">
                <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                </asp:ButtonField>
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" ForeColor="White" Font-Bold="True" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
        <asp:Button ID="Confirm" runat="server" Text="ยืนยันการเบิก" cssclass="btn btn-success"/>
      </div>
  </div>

  <div class="card mt-3">
  <div class="card-header">
    พิมพ์รายการเบิกจ่าย
  </div>
  <div class="card-body text-center">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" width="100%"></rsweb:ReportViewer>
  </div>
  </div>
    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>
</asp:Content>
