Imports System.Data.SqlClient
Imports System.Data

Public Class Form_LogIn

    Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs) Handles OKButton.Click
        If (Mi_conexion.Conectar()) Then
            Dim Cmd_Login = New SqlCommand
            Cmd_Login.CommandTimeout = 500
            Cmd_Login.CommandType = CommandType.StoredProcedure
            Cmd_Login.CommandText = "[PDV].[dbo].[Sys_Login]"
            Cmd_Login.Parameters.Clear()
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@cUsu", tb_usuario.Text))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@cPass", tb_password.Password))
            Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@cApp", strApp))

            Cmd_Login.Connection = Mi_conexion.conexion
            Dim reader As SqlDataReader = Cmd_Login.ExecuteReader()

            Dim Logearse As String = ""

            Try
                While reader.Read
                    Logearse = reader("Usuario")
                End While
            Catch ex As Exception
            Finally
                reader.Close()
            End Try

            If (Logearse <> "") Then
                xOpererador = Val(Split(Logearse.ToString, "#")(0).ToString)
                xNombreUsuario = Split(Logearse.ToString, "#")(1).ToString
                xOpciones = Split(Logearse.ToString, "#")(2).ToString
                xAdmin = IIf(Split(Logearse.ToString, "#")(3).ToString = "1", True, False)
                Dim xStatus As Int16 = CInt(Split(Logearse.ToString, "#")(4).ToString)

                Select Case (xStatus)
                    Case 0
                        IniciarSesion()
                    Case 1
                        MessageBox.Show("Usuario ya esta en uso", "", MessageBoxButton.OK, MessageBoxImage.Hand)
                    Case 2
                        ReanudarSesion()
                End Select

            End If

        Else
            MessageBox.Show("Usuario y/o Contraseña Incorrecto", "", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If


    End Sub
    Private Sub btn_salir_onclic() Handles btn_salir.Click
        Me.Close()
    End Sub

    Private Sub IniciarSesion()
        Me.Visibility = Windows.Visibility.Hidden
        Dim xform As New Frm_Inicio
        If (xform.ShowDialog()) Then
            Dim yform As New MainWindow
            yform.Show()
            Me.Close()
        Else
            Me.Visibility = Windows.Visibility.Visible
        End If

    End Sub
    Private Sub ReanudarSesion()
        Dim xForm As New Frm_Reanudar
        xForm.Show()

        Me.Close()

    End Sub
End Class
