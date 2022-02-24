Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports Microsoft.Reporting.WebForms
Imports ZXing
Imports ZXing.Common

Public Class NewItem
    Inherits System.Web.UI.Page
    ReadOnly StrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("CanAccess") Then
            Session("LastPage") = "~" & Request.RawUrl
            Response.Redirect("~/Login.aspx")
        End If

        If Not (Session("Process") = "PURCHASE" Or Session("Process") = "ADMIN") Then
            BtnUpload.Enabled = False
            BtnAdd.Enabled = False
            BtnEdit.Enabled = False
        End If

        If Not Page.IsPostBack Then
            Dim SqlDrpLocation = "With TableA as(
            SELECT [LocationName]
		            ,SUBSTRING([CabinetLevel], 1, 1) as [Cabinet_Name]
		            ,SUBSTRING([CabinetLevel], 2, 4) as [Cabinet_lvl]
		            ,[CabinetNo]
            FROM [Tooling_Mecha].[dbo].[Location]
            )

            select [LocationName] from TableA
            ORDER BY [Cabinet_Name],Convert(int,[Cabinet_lvl]),[CabinetNo] asc"
            StandardFunction.setDropdownlist(DrpLocation, SqlDrpLocation, False)
        End If

        LoadTable()
        DrpLocation.AutoPostBack = False
    End Sub
    Sub LoadTable()
        Dim SqlLoadMaster As String = "
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

        SELECT tx.[ItemCode]
            ,[ItemName]
            ,tx.[Spec]
            ,[DwgNo]
            ,[UnitPrice]
            ,[VendorCode]
            ,[MinStock]
            ,[Location]
	        ,[Qty]
        FROM [Tooling_Mecha].[dbo].[Tooling_Master] as tx
        LEFT JOIN TableC as ty
        ON tx.[ItemCode] = ty.[ItemCode]
		WHERE tx.[ItemCode] like '" & TxtSearch.Text & "%'
		OR tx.[Spec] like '%" & TxtSearch.Text & "%'
		OR tx.[Location] like '" & TxtSearch.Text & "%'
"
        StandardFunction.fillDataToDataGrid(GridViewMaster, SqlLoadMaster)
    End Sub
    Private Sub GridViewMaster_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridViewMaster.PageIndexChanging
        GridViewMaster.PageIndex = e.NewPageIndex
        GridViewMaster.DataBind()
    End Sub

    Protected Sub GridViewMaster_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridViewMaster.SelectedIndexChanged
        Dim row As GridViewRow = GridViewMaster.SelectedRow
        Dim sendText As New ArrayList
        sendText.Add(row.Cells(1).Text)
        sendText.Add(row.Cells(2).Text)
        sendText.Add(row.Cells(3).Text.Replace("&quot;", """"))
        sendText.Add(row.Cells(4).Text.Replace("&nbsp;", ""))
        sendText.Add(row.Cells(5).Text)
        sendText.Add(row.Cells(6).Text.Replace("&nbsp;", ""))
        sendText.Add(row.Cells(7).Text)

        Session("Edit") = sendText

        Label1.Text = "Label of : " & row.Cells(2).Text
        Dim imbyte = CreateImgByte(row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text.Replace("&quot;", """"))
        Dim dtx = CreateDatatable(imbyte, row.Cells(1).Text, row.Cells(2).Text, row.Cells(3).Text.Replace("&quot;", """"), row.Cells(8).Text)
        CreateReport(ReportViewer1, dtx)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "$('#exampleModal').modal();", True)
    End Sub

    Protected Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Clearform()
    End Sub

    Sub Clearform()
        TxtItemCode.Text = ""
        TxtItemName.Text = ""
        TxtSpec.Text = ""
        TxtDwgNo.Text = ""
        TxtUnitPrice.Text = ""
        TxtVendorCode.Text = ""
        TxtMinStock.Text = ""
        GridViewMaster.SelectedIndex = -1
        TxtSearch.Text = ""
    End Sub

    Function CreateDatatable(Image As String, ItemCode As String, ItemName As String, Spec As String, Location As String)
        Dim dt As New DataTable
        dt.Columns.Add("Image")
        dt.Columns.Add("ItemCode")
        dt.Columns.Add("ItemName")
        dt.Columns.Add("Spec")
        dt.Columns.Add("Location")
        dt.Rows.Add(Image, ItemCode, ItemName, Spec, Location)
        Return dt
    End Function

    Function CreateImgByte(ItemCode As String, ItemName As String, Spec As String) As String

        Dim Writer As BarcodeWriter = New BarcodeWriter
        Writer.Format = BarcodeFormat.QR_CODE
        Dim A = New EncodingOptions
        A.Width = 150
        A.Height = 150
        Writer.Options = A

        Dim GetText As Bitmap = Writer.Write(ItemCode & vbTab & ItemName & vbTab & Spec & vbTab & vbTab & vbTab & vbNewLine)
        'GetText.Save(Server.MapPath(fileName), Imaging.ImageFormat.Jpeg)
        'Dim bytes As Byte() = File.ReadAllBytes(Server.MapPath(fileName))

        Dim stream = New MemoryStream
        GetText.Save(stream, Imaging.ImageFormat.Jpeg)
        Dim bytes As Byte() = stream.ToArray()
        stream.Close()
        Dim x = Convert.ToBase64String(bytes)
        Return x
    End Function

    Sub CreateReport(targetReport As ReportViewer, dt As DataTable)

        targetReport.ProcessingMode = ProcessingMode.Local
        targetReport.LocalReport.ReportPath = Server.MapPath("~/QReport.rdlc")
        Dim ds As DataTable = dt
        Dim datasource As New ReportDataSource("QrCode", ds)
        targetReport.LocalReport.DataSources.Clear()
        targetReport.LocalReport.DataSources.Add(datasource)

    End Sub

    Protected Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click

        Dim SqlInsert As String = "INSERT INTO [Tooling_Mecha].[dbo].[Tooling_Master]
           ([ItemCode]
           ,[ItemName]
           ,[Spec]
           ,[DwgNo]
           ,[UnitPrice]
           ,[VendorCode]
           ,[MinStock]
           ,[Location]
           ,[Add_date])
     VALUES
           ('" & TxtItemCode.Text.Trim() & "'
           ,'" & TxtItemName.Text.Trim() & "'
           ,'" & TxtSpec.Text.Replace(Chr(34), """").Replace("'", "''").Trim() & "'
           ,'" & TxtDwgNo.Text.Replace(Chr(34), """").Replace("'", "''").Trim() & "'
           ," & TxtUnitPrice.Text.Trim() & "
           ,'" & TxtVendorCode.Text.Trim() & "'
           ," & TxtMinStock.Text & "
           ,'" & DrpLocation.Text & "'
           ,Convert(datetime,'" & StrDate & "'))"

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString 'คำสั่งเชื่อม SQL จาก IP ไหน,Password อะไร'
            con.Open()
            command = New SqlCommand(SqlInsert, con)
            command.ExecuteNonQuery() 'คำสั่งเปิดใช้งาน การเชื่อมต่อ Sql'

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ผิด');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
        End Try

        TextBox1.Text = SqlInsert
        Clearform()
        LoadTable()
    End Sub

    Protected Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        Dim SqlUpdate As String = "
        UPDATE [Tooling_Mecha].[dbo].[Tooling_Master]
           SET [ItemName] = '" & TxtItemName.Text & "'
              ,[Spec] = '" & TxtSpec.Text.Replace(Chr(34), """").Replace("'", "''").Trim() & "'
              ,[DwgNo] = '" & TxtDwgNo.Text.Replace(Chr(34), """").Replace("'", "''").Trim() & "'
              ,[UnitPrice] = " & TxtUnitPrice.Text & "
              ,[VendorCode] = '" & TxtVendorCode.Text & "'
              ,[MinStock] = " & TxtMinStock.Text & "
              ,[Location] = '" & DrpLocation.Text & "'
         WHERE [ItemCode] = '" & TxtItemCode.Text & "'"

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString 'คำสั่งเชื่อม SQL จาก IP ไหน,Password อะไร'
            con.Open()
            command = New SqlCommand(SqlUpdate, con)
            command.ExecuteNonQuery() 'คำสั่งเปิดใช้งาน การเชื่อมต่อ Sql'

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ผิด'" & ex.Message & ");", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
        End Try

        Clearform()
        LoadTable()
    End Sub

    Protected Sub BtnSelect_Click(sender As Object, e As EventArgs) Handles BtnSelect.Click
        StandardFunction.getSheetExcel(Me, FileUpload, DrpSheet)
    End Sub

    Protected Sub BtnUpload_Click(sender As Object, e As EventArgs) Handles BtnUpload.Click

        Dim dtU As DataTable = StandardFunction.NewUploadXls(Me, GridViewMaster, DrpSheet)
        Dim Alert As New StringBuilder

        If IsNothing(dtU) Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('Format ผิด');", True)
        Else
            For i = 0 To dtU.Rows.Count - 1
                Dim sqlInsertString As String = "INSERT INTO [Tooling_Mecha].[dbo].[Tooling_Master]
               ([ItemCode]
               ,[ItemName]
               ,[Spec]
               ,[DwgNo]
               ,[UnitPrice]
               ,[VendorCode]
               ,[MinStock]
               ,[Location]
               ,[Add_date])
         VALUES
               ('" & dtU.Rows(i).Item("ItemCode").ToString.ToUpper.Replace("'", "").Trim() & "'
               ,'" & dtU.Rows(i).Item("ItemName").ToString.Replace("'", "").Trim() & "'
               ,'" & dtU.Rows(i).Item("Spec").ToString.Replace(Chr(34), """").Replace("'", "''").Trim() & "'
               ,'" & dtU.Rows(i).Item("DwgNo").ToString.Replace(Chr(34), """").Replace("'", "''").Trim() & "'
               ," & dtU.Rows(i).Item("UnitPrice").ToString.Replace("'", "").Trim() & "
               ,'" & dtU.Rows(i).Item("VendorCode").ToString.Replace("'", "").Trim() & "'
               ," & dtU.Rows(i).Item("MinStock").ToString.Replace("'", "").Trim() & "
               ,'" & dtU.Rows(i).Item("Location").ToString.Replace("'", "").Trim() & "'
               ,Convert(datetime,'" & StrDate & "'))"

                Dim con As New SqlConnection
                Dim command As SqlCommand

                Try

                    con.ConnectionString = StandardFunction.connectionString
                    con.Open()
                    command = New SqlCommand(sqlInsertString, con)
                    command.ExecuteNonQuery()

                Catch ex As Exception
                    TextBox1.Text += sqlInsertString & vbCrLf
                    Alert.Append(i & " : " & dtU.Rows(i).Item("ItemCode").ToString.ToUpper.Replace(Chr(34), """").Replace("'", "''").Trim() & " : " & ex.Message & "<br/>")
                Finally
                    con.Close()
                End Try
            Next
        End If

        If Alert.ToString() = "" Then
            'MsgBox("I'm OK")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "
                        var el = document.getElementById('alertfalse');
                            el.className = 'alert alert-success';   
                            el.innerHTML = 'เพิ่มข้อมูลสำเร็จ (Completed)';
                            $('#alertfalse').show();
                            setTimeout(
                                function(){
                                 el.parentNode.removeChild(el);
                                 },3000);", True)
        Else
            lblAlarm.Text = Alert.ToString()
            'MsgBox("I'm not OK")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "
                        var el = document.getElementById('alertfalse');
                            el.className = 'alert alert-danger';   
                            $('#alertfalse').show();", True)
        End If

        'TextBox1.Text = Alert.ToString
        DrpSheet.Items.Clear()
    End Sub

    Protected Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        LoadTable()
    End Sub

    Private Sub BtnSelect2Edit_Click(sender As Object, e As EventArgs) Handles BtnSelect2Edit.Click
        Dim x = Session("Edit")
        TxtItemCode.Text = x(0)
        TxtItemName.Text = x(1)
        TxtSpec.Text = x(2)
        TxtDwgNo.Text = x(3)
        TxtUnitPrice.Text = x(4)
        TxtVendorCode.Text = x(5)
        TxtMinStock.Text = x(6)
        Session("Edit") = Nothing
    End Sub

    Protected Sub BtnPrintAll_Click(sender As Object, e As EventArgs) Handles BtnPrintAll.Click
        Dim Sql = "
        SELECT [ItemCode]
		       ,[ItemName]
		       ,[Spec]
		       ,[Location]
	        FROM [Tooling_Mecha].[dbo].[Tooling_Master] as t1
        LEFT JOIN [Tooling_Mecha].[dbo].[Location] as t2
        ON t1.[Location]= t2.[LocationName]
            WHERE [Spec] like '%" & TxtSearch.Text & "%'
            OR [Location] like '" & TxtSearch.Text & "%'
            ORDER BY [CabinetNo] asc"
        Dim dtGet = StandardFunction.GetDataTable(Sql)

        dtGet.Columns.Add("Image")
        For i = 0 To dtGet.Rows.Count - 1
            Dim x = CreateImgByte(dtGet.Rows(i).Item("ItemCode").ToString(),
                          dtGet.Rows(i).Item("ItemName").ToString(),
                          dtGet.Rows(i).Item("Spec").ToString())
            dtGet.Rows(i).Item("Image") = x
        Next

        CreateReport(ReportViewer1, dtGet)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "myModal", "$('#exampleModal').modal();", True)
    End Sub
End Class