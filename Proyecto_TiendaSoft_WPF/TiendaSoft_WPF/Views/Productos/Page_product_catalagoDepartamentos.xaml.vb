Imports System.Data.SqlClient
Imports System.Data

Class Page_product_catalagoDepartamentos
    Private Sub root_layout() Handles rootLayout.Loaded
        cargarUI()
        cargarDatos()
        activarCampos(False)
        Try
            DataGrid1.SelectedIndex = 0
        Catch ex As Exception
        End Try
    End Sub

    Private Sub cargarUI()
        DataGrid1.Columns.Clear()
        DataGrid1.IsReadOnly = True
        DataGrid1.AutoGenerateColumns = False
        DataGrid1.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed

        DataGrid1.Columns.Insert(0, Resources("descripcion"))
        DataGrid1.Columns(0).Width = DataGrid1.ActualWidth - 8
    End Sub

    Private Sub cargarDatos()
        If (Mi_conexion.Conectar) Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[Global].[Sys_Departamentos]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "CARGARCATALAGO"))

            SqlComand.Connection = Mi_conexion.conexion
            Dim DataAdapter As New SqlDataAdapter(SqlComand)
            Dim dataSet As New DataSet

            DataAdapter.Fill(dataSet, "Resultado")
            DataGrid1.ItemsSource = dataSet.Tables(0).DefaultView

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub

    Private Sub DataGrid1_selection(sender As Object, e As RoutedEventArgs) Handles DataGrid1.SelectionChanged
        Try
            Dim dataRow As DataRow = DataGrid1.SelectedItem.Row
            tb_nombre.Text = dataRow.Item("descripcion")
            cb_activo.IsChecked = CType(dataRow.Item("activo"), Boolean)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Buttons_Click(sender As Object, e As Windows.RoutedEventArgs) Handles btn_nuevo.Click, btn_modif.Click, btn_Cancelar.Click, btn_Guardar.Click
        Select Case sender.name
            Case "btn_nuevo"
                limpiarCampos()
                activarCampos(True)
            Case "btn_modif"
                activarCampos(True)
            Case "btn_Cancelar"
                cargarDatos()
                activarCampos(False)
            Case "btn_Guardar"
                activarCampos(False)
                guardarCambios()
        End Select

    End Sub

    Private Sub limpiarCampos()
        DataGrid1.SelectedItem = Nothing
        cargarDatos()

        tb_nombre.Text = ""
        cb_activo.IsChecked = False
    End Sub

    Private Sub activarCampos(p1 As Boolean)
        form.Background = IIf(p1, Brushes.White, Brushes.WhiteSmoke)

        tb_nombre.IsEnabled = p1
        cb_activo.IsEnabled = p1
        btn_Guardar.IsEnabled = p1
        btn_Cancelar.IsEnabled = p1

        DataGrid1.IsEnabled = Not p1

        btn_nuevo.IsEnabled = Not p1
        btn_modif.IsEnabled = Not p1

        If (p1) Then
            btn_Guardar.Visibility = Windows.Visibility.Visible
            btn_Cancelar.Visibility = Windows.Visibility.Visible

            btn_nuevo.Visibility = Windows.Visibility.Collapsed
            btn_modif.Visibility = Windows.Visibility.Collapsed
        Else
            btn_Guardar.Visibility = Windows.Visibility.Collapsed
            btn_Cancelar.Visibility = Windows.Visibility.Collapsed

            btn_nuevo.Visibility = Windows.Visibility.Visible
            btn_modif.Visibility = Windows.Visibility.Visible
        End If


    End Sub

    Private Sub guardarCambios()
        If (tb_nombre.Text <> "") Then
            If (Mi_conexion.Conectar) Then

                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[Sys_Departamentos]"
                SqlComand.Parameters.Clear()

                If (DataGrid1.SelectedIndex = -1) Then
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "NUEVO"))
                Else
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "MODIFICAR"))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cId", DataGrid1.SelectedItem.Row.item("id_familia")))
                End If

                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cDescripcion", tb_nombre.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cActivo", cb_activo.IsChecked))

                SqlComand.Connection = Mi_conexion.conexion
                Dim reader As SqlDataReader = SqlComand.ExecuteReader()

                Try
                    reader.Read()
                    MessageBox.Show(reader("Mensaje"), "", MessageBoxButton.OK, MessageBoxImage.Information)
                Catch ex As Exception
                End Try

                limpiarCampos()
                cargarDatos()

            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Else
            MessageBox.Show("Verifique que los campos no esten vacios", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub

End Class

