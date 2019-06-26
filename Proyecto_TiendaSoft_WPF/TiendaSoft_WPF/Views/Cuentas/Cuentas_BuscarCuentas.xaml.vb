Imports System.Data.SqlClient
Imports System.Data

Public Class Cuentas_BuscarCuentas
    Dim DataAdapter As SqlDataAdapter

    Dim fcobrar As Bloquera_TF.Frm_Cobrar

    Public Sub New(f As Bloquera_TF.Frm_Cobrar)
        InitializeComponent()
        Me.fcobrar = f
    End Sub

    Private Sub me_LoadedDone() Handles Me.Loaded
        CargarUI()
        Me.tb_Search.Focus()
    End Sub

    Private Sub CargarUI()
        Me.dataGrid.AutoGenerateColumns = False
        dataGrid.Columns.Clear()
        dataGrid.IsReadOnly = True

        Dim cs = New Style(GetType(DataGridCell))
        cs.Setters.Add(New Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Right))

        Dim c1 As New DataGridTextColumn
        c1.Header = "id"
        c1.Visibility = Windows.Visibility.Collapsed
        c1.Width = 40
        c1.Binding = New Binding("id")

        Dim c2 As New DataGridTextColumn
        c2.Header = "NOMBRE"
        c2.Width = 300
        c2.Binding = New Binding("nombre")

        Dim c3 As New DataGridTextColumn
        c3.Header = "SALDO"
        c3.Width = 120
        c3.Binding = New Binding("saldoActual")
        c3.CellStyle = cs

        dataGrid.Columns.Add(c1)
        dataGrid.Columns.Add(c2)
        dataGrid.Columns.Add(c3)

    End Sub

    Private Sub tb_search_KeyDown(sender As Object, e As KeyEventArgs) Handles tb_Search.KeyDown
        Dim x = tb_Search.Text

        If e.Key = Key.Enter And x.Length > 0 And Mi_conexion.Conectar Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[Global].[Sys_PCuentas]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "BUSQUEDARAPIDA"))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xNombre", x))

            SqlComand.Connection = Mi_conexion.conexion

            Dim dataSet As New DataSet
            DataAdapter = New SqlDataAdapter(SqlComand)
            DataAdapter.Fill(dataSet, "Resultado")

            Try
                Me.dataGrid.ItemsSource = dataSet.Tables(0).DefaultView
            Catch ex As Exception
            End Try
        Else
            Me.dataGrid.ItemsSource = Nothing
        End If

    End Sub

    Private Sub datGrid_dobleClick() Handles dataGrid.MouseDoubleClick
        If dataGrid.SelectedIndex >= 0 Then
            fcobrar.cuentaCredito = CType(dataGrid.SelectedItem("id"), Integer)
            Me.DialogResult = True

        End If
    End Sub
    
End Class
