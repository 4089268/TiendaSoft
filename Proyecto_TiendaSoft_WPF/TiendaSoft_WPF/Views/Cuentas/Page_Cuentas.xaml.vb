Imports System.Threading
Imports System.Globalization
Imports System.Data.SqlClient
Imports System.Data

Class Page_Cuentas
    Private Sub me_loaded_Done() Handles Me.Loaded
        Thread.CurrentThread.CurrentCulture = New CultureInfo("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")

        cargarUI()
        cargarDatos()
    End Sub

    Private Sub cargarUI()

        btn_guardar.Visibility = Windows.Visibility.Collapsed
        btn_cancelar.Visibility = Windows.Visibility.Collapsed
        modal.Visibility = Windows.Visibility.Collapsed

        Dim rightCellStyle = New Style(GetType(DataGridCell))
        rightCellStyle.Setters.Add(New Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Right))

        Dim anchoCol = DataGrid_Cuentas.ActualWidth

        '' *** GRID CUENTAS ***
        DataGrid_Cuentas.Columns.Clear()
        DataGrid_Cuentas.IsReadOnly = True
        DataGrid_Cuentas.AutoGenerateColumns = False
        DataGrid_Cuentas.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed

        DataGrid_Cuentas.Columns.Insert(0, Resources("img"))
        DataGrid_Cuentas.Columns(0).Width = anchoCol - 8

        Dim TextBoxColumn2 As New DataGridTextColumn
        TextBoxColumn2.Header = "x2"
        TextBoxColumn2.Binding = New Binding("telefono")
        TextBoxColumn2.Visibility = Windows.Visibility.Collapsed
        DataGrid_Cuentas.Columns.Add(TextBoxColumn2)

        Dim TextBoxColumn3 As New DataGridTextColumn
        TextBoxColumn3.Header = "x3"
        TextBoxColumn3.Binding = New Binding("direccion")
        TextBoxColumn3.Visibility = Windows.Visibility.Collapsed
        DataGrid_Cuentas.Columns.Add(TextBoxColumn3)

        Dim TextBoxColumn4 As New DataGridTextColumn
        TextBoxColumn4.Header = "x4"
        TextBoxColumn4.Binding = New Binding("saldo")
        TextBoxColumn4.Visibility = Windows.Visibility.Collapsed
        DataGrid_Cuentas.Columns.Add(TextBoxColumn4)


        '' *** GRID MOVIMIENTOS ***
        dg_movimientos.Columns.Clear()
        dg_movimientos.IsReadOnly = True
        dg_movimientos.AutoGenerateColumns = False
        dg_movimientos.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed

        Dim tb1 As New DataGridTextColumn
        tb1.Header = "MOVIMIENTO"
        tb1.Width = 140
        tb1.Binding = New Binding("descripcion")
        dg_movimientos.Columns.Add(tb1)

        Dim tb2 As New DataGridTextColumn
        tb2.Header = "SALDO ANTERIOR"
        tb2.Width = 180
        tb2.Binding = New Binding("saldoAnterior")
        tb2.CellStyle = rightCellStyle
        dg_movimientos.Columns.Add(tb2)

        Dim tb3 As New DataGridTextColumn
        tb3.Header = "SALDO NUEVO"
        tb3.Width = 180
        tb3.Binding = New Binding("saldoNuevo")
        tb3.CellStyle = rightCellStyle
        dg_movimientos.Columns.Add(tb3)

        Dim tb4 As New DataGridTextColumn
        tb4.Header = "DIFERENCIA"
        tb4.Width = 180
        tb4.Binding = New Binding("importe")
        tb4.CellStyle = rightCellStyle
        dg_movimientos.Columns.Add(tb4)

        Dim tb5 As New DataGridTextColumn
        tb5.Header = "FECHA"
        tb5.Width = 150
        tb5.Binding = New Binding("fecha")
        dg_movimientos.Columns.Add(tb5)

        Dim tb6 As New DataGridTextColumn
        tb6.Header = "COMENTARIO"
        tb6.Width = 200
        tb6.Binding = New Binding("comentario")
        dg_movimientos.Columns.Add(tb6)

    End Sub

    Private Sub limpiar_UI()
        tb_dirreccion.Text = ""
        tb_nombre.Text = ""
        tb_telefon.Text = ""
        tb_saldo.Text = ""
        dg_movimientos.ItemsSource = Nothing

    End Sub

    Private Sub cargarDatos()
        Try
            If (Mi_conexion.Conectar) Then
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[Sys_PCuentas]"
                SqlComand.Parameters.Clear()
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "CARGARCUENTAS"))

                SqlComand.Connection = Mi_conexion.conexion
                Dim DataAdapter As New SqlDataAdapter(SqlComand)
                Dim dataSet As New DataSet

                DataAdapter.Fill(dataSet, "Resultado")
                DataGrid_Cuentas.ItemsSource = dataSet.Tables(0).DefaultView
            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Algo Salio Mal :(", "", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try

    End Sub

    Private Sub DatGridCuentas_rowSelected() Handles DataGrid_Cuentas.SelectionChanged
        Try
            limpiar_UI()
            Dim x As DataRowView = DataGrid_Cuentas.SelectedItem
            tb_nombre.Text = x.Item("nombre")
            tb_dirreccion.Text = x.Item("direccion")
            tb_telefon.Text = x.Item("telefono")
            tb_saldo.Text = "$" & x.Item("saldo")


            ''cargar movimientos de la cuenta
            Try
                If (Mi_conexion.Conectar) Then
                    Dim SqlComand = New SqlCommand
                    SqlComand.CommandTimeout = 500
                    SqlComand.CommandType = CommandType.StoredProcedure
                    SqlComand.CommandText = "[Global].[Sys_PCuentas]"
                    SqlComand.Parameters.Clear()
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "CARGARMOV"))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xid_cuenta", x.Item("id")))

                    SqlComand.Connection = Mi_conexion.conexion
                    Dim DataAdapter As New SqlDataAdapter(SqlComand)
                    Dim dataSet As New DataSet

                    DataAdapter.Fill(dataSet, "Resultado")

                    If dataSet.Tables.Count > 0 Then
                        dg_movimientos.ItemsSource = dataSet.Tables(0).DefaultView
                    End If

                Else
                    MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                End If
            Catch ex As Exception
                MessageBox.Show("Algo Salio Mal :(", "", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try


        Catch ex As Exception
        End Try
    End Sub

    Private Sub boton_nuevo_click() Handles btn_nuevo.Click
        modal.Visibility = Windows.Visibility.Visible
        Dim xf As New Nueva_cuenta
        xf.ShowDialog()

        modal.Visibility = Windows.Visibility.Collapsed
        If xf.DialogResult Then
            cargarDatos()
        End If
    End Sub

    '**** EVENTOS BOTONES ***
    Private Sub botones_abonar_retiro_Click(sender As Object, e As RoutedEventArgs) Handles btn_abonar.Click, btn_retirar.Click
        Select Case sender.name
            Case "btn_abonar"
                Dim xf As New Cuentas_AbonoRetiro("ABONAR", 1)
                xf.ShowDialog()
                If xf.DialogResult = True Then
                    limpiar_UI()
                    cargarDatos()
                End If
            Case "btn_retirar"
                Dim xf As New Cuentas_AbonoRetiro("RETIRAR", 1)
                xf.ShowDialog()
                If xf.DialogResult = True Then
                    limpiar_UI()
                    cargarDatos()
                End If
        End Select
        DatGridCuentas_rowSelected()
    End Sub

    Private Sub tb_search_keyDown(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        If (e.Key = Key.Enter) Then
            If (tb_search.SearchText <> "") Then
                Dim dataview As DataView = DataGrid_Cuentas.ItemsSource
                dataview.RowFilter = String.Format("nombre like '%" & tb_search.SearchText & "%'")
                DataGrid_Cuentas.ItemsSource = dataview
            Else
                cargarDatos()
            End If
        End If
    End Sub

End Class
