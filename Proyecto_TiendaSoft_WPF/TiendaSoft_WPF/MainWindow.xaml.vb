﻿Imports DevExpress.Xpf.Bars
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

        '******* CARGAR EL LOGO Y EL NOMBRE DE LA EMPRESA
        If Not IsNothing(DatosEmpresa.Logo) Then
            Me.logo.Source = DatosEmpresa.Logo
        End If
        Me.lbl_nombre.Content = DatosEmpresa.Nombre

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

        barManager.Bars(0).ItemLinks("respaldo").IsVisible = xAdmin
        barManager.Bars(0).ItemLinks("config").IsVisible = xAdmin


        ''**** DESACTIVADOS HASTA TERMINARLOS ****
        barManager.Bars(0).ItemLinks("factura").IsEnabled = False   ' ** Boton Factura ** 
        'barManager.Bars(0).ItemLinks("reportes").IsEnabled = False  ' ** Boton Reportes **
        barManager.Bars(0).ItemLinks("gastos").IsEnabled = False  ' ** Boton Reportes **
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
    Public Sub ActualizarDatosEmpresa()
        If Not IsNothing(DatosEmpresa.Logo) Then
            Me.logo.Source = DatosEmpresa.Logo
        End If
        Me.lbl_nombre.Content = DatosEmpresa.Nombre
    End Sub
    Private Sub EjecutarRespaldo()

        Dim resp = MessageBox.Show("¿Desear continuar con el respaldo de la base de datos?", "Respaldo", MessageBoxButton.YesNo, MessageBoxImage.Information)
        If (Not resp = MessageBoxResult.Yes) Then
            Return
        End If

        Me.Cursor = Cursors.Wait
        If (Mi_conexion.Conectar) Then
            Try
                Dim res As DataTable = Mi_conexion.Ejecutar_query("EXEC [Global].[usp_Respaldar] ")
                If res.Rows.Count > 0 Then
                    If (res.Rows(0).Item("Error").ToString = "0") Then
                        Dim ruta As String = res.Rows(0).Item("Mensaje").ToString
                        Dim msg As String = String.Format("Respaldo realizado en la ruta {0}", ruta)
                        MessageBox.Show(msg, "", MessageBoxButton.OK, MessageBoxImage.Information)

                        Dim x = ruta.Split("\")
                        Dim xn As String = ""

                        For i = 0 To (x.Count - 2)
                            xn = xn & x(i) & "\"
                        Next
                        Process.Start(xn)
                    Else
                        MessageBox.Show("No se pudor hacer el respaldo." & vbNewLine & res.Rows(0).Item("Mensaje"), "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                    End If

                End If

            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try

        Else
            MessageBox.Show("No se pudo conectar a la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
        Me.Cursor = Cursors.Arrow
        restaurarBotones()

    End Sub


    '*********** EVENTOS UI ***********
    Private Sub actualizar_FechaHora(ByVal sender As Object, ByVal e As EventArgs)
        labelHora.Content = Date.Now.ToString("F")
        CommandManager.InvalidateRequerySuggested()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles btn_ventas.ItemClick, btn_productos.ItemClick, btn_configuracion.ItemClick, btn_corte.ItemClick, btn_inventario.ItemClick, btn_cuentas.ItemClick, btn_reportes.ItemClick, btn_gastos.ItemClick, btn_respaldo.ItemClick
        If (Not sender.name = "btn_respaldo") Then
            restaurarBotones()
            sender.IsEnabled = False
        End If

        Select Case sender.name
            Case "btn_ventas"
                navFrame.Navigate(New Frm_Ventas)

            Case "btn_configuracion"
                navFrame.Navigate(New Page_Configuracion(Me))

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

            Case "btn_respaldo"
                EjecutarRespaldo

        End Select
    End Sub
    Private Sub BorderSalir_clik() Handles border_salir.MouseLeftButtonUp
        Me.Close()
    End Sub
    Private Sub Minimizar_click() Handles border_minimizar.MouseLeftButtonUp
        Me.WindowState = WindowState.Minimized
    End Sub
    Private Sub saliendo(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing

        Dim formSaliendo As New Frm_Saliendo

        If (Not formSaliendo.ShowDialog()) Then
            e.Cancel = True
        End If

    End Sub
    Private Sub Me_update() Handles Me.SizeChanged
        Me.WindowState = WindowState.Maximized
    End Sub

    Private Sub Salir_desdeCorte()
    End Sub

    Sub editarProducto(p1 As String)
        restaurarBotones()
        btn_productos.IsEnabled = False
        navFrame.Navigate(New Page_productos(p1))
    End Sub

End Class