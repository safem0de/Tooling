Imports System.Data.SqlClient

Public Class IncomingItems
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("CanAccess") Then
            If Not (Session("Process") = "PURCHASE" Or Session("Process") = "ADMIN" Or Session("Process") = "IQC") Then
                Session("LastPage") = "~" & Request.RawUrl
                Response.Redirect("~/Login.aspx")
            End If
        End If

        LoadPage()

        If Not IsPostBack Then
            CssDefault(1)
        End If
    End Sub

    Sub LoadPage()

        Dim SqlRecieve = "
            SELECT [PartNo] as [ItemCode]
	              ,t2.[Spec]
	              ,t2.[ItemName]
                  ,[PO_No]
                  ,[Qty]
                  ,[Status]
                  ,FORMAT([RequestDate],'yyyy-MM-dd') as [RequestDate]
              FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] as t1
              LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
              ON t1.[PartNo] = t2.[ItemCode]
            WHERE [IQC_RecieveDate] Is Null
        "
        StandardFunction.fillDataTableToDataGrid(GrdRecieve, SqlRecieve, "-")

        Dim SqlIncoming = "
            SELECT [PartNo] as [ItemCode]
	                ,IsNull(t2.[Spec],'No Master') as [Spec]
	                ,IsNull(t2.[ItemName],'No Master') as [ItemName]
                    ,[PO_No]
                    ,[Qty]
                    ,[Status]
                    ,FORMAT([RequestDate],'yyyy-MM-dd') as [RequestDate]
                    ,FORMAT([IQC_RecieveDate],'yyyy-MM-dd HH:mm:ss') as [IQC_RecieveDate]
                FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] as t1
                LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
                ON t1.[PartNo] = t2.[ItemCode]
	            LEFT JOIN [Tooling_Mecha].[dbo].[Incoming_Tooling_IQC] as t3
                ON t1.[id_Req_detail] = t3.[id_Req_detail]
	            WHERE
		            Not [IQC_RecieveDate] Is Null
                AND
                    t3.[id_Req_detail] Is Null
        "
        StandardFunction.fillDataTableToDataGrid(GrdIncoming, SqlIncoming, "-")

        Dim StrDateSearch As String = Nothing
        If Not TxtMonth.Text = Nothing Then

            StrDateSearch = " 
                    And
                    (
                        (Month([RequestDate]) = " & Month(CDate(TxtMonth.Text)) & " And Year([RequestDate]) = " & Year(CDate(TxtMonth.Text)) & ")
			            Or
			            (MONTH([Date_iqc]) = " & Month(CDate(TxtMonth.Text)) & " And YEAR([Date_iqc]) = " & Year(CDate(TxtMonth.Text)) & ")
		            )
            "
        End If

        Dim SqlFinish = "
                SELECT	[PartNo] as [ItemCode]
		                ,t2.[Spec]
		                ,t2.[ItemName]
		                ,[PO_No]
		                ,FORMAT([RequestDate],'yyyy-MM-dd') as [RequestDate]
		                ,FORMAT([Date_iqc],'yyyy-MM-dd') as [IQC_Finish]
		                ,[Qty]
                        ,([Qty]-[NG]) as [OK]
		                ,[NG]
					    ,[WorkMin]
		                ,[Side_1]
		                ,[Side_2]
		                ,[Side_3]
		                ,[Side_4]
		                ,[Reject_Case]
					    ,CASE
						    WHEN t4.[id_Req_detail] IS NULL Then 'Not Recieved'
						    Else 'Recieved'
					    END as [Status]
	                FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] as t1
	                LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
	                ON t1.[PartNo] = t2.[ItemCode]
	                LEFT JOIN [Tooling_Mecha].[dbo].[Incoming_Tooling_IQC] as t3
	                ON t1.[id_Req_detail] = t3.[id_Req_detail]
				    LEFT JOIN [Tooling_Mecha].[dbo].[Recieve_Tooling] as t4
				    ON t1.[id_Req_detail] = t4.[id_Req_detail]
	                WHERE
		                (Not [IQC_RecieveDate] Is Null
					    AND
		                Not t3.[id_Req_detail] Is Null)
					AND
					(
						[ItemCode] like '" & TxtSearch.Text & "'+'%'
						OR
						[ItemName] like '" & TxtSearch.Text & "'+'%'
						OR
						[Spec] like '" & TxtSearch.Text & "'+'%'
                        OR
                        [PO_No] like '" & TxtSearch.Text & "'+'%'
					)
                    
                    " & StrDateSearch & "                     
        
                    Order by [Date_iqc] desc
            "
        Dim LotCount_dt = StandardFunction.GetDataTable(SqlFinish)
        LblLotCount.Text = "(" & LotCount_dt.Rows.Count.ToString & " Lots)"
        StandardFunction.fillDataTableToDataGrid(GrdFinish, SqlFinish, "-")
        TxtTest.Text = SqlFinish
    End Sub

    Sub CssDefault(Step_C As Integer)
        PanelRecieve.Visible = False
        PanelIncoming.Visible = False
        PanelFinish.Visible = False

        LnkRecieve.CssClass = "btn btn-outline-warning col-12"
        LnkIncoming.CssClass = "btn btn-outline-warning col-12"
        LnkFinish.CssClass = "btn btn-outline-warning col-12"

        If Step_C = 1 Then
            Page.Title = "Recieve"
            PanelRecieve.Visible = True
            LnkRecieve.CssClass = "btn btn-warning col-12"
        ElseIf Step_C = 2 Then
            Page.Title = "Incoming"
            PanelIncoming.Visible = True
            LnkIncoming.CssClass = "btn btn-warning col-12"
        ElseIf Step_C = 3 Then
            Page.Title = "Finish" + " (" + (GrdFinish.PageIndex + 1).ToString + ")"
            PanelFinish.Visible = True
            LnkFinish.CssClass = "btn btn-warning col-12"
        End If
    End Sub

    Private Sub LnkRecieve_Click(sender As Object, e As EventArgs) Handles LnkRecieve.Click
        CssDefault(1)
    End Sub

    Private Sub LnkIncoming_Click(sender As Object, e As EventArgs) Handles LnkIncoming.Click
        CssDefault(2)
    End Sub

    Private Sub LnkFinish_Click(sender As Object, e As EventArgs) Handles LnkFinish.Click
        CssDefault(3)
    End Sub

    Protected Sub GrdRecieve_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdRecieve.SelectedIndexChanged
        Dim row As GridViewRow = GrdRecieve.SelectedRow
        Dim SqlGetId_detail = "
            DECLARE @idz int;

            SELECT @idz = [id_Req_detail]
              FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details]
            WHERE	[PartNo] = '" & row.Cells(1).Text & "'
            AND		[PO_No] = '" & row.Cells(4).Text & "'
            AND		[Qty] = '" & row.Cells(5).Text & "'
            AND		[Status] = '" & row.Cells(6).Text & "'
            AND		[RequestDate] = '" & row.Cells(7).Text & "'

            UPDATE [Tooling_Mecha].[dbo].[Incoming_Tooling_Details]
            SET [IQC_RecieveDate] = GETDATE()
            WHERE [id_Req_detail] = @idz
        "

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlGetId_detail, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
        End Try

        Response.Redirect(Request.RawUrl)
    End Sub

    Private Sub GrdIncoming_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdIncoming.SelectedIndexChanged
        Session("EditData") = Nothing
        Dim row As GridViewRow = GrdIncoming.SelectedRow
        Dim SqlgetId = "
            SELECT [id_Req_detail]
              FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details]
              WHERE
                  [PartNo] = '" & row.Cells(1).Text & "'
              AND [PO_No] = '" & row.Cells(4).Text & "'
              AND [Qty] = " & row.Cells(5).Text & "
              AND [Status] = '" & row.Cells(6).Text & "'
              AND [RequestDate] = '" & row.Cells(7).Text & "'
              AND FORMAT([IQC_RecieveDate],'yyyy-MM-dd HH:mm:ss') = '" & row.Cells(8).Text & "'
        "
        Dim x = StandardFunction.getSQLDataString(SqlgetId)

        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim SqlUpdate = "
            UPDATE [Tooling_Mecha].[dbo].[Incoming_Tooling_Details]
               SET [IQC_IncomingDate] = GETDATE()
             WHERE [id_Req_detail] = " & CInt(x) & "
        "

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlUpdate, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
        End Try

        Response.Redirect("IncomingDetails" & "?Id=" & x)
    End Sub

    Protected Sub BtnDownload_Click(sender As Object, e As EventArgs) Handles BtnDownload.Click
        Dim arr As New ArrayList

        For i = 1 To 12
            Dim Sql = "
            SELECT	ROW_NUMBER() OVER(ORDER BY [Date_iqc]) AS [No.]
                    ,[PartNo]
		            ,IsNull(t2.[Spec],'No Master') as [Spec]
		            ,IsNull(t2.[ItemName],'No Master') as [ItemName]
		            ,[PO_No]
		            ,FORMAT([RequestDate],'yyyy-MM-dd') as [RequestDate]
		            ,FORMAT([Date_iqc],'yyyy-MM-dd') as [IQC_Finish]
		            ,[Qty]
                    ,([Qty]-[NG]) as [OK]
		            ,[NG]
		            ,[Side_1]
		            ,[Side_2]
		            ,[Side_3]
		            ,[Side_4]
		            ,[Reject_Case]
	            FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] as t1
	            LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
	            ON t1.[PartNo] = t2.[ItemCode]
	            LEFT JOIN [Tooling_Mecha].[dbo].[Incoming_Tooling_IQC] as t3
	            ON t1.[id_Req_detail] = t3.[id_Req_detail]
	            WHERE
		            Not [IQC_RecieveDate] Is Null
	            AND Not t3.[id_Req_detail] Is Null
                AND MONTH([Date_iqc]) = " & i & "
                AND YEAR([Date_iqc]) = YEAR(GETDATE())
        "
            arr.Add({Sql, MonthName(i)})
        Next

        StandardFunction.ExportExcelMultiSheet(Me.Page, arr, "IncomingTooling")
    End Sub

    Private Sub GrdRecieve_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdRecieve.RowDataBound
        If e.Row.Cells(6).Text.Contains("Urgent") Then
            e.Row.Cells(6).ForeColor = Drawing.Color.Red
            e.Row.Cells(6).Font.Bold = True
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each button As Button In e.Row.Cells(0).Controls.OfType(Of Button)()
                If button.CommandName = "Select" And Session("Process") = "PURCHASE" Then
                    button.Attributes.Add("disabled", "disabled")
                End If
            Next
        End If
    End Sub

    Private Sub GrdIncoming_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdIncoming.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each button As Button In e.Row.Cells(0).Controls.OfType(Of Button)()
                If button.CommandName = "Select" And Session("Process") = "PURCHASE" Then
                    button.Attributes.Add("disabled", "disabled")
                End If
            Next
        End If
    End Sub

    Private Sub GrdFinish_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GrdFinish.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            For Each button As Button In e.Row.Cells(0).Controls.OfType(Of Button)()
                If button.CommandName = "Select" And Session("Process") = "IQC" Then
                    button.Attributes.Add("disabled", "disabled")
                End If
            Next

            For Each button As Button In e.Row.Cells(1).Controls.OfType(Of Button)()
                If button.CommandName = "Edit" And Session("Process") = "PURCHASE" Then
                    button.Attributes.Add("disabled", "disabled")
                End If
            Next
        End If
    End Sub

    Private Sub GrdFinish_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GrdFinish.RowEditing
        Dim r = e.NewEditIndex
        Dim x = GetIdFrom_Finish(GrdFinish, e.NewEditIndex)
        If x > 0 Then
            Response.Redirect("IncomingDetails?Id=" & x)
        Else
            Response.Redirect("IncomingItems")
        End If

    End Sub

    Private Sub GrdFinish_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GrdFinish.PageIndexChanging
        GrdFinish.PageIndex = e.NewPageIndex
        GrdFinish.DataBind()

        Page.Title = "Finish" + " (" + (GrdFinish.PageIndex + 1).ToString + ")"
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        LoadPage()
    End Sub

    Function GetIdFrom_Finish(TargetGrid As GridView, r As Integer) As Integer
        Dim start As Integer = 2
        'MsgBox(GrdFinish.Rows(r).Cells(start).Text)

        Dim GetIdSQL = "
        SELECT 
              t3.[id_Req_detail]
        FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] as t1

        LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
        ON t1.[PartNo] = t2.[ItemCode]

        LEFT JOIN [Tooling_Mecha].[dbo].[Incoming_Tooling_IQC] as t3
        ON t1.[id_Req_detail] = t3.[id_Req_detail]
        WHERE
	        Not [IQC_RecieveDate] Is Null
        AND
	        Not t3.[id_Req_detail] Is Null

        AND [PartNo] = '" & TargetGrid.Rows(r).Cells(start).Text & "'
        AND t2.[Spec] = '" & TargetGrid.Rows(r).Cells(start + 1).Text.Replace("&quot;", """") & "'
        AND t2.[ItemName] = '" & TargetGrid.Rows(r).Cells(start + 2).Text & "'
        AND [PO_No] = '" & TargetGrid.Rows(r).Cells(start + 3).Text & "'
        AND FORMAT([RequestDate],'yyyy-MM-dd') = '" & TargetGrid.Rows(r).Cells(start + 4).Text & "'
        AND FORMAT([Date_iqc],'yyyy-MM-dd') = '" & TargetGrid.Rows(r).Cells(start + 5).Text & "'
        AND [Qty] = " & TargetGrid.Rows(r).Cells(start + 6).Text & "
        AND ([Qty]-[NG]) = " & TargetGrid.Rows(r).Cells(start + 7).Text & "
        AND [NG] = " & TargetGrid.Rows(r).Cells(start + 8).Text & "
        AND [WorkMin] = " & TargetGrid.Rows(r).Cells(start + 9).Text & "
        AND [Side_1] = " & TargetGrid.Rows(r).Cells(start + 10).Text & "
        AND [Side_2] = " & TargetGrid.Rows(r).Cells(start + 11).Text & "
        AND [Side_3] = " & TargetGrid.Rows(r).Cells(start + 12).Text & "
        AND [Side_4] = " & TargetGrid.Rows(r).Cells(start + 13).Text & "
        "
        Dim x = StandardFunction.getSQLDataString(GetIdSQL)
        TxtTest.Text = GetIdSQL

        If IsNumeric(CInt(x)) Then
            Return CInt(x)
        Else
            Return -1
        End If

    End Function

    Protected Sub BtnForJpn_Click(sender As Object, e As EventArgs) Handles BtnForJpn.Click
        Dim arr As New ArrayList

        For i = 1 To 12
            Dim Sql = "
                SELECT	ROW_NUMBER() OVER(ORDER BY [Date_iqc]) AS [No.]
		                ,FORMAT([Date_iqc],'yyyy-MM-dd') as [IQC_Finish]
		                ,IsNull(t2.[Spec],'No Master') as [Spec]
		                ,[WorkMin]
		                ,CASE
			                WHEN t2.[ItemName] like 'WOODRUFF%' Then [Qty]
			                WHEN t2.[ItemName] like 'SCREW BIT%' Then [Qty]
			                WHEN t2.[ItemName] like 'HAND REAMER%' Then [Qty]
			                WHEN t2.[ItemName] like 'HALF REAMER%' Then [Qty]
			                WHEN t2.[ItemName] like 'WORK ARBOR%' Then [Qty]
			                WHEN t2.[ItemName] like 'EDGE CENTER%' Then [Qty]
			                ELSE 2
		                END as [Qty]
                        ,'' as [Category]
		                ,'Manual' as [Category]
		                ,IsNull(t2.[ItemName],'No Master') as [REMARK]
		                ,'' as [ ]
		                ,[Qty] as [Total Rec/Lot]
		                ,'' as [Total Rec/pcs.]
                FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] as t1
                LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
                ON t1.[PartNo] = t2.[ItemCode]
                LEFT JOIN [Tooling_Mecha].[dbo].[Incoming_Tooling_IQC] as t3
                ON t1.[id_Req_detail] = t3.[id_Req_detail]

                WHERE [Date_iqc] IS NOT NULL
                AND MONTH([Date_iqc]) = " & i & "
                AND YEAR([Date_iqc]) = YEAR(GETDATE())
        "
            arr.Add({Sql, MonthName(i)})
        Next

        StandardFunction.ExportExcelMultiSheet(Me.Page, arr, "Report Tooling For JPN")
    End Sub

    Private Sub GrdFinish_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GrdFinish.SelectedIndexChanged
        Dim row As GridViewRow = GrdFinish.SelectedRow
        Dim x = GetIdFrom_Finish(GrdFinish, row.RowIndex)
        'MsgBox(x)

        Dim StrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim SqlEnableForm1 = "
                    DECLARE @Item int
                    SET @Item = " & CInt(x) & "

                    SELECT
                          CASE WHEN EXISTS 
                          (
                                SELECT * FROM [Tooling_Mecha].[dbo].[Recieve_Tooling] WHERE [id_Req_detail] = @Item
                          )
                          THEN 'TRUE'
                          ELSE 'FALSE'
                    END
                    "

        Dim Chk = StandardFunction.getSQLDataString(SqlEnableForm1)
        If CBool(Chk) Then
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('รับไปแล้ว');", True)
            Exit Sub
        End If

        Dim Sql As String = "

            DECLARE @GetDetailId int,
            @GetRecId int,
            @Itemcode varchar(25),
            @LotNo varchar(25),
            @Qty int;

            SET @GetDetailId = " & x & ";

              SELECT @Itemcode = [PartNo]
        	            , @LotNo = [PO_No]
        	            , @Qty = [Qty]
              FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details]
              WHERE [id_Req_detail] = @GetDetailId 


            BEGIN TRANSACTION
            BEGIN TRY
              -- statement #0
              SELECT @Itemcode = [PartNo]
        	            , @LotNo = [PO_No]
        	            , @Qty = [Qty]
              FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details]
              WHERE [id_Req_detail] = @GetDetailId 


                            -- Statement #1

              INSERT INTO[Tooling_Mecha].[dbo].[Recieve_Tooling]
           		            ([Recieve_date]
           		            ,[EmpNo]
           		            ,[id_Req_detail])
                   VALUES
           		            ('" & StrDate & "'
           		            ,'" & Session("User") & "'
           		            , @GetDetailId)

                            -- Statement #2

                            SELECT @GetRecId = [Recieve_id]
                    FROM [Tooling_Mecha].[dbo].[Recieve_Tooling]
                    WHERE [Recieve_date] = '" & StrDate & "'
                    AND [EmpNo] = '" & Session("User") & "'
                 AND [id_Req_detail] = @GetDetailId 

              print @GetDetailId;

                            -- Statement #3

              INSERT INTO [Tooling_Mecha].[dbo].[Recieve_Tooling_Details]
                       ([Recieve_id]
                       ,[ItemCode]
                       ,[Qty]
                       ,[LotNo]
                       ,[Status]
                 )
                 VALUES
                       (@GetRecId
                       ,@Itemcode
                       ,@Qty
                       ,@LotNo
                       ,0
                )
                    COMMIT
                    END TRY
                    BEGIN CATCH
                    ROLLBACK
                    END CATCH
                    "

        'TxtTest.Text = Sql
        Dim con As New SqlConnection
        Dim command As SqlCommand

        Try
            con.ConnectionString = StandardFunction.connectionString 'คำสั่งเชื่อม SQL จาก IP ไหน,Password อะไร'
            con.Open()
            command = New SqlCommand(Sql, con)
            command.ExecuteNonQuery() 'คำสั่งเปิดใช้งาน การเชื่อมต่อ Sql'

        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('" & ex.Message & "');", True)
        Finally
            con.Close()
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('OK');", True)
        End Try

        Response.Redirect(Request.RawUrl)
    End Sub
End Class