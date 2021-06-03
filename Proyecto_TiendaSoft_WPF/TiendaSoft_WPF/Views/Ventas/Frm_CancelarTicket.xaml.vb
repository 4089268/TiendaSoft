Imports System.Data
Imports System.Data.SqlClient

Public Class Frm_CancelarTicket

    Private IdVentaActual As Integer = -1

    Private Sub me_loaded_Done() Handles Me.Loaded
        Me.dp_fecha1.Text = Date.Now().ToString("dd/MM/yyyy")
        Me.dp_fecha2.Text = Date.Now().ToString("dd/MM/yyyy")

        Me.lbl_total.Content = "$0.00"
        Me.lbl_pago.Content = "$0.00"
        Me.lbl_cambio.Content = "$0.00"
        Me.lbl_tipoPago.Content = ""

        Me.CargarDatos()

    End Sub
    Private Sub CargarDatos()
        If Not Mi_conexion.Conectar Then
            Return
        End If

        Try
            Dim DatosVentas As New DataTable
            Dim params As New List(Of SqlParameter)
            params.Add(New SqlParameter("@Alias", "CARGARVENTAS"))
            params.Add(New SqlParameter("@Fecha1", CType(dp_fecha1.Text, Date).ToString("yyyyMMdd")))
            params.Add(New SqlParameter("@Fecha2", CType(dp_fecha2.Text, Date).ToString("yyyyMMdd")))
            Mi_conexion.Ejecutar_Procedimiento_dataAdapter("[Global].[SP_Ventas]", params).Fill(DatosVentas)
            Me.grid_ventas.ItemsSource = DatosVentas.DefaultView
            LimpiarCampos()
        Catch ex As Exception
            MessageBox.Show("Error" & vbNewLine & ex.Message)
            Console.WriteLine(ex.Message)
        End Try
    End Sub
    Private Sub CargarDatos_DetalleVenta(id_venta As Integer)
        If Not Mi_conexion.Conectar Then
            Return
        End If

        IdVentaActual = id_venta

        Dim DatosDetVentas As New DataTable
        Dim params As New List(Of SqlParameter)
        params.Add(New SqlParameter("@Alias", "CARGARDETVENTA"))
        params.Add(New SqlParameter("@IdVenta", id_venta))
        Mi_conexion.Ejecutar_Procedimiento_dataAdapter("[Global].[SP_Ventas]", params).Fill(DatosDetVentas)
        Me.grid_detVentas.ItemsSource = DatosDetVentas.DefaultView

    End Sub
    Private Sub LimpiarCampos()
        Me.lbl_total.Content = "$0.00"
        Me.lbl_pago.Content = "$0.00"
        Me.lbl_cambio.Content = "$0.00"
        Me.lbl_tipoPago.Content = ""
        Me.grid_detVentas.ItemsSource = Nothing
    End Sub


    '********** EVENTOS UI **********
    Private Sub BtnCargarDatos_Click() Handles btn_cargar.MouseLeftButtonUp
        Me.CargarDatos()
    End Sub
    Private Sub GridVentas_SelectionChanged() Handles grid_ventas.SelectionChanged
        If grid_ventas.SelectedIndex >= 0 Then
            Dim datosVenta As DataRowView = grid_ventas.SelectedItem

            Me.lbl_total.Content = "$" & Math.Round(CType(datosVenta("total"), Decimal), 2)
            Me.lbl_pago.Content = "$" & Math.Round(CType(datosVenta("pago"), Decimal), 2)
            Me.lbl_cambio.Content = "$" & Math.Round(CType(datosVenta("cambio"), Decimal), 2)
            Me.lbl_tipoPago.Content = datosVenta("tipoCobro")
            Me.CargarDatos_DetalleVenta(CType(datosVenta("id_venta"), Integer))
        End If
    End Sub
    Private Sub BtnCancelarVenta() Handles btn_cancelarVenta.MouseLeftButtonUp
        If IdVentaActual >= 0 Then

            If xAdmin Then

                If Mi_conexion.Conectar Then
                    Dim paramas As New List(Of SqlParameter)
                    paramas.Add(New SqlParameter("@Alias", "CANCELARVENTA"))
                    paramas.Add(New SqlParameter("@IdOperador", xOpererador))
                    paramas.Add(New SqlParameter("@IdVenta", IdVentaActual))

                    Try
                        Mi_conexion.Ejecutar_Procedimiento_dataReader("[Global].[SP_Ventas]", paramas)
                        MessageBox.Show("Venta Cancelada", "", MessageBoxButton.OK, MessageBoxImage.Information)
                    Catch ex As Exception
                        MessageBox.Show("No se pudo cancelar la venta", "", MessageBoxButton.OK, MessageBoxImage.Error)
                    End Try

                    CargarDatos()
                End If

            Else
                MessageBox.Show("Necesitas ser administrado")
                Dim validarCredenciales As New Form_LogIn(True)
                If (validarCredenciales.ShowDialog) Then

                    If Mi_conexion.Conectar Then
                        Dim paramas As New List(Of SqlParameter)
                        paramas.Add(New SqlParameter("@Alias", "CANCELARVENTA"))
                        paramas.Add(New SqlParameter("@IdOperador", validarCredenciales.DatosCredenciales.Opererador))
                        paramas.Add(New SqlParameter("@IdVenta", IdVentaActual))

                        Try
                            Mi_conexion.Ejecutar_Procedimiento_dataReader("[Global].[SP_Ventas]", paramas)
                            MessageBox.Show("Venta Cancelada", "", MessageBoxButton.OK, MessageBoxImage.Information)
                        Catch ex As Exception
                            MessageBox.Show("No se pudo cancelar la venta", "", MessageBoxButton.OK, MessageBoxImage.Error)
                        End Try

                        CargarDatos()

                    End If

                End If
            End If

        End If
    End Sub

End Class
