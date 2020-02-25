Imports DevExpress.Xpf.Bars
Imports System.Threading
Imports System.Globalization
Imports System.Data.SqlClient
Imports System.Data

Class MainWindow
    Dim dispatcherTimer As Windows.Threading.DispatcherTimer

    Private Sub rootGrid_onLoaded() Handles rootGrid.Loaded
        Thread.CurrentThread.CurrentCulture = New CultureInfo("es-MX")
        tb_version.Text = xversion
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

        Me.lbl_nombre.Content = xNombre

        ''Mostrar los Productos Bajos en inventario        
        ''Dim xtask As New Tasks.Task(AddressOf mostrarProductosBajos)
        ''xtask.Start()
        mostrarProductosBajos()

    End Sub

    Private Sub mostrarProductosBajos()
        Dim xInventBajos As New Inventarios_Bajos
        xInventBajos.Show()
    End Sub

    Private Sub VerificarOpciones()
        navFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden
        navFrame.Navigate(New Frm_Ventas)
        btn_ventas.IsEnabled = False

        Dim opcion1 As String = "0" 'VENTAS
        Dim opcion2 As String = "0" 'PRODUCTOS
        Dim opcion3 As String = "0" 'INVENTARIO
        Dim opcion4 As String = "0" 'FACTURA
        Dim opcion5 As String = "0" 'CORTE
        Dim opcion6 As String = "0" 'REPORTES
        Dim opcion7 As String = "0" 'CUENTAS
        Dim opcion8 As String = "0" 'GASTOS

        Try
            opcion1 = xOpciones.Substring(0, 1)
            opcion2 = xOpciones.Substring(1, 1)
            opcion3 = xOpciones.Substring(2, 1)
            opcion4 = xOpciones.Substring(3, 1)
            opcion5 = xOpciones.Substring(4, 1)
            opcion6 = xOpciones.Substring(5, 1)
            opcion7 = xOpciones.Substring(6, 1)
            opcion8 = xOpciones.Substring(7, 1)
        Catch ex As Exception
        End Try

        '*** ACTIVAR TODAS LOS BOTONES DEL MENU PRINCIPAL ***
        For Each Bar As BarItemLinkBase In barManager.Bars(0).ItemLinks
            Bar.IsEnabled = True
        Next

        barManager.Bars(0).ItemLinks("ventas").IsVisible = CBool(opcion1)
        barManager.Bars(0).ItemLinks("sventas").IsVisible = CBool(opcion1)
        barManager.Bars(0).ItemLinks("productos").IsVisible = CBool(opcion2)
        barManager.Bars(0).ItemLinks("sproductos").IsVisible = CBool(opcion2)
        barManager.Bars(0).ItemLinks("inventario").IsVisible = CBool(opcion3)
        barManager.Bars(0).ItemLinks("sinventario").IsVisible = CBool(opcion3)
        barManager.Bars(0).ItemLinks("factura").IsVisible = CBool(opcion4)
        barManager.Bars(0).ItemLinks("sfactura").IsVisible = CBool(opcion4)
        barManager.Bars(0).ItemLinks("cuentas").IsVisible = CBool(opcion5)
        barManager.Bars(0).ItemLinks("scuentas").IsVisible = CBool(opcion5)
        barManager.Bars(0).ItemLinks("gastos").IsVisible = CBool(opcion6)
        barManager.Bars(0).ItemLinks("sgastos").IsVisible = CBool(opcion6)
        barManager.Bars(0).ItemLinks("reportes").IsVisible = CBool(opcion7)
        barManager.Bars(0).ItemLinks("sreportes").IsVisible = CBool(opcion7)
        barManager.Bars(0).ItemLinks("corte").IsVisible = CBool(opcion8)

        barManager.Bars(0).ItemLinks("config").IsVisible = xAdmin
        barManager.Bars(0).ItemLinks("salir").IsVisible = True


        ''**** DESACTIVADOS HASTA TERMINARLOS ****
        barManager.Bars(0).ItemLinks("factura").IsEnabled = False   ' ** Boton Factura ** 
        barManager.Bars(0).ItemLinks("reportes").IsEnabled = False  ' ** Boton Reportes **
        barManager.Bars(0).ItemLinks("gastos").IsEnabled = False  ' ** Boton Reportes **
    End Sub

    Private Sub actualizar_FechaHora(ByVal sender As Object, ByVal e As EventArgs)
        labelHora.Content = Date.Now.ToString("F")
        CommandManager.InvalidateRequerySuggested()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles btn_ventas.ItemClick, btn_productos.ItemClick, btn_Salir.ItemClick, btn_configuracion.ItemClick, btn_corte.ItemClick, btn_inventario.ItemClick, btn_cuentas.ItemClick, btn_reportes.ItemClick, btn_gastos.ItemClick
        restaurarBotones()
        sender.IsEnabled = False

        Select Case sender.name
            Case "btn_ventas"
                navFrame.Navigate(New Frm_Ventas)

            Case "btn_configuracion"
                navFrame.Navigate(New Page_Configuracion)

            Case "btn_productos"
                navFrame.Navigate(New Page_productos)

            Case "btn_corte"
                navFrame.Navigate(New Page_Corte)

            Case "btn_cuentas"
                navFrame.Navigate(New Page_Cuentas)

            Case "btn_inventario"
                navFrame.Navigate(New Page_Inventario(Me))

            Case "btn_reportes"
                navFrame.Navigate(New Page_Reportes)

            Case "btn_gastos"
                navFrame.Navigate(New Page_Gastos)


            Case "btn_Salir"
                restaurarBotones()
                Me.Close()
        End Select
    End Sub

    Private Sub restaurarBotones()
        btn_ventas.IsEnabled = True
        btn_productos.IsEnabled = True
        btn_inventario.IsEnabled = True
        bnt_factura.IsEnabled = True
        btn_corte.IsEnabled = True
        btn_reportes.IsEnabled = True
        btn_configuracion.IsEnabled = True
        btn_cuentas.IsEnabled = True
        btn_gastos.IsEnabled = True
    End Sub

    Private Sub MostrarBotones(val As Boolean)
        For Each item As BarItemLinkBase In barManager.Bars(0).ItemLinks
            item.IsVisible = val
            item.IsEnabled = val
        Next
    End Sub

    Private Sub saliendo(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing

        Dim formSaliendo As New Frm_Saliendo
        If (formSaliendo.ShowDialog()) Then
            Dim form As New Form_LogIn
            form.Show()
        Else
            btn_Salir.IsEnabled = True
            e.Cancel = True
        End If
    End Sub

    Private Sub Salir_desdeCorte()


    End Sub

    Sub editarProducto(p1 As String)
        restaurarBotones()
        btn_productos.IsEnabled = False
        navFrame.Navigate(New Page_productos(p1))
    End Sub

End Class