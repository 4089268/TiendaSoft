Imports System.Data.SqlClient
Imports System.Data

Public Class Frm_Conexion
    Dim conectada As Boolean = False

    Public Sub New()
        InitializeComponent()
        Me.txt_svr.Text = My.Settings.Server
        Me.txt_usu.Text = My.Settings.user
    End Sub

    Private Sub btn_reconectar_Click(sender As Object, e As RoutedEventArgs) Handles btn_reconectar.Click
        rectangleStatus.Fill = Brushes.Yellow
        lbl_status.Content = "PROBANDO CONEXION .....!"

        My.Settings.Server = Me.txt_svr.Text
        My.Settings.user = Me.txt_usu.Text
        My.Settings.password = Me.txt_pass.Text
        My.Settings.Save()

        Try
            Dim xconexion As New SqlConnection
            Dim _conectionString = "Server=" & My.Settings.Server & ";Database=" & strBasedeDatos & ";User Id=" & My.Settings.user & "; Password=" & My.Settings.password & ";"
            xconexion = New SqlClient.SqlConnection(_conectionString)
            xconexion.Open()
            xconexion.Close()

            conectada = True
            rectangleStatus.Fill = Brushes.Green
            lbl_status.Content = "!CONECTADO CON EL SERVIDOR¡"

        Catch ex As Exception
            conectada = False
            rectangleStatus.Fill = Brushes.Red
            lbl_status.Content = "¡NO CONECTADO CON EL SERVIDOR!"
        End Try

    End Sub

    Private Sub btn_salir_Click(sender As Object, e As RoutedEventArgs) Handles btn_salir.Click
        Me.Close()
    End Sub

    Private Sub saliedno() Handles Me.Closing
        If (conectada) Then
            Me.DialogResult = True
        Else
            Me.DialogResult = False
        End If
    End Sub

End Class
