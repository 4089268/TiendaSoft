Imports System.Data
Imports System.Data.SqlClient

Class Page_inv_reporteInv

    Dim tablaDatos As DataTable
    Dim navService As NavigationService
    Dim xpageInventar As Page_Inventario

    Private Sub rootLayout_loaded() Handles rootLayout.Loaded
        navService = Me.NavigationService
        cargarUI()
        cargarDatos()

    End Sub
    Private Sub cargarUI()
        myDatagrid.IsReadOnly = True
        myDatagrid.AutoGenerateColumns = False
        myDatagrid.Columns.Clear()
        myDatagrid.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed

        myDatagrid.Columns.Add(crear_datagridColumn("codigo", 160, "CODIGO", False))
        myDatagrid.Columns.Add(crear_datagridColumn("descripcion", 450, "DESCRIPCION", False))
        myDatagrid.Columns.Add(crear_datagridColumn("precio_c", 120, "PRECIO COMPRA", True))
        myDatagrid.Columns.Add(crear_datagridColumn("precio_v", 120, "PRECIO VENTA", True))
        myDatagrid.Columns.Add(crear_datagridColumn("existencia", 120, "EXISTENCIA", False))
        myDatagrid.Columns.Add(crear_datagridColumn("minimo", 120, "INV. MINIMO", False))

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

    Private Sub cargarDatos()
        Try
            If (Mi_conexion.Conectar) Then
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[sys_Inventario]"
                SqlComand.Parameters.Clear()
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "REPORTE"))

                SqlComand.Connection = Mi_conexion.conexion
                Dim DataAdapter As New SqlDataAdapter(SqlComand)
                Dim dataSet As New DataSet

                DataAdapter.Fill(dataSet, "Resultado")

                Dim xpTotal As String = dataSet.Tables(0).Rows(0).Item("productos").ToString
                Dim xpInventario As String = dataSet.Tables(3).Rows(0).Item("productos").ToString
                lbl_productTotal.Content = xpInventario & " de " & xpTotal & " productos Registrados"
                lbl_costoTotal.Content = "$" & dataSet.Tables(1).Rows(0).Item("costo_inventario").ToString

                tablaDatos = dataSet.Tables(2)
                myDatagrid.ItemsSource = tablaDatos.DefaultView

            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("ERROR AL CARGAR LOS DATOS", "", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
        
    End Sub
    Private Sub tb_search_search(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        If (e.Key = Key.Enter) Then
            If (tb_search.SearchText <> "") Then
                Dim dataview As DataView = myDatagrid.ItemsSource
                dataview.RowFilter = String.Format("descripcion like '%" & tb_search.SearchText & "%'")
                myDatagrid.ItemsSource = dataview
            Else
                cargarDatos()
            End If
        End If
    End Sub

    Private Sub btn_agregar_click() Handles btn_agregar.Click
        Try
            navService.Navigate(New Page_inv_agregar(myDatagrid.SelectedItem.Row.item("codigo").ToString))
        Catch ex As Exception
            MessageBox.Show("Seleccione un producto", "", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub

    Private Sub btn_editar_click() Handles btn_editar.click
        Try
            If (CBool(xOpciones.Substring(1, 1))) Then
                xpageInventar.editarProdcuto(myDatagrid.SelectedItem.Row.item("descripcion").ToString)
            Else
                MessageBox.Show("No tienes permiso para editar productos", "", MessageBoxButton.OK, MessageBoxImage.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Seleccione un producto", "", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try
    End Sub


#Region "*-**********************Constructores"
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(father As Page_Inventario)
        InitializeComponent()
        xpageInventar = father

    End Sub
#End Region

End Class
