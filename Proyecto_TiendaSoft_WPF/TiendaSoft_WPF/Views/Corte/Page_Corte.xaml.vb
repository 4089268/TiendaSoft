Imports System.Data.SqlClient
Imports System.Data
Imports System.Threading
Imports System.Globalization

Class Page_Corte
    Dim lb_nombre As Label
    Dim lb_fecha As Label

    Private Sub rootLayout_loaded() Handles rootLayout.Loaded
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")

        Crear_labels(1)
        btn_Cajero.Focus()
        Crear_labels(1)
        executar_corteCaja(1)
    End Sub

    Private Sub executar_corteCaja(tipo As Int16)
        Try
            Mi_conexion.Conectar()

            Dim sqlAdapter As SqlDataAdapter = Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[Sys_CorteCaja]", {"xtipo", "xidOperador"}, {tipo, xOpererador})
            Dim dataSet As DataSet = New DataSet
            sqlAdapter.Fill(dataSet)

            Dim ventasTotales As Decimal = dataSet.Tables(0).Rows(0).Item("Ventas_Totales")
            Dim ganancias As Decimal = dataSet.Tables(1).Rows(0).Item("Ganancias")

            Select Case (tipo)
                Case 1
                    cabecera1.Content = "Dinero en Caja"
                    gridDineroCaja.Visibility = Windows.Visibility.Visible
                    Dim c_xInicial As Decimal = dataSet.Tables(2).Rows(0).Item("m_inicial")
                    Dim c_xEntradas As Decimal = dataSet.Tables(2).Rows(0).Item("Entradas")
                    Dim c_xSalidas As Decimal = dataSet.Tables(2).Rows(0).Item("Salidas")
                    Dim v_xefectivo2 As Decimal = dataSet.Tables(3).Rows(0).Item("Efectivo")
                    Dim c_xTotal As Decimal = (c_xInicial + c_xEntradas + v_xefectivo2 - c_xSalidas)


                    c_mInicial.Content = "$" & c_xInicial.ToString("N2")
                    c_Entradas.Content = "+ $" & c_xEntradas.ToString("N2")
                    c_Salidas.Content = "- $" & c_xSalidas.ToString("N2")
                    c_Ventas.Content = "+ $" & v_xefectivo2.ToString("N2")

                    c_Total.Content = "$" & c_xTotal.ToString("N2")

                    dg_turnos.Visibility = Windows.Visibility.Collapsed
                    gridDineroCaja.Visibility = Windows.Visibility.Visible

                Case 2
                    cabecera1.Content = "Turnos del Dia"
                    gridDineroCaja.Visibility = Windows.Visibility.Collapsed
                    dg_turnos.Visibility = Windows.Visibility.Visible
                    dg_turnos.ItemsSource = dataSet.Tables(2).DefaultView
            End Select

            tb_ventasTotales.Content = "$ " & ventasTotales.ToString("N2")
            tb_Ganancias.Content = "$ " & ganancias.ToString("N2")

            ''****** VENTAS POR TIPO DE PAGO *****
            Dim v_xefectivo As Decimal = dataSet.Tables(3).Rows(0).Item("Efectivo")
            Dim v_xtarjeta As Decimal = dataSet.Tables(3).Rows(0).Item("Tarjeta")
            Dim v_xtotal As Decimal = v_xefectivo + v_xtarjeta
            Me.v_Efectivo.Content = "$" & v_xefectivo.ToString("N2")
            Me.v_tarjeta.Content = "$" & v_xtarjeta.ToString("N2")
            Me.v_total.Content = "$" & v_xtotal.ToString("N2")


            TotalEntradas.Content = dataSet.Tables(4).Rows.Count
            totalSalidas.Content = dataSet.Tables(5).Rows.Count
            dg_entradas.ItemsSource = dataSet.Tables(4).DefaultView
            dg_salidas.ItemsSource = dataSet.Tables(5).DefaultView
            TotalVentas.Content = dataSet.Tables(7).Rows.Count
            dg_ventas.ItemsSource = dataSet.Tables(7).DefaultView

            Select Case tipo
                Case 1
                    dg_salidas_nombre.Visibility = Visibility.Collapsed
                    dg_salidas_hora.Visibility = Visibility.Collapsed
                    dg_entradas_nombre.Visibility = Visibility.Collapsed
                    dg_entradas_hora.Visibility = Visibility.Collapsed
                    dg_ventas_nombre.Visibility = Visibility.Collapsed

                    lb_nombre.Content = "Corte de " & dataSet.Tables(6).Rows(0).Item("nombre") & " del " & dataSet.Tables(6).Rows(0).Item("fecha")
                    lb_fecha.Content = " De " & dataSet.Tables(6).Rows(0).Item("h_inicial") & " a las " & dataSet.Tables(6).Rows(0).Item("h_final") & " - (Turno Actual)"
                Case 2
                    dg_salidas_nombre.Visibility = Visibility.Visible
                    dg_salidas_hora.Visibility = Visibility.Visible
                    dg_entradas_nombre.Visibility = Visibility.Visible
                    dg_entradas_hora.Visibility = Visibility.Visible
                    dg_ventas_nombre.Visibility = Visibility.Visible

                    lb_fecha.Content = " Corte del  " & dataSet.Tables(6).Rows(0).Item("fecha")
            End Select

            Mi_conexion.cerrarConexion()
        Catch ex As Exception
            MessageBox.Show("ALGO SALIO MAL :(", "ERR", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub Crear_labels(x As Integer)
        Me.lb_nombre = New Label
        Me.lb_fecha = New Label
        sp_datos.Children.Clear()
        Select Case x
            Case 1
                lb_nombre.FontSize = 18
                lb_nombre.FontWeight = FontWeights.Medium
                lb_nombre.Content = "Corte de ** del **"

                lb_fecha.FontSize = 18
                lb_fecha.FontWeight = FontWeights.Medium
                lb_fecha.Content = "De ** a las ** - (Turno Actual)"

                sp_datos.Children.Add(lb_nombre)
                sp_datos.Children.Add(lb_fecha)

            Case 2
                lb_fecha.FontSize = 26
                lb_fecha.FontWeight = FontWeights.Medium
                lb_fecha.Content = "Corte del ***"
                sp_datos.Children.Add(lb_fecha)
        End Select
    End Sub

    '********** EVENTOS UI  ********
    Private Sub butons_click(sender As Object, e As RoutedEventArgs) Handles btn_Cajero.Click, btn_Dia.Click
        Select Case sender.name
            Case "btn_Cajero"
                Crear_labels(1)
                executar_corteCaja(1)

            Case "btn_Dia"
                Crear_labels(2)
                executar_corteCaja(2)
        End Select
    End Sub
    Private Sub DetalleVenta_click(Sender As Object, e As RoutedEventArgs) Handles dg_ventas.SelectionChanged
        If Not IsNothing(dg_ventas.SelectedItem) Then
            Dim idVenta As Integer = dg_ventas.SelectedItem.Row.item("id_venta")
            Dim xform As New Frm_DetalleVenta(idVenta.ToString)
            xform.ShowDialog()
        End If
    End Sub
    Private Sub btn_imprimir_click() Handles btn_imprimir.Click
        MessageBox.Show("btn imprimir click")
    End Sub

End Class
