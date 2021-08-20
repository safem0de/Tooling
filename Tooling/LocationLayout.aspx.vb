Public Class LocationLayout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim Sql_init = "SELECT [Cabinet]
              FROM [Tooling_Mecha].[dbo].[Location]
              GROUP BY [Cabinet]
              ORDER BY [Cabinet] asc"

            Dim dsx As DataSetReport = GetFirstElement(Sql_init)
            Dim html As New StringBuilder()

            html.Append("<div class='row'>")
            For i = 0 To dsx.Tables.Count - 1

                If Len(dsx.Tables(i).TableName) = 1 Then
                    html.Append("<div class='col-sm-2'>")
                    html.Append("<div Class='card text-white bg-primary mb-3'>")
                    html.Append("<div Class='card-header'>")
                    html.Append("Location : " & dsx.Tables(i).TableName)
                    html.Append("</div>")
                    html.Append("<div class='list-group list-group-flush'>")
                    For Each row As DataRow In dsx.Tables(i).Rows

                        html.Append(
                            "<li class='list-group-item'>
                            <a href='LocationDetails/" & row.Item("CabinetLevel") & "'>
                            " & row.Item("CabinetLevel") & "</a>
                            <span class='badge badge-pill badge-info float-right'>" & row.Item("items") & "</span>
                            </li>"
                        )

                    Next
                    html.Append("</div>")
                    html.Append("</div>")
                    html.Append("</div>")
                End If

            Next
            html.Append("</div>")

            PlaceHolder1.Controls.Add(New Literal() With {
               .Text = html.ToString()
             })
        End If
    End Sub

    Function GetFirstElement(x As String) As DataSetReport

        Dim ds As New DataSetReport
        ds.Clear()
        Dim dt_init = StandardFunction.GetDataTable(x)

        For Each r As DataRow In dt_init.Rows
            ds.Tables.Add(GetDataFromTable(r(0)))
        Next

        Return ds
    End Function

    Function GetDataFromTable(x As String) As DataTable
        Dim SqlX = "SELECT [CabinetLevel]
              ,SUBSTRING([CabinetLevel], 2, 4) as [asc]
              ,COUNT([CabinetLevel]) as [items]
              FROM [Tooling_Mecha].[dbo].[Location]
              WHERE [Cabinet] = '" & x & "'
              GROUP BY [CabinetLevel]
              ORDER BY Convert(int,SUBSTRING([CabinetLevel], 2, 4)) asc"
        Dim dX As DataTable = StandardFunction.GetDataTable(SqlX)
        dX.TableName = x
        Return dX
    End Function
End Class