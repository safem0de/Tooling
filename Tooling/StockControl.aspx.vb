Imports System.Data.SqlClient

Public Class StockControl
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("CanAccess") Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not (Session("Process") = "PURCHASE" Or Session("Process") = "ADMIN") Then
            Response.Redirect("~/Default.aspx")
        End If

        If Not IsPostBack Then
            LoadTable()
        End If

    End Sub

    Sub LoadTable()
        Dim Sqldt = "
        SELECT t1.[Recieve_date]
		        ,[ItemCode]
		        ,[Qty]
		        ,[LotNo]
        FROM [Tooling_Mecha].[dbo].[Recieve_Tooling] as t1
        Left Join [Tooling_Mecha].[dbo].[Recieve_Tooling_Details] as t2
        On t1.[Recieve_id] = t2.[Recieve_id]
        WHERE [Status] = 0
        AND [ItemCode] like '" & TxtItemCode.Text & "%'
        "
        StandardFunction.fillDataToDataGrid(GridViewStock, Sqldt)
    End Sub

    Protected Sub ChkBoxStatus_CheckedChanged(sender As Object, e As EventArgs)
        Dim StrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim con As New SqlConnection
        Dim command As SqlCommand

        For Each row As GridViewRow In GridViewStock.Rows
            Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("ChkBoxStatus"), CheckBox)


            If chkRow.Checked Then
                'MsgBox(row.Cells(4).Text)

                Dim SqlUpdate As String = "
                UPDATE [Tooling_Mecha].[dbo].[Recieve_Tooling_Details]
                   SET [Status] = 1
                      ,[AvailableDate] = Convert(datetime,'" & StrDate & "')
                 WHERE [LotNo] = '" & row.Cells(4).Text & "'"

                Try
                    con.ConnectionString = StandardFunction.connectionString 'คำสั่งเชื่อม SQL จาก IP ไหน,Password อะไร'
                    con.Open()
                    Command = New SqlCommand(SqlUpdate, con)
                    Command.ExecuteNonQuery() 'คำสั่งเปิดใช้งาน การเชื่อมต่อ Sql'

                Catch ex As Exception
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ผิด');", True)
                Finally
                    con.Close()
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
                End Try
            End If
        Next

        LoadTable()
    End Sub

    Protected Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        LoadTable()
    End Sub

    Protected Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        TxtItemCode.Text = ""
        LoadTable()
    End Sub

    Protected Sub BtnDownload_Click(sender As Object, e As EventArgs) Handles BtnDownload.Click
        Dim NotShow = "
        SELECT t2.[Recieve_date]
	          ,t1.[ItemCode]
              ,[Qty]
              ,[LotNo]
              ,[Status]
              ,t2.[EmpNo]
          FROM [Tooling_Mecha].[dbo].[Recieve_Tooling_Details] as t1
          Left Join [Tooling_Mecha].[dbo].[Recieve_Tooling] as t2
          on t1.Recieve_id = t2.Recieve_id
          where [Status] = 0
        "
        Dim Show = "
            SELECT t2.[Recieve_date]
		      ,t1.[ItemCode]
              ,[Qty]
              ,[LotNo]
              ,[Status]
              ,[AvailableDate]
	          ,t2.[EmpNo]
          FROM [Tooling_Mecha].[dbo].[Recieve_Tooling_Details] as t1
            Left Join [Tooling_Mecha].[dbo].[Recieve_Tooling] as t2
          on t1.Recieve_id = t2.Recieve_id
          where Not [AvailableDate] Is null
        "

        Dim arr As New ArrayList
        arr.Add(NotShow)
        arr.Add(Show)
        StandardFunction.ExportExcelMultiSheet(Me, arr, "HistoryFIFO")
    End Sub
End Class