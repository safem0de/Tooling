<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.vb" Inherits="Tooling.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>ลงทะเบียน (Register)</h3>
    <div class="container">
        <div class="row">
            <div class="col-6">
                <div class="d-flex flex-row">
                    <div class="form-group m-2">
                        <asp:Label ID="RegisterName" runat="server" Text="ชื่อ"></asp:Label>
                        <asp:TextBox ID="Txt_RegisterName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group m-2">
                        <asp:Label ID="RegisterSurname" runat="server" Text="นามสกุล"></asp:Label>
                        <asp:TextBox ID="Txt_RegisterSurname" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="d-flex flex-row">
                    <div class="form-group m-2">
                        <asp:Label ID="Regist_EmpNo" runat="server" Text="รหัสพนักงาน"></asp:Label>
                        <asp:TextBox ID="Txt_Regist_EmpNo" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group m-2">
                        <asp:Label ID="Regist_Process" runat="server" Text="Process"></asp:Label>
                        <asp:DropDownList ID="DrpProcess" runat="server" CssClass="form-control">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>PURCHASE</asp:ListItem>
                                <asp:ListItem>TORNOS &MITSUBISHI (M1)</asp:ListItem>
                                <asp:ListItem>L-16 (M1)</asp:ListItem>
                                <asp:ListItem>L-10 (M1)</asp:ListItem>
                                <asp:ListItem>HOBBING (M1)</asp:ListItem>
                                <asp:ListItem>B0205 (M1)</asp:ListItem>
                                <asp:ListItem>RL-20 (M1)</asp:ListItem>
                                <asp:ListItem>CUTTING & ESCO (M1)</asp:ListItem>
                                <asp:ListItem>NP-GL (M1)</asp:ListItem>
                                <asp:ListItem>TURNING MB50 (M1)</asp:ListItem>
                                <asp:ListItem>DEBURRIG,DILL&TAPPING (M1)</asp:ListItem>
                                <asp:ListItem>OCEAN (M1)</asp:ListItem>
                                <asp:ListItem>ROLLING (M1)</asp:ListItem>
                                <asp:ListItem>TURNING B-12,F16 (M1)</asp:ListItem>
                                <asp:ListItem>CGM (M1)</asp:ListItem>
                                <asp:ListItem>M/C CENTER (M1)</asp:ListItem>
                                <asp:ListItem>WASHING (M1)</asp:ListItem>
                                <asp:ListItem>BARREL (M1)</asp:ListItem>
                                <asp:ListItem>AUTO INSPECTION (M1)</asp:ListItem>
                                <asp:ListItem>TURNING WASING (M1)</asp:ListItem>
                                <asp:ListItem>SHAFT ASSY (M1)</asp:ListItem>
                                <asp:ListItem>MULTI SWISS (M1)</asp:ListItem>
                                <asp:ListItem>S.G (M1)</asp:ListItem>
                                <asp:ListItem>PRICKEL (M1)</asp:ListItem>
                                <asp:ListItem>I-CUT,D-CUT R-GRINDING (M1)</asp:ListItem>
                                <asp:ListItem>FACE GRINDING (M1)</asp:ListItem>
                                <asp:ListItem>CHAMFER (M1)</asp:ListItem>
                                <asp:ListItem>HEAT (M1)</asp:ListItem>
                                <asp:ListItem>INSPECTION (M1)</asp:ListItem>
                                <asp:ListItem>PACKING (M1)</asp:ListItem>
                                <asp:ListItem>QC (M1)</asp:ListItem>
                                <asp:ListItem>MM (M1)</asp:ListItem>
                                <asp:ListItem>ENGINEER TECH (M1)</asp:ListItem>
                                <asp:ListItem>ENGINEER TECH (M2)</asp:ListItem>
                                <asp:ListItem>STAFF (M1)</asp:ListItem>
                                <asp:ListItem>END BAR (M1)</asp:ListItem>
                                <asp:ListItem>M-32 (M1)</asp:ListItem>

                                <asp:ListItem>TM-AUTOMOTIVE (M2)</asp:ListItem>
                                <asp:ListItem>SR-MIYANO (M2)</asp:ListItem>
                                <asp:ListItem>OCEAN-CUTTING (M2)</asp:ListItem>
                                <asp:ListItem>HONING (M2)</asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="form-group m-2">
                    <asp:Label ID="Regist_Password" runat="server" Text="รหัสผ่าน"></asp:Label>
                    <asp:TextBox ID="Txt_Regist_Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                </div>
                <div class="form-group m-2">
                    <asp:Label ID="Regist_Conf_Password" runat="server" Text="ยืนยันรหัสผ่าน"></asp:Label>
                    <asp:TextBox ID="Txt_Regist_Conf_Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                </div>
                <div class="form-group m-2">
                    <asp:Label ID="Rfid_card" runat="server" Text="แตะบัตรพนักงาน"></asp:Label>
                    <asp:TextBox ID="TxtRfid" runat="server" CssClass="form-control" TextMode="Password" ></asp:TextBox>
                </div>

                <asp:Button ID="BtnRegister" runat="server" Text="ลงทะเบียน | Register" CssClass="btn btn-success" />
            </div>
        </div>
    </div>
</asp:Content>
