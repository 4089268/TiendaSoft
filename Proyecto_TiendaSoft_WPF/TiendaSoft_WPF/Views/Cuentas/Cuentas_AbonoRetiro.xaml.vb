Imports System.Data.SqlClient
Imports System.Data

Public Class Cuentas_AbonoRetiro
    Property tipo As String = ""
    Property id_Cuenta As String = ""

    Public Sub New(xtipo As String, id As Integer)
        InitializeComponent()
        Me.tipo = xtipo
        Me.id_Cuenta = id
    End Sub

    Private Sub me_loadedDone() Handles Me.Loaded
        Me.Title = tipo
        Me.btn_accion.Content = tipo

        Try
            If (Mi_conexion.Conectar) Then
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[Sys_PCuentas]"
                SqlComand.Parameters.Clear()
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "BUSQUEDAID"))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xid_cuenta", id_Cuenta))

                SqlComand.Connection = Mi_conexion.conexion
                Dim DataAdapter As New SqlDataAdapter(SqlComand)
                Dim dataSet As New DataSet
                DataAdapter.Fill(dataSet, "Resultado")

                Me.tb_nombre.Text = dataSet.Tables(0).Rows(0).Item("nombre")
                Me.tb_saldo.Text = dataSet.Tables(0).Rows(0).Item("saldoActual")

            End If
        Catch ex As Exception
            MessageBox.Show("Algo Salio Mal :(", "", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try

    End Sub

    Private Sub validar_numbers(sender As Object, e As TextCompositionEventArgs) Handles tb_cantidad.PreviewTextInput
        Try
            Dim regex As New System.Text.RegularExpressions.Regex("^(\d+)(.{0,1})(\d{0,2})$")
            e.Handled = Not (regex.IsMatch(e.Text))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btn_acccion() Handles btn_accion.Click

        If tb_cantidad.Text.Length > 0 Then
            Try
                If (Mi_conexion.Conectar) Then
                    Dim SqlComand = New SqlCommand
                    SqlComand.CommandTimeout = 500
                    SqlComand.CommandType = CommandType.StoredProcedure
                    SqlComand.CommandText = "[Global].[Sys_PCuentas]"
                    SqlComand.Parameters.Clear()
                    If tipo = "ABONAR" Then
                        SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "ABONO"))
                    Else
                        SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "RETIRO"))
                    End If

                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xid_cuenta", id_Cuenta))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xSaldo", CType(tb_cantidad.Text, Integer)))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xComentario", tb_coment.Text))

                    SqlComand.Connection = Mi_conexion.conexion
                    Dim DataAdapter As New SqlDataAdapter(SqlComand)
                    Dim dataSet As New DataSet
                    DataAdapter.Fill(dataSet, "Resultado")

                    Me.DialogResult = True

                End If
            Catch ex As Exception
                MessageBox.Show("Algo Salio Mal :(", "", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        Else
        End If

    End Sub

End Class
