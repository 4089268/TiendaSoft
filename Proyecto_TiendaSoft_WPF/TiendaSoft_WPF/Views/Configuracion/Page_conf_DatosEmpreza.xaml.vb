Imports System.Data.SqlClient
Imports System.Data

Class Page_conf_DatosEmpreza
    Dim navService As NavigationService
    Private tmpDatosEmpresa As New DM_DatosEmpresa
    Dim xMainWindow As MainWindow

    Public Sub New(xm As MainWindow)
        CargarDatos()
        InitializeComponent()
        Me.xMainWindow = xm
        Me.DataContext = tmpDatosEmpresa
    End Sub
    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        navService = Me.NavigationService
        cargarUi()
    End Sub

    Private Sub cargarUi()
        'tb_nombre.Text = tmpDatosEmpresa.Nombre
        'tb_direccion.Text = tmpDatosEmpresa.Direccion
        'tb_colonia.Text = tmpDatosEmpresa.Colonia
        'tb_ciudad.Text = tmpDatosEmpresa.Ciudad
        'tb_estado.Text = tmpDatosEmpresa.Estado
        'tb_codigoPostal.Text = tmpDatosEmpresa.Codigo_postal
        'tb_rfc.Text = tmpDatosEmpresa.Rfc
        'tb_telefono.Text = tmpDatosEmpresa.Telefono1
    End Sub
    Private Sub CargarDatos()
        If (Mi_conexion.Conectar) Then

            Dim params As New List(Of SqlParameter)
            params.Add(New SqlParameter("@xAlias", "OBTENER"))
            Using reader As SqlDataReader = Mi_conexion.Ejecutar_Procedimiento_dataReader("[Global].[sp_datos_empresa]", params)
                If reader.Read Then
                    tmpDatosEmpresa.Nombre = reader("nombre_comercial").ToString
                    tmpDatosEmpresa.Ciudad = reader("ciudad").ToString
                    tmpDatosEmpresa.Codigo_postal = reader("codigo_postal").ToString
                    tmpDatosEmpresa.Colonia = reader("colonia").ToString
                    tmpDatosEmpresa.Direccion = reader("direccion").ToString
                    tmpDatosEmpresa.Estado = reader("estado").ToString
                    tmpDatosEmpresa.Rfc = reader("rfc").ToString
                    tmpDatosEmpresa.Telefono1 = reader("telefono1").ToString

                    Try
                        Dim datosImagen = CType(reader("logo"), Byte())
                        tmpDatosEmpresa.CargarImagen(datosImagen)
                        Me.img1.Source = tmpDatosEmpresa.Logo
                    Catch ex As Exception
                    End Try

                End If
            End Using
        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            navService.Source = New Uri("Views/Page_Configuracion.xaml", UriKind.Relative)
        End If
    End Sub
    Private Sub GuardarCambios()
        If (tb_nombre.Text <> "") Then
            If (Mi_conexion.Conectar) Then
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[sp_datos_empresa]"
                SqlComand.Parameters.Clear()

                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "MODIFICAR"))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cNombre", tb_nombre.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cDireccion", tb_direccion.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cColonia", tb_colonia.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cCiudad", tb_ciudad.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cEstado", tb_estado.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cCodigoPostal", tb_codigoPostal.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cRfc", tb_rfc.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cTelefono", tb_telefono.Text))

                'Preparando imagen para guardarla'
                Try
                    Dim bitmapImage As New BitmapImage
                    bitmapImage.BeginInit()
                    bitmapImage.UriSource = New Uri(img1.Source.ToString)
                    bitmapImage.EndInit()
                    Dim memStream As New IO.MemoryStream
                    'Dim encoder As New JpegBitmapEncoder
                    Dim encoder As New PngBitmapEncoder
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage))
                    encoder.Save(memStream)
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@cLogo", CType(memStream.ToArray(), Byte())))
                Catch ex As Exception
                End Try

                SqlComand.Connection = Mi_conexion.conexion
                Dim reader As SqlDataReader = SqlComand.ExecuteReader()

                Try
                    reader.Read()
                    MessageBox.Show(reader("Mensaje"), "", MessageBoxButton.OK, MessageBoxImage.Information)
                Catch ex As Exception
                End Try

                CargarDatos()
                DatosEmpresa = tmpDatosEmpresa
                xMainWindow.ActualizarDatosEmpresa()
            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Else
            MessageBox.Show("Verifique que los campos no esten vacios", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation)
        End If
    End Sub
    Private Sub cargarImagen(foto As Byte())
        Try
            Dim bitmapImage As New BitmapImage
            Dim bytes As Byte() = foto
            Dim ms As New System.IO.MemoryStream(bytes)
            bitmapImage.BeginInit()
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad
            bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat
            bitmapImage.StreamSource = ms
            bitmapImage.EndInit()
            img1.Source = bitmapImage

        Catch ex As Exception
            img1.Source = New BitmapImage
        End Try
    End Sub


    '************ EVENTOS UI ************
    Sub buttons_click(sender As Object, e As RoutedEventArgs) Handles btn_regresar.Click, btn_cargarImagen.MouseLeftButtonUp, btn_Guardar.MouseLeftButtonUp
        Select Case sender.name
            Case "btn_regresar"
                navService.Navigate(New Page_Configuracion(xMainWindow))
            Case "btn_cargarImagen"
                cargarArchivoImagen()
            Case "btn_Guardar"
                GuardarCambios()
        End Select
    End Sub
    Private Sub validar_numbers(sender As Object, e As TextCompositionEventArgs) Handles tb_telefono.PreviewTextInput, tb_codigoPostal.PreviewTextInput
        Dim regex As System.Text.RegularExpressions.Regex

        regex = New System.Text.RegularExpressions.Regex("[^0-9]+")

        e.Handled = regex.IsMatch(e.Text)

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
End Class
