Imports System.Threading
Imports System.Globalization
Imports System.Data.SqlClient
Imports System.Data

Public Class Frm_SalidaDinero
    Dim xtipo As Int16 = 0
    Public Sub New(tipo As Int16)
        InitializeComponent()
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
        Me.xtipo = tipo

        If (tipo = 1) Then
            Me.Title = "Entrada Dinero"
            lbl_title.Content = "Entrada Dinero"
            tb_descripcion.Text = "Entrada Dinero"
        End If

        If (tipo = 2) Then
            Me.Title = "Salida Dinero"
            lbl_title.Content = "Salida Dinero"
            tb_descripcion.Text = "Salida Dinero"
        End If

    End Sub

    Private Sub btn_cancelarClick() Handles btn_cancelar.Click
        Me.Close()
    End Sub

    Private Sub btn_aceptarClick() Handles btn_aceptar.Click
        If (xtipo <> 0) Then
            Try

                Dim Cmd_Login = New SqlCommand
                Cmd_Login.CommandTimeout = 500
                Cmd_Login.CommandType = CommandType.StoredProcedure
                Cmd_Login.CommandText = "[Global].[ES_Dinero]"
                Cmd_Login.Parameters.Clear()

                If (xtipo = 1) Then
                    Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "ENTRADAS"))
                ElseIf (xtipo = 2) Then
                    Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "SALIDA"))
                End If

                Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@idOperador", xOpererador))
                Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xMonto", CDec(tb_monto.Text)))
                Cmd_Login.Parameters.Add(New SqlClient.SqlParameter("@xDescripcion", tb_descripcion.Text))

                Cmd_Login.Connection = Mi_conexion.conexion
                Dim reader As SqlDataReader = Cmd_Login.ExecuteReader()

                Dim mensaje As String = ""
                Try
                    While reader.Read
                        mensaje = reader("mensaje")
                    End While

                    MessageBox.Show(mensaje, "", MessageBoxButton.OK, MessageBoxImage.Information)

                Catch ex As Exception

                Finally
                    reader.Close()
                End Try

            Catch ex As Exception
                MessageBox.Show("ALGO SALIO MAL :(", "ERR", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try

        Else
            MessageBox.Show("Parametro Incorrecto, Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
        Me.Close()
    End Sub

    Private Sub validar_numeros(sender As Object, e As TextCompositionEventArgs) Handles tb_monto.PreviewTextInput
        Dim regex As New System.Text.RegularExpressions.Regex("([^0-9]+).([0-9]*)")
        e.Handled = regex.IsMatch(e.Text)
    End Sub

End Class
