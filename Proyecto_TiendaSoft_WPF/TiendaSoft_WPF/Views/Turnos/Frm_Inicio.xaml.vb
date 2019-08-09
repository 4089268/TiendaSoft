Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.Threading

Public Class Frm_Inicio
    Dim xdialogResult As Boolean = False
    Dim xadmin As Boolean = False

    Private Sub btn_iniciar() Handles btn_start.Click

        Thread.CurrentThread.CurrentCulture = New CultureInfo("es-MX")
        Try
            If (Mi_conexion.Conectar()) Then
                Dim reader As SqlDataReader
                If (tb_monto.Text.Length <= 0) Then
                    tb_monto.Text = "0"
                End If
                reader = Mi_conexion.Ejecutar_Procedimiento("[Global].[Sys_PSesiones]", {"xAlias", "idOperador", "xMonto"}, {"INICIAR", xOpererador, IIf(tb_monto.Text = "", 0, CDec(tb_monto.Text))})

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

        Catch ex As Exception
        End Try

        Me.Close()
    End Sub

    Private Sub Saliendo() Handles Me.Closing
        Me.DialogResult = xdialogResult
    End Sub

    Private Sub validar_numeros(sender As Object, e As TextCompositionEventArgs) Handles tb_monto.PreviewTextInput
        Dim regex As New System.Text.RegularExpressions.Regex("([^0-9]+).([0-9]*)")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

    Public Sub New(admin As Boolean)
        InitializeComponent()
        xadmin = admin
    End Sub

    Public Sub me_loadedDone() Handles Me.Loaded
        If xadmin Then
            Try
                If (Mi_conexion.Conectar()) Then
                    Dim reader As SqlDataReader
                    reader = Mi_conexion.Ejecutar_Procedimiento("[Global].[Sys_PSesiones]", {"xAlias", "idOperador", "xMonto"}, {"INICIAR", xOpererador, 0})

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

            Catch ex As Exception
            Finally
                Me.Close()
            End Try
        End If
    End Sub

End Class
