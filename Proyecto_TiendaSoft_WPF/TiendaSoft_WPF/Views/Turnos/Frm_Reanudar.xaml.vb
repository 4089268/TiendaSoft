Imports System.Data.SqlClient
Imports System.Data

Public Class Frm_Reanudar


    Private Sub butons_click(sender As Object, e As Windows.RoutedEventArgs) Handles btn_continuar.Click, btn_nuevo.Click, btn_cancelar.Click
        Select Case (sender.name)
            Case "btn_continuar"
                reanudar()

            Case "btn_nuevo"
                Me.Visibility = Windows.Visibility.Hidden
                Dim xformA As New Frm_Corte
                If (xformA.ShowDialog) Then
                    Dim xFormB As New Frm_Inicio
                    If (xFormB.ShowDialog) Then
                        Dim x As New MainWindow
                        x.Show()
                        Me.Close()
                    Else
                        cancelar()
                    End If
                Else
                    cancelar()
                End If

            Case "btn_cancelar"
                cancelar()
        End Select
    End Sub

    Private Sub reanudar()
        If (Mi_conexion.Conectar()) Then
            Dim Cmd_Login = New SqlCommand
            Cmd_Login.CommandTimeout = 500
            Cmd_Login.CommandType = CommandType.StoredProcedure
            Cmd_Login.CommandText = "[PDV].[Global].[Sys_PSesiones]"
            Cmd_Login.Parameters.Clear()
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "REAUNUDAR"))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@idOperador", xOpererador))

            Cmd_Login.Connection = Mi_conexion.conexion
            Dim reader As SqlDataReader = Cmd_Login.ExecuteReader()

            Dim mensaje As String = ""

            Try
                While reader.Read
                    mensaje = reader("mensaje")
                End While
            Catch ex As Exception
            Finally
                reader.Close()
            End Try

            If (mensaje.Length > 0) Then
                Dim form As New MainWindow
                form.Show()
            End If

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)

        End If

        Me.Close()

    End Sub
    Private Sub cancelar()
        Dim xform As New Form_LogIn
        xform.Show()
        Me.Close()
    End Sub

End Class
