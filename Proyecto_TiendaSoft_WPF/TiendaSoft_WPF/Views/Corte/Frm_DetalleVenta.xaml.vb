Imports System.Data.SqlClient
Imports System.Data

Public Class Frm_DetalleVenta
    Public Sub New(id_ventas As String)
        InitializeComponent()
        Me.Title = "Detalle Venta:  " & id_ventas.ToString
        CargarDatos(id_ventas)
    End Sub

    Private Sub CargarDatos(id_ventas As String)
        If (Mi_conexion.Conectar()) Then
            Dim Cmd_Procedure = New SqlCommand
            Cmd_Procedure.CommandTimeout = 500
            Cmd_Procedure.CommandType = CommandType.StoredProcedure
            Cmd_Procedure.CommandText = "[Global].[Sys_Mostrar_DetalleVenta]"
            Cmd_Procedure.Parameters.Clear()

            Cmd_Procedure.Parameters.Add(New SqlClient.SqlParameter("@idVenta", id_ventas))

            Cmd_Procedure.Connection = Mi_conexion.conexion

            Dim sqlAdapter As New SqlDataAdapter
            sqlAdapter.SelectCommand = Cmd_Procedure

            Dim dataSet As DataSet = New DataSet
            sqlAdapter.Fill(dataSet)

            For Each Row As DataRow In dataSet.Tables(0).Rows
                agregar_ventas(Row.Item("Descripcion").ToString, Row.Item("precio_c").ToString, Row.Item("precio_v").ToString, Row.Item("cantidad").ToString, Row.Item("importe").ToString, Row.Item("ganancia").ToString)
            Next


            lbl_importe.Content = (dataSet.Tables(1).Rows(0).Item("total"))
            lbl_ganancia.Content = (dataSet.Tables(1).Rows(0).Item("gananciasTotal"))

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)

        End If
    End Sub

    Private Sub agregar_ventas(descripcion As String, precio_c As String, precio_v As String, cantidad As String, importe As String, ganancia As String)
        Dim lb_descripcion As New Label
        lb_descripcion.Width = 280
        lb_descripcion.FontWeight = FontWeights.Medium
        lb_descripcion.Content = descripcion

        Dim lb_precio_C As New Label
        lb_precio_C.Width = 110
        lb_precio_C.FontWeight = FontWeights.Medium
        lb_precio_C.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        lb_precio_C.Content = precio_c

        Dim lb_precio_V As New Label
        lb_precio_V.Width = 110
        lb_precio_V.FontWeight = FontWeights.Medium
        lb_precio_V.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        lb_precio_V.Content = precio_v

        Dim lb_cantidad As New Label
        lb_cantidad.Width = 80
        lb_cantidad.FontWeight = FontWeights.Medium
        lb_cantidad.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        lb_cantidad.Content = cantidad

        Dim lb_importe As New Label
        lb_importe.Width = 130
        lb_importe.FontWeight = FontWeights.Medium
        lb_importe.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        lb_importe.Content = importe


        Dim lb_ganancia As New Label
        lb_ganancia.Width = 110
        lb_ganancia.FontWeight = FontWeights.Medium
        lb_ganancia.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        lb_ganancia.Content = ganancia

        Dim myStackPanel As New StackPanel
        myStackPanel.Orientation = Orientation.Horizontal
        myStackPanel.Margin = New Thickness(10, 0, 0, 0)

        myStackPanel.Children.Add(lb_descripcion)
        myStackPanel.Children.Add(lb_precio_C)
        myStackPanel.Children.Add(lb_precio_V)
        myStackPanel.Children.Add(lb_cantidad)
        myStackPanel.Children.Add(lb_importe)
        myStackPanel.Children.Add(lb_ganancia)

        Me.sp_datos.Children.Add(myStackPanel)

    End Sub


End Class
