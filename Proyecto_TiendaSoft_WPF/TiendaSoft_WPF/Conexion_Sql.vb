Imports System.Data.SqlClient
Imports System.Data

Public Class Conexion_Sql
    Property conexion As SqlConnection

    Function Conectar() As Boolean
        Try
            conexion = New SqlClient.SqlConnection(My.Settings.TiendaSoftConnectionString)
            conexion.Open()
            conexion.FireInfoMessageEventOnUserErrors = True
            RemoveHandler conexion.InfoMessage, AddressOf InfoMessage
            AddHandler conexion.InfoMessage, AddressOf InfoMessage
            Return True

        Catch ex As Exception
            MessageBox.Show("!Error de Conexión con el Servidor!", "Mensaje de Conexión", MessageBoxButton.OK, MessageBoxImage.Error)
            Return False
        End Try
    End Function

    Private Sub InfoMessage(ByVal sender As Object, ByVal e As System.Data.SqlClient.SqlInfoMessageEventArgs)
        If e.Message.Contains("SET") = False Then
            MessageBox.Show(e.Message, "Punto de Venta", MessageBoxButton.OK, MessageBoxImage.Information)
        End If
    End Sub
End Class
