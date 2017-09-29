Imports System.Data.SqlClient
Imports System.Data

Public Class Frm_Saliendo
    Dim xdialogResult As Boolean = False


    Private Sub btn_abiertoClick() Handles btn_abierto.Click
        If (Mi_conexion.Conectar()) Then
            Dim Cmd_Login = New SqlCommand
            Cmd_Login.CommandTimeout = 500
            Cmd_Login.CommandType = CommandType.StoredProcedure
            Cmd_Login.CommandText = "[PDV].[Global].[Sys_PSesiones]"
            Cmd_Login.Parameters.Clear()
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "PAUSAR"))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@idOperador", xOpererador))

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
    Private Sub btn_cerrarClick() Handles btn_cerrar.Click
        Dim formulario As New Frm_Corte

        If (formulario.ShowDialog()) Then
            xdialogResult = True
            Me.Close()
        Else
            xdialogResult = False
            Me.Close()
        End If

    End Sub
    Private Sub btn_cancelarClick() Handles btn_cancelar.Click
        xdialogResult = False
        Me.Close()
    End Sub


    Private Sub saliendo() Handles Me.Closing
        Try
            Me.DialogResult = xdialogResult
        Catch ex As Exception
        End Try
    End Sub
End Class
