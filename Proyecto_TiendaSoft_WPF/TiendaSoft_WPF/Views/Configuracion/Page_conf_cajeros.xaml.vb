Imports System.Data.SqlClient
Imports System.Data

Class Page_conf_cajeros

    Dim navService As NavigationService
    Dim tablaUsuarios As DataTable

    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        navService = Me.NavigationService
        cargarUi()
        cargarDatos()
        DataGrid1.SelectedIndex = 0

    End Sub

    Sub buttons_click(sender As Object, e As RoutedEventArgs) Handles btn_regresar.Click, btn_Guardar.Click, btn_Cancelar.Click, btn_nuevo.Click
        Select Case sender.name
            Case "btn_regresar"
                navService.Source = New Uri("Views/Page_Configuracion.xaml", UriKind.Relative)
            Case "btn_Cancelar"
                LimpiarDatos()
                cargarUsuario()
                ActivarCampos(False)
            Case "btn_Guardar"
                GuardarModificacionUsuario()
                ActivarCampos(False)
            Case "btn_nuevo"
                LimpiarDatos()
                ActivarCampos(True)
        End Select

    End Sub

    Private Sub cargarDatos()
        If (Mi_conexion.Conectar) Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[PDV].[Global].[Sys_ConfUsuarios]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@Alias", "CARGAR_USUARIOS"))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cApp", strApp))

            SqlComand.Connection = Mi_conexion.conexion
            Dim DataAdapter As New SqlDataAdapter(SqlComand)
            Dim dataSet As New DataSet

            DataAdapter.Fill(dataSet, "Resultado")

            tablaUsuarios = dataSet.Tables(0)

            DataGrid1.ItemsSource = tablaUsuarios.DefaultView

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub

    Private Sub cargarUi()

        DataGrid1.Columns.Clear()
        DataGrid1.IsReadOnly = True
        DataGrid1.AutoGenerateColumns = False
        DataGrid1.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed


        Dim TextBoxColumn1 As New DataGridTextColumn
        TextBoxColumn1.Header = "Nombre"
        TextBoxColumn1.Binding = New Binding("nombre")
        TextBoxColumn1.Width = 160

        DataGrid1.Columns.Insert(0, Resources("img"))
        DataGrid1.Columns.Add(TextBoxColumn1)

        ActivarCampos(False)

    End Sub

    Private Sub DataGrid_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles DataGrid1.SelectionChanged
        cargarUsuario()
    End Sub

    Private Sub cargarUsuario()
        Try
            Dim dataRow As DataRow = DataGrid1.SelectedItem.Row

            tb_lastModif.Text = dataRow.Item("LastPswChanged")
            tb_nombre.Text = dataRow.Item("nombre")
            tb_usuario.Text = dataRow.Item("usuario")
            tb_password.Password = dataRow.Item("password")
            cb_activo.IsChecked = CBool(dataRow.Item("activo"))

            Dim opciones As String = dataRow.Item("opciones")

            If (opciones.Length < 6 And opciones.Length > 0) Then
                opciones = "000000"
            End If

            If (opciones.Length < 6 And opciones.Length > 0) Then
                For i = opciones.Length - 1 To 5
                    opciones = opciones & "0"
                Next
            End If

            cb_ventas.IsChecked = CBool(opciones.Substring(0, 1))
            cb_productos.IsChecked = CBool(opciones.Substring(1, 1))
            cb_inventario.IsChecked = CBool(opciones.Substring(2, 1))
            cb_factura.IsChecked = CBool(opciones.Substring(3, 1))
            cb_corte.IsChecked = CBool(opciones.Substring(4, 1))
            cb_reporte.IsChecked = CBool(opciones.Substring(5, 1))
        Catch ex As Exception
        End Try

    End Sub

    Private Sub GuardarModificacionUsuario()

        If (tb_nombre.Text <> "" And tb_usuario.Text <> "" And tb_password.Password <> "") Then
            Dim opciones As String = ""
            opciones = opciones & IIf(cb_ventas.IsChecked, "1", "0")
            opciones = opciones & IIf(cb_productos.IsChecked, "1", "0")
            opciones = opciones & IIf(cb_inventario.IsChecked, "1", "0")
            opciones = opciones & IIf(cb_factura.IsChecked, "1", "0")
            opciones = opciones & IIf(cb_corte.IsChecked, "1", "0")
            opciones = opciones & IIf(cb_reporte.IsChecked, "1", "0")

            If (Mi_conexion.Conectar) Then
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[PDV].[Global].[Sys_ConfUsuarios]"
                SqlComand.Parameters.Clear()

                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cApp", strApp))

                If (DataGrid1.SelectedIndex = -1) Then
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@Alias", "NUEVO"))
                Else
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@Alias", "MODIFICAR"))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@idUsuario", DataGrid1.SelectedItem.Row.item("id_operador")))
                End If

                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@nombre", tb_nombre.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@usuario", tb_usuario.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@password", tb_password.Password))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@opcinones", opciones))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@activo", cb_activo.IsChecked))

                SqlComand.Connection = Mi_conexion.conexion
                Dim reader As SqlDataReader = SqlComand.ExecuteReader()

                Try
                    reader.Read()
                    MessageBox.Show(reader("mensaje"), "", MessageBoxButton.OK, MessageBoxImage.Information)
                Catch ex As Exception
                End Try
                cargarDatos()
                LimpiarDatos()
            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Else
            MessageBox.Show("Verifique que los campos no esten vacios", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If


    End Sub

    Private Sub LimpiarDatos()
        DataGrid1.SelectedItem = Nothing
        cargarDatos()

        tb_lastModif.Text = ""
        tb_nombre.Text = ""
        tb_usuario.Text = ""
        tb_password.Password = ""
        cb_activo.IsChecked = False
        cb_corte.IsChecked = False
        cb_activo.IsChecked = False
        cb_factura.IsChecked = False
        cb_inventario.IsChecked = False
        cb_productos.IsChecked = False
        cb_reporte.IsChecked = False
        cb_ventas.IsChecked = False

    End Sub

    Private Sub tb_search_search(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        If (e.Key = 6) Then
            If (tb_search.SearchText <> "") Then
                Dim dataview As DataView = tablaUsuarios.DefaultView
                dataview.RowFilter = String.Format("nombre like '%" & tb_search.SearchText & "%'")
                DataGrid1.ItemsSource = dataview
            Else
                cargarDatos()
            End If
        End If
    End Sub

    Private Sub ActivarCampos(val As Boolean)
        If val Then
            form.Background = Brushes.White
        Else
            form.Background = Brushes.WhiteSmoke
        End If

        DataGrid1.IsEnabled = Not val
        btn_nuevo.IsEnabled = Not val

        btn_Guardar.IsEnabled = val
        btn_Cancelar.IsEnabled = val

        tb_nombre.IsEnabled = val
        tb_usuario.IsEnabled = val
        tb_password.IsEnabled = val
        cb_activo.IsEnabled = val
        cb_corte.IsEnabled = val
        cb_activo.IsEnabled = val
        cb_factura.IsEnabled = val
        cb_inventario.IsEnabled = val
        cb_productos.IsEnabled = val
        cb_reporte.IsEnabled = val
        cb_ventas.IsEnabled = val

    End Sub

End Class
