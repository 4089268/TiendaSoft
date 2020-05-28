Imports System.Data.SqlClient
Imports System.Data

Public Class Form_LogIn
    Property ValidarDatos As Boolean = False
    Property DatosCredenciales As Credenciales

    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(validar As Boolean)
        InitializeComponent()

        ValidarDatos = True
        '***** CUANDO SE NECECITE EJECUTAR UNA FUNCION CON CON CREDENCIALES DE ADMINISTRADOR 
        '***** SE LLAMARA PARA LOGEARSE CON DATOS DE ADMIN 
        If (validar) Then
            Me.rootGrid.RowDefinitions(0).Height = New GridLength(0)
            lbl_titulo.Content = "Comprobando credenciales"
            Me.Height = 180
        End If
    End Sub

    Private Sub me_loadedDone() Handles Me.Loaded
        If Not IsNothing(DatosEmpresa.Logo) Then
            Me.img_logo.Source = DatosEmpresa.Logo
        End If

        Me.lbl_name.Content = DatosEmpresa.Nombre
        tb_usuario.Focus()
    End Sub


    '********* EVENTOS UI *********
    Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs) Handles OKButton.MouseLeftButtonUp

        If Not Mi_conexion.Conectar Then
            Return
        End If

        Dim params As New List(Of SqlParameter)
        params.Add(New SqlParameter("@cUsu", tb_usuario.Text))
        params.Add(New SqlParameter("@cPass", tb_password.Password))
        params.Add(New SqlParameter("@cApp", strApp))
        Dim Logearse As String = ""
        Using xreader As SqlDataReader = Mi_conexion.Ejecutar_Procedimiento_dataReader("[dbo].[Sys_Login]", params)
            If xreader.Read Then
                Logearse = xreader("Usuario")
            End If
        End Using

        If (Logearse <> "") Then

            If ValidarDatos Then

                '********* VALIDAR CREDENCIALES DE ADMINISTRADOR *********
                If IIf(Split(Logearse.ToString, "#")(3).ToString = "1", True, False) Then

                    DatosCredenciales = New Credenciales
                    DatosCredenciales.Opererador = Integer.Parse(Split(Logearse.ToString, "#")(0).ToString)
                    DatosCredenciales.NombreUsuario = Split(Logearse.ToString, "#")(1).ToString
                    DatosCredenciales.Opciones = Split(Logearse.ToString, "#")(2).ToString
                    DatosCredenciales.Admin = IIf(Split(Logearse.ToString, "#")(3).ToString = "1", True, False)
                    Me.DialogResult = True

                Else
                    MessageBox.Show("Se necesitan credenciales de administrador", "", MessageBoxButton.OK, MessageBoxImage.Information)
                End If

            Else
                '********* LOGEARSE NORMAL *********
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

            End If

        Else
            MessageBox.Show("Usuario y/o contraseña no validos", "", MessageBoxButton.OK, MessageBoxImage.Stop)
        End If

    End Sub
    Private Sub Btn_salir_onclic() Handles btn_salir.MouseLeftButtonUp
        If ValidarDatos Then
            Me.DialogResult = False
        Else
            Me.Close()
        End If
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
Public Class Credenciales
    Property Opererador As Integer = 0
    Property NombreUsuario As String = ""
    Property Opciones As String = ""
    Property Admin As Boolean = False
End Class