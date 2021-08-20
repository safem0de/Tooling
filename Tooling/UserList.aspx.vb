Public Class UserList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadPage()
        End If
    End Sub

    Sub LoadPage()
        Dim SqlNameList = "
        SELECT [EmpNo]
              ,CONCAT(UPPER(SUBSTRING([Name],1,1)),LOWER(SUBSTRING([Name],2,LEN([Name])))) as [Name]
              ,CONCAT(UPPER(SUBSTRING([Surname],1,1)),LOWER(SUBSTRING([Surname],2,LEN([Surname])))) as [Surname]
              ,[Process]
          FROM [Tooling_Mecha].[dbo].[UserLogin]
          ORDER by [Process] asc
        "
        StandardFunction.fillDataTableToDataGrid(GrdNameList, SqlNameList, "")
    End Sub
End Class