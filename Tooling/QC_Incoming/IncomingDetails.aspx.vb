Imports System.Data.SqlClient

Public Class IncomingDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Session("CanAccess") Then
            If Not (Session("Process") = "PURCHASE" Or Session("Process") = "ADMIN" Or Session("Process") = "IQC") Then
                Session("LastPage") = "~" & Request.RawUrl
                Response.Redirect("~/Login.aspx")
            End If
        End If

        If Not IsPostBack Then
            LoadPage()
        End If

    End Sub

    Sub LoadPage()
        If Not String.IsNullOrEmpty(Request.QueryString("Id")) And IsNumeric(Request.QueryString("Id")) Then
            Dim key1 = Request.QueryString("Id")
            Dim SqlEnableForm1 = "
                DECLARE @Item int
                SET @Item = " & CInt(key1) & "

                SELECT
                      CASE WHEN EXISTS 
                      (
                            SELECT * FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] WHERE [id_Req_detail] = @Item
                      )
                      THEN 'TRUE'
                      ELSE 'FALSE'
                END
                "
            Dim x = StandardFunction.getSQLDataString(SqlEnableForm1)
            If Not CBool(x) Then
                Response.Redirect("IncomingItems")
            Else
                Dim SqldtObj = "
                        SELECT [PartNo]
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
                        WHERE [id_Req_detail] = " & CInt(key1) & "
                    "
                Dim dt0 = StandardFunction.GetDataTable(SqldtObj)

                For Each r As DataRow In dt0.Rows
                    LblDataPartNo.Text = r.Item("PartNo")
                    LblDataSpec.Text = r.Item("Spec")
                    LblDataName.Text = r.Item("ItemName")
                    lblDataPO.Text = r.Item("PO_No")
                    LblDataQty.Text = r.Item("Qty")
                    LblDataRequestDate.Text = r.Item("RequestDate")
                Next

                Dim sqlGetObject = "
                        SELECT	[IQC_id]
		                ,[NG]
		                ,[Side_1]
		                ,[Side_2]
		                ,[Side_3]
		                ,[Side_4]
		                ,[Reject_Case]
					    ,[WorkMin]
	                FROM [Tooling_Mecha].[dbo].[Incoming_Tooling_Details] as t1
	                LEFT JOIN [Tooling_Mecha].[dbo].[Tooling_Master] as t2
	                ON t1.[PartNo] = t2.[ItemCode]
	                LEFT JOIN [Tooling_Mecha].[dbo].[Incoming_Tooling_IQC] as t3
	                ON t1.[id_Req_detail] = t3.[id_Req_detail]
	                WHERE
		                Not [IQC_RecieveDate] Is Null
	                AND
		                Not t3.[id_Req_detail] Is Null
				    AND
					    t3.[id_Req_detail] = " & CInt(key1) & "
                    "
                Dim dt1 = StandardFunction.GetDataTable(sqlGetObject)
                Dim objData As New IQCDetails

                If dt1.Rows.Count = 1 Then
                    For Each s As DataRow In dt1.Rows
                        objData.IQC_id = s.Item("IQC_id")
                        objData.NG = s.Item("NG")
                        objData.Side1 = s.Item("Side_1")
                        objData.Side2 = s.Item("Side_2")
                        objData.Side3 = s.Item("Side_3")
                        objData.Side4 = s.Item("Side_4")
                        objData.RejCase = s.Item("Reject_Case")
                        objData.WorkMin = s.Item("WorkMin")
                    Next
                    'MsgBox(objData.IQC_id)
                    Session("EditData") = objData
                    TxtNGQty.Text = objData.NG
                    TxtWorkMin.Text = objData.WorkMin
                    TxtSide1.Text = objData.Side1
                    TxtSide2.Text = objData.Side2
                    TxtSide3.Text = objData.Side3
                    TxtSide4.Text = objData.Side4
                    TxtRejectRemark.Text = objData.RejCase
                End If

            End If
        Else
            Response.Redirect("IncomingItems")
        End If

        If Not IsNothing(Session("EditData")) Then
            BtnConfirm.Visible = False
            BtnConfirm.Enabled = False
        Else
            BtnEdit.Visible = False
            BtnEdit.Enabled = False
        End If
    End Sub

    Protected Sub BtnConfirm_Click(sender As Object, e As EventArgs) Handles BtnConfirm.Click
        BtnConfirm.Enabled = False
        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim key1 = Request.QueryString("Id")

        Dim SqlConfirm = "
            INSERT INTO [Tooling_Mecha].[dbo].[Incoming_Tooling_IQC]
                       ([Date_iqc]
                       ,[id_Req_detail]
                       ,[NG]
                       ,[Side_1]
                       ,[Side_2]
                       ,[Side_3]
                       ,[Side_4]
                       ,[Reject_Case]
                       ,[WorkMin]
                        )
                 VALUES
                       (GETDATE()
                       ," & CInt(key1) & "
                       ," & TxtNGQty.Text & "
                       ," & TxtSide1.Text & "
                       ," & TxtSide2.Text & "
                       ," & TxtSide3.Text & "
                       ," & TxtSide4.Text & "
                       ,'" & TxtRejectRemark.Text & "'
                       ," & TxtWorkMin.Text & ")
        "

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlConfirm, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
        End Try
        Clearform()
        'TxtTest.Text = SqlConfirm
        Response.Redirect("IncomingItems")
    End Sub

    Sub Clearform()
        TxtNGQty.Text = Nothing
        TxtSide1.Text = Nothing
        TxtSide2.Text = Nothing
        TxtSide3.Text = Nothing
        TxtSide4.Text = Nothing
        TxtRejectRemark.Text = Nothing
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        BtnEdit.Enabled = False
        Dim con As New SqlConnection
        Dim command As SqlCommand

        Dim objData As IQCDetails = CType(Session("EditData"), IQCDetails)

        Dim SqlEditUpdate = "
            UPDATE [Tooling_Mecha].[dbo].[Incoming_Tooling_IQC]
               SET [NG] = " & CInt(TxtNGQty.Text) & "
                  ,[Side_1] = " & CInt(TxtSide1.Text) & "
                  ,[Side_2] = " & CInt(TxtSide2.Text) & "
                  ,[Side_3] = " & CInt(TxtSide3.Text) & "
                  ,[Side_4] = " & CInt(TxtSide4.Text) & "
                  ,[Reject_Case] = '" & TxtRejectRemark.Text & "'
                  ,[WorkMin] = " & CInt(TxtWorkMin.Text) & "
             WHERE [IQC_id] = " & objData.IQC_id & "
        "

        Try
            con.ConnectionString = StandardFunction.connectionString
            con.Open()
            command = New SqlCommand(SqlEditUpdate, con)
            command.ExecuteNonQuery()
        Catch ex As Exception
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('" & ex.Message & "');", True)
        Finally
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Window-script", "alert('บันทึกข้อมูลเรียบร้อย');", True)
            con.Close()
        End Try
        Clearform()
        'TxtTest.Text = SqlEditUpdate
        Response.Redirect("IncomingItems")
    End Sub
End Class