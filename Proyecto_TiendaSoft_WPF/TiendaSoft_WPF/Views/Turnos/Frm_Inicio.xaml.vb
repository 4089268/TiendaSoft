Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Threading

Public Class Frm_Inicio
    Dim xdialogResult As Boolean = False

    Private Sub btn_iniciar() Handles btn_start.Click

        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")

        If (Mi_conexion.Conectar()) Then
            Dim Cmd_Login = New SqlCommand
            Cmd_Login.CommandTimeout = 500
            Cmd_Login.CommandType = CommandType.StoredProcedure
            Cmd_Login.CommandText = "[PDV].[Global].[Sys_PSesiones]"
            Cmd_Login.Parameters.Clear()
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "INICIAR"))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@idOperador", xOpererador))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xMonto", CDec(tb_monto.Text)))

            Cmd_Login.Connection = Mi_conexion.conexion
            Dim reader As SqlDataReader = Cmd_Login.ExecuteReader()

            Dim mensaje As String = ""
            Try
                While reader.Read
                    mensaje = reader("mensaje")
                End While
                xdialogResult = True
            Catch ex As Exception
                xdialogResult = False
            Finally
                reader.Close()
            End Try

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
            xdialogResult = False
        End If

        Me.Close()
    End Sub

    Private Sub Saliendo() Handles Me.Closing
        Me.DialogResult = xdialogResult
    End Sub

    Private Sub validar_numeros(sender As Object, e As TextCompositionEventArgs) Handles tb_monto.PreviewTextInput
        Dim regex As New System.Text.RegularExpressions.Regex("([^0-9]+).([0-9]*)")
        e.Handled = regex.IsMatch(e.Text)
    End Sub
End Class
