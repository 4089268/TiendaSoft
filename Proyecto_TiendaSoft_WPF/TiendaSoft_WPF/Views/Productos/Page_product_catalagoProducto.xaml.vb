﻿
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Threading

Class Page_product_catalagoProducto
    Dim tablaDatos As DataTable
    Private _p1 As String = ""
    Property xmlProducto As String = ""

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
        cargarDatos()

        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")

        habilitarCampos(False)
        tb_existencia.IsEnabled = False
        tb_minimo.IsEnabled = False

        Try
            DataGrid1.SelectedIndex = 0
        Catch ex As Exception
        End Try

        If (_p1.Length > 0) Then
            tb_search.SearchText = _p1
            Try
                Dim dataview As DataView = DataGrid1.ItemsSource
                dataview.RowFilter = String.Format("descripcion like '%" & tb_search.SearchText & "%'")
                DataGrid1.ItemsSource = dataview
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
        DataGrid1.Columns(0).Width = DataGrid1.ActualWidth - 8
    End Sub

    Private Sub cargarDatos()
        If (Mi_conexion.Conectar) Then
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

            tablaDatos = dataSet.Tables(0)
            DataGrid1.ItemsSource = tablaDatos.DefaultView

            'cargar Catalagos
            Dim SqlComand2 = New SqlCommand
            SqlComand2.CommandTimeout = 500
            SqlComand2.CommandType = CommandType.StoredProcedure
            SqlComand2.CommandText = "[Global].[Sys_Departamentos]"
            SqlComand2.Parameters.Clear()
            SqlComand2.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "CARGARCATALAGO"))

            SqlComand2.Connection = Mi_conexion.conexion
            Dim DataAdapter2 As New SqlDataAdapter(SqlComand2)
            Dim dataSet2 As New DataSet
            DataAdapter2.Fill(dataSet2, "Resultado")

            cb_departa.ItemsSource = dataSet2.Tables(0).DefaultView

            cb_departa.SelectedIndex = 0

            'cargar Catalago tipoProductos


        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
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

        'tb_existencia.IsEnabled = val
        'tb_minimo.IsEnabled = val

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

                Dim form As New modal_EditarPaquete(Me)
                form.Owner = Me.Parent
                form.xml = xmlProducto
                Dim res As Boolean = form.ShowDialog()

        End Select

    End Sub

    Private Sub DataGrid1_selection(sender As Object, e As RoutedEventArgs) Handles DataGrid1.SelectionChanged
        cargarProducto()
    End Sub

    'valida que unicamente se tecleen numeros
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

    Private Sub cargarProducto()
        Try
            Dim us As New CultureInfo("en-US")
            Dim dataRow As DataRow = DataGrid1.SelectedItem.Row

            tb_codigo.Text = dataRow.Item("codigo")
            tb_descripcion.Text = dataRow.Item("descripcion")

            tb_precioMayo.Text = 0

            cb_departa.SelectedValue = dataRow.Item("id_familia")

            tb_existencia.Text = CInt(dataRow.Item("existencia"))
            tb_minimo.Text = CInt(dataRow.Item("minimo"))

            cb_invent.IsChecked = CBool(dataRow.Item("usaInventario"))

            cb_unidad.IsChecked = Not CBool(dataRow.Item("agranel"))
            cb_granel.IsChecked = CBool(dataRow.Item("agranel"))

            Select Case dataRow.Item("tipoProducto")
                Case "0"
                    tipo1.IsChecked = True
                Case "1"
                    tipo2.IsChecked = True
                Case "2"
                    tipo3.IsChecked = True
                    cargarXml(dataRow.Item("codigo"))
            End Select


            tb_precioVent.Text = IIf(dataRow.Item("precio_v").ToString = "", "0", CDec(dataRow.Item("precio_v")).ToString("####0.00", us))
            Try
                tb_precioComp.Text = CDec(dataRow.Item("precio_c")).ToString("####0.00", us)
            Catch ex As Exception
                tb_precioComp.Text = "0"
            End Try

            cargar_imagen(dataRow.Item("codigo"))

            

        Catch ex As Exception
        End Try


    End Sub

    Private Sub inventario_Checked() Handles cb_invent.Checked, cb_invent.Unchecked

        tb_existencia.IsEnabled = IIf(cb_invent.IsChecked And cb_invent.IsEnabled, True, False)
        tb_minimo.IsEnabled = IIf(cb_invent.IsChecked And cb_invent.IsEnabled, True, False)
    End Sub

    Private Sub LimpiarCampos()
        DataGrid1.SelectedItem = Nothing
        cargarDatos()

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
        img1.Source = Nothing

        xmlProducto = ""
        img1.Source = Nothing

    End Sub

    Private Sub GuardarCambios()
        Dim us As New CultureInfo("en-US")

        If (tb_codigo.Text <> "" And tb_descripcion.Text <> "" And tb_precioComp.Text <> "" And tb_precioVent.Text <> "") Then
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
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@id_producto", DataGrid1.SelectedItem.Row.item("id_producto")))
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

                Dim tipoProducto As Int16 = -1

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
                cargarDatos()

            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Else
            MessageBox.Show("Verifique que los campos no esten vacios", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
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

    Private Sub tb_search_search(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        If (e.Key = Key.Enter) Then
            If (tb_search.SearchText <> "") Then
                Dim dataview As DataView = DataGrid1.ItemsSource
                dataview.RowFilter = String.Format("descripcion like '%" & tb_search.SearchText & "%'")
                DataGrid1.ItemsSource = dataview
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
            Dim form As New modal_EditarPaquete(Me)
            form.Owner = Me.Parent
            Dim res As Boolean = form.ShowDialog()
            If (Not res) Then
                tipo3.IsChecked = False
            End If
        End If

        If (tipo3.IsChecked) Then
            btn_modificarpaquete.Visibility = Windows.Visibility.Visible
        Else
            btn_modificarpaquete.Visibility = Windows.Visibility.Collapsed
        End If

    End Sub

    Private Sub cargarXml(codigo As Int32)
        If (Mi_conexion.Conectar) Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[Global].[Sys_Productos]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "OBTENERPAQUETE"))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@Codigo", codigo))

            SqlComand.Connection = Mi_conexion.conexion
            Dim DataAdapter As New SqlDataAdapter(SqlComand)
            Dim dataSet As New DataSet

            DataAdapter.Fill(dataSet, "Resultado")
            xmlProducto = CType(dataSet.Tables(0).Rows(0).Item(0), String)

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

End Class
