Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Class Page_inv_movimientos
    Private Sub rootLayout_loaded() Handles rootLayout.Loaded
        cargarUI()
        ''cargarDatos()
    End Sub

    Private Sub cargarUI()
        myDatagrid.IsReadOnly = True
        myDatagrid.AutoGenerateColumns = False
        myDatagrid.Columns.Clear()
        myDatagrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed

        myDatagrid.Columns.Add(crear_datagridColumn("nombre", 250, "NOOMBRE USUARIO", False))
        myDatagrid.Columns.Add(crear_datagridColumn("fecha", 200, "FECHA MOVIMIENTO", False))
        myDatagrid.Columns.Add(crear_datagridColumn("descripcion", 400, "PRODUCTO", False))
        myDatagrid.Columns.Add(crear_datagridColumn("movDescripcion", 130, "MOVIMIENTO", False))
        myDatagrid.Columns.Add(crear_datagridColumn("inv_anterior", 110, "INV. ANTERIOR", False))
        myDatagrid.Columns.Add(crear_datagridColumn("int_nuevo", 110, "INV. NUEVO", False))

        dp_fecha1.SelectedDate = Date.Now
        dp_fecha2.SelectedDate = Date.Now
    End Sub

    Private Function crear_datagridColumn(binding As String, ancho As Integer, cabecera As String, moneda As Boolean) As DataGridTextColumn

        Dim myStyle As New Style
        myStyle.Setters.Add(New Setter(Label.FontWeightProperty, FontWeights.Medium))

        Dim myColumn As New DataGridTextColumn
        myColumn.FontSize = 12
        Dim myBin As New Binding(binding)
        If (moneda) Then
            myBin.StringFormat = "{0:C}"
        End If
        myColumn.Binding = myBin
        myColumn.Header = cabecera
        myColumn.HeaderStyle = myStyle
        myColumn.Width = ancho



        Return myColumn
    End Function

    Private Sub cargarDatos() Handles btn_buscar.Click
        If (Mi_conexion.Conectar) Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[Global].[sys_Inventario]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "MOVIMIENTOS"))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@fecha1", dp_fecha1.SelectedDate.Value.Date.ToString("yyyy-MM-dd")))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@fecha2", dp_fecha2.SelectedDate.Value.Date.ToString("yyyy-MM-dd")))

            SqlComand.Connection = Mi_conexion.conexion
            Dim DataAdapter As New SqlDataAdapter(SqlComand)
            Dim dataSet As New DataSet

            DataAdapter.Fill(dataSet, "Resultado")

            myDatagrid.ItemsSource = dataSet.Tables(0).DefaultView

            lbl_totMovimientos.Content = "Movimientos: " & dataSet.Tables(0).Rows.Count

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub
    Private Sub tb_search_search(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        Try
            If (e.Key = Key.Enter) Then
                If (tb_search.SearchText <> "") Then
                    Dim dataview As DataView = myDatagrid.ItemsSource
                    dataview.RowFilter = String.Format("descripcion like '%" & tb_search.SearchText & "%'")
                    myDatagrid.ItemsSource = dataview
                Else
                    cargarDatos()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

End Class

