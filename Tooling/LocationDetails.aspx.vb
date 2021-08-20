Public Class LocationDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Request.Url.AbsolutePath.ToLower().Equals("/locationdetails") Then
                Response.Redirect("~/LocationLayout")
            End If

            Dim Sql_init = "
            With TableA as
            (
            select [ItemCode]
		            ,SUM(Qty) as 'Total'
            from [Tooling_Mecha].[dbo].[Issue_Tooling_Details]
            Group by [ItemCode]

            ),
            TableB as(

            select [ItemCode]
		            ,SUM(Qty) as 'Total'
            from [Tooling_Mecha].[dbo].[Recieve_Tooling_Details]
            where [Status] = 1
            Group by [ItemCode]
            ),
            TableC as(
            SELECT t2.[ItemCode]
			        ,[LocationName]
                    ,[CabinetNo]
                    ,IsNull([Spec],'empty') as [Spec]
                    ,(Isnull(t4.[Total],0) - Isnull(t3.[Total],0)) as [Qty]
                    FROM [Tooling_Mecha].[dbo].[Location] as t1 
                LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
                ON t1.LocationName = t2.Location
                LEFT JOIN TableA as t3
                ON t2.ItemCode = t3.ItemCode
                LEFT JOIN TableB as t4
                ON t2.ItemCode = t4.ItemCode
            )

        select	[ItemCode]
		        ,[LocationName]
		        ,[Spec]
		        ,[Qty]
        From TableC
        WHERE [LocationName] LIKE '" & Request.Url.AbsolutePath.ToLower().Replace("/locationdetails/", "") & "-%'
        order by [CabinetNo] asc
"

            Dim dsx As DataTable = StandardFunction.GetDataTable(Sql_init)

            Dim html As New StringBuilder()
            html.Append("<div class='row'>")

            For Each row As DataRow In dsx.Rows
                html.Append("<div class='col-sm-2'>")
                html.Append("<div Class='card mb-3'>")
                html.Append("<div Class='card-header'>")
                html.Append(row.Item("LocationName"))
                html.Append("</div>")
                html.Append("<div class='card-body'>")
                html.Append("<b>" & row.Item("ItemCode") & "</b>")
                html.Append("<h6 class='card-title'>" & row.Item("Spec") & "</h6>")
                html.Append("<p class='card-text'>" & row.Item("Qty") & " pcs.</p>")
                html.Append("</div>")
                html.Append("</div>")
                html.Append("</div>")
            Next

            html.Append("</div>")

            'Append the HTML string to Placeholder.
            PlaceHolder1.Controls.Add(New Literal() With {
                   .Text = html.ToString()
                 })

        End If
    End Sub

    'Function GetDataTable(x As String) As DataTable
    '    Dim dX As DataTable = StandardFunction.GetDataTable(x)
    '    dX.TableName = x
    '    Return dX
    'End Function

End Class