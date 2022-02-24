Imports System.Data.SqlClient

Public Class Incoming
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("CanAccess") Then
            If Not (Session("Process") = "PURCHASE" Or Session("Process") = "ADMIN") Then
                Response.Redirect("~/Login.aspx")
            Else
                Session("LastPage") = "~" & Request.RawUrl
            End If
        End If

        If Not IsPostBack Then
            TxtPartNo.AutoPostBack = True
        End If

        LoadPage()

    End Sub

    Sub LoadPage()
        TxtPartNo.Focus()

        If IsNothing(Session("InComing")) Then
            Dim arr As New ArrayList
            'arr.Add("No.")
            arr.Add("PartNo")
            arr.Add("PO_No")
            arr.Add("Qty")
            arr.Add("Status")
            arr.Add("RequestDate")
            BindColumn(GrdIncoming, AddColumns(arr), "InComing")
        Else
            BindColumn(GrdIncoming, Session("InComing"), "InComing")
        End If
    End Sub

    Sub BindColumn(GV As GridView, dt As DataTable, Optional SsName As String = Nothing)
        If Not SsName = Nothing Then
            Session(SsName) = dt
        End If
        GV.DataSource = dt
        GV.DataBind()
    End Sub

    Function AddColumns(ArrCol As ArrayList) As DataTable
        Dim dt As New DataTable
        For Each x In ArrCol
            dt.Columns.Add(x)
        Next
        dt.Rows.Add()
        Return dt
    End Function

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click

        Dim SqlEnablePO = "
            DECLARE @Item varchar(25);
            SET @Item = '" & TxtPONo.Text & "';

              SELECT
                    CASE WHEN EXISTS 
                    (
                        SELECT * FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] WHERE [PO_No] = @Item
                    )
                    THEN 'TRUE'
                    ELSE 'FALSE'
		      END
        "
        Dim Check As Boolean = CBool(StandardFunction.getSQLDataString(SqlEnablePO))

        If Not Check Then
            Dim dtInComing As DataTable = Session("InComing")
            For i As Integer = dtInComing.Rows.Count - 1 To 0 Step -1
                Dim row As DataRow = dtInComing.Rows(i)
                If row.Item(0) Is Nothing Or row.Item(0).ToString = "" Then
                    dtInComing.Rows.Remove(row)
                ElseIf row.Item("PO_No").Equals(TxtPONo.Text) Then
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('P/O No, : " & TxtPONo.Text & " ซ้ำ!!(Duplicate)');", True)
                    Exit Sub
                End If
            Next

            dtInComing.Rows.Add(TxtPartNo.Text.ToUpper, TxtPONo.Text, TxtQty.Text, DrpStatus.SelectedValue, TxtRequestDate.Text)
            Clearform()

            BindColumn(GrdIncoming, dtInComing, "InComing")
        Else
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('P/O No. : " & TxtPONo.Text & "\nเคยถูกรีเควสเช็คสเปคไปแล้ว!! (Requested Please Check)');", True)
            Exit Sub
        End If

        ViewState("exist") = Nothing
    End Sub

    Sub Clearform()
        TxtPartNo.Text = Nothing
        TxtPONo.Text = Nothing
        TxtPONo.Enabled = False
        TxtQty.Text = Nothing
        TxtQty.Enabled = False
        DrpStatus.SelectedIndex = 0
        DrpStatus.Enabled = False
        TxtRequestDate.Text = Nothing
        TxtRequestDate.Enabled = False
    End Sub

    Protected Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click
        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim xdt = Session("InComing")

        Dim StrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim SqlTransac As New StringBuilder("
            INSERT INTO [Tooling_Mecha].[dbo].[Incoming_Tooling_Details]
               ([id_Req]
               ,[PartNo]
               ,[PO_No]
               ,[Qty]
               ,[Status]
               ,[RequestDate])
            VALUES
        ")

        Dim SqlTransacVal As New StringBuilder()

        For Each i As DataRow In xdt.Rows
            SqlTransacVal.Append("(@GetReq_id,")
            SqlTransacVal.Append("'" & i.Item("PartNo") & "',")
            SqlTransacVal.Append("'" & i.Item("PO_No") & "',")
            SqlTransacVal.Append(i.Item("Qty") & ",")
            SqlTransacVal.Append("'" & i.Item("Status") & "',")
            SqlTransacVal.Append("'" & i.Item("RequestDate") & "'),")
        Next

        SqlTransac.Append(SqlTransacVal.ToString.Substring(0, SqlTransacVal.Length - 1))

        Dim Sql As String = "
              DECLARE @RecDate datetime,
                        @ReqBy	varchar(5),
                        @Remark varchar(255),
                        @GetReq_id int;

              SET @RecDate = '" & StrDate & "'
              SET @ReqBy = '" & Session("User") & "'
              SET @Remark = '" & TxtRemark.Text & "'

              BEGIN TRANSACTION
               BEGIN TRY

               INSERT INTO [Tooling_Mecha].[dbo].[Incoming_Tooling]
                         ([Recieve_Date]
                         ,[Request_By]
                         ,[Remark])
                   VALUES
                         (@RecDate
                         ,@ReqBy
                         ,@Remark)

               SELECT @GetReq_id=[id_Req]
                 FROM [Tooling_Mecha].[dbo].[Incoming_Tooling]
                 WHERE
                [Recieve_Date] = @RecDate
                AND
                [Request_By] = @ReqBy
                AND
                [Remark] = @Remark

               " & SqlTransac.ToString() & "

                COMMIT
               END TRY
               BEGIN CATCH
                ROLLBACK
               END CATCH
              "
        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(Sql, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
        End Try

        Session("InComing") = Nothing
        TxtRemark.Text = Nothing
        LoadPage()
    End Sub

    Private Sub BtnHidden_Click(sender As Object, e As EventArgs) Handles BtnHidden.Click
        Dim SqlEnableForm = "
            DECLARE @Item varchar(25)
            SET @Item = '" & TxtPartNo.Text & "'

            SELECT
                  CASE WHEN EXISTS 
                  (
                        SELECT * FROM [Tooling_Mecha].[dbo].[Tooling_Master] WHERE [ItemCode] = @Item
                  )
                  THEN 'TRUE'
                  ELSE 'FALSE'
            END
        "
        Dim x = StandardFunction.getSQLDataString(SqlEnableForm)
        If CBool(x) Then
            TxtPONo.Enabled = True
            TxtQty.Enabled = True
            DrpStatus.Enabled = True
            TxtRequestDate.Enabled = True

            TxtPONo.Focus()
            ViewState("exist") = TxtPartNo.Text
        End If
    End Sub

    Private Sub TxtPartNo_TextChanged(sender As Object, e As EventArgs) Handles TxtPartNo.TextChanged

        If Not IsNothing(ViewState("exist")) Then
            If Not ViewState("exist").Equals(TxtPartNo.Text) Then
                Response.Redirect(Request.RawUrl)
            End If
        Else
            BtnHidden_Click(sender, e)
        End If

    End Sub

    Private Sub GrdIncoming_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GrdIncoming.RowDeleting
        Dim deldt As DataTable = Session("InComing")

        If e.RowIndex >= 0 And GrdIncoming.Rows.Count <> 1 Then
            deldt.Rows.RemoveAt(e.RowIndex)
            BindColumn(GrdIncoming, deldt)
        Else
            Dim arr As New ArrayList
            'arr.Add("No.")
            arr.Add("PartNo")
            arr.Add("PO_No")
            arr.Add("Qty")
            arr.Add("Status")
            arr.Add("RequestDate")
            BindColumn(GrdIncoming, AddColumns(arr), "InComing")
        End If
    End Sub
End Class