<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IncomingItems.aspx.vb" Inherits="Tooling.IncomingItems" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-4">
                <asp:LinkButton ID="LnkRecieve" runat="server" CssClass="btn btn-outline-warning col-12">
                    <span class="badge badge-secondary">1</span>
                    <h5 class="text-dark">รับงาน (Recieve Tooling)</h5>
                </asp:LinkButton>
            </div>
            <div class="col-md-4">
                <asp:LinkButton ID="LnkIncoming" runat="server" CssClass="btn btn-outline-warning col-12">
                    <span class="badge badge-secondary">2</span>
                    <h5 class="text-dark">กำลังดำเนินการ (Incoming Tooling)</h5>
                </asp:LinkButton>
            </div>
            <div class="col-md-4">
                <asp:LinkButton ID="LnkFinish" runat="server" CssClass="btn btn-outline-warning col-12">
                    <span class="badge badge-secondary">3</span>
                    <h5 class="text-dark">ตรวจสอบแล้ว (Finished)</h5>
                </asp:LinkButton>
            </div>
        </div>
    </div>

    <hr />

    <asp:Panel ID="PanelRecieve" runat="server">
        <asp:GridView ID="GrdRecieve" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="col-md-12 text-center" AllowPaging="False" PageSize="20">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Recieve" ControlStyle-CssClass="btn btn-outline-primary"/>
            </Columns>
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
    </asp:Panel>

    <asp:Panel ID="PanelIncoming" runat="server">
        <asp:GridView ID="GrdIncoming" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="col-md-12 text-center">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Incoming" ControlStyle-CssClass="btn btn-outline-primary"/>
            </Columns>
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
    </asp:Panel>

    <asp:Panel ID="PanelFinish" runat="server">
        <div class="row justify-content-center mb-3">
            <h4>
                History Searching <asp:Label ID="LblLotCount" runat="server" Text=""></asp:Label>
            </h4>
        </div>
        <div class="row justify-content-center mb-3">
            <div class="col-md-2 col-sm-3">
                <asp:TextBox ID="TxtMonth" runat="server" class="form-control col-12" TextMode="Month"></asp:TextBox>
            </div>
            <div class="col-md-4 col-sm-6">
                <asp:TextBox ID="TxtSearch" runat="server" class="form-control col-12" placeholder="ค้นหารายการ ItemCode, Spec, ItemName, P/O"></asp:TextBox>
            </div>
            <div class="col-md-2 col-sm-3">
                <asp:Button ID="BtnSearch" runat="server" Text="Search" class="btn btn-info"/>
            </div>
        </div>
        <asp:GridView ID="GrdFinish" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
            CssClass="col-md-12 text-center" Width="100%" PageSize="10" AllowPaging="True">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Received" ControlStyle-CssClass="btn btn-outline-info btn-sm"/>
                <asp:ButtonField ButtonType="Button" CommandName="Edit" Text="Edit" ControlStyle-CssClass="btn btn-outline-warning btn-sm"/>
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Left" />
            <RowStyle BackColor="#EFF3FB"/>
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
        <br />
        <asp:Button ID="BtnDownload" runat="server" Text="Download Excel" class="btn btn-info"/>
        <asp:Button ID="BtnForJpn" runat="server" Text="Download Report" class="btn btn-secondary"/>
    </asp:Panel>

    <asp:TextBox ID="TxtTest" runat="server" TextMode="MultiLine"></asp:TextBox>
</asp:Content>
