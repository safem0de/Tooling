Imports System.Data.SqlClient

Public Class Recieve
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("CanAccess") Then
            Response.Redirect("~/Login.aspx")
        End If

        If Not (Session("Process") = "PURCHASE" Or Session("Process") = "ADMIN") Then
            Response.Redirect("~/Default.aspx")
        End If

        If Not Page.IsPostBack Then
            BindColumn(GridViewRecieve, AddColumn)
            TxtItemCode.Focus()
        End If
    End Sub
    Sub BindColumn(GV As GridView, dt As DataTable)
        Session("recieve") = dt
        GV.DataSource = dt
        GV.DataBind()
    End Sub
    Function AddColumn() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ItemCode")
        dt.Columns.Add("ItemName")
        dt.Columns.Add("Spec")
        dt.Columns.Add("Qty")
        dt.Columns.Add("LotNo")
        dt.Columns.Add("Status")
        dt.Rows.Add()
        Return dt
    End Function

    Sub Clearform()
        TxtItemCode.Text = ""
        TxtSpec.Text = ""
        TxtItemName.Text = ""
        TxtLotNo.Text = ""
        TxtQty.Text = 1
        TxtItemCode.Focus()
    End Sub

    Protected Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click

        Dim SqlCheck = "SELECT [ItemName]
              ,[Spec]
          FROM [Tooling_Mecha].[dbo].[Tooling_Master]
          WHERE [ItemCode] = '" & TxtItemCode.Text & "'"

        Dim x = StandardFunction.GetDataTable(SqlCheck)
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
            Next
        End If

        Dim SqlCheckLot = "SELECT [Recieve_details_id]
          FROM [Tooling_Mecha].[dbo].[Recieve_Tooling_Details]
          WHERE [LotNo] = '" & TxtLotNo.Text & "'"

        If StandardFunction.getSQLDataString(SqlCheckLot) <> "" Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('Lot ซ้ำ');", True)
            Exit Sub
        End If

        Dim dt As DataTable = Session("recieve")

        Dim IsDuplicate As Boolean = True
        For i As Integer = dt.Rows.Count - 1 To 0 Step -1
            If dt.Rows(i).Item("ItemCode").ToString.Equals(TxtItemCode.Text) And
                dt.Rows(i).Item("LotNo").ToString.Equals(TxtLotNo.Text) Then
                dt.Rows(i).Item("Qty") = CInt(dt.Rows(i).Item("Qty")) + CInt(TxtQty.Text)
                IsDuplicate = Not (IsDuplicate)
            End If
        Next

        If IsDuplicate Then
            dt.Rows.Add(TxtItemCode.Text, TxtItemName.Text, TxtSpec.Text, TxtQty.Text, TxtLotNo.Text, DrpStatus.Text)
        End If

        For i As Integer = dt.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dt.Rows(i)
            If row.Item(0) Is Nothing Then
                dt.Rows.Remove(row)
            ElseIf row.Item(0).ToString = "" Then
                dt.Rows.Remove(row)
            End If
        Next

        BindColumn(GridViewRecieve, dt)
        Clearform()

        TxtItemCode.Focus()
    End Sub

    Private Sub GridViewRecieve_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridViewRecieve.RowDeleting
        Dim deldt As DataTable = Session("recieve")

        If e.RowIndex >= 0 And GridViewRecieve.Rows.Count <> 1 Then
            deldt.Rows.RemoveAt(e.RowIndex)
            BindColumn(GridViewRecieve, deldt)
        Else
            BindColumn(GridViewRecieve, AddColumn())
        End If
    End Sub

    Protected Sub Confirm_Click(sender As Object, e As EventArgs) Handles Confirm.Click

        Dim dtc As DataTable = Session("Recieve")

        For i As Integer = dtc.Rows.Count - 1 To 0 Step -1
            Dim row As DataRow = dtc.Rows(i)
            If row.Item(0) Is Nothing Or row.Item(0).ToString = "" Then
                Exit Sub
            End If
        Next

        Dim StrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim Sql As String = "
        INSERT INTO [Tooling_Mecha].[dbo].[Recieve_Tooling]
                   ([Recieve_date]
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
        SELECT [Recieve_id]
        FROM [Tooling_Mecha].[dbo].[Recieve_Tooling]
        WHERE [Recieve_date] = '" & StrDate & "'
        AND [EmpNo] = '" & Session("User") & "'"

        Dim Recieve_Id As String = StandardFunction.getSQLDataString(Sql_Id)
        Session("Login") = CreateDatatable(Recieve_Id, StrDate, Session("User"), Session("Process"))

        Dim list As New ArrayList
        Dim Alert As String

        For Each i As DataRow In dtc.Rows
            If i.Item("ItemCode") Is Nothing Or i.Item("ItemCode").ToString = "" Then
                Alert = "ไม่พบรายการ"
            Else
                Dim bitStatus As Integer
                If i.Item("Status").ToString = "InActive" Then
                    bitStatus = 0
                Else
                    bitStatus = 1
                End If

                list.Add({i.Item("ItemCode").ToString,
                         i.Item("Qty").ToString,
                         i.Item("LotNo").ToString,
                         bitStatus})
            End If
        Next

        For Each j In list

            Dim SqlDetails As String =
            "INSERT INTO [Tooling_Mecha].[dbo].[Recieve_Tooling_Details]
                   ([Recieve_id]
                   ,[ItemCode]
                   ,[Qty]
                   ,[LotNo]
                   ,[Status])
             VALUES
                   (" & Recieve_Id & "
                   ,'" & j(0) & "'
                   ," & j(1) & "
                   ,'" & j(2) & "'
                   ," & j(3) & ")"

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
                'TextBox1.Text = SqlDetails
            End Try

        Next

        BindColumn(GridViewRecieve, AddColumn)
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