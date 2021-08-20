<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="Tooling._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style=" position:relative; ">
        <h3>Tooling Issue by Process</h3>
        <br />
        <canvas class="img-fluid border rounded" id="myChart" height="100vh"></canvas>
        <div style="position:absolute;"></div>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>วิธีการใช้งานเว็บเบิก/จ่าย Tooling</h2>
            <ul>
                <li>การเบิก Tooling</li>
                <li>การค้นหา Tooling</li>
                <li>แผนผังการจัดเก็บ Tooling</li>
                <li>อื่นๆ</li>
            </ul>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">ไปยัง WI &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>รายงาน สรุปยอดการเบิก</h2>

            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>รายละเอียดเกี่ยวกับ Website</h2>
            <p>
                รายละเอียดของการ Website Update แต่ละครั้ง
            </p>
            <p>
                <a class="btn btn-default" href="~/About" runat="server">Learn more &raquo;</a>
            </p>
        </div>
    </div>
    <%--<asp:GridView ID="GrdTest" runat="server"></asp:GridView>--%>
</asp:Content>
