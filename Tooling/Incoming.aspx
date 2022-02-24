<%@ Page Title="Tooling Incoming" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Incoming.aspx.vb" Inherits="Tooling.Incoming" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h2>Incoming Input</h2>
        <div class="row">
            <div class="col-sm-3">
                <div class="form-group">
                    <asp:Label ID="LblPartNo" runat="server" Text="Part No."></asp:Label>
                    <asp:TextBox ID="TxtPartNo" runat="server" CssClass="form-control" required="True" TabIndex="1"></asp:TextBox>
                    <small class="form-text text-muted">กรุณาระบุ PartNo. เช่น N1212312121</small>
                    <input id="Text1" type="text" TabIndex="2" style="width:0; color:transparent; border:none; "/>
                    <input id="Text2" type="text" TabIndex="3" style="width:0; color:transparent; border:none; "/>
                    <input id="Text3" type="text" TabIndex="4" style="width:0; color:transparent; border:none; "/>
                    <input id="Text4" type="text" TabIndex="5" style="width:0; color:transparent; border:none; "/>
                    <asp:Button ID="BtnHidden" TabIndex="6" runat="server" Text="" style="width:0; color:transparent; border:none; cursor:initial;"/>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <asp:Label ID="LblPONo" runat="server" Text="P/O No."></asp:Label>
                    <asp:TextBox ID="TxtPONo" runat="server" CssClass="form-control" required="True" Enabled="false"></asp:TextBox>
                    <small class="form-text text-muted">กรุณาระบุ P/O No. เช่น M150xxx</small>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <asp:Label ID="LblQty" runat="server" Text="Q'ty"></asp:Label>
                    <asp:TextBox ID="TxtQty" runat="server" CssClass="form-control" required="True" TextMode="Number" Enabled="false"></asp:TextBox>
                    <small class="form-text text-muted">กรุณาระบุจำนวน. เช่น 9,999</small>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <asp:Label ID="LblStatus" runat="server" Text="Status"></asp:Label>
                    <asp:DropDownList ID="DrpStatus" runat="server" CssClass="form-control" required="True" Enabled="false">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Normal</asp:ListItem>
                        <asp:ListItem>Urgent</asp:ListItem>
                    </asp:DropDownList>
                    <small class="form-text text-muted">กรุณาระบุสถานะ. เช่น Urgent, Normal</small>
                </div>
            </div>
            <div class="col-sm-3">
                <div class="form-group">
                    <asp:Label ID="LblRequestDate" runat="server" Text="Request Date"></asp:Label>
                    <asp:TextBox ID="TxtRequestDate" runat="server" CssClass="form-control" required="True" TextMode="Date" Enabled="false"></asp:TextBox>
                    <small class="form-text text-muted">กรุณาระบุวันที่. เช่น 24/07/20xx</small>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-3">
                <asp:Button ID="BtnAdd" runat="server" Text="Add Item" class="btn btn-primary" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 mt-2">
                <div class="card">
                    <div class="card-header">รายการที่ต้องตรวจโดยฝ่ายคุณภาพ (Incoming Items)</div>
                    <div class="card-body">
                        <asp:GridView ID="GrdIncoming" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" CssClass="col-12 text-center">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:ButtonField ButtonType="Button" CommandName="Delete" Text="ลบ" ControlStyle-CssClass="btn btn-danger"/>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </div>
                    <div class="card-footer">
                        <div class="form-group">
                            <asp:Label ID="Remark" runat="server" Text="Remark"></asp:Label>
                            <asp:TextBox ID="TxtRemark" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            <small class="form-text text-muted">ระบุข้อความ เช่น PTD Rework, etc.</small>
                        </div>
                        <asp:Button ID="BtnConfirm" runat="server" Text="ยืนยันข้อมูลสำหรับ Incoming" class="btn btn-info" UseSubmitBehavior="false"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
