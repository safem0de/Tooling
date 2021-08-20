Imports System.Drawing
Imports System.Globalization

Public Class Stock
	Inherits System.Web.UI.Page
	ReadOnly SQLLoadTable = "
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
			Group by [ItemCode]
			),
			TableC as(
			SELECT t1.[ItemCode]
					,MAX(IsNull(t3.[Issue_date],t5.[Recieve_date])) as [lastest_date]
			  FROM [Tooling_Mecha].[dbo].[Tooling_Master] as t1
			LEFT JOIN [Tooling_Mecha].[dbo].[Issue_Tooling_Details] as t2
			ON t1.ItemCode = t2.ItemCode
			LEFT JOIN [Tooling_Mecha].[dbo].[Issue_Tooling] as t3
			ON t2.Issue_id = t3.Issue_id
			LEFT JOIN [Tooling_Mecha].[dbo].[Recieve_Tooling_Details] as t4
			ON t1.ItemCode = t4.ItemCode
			LEFT JOIN [Tooling_Mecha].[dbo].[Recieve_Tooling] as t5
			ON t4.Recieve_id = t5.Recieve_id

			GROUP BY t1.[ItemCode]
			),
			TableD as(
			SELECT t2.[ItemCode]
					,t2.[ItemName]
					,IsNull([Spec],'empty') as [Spec]
					,[Location]
					,(Isnull(t4.[Total],0) - Isnull(t3.[Total],0)) as [Qty]
					,[MinStock]
					,[UnitPrice]
					,DateDiff(day,[lastest_date],GETDATE())as [CurrentUsed]
					,CASE
						WHEN Isnull(t4.[Total],0) - Isnull(t3.[Total],0) > [MinStock] THEN 'Regular'
						WHEN Isnull(t4.[Total],0) - Isnull(t3.[Total],0) = [MinStock] THEN 'Need to Order'
						ELSE 'Urgent to Order'
					END AS [StockStatus]
					FROM [Tooling_Mecha].[dbo].[Tooling_Master] as t2
				LEFT JOIN TableA as t3
				ON t2.ItemCode = t3.ItemCode
				LEFT JOIN TableB as t4
				ON t2.ItemCode = t4.ItemCode
				LEFT JOIN TableC as t5
				ON t2.ItemCode = t5.ItemCode
			),

			TableE as(
			select *
			,CASE
				WHEN [StockStatus] = 'Urgent to Order' Then (ABS([Qty]-[MinStock])+1)*[UnitPrice]
				WHEN [StockStatus] = 'Need to Order' Then (ABS([Qty]-[MinStock])+1)*[UnitPrice]
				else 0
			END AS [Cost]
			,CASE
				WHEN [CurrentUsed] > 360 Then 'SlowMove'
				WHEN [CurrentUsed] <= 360 Then 'Normal'
				ELSE 'New Wait.. Recieve'
			END AS [UsedStatus]
			from TableD
			)

			select	[ItemCode]
					,[ItemName]
					,[Spec]
					,[Location]
					,[Qty]
					,[MinStock]
					,[StockStatus]
					,FORMAT([Cost],'N') as [Costs]
					,[UsedStatus]
			from TableE
			Order by Case when [StockStatus] = 'Urgent to Order' then 1
			when [StockStatus] = 'Need to Order' then 2
			else 3
			end asc , [Cost] desc
			"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		StandardFunction.fillDataTableToDataGrid(GridViewStockReport, SQLLoadTable, "")
	End Sub

	Private Sub GridViewStockReport_Load(sender As Object, e As EventArgs) Handles GridViewStockReport.Load
		Dim UrgentCount As Integer
		Dim NeedCount As Integer
		Dim RegularCount As Integer

		Dim CostUrgent As Double
		Dim CostNeed As Double
		For Each row As GridViewRow In GridViewStockReport.Rows
			If row.Cells(6).Text.Equals("Urgent to Order") Then
				row.Cells(6).BackColor = Color.Red
				row.Cells(6).Font.Bold = True
				UrgentCount += 1
				CostUrgent += CDbl(row.Cells(7).Text)
			ElseIf row.Cells(6).Text.Equals("Need to Order") Then
				row.Cells(6).BackColor = Color.Yellow
				NeedCount += 1
				CostNeed += CDbl(row.Cells(7).Text)
			Else
				row.Cells(6).BackColor = Color.Green
				RegularCount += 1
			End If
		Next

		LblUrgent.Text = UrgentCount
		LblNeedtoOrder.Text = NeedCount
		LblRegular.Text = RegularCount

		LblUrgent.ToolTip = "Total Urgent Order Costs = " & CostUrgent.ToString("C3", CultureInfo.CreateSpecificCulture("th-TH"))
		LblNeedtoOrder.ToolTip = "Total Need to Order Costs = " & CostNeed.ToString("C3", CultureInfo.CreateSpecificCulture("th-TH"))
	End Sub

	Protected Sub BtnDowloadExcel_Click(sender As Object, e As EventArgs) Handles BtnDowloadExcel.Click
		StandardFunction.ExportExcel(Me.Page, SQLLoadTable, "Stock" & DateTime.Now.ToString("yyyyMMdd"))
	End Sub
End Class