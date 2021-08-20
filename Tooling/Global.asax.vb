Imports System.Web.Optimization

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Private Shared Sub RegisterRoutes(routes As RouteCollection)
        'routes.MapPageRoute("test", "test", "~/test.aspx")
        routes.MapPageRoute("LocationDetails", "LocationDetails/LocationId", "~/LocationDetails.aspx")
    End Sub
End Class