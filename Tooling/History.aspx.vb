Public Class History
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If

        LoadPage()
    End Sub

    Private Sub GridViewHistory_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridViewHistory.PageIndexChanging
        GridViewHistory.PageIndex = e.NewPageIndex
        GridViewHistory.DataBind()
    End Sub

    Sub LoadPage()
        Dim SqlGridLoad = "
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
        ),
		TableD as(
        SELECT [Issue_date] as [Date]
		        ,[ItemCode]
		        ,Convert(int,-1*[Qty]) as [Qty]
		        ,[Reason] as [Reason/LotNo]
		        ,t2.[EmpNo]
				,t3.[Process]
		        ,'Issue' as [Status]
        FROM [Tooling_Mecha].[dbo].[Issue_Tooling_Details] as t1
        Left Join [Tooling_Mecha].[dbo].[Issue_Tooling] as t2
        on t1.Issue_id = t2.Issue_id
		Left Join [Tooling_Mecha].[dbo].[UserLogin]as t3
        On t2.[EmpNo] = t3.[EmpNo]
        ),
        TableE as (
        SELECT [Recieve_date] as [Date]
                ,[ItemCode]
                ,[Qty]
                ,[LotNo] as [Reason/LotNo]
                ,t2.[EmpNo]
				,t3.[Process]
	            ,'Recieve' as [Status]
            FROM [Tooling_Mecha].[dbo].[Recieve_Tooling_Details] as t1
        Left Join [Tooling_Mecha].[dbo].[Recieve_Tooling] as t2
        On t1.Recieve_id = t2.Recieve_id
		Left Join [Tooling_Mecha].[dbo].[UserLogin]as t3
        On t2.[EmpNo] = t3.[EmpNo]
        where t1.[Status] = 1
        ),
	    TableF as (
        select * from TableD
        union
        select * from TableE
	
	    )
        "

        If (Not TxtItemCode.Text = "") _
            And TxtStartDate.Text = "" _
            And TxtEndDate.Text = "" _
            And DrpStatus.SelectedIndex = 0 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo] ,t1.[Process]   ,t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
		where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf (Not TxtItemCode.Text = "") _
            And TxtStartDate.Text = "" _
            And TxtEndDate.Text = "" _
            And DrpStatus.SelectedIndex = 1 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo] ,t1.[Process],  t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
		where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        and t1.[Status] = 'Recieve'
        group by t1.[Date],t1.[ItemCode],t3.[Spec],t1.[Qty],t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf (Not TxtItemCode.Text = "") _
            And TxtStartDate.Text = "" _
            And TxtEndDate.Text = "" _
            And DrpStatus.SelectedIndex = 2 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
		where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        and t1.[Status] = 'Issue'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf (Not TxtItemCode.Text = "") _
             And (Not TxtStartDate.Text = "") _
             And TxtEndDate.Text = "" _
             And DrpStatus.SelectedIndex = 0 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        and t1.[Date] >= '" & TxtStartDate.Text & "'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf (Not TxtItemCode.Text = "") _
         And (Not TxtStartDate.Text = "") _
         And TxtEndDate.Text = "" _
         And DrpStatus.SelectedIndex = 1 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        and t1.[Date] >= '" & TxtStartDate.Text & "'
        and t1.[Status] = 'Recieve'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf (Not TxtItemCode.Text = "") _
         And (Not TxtStartDate.Text = "") _
         And TxtEndDate.Text = "" _
         And DrpStatus.SelectedIndex = 2 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        and t1.[Date] >= '" & TxtStartDate.Text & "'
        and t1.[Status] = 'Issue'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf TxtItemCode.Text = "" _
             And (Not TxtStartDate.Text = "") _
             And (Not TxtEndDate.Text = "") _
             And DrpStatus.SelectedIndex = 0 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[Date] >= '" & TxtStartDate.Text & "'
		and t1.[Date] <= DATEADD(day,1,'" & TxtEndDate.Text & "')
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf TxtItemCode.Text = "" _
             And (Not TxtStartDate.Text = "") _
             And (Not TxtEndDate.Text = "") _
             And DrpStatus.SelectedIndex = 1 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[Date] >= '" & TxtStartDate.Text & "'
		and t1.[Date] <= DATEADD(day,1,'" & TxtEndDate.Text & "')
        and t1.[Status] = 'Recieve'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf TxtItemCode.Text = "" _
             And (Not TxtStartDate.Text = "") _
             And (Not TxtEndDate.Text = "") _
             And DrpStatus.SelectedIndex = 2 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[Date] >= '" & TxtStartDate.Text & "'
		and t1.[Date] <= DATEADD(day,1,'" & TxtEndDate.Text & "')
        and t1.[Status] = 'Issue'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf (Not TxtItemCode.Text = "") _
             And (Not TxtStartDate.Text = "") _
             And (Not TxtEndDate.Text = "") _
             And DrpStatus.SelectedIndex = 0 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        and t1.[Date] >= '" & TxtStartDate.Text & "'
		and t1.[Date] <= DATEADD(day,1,'" & TxtEndDate.Text & "')
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf (Not TxtItemCode.Text = "") _
             And (Not TxtStartDate.Text = "") _
             And (Not TxtEndDate.Text = "") _
             And DrpStatus.SelectedIndex = 1 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        and t1.[Date] >= '" & TxtStartDate.Text & "'
		and t1.[Date] <= DATEADD(day,1,'" & TxtEndDate.Text & "')
        and t1.[Status] = 'Recieve'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"

        ElseIf (Not TxtItemCode.Text = "") _
             And (Not TxtStartDate.Text = "") _
             And (Not TxtEndDate.Text = "") _
             And DrpStatus.SelectedIndex = 2 Then
            SqlGridLoad += "
        select  t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        where t1.[ItemCode] = '" & TxtItemCode.Text & "'
        and t1.[Date] >= '" & TxtStartDate.Text & "'
		and t1.[Date] <= DATEADD(day,1,'" & TxtEndDate.Text & "')
         and t1.[Status] = 'Issue'
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"
        Else
            SqlGridLoad += "
        select  Top(1000) t1.[Date]   ,t1.[ItemCode]	,t3.[Spec]  ,t3.[Location]
                ,t1.[Qty]   ,SUM(t2.[Qty]) as Stock 
                ,t1.[Reason/LotNo]  ,t1.[EmpNo],t1.[Process],t1.[Status]
        from TableF as t1
        left join TableF as t2
		on t1.[Date] >= t2.[Date] and t1.[ItemCode] = t2.[ItemCode]
		left join [Tooling_Mecha].[dbo].[Tooling_Master] as t3
		on t1.[ItemCode] = t3.[ItemCode]
        group by t1.[Date],t1.[ItemCode],t3.[Spec], t1.[Qty], t1.[Reason/LotNo],t1.[EmpNo],t1.[Process],t1.[Status],t3.[Location]
        order by t1.[Date] desc"
        End If
        Session("Download") = SqlGridLoad
        StandardFunction.fillDataTableToDataGrid(GridViewHistory, SqlGridLoad, "-")
    End Sub

    Protected Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        TxtItemCode.Text = ""
        TxtStartDate.Text = ""
        TxtEndDate.Text = ""
        DrpStatus.SelectedIndex = 0
        LoadPage()
    End Sub

    Protected Sub BtnDownload_Click(sender As Object, e As EventArgs) Handles BtnDownload.Click
        StandardFunction.ExportExcel(Me.Page, Session("Download"), "History " & DateTime.Now.ToString().Replace("/", "-").Replace(":", "."))
    End Sub

    Protected Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        LoadPage()
    End Sub
End Class