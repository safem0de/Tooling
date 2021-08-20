<%@ Page Title="About" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.vb" Inherits="Tooling.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %> Tooling Website</h2>
    <hr />
    <h5>V.1.0.0 (17 August 2020)</h5>
    <p><b>1. HomePage</b> : Show All Issue Graph Daily Process/Qty</p>
    <p><b>2. New Item (เพิ่มรายการ)</b><br />
        &emsp;2.1 สามารถ เพิ่มรายการ แก้ไข ได้เฉพาะผู้ที่ทำงานในหน่วยงาน Purchase เท่านั้น<br />
        &emsp;2.2 สามารถ ค้นหา ItemCode,Spec,Location ได้
        <div class="row">
            <input type="text" placeholder="ไอเท็ม / สเปค / ตำแหน่งที่จัดเก็บ " class="form-control" />
            <button class="btn btn-success">ค้นหา</button>
        </div>
        <br />
        &emsp;2.3 ตารางรายการทั้งหมด ระบุข้อมูลที่เกี่ยวข้องกับ Tooling ทั้งหมดที่ Purchase Section
        ทำการลงทะเบียน<br />
        &emsp;2.4 ปุ่ม <button class="btn btn-warning">Select</button> ด้านหน้าตารางสามารถ เรียกดู QR-Code Tooling ที่ประกอบด้วยข้อมูล ItemCode, ItemName, Spec
        สามารถบันทึกเป็น Pdf,Excel,Word และ Print ได้<br />
        &emsp;2.5 ปุ่ม <button class="btn btn-info">Print All</button> ซึ่งอยู่ด้านล่าง สามารถพิมพ์หลายรายการ หรือ บันทึกข้อมูลหลายรายการพร้อมกันได้<br />
    </p>
    <p><b>3. Stock (Tooling ในคลัง)</b><br />
        &emsp;3.1 พร้อมจ่าย (Active)<br />
        &emsp;&emsp;3.1.1
        <a class="btn btn-danger" role="button">
                <i class="fa fa-fire" aria-hidden="true"></i>
                Urgent to Order
            <span class="badge badge-light">จำนวนไอเท็มที่ต้องรีบสั่งและรับเข้าโดยด่วน</span>
            <span class="sr-only">unread messages</span> items
        </a> เมื่อนำ mouse ชี้ จำนวนไอเท็มจะปรากฏค่าใช้จ่ายทั้งหมดที่ต้องรีบสั่งและรับเข้าโดยด่วน
        <br />
        &emsp;&emsp;3.1.2
        <a class="btn btn-warning" role="button">
            <i class="fa fa-truck" aria-hidden="true"></i>
            Need to Order
            <span class="badge badge-light">จำนวนไอเท็มที่ต้องรีบสั่งซื้อ</span>
            <span class="sr-only">unread messages</span> items
        </a> เมื่อนำ mouse ชี้ จำนวนไอเท็มจะปรากฏค่าใช้จ่ายในการซื้อทั้งหมดที่ต้องรีบสั่งซื้อ
        <br />
        &emsp;&emsp;3.1.3
        <a class="btn btn-success" href="#" role="button">
            <i class="fa fa-check-circle" aria-hidden="true"></i>
            Regular
            <span class="badge badge-light">จำนวนไอเท็มที่มี stock อยู่ในระดับปกติ</span>
            <span class="sr-only">unread messages</span> items
        </a>
        <br />
        &emsp;&emsp;3.1.4 ปุ่ม
        <button class="btn btn-info">Download Excel</button>
        ใช้สำหรับการดาว์นโหลดไฟล์ Excel เกี่ยวกับ ชื่อ ราคา สถานะการสั่งซื้อที่ควรจะเป็น สถานะความถี่การใช้งาน เทียบต่อปี<br />
        &emsp;3.2 มีอยู่ในคลัง Control FIFO (In-Active) : เพื่อควบคุมให้เกิดการใช้ Tooling ที่รับเข้ามาก่อนได้ถูกใช้ก่อน<br />
    </p>
    <p><b>4. การรับ-จ่าย และประวัติการรับ-จ่าย</b><br />
        &emsp;4.1 การเบิก-จ่าย
        <br />
        &emsp;4.2 การรับ
        <br />
        &emsp;4.3 ประวัติการรับ-จ่าย
        &emsp;&emsp;4.3.1 ค้นหาจาก ItemCode เพียงอย่างเดียว
        &emsp;&emsp;4.3.2 ค้นหาจาก ItemCode และ สถานะการรับ
        &emsp;&emsp;4.3.3 ค้นหาจาก ItemCode และ สถานะการจ่าย
        &emsp;&emsp;4.3.4 ค้นหาจาก ItemCode และ StartDate

        <br />
    </p>
    <p><b>5. ตำแหน่งการจัดเก็บ</b><br />
        &emsp;5.1 การเพิ่มตำแหน่งจัดเก็บ<br />
        &emsp;&emsp;เช่น ชื่อ Cabinet = <b>N</b>, ชั้น Cabinet = <b>5</b> , ช่องที่ <b>1</b> คือ N5-01<br />
        &emsp;5.2 แผนภาพตู้จัดเก็บ<br />
        <div class="col-3">
            <div class="card text-white bg-primary mb-3">
                <div class="card-header">Location : A</div>
                <div class="list-group list-group-flush">
                    <li class="list-group-item">
                        <a href="#">A4</a>
                        <span class="badge badge-pill badge-info float-right">3</span>
                    </li>
                    <li class="list-group-item">
                        <a href="#">A5</a>
                        <span class="badge badge-pill badge-info float-right">54</span>
                    </li>
                    <li class="list-group-item">
                        <a href="#">A6</a>
                        <span class="badge badge-pill badge-info float-right">54</span>
                    </li>
                </div>
            </div>
        </div>
        <br />
        &emsp;5.3 แผนภาพภายในชั้นที่จัดเก็บ<br />
        <div class="row">
            <div class="col-sm-2">
                <div class="card mb-3">
                    <div class="card-header">A4-01</div>
                    <div class="card-body">
                        <b>ไอเท็มโค้ด</b>
                        <h6 class="card-title">สเปค</h6>
                        <p class="card-text">จำนวน pcs.</p>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <div class="card mb-3">
                    <div class="card-header">A4-02 (ตัวอย่าง)</div>
                    <div class="card-body">
                        <b>N1234XXXXR</b>
                        <h6 class="card-title">M3*0.5</h6>
                        <p class="card-text">55 pcs.</p>
                    </div>
                </div>
            </div>
                <div class="col-sm-2">
                    <div class="card mb-3">
                        <div class="card-header">A4-03 (ว่าง)</div>
                        <div class="card-body">
                            <b></b>
                            <h6 class="card-title">empty</h6>
                            <p class="card-text">0 pcs.</p>
                        </div>
                    </div>
                </div>
            </div>
    </p>

    <hr />
    <h5>V.1.0.1 (25 August 2020)</h5>
    <p>
        <b>1.แก้ไขบัคเรื่อง การนับ Stock การเบิกจ่าย</b><br />
        &emsp;- เนื่องจากไม่ได้นำ Stock การจ่ายมาหักออก Stock ที่รับ (ป้องกันการเบิกเกินจำนวน)
    </p>

    <hr />
    <h5>V.1.0.2 (04 September 2020)</h5>
    <p>
        <b>1.เพิ่มรายการ (Register) M-32</b><br />
        <b>2.เพิ่มคอลัมน์ Process (History)</b><br />
    </p>
    <footer>D9302 : <a href="Contact.aspx">Contact</a></footer>
</asp:Content>
