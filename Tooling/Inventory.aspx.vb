Public Class Inventory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		TxtMonth.AutoPostBack = True
		Page.Title = "Inventory"
		If Not IsPostBack Then
			BtnDownloadInven.Visible = False
		End If
	End Sub

    Private Sub TxtMonth_TextChanged(sender As Object, e As EventArgs) Handles TxtMonth.TextChanged
        'MsgBox(CInt(DateTime.Now.ToOADate))
        Dim Nowzz = CInt(CDate(DateTime.Now.ToString("yyyy-MM")).ToOADate)
        Dim Selectzz = CInt(CDate(TxtMonth.Text).ToOADate)
		'MsgBox(Nowzz)
		'MsgBox(Selectzz)
		If Selectzz < Nowzz And Nowzz - Selectzz < 300 Then
			BtnDownloadInven.Visible = True
			'MsgBox(Nowzz - Selectzz)
			Dim SqlInventory = "
                Declare @d as date;
				SET @d = Convert(Date,'" & CDate(TxtMonth.Text).ToString("yyyy-MM-dd") & "');

With
TableZ as(
	SELECT
		[ItemCode]
		,MAX(t2.[Issue_date]) as [last_Issue]
	FROM [Tooling_Mecha].[dbo].[Issue_Tooling_Details] as t1
	Left Join [Tooling_Mecha].[dbo].[Issue_Tooling] as t2
	on t1.[Issue_id] = t2.[Issue_id]
	Group By [ItemCode]
),
TableA as(
SELECT 
		[ItemCode]
		,MAX(t2.[Recieve_date]) as [last_recieve]
	FROM [Tooling_Mecha].[dbo].[Recieve_Tooling_Details] as t1
	Left Join [Tooling_Mecha].[dbo].[Recieve_Tooling] as t2
	on t1.[Recieve_id] = t2.[Recieve_id]
	group by [ItemCode]
),
TableB as(
SELECT
		t1.[ItemCode],
		SUM(t1.[Qty]) as [Rec_Bef]
	FROM [Tooling_Mecha].[dbo].[Recieve_Tooling_Details] as t1
	Left Join [Tooling_Mecha].[dbo].[Recieve_Tooling] as t2
	on t1.[Recieve_id] = t2.[Recieve_id]
	Where t2.[Recieve_date] < DATEADD(DAY, 1, EOMONTH(@d, -1))
	--AND [Itemcode] = 'N111E030'
	Group By t1.[ItemCode]
),
TableC as(
SELECT
		t1.[ItemCode],
		SUM(t1.[Qty]) as [Iss_Bef]
	FROM [Tooling_Mecha].[dbo].[Issue_Tooling_Details] as t1
	Left Join [Tooling_Mecha].[dbo].[Issue_Tooling] as t2
	on t1.[Issue_id] = t2.[Issue_id]
	Where t2.[Issue_date] < DATEADD(DAY, 1, EOMONTH(@d, -1))
	--AND [Itemcode] = 'N111E030'
	Group By t1.[ItemCode]
),
TableD as(
SELECT
		t1.[ItemCode],
		SUM(t1.[Qty]) as [Rec_Betw]
	FROM [Tooling_Mecha].[dbo].[Recieve_Tooling_Details] as t1
	Left Join [Tooling_Mecha].[dbo].[Recieve_Tooling] as t2
	on t1.[Recieve_id] = t2.[Recieve_id]
	Where
	t2.[Recieve_date] < DATEADD(DAY, 1, EOMONTH(@d, 0))
	AND
	t2.[Recieve_date] >= DATEADD(DAY, 1, EOMONTH(@d, -1))
	--AND [Itemcode] = 'N111E030'
	Group By t1.[ItemCode]		
),
TableE as(
SELECT
		t1.[ItemCode],
		SUM(t1.[Qty]) as [Iss_Betw]
	FROM [Tooling_Mecha].[dbo].[Issue_Tooling_Details] as t1
	Left Join [Tooling_Mecha].[dbo].[Issue_Tooling] as t2
	on t1.[Issue_id] = t2.[Issue_id]
	Where
	t2.[Issue_date] < DATEADD(DAY, 1, EOMONTH(@d, 0))
	AND
	t2.[Issue_date] >= DATEADD(DAY, 1, EOMONTH(@d, -1))
	--AND [Itemcode] = 'N111E030'
	Group By t1.[ItemCode]
)

SELECT 
	t1.[ItemCode],
	t1.[ItemName],
	t1.[Spec],
	format(t1.[UnitPrice],'N2') as [UnitPrice],
	Case
		When ABS(DATEDIFF(day,[last_issue],[last_recieve])) IS NULL Then 'NOT USED'
		When ABS(DATEDIFF(day,[last_issue],[last_recieve])) > 180 Then 'SLOW MOVE'
		When ABS(DATEDIFF(day,[last_issue],[last_recieve])) > 360 Then 'DEAD STOCK'
		Else 'USED'
	End as 'Status',
	--t2.[Rec_Bef],
	--t3.[Iss_Bef],
	(Isnull(t2.[Rec_Bef],0)-Isnull(t3.[Iss_Bef],0)) as [Initial],
	format((Isnull(t2.[Rec_Bef],0)-Isnull(t3.[Iss_Bef],0)) * t1.[UnitPrice] ,'N2') as [Initial_Amount],
	----Receive in Selected Month
	Isnull(t4.[Rec_Betw],0) as [Receive],
	format(Isnull(t4.[Rec_Betw],0) * t1.[UnitPrice] ,'N2') as [Receive_Amount],
	----Issue in Selected Month
	Isnull(t5.[Iss_Betw],0) as [Issue],
	format(Isnull(t5.[Iss_Betw],0) * t1.[UnitPrice] ,'N2') as [Issue_Amount],
	----Balance
	((Isnull(t2.[Rec_Bef],0)-Isnull(t3.[Iss_Bef],0))+Isnull(t4.[Rec_Betw],0)-Isnull(t5.[Iss_Betw],0)) as [Balance],
	format(((Isnull(t2.[Rec_Bef],0)-Isnull(t3.[Iss_Bef],0))+Isnull(t4.[Rec_Betw],0)-Isnull(t5.[Iss_Betw],0)) * t1.[UnitPrice] ,'N2') as [Balance_Amount],
	t1.[Location],
	t1.[DwgNo],
	t1.[VendorCode]
FROM [Tooling_Mecha].[dbo].[Tooling_Master] as t1
	Left Join TableB as t2
	on t1.[ItemCode] = t2.[ItemCode]
	Left Join TableC as t3
	on t1.[ItemCode] = t3.[ItemCode]
	Left Join TableD as t4
	on t1.[ItemCode] = t4.[ItemCode]
	Left Join TableE as t5
	on t1.[ItemCode] = t5.[ItemCode]
	Left Join TableZ as t6
	on t1.[ItemCode] = t6.[ItemCode]
	Left Join TableA as t7
	on t1.[ItemCode] = t7.[ItemCode]
--WHERE t1.[ItemCode] = 'N111E030'
            "
			StandardFunction.fillDataToDataGrid(GrdInventory, SqlInventory)
			Session("DownloadInven") = SqlInventory
		Else
			Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "alert('ดูข้อมูลย้อนหลังไม่เกิน 300 วัน');", True)
            Exit Sub
        End If
    End Sub

	Private Sub BtnDownloadInven_Click(sender As Object, e As EventArgs) Handles BtnDownloadInven.Click
		If Not Session("DownloadInven") = Nothing Then
			StandardFunction.ExportExcel(Me.Page, Session("DownloadInven"), "Inventory")
		End If
	End Sub
End Class