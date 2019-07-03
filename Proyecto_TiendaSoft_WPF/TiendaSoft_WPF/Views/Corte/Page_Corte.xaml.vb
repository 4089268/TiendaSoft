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

        '' **** BOTON IMPRIMIR DESACTIVADO HASTA  TERMINAR ***
        Me.btn_imprimir.Visibility = Windows.Visibility.Collapsed

        btn_Cajero.Focus()
        Crear_labels(1)
        executar_corteCaja(1)

        ''DESACTIVAR BOTON CERRARTURNO
        btn_cerrarTurno.IsEnabled = False
        btn_cerrarTurno.Visibility = Windows.Visibility.Collapsed
    End Sub

    Private Sub butons_click(sender As Object, e As RoutedEventArgs) Handles btn_Cajero.Click, btn_Dia.Click, btn_cerrarTurno.Click
        Select Case sender.name
            Case "btn_Cajero"
                Crear_labels(1)
                executar_corteCaja(1)

            Case "btn_Dia"
                Crear_labels(2)
                executar_corteCaja(2)

            Case "btn_cerrarTurno"
                btn_cerrarClick()
        End Select
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
                'btn_cerrarTurno.Visibility = Windows.Visibility.Visible

            Case 2
                lb_fecha.FontSize = 26
                lb_fecha.FontWeight = FontWeights.Medium
                lb_fecha.Content = "Corte del ***"
                sp_datos.Children.Add(lb_fecha)
                btn_cerrarTurno.Visibility = Windows.Visibility.Hidden
        End Select
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

                    sv_turnos.Visibility = Windows.Visibility.Collapsed
                    gridDineroCaja.Visibility = Windows.Visibility.Visible

                Case 2
                    cabecera1.Content = "Turnos del Dia"
                    gridDineroCaja.Visibility = Windows.Visibility.Collapsed
                    sv_turnos.Visibility = Windows.Visibility.Visible

                    sp_turnos.Children.Clear()
                    For Each xrow As DataRow In dataSet.Tables(2).Rows
                        Agr_DetalleTurnos(xrow.Item("nombre"), xrow.Item("fecha"), xrow.Item("m_inicial"), xrow.Item("m_final"), sp_turnos)
                    Next

            End Select

            tb_ventasTotales.Content = "$ " & ventasTotales.ToString("N2")
            tb_Ganancias.Content = "$ " & ganancias.ToString("N2")

            ''Ventas por tipo de pago
            Dim v_xefectivo As Decimal = dataSet.Tables(3).Rows(0).Item("Efectivo")
            Dim v_xtarjeta As Decimal = dataSet.Tables(3).Rows(0).Item("Tarjeta")
            Dim v_xtotal As Decimal = v_xefectivo + v_xtarjeta
            Me.v_Efectivo.Content = "$" & v_xefectivo.ToString("N2")
            Me.v_tarjeta.Content = "$" & v_xtarjeta.ToString("N2")
            Me.v_total.Content = "$" & v_xtotal.ToString("N2")


            ''EntradasDeEfectivo
            sp_entradas.Children.Clear()
            TotalEntradas.Content = dataSet.Tables(4).Rows.Count
            For Each xrow As DataRow In dataSet.Tables(4).Rows
                Select Case (tipo)
                    Case 1
                        Agr_EntradasSalidasEfectivo(1, xrow.Item("descripcion").ToString, xrow.Item("monto").ToString, "", "", sp_entradas)
                        lbl_Enombre.Visibility = Windows.Visibility.Collapsed
                        lbl_Ehora.Visibility = Windows.Visibility.Collapsed
                        lbl_Enombre.Margin = New Thickness(0, 0, 0, 0)
                        lbl_EDescripcion.Margin = New Thickness(10, 0, 0, 0)
                    Case 2
                        Agr_EntradasSalidasEfectivo(2, xrow.Item("descripcion").ToString, xrow.Item("monto").ToString, xrow.Item("nombre").ToString, xrow.Item("fecha").ToString, sp_entradas)
                        lbl_Enombre.Visibility = Windows.Visibility.Visible
                        lbl_Ehora.Visibility = Windows.Visibility.Visible
                        lbl_Enombre.Margin = New Thickness(10, 0, 0, 0)
                        lbl_EDescripcion.Margin = New Thickness(0, 0, 0, 0)
                End Select
            Next

            ''SalidasDeEfectivo
            sp_salidas.Children.Clear()
            totalSalidas.Content = dataSet.Tables(5).Rows.Count
            For Each xrow As DataRow In dataSet.Tables(5).Rows
                Select Case (tipo)
                    Case 1
                        Agr_EntradasSalidasEfectivo(1, xrow.Item("descripcion").ToString, xrow.Item("monto").ToString, "", "", sp_salidas)
                        lbl_Snombre.Visibility = Windows.Visibility.Collapsed
                        lbl_Shora.Visibility = Windows.Visibility.Collapsed
                        lbl_Snombre.Margin = New Thickness(0, 0, 0, 0)
                        lbl_SDescripcion.Margin = New Thickness(10, 0, 0, 0)
                    Case 2
                        Agr_EntradasSalidasEfectivo(2, xrow.Item("descripcion").ToString, xrow.Item("monto").ToString, xrow.Item("nombre").ToString, xrow.Item("fecha").ToString, sp_salidas)
                        lbl_Snombre.Visibility = Windows.Visibility.Visible
                        lbl_Shora.Visibility = Windows.Visibility.Visible
                        lbl_Snombre.Margin = New Thickness(10, 0, 0, 0)
                        lbl_SDescripcion.Margin = New Thickness(0, 0, 0, 0)
                End Select
            Next

            ''Detalle ventas
            sp_Ventas.Children.Clear()
            TotalVentas.Content = dataSet.Tables(7).Rows.Count
            For Each xrow As DataRow In dataSet.Tables(7).Rows
                Select Case (tipo)
                    Case 1
                        Agr_DetalleVentas(1, "", xrow.Item("fecha").ToString, xrow.Item("total").ToString, xrow.Item("pago").ToString, xrow.Item("cambio").ToString, xrow.Item("Tipo_Cobro").ToString, xrow.Item("id_venta").ToString, Me.sp_Ventas)
                        lbl_ventaNombre.Visibility = Windows.Visibility.Collapsed
                        lbl_fecha.Margin = New Thickness(25, 0, 0, 0)
                    Case 2
                        Agr_DetalleVentas(2, xrow.Item("nombre").ToString, xrow.Item("fecha").ToString, xrow.Item("total").ToString, xrow.Item("pago").ToString, xrow.Item("cambio").ToString, xrow.Item("Tipo_Cobro").ToString, xrow.Item("id_venta").ToString, Me.sp_Ventas)
                        lbl_ventaNombre.Visibility = Windows.Visibility.Visible
                        lbl_fecha.Margin = New Thickness(0, 0, 0, 0)
                End Select
            Next

            '' DATOS VENDEDOR 
            Select Case tipo
                Case 1
                    lb_nombre.Content = "Corte de " & dataSet.Tables(6).Rows(0).Item("nombre") & " del " & dataSet.Tables(6).Rows(0).Item("fecha")
                    lb_fecha.Content = " De " & dataSet.Tables(6).Rows(0).Item("h_inicial") & " a las " & dataSet.Tables(6).Rows(0).Item("h_final") & " - (Turno Actual)"
                Case 2
                    lb_fecha.Content = " Corte del  " & dataSet.Tables(6).Rows(0).Item("fecha")
            End Select

            Mi_conexion.cerrarConexion()
        Catch ex As Exception
            MessageBox.Show("ALGO SALIO MAL :(", "ERR", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try

    End Sub

    Private Sub Agr_EntradasSalidasEfectivo(xtipo As Integer, descripcion As String, monto As String, nombre As String, fecha As String, stackpanel As StackPanel)
        Dim mySp As New StackPanel
        mySp.Orientation = Orientation.Horizontal
        mySp.Margin = New Thickness(10, 5, 10, 5)

        If (xtipo = 2) Then
            Dim lbl_nombre As New Label
            lbl_nombre.Content = nombre
            lbl_nombre.Style = CType(FindResource("TableData"), Style)
            lbl_nombre.MinWidth = 150
            mySp.Children.Add(lbl_nombre)

            Dim lbl_fecha As New Label
            lbl_fecha.Content = fecha
            lbl_fecha.Style = CType(FindResource("TableData"), Style)
            lbl_fecha.MinWidth = 110
            mySp.Children.Add(lbl_fecha)
        End If

        Dim lbl_descripcion As New Label
        lbl_descripcion.Content = descripcion
        lbl_descripcion.Style = CType(FindResource("TableData"), Style)
        lbl_descripcion.MinWidth = 160
        mySp.Children.Add(lbl_descripcion)


        Dim lbl_monto As New Label
        lbl_monto.Content = monto
        lbl_monto.Style = CType(FindResource("TableData"), Style)
        lbl_monto.MinWidth = 110
        mySp.Children.Add(lbl_monto)

        stackpanel.Children.Add(mySp)

    End Sub

    Private Sub Agr_DetalleVentas(xtipo As Integer, nombre As String, fecha As String, total As String, pago As String, cambio As String, tipoCobro As String, idventa As Integer, stackpanel As StackPanel)
        Dim myButton As New Button
        myButton.Background = Brushes.Transparent
        myButton.BorderThickness = New Thickness(0)

        Dim mySp As New StackPanel
        mySp.Orientation = Orientation.Horizontal

        If (xtipo = 2) Then
            Dim lbl_nombre As New Label
            lbl_nombre.Content = nombre
            lbl_nombre.Style = CType(FindResource("TableData"), Style)
            lbl_nombre.Width = 150
            lbl_nombre.Margin = New Thickness(1, 0, 1, 0)
            lbl_nombre.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
            mySp.Children.Add(lbl_nombre)
        End If

        Dim lbl_fecha As New Label
        lbl_fecha.Content = fecha
        lbl_fecha.Style = CType(FindResource("TableData"), Style)
        lbl_fecha.Width = 110
        lbl_fecha.Margin = New Thickness(1, 0, 1, 0)
        lbl_fecha.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_fecha)

        Dim lbl_total As New Label
        lbl_total.Content = total
        lbl_total.Style = CType(FindResource("TableData"), Style)
        lbl_total.Width = 110
        lbl_total.Margin = New Thickness(1, 0, 1, 0)
        lbl_total.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_total)

        Dim lbl_pago As New Label
        lbl_pago.Content = pago
        lbl_pago.Style = CType(FindResource("TableData"), Style)
        lbl_pago.Width = 110
        lbl_pago.Margin = New Thickness(1, 0, 1, 0)
        lbl_pago.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_pago)

        Dim lbl_cambio As New Label
        lbl_cambio.Content = cambio
        lbl_cambio.Style = CType(FindResource("TableData"), Style)
        lbl_cambio.Width = 110
        lbl_cambio.Margin = New Thickness(1, 0, 1, 0)
        lbl_cambio.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_cambio)

        Dim lbl_tCobro As New Label
        lbl_tCobro.Content = tipoCobro
        lbl_tCobro.Style = CType(FindResource("TableData"), Style)
        lbl_tCobro.Width = 110
        lbl_tCobro.Margin = New Thickness(1, 0, 1, 0)
        lbl_tCobro.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_tCobro)

        Dim lbl_idventa As New TextBox
        lbl_idventa.Text = idventa
        lbl_idventa.Visibility = Windows.Visibility.Collapsed
        mySp.Children.Add(lbl_idventa)

        myButton.Content = mySp
        stackpanel.Children.Add(myButton)

        Try
            RemoveHandler myButton.Click, AddressOf DetalleVenta_click
        Catch ex As Exception
        End Try
        AddHandler myButton.Click, AddressOf DetalleVenta_click

    End Sub
    Private Sub Agr_DetalleTurnos(nombre As String, fecha As String, m_inicial As String, m_final As String, stackpanel As StackPanel)
        Dim mySp As New StackPanel
        mySp.Orientation = Orientation.Horizontal

        Dim lbl_nombre As New Label
        lbl_nombre.Content = nombre
        lbl_nombre.Style = CType(FindResource("TableData"), Style)
        lbl_nombre.Width = 160
        lbl_nombre.Margin = New Thickness(1, 0, 1, 0)
        lbl_nombre.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_nombre)

        Dim lbl_fecha As New Label
        lbl_fecha.Content = fecha
        lbl_fecha.Style = CType(FindResource("TableData"), Style)
        lbl_fecha.Width = 160
        lbl_fecha.Margin = New Thickness(1, 0, 1, 0)
        lbl_fecha.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_fecha)

        Dim lbl_minicial As New Label
        lbl_minicial.Content = m_inicial
        lbl_minicial.Style = CType(FindResource("TableData"), Style)
        lbl_minicial.Width = 120
        lbl_minicial.Margin = New Thickness(1, 0, 1, 0)
        lbl_minicial.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_minicial)

        Dim lbl_mfinal As New Label
        lbl_mfinal.Content = m_final
        lbl_mfinal.Style = CType(FindResource("TableData"), Style)
        lbl_mfinal.Width = 120
        lbl_mfinal.Margin = New Thickness(1, 0, 1, 0)
        lbl_mfinal.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mySp.Children.Add(lbl_mfinal)

        stackpanel.Children.Add(mySp)

    End Sub

    Private Sub DetalleVenta_click(Sender As Object, e As RoutedEventArgs)

        For Each xobject As Object In Sender.content.Children
            If (xobject.GetType.FullName = "System.Windows.Controls.TextBox") Then
                Dim xform As New Frm_DetalleVenta(xobject.Text.ToString)
                xform.ShowDialog()
            End If
        Next
    End Sub

    Private Sub btn_cerrarClick()
        ''Dim formulario As New Frm_Corte

        ''If (formulario.ShowDialog()) Then
        ''  Application.Current.Windows(0).Close()
        ''End If
    End Sub
End Class
