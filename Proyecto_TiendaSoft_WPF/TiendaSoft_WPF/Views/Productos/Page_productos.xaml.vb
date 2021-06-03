Class Page_productos

    Private _p1 As String = ""

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(p1 As String)
        InitializeComponent()
        _p1 = p1
    End Sub

    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        btn_catalagoP.IsEnabled = False
        If (_p1.Length > 0) Then
            Product_Frame.Navigate(New Page_product_catalagoProducto(_p1))
        Else
            Product_Frame.Source = New Uri("/TIENDASOFT;component/Views/Productos/Page_product_catalagoProducto.xaml", UriKind.Relative)
        End If

    End Sub

    Sub buttonsClick(sender As Object, e As RoutedEventArgs) Handles btn_catalagoP.Click, btn_deapartamento.Click, btn_ubicaciones.Click
        restaurarBotones()
        sender.IsEnabled = False

        Select Case sender.name
            Case "btn_catalagoP"
                Product_Frame.Source = New Uri("/TIENDASOFT;component/Views/Productos/Page_product_catalagoProducto.xaml", UriKind.Relative)
            Case "btn_deapartamento"
                Product_Frame.Source = New Uri("/TIENDASOFT;component/Views/Productos/Page_product_catalagoDepartamentos.xaml", UriKind.Relative)
            Case "btn_ubicaciones"
                Product_Frame.Source = New Uri("/TIENDASOFT;component/Views/Productos/Page_product_catalogoUbicaciones.xaml", UriKind.Relative)

        End Select
    End Sub

    Private Sub restaurarBotones()
        btn_catalagoP.IsEnabled = True
        btn_deapartamento.IsEnabled = True
        btn_ubicaciones.IsEnabled = True

        btn_promocion.IsEnabled = False
        btn_impportar.IsEnabled = False

    End Sub

    
End Class
