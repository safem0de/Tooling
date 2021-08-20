Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Data.OleDb
Imports System.IO
Imports System.Web.Script.Serialization
Imports ClosedXML.Excel

Public Class StandardFunction
    '10.124.128.141 : test IP
    'localhost : public IP
    Public Shared connectionString As String = "Data Source=10.121.1.85\SQLEXPRESS; User ID=sa;Password=sa@admin;Connection Timeout=50;"
    Public Shared Sub fillDataToDataGrid(targetDataGridView As GridView, sqlCommandStr As String)
        Dim ds As Data.DataSet = GetSQLDataSet(sqlCommandStr)
        If (ds.Tables.Count > 0) Then
            targetDataGridView.DataSource = ds
            targetDataGridView.DataBind()
        Else

        End If
    End Sub

    Public Shared Sub fillDataTableToDataGrid(Target As GridView, SqlString As String, insertStr As String)
        Dim dt As DataTable = GetDataTable(SqlString)
        If (dt.Rows.Count > 0) Then
            For i = 0 To dt.Rows.Count - 1
                For j = 0 To dt.Columns.Count - 1
                    If IsNothing(dt.Rows(i).Item(j).ToString) Then

                        dt.Rows(i).Item(j) = insertStr
                        'MsgBox(dt.Rows(i).Item(j).ToString)
                    End If
                Next
            Next
            Target.DataSource = dt
            Target.DataBind()
        Else

        End If
    End Sub

    Public Shared Function GetDataTable(ByVal sqlCommand As String) As DataTable
        Dim table As New DataTable
        Try
            Dim DBConnection As SqlConnection = New SqlConnection(connectionString)

            Dim command As New SqlCommand(sqlCommand, DBConnection)
            Dim adapter As SqlDataAdapter = New SqlDataAdapter()
            adapter.SelectCommand = command

            table.Locale = System.Globalization.CultureInfo.InvariantCulture

            adapter.Fill(table)
        Catch ex As Exception
            table = GetDataTable("Select 'No Data' as [NoData]")
        End Try

        Return table
    End Function

    Public Shared Function GetSQLDataSet(ByVal sqlCommandStr As String) As Data.DataSet
        Dim ds As New Data.DataSet()

        Try
            ' Connect to the database and run the query.
            Dim connection As New SqlConnection(connectionString)
            Dim adapter As New SqlDataAdapter(sqlCommandStr, connection)

            ' Fill the DataSet.
            adapter.Fill(ds)

        Catch ex As Exception
            ds = GetSQLDataSet("Select 'No Data' as [NoData]")
        End Try

        Return ds
    End Function

    Public Shared Function getSQLDataString(txtQry As String) As String
        'Set parameter
        Dim con As New SqlClient.SqlConnection
        Dim command As SqlCommand
        Dim returnString As String = ""

        Try
            'Get data from SQL to SQL_reader
            con.ConnectionString = connectionString
            con.Open()
            command = New SqlCommand(txtQry, con)
            Dim rdr As SqlDataReader = command.ExecuteReader

            'Add data from SQL_reader to cbo
            Do While rdr.Read()
                returnString = rdr(0)
            Loop
        Catch ex As Exception

        Finally
            'Close connection
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try
        Return returnString
    End Function

    Public Shared Sub setDropdownlist(targetDropdownlist As DropDownList, txtQry As String, addAll As Boolean)
        'Set parameter
        Dim con As New SqlClient.SqlConnection
        Dim command As SqlCommand
        Dim items As ArrayList = New ArrayList

        Try
            'Get data from SQL to SQL_reader
            con.ConnectionString = connectionString
            con.Open()
            command = New SqlCommand(txtQry, con)
            Dim rdr As SqlDataReader = command.ExecuteReader

            'Add All
            If addAll Then
                items.Add("")
            End If

            'Add data from SQL_reader to cbo
            Do While rdr.Read()
                items.Add(rdr(0))
            Loop

        Catch ex As Exception
        Finally
            'Close connection
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

        targetDropdownlist.DataSource = items
        targetDropdownlist.DataBind()

    End Sub

    Public Shared Function GenerateHash(ByVal TextToHash As String) As String
        Dim UTF8Encoder As New UTF8Encoding 'Creates a new instance of UTF8 Encoding
        Dim StringAsBytes() As Byte = UTF8Encoder.GetBytes(TextToHash) 'Converts the TextToHash value into its byte equivalent

        Dim MD5Provider As New MD5CryptoServiceProvider 'Creates a new instance of the MD5 cryptography class
        Dim ByteHash() As Byte = MD5Provider.ComputeHash(StringAsBytes) 'Generates the hash for StringAsBytes

        Return Convert.ToBase64String(ByteHash) 'Returns the ByteHash as a string
    End Function

    Public Shared Function GetIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = context.Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return context.Request.ServerVariables("REMOTE_ADDR")
        Else
            Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return ipArray(0)
        End If
    End Function

    Public Shared Sub getSheetExcel(targetPage As Page, targetFileUpload As FileUpload, targetDropdown As DropDownList)
        If (targetFileUpload.HasFile) Then        ' CHECK IF ANY FILE HAS BEEN SELECTED.

            If Not IsDBNull(targetFileUpload.PostedFile) And targetFileUpload.PostedFile.ContentLength > 0 Then

                If (".xls" = System.IO.Path.GetExtension(targetFileUpload.FileName) Or ".xlsx" = System.IO.Path.GetExtension(targetFileUpload.FileName)) Then
                    ' SAVE THE SELECTED FILE IN THE ROOT DIRECTORY.
                    targetFileUpload.SaveAs(targetPage.Server.MapPath("~") & "TempFiles\" & targetFileUpload.FileName)
                    targetPage.Session("uploadFilePath") = targetPage.Server.MapPath("~") & "TempFiles\" & targetFileUpload.FileName

                    ' SET A CONNECTION WITH THE EXCEL FILE.
                    Dim myExcelConn As OleDbConnection =
                    New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" &
                        targetPage.Server.MapPath("~") & "TempFiles\" & targetFileUpload.FileName() &
                        ";Extended Properties=Excel 12.0;")
                    Try
                        myExcelConn.Open()

                        'Set dropdown
                        Dim dtSheets As DataTable = myExcelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                        Dim drSheet As DataRow

                        targetDropdown.Items.Clear()

                        For Each drSheet In dtSheets.Rows
                            If drSheet("TABLE_NAME").ToString().Contains("$") Then
                                targetDropdown.Items.Add(drSheet("TABLE_NAME").ToString())
                            End If
                        Next
                    Catch ex As Exception

                    Finally
                        ' CLEAR.
                        'myExcelConn = Nothing
                        myExcelConn.Close()
                    End Try
                End If
            End If
        End If
    End Sub

    Public Shared Function uploadXls(targetPage As Page, targetGridView As GridView, targetDropdown As DropDownList) As DataTable
        Dim dt As DataTable = New DataTable
        ' SET A CONNECTION WITH THE EXCEL FILE.
        Dim myExcelConn As OleDbConnection =
                    New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" &
                        targetPage.Session("uploadFilePath") &
                        ";Extended Properties=Excel 12.0;")
        Try
            myExcelConn.Open()

            ' GET DATA FROM EXCEL SHEET.
            Dim objOleDB As New OleDbCommand("SELECT * FROM [" & targetDropdown.SelectedValue & "]", myExcelConn)

            ' READ THE DATA EXTRACTED FROM THE EXCEL FILE.
            Dim objBulkReader As OleDbDataReader
            objBulkReader = objOleDB.ExecuteReader

            dt.Load(objBulkReader)

            ' FINALLY, BIND THE EXTRACTED DATA TO THE GRIDVIEW.
            targetGridView.DataSource = dt
            targetGridView.DataBind()

        Catch ex As Exception

        Finally
            ' CLEAR.
            myExcelConn.Close() : myExcelConn = Nothing
            deleteFilePath(targetPage, targetPage.Session("uploadFilePath").ToString)
        End Try
        Return dt
    End Function

    Public Shared Function NewUploadXls(targetPage As Page, targetGridView As GridView, targetDropdown As DropDownList, Optional colDate As Boolean = False) As DataTable
        Dim dt As DataTable = New DataTable

        ' SET A CONNECTION WITH THE EXCEL FILE.
        Dim myExcelConn As OleDbConnection =
                    New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" &
                        targetPage.Session("uploadFilePath") &
                        ";Extended Properties=Excel 12.0;")

        Dim x = GetColumnHeader("Tooling_Master", "Tooling_Mecha")

        Try
            myExcelConn.Open()

            ' GET DATA FROM EXCEL SHEET.
            Dim objOleDB As New OleDbCommand("SELECT * FROM [" & targetDropdown.SelectedValue & "]", myExcelConn)

            ' READ THE DATA EXTRACTED FROM THE EXCEL FILE.
            Dim objBulkReader As OleDbDataReader
            objBulkReader = objOleDB.ExecuteReader

            dt.Load(objBulkReader)

            If dt.Columns.Count = x.Count Then

                For i = 0 To dt.Columns.Count - 1
                    For j = 0 To x.Count - 1
                        Dim x0 = dt.Columns.Item(i).ColumnName.ToLower().Replace(" ", "")
                        Dim x1 = x(j).ToString.ToLower()
                        If x0 = x1 Then
                            dt.Columns.Item(i).ColumnName = x(j).ToString()
                        End If
                    Next
                Next

                targetGridView.DataSource = dt
                targetGridView.DataBind()

            Else
                dt = Nothing
            End If

            'targetGridView.DataSource = dt
            'targetGridView.DataBind()


        Catch ex As Exception
        Finally
            ' CLEAR.
            myExcelConn.Close() : myExcelConn = Nothing
            deleteFilePath(targetPage, targetPage.Session("uploadFilePath").ToString)
        End Try
        Return dt
    End Function

    Public Shared Sub deleteFilePath(targetPage As Page, FilePath As String)
        'Dim Directory As String = targetPage.Server.MapPath("~/TempFiles/")
        'Dim FilePath As String = Directory & yourFileName
        Dim fileInfo As FileInfo = New FileInfo(FilePath)

        If fileInfo.Exists Then
            fileInfo.Delete()
        End If

    End Sub

    Public Shared Sub TransposeTable(targetDataGridView As GridView, sqlCommandStr As String)

        Dim table As New DataTable
        Try
            Dim DBConnection As SqlConnection = New SqlConnection(StandardFunction.connectionString)

            Dim command As New SqlCommand(sqlCommandStr, DBConnection)
            Dim adapter As SqlDataAdapter = New SqlDataAdapter()
            adapter.SelectCommand = command

            table.Locale = System.Globalization.CultureInfo.InvariantCulture

            adapter.Fill(table)
        Catch ex As Exception
            table = GetDataTable("Select 'No Data' as [NoData]")
        End Try

        Dim OutputTable As New DataTable
        OutputTable.Columns.Add(table.Columns(0).ColumnName)
        For Each InputRow As DataRow In table.Rows
            Dim NewColumnName As String = InputRow.Item(0)
            OutputTable.Columns.Add(NewColumnName)
        Next InputRow

        For RowCount As Integer = 1 To table.Columns.Count - 1
            Dim NewRow As Data.DataRow = OutputTable.NewRow
            NewRow.Item(0) = table.Columns(RowCount).ColumnName
            For ColumnCount As Integer = 0 To table.Rows.Count - 1
                Dim ColumnValue As String = table.Rows(ColumnCount)(RowCount)
                NewRow(ColumnCount + 1) = ColumnValue
            Next ColumnCount
            OutputTable.Rows.Add(NewRow)
        Next RowCount

        If (OutputTable.Rows.Count > 0) Then

            targetDataGridView.DataSource = OutputTable
            targetDataGridView.DataBind()
        Else

        End If

    End Sub

    Public Shared Function Encrypt(ByVal text As String) As String
        Dim hash As String = "S@fem0de"
        Dim data As Byte() = UTF8Encoding.UTF8.GetBytes(text)

        Using md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
            Dim keys As Byte() = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash))

            Using tripDes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider() With {
            .Key = keys,
            .Mode = CipherMode.ECB
        }
                Dim transform As ICryptoTransform = tripDes.CreateEncryptor()
                Dim results As Byte() = transform.TransformFinalBlock(data, 0, data.Length)
                Return Convert.ToBase64String(results, 0, results.Length)
            End Using
        End Using
    End Function

    Public Shared Function Decrypt(ByVal text As String) As String
        Dim hash As String = "S@fem0de"
        Dim data As Byte() = Convert.FromBase64String(text)

        Using md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
            Dim keys As Byte() = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash))

            Using tripDes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider() With {
            .Key = keys,
            .Mode = CipherMode.ECB
        }
                Dim transform As ICryptoTransform = tripDes.CreateDecryptor()
                Dim results As Byte() = transform.TransformFinalBlock(data, 0, data.Length)
                Return UTF8Encoding.UTF8.GetString(results)
            End Using
        End Using
    End Function

    Public Shared Sub ExportExcel(TargetPage As Page, SqlString As String, fileName As String)

        Dim dt As DataTable = GetDataTable(SqlString)
        Dim wb As New XLWorkbook
        Dim ws = wb.Worksheets.Add(dt, fileName)
        ws.Columns.AdjustToContents()
        TargetPage.Response.Clear()
        TargetPage.Response.Buffer = True
        TargetPage.Response.Charset = ""
        TargetPage.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        TargetPage.Response.AddHeader("content-disposition", "attachment;filename=" & fileName & ".xlsx")
        Dim MyMemoryStream As New MemoryStream
        wb.SaveAs(MyMemoryStream)
        MyMemoryStream.WriteTo(TargetPage.Response.OutputStream)
        TargetPage.Response.Flush()
        TargetPage.Response.End()

    End Sub

    Public Shared Sub ExportExcelMultiSheet(TargetPage As Page, SqlString_Arr As ArrayList, fileName As String)

        Dim ds As New DataSet
        For Each i In SqlString_Arr
            ds.Tables.Add(GetDataTable(i))
        Next

        Dim wb As New XLWorkbook
        For j = 0 To ds.Tables.Count - 1
            Dim ws = wb.Worksheets.Add(ds.Tables(j), fileName & "(" & j & ")")
            ws.Columns.AdjustToContents()
        Next

        TargetPage.Response.Clear()
        TargetPage.Response.Buffer = True
        TargetPage.Response.Charset = ""
        TargetPage.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        TargetPage.Response.AddHeader("content-disposition", "attachment;filename=" & fileName & ".xlsx")
        Dim MyMemoryStream As New MemoryStream
        wb.SaveAs(MyMemoryStream)
        MyMemoryStream.WriteTo(TargetPage.Response.OutputStream)
        TargetPage.Response.Flush()
        TargetPage.Response.End()

    End Sub

    Public Shared Sub ExportCSV(TargetPage As Page, Sql As String)

        Dim dt As DataTable = StandardFunction.GetDataTable(Sql)
        Dim csv As String = ""
        For Each column As DataColumn In dt.Columns
            'Add the Header row for CSV file.
            csv += column.ColumnName + ","c
        Next

        'Add new line.
        csv += vbCr & vbLf

        For Each row As DataRow In dt.Rows
            For Each column As DataColumn In dt.Columns
                'Add the Data rows.
                csv += row(column.ColumnName).ToString().Replace(",", ";") + ","c
            Next

            'Add new line.
            csv += vbCr & vbLf
        Next

        'Download the CSV file.
        TargetPage.Response.Clear()
        TargetPage.Response.Buffer = True
        TargetPage.Response.AddHeader("content-disposition", "attachment;filename=Sparepart.csv")
        TargetPage.Response.Charset = ""
        TargetPage.Response.ContentType = "application/text"
        TargetPage.Response.Output.Write(csv)
        TargetPage.Response.Flush()
        TargetPage.Response.End()
    End Sub

    Public Shared Function GetColumnHeader(TableName As String, DBName As String) As ArrayList
        Dim SqlColumn = "Select [COLUMN_NAME]
        From " & DBName & ".INFORMATION_SCHEMA.COLUMNS
        WHERE [TABLE_NAME] = '" & TableName & "'
        And Not [Column_name] = 'Add_date'"

        Dim con As New SqlConnection
        Dim command As SqlCommand
        Dim items As ArrayList = New ArrayList

        Try
            'Get data from SQL to SQL_reader
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlColumn, con)
            Dim rdr As SqlDataReader = command.ExecuteReader

            'Add data from SQL_reader to cbo
            Do While rdr.Read()
                items.Add(rdr(0))
            Loop

        Catch ex As Exception
        Finally
            'Close connection
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        End Try

        Return items
    End Function

    Public Shared Function GetGraph(Sql As String) As Array

        Dim xColumns As New ArrayList
        Dim Data As New ArrayList
        Dim xRows As New ArrayList
        Dim temp As New ArrayList

        Dim dt As DataTable = GetDataTable(Sql)

        For i = 0 To (dt.Columns.Count - 1)
            If i > 0 Then
                xColumns.Add(dt.Columns(i).ColumnName)
                For Each r As DataRow In dt.Rows
                    temp.Add(r.Item(i).ToString)
                Next
                Data.Add(temp.ToArray)
                temp.Clear()
            Else
                For Each r As DataRow In dt.Rows
                    xRows.Add(CDate(r.Item(i)).ToString("yyyy/MM/dd"))
                Next
            End If
        Next

        Dim JSON_xColumns = New JavaScriptSerializer().Serialize(xColumns)
        Dim JSON_Data = New JavaScriptSerializer().Serialize(Data)
        Dim JSON_xRows = New JavaScriptSerializer().Serialize(xRows)

        Return {JSON_xColumns, JSON_Data, JSON_xRows} '{x(0),x(1),x(2)}

    End Function

End Class
