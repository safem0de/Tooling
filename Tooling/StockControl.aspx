<%@ Page Title="StockControl" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="StockControl.aspx.vb" Inherits="Tooling.StockControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Stock Control (ควบคุม Stock FIFO)</h3>

        <div class="form-inline">
            <div class="input-group my-3 col-auto">
                <div class="input-group-prepend">
                    <asp:Label ID="ItemCode" runat="server" Text="ItemCode" CssClass="input-group-text"></asp:Label>
                </div>
                <asp:TextBox ID="TxtItemCode" runat="server" CssClass="form-control"></asp:TextBox>
                <div class="input-group-append">
                    <asp:Button ID="BtnSearch" runat="server" Text="ค้นหา" class="btn btn-primary" />
                    <asp:Button ID="BtnClear" runat="server" Text="ล้างฟอร์ม" class="btn btn-danger" />
                </div>
            </div>
        </div>
        <hr />

    <div class="container-fluid">
        
        <div class="row">
            <div class="col-md-6">
                <h5>รายการที่รับมา (Old Recieved)</h5>
                <asp:GridView ID="GridViewStock" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="ChkBoxStatus" runat="server" OnCheckedChanged="ChkBoxStatus_CheckedChanged" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
            </div>

            <div class="col-md-6">
            <h5>รายการล่าสัตว์ที่รับมา (New Recieved)</h5>
            <asp:GridView ID="GridViewlatest" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkBoxLatest" runat="server" OnCheckedChanged="ChkBoxLatest_CheckedChanged" AutoPostBack="True"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
        </div>
        </div>

    </div>

    <br />
    <asp:Button ID="BtnDownload" runat="server" Text="Download" class="btn btn-outline-info" />

</asp:Content>