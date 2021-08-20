Imports System.Data.SqlClient

Public Class Location
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("CanAccess") Then
            Response.Redirect("~/Login.aspx")
        End If
    End Sub

    Protected Sub Add_Click(sender As Object, e As EventArgs) Handles Add.Click
        Dim check = True

        If TxtCabinetNo.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "
                        var el = document.getElementById('alertfalse');   
                        el.className = 'alert alert-danger';
                        el.innerHTML = 'กรุณาเลือกหมายเลข Cabinet';
                            $('#alertfalse').show();
                            setTimeout(
                                function(){
                                 el.parentNode.removeChild(el);
                                 },3000);", True)
            Exit Sub
        End If

        If IsNumeric(TxtCabinetNo.Text) And CInt(TxtCabinetNo.Text) <= 0 Then
            check = False
        End If

        If check = False Then

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "
                        var el = document.getElementById('alertfalse');   
                        el.className = 'alert alert-danger';
                        el.innerHTML = 'เลือกหมายเลข Cabinet ที่มากกว่า 0';
                            $('#alertfalse').show();
                            setTimeout(
                                function(){
                                 el.parentNode.removeChild(el);
                                 },3000);", True)
            Exit Sub
        Else
            Dim LocName = ""
            If CInt(TxtCabinetNo.Text) < 10 Then
                LocName = 0 & TxtCabinetNo.Text
            ElseIf CInt(TxtCabinetNo.Text) >= 10 Then
                LocName = TxtCabinetNo.Text
            End If

            Dim StrLocName = DrpCabinetName.Text & DrpCabinetLevel.Text & "-" & LocName

            Dim SqlInsLocation = "INSERT INTO [Tooling_Mecha].[dbo].[Location]
               ([LocationName]
               ,[Cabinet]
               ,[CabinetLevel]
               ,[CabinetNo])
                VALUES
               ('" & StrLocName & "'
               ,'" & DrpCabinetName.Text & "'
               ,'" & DrpCabinetName.Text & DrpCabinetLevel.Text & "'
               ," & TxtCabinetNo.Text & ")"

            Dim con As New SqlConnection
            Dim command As SqlCommand

            Try
                con.ConnectionString = StandardFunction.connectionString 'คำสั่งเชื่อม SQL จาก IP ไหน,Password อะไร'
                con.Open()
                command = New SqlCommand(SqlInsLocation, con)
                command.ExecuteNonQuery() 'คำสั่งเปิดใช้งาน การเชื่อมต่อ Sql'

            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "
                        var el = document.getElementById('alertfalse');    
                            el.className = 'alert alert-danger';
                            el.innerHTML = 'ข้อมูลซ้ำ (Duplicate Data)';
                            $('#alertfalse').show();
                            setTimeout(
                                function(){
                                 el.parentNode.removeChild(el);
                                 },3000);", True)

            Finally
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "
                        var el = document.getElementById('alertfalse');
                            el.className = 'alert alert-success';   
                            el.innerHTML = 'เพิ่มข้อมูลสำเร็จ (Completed)';
                            $('#alertfalse').show();
                            setTimeout(
                                function(){
                                 el.parentNode.removeChild(el);
                                 },3000);", True)
                con.Close()
            End Try
            TxtCabinetNo.Text = ""
        End If
    End Sub
End Class