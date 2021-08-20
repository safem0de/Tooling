Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Session("CanAccess") Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Login", "
                            document.getElementById('login').style.display = 'none';
                            document.getElementById('register').style.display = 'none';
                            ", True)
            LblWelcome.Text = "Welcome ;) " & Session("User") & " "
            LblWelcome.Font.Bold = True
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Logout", "
                            document.getElementById('logoutz').style.display = 'none';
                            ", True)
        End If
    End Sub

    Protected Sub Logout(ByVal sender As Object, ByVal e As EventArgs)
        Session.Clear()
        Response.Redirect("~/Login.aspx")
    End Sub
End Class