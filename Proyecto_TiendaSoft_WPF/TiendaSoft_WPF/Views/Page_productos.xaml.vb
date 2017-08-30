Class Page_productos

    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        btn_catalagoP.IsEnabled = False
        Product_Frame.Source = New Uri("/TiendaSoft_WPF;component/Views/Productos/Page_product_catalagoProducto.xaml", UriKind.Relative)
    End Sub

    Sub buttonsClick(sender As Object, e As RoutedEventArgs) Handles btn_catalagoP.Click, btn_deapartamento.Click
        restaurarBotones()
        sender.IsEnabled = False

        Select Case sender.name
            Case "btn_catalagoP"
                Product_Frame.Source = New Uri("/TiendaSoft_WPF;component/Views/Productos/Page_product_catalagoProducto.xaml", UriKind.Relative)
            Case "btn_deapartamento"
                Product_Frame.Source = New Uri("/TiendaSoft_WPF;component/Views/Productos/Page_product_catalagoDepartamentos.xaml", UriKind.Relative)

        End Select
    End Sub

    Private Sub restaurarBotones()
        btn_catalagoP.IsEnabled = True
        btn_promocion.IsEnabled = True
        btn_deapartamento.IsEnabled = True
        btn_impportar.IsEnabled = True
    End Sub

End Class
