Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Public Class Frm_Corte
    Dim xDialogResult As Boolean = False

    Private Sub rootGrid_onLoaded() Handles rootGrid.Loaded
        System.Threading.Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")

        If (Mi_conexion.Conectar()) Then
            Dim Cmd_Login = New SqlCommand
            Cmd_Login.CommandTimeout = 500
            Cmd_Login.CommandType = CommandType.StoredProcedure
            Cmd_Login.CommandText = "[PDV].[Global].[Sys_PSesiones]"
            Cmd_Login.Parameters.Clear()
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "ESPERADO"))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@idOperador", xOpererador))

            Cmd_Login.Connection = Mi_conexion.conexion
            Dim reader As SqlDataReader = Cmd_Login.ExecuteReader()

            Try
                While reader.Read
                    tb_montoEsperado.Text = CDec(reader("m_esperado")).ToString("#,##0.00")
                End While

            Catch ex As Exception
                xDialogResult = False
                Me.Close()
            Finally
                reader.Close()
            End Try

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
            xDialogResult = False
            Me.Close()
        End If
    End Sub

    Private Sub btn_cerrarClick() Handles btn_cerrar.Click
        If (Mi_conexion.Conectar()) Then
            Dim Cmd_Login = New SqlCommand
            Cmd_Login.CommandTimeout = 500
            Cmd_Login.CommandType = CommandType.StoredProcedure
            Cmd_Login.CommandText = "[PDV].[Global].[Sys_PSesiones]"
            Cmd_Login.Parameters.Clear()
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "TERMINAR"))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@idOperador", xOpererador))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xMonto", CDec(tb_montoFinal.Text)))

            Cmd_Login.Connection = Mi_conexion.conexion
            Dim reader As SqlDataReader = Cmd_Login.ExecuteReader()

            Try
                While reader.Read
                    If (reader("mensaje") = "TERMINADO") Then
                        xDialogResult = True
                    Else
                        MessageBox.Show("Error al conectarse con la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
                        xDialogResult = False
                    End If
                End While

            Catch ex As Exception
                xDialogResult = False
                MessageBox.Show("Error al conectarse con la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
            Finally
                reader.Close()
                Me.Close()
            End Try

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
            xDialogResult = False
            Me.Close()
        End If

        Me.Close()
    End Sub
    Private Sub btn_CancelarClick() Handles btn_Cancelar.Click
        xDialogResult = False
        Me.Close()
    End Sub

    Private Sub validarNumeros(sender As Object, e As TextCompositionEventArgs) Handles tb_montoFinal.PreviewTextInput
        Dim regex As New System.Text.RegularExpressions.Regex("([^0-9]+).([0-9]*)")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

    Private Sub tb_montoFinal_changed() Handles tb_montoFinal.EditValueChanged
        Try
            tb_diferencia.Text = (CDec(tb_montoFinal.Value) - CDec(tb_montoEsperado.Text)).ToString("#,##0.00")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Saliedno() Handles Me.Closing
        Me.DialogResult = xDialogResult
    End Sub

End Class
