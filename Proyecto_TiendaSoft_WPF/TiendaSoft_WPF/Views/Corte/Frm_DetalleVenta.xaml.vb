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
            dg_ventas.ItemsSource = dataSet.Tables(0).DefaultView

            tb_importe.Text = dataSet.Tables(1).Rows(0).Item("total")
            tb_ganancia.Text = dataSet.Tables(1).Rows(0).Item("ganancia")
            tb_fecha.Text = dataSet.Tables(1).Rows(0).Item("fecha")
            tb_operador.Text = dataSet.Tables(1).Rows(0).Item("operador")
            tb_tipoPago.Text = dataSet.Tables(1).Rows(0).Item("tipoCobro")
        Else
            MessageBox.Show("Error al conectarse con la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub

End Class
