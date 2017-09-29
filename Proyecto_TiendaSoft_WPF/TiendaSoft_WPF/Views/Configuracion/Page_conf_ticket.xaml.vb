Imports DevExpress.Xpf.Editors
Imports System.Globalization
Imports System.Data.SqlClient
Imports System.Data

Class Page_conf_ticket
    Dim navService As NavigationService
    Private xheight As Double = 600

    Dim Conf_Impresion As Conf_Impresion
    Dim datarow As DataRow
    Dim dataSetDatos As DataSet

#Region "****** variables ******"
    Dim nombre As String
    Dim ciudad As String
    Dim codigoPostal As String
    Dim colonia As String
    Dim direccion As String
    Dim estado As String
    Dim rfc As String
    Dim telefono As String
    Dim imagen As Byte()
#End Region

    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        navService = Me.NavigationService

        'Agregar eventos a los checkboxs'
        Addhandle_CheckBox(chb_titulo)
        Addhandle_CheckBox(chb_Direccion)
        Addhandle_CheckBox(chb_telefono)
        Addhandle_CheckBox(chb_linea)
        Addhandle_CheckBox(chb_lineab)
        Addhandle_CheckBox(chb_rfc)
        Addhandle_CheckBox(chb_folio)
        Addhandle_CheckBox(chb_cajero)
        Addhandle_CheckBox(chb_Fecha)
        Addhandle_CheckBox(chb_Cabecera)

        'Agregar eventos a los NumericSpinners'
        Addhandle_NumericSpinner(sp_general_mar)
        Addhandle_NumericSpinner(sp_general_ta)
        Addhandle_NumericSpinner(sp_titulo_ta)
        Addhandle_NumericSpinner(sp_titulo_es)
        Addhandle_NumericSpinner(sp_Direccion_es)
        Addhandle_NumericSpinner(sp_Direccion_ta)
        Addhandle_NumericSpinner(sp_telefono_es)
        Addhandle_NumericSpinner(sp_telefono_ta)
        Addhandle_NumericSpinner(sp_linea_ta)
        Addhandle_NumericSpinner(sp_linea_es)
        Addhandle_NumericSpinner(sp_lineab_ta)
        Addhandle_NumericSpinner(sp_lineab_es)
        Addhandle_NumericSpinner(sp_rfc_ta)
        Addhandle_NumericSpinner(sp_rfc_es)
        Addhandle_NumericSpinner(sp_folio_ta)
        Addhandle_NumericSpinner(sp_folio_es)
        Addhandle_NumericSpinner(sp_cajero_ta)
        Addhandle_NumericSpinner(sp_cajero_es)
        Addhandle_NumericSpinner(sp_Fecha_ta)
        Addhandle_NumericSpinner(sp_Fecha_es)
        Addhandle_NumericSpinner(sp_Cabecera_es)
        Addhandle_NumericSpinner(sp_Cabecera_ta)
        Addhandle_NumericSpinner(sp_Datos_es)
        Addhandle_NumericSpinner(sp_Datos_ta)
        Addhandle_NumericSpinner(sp_Total_es)
        Addhandle_NumericSpinner(sp_Total_ta)
        Addhandle_NumericSpinner(sp_Cambio_es)
        Addhandle_NumericSpinner(sp_Cambio_ta)

        cargarDatos()
        inicializarConfImpresion()
        DrawTicket()
    End Sub

    Private Sub cargarDatos()
        If (Mi_conexion.Conectar) Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[PDV].[Global].[sp_datos_empresa]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "OBTENER"))

            SqlComand.Connection = Mi_conexion.conexion
            Dim DataAdapter As New SqlDataAdapter(SqlComand)
            Dim dataSet As New DataSet

            DataAdapter.Fill(dataSet, "Resultado")

            Me.nombre = dataSet.Tables(0).Rows(0).Item("nombre_comercial")
            Me.ciudad = dataSet.Tables(0).Rows(0).Item("ciudad")
            Me.codigoPostal = dataSet.Tables(0).Rows(0).Item("codigo_postal")
            Me.colonia = dataSet.Tables(0).Rows(0).Item("colonia")
            Me.direccion = dataSet.Tables(0).Rows(0).Item("direccion")
            Me.estado = dataSet.Tables(0).Rows(0).Item("estado")
            Me.rfc = dataSet.Tables(0).Rows(0).Item("rfc")
            Me.telefono = dataSet.Tables(0).Rows(0).Item("telefono1")
            Me.imagen = (CType(dataSet.Tables(0).Rows(0).Item("logo"), Byte()))




            Dim SqlComand2 = New SqlCommand
            SqlComand2.CommandTimeout = 500
            SqlComand2.CommandType = CommandType.StoredProcedure
            SqlComand2.CommandText = "[Global].[Sys_ConfImpresion]"
            SqlComand2.Parameters.Clear()
            SqlComand2.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "CARGAR"))

            SqlComand2.Connection = Mi_conexion.conexion
            Dim DataAdapter2 As New SqlDataAdapter(SqlComand2)

            dataSetDatos = New DataSet
            DataAdapter2.Fill(dataSetDatos, "Resultado")

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            navService.Source = New Uri("Views/Page_Configuracion.xaml", UriKind.Relative)
        End If
    End Sub

    Private Sub inicializarConfImpresion()
        Conf_Impresion = New Conf_Impresion
        'General
        Conf_Impresion.GeneralMargen = CInt(dataSetDatos.Tables(0).Rows(0).Item("GeneralMargen"))
        Conf_Impresion.GeneralAncho = CInt(dataSetDatos.Tables(0).Rows(0).Item("GeneralAncho"))

        'Titulo
        Conf_Impresion.MostrarTitulo = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarTitulo"))
        Conf_Impresion.TituloTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("TituloTamanoFuente"))
        Conf_Impresion.TituloEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("TiruloEspacio"))

        'Direccion
        Conf_Impresion.MostrarDireccion = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarDireccion"))
        Conf_Impresion.DireccionTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("DireccionTamanoFuente"))
        Conf_Impresion.DireccionEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("DireccionEspacio"))

        'Telefono
        Conf_Impresion.MostrarTelefono = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarTelefono"))
        Conf_Impresion.TelefonoTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("TelefonoTamanoFuente"))
        Conf_Impresion.TelefonoEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("TelefonoEspacio"))

        'RFC
        Conf_Impresion.MostrarRfc = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarRfc"))
        Conf_Impresion.RfcTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("RfcTamanoFuente"))
        Conf_Impresion.RfcEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("RfcEspacio"))

        'Folio
        Conf_Impresion.MostrarFolio = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarFolio"))
        Conf_Impresion.FolioTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("FolioTamanoFuente"))
        Conf_Impresion.FolioEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("FolioEspacio"))

        'Cajero
        Conf_Impresion.MostrarCajero = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarCajero"))
        Conf_Impresion.CajeroTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("CajeroTamanoFuente"))
        Conf_Impresion.CajeroEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("CajeroEspacio"))

        'Fecha
        Conf_Impresion.MostrarFecha = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarFecha"))
        Conf_Impresion.FechaTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("FechaTamanoFuente"))
        Conf_Impresion.FechaEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("FechaEspacio"))

        'Linea A
        Conf_Impresion.MostrarLinea = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarLineaA"))
        Conf_Impresion.LineaTamaño = CInt(dataSetDatos.Tables(0).Rows(0).Item("LineaATamano"))
        Conf_Impresion.LineaEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("LineaAEspacio"))

        'Linea B
        Conf_Impresion.MostrarLineab = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarLineaB"))
        Conf_Impresion.LineaTamañob = CInt(dataSetDatos.Tables(0).Rows(0).Item("LineaATamano"))
        Conf_Impresion.LineaEspaciob = CInt(dataSetDatos.Tables(0).Rows(0).Item("LineaAEspacio"))

        'cabecera
        Conf_Impresion.MostrarCabecera = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarCabecera"))
        Conf_Impresion.CabeceraTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("CabeceraTamanoFuente"))
        Conf_Impresion.CabeceraEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("CabeceraEspacio"))

        'Productos
        Conf_Impresion.ProductosEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("ProductosEspacio"))
        Conf_Impresion.ProductosTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("ProductosTamanoFuente"))

        'Total
        Conf_Impresion.TotalEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("TotalEspacio"))
        Conf_Impresion.TotalTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("TotalTamanoFuente"))

        'Cambio
        Conf_Impresion.CambioEspacio = CInt(dataSetDatos.Tables(0).Rows(0).Item("CambioEspacio"))
        Conf_Impresion.CambioTamañoFuente = CInt(dataSetDatos.Tables(0).Rows(0).Item("CambioTamanoFuente"))



        '' ****** UI ******

        sp_Cambio_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("CambioEspacio"))
        sp_Cambio_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("CambioTamanoFuente"))

        chb_Cabecera.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarCabecera"))
        sp_Cabecera_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("CabeceraTamanoFuente"))
        sp_Cabecera_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("CabeceraEspacio"))

        chb_lineab.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarLineaB"))
        sp_lineab_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("LineaBTamano"))
        sp_lineab_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("LineaBEspacio"))

        chb_linea.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarLineaA"))
        sp_linea_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("LineaATamano"))
        sp_linea_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("LineaAEspacio"))

        chb_Fecha.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarFecha"))
        sp_Fecha_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("FechaTamanoFuente"))
        sp_Fecha_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("FechaEspacio"))

        chb_cajero.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarCajero"))
        sp_cajero_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("CajeroTamanoFuente"))
        sp_cajero_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("CajeroEspacio"))

        chb_folio.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarFolio"))
        sp_folio_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("FolioTamanoFuente"))
        sp_folio_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("FolioEspacio"))

        chb_rfc.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarRfc"))
        sp_rfc_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("RfcTamanoFuente"))
        sp_rfc_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("RfcEspacio"))

        chb_telefono.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarTelefono"))
        sp_telefono_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("TelefonoTamanoFuente"))
        sp_telefono_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("TelefonoEspacio"))

        chb_Direccion.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarDireccion"))
        sp_Direccion_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("DireccionTamanoFuente"))
        sp_Direccion_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("DireccionEspacio"))

        chb_titulo.IsChecked = CBool(dataSetDatos.Tables(0).Rows(0).Item("MostrarTitulo"))
        sp_titulo_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("TituloTamanoFuente"))
        sp_titulo_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("TiruloEspacio"))

        sp_general_mar.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("GeneralMargen"))
        sp_general_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("GeneralAncho"))

        sp_Datos_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("ProductosEspacio"))
        sp_Datos_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("ProductosTamanoFuente"))

        sp_Total_es.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("TotalEspacio"))
        sp_Total_ta.Value = CInt(dataSetDatos.Tables(0).Rows(0).Item("TotalTamanoFuente"))

    End Sub

    Private Sub DrawTicket()
        Dim drawingVisual As New DrawingVisual
        Dim dc As DrawingContext = drawingVisual.RenderOpen

        Dim p As New Point(Conf_Impresion.GeneralMargen, Conf_Impresion.GeneralMargen)

        img_visual.Width = Conf_Impresion.GeneralAncho
        img_visual.Height = xheight

        dc.DrawRectangle(Brushes.Gray, Nothing, New Rect(0, 0, Conf_Impresion.GeneralAncho, xheight))
        dc.DrawRectangle(Brushes.White, Nothing, New Rect(2, 2, Conf_Impresion.GeneralAncho - 5, xheight - 5))


        '*** TITULO ***
        If (Conf_Impresion.MostrarTitulo) Then
            'Dim bitmapImage As New BitmapImage
            'Dim ms As New System.IO.MemoryStream(imagen)
            'bitmapImage.BeginInit()
            'bitmapImage.CacheOption = BitmapCacheOption.OnLoad
            'bitmapImage.StreamSource = ms
            'bitmapImage.EndInit()
            'dc.DrawImage(bitmapImage, New Rect(New Size(80, Conf_Impresion.TituloTamañoFuente)))

            DrawingText(dc, nombre, Conf_Impresion.TituloTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.TituloEspacio

        Else
            DrawingText(dc, nombre, Conf_Impresion.TituloTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.TituloEspacio

        End If

        '*** Direccion ***
        If (Conf_Impresion.MostrarDireccion) Then
            DrawingText(dc, direccion & " " & codigoPostal, Conf_Impresion.DireccionTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.DireccionEspacio

            DrawingText(dc, colonia & " " & ciudad & " " & estado, Conf_Impresion.DireccionTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.DireccionEspacio
        End If

        '*** Telefono ***
        If (Conf_Impresion.MostrarTelefono) Then
            DrawingText(dc, "Tel.: " & telefono, Conf_Impresion.TelefonoTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.TelefonoEspacio
        End If

        '*** RFC ***
        If (Conf_Impresion.MostrarRfc) Then
            DrawingText(dc, "RFC: " & rfc, Conf_Impresion.RfcTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.RfcEspacio
        End If

        '*** FOLIO ***
        If (Conf_Impresion.MostrarFolio) Then
            DrawingText(dc, "No.: 000025654", Conf_Impresion.FolioTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.FolioEspacio
        End If

        '*** CAJERO ***
        If (Conf_Impresion.MostrarCajero) Then
            DrawingText(dc, "Atendio: Juan Salvador Rangel", Conf_Impresion.CajeroTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.CajeroEspacio
        End If

        '*** FECHA ***
        If (Conf_Impresion.MostrarFecha) Then
            DrawingText(dc, DateTime.Now.ToString("MM-dd-yyyy"), Conf_Impresion.FechaTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.FechaEspacio
        End If
       
        '*** CABECERA ***
        If (True) Then
            DrawingText(dc, "PRODUCTO", Conf_Impresion.CabeceraTamañoFuente, p.X, p.Y)
            DrawingText(dc, "Cantidad", Conf_Impresion.CabeceraTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 5, p.Y)
            DrawingText(dc, "IMPORTE", Conf_Impresion.CabeceraTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 7, p.Y)
            p.Y += Conf_Impresion.CabeceraEspacio
        End If

        '*** LINEA ***
        If (Conf_Impresion.MostrarLinea) Then
            dc.DrawLine(New Pen(Brushes.Black, Conf_Impresion.LineaTamaño), New Point(p.X, p.Y), New Point(Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen, p.Y))
            p.Y += Conf_Impresion.LineaEspacio
        End If

        For i As Integer = 0 To 5
            If (True) Then
                DrawingText(dc, "Cocacola ", Conf_Impresion.ProductosTamañoFuente, p.X, p.Y)
                DrawingText(dc, "1", Conf_Impresion.ProductosTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 5, p.Y)
                DrawingText(dc, "$21.00", Conf_Impresion.ProductosTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 7, p.Y)
                p.Y += Conf_Impresion.ProductosEspacio
            End If
        Next i

        p.Y += 20
        '*** LINEA 2 ***
        If (Conf_Impresion.MostrarLineab) Then
            dc.DrawLine(New Pen(Brushes.Black, Conf_Impresion.LineaTamañob), New Point(p.X, p.Y), New Point(Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen, p.Y))
            p.Y += Conf_Impresion.LineaEspaciob
        End If
        '*** TOTAL ***
        DrawingText(dc, "TOTAL: ", Conf_Impresion.TotalTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 5, p.Y)
        DrawingText(dc, "$105.00", Conf_Impresion.TotalTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 7, p.Y)
        p.Y += Conf_Impresion.TotalEspacio


        '*** CAMBIO ***
        DrawingText(dc, "EFECTIVO", Conf_Impresion.CambioTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 5, p.Y)
        DrawingText(dc, "$200.00", Conf_Impresion.CambioTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 7, p.Y)
        p.Y += Conf_Impresion.CabeceraEspacio
        DrawingText(dc, "CAMBIO", Conf_Impresion.CambioTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 5, p.Y)
        DrawingText(dc, "$95.00", Conf_Impresion.CambioTamañoFuente, ((Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen) / 9) * 7, p.Y)
        p.Y += Conf_Impresion.CabeceraEspacio


        dc.Close()
        Dim bmp As New RenderTargetBitmap(400, 800, 96, 96, PixelFormats.Pbgra32)
        bmp.Render(drawingVisual)
        img_visual.Source = bmp



    End Sub

    Private Sub DrawingText(Dc As DrawingContext, xContent As String, xFontSize As Integer, xPositionX As Integer, xPosiosionY As Integer)
        Dim ft As New FormattedText(xContent, New CultureInfo("en-EU"), Windows.FlowDirection.LeftToRight, New Typeface(New FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, New FontStretch()), xFontSize, Brushes.Black)
        Dc.DrawText(ft, New Point(xPositionX, xPosiosionY))

    End Sub


#Region " *** Add Handles methods*** "
    Private Sub Addhandle_NumericSpinner(sender As SpinEdit)
        Try
            RemoveHandler sender.EditValueChanged, AddressOf NumericSpinner_handles
        Catch ex As Exception
        End Try

        AddHandler sender.EditValueChanged, AddressOf NumericSpinner_handles
    End Sub

    Private Sub Addhandle_CheckBox(sender As CheckBox)
        Try
            RemoveHandler sender.Checked, AddressOf CheckBoxes_handles
            RemoveHandler sender.Unchecked, AddressOf CheckBoxes_handles
        Catch ex As Exception
        End Try

        AddHandler sender.Checked, AddressOf CheckBoxes_handles
        AddHandler sender.Unchecked, AddressOf CheckBoxes_handles
    End Sub

#End Region

#Region " *** Handles methods *** "
    Sub buttons_click(sender As Object, e As RoutedEventArgs) Handles btn_regresar.Click, btn_guardarCambios.Click, btn_imprimir.Click
        Select Case sender.name
            Case "btn_regresar"
                navService.Source = New Uri("Views/Page_Configuracion.xaml", UriKind.Relative)
            Case "btn_guardarCambios"
                GuardarCambios()
            Case "btn_imprimir"
                Imprimir()
        End Select
    End Sub

    Private Sub CheckBoxes_handles(sender As Object, e As Windows.RoutedEventArgs)
        Try
            Select Case sender.name
                Case "chb_titulo"
                    Conf_Impresion.MostrarTitulo = CType(sender, CheckBox).IsChecked
                Case "chb_Direccion"
                    Conf_Impresion.MostrarDireccion = CType(sender, CheckBox).IsChecked
                Case "chb_telefono"
                    Conf_Impresion.MostrarTelefono = CType(sender, CheckBox).IsChecked
                Case "chb_linea"
                    Conf_Impresion.MostrarLinea = CType(sender, CheckBox).IsChecked
                Case "chb_lineab"
                    Conf_Impresion.MostrarLineab = CType(sender, CheckBox).IsChecked
                Case "chb_rfc"
                    Conf_Impresion.MostrarRfc = CType(sender, CheckBox).IsChecked
                Case "chb_folio"
                    Conf_Impresion.MostrarFolio = CType(sender, CheckBox).IsChecked
                Case "chb_cajero"
                    Conf_Impresion.MostrarCajero = CType(sender, CheckBox).IsChecked
                Case "chb_fecha"
                    Conf_Impresion.MostrarFecha = CType(sender, CheckBox).IsChecked
                Case "chb_Cabecera"
                    Conf_Impresion.MostrarCabecera = CType(sender, CheckBox).IsChecked
            End Select
            DrawTicket()
        Catch ex As Exception
        End Try

    End Sub

    Private Sub NumericSpinner_handles(sender As Object, e As DevExpress.Xpf.Editors.EditValueChangedEventArgs)
        Select Case sender.name
            Case "sp_titulo_ta"
                Conf_Impresion.TituloTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_titulo_es"
                Conf_Impresion.TituloEspacio = CType(sender, SpinEdit).Value
            Case "sp_general_mar"
                Conf_Impresion.GeneralMargen = CType(sender, SpinEdit).Value
            Case "sp_general_ta"
                Conf_Impresion.GeneralAncho = CType(sender, SpinEdit).Value
            Case "sp_Direccion_ta"
                Conf_Impresion.DireccionTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_Direccion_es"
                Conf_Impresion.DireccionEspacio = CType(sender, SpinEdit).Value
            Case "sp_telefono_ta"
                Conf_Impresion.TelefonoTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_telefono_es"
                Conf_Impresion.TelefonoEspacio = CType(sender, SpinEdit).Value
            Case "sp_linea_ta"
                Conf_Impresion.LineaTamaño = CType(sender, SpinEdit).Value
            Case "sp_linea_es"
                Conf_Impresion.LineaEspacio = CType(sender, SpinEdit).Value

            Case "sp_lineab_ta"
                Conf_Impresion.LineaTamañob = CType(sender, SpinEdit).Value
            Case "sp_lineab_es"
                Conf_Impresion.LineaEspaciob = CType(sender, SpinEdit).Value


            Case "sp_rfc_ta"
                Conf_Impresion.RfcTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_rfc_es"
                Conf_Impresion.RfcEspacio = CType(sender, SpinEdit).Value
            Case "sp_folio_ta"
                Conf_Impresion.FolioTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_folio_es"
                Conf_Impresion.FolioEspacio = CType(sender, SpinEdit).Value
            Case "sp_cajero_ta"
                Conf_Impresion.CajeroTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_cajero_es"
                Conf_Impresion.CajeroEspacio = CType(sender, SpinEdit).Value
            Case "sp_Fecha_ta"
                Conf_Impresion.FechaTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_Fecha_es"
                Conf_Impresion.FechaEspacio = CType(sender, SpinEdit).Value
            Case "sp_Cabecera_es"
                Conf_Impresion.CabeceraEspacio = CType(sender, SpinEdit).Value
            Case "sp_Cabecera_ta"
                Conf_Impresion.CabeceraTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_Datos_es"
                Conf_Impresion.ProductosEspacio = CType(sender, SpinEdit).Value
            Case "sp_Datos_ta"
                Conf_Impresion.ProductosTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_Total_es"
                Conf_Impresion.TotalEspacio = CType(sender, SpinEdit).Value
            Case "sp_Total_ta"
                Conf_Impresion.TotalTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_Cambio_ta"
                Conf_Impresion.CambioTamañoFuente = CType(sender, SpinEdit).Value
            Case "sp_Cambio_es"
                Conf_Impresion.CambioEspacio = CType(sender, SpinEdit).Value

        End Select

        DrawTicket()
    End Sub

#End Region

    Private Sub GuardarCambios()
        If (Mi_conexion.Conectar) Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[Global].[Sys_ConfImpresion]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "MODIFICAR"))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xGeneralAncho", sp_general_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xGeneralMargen", sp_general_mar.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarTitulo", chb_titulo.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xTituloTamanoFuente", sp_titulo_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xTiruloEspacio", sp_titulo_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarDireccion", chb_Direccion.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xDireccionTamanoFuente", sp_Direccion_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xDireccionEspacio", sp_Direccion_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarTelefono", chb_telefono.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xTelefonoEspacio", sp_telefono_es.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xTelefonoTamanoFuente", sp_telefono_ta.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarRfc", chb_rfc.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xRfcTamanoFuente", sp_rfc_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xRfcEspacio", sp_rfc_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarFolio", chb_folio.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xFolioTamanoFuente", sp_folio_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xFolioEspacio", sp_folio_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarCajero", chb_cajero.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xCajeroTamanoFuente", sp_cajero_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xCajeroEspacio", sp_cajero_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarFecha", chb_Fecha.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xFechaTamanoFuente", sp_Fecha_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xFechaEspacio", sp_Fecha_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarCabecera", chb_Cabecera.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xCabeceraTamanoFuente", sp_Cabecera_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xCabeceraEspacio", sp_Cabecera_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xProductosTamanoFuente", sp_Datos_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xProductosEspacio", sp_Datos_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xCambioTamanoFuente", sp_Cambio_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xCambioEspacio", sp_Cambio_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xTotalTamanoFuente", sp_Total_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xTotalEspacio", sp_Total_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarLineaA", chb_linea.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xLineaATamano", sp_linea_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xLineaAEspacio", sp_linea_es.Value))

            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xMostrarLineaB", chb_lineab.IsChecked))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xLineaBTamano", sp_lineab_ta.Value))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xLineaBEspacio", sp_lineab_es.Value))


            SqlComand.Connection = Mi_conexion.conexion
            Dim DataAdapter As New SqlDataAdapter(SqlComand)
            Dim dataSet As New DataSet

            DataAdapter.Fill(dataSet, "Resultado")
            MessageBox.Show(dataSet.Tables(0).Rows(0).Item("mensaje"), "", MessageBoxButton.OK, MessageBoxImage.Information)

        Else
            MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            navService.Source = New Uri("Views/Page_Configuracion.xaml", UriKind.Relative)
        End If

    End Sub

    Private Sub Imprimir()
        Dim printDialog As New PrintDialog()
        If printDialog.ShowDialog() = True Then
            Dim paginator As New Paginador_Ticket(New Typeface("Calibri"), 16, 40, New Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight))
            printDialog.PrintDocument(paginator, "Ticket")
        End If
    End Sub

End Class
