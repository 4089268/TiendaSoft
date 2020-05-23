Imports System.Data
Imports System.Data.SqlClient

Class Page_inv_reporteInv

    Dim tablaDatos As DataTable
    Dim navService As NavigationService
    Dim xpageInventar As Page_Inventario
    Private fecha1 As DateTime

#Region "*-**********************Constructores"
    Public Sub New()
        InitializeComponent()
        fecha1 = DateTime.Now.Subtract(New TimeSpan(7, 0, 0, 0))
    End Sub
    Public Sub New(father As Page_Inventario)
        InitializeComponent()
        xpageInventar = father
        fecha1 = DateTime.Now.Subtract(New TimeSpan(7, 0, 0, 0))
    End Sub
#End Region

    Private Sub rootLayout_loaded() Handles rootLayout.Loaded
        navService = Me.NavigationService
        cargarDatos()
    End Sub
    Private Sub cargarDatos()
        busyI.IsBusy = True
        Try
            If (Mi_conexion.Conectar) Then

                Dim datosReporte As New DataSet
                Dim params = New List(Of SqlParameter)
                params.Add(New SqlParameter("@xAlias", "REPORTE"))
                params.Add(New SqlParameter("@fecha1", fecha1.ToString("yyyyMMdd")))
                Mi_conexion.Ejecutar_Procedimiento_dataAdapter("[Global].[sys_Inventario]", params).Fill(datosReporte)

                Dim xpTotal As String = datosReporte.Tables(0).Rows(0).Item("productos").ToString
                Dim xpInventario As String = datosReporte.Tables(3).Rows(0).Item("productos").ToString
                lbl_productTotal.Content = xpInventario & " de " & xpTotal & " productos Registrados"
                lbl_costoTotal.Content = "$" & datosReporte.Tables(1).Rows(0).Item("costo_inventario").ToString

                myDatagrid.ItemsSource = datosReporte.Tables(2).DefaultView

            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("ERROR AL CARGAR LOS DATOS", "", MessageBoxButton.OK, MessageBoxImage.Error)
        Finally
            busyI.IsBusy = False
        End Try
    End Sub


    '********* EVENTOS UI '*********
    Private Sub DataGrid_SelectionChanged() Handles myDatagrid.SelectionChanged, myDatagrid.LostFocus
        Dim val As Boolean = myDatagrid.SelectedIndex >= 0
        btn_editar.IsEnabled = val
        btn_agregar.IsEnabled = val
        btn_ajuste.IsEnabled = val
        btn_movim.IsEnabled = val
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
    Private Sub DiasPicker_Update() Handles dp_dias.ValueChanged
        fecha1 = DateTime.Now.Subtract(New TimeSpan(CType(dp_dias.Value, Integer), 0, 0, 0))
        If Not IsNothing(busyI) Then
            cargarDatos()
        End If
    End Sub
    Private Sub BotonAccion_Click(sender As Object, e As RoutedEventArgs) Handles btn_movim.MouseLeftButtonUp, btn_ajuste.MouseLeftButtonUp, btn_editar.MouseLeftButtonUp, btn_agregar.MouseLeftButtonUp
        Try

            Select Case CType(sender, Border).Name
                Case "btn_editar"
                    If (CBool(xOpciones.Substring(1, 1))) Then
                        'xpageInventar.editarProdcuto(myDatagrid.SelectedItem.Row.item("descripcion").ToString)
                        xpageInventar.editarProdcuto(myDatagrid.SelectedItem.Row.item("codigo").ToString)
                    Else
                        MessageBox.Show("No tienes permiso para editar productos", "", MessageBoxButton.OK, MessageBoxImage.Information)
                    End If
                Case "btn_agregar"
                    navService.Navigate(New Page_inv_agregar(myDatagrid.SelectedItem.Row.item("codigo").ToString))
                Case "btn_ajuste"
                    navService.Navigate(New Page_inv_remplazar(myDatagrid.SelectedItem.Row.item("codigo").ToString))
                Case "btn_movim"
                    navService.Navigate(New Page_inv_movimientos(myDatagrid.SelectedItem.Row.item("codigo").ToString))
            End Select

        Catch ex As Exception
            MessageBox.Show("Seleccione un producto", "", MessageBoxButton.OK, MessageBoxImage.Warning)
        End Try

    End Sub

End Class
