Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        LoadChart()
    End Sub

    Sub LoadChart()
        Dim SqlChart = "
        DECLARE 
            @columns NVARCHAR(MAX) = '', 
            @sql     NVARCHAR(MAX) = '';

        -- select the category names
        SELECT 
            @columns+=QUOTENAME(process) + ','
        FROM 
            [Tooling_Mecha].[dbo].[UserLogin]
        GROUP BY 
            process;
 
        -- remove the last comma
        SET @columns = LEFT(@columns, LEN(@columns) - 1);

        PRINT @columns;
        -- construct dynamic SQL
        SET @sql ='
        with TableA as
        (
        SELECT  [Issue_date] as [Date]
		        ,t3.[Process]
		        ,t2.[Qty]
          FROM [Tooling_Mecha].[dbo].[Issue_Tooling] as t1
        left join [Tooling_Mecha].[dbo].[Issue_Tooling_Details] as t2
        on t1.[Issue_id]=t2.[Issue_id]
        left join [Tooling_Mecha].[dbo].[UserLogin] as t3
        on t1.EmpNo = t3.EmpNo

		WHERE Month([Issue_date]) = Month(GETDATE())
		OR Month([Issue_date]) = Month(GETDATE())-1
        )

        SELECT * FROM
        (
        SELECT	Convert(Date,[Date],103) as [Date],
		        [Process],
		        [Qty]
        FROM TableA

        )t 
        PIVOT(
            SUM (Qty) 
            FOR [Process] IN ('+ @columns +')
        ) AS pivot_table
        Order By pivot_table.[Date] asc;
		';
        PRINT @sql;
        -- execute the dynamic SQL
        EXECUTE sp_executesql @sql;"
        'StandardFunction.fillDataTableToDataGrid(GrdTest, SqlChart, "-")
        Dim GetChart = StandardFunction.GetGraph(SqlChart)
        Dim Columnz = GetChart(0)
        Dim Dataz = GetChart(1)
        Dim Rowz = GetChart(2)

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "window-script", "

            //alert(" & Columnz & ")
            //alert(" & Dataz & ")
            //alert(" & Rowz & ")

            var x_row = " & Rowz & "
            var x_column = " & Columnz & "
            var stack = " & Dataz & "

            var colorSet = []
            var backgroundSet = []

            for (i = 0; i < x_column.length; i++) {
                var minR = 100, maxR = 255;
                var minG = 0, maxG = 150;
                var minB = 0, maxB = 150;

                var Red = Math.round(Math.random() * ((maxR - minR) + 1) + minR);
                var Green = Math.round(Math.random() * ((maxG - minG) + 1) + minG);
                var Blue = Math.round(Math.random() * ((maxB - minB) + 1) + minB);

                colorSet.push('rgba(' + Red + ',' + Green + ',' + Blue + ',0.5)');
                backgroundSet.push('rgba(' + Red + ',' + Green + ',' + Blue + ',1)');
            }
            colorSet.sort()
            backgroundSet.sort()

            var setData = []
            for (i = 0; i < (x_column.length); i++) {
                setData.push(
                    {
                        label: x_column[i],
                        data: stack[i],
                        backgroundColor: colorSet[i],
                        borderColor: backgroundSet[i],
                        borderWidth: 1
                    }
                )
            }

            var ctx = document.getElementById('myChart').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar',

                data: {
                    labels: x_row,
                    datasets: setData
                },
                options: {
                    scales: {
                        yAxes: [{
                            stacked: true,
                            ticks: {
                                beginAtZero: true,
                                stepSize: 50
                            },

                            scaleLabel: {
                                display: true,
                                labelString: 'จำนวน',
                            },
                        }],

                        xAxes: [{
                            stacked: true,
                            barPercentage: 0.5,
                            scaleLabel: {
                                display: true,
                                labelString: 'วันที่',
                            },
                        }]
                    },
                }
            });
        ", True)
    End Sub
End Class