<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IncomingDetails.aspx.vb" Inherits="Tooling.IncomingDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <div class="alert alert-warning alert-dismissible fade show" role="alert">
            <h3>Incoming Inspection Request</h3>
            <div class="row">
                <div class="col-md-4">
                    <div class="form-group row">
                        <asp:Label ID="LblPartNo" runat="server" Text="ItemCode." class="col-sm-4 col-form-label" Font-Bold="True"></asp:Label>
                        <asp:Label ID="LblDataPartNo" runat="server" Text="" class="col-sm-8 col-form-label"></asp:Label>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group row">
                        <asp:Label ID="LblSpec" runat="server" Text="Spec" class="col-sm-4 col-form-label" Font-Bold="True"></asp:Label>
                        <asp:Label ID="LblDataSpec" runat="server" Text="" class="col-sm-8 col-form-label"></asp:Label>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group row">
                        <asp:Label ID="LblName" runat="server" Text="Name" class="col-sm-4 col-form-label" Font-Bold="True"></asp:Label>
                        <asp:Label ID="LblDataName" runat="server" Text="" class="col-sm-8 col-form-label"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4">
                    <div class="form-group row">
                        <asp:Label ID="LblPONo" runat="server" Text="P/O No." class="col-sm-4 col-form-label" Font-Bold="True"></asp:Label>
                        <asp:Label ID="lblDataPO" runat="server" Text="" class="col-sm-8 col-form-label"></asp:Label>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group row">
                        <asp:Label ID="LblQty" runat="server" Text="Qty" class="col-sm-4 col-form-label" Font-Bold="True"></asp:Label>
                        <asp:Label ID="LblDataQty" runat="server" Text="" class="col-sm-8 col-form-label"></asp:Label>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="form-group row">
                        <asp:Label ID="LblRequestDate" runat="server" Text="RequestDate" class="col-sm-4 col-form-label" Font-Bold="True"></asp:Label>
                        <asp:Label ID="LblDataRequestDate" runat="server" Text="" class="col-sm-8 col-form-label"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-3">
                <div class="form-group row">
                    <label class="col-sm-4 col-form-label">NG</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtNGQty" runat="server" CssClass="form-control" TextMode="Number" Min="0" Text="0" required="true"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="form-group row">
                    <label class="col-sm-4 col-form-label">Working time</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtWorkMin" runat="server" CssClass="form-control" TextMode="Number" Min="0" Text="0" required="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-3">
                <div class="form-group row">
                    <label class="col-sm-4 col-form-label">OK 1 ด้าน</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtSide1" runat="server" CssClass="form-control" TextMode="Number" Min="0" Text="0" required="true"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="form-group row">
                    <label class="col-sm-4 col-form-label">OK 2 ด้าน</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtSide2" runat="server" CssClass="form-control" TextMode="Number" Min="0" Text="0" required="true"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="form-group row">
                    <label class="col-sm-4 col-form-label">OK 3 ด้าน</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtSide3" runat="server" CssClass="form-control" TextMode="Number" Min="0" Text="0" required="true"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="col-md-3">
                <div class="form-group row">
                    <label class="col-sm-4 col-form-label">OK 4 ด้าน</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="TxtSide4" runat="server" CssClass="form-control" TextMode="Number" Min="0" Text="0" required="true"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="form-group col-md-6">
                <label>Reject Case Remark</label>
                <asp:TextBox ID="TxtRejectRemark" runat="server" CssClass="form-control col-12" TextMode="MultiLine" Rows="3"></asp:TextBox>
            </div>
        </div>

        <asp:Button ID="BtnConfirm" runat="server" Text="บันทึกข้อมูลการตรวจเช็ค Tooling" class="btn btn-success"/>
        <asp:Button ID="BtnEdit" runat="server" Text="แก้ไขข้อมูลการตรวจเช็ค Tooling" class="btn btn-primary"/>

    </div>
    <asp:TextBox ID="TxtTest" runat="server" TextMode="MultiLine"></asp:TextBox>
</asp:Content>
