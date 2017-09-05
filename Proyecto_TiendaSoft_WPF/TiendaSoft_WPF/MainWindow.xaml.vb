Imports DevExpress.Xpf.Bars

Class MainWindow
    Dim dispatcherTimer As Windows.Threading.DispatcherTimer

    Private Sub rootGrid_onLoaded() Handles rootGrid.Loaded
        MostrarBotones(False)

        If (xOpererador > 0 And xNombreUsuario.Length > 0 And xOpciones.Length > 0) Then

            dispatcherTimer = New Windows.Threading.DispatcherTimer()
            AddHandler dispatcherTimer.Tick, AddressOf actualizar_FechaHora
            dispatcherTimer.Interval = New TimeSpan(0, 0, 1)
            dispatcherTimer.Start()

            label_NombreOperador.Content = xNombreUsuario
            VerificarOpciones()
        Else
            Me.Close()
        End If

    End Sub

    Private Sub VerificarOpciones()
        navFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden
        navFrame.Source = New Uri("Views/Frm_Ventas.xaml", UriKind.Relative)
        btn_ventas.IsEnabled = False

        Dim opcion1 As String = "0"
        Dim opcion2 As String = "0"
        Dim opcion3 As String = "0"
        Dim opcion4 As String = "0"
        Dim opcion5 As String = "0"
        Dim opcion6 As String = "0"

        Try
            opcion1 = xOpciones.Substring(0, 1)
            opcion2 = xOpciones.Substring(1, 1)
            opcion3 = xOpciones.Substring(2, 1)
            opcion4 = xOpciones.Substring(3, 1)
            opcion5 = xOpciones.Substring(4, 1)
            opcion6 = xOpciones.Substring(5, 1)
        Catch ex As Exception
        End Try

        barManager.Bars(0).ItemLinks(0).IsEnabled = CBool(opcion1)
        barManager.Bars(0).ItemLinks(1).IsEnabled = CBool(opcion1)
        barManager.Bars(0).ItemLinks(2).IsEnabled = CBool(opcion2)
        barManager.Bars(0).ItemLinks(3).IsEnabled = CBool(opcion2)
        barManager.Bars(0).ItemLinks(4).IsEnabled = CBool(opcion3)
        barManager.Bars(0).ItemLinks(5).IsEnabled = CBool(opcion3)
        barManager.Bars(0).ItemLinks(6).IsEnabled = CBool(opcion4)
        barManager.Bars(0).ItemLinks(7).IsEnabled = CBool(opcion4)
        barManager.Bars(0).ItemLinks(8).IsEnabled = CBool(opcion5)
        barManager.Bars(0).ItemLinks(9).IsEnabled = CBool(opcion5)
        barManager.Bars(0).ItemLinks(10).IsEnabled = CBool(opcion6)
        barManager.Bars(0).ItemLinks(13).IsEnabled = xAdmin

        barManager.Bars(0).ItemLinks(0).IsVisible = CBool(opcion1)
        barManager.Bars(0).ItemLinks(1).IsVisible = CBool(opcion1)
        barManager.Bars(0).ItemLinks(2).IsVisible = CBool(opcion2)
        barManager.Bars(0).ItemLinks(3).IsVisible = CBool(opcion2)
        barManager.Bars(0).ItemLinks(4).IsVisible = CBool(opcion3)
        barManager.Bars(0).ItemLinks(5).IsVisible = CBool(opcion3)
        barManager.Bars(0).ItemLinks(6).IsVisible = CBool(opcion4)
        barManager.Bars(0).ItemLinks(7).IsVisible = CBool(opcion4)
        barManager.Bars(0).ItemLinks(8).IsVisible = CBool(opcion5)
        barManager.Bars(0).ItemLinks(9).IsVisible = CBool(opcion5)
        barManager.Bars(0).ItemLinks(10).IsVisible = CBool(opcion6)
        barManager.Bars(0).ItemLinks(13).IsVisible = xAdmin

        barManager.Bars(0).ItemLinks(11).IsEnabled = True ''btn Salir
        barManager.Bars(0).ItemLinks(11).IsVisible = True

    End Sub

    Private Sub actualizar_FechaHora(ByVal sender As Object, ByVal e As EventArgs)
        labelHora.Content = Date.Now.ToString("F")
        CommandManager.InvalidateRequerySuggested()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles btn_ventas.ItemClick, btn_productos.ItemClick, btn_Salir.ItemClick, btn_configuracion.ItemClick
        restaurarBotones()
        sender.IsEnabled = False

        Select Case sender.name
            Case "btn_ventas"
                navFrame.Source = New Uri("Views/Frm_Ventas.xaml", UriKind.Relative)

            Case "btn_configuracion"
                navFrame.Source = New Uri("Views/Page_Configuracion.xaml", UriKind.Relative)

            Case "btn_productos"
                navFrame.Source = New Uri("Views/Page_productos.xaml", UriKind.Relative)

            Case "btn_Salir"
                restaurarBotones()
                Me.Close()
        End Select
    End Sub

    Private Sub restaurarBotones()
        btn_ventas.IsEnabled = True
        btn_ventas.IsEnabled = True
        btn_productos.IsEnabled = True
        btn_inventario.IsEnabled = True
        bnt_factura.IsEnabled = True
        btn_corte.IsEnabled = True
        btn_reportes.IsEnabled = True
        btn_configuracion.IsEnabled = True
    End Sub

    Private Sub MostrarBotones(val As Boolean)
        For Each item As BarItemLinkBase In barManager.Bars(0).ItemLinks
            item.IsVisible = val
            item.IsEnabled = val
        Next
    End Sub


    Private Sub saliendo(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        If (MessageBox.Show("Esta saliendo del sistema, ¿Desea continuar?", "Saliendo", MessageBoxButton.YesNo, MessageBoxImage.Warning) = MessageBoxResult.Yes) Then
            Dim form As New Form_LogIn
            form.Show()
        Else
            e.Cancel = True
        End If
    End Sub

End Class
