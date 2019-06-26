Imports System.Data.SqlClient
Imports System.Data

Public Class Nueva_cuenta
    Dim xguardado As Boolean = False

    Private Sub me_loadedDone() Handles Me.Loaded
        tb_direccion.Text = ""
        tb_nombre.Text = ""
        tb_saldo.Text = ""
        tb_telefono.Text = ""
    End Sub

    Private Sub boton_agregar_click() Handles btn_Guardar.Click
        Try
            If tb_nombre.Text.Length > 0 And tb_direccion.Text.Length > 0 And tb_saldo.Text.Length > 0 And tb_telefono.Text.Length > 0 Then
                If (Mi_conexion.Conectar) Then
                    Dim SqlComand = New SqlCommand
                    SqlComand.CommandTimeout = 500
                    SqlComand.CommandType = CommandType.StoredProcedure
                    SqlComand.CommandText = "[Global].[Sys_PCuentas]"
                    SqlComand.Parameters.Clear()
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "NUEVO"))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xNombre", tb_nombre.Text))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xDireccion", tb_direccion.Text))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xTelefono", tb_telefono.Text))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xSaldo", tb_saldo.Text))


                    SqlComand.Connection = Mi_conexion.conexion
                    Dim DataAdapter As New SqlDataAdapter(SqlComand)
                    Dim dataSet As New DataSet

                    DataAdapter.Fill(dataSet, "Resultado")
                    'MessageBox.Show(dataSet.Tables(0).Rows(0).Item("mensaje"), "", MessageBoxButton.OK, MessageBoxImage.Information)
                Else
                    MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                End If
            Else
                MessageBox.Show("FALTAN DATOS POR CAPTURAR", "", MessageBoxButton.OK, MessageBoxImage.Exclamation)
            End If

        Catch ex As Exception
            MessageBox.Show("ALGO SALIO MAL :(", "", MessageBoxButton.OK, MessageBoxImage.Error)
            Me.Close()
        End Try

        xguardado = True
        Me.Close()
    End Sub

    Private Sub me_close_donde() Handles Me.Closing
        Me.DialogResult = xguardado
    End Sub

    Private Sub validar_numeros(sender As Object, e As TextCompositionEventArgs) Handles tb_telefono.PreviewTextInput, tb_saldo.PreviewTextInput
        Try
            Dim regex As System.Text.RegularExpressions.Regex

            Select Case sender.name
                Case "tb_telefono"
                    regex = New System.Text.RegularExpressions.Regex("[^0-9]+")
                Case "tb_saldo"
                    regex = New System.Text.RegularExpressions.Regex("([^0-9,]+).([^0-9]{2}+)")
            End Select
            e.Handled = regex.IsMatch(e.Text)

        Catch ex As Exception
        End Try
    End Sub
End Class
