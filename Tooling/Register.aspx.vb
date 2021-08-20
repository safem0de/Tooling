Imports System.Data.SqlClient

Public Class Register
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub BtnRegister_Click(sender As Object, e As EventArgs) Handles BtnRegister.Click
        Dim check As Boolean = True
        Dim alert As New StringBuilder("Pls input")

        If Txt_RegisterName.Text = "" Then
            alert.Append(" ชื่อ,")
            check = False
        End If

        If Txt_RegisterSurname.Text = "" Then
            alert.Append(" นามสกุล,")
            check = False
        End If

        If Txt_Regist_EmpNo.Text = "" Or (Len(Txt_Regist_EmpNo.Text) > 5 And Len(Txt_Regist_EmpNo.Text) < 4) Then
            alert.Append(" รหัสพนักงาน,")
            check = False
        End If

        If DrpProcess.Text = "" Then
            alert.Append(" process,")
            check = False
        End If

        If Not Txt_Regist_Password.Text.Equals(Txt_Regist_Conf_Password.Text) Then
            alert.Append(" รหัสผ่าน,")
            check = False
        End If

        If TxtRfid.Text = "" Or Len(TxtRfid.Text) <> 10 Then
            alert.Append(" บัตรพนักงาน,")
            check = False
        End If

        If check = False Then
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & alert.ToString.Substring(0, alert.Length - 1) & "');", True)
        Else
            Dim con As New SqlConnection
            Dim command As SqlCommand

            Dim SqlRegister = "INSERT INTO [Tooling_Mecha].[dbo].[UserLogin]
                   ([EmpNo]
                   ,[Name]
                   ,[Surname]
                   ,[Process]
                   ,[Password]
                   ,[RFID]
                   ,[Encrypt_RFID])
             VALUES
                   ('" & Txt_Regist_EmpNo.Text.ToUpper() & "'
                   ,'" & Txt_RegisterName.Text & "'
                   ,'" & Txt_RegisterSurname.Text & "'
                   ,'" & DrpProcess.Text.ToUpper() & "'
                   ,'" & StandardFunction.Encrypt(Txt_Regist_Password.Text) & "'
                   ,'" & TxtRfid.Text & "'
                   ,'" & StandardFunction.Encrypt(TxtRfid.Text) & "')"

            Try
                ' พยายามลองทำคำสั่งในนี้
                con.ConnectionString = StandardFunction.connectionString
                con.Open()
                command = New SqlCommand(SqlRegister, con)
                command.ExecuteNonQuery()
            Catch ex As Exception
                ' Error/fail แล้วทำตรงนี้
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลผิดพลาด');", True)
            Finally
                'ไม่ว่าจะ Error หรือ OK ก็ต้องเข้า loop นี้
                Clearform()
                con.Close()
                Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('ลงทะเบียนสำเร็จ (Completed)');", True)
            End Try
        End If
    End Sub

    Sub Clearform()
        Txt_Regist_EmpNo.Text = ""
        Txt_RegisterName.Text = ""
        Txt_RegisterSurname.Text = ""
        DrpProcess.SelectedIndex = -1
    End Sub
End Class