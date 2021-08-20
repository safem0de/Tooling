<%@ Page Title="StockControl" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StockControl.aspx.vb" Inherits="Tooling.StockControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Stock Control (ควบคุม Stock FIFO)</h3>
    <div class="form-inline">
        <div class="input-group my-3 col-3">
            <div class="input-group-prepend">
                <asp:Label ID="ItemCode" runat="server" Text="ItemCode" CssClass="input-group-text"></asp:Label>
            </div>
            <asp:TextBox ID="TxtItemCode" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="input-group my-3 col-3">
        <asp:Button ID="BtnSearch" runat="server" Text="ค้นหา" class="btn btn-primary"/>&nbsp;
        <asp:Button ID="BtnClear" runat="server" Text="ล้างฟอร์ม" class="btn btn-danger"/>
    </div>
    </div>

    <asp:GridView ID="GridViewStock" runat="server">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:CheckBox ID="ChkBoxStatus" runat="server" OnCheckedChanged="ChkBoxStatus_CheckedChanged" AutoPostBack="true"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <asp:Button ID="BtnDownload" runat="server" Text="Download" class="btn btn-outline-info"/>
</asp:Content>