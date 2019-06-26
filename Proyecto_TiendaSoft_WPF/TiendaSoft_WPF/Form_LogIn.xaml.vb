Imports System.Data.SqlClient
Imports System.Data

Public Class Form_LogIn

    Private Sub me_loadedDone() Handles Me.Loaded
        ''Verificar La conexion de la base de Datos

        Mi_conexion = New Conexion_Sql
        If (Not Mi_conexion.Conectar) Then
            ''Formulario Confugurar Conexion
            Dim conexionForm As New Frm_Conexion
            Dim xres As Boolean = False
            xres = conexionForm.ShowDialog()
            If (xres) Then
                Dim xl As New Form_LogIn
                xl.Show()
                Me.Close()
            Else
                Me.Close()
            End If
        End If

        Me.lbl_name.Content = xNombre
        tb_usuario.Focus()
    End Sub

    Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs) Handles OKButton.Click
        Dim reader As SqlDataReader = Mi_conexion.Ejecutar_Procedimiento("[dbo].[Sys_Login]", {"cUsu", "cPass", "cApp"}, {tb_usuario.Text, tb_password.Password, strApp})
        Dim Logearse As String = ""
        Try
            While reader.Read
                Logearse = reader("Usuario")
            End While
            reader.Close()

            If (Logearse <> "") Then
                xOpererador = Val(Split(Logearse.ToString, "#")(0).ToString)
                xNombreUsuario = Split(Logearse.ToString, "#")(1).ToString
                xOpciones = Split(Logearse.ToString, "#")(2).ToString
                xAdmin = IIf(Split(Logearse.ToString, "#")(3).ToString = "1", True, False)
                Dim xStatus As Integer = CInt(Split(Logearse.ToString, "#")(4).ToString)

                If (xAdmin) Then
                    IniciarSesion(True)
                Else
                    Select Case (xStatus)
                        Case 0
                            IniciarSesion(False)
                        Case 1
                            MessageBox.Show("Usuario ya esta en uso", "", MessageBoxButton.OK, MessageBoxImage.Information)
                            ReanudarSesion()
                        Case 2
                            ReanudarSesion()
                    End Select
                End If
                
            Else
                MessageBox.Show("Usuario y/o contraseña no validos", "", MessageBoxButton.OK, MessageBoxImage.Stop)
            End If

        Catch ex As Exception
            MessageBox.Show("Se Perdio la Conexion con la Base de Datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
            Me.Close()
        End Try



    End Sub

    Private Sub btn_salir_onclic() Handles btn_salir.Click
        Me.Close()
    End Sub

    Private Sub IniciarSesion(admin As Boolean)
        Me.Visibility = Windows.Visibility.Hidden
        Dim xform As New Frm_Inicio(admin)
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

    Private Sub tb_password_keyInput(sender As Object, e As KeyEventArgs) Handles tb_password.KeyDown
        If e.Key = Key.Enter Then
            OKButton_Click(sender, Nothing)
        End If
    End Sub

End Class
