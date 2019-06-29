
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Threading

Class Page_product_catalagoProducto
    Dim listaProductos As New List(Of itemProducto)
    Dim tmpListaProductos As New List(Of itemProducto)

    Private _p1 As String = ""
    Property xmlProducto As String = ""
    Dim us As New CultureInfo("es-MX")
#Region "****CONTRUCTORES***"
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(p1 As String)
        InitializeComponent()
        _p1 = p1
    End Sub

#End Region

    Sub Layout_onLoaded() Handles rootLayout.Loaded
        cargarUI()
        LimpiarCampos()
        'cargarDatos()
        Thread.CurrentThread.CurrentCulture = New CultureInfo("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")

        habilitarCampos(False)
        tb_existencia.IsEnabled = False
        tb_minimo.IsEnabled = False

        If (_p1.Length > 0) Then
            tb_search.SearchText = _p1
            Try
                tmpListaProductos.Clear()

                For Each xpr In listaProductos
                    If xpr.descripcion.Contains("tb_search.SearchText") Then
                        Dim xnewProd As itemProducto = xpr
                        tmpListaProductos.Add(xnewProd)
                    End If
                Next
                DataGrid1.ItemsSource = Nothing
                DataGrid1.ItemsSource = tmpListaProductos
            Catch ex As Exception
            End Try
        End If

    End Sub

    Private Sub cargarUI()
        DataGrid1.Columns.Clear()
        DataGrid1.IsReadOnly = True
        DataGrid1.AutoGenerateColumns = False
        DataGrid1.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed

        DataGrid1.Columns.Insert(0, Resources("img"))
        DataGrid1.Columns(0).Width = DataGrid1.ActualWidth - 12
    End Sub

    Private Sub cargarDatos()
        If (Mi_conexion.Conectar) Then

            '******* CARGAR PROD *******
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[Global].[Sys_Productos]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "CARGARCATALAGO"))

            SqlComand.Connection = Mi_conexion.conexion
            Dim DataAdapter As New SqlDataAdapter(SqlComand)
            Dim dataSet As New DataSet
            DataAdapter.Fill(dataSet, "Resultado")
            procesarDatos(dataSet.Tables(0))


            '******* CARGAR CATALAGOS *******
            Dim SqlComand2 = New SqlCommand
            SqlComand2.CommandTimeout = 500
            SqlComand2.CommandType = CommandType.StoredProcedure
            SqlComand2.CommandText = "[Global].[Sys_Departamentos]"
            SqlComand2.Parameters.Clear()
            SqlComand2.Parameters.Add(New SqlParameter("@cAlias", "CARGARCATALAGO"))

            SqlComand2.Connection = Mi_conexion.conexion
            Dim DataAdapter2 As New SqlDataAdapter(SqlComand2)
            Dim dataSet2 As New DataSet
            DataAdapter2.Fill(dataSet2, "Resultado")

            cb_departa.ItemsSource = dataSet2.Tables(0).DefaultView
            cb_departa.SelectedIndex = 0

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub

    Private Sub procesarDatos(datos As DataTable)
        listaProductos.Clear()
        If datos.Rows.Count > 0 Then
            For Each xrow In datos.Rows
                Dim xProdct As New itemProducto
                xProdct.id_producto = CType(xrow.item("id_producto"), Integer)
                xProdct.codigo = xrow.item("codigo")
                xProdct.descripcion = xrow.item("descripcion")
                xProdct.agranel = CType(xrow.item("agranel"), Boolean)
                xProdct.usaInventario = CType(xrow.item("usaInventario"), Boolean)
                xProdct.existencia = CType(xrow.item("existencia"), Double)
                xProdct.minimo = CType(xrow.item("minimo"), Double)
                xProdct.id_familia = CType(xrow.item("id_familia"), Integer)
                xProdct.tipoProducto = CType(xrow.item("tipoProducto"), Integer)
                xProdct.precio_c = CType(xrow.item("precio_c"), Double)
                xProdct.precio_v = CType(xrow.item("precio_v"), Double)
                listaProductos.Add(xProdct)
            Next
        End If
        DataGrid1.ItemsSource = listaProductos

    End Sub

    Private Sub habilitarCampos(val As Boolean)
        If val Then
            form.Background = Brushes.White
        Else
            form.Background = Brushes.WhiteSmoke
        End If

        DataGrid1.IsEnabled = Not val

        btn_nuevo.IsEnabled = Not val
        btn_modif.IsEnabled = Not val

        btn_Cancelar.IsEnabled = val
        btn_Guardar.IsEnabled = val
        btn_cargarImagen.IsEnabled = val

        tb_codigo.IsEnabled = val
        tb_descripcion.IsEnabled = val
        cb_granel.IsEnabled = val
        cb_unidad.IsEnabled = val
        tb_precioComp.IsEnabled = val

        tb_precioVent.IsEnabled = val
        tb_precioMayo.IsEnabled = val
        cb_departa.IsEnabled = val
        cb_invent.IsEnabled = val
        tipo1.IsEnabled = val
        tipo2.IsEnabled = val
        tipo3.IsEnabled = val
        btn_modificarpaquete.IsEnabled = val

        If (val) Then
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

        tb_codigo.Focus()

    End Sub

    Private Sub cargarProducto()
        If Not IsNothing(DataGrid1.SelectedItem) Then
            Dim xproducto As itemProducto = DataGrid1.SelectedItem
            tb_codigo.Text = xproducto.codigo
            tb_descripcion.Text = xproducto.descripcion
            tb_precioMayo.Text = 0
            cb_departa.SelectedValue = xproducto.id_familia
            tb_existencia.Text = xproducto.existencia
            tb_minimo.Text = xproducto.minimo
            cb_invent.IsChecked = xproducto.usaInventario
            cb_unidad.IsChecked = Not xproducto.agranel
            cb_granel.IsChecked = xproducto.agranel

            Select Case xproducto.tipoProducto
                Case -1 To 0
                    tipo1.IsChecked = True
                Case 1
                    tipo2.IsChecked = True
                Case 2
                    tipo3.IsChecked = True
                    cargarXml(xproducto.codigo)
            End Select

            'tb_precioVent.Text = IIf(dataRow.Item("precio_v").ToString = "", "0", CDec(dataRow.Item("precio_v")).ToString("####0.00"))
            tb_precioComp.Text = xproducto.precio_c.ToString("####0.00")
            tb_precioVent.Text = xproducto.precio_v.ToString("####0.00")
            cargar_imagen(xproducto.codigo)
        End If
    End Sub

    Private Sub cargarXml(codigo As String)
        If (Mi_conexion.Conectar) Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[Global].[Sys_Productos]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "OBTENERPAQUETE"))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@Codigo", codigo))

            SqlComand.Connection = Mi_conexion.conexion
            Using datareader As SqlDataReader = SqlComand.ExecuteReader
                If datareader.Read Then
                    xmlProducto = CType(datareader.Item(0), String)
                End If
            End Using
        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub

    Private Sub cargar_imagen(codigo As String)
        Try
            If (Mi_conexion.Conectar()) Then
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[sys_cargarImagen]"
                SqlComand.Parameters.Clear()
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@codigo", codigo))

                SqlComand.Connection = Mi_conexion.conexion

                Dim DataAdapter As New SqlDataAdapter(SqlComand)
                Dim dataSet As New DataSet

                DataAdapter.Fill(dataSet, "Resultado")

                ''CargarImagen
                Try
                    Dim bitmapImage As New BitmapImage
                    Dim bytes As Byte() = CType(dataSet.Tables(0).Rows(0).Item("foto1"), Byte())
                    Dim ms As New System.IO.MemoryStream(bytes)
                    bitmapImage.BeginInit()
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad
                    bitmapImage.StreamSource = ms
                    bitmapImage.EndInit()
                    img1.Source = bitmapImage

                Catch ex As Exception
                    img1.Source = New BitmapImage
                End Try

            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub cargarArchivoImagen()
        Dim openFile As New System.Windows.Forms.OpenFileDialog
        Dim bitmapImg As New BitmapImage
        openFile.Title = "Cargar Imagen"
        openFile.Filter = "Todos(*.*)|*.*|Imagenes|*.jpg;*.gif;*.png;*.bmp"
        If (openFile.ShowDialog()) Then
            Try
                bitmapImg.BeginInit()
                bitmapImg.UriSource = New Uri(openFile.FileName)
                bitmapImg.EndInit()
                img1.Source = bitmapImg
            Catch ex As Exception
                MessageBox.Show("No se cargo la imagen", "", MessageBoxButton.OK, MessageBoxImage.Information)
            End Try

        End If

    End Sub

    Private Sub LimpiarCampos()
        DataGrid1.SelectedItem = Nothing
        cargarDatos()

        tb_search.SearchText = ""
        tb_codigo.Text = ""
        tb_descripcion.Text = ""

        tb_precioComp.Text = "0"
        tb_precioVent.Text = "0"
        tb_precioMayo.Text = "0"

        tb_existencia.Text = "0"
        tb_minimo.Text = "0"
        cb_invent.IsChecked = False
        cb_unidad.IsChecked = False
        cb_granel.IsChecked = False
        tipo1.IsChecked = False
        tipo2.IsChecked = False
        tipo3.IsChecked = False

        xmlProducto = ""
        img1.Source = Nothing

    End Sub

    Private Sub GuardarCambios()
        Dim ok As Boolean = True
        If (tb_codigo.Text.Length <= 0 And tb_descripcion.Text.Length <= 0 And tb_precioComp.Text.Length <= 0 And tb_precioVent.Text.Length <= 0) Then
            ok = False
        End If

        If (cb_unidad.IsChecked = False And cb_granel.IsChecked = False) Then
            ok = False
        End If

        If (tipo1.IsChecked = False And tipo2.IsChecked = False And tipo3.IsChecked = False) Then
            ok = False
        End If

        If (ok) Then
            If (Mi_conexion.Conectar) Then
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[Sys_Productos]"
                SqlComand.Parameters.Clear()

                If (DataGrid1.SelectedIndex = -1) Then
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "NUEVO"))
                Else
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "MODIFICAR"))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@id_producto", CType(DataGrid1.SelectedItem, itemProducto).id_producto))
                End If

                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@Codigo", CType(tb_codigo.Text, Int64)))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@Descripcion", tb_descripcion.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@Granel", cb_granel.IsChecked))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@precioC", System.Convert.ToDecimal(tb_precioComp.Text, us)))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@precioV", System.Convert.ToDecimal(tb_precioVent.Text, us)))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@usInv", cb_invent.IsChecked))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@existen", CInt(tb_existencia.Text)))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@minimo", CInt(tb_minimo.Text)))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@idDepartamento", CInt(cb_departa.SelectedValue)))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xmlProducto", xmlProducto))

                Dim tipoProducto As Int16 = 0

                If tipo1.IsChecked Then
                    tipoProducto = 0
                End If
                If tipo2.IsChecked Then
                    tipoProducto = 1
                End If
                If tipo3.IsChecked Then
                    tipoProducto = 2
                End If

                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@tipoProducto", tipoProducto))


                'Preparando imagen para guardarla'
                Try
                    Dim bitmapImage As New BitmapImage
                    bitmapImage.BeginInit()
                    bitmapImage.UriSource = New Uri(img1.Source.ToString)
                    bitmapImage.EndInit()
                    Dim memStream As New MemoryStream
                    Dim encoder As New JpegBitmapEncoder
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage))
                    encoder.Save(memStream)
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@image", CType(memStream.ToArray(), Byte())))
                Catch ex As Exception
                End Try

                SqlComand.Connection = Mi_conexion.conexion
                Dim reader As SqlDataReader = SqlComand.ExecuteReader()
                Try
                    reader.Read()
                    MessageBox.Show(reader("Mensaje"), "", MessageBoxButton.OK, MessageBoxImage.Information)
                Catch ex As Exception
                End Try
                LimpiarCampos()
            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Else
            MessageBox.Show("Verifique que los campos no esten vacios", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub


    '************ EVENTOS UI ************
    Private Sub DataGrid1_selection(sender As Object, e As RoutedEventArgs) Handles DataGrid1.SelectionChanged
        cargarProducto()
    End Sub

    Private Sub button_click(sender As Object, e As RoutedEventArgs) Handles btn_Cancelar.Click, btn_nuevo.Click, btn_modif.Click, btn_Guardar.Click, btn_cargarImagen.Click, btn_modificarpaquete.Click
        Select Case sender.name
            Case "btn_nuevo"
                LimpiarCampos()
                habilitarCampos(True)
            Case "btn_modif"
                habilitarCampos(True)
            Case "btn_Cancelar"
                cargarProducto()
                habilitarCampos(False)
            Case "btn_Guardar"
                GuardarCambios()
                habilitarCampos(False)
            Case "btn_cargarImagen"
                cargarArchivoImagen()
            Case "btn_modificarpaquete"
                Dim form As New modal_EditarPaquete
                form.xml = xmlProducto
                If form.ShowDialog() Then
                    xmlProducto = form.xml
                End If
        End Select
    End Sub

    Private Sub validar_numbers(sender As Object, e As TextCompositionEventArgs) Handles tb_precioVent.PreviewTextInput, tb_precioComp.PreviewTextInput, tb_codigo.PreviewTextInput
        Try
            Dim regex As System.Text.RegularExpressions.Regex

            If (sender.name = "tb_codigo") Then
                regex = New System.Text.RegularExpressions.Regex("[^0-9]+")
            Else
                regex = New System.Text.RegularExpressions.Regex("([^0-9,]+).([^0-9]{2}+)")
            End If
            e.Handled = regex.IsMatch(e.Text)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub inventario_Checked() Handles cb_invent.Checked, cb_invent.Unchecked, cb_invent.LayoutUpdated

        tb_existencia.IsEnabled = IIf(cb_invent.IsChecked And cb_invent.IsEnabled, True, False)
        tb_existencia.Visibility = IIf(cb_invent.IsChecked, Visibility.Visible, Visibility.Collapsed)
        lb_invt_hay.Visibility = IIf(cb_invent.IsChecked, Visibility.Visible, Visibility.Collapsed)

        tb_minimo.IsEnabled = IIf(cb_invent.IsChecked And cb_invent.IsEnabled, True, False)
        tb_minimo.Visibility = IIf(cb_invent.IsChecked, Visibility.Visible, Visibility.Collapsed)
        lb_invt_min.Visibility = IIf(cb_invent.IsChecked, Visibility.Visible, Visibility.Collapsed)

    End Sub

    Private Sub tb_search_search(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        If (e.Key = Key.Enter) Then
            If (tb_search.SearchText <> "") Then
                tmpListaProductos.Clear()

                For Each xpr In listaProductos
                    If xpr.descripcion.ToUpper.Contains(tb_search.SearchText.ToUpper) Then
                        Dim xnewProd As itemProducto = xpr
                        tmpListaProductos.Add(xnewProd)
                    End If
                Next
                DataGrid1.ItemsSource = Nothing
                DataGrid1.ItemsSource = tmpListaProductos
            Else
                cargarDatos()
            End If
        End If
    End Sub

    Private Sub CheckBoxGroup1(sender As Object, e As Windows.RoutedEventArgs) Handles cb_granel.Checked, cb_granel.Unchecked, cb_unidad.Checked, cb_unidad.Unchecked
        If (sender.name = "cb_unidad") Then
            cb_granel.IsChecked = Not cb_unidad.IsChecked
        Else
            cb_unidad.IsChecked = Not cb_granel.IsChecked
        End If
        tiposUnidad.Visibility = IIf(cb_unidad.IsChecked, 0, 2)
    End Sub

    Private Sub CheckBoxGroup2(sender As Object, e As Windows.RoutedEventArgs) Handles tipo1.Checked, tipo2.Checked, tipo3.Checked, tipo3.Unchecked
        If (Not sender.name = "tipo1") Then
            tipo1.IsChecked = Not sender.IsChecked
        End If
        If (Not sender.name = "tipo2") Then
            tipo2.IsChecked = Not sender.IsChecked
        End If
        If (Not sender.name = "tipo3") Then
            tipo3.IsChecked = Not sender.IsChecked
        End If

        If (sender.name = "tipo3" And tipo3.IsChecked And tipo3.IsEnabled) Then
            Dim form As New modal_EditarPaquete
            If (form.ShowDialog()) Then
                xmlProducto = form.xml
            Else
                tipo3.IsChecked = False
            End If
        End If

        If (tipo3.IsChecked) Then
            btn_modificarpaquete.Visibility = Windows.Visibility.Visible
        Else
            btn_modificarpaquete.Visibility = Windows.Visibility.Collapsed
        End If

    End Sub

    Private Sub Image_Loaded(sender As Object, e As RoutedEventArgs)
        Select Case (sender.tag)
            Case -1 To 0
                CType(sender, Image).Source = FindResource("ImageIcon0")
            Case 1
                CType(sender, Image).Source = FindResource("ImageIcon1")
            Case 2
                CType(sender, Image).Source = FindResource("ImageIcon2")
        End Select
    End Sub
End Class
