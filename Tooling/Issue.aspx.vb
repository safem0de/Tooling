Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Microsoft.Reporting.WebForms

Public Class Issue
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("CanAccess") Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not Page.IsPostBack Then
            BindColumn(GridViewIssue, AddColumn)
            TxtItemCode.Focus()
        End If
    End Sub

    Sub Clearform()
        TxtItemCode.Text = ""
        TxtItemName.Text = ""
        TxtSpec.Text = ""
        TxtQty.Text = 1
        TxtItemCode.Focus()
    End Sub
    Sub BindColumn(GV As GridView, dt As DataTable)
        Session("issue") = dt
        GV.DataSource = dt
        GV.DataBind()
    End Sub
    Function AddColumn() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ItemCode")
        dt.Columns.Add("ItemName")
        dt.Columns.Add("Spec")
        dt.Columns.Add("Qty")
        'dt.Columns.Add("LotNo")
        dt.Columns.Add("Reason")
        dt.Columns.Add("EmpNo")
        dt.Rows.Add()
        Return dt
    End Function
    Protected Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click

        Dim SqlCheck = "
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
					,t2.[ItemName]
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

        select	[ItemName]
		        ,[Spec]
		        ,[Qty] as [Available]
        From TableC
		Where [ItemCode] = '" & TxtItemCode.Text & "'" 'เช็คจาก itemcode แล้ว ด้านล่างเลยให้ check itemName กับ Spec

        TextBox1.Text = SqlCheck
        Dim x = StandardFunction.GetDataTable(SqlCheck)
        Dim z As Integer = 0

        If x.Rows.Count = 0 Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('Barcode ผิด');", True)
            Exit Sub
        Else
            For Each rx As DataRow In x.Rows
                If TxtItemName.Text.Equals(rx.Item("ItemName")) And TxtSpec.Text.Equals(rx.Item("Spec")) Then
                Else
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('Barcode ผิด');", True)
                    Exit Sub
                End If

                If CInt(TxtQty.Text) <= CInt(rx.Item("Available")) And CInt(TxtQty.Text) > 0 Then
                    z = CInt(rx.Item("Available"))
                Else
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ไม่สามารถเบิก Tooling เกินจำนวน " & CInt(rx.Item("Available")) & " pcs ได้');", True)
                    Exit Sub
                End If
            Next
        End If

        Dim dt As DataTable = Session("issue")

        Dim IsDuplicate As Boolean = True
        'Dim LotIssue As String
        For i As Integer = dt.Rows.Count - 1 To 0 Step -1
            If dt.Rows(i).Item("ItemCode").ToString = TxtItemCode.Text Then
                If CInt(dt.Rows(i).Item("Qty")) + CInt(TxtQty.Text) > z Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ไม่สามารถเบิก Tooling เกินจำนวนได้');", True)
                    Exit Sub
                Else
                    dt.Rows(i).Item("Qty") = CInt(dt.Rows(i).Item("Qty")) + CInt(TxtQty.Text)
                    IsDuplicate = Not (IsDuplicate)
                End If
            End If
        Next

        If IsDuplicate Then
            dt.Rows.Add(TxtItemCode.Text, TxtItemName.Text, TxtSpec.Text, TxtQty.Text, DrpReason.Text, Session("User"))
        End If

        For i As Integer = dt.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dt.Rows(i)
            If row.Item(0) Is Nothing Then
                dt.Rows.Remove(row)
            ElseIf row.Item(0).ToString = "" Then
                dt.Rows.Remove(row)
            End If
        Next

        BindColumn(GridViewIssue, dt)
        Clearform()

        TxtItemCode.Focus()
    End Sub

    Private Sub GridViewIssue_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewIssue.RowDeleting
        Dim deldt As DataTable = Session("issue")

        If e.RowIndex >= 0 And GridViewIssue.Rows.Count <> 1 Then
            deldt.Rows.RemoveAt(e.RowIndex)
            BindColumn(GridViewIssue, deldt)
        Else
            BindColumn(GridViewIssue, AddColumn())
        End If
    End Sub

    Protected Sub Confirm_Click(sender As Object, e As EventArgs) Handles Confirm.Click
        Dim dtc As DataTable = Session("Issue")

        For i As Integer = dtc.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dtc.Rows(i)
            If row.Item(0) Is Nothing Or row.Item(0).ToString = "" Then
                Exit Sub
            End If
        Next

        Dim StrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim Sql As String = "
        INSERT INTO [Tooling_Mecha].[dbo].[Issue_Tooling]
                   ([Issue_date]
                   ,[EmpNo])
             VALUES
                   (Convert(datetime,'" & StrDate & "')
                   ,'" & Session("User") & "')"

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString 'คำสั่งเชื่อม SQL จาก IP ไหน,Password อะไร'
            con.Open()
            command = New SqlCommand(Sql, con)
            command.ExecuteNonQuery() 'คำสั่งเปิดใช้งาน การเชื่อมต่อ Sql'

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ผิด');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
        End Try

        Dim Sql_Id As String = "
        SELECT [Issue_id]
        FROM [Tooling_Mecha].[dbo].[Issue_Tooling]
        WHERE [Issue_date] = '" & StrDate & "'
        AND [EmpNo] = '" & Session("User") & "'"

        Dim Issue_Id As String = StandardFunction.getSQLDataString(Sql_Id)
        Session("Login") = CreateDatatable(Issue_Id, StrDate, Session("User"), Session("Process"))

        Dim list As New ArrayList
        Dim Alert As String

        For Each i As DataRow In dtc.Rows
            If i.Item("ItemCode") Is Nothing Or i.Item("ItemCode").ToString = "" Then
                Alert = "ไม่พบรายการ"
            Else
                list.Add({i.Item("ItemCode").ToString,
                         i.Item("Qty").ToString,
                        i.Item("Reason").ToString})
            End If
        Next

        For Each j In list

            Dim SqlDetails As String =
            "INSERT INTO [Tooling_Mecha].[dbo].[Issue_Tooling_Details]
                   ([Issue_id]
                   ,[ItemCode]
                   ,[Qty]
                   ,[Reason])
             VALUES
                   (" & Issue_Id & "
                   ,'" & j(0) & "'
                   ," & j(1) & "
                   ,'" & j(2) & "')"

            Try
                con.ConnectionString = StandardFunction.connectionString 'คำสั่งเชื่อม SQL จาก IP ไหน,Password อะไร'
                con.Open()
                command = New SqlCommand(SqlDetails, con)
                command.ExecuteNonQuery() 'คำสั่งเปิดใช้งาน การเชื่อมต่อ Sql'

            Catch ex As Exception
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ผิด');", True)
            Finally
                con.Close()
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
            End Try
        Next

        RefreshReport()
        BindColumn(GridViewIssue, AddColumn)
    End Sub

    Sub RefreshReport()

        ReportViewer1.ProcessingMode = ProcessingMode.Local
        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report.rdlc")
        Dim ds As DataTable = Session("Issue")
        Dim ds1 As DataTable = Session("Login")
        Dim datasource As New ReportDataSource("PrintIssue", ds)
        Dim datasource1 As New ReportDataSource("PrintIssueDetails", ds1)
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.DataSources.Add(datasource)
        ReportViewer1.LocalReport.DataSources.Add(datasource1)

    End Sub

    Function CreateDatatable(id As String, datety As String, EmpNo As String, Process As String)
        Dim dt As New DataTable
        dt.Columns.Add("Id")
        dt.Columns.Add("Date")
        dt.Columns.Add("EmpNo")
        dt.Columns.Add("Process")
        dt.Rows.Add(id, datety, EmpNo, Process)
        Return dt
    End Function

End Class