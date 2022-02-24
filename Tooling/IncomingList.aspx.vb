Imports Microsoft.Reporting.WebForms

Public Class IncomingList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("CanAccess") Then
            If Not (Session("Process") = "PURCHASE" Or Session("Process") = "ADMIN") Then
                Session("LastPage") = "~" & Request.RawUrl
                Response.Redirect("~/Login.aspx")
            End If
        End If

        LoadPage()
    End Sub

    Sub LoadPage()
        Dim SqlShowGrd = "
        SELECT FORMAT([Recieve_Date],'yyyy-MM-dd HH:mm:ss') as [Recieve_Date]
              ,[Request_By]
              ,[Remark]
          FROM [Tooling_Mecha].[dbo].[Incoming_Tooling]
        "
        StandardFunction.fillDataTableToDataGrid(GrdPrint, SqlShowGrd, "-")
    End Sub

    Private Sub GrdPrint_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdPrint.SelectedIndexChanged
        Dim row As GridViewRow = GrdPrint.SelectedRow
        'MsgBox(row.Cells(1).Text)
        'MsgBox(row.Cells(2).Text)
        'MsgBox(row.Cells(3).Text)
        Dim Sqlgetdt = "
            DECLARE @idz int;

            SELECT @idz=[id_Req]
              FROM [Tooling_Mecha].[dbo].[Incoming_Tooling]
            WHERE [Recieve_Date] = '" & row.Cells(1).Text & "'
            AND	[Request_By] = '" & row.Cells(2).Text & "'
            AND [Remark] = '" & row.Cells(3).Text.Replace("&nbsp;", "") & "'

            SELECT [PartNo] as [ItemCode]
	              ,[ItemName]
	              ,[Spec]
                  ,[PO_No]
                  ,[Qty]
                  ,[Status]
	              ,[Request_By] as [RequestBy]
	              ,FORMAT([Recieve_Date],'yyyy-MM-dd') as [RecieveDate]
                  ,FORMAT([RequestDate],'yyyy-MM-dd') as [RequestDate]
                  --,[InspectionBy]
              FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] as t1
              LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
              ON t1.[PartNo] = t2.[ItemCode]
              LEFT JOIN [Tooling_Mecha].[dbo].[Incoming_Tooling] as t3
              ON t1.[id_Req] = t3.[id_Req]
              WHERE t1.[id_Req] = @idz
        "
        'TxtTest.Text = Sqlgetdt
        Dim x As DataTable = StandardFunction.GetDataTable(Sqlgetdt)

        CreateReport(ReportPrintTag, x, "DataSetTooling")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "$('#exampleModal').modal();", True)
    End Sub

    Sub CreateReport(targetReport As ReportViewer, dt As DataTable, DataSetName As String)

        targetReport.ProcessingMode = ProcessingMode.Local
        targetReport.LocalReport.ReportPath = Server.MapPath("~/Incoming_Tooling.rdlc")
        Dim ds As DataTable = dt
        Dim datasource As New ReportDataSource(DataSetName, ds)
        targetReport.LocalReport.DataSources.Clear()
        targetReport.LocalReport.DataSources.Add(datasource)

    End Sub
End Class