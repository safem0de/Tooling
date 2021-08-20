<%@ Page Title="Master" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="NewItem.aspx.vb" Inherits="Tooling.NewItem" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>New Item (เพิ่มรายการ)</h3>
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
        <asp:Label ID="DwgNo" runat="server" Text="DwgNo" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtDwgNo" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="UnitPrice" runat="server" Text="UnitPrice" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtUnitPrice" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="VendorCode" runat="server" Text="VendorCode" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtVendorCode" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="MinStock" runat="server" Text="MinStock" CssClass="input-group-text"></asp:Label>
    </div>
        <asp:TextBox ID="TxtMinStock" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
    </div>

    <div class="input-group mb-3 mt-3 col-3">
    <div class="input-group-prepend">
        <asp:Label ID="Location" runat="server" Text="Location" CssClass="input-group-text"></asp:Label>
    </div>
        <%--<asp:TextBox ID="TxtLocation" runat="server" CssClass="form-control"></asp:TextBox>--%>
        <asp:DropDownList ID="DrpLocation" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>

    <div class="input-group mb-3 mt-3 col-12 justify-content-end">

        <asp:FileUpload ID="FileUpload" runat="server" CssClass="border rounded-left py-1"/>
        
        <div class="input-group-append">
            <asp:Button ID="BtnSelect" runat="server" Text="Select Sheet" class="btn btn-outline-secondary"/>
            <asp:DropDownList ID="DrpSheet" runat="server" ></asp:DropDownList>
            <asp:Button ID="BtnUpload" runat="server" Text="Upload" class="btn btn-outline-secondary" />
        </div>
    </div>

    <div class="input-group mb-3 mt-3 col-12 justify-content-end">
        <asp:TextBox ID="TxtSearch" runat="server" CssClass="form-control" placeholder="ไอเท็มโค้ด / สเปค / ตำแหน่งที่จัดเก็บ"></asp:TextBox>
        <asp:Button ID="BtnSearch" runat="server" Text="ค้นหา" cssclass="btn btn-outline-success" />
        &nbsp;<asp:Button ID="BtnAdd" runat="server" Text="เพิ่มรายการ" cssclass="btn btn-success"/>
        &nbsp;<asp:Button ID="BtnEdit" runat="server" Text="แก้ไขรายการ" cssclass="btn btn-primary"/>
        &nbsp;<asp:Button ID="BtnDelete" runat="server" Text="ล้างแบบฟอร์ม" cssclass="btn btn-danger"/>
    </div>

</div>
    
    <div class="alert alert-warning" role="alert" id="alertfalse" style="display:none;" >
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        <asp:Label ID="lblAlarm" runat="server" Text="<br/><br/>"></asp:Label>
    </div>

<div class="card">
  <div class="card-header">
    รายการทั้งหมด
  </div>

  <div class="card-body text-center">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
          <ContentTemplate>
              <asp:GridView ID="GridViewMaster" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" cssClass="col-12 mx-auto mb-3" AllowPaging="True">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:ButtonField CommandName="Select" HeaderText="Select" ShowHeader="True" Text="Select" ControlStyle-CssClass="btn btn-warning" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" ForeColor="White" Font-Bold="True" />
            <PagerSettings PageButtonCount="20" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#FFFF80" Font-Bold="True" ForeColor="Black" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
          </ContentTemplate>
      </asp:UpdatePanel>
        
      </div>
  </div>

    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
    <asp:Button ID="BtnPrintAll" runat="server" Text="Print All" CssClass="btn btn-outline-info"/>
    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" ShowBackButton="False" ShowExportControls="True" ShowFindControls="False" ShowRefreshButton="False" ShowZoomControl="False" Width="100%">
                    </rsweb:ReportViewer>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnSelect2Edit" runat="server" Text="Edit" class="btn btn-secondary" />
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
