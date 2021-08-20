Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click
        Dim SQLLoginPass = "SELECT [Process]
          FROM [Tooling_Mecha].[dbo].[UserLogin]
          WHERE [Password] = '" & StandardFunction.Encrypt(TxtPassword.Text) & "'
          AND [EmpNo] = '" & TxtEmpNo.Text.ToUpper() & "'"

        Dim SQLLoginRFID = "SELECT [Process]
          FROM [Tooling_Mecha].[dbo].[UserLogin]
          WHERE [Encrypt_RFID] = '" & StandardFunction.Encrypt(TxtPassword.Text) & "'
          AND [EmpNo] = '" & TxtEmpNo.Text.ToUpper() & "'"

        Dim Pass = StandardFunction.getSQLDataString(SQLLoginPass)
        Dim Rfid = StandardFunction.getSQLDataString(SQLLoginRFID)

        If Not Pass = "" Then
            Session("Process") = Pass
            Session("CanAccess") = True
            Session("User") = TxtEmpNo.Text.ToUpper()
            Session.Timeout = 30
        ElseIf Not Rfid = "" Then
            Session("Process") = Rfid
            Session("CanAccess") = True
            Session("User") = TxtEmpNo.Text.ToUpper()
            Session.Timeout = 30
        Else
            Exit Sub
        End If

        Response.Redirect("~/")
    End Sub
End Class