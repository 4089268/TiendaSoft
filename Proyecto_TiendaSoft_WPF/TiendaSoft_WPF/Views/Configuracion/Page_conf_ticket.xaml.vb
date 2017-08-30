Imports DevExpress.Xpf.Editors
Imports System.Globalization

Class Page_conf_ticket
    Dim navService As NavigationService
    Private xheight As Double = 600

    Dim Conf_Impresion As Conf_Impresion

    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        navService = Me.NavigationService

        'Agregar eventos a los checkboxs'
        Addhandle_CheckBox(chb_titulo)
        Addhandle_CheckBox(chb_Direccion)
        Addhandle_CheckBox(chb_telefono)
        Addhandle_CheckBox(chb_linea)
        Addhandle_CheckBox(chb_rfc)
        Addhandle_CheckBox(chb_folio)
        Addhandle_CheckBox(chb_cajero)
        Addhandle_CheckBox(chb_Fecha)


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

        Addhandle_NumericSpinner(sp_rfc_ta)
        Addhandle_NumericSpinner(sp_rfc_es)
        Addhandle_NumericSpinner(sp_folio_ta)
        Addhandle_NumericSpinner(sp_folio_es)
        Addhandle_NumericSpinner(sp_cajero_ta)
        Addhandle_NumericSpinner(sp_cajero_es)
        Addhandle_NumericSpinner(sp_Fecha_ta)
        Addhandle_NumericSpinner(sp_Fecha_es)



        chb_titulo.IsChecked = True
        chb_Direccion.IsChecked = True
        chb_telefono.IsChecked = True
        chb_linea.IsChecked = True

        inicializarConfImpresion()
        DrawTicket()
    End Sub


    Private Sub inicializarConfImpresion()
        Conf_Impresion = New Conf_Impresion
        'General
        Conf_Impresion.GeneralMargen = 5
        Conf_Impresion.GeneralAncho = 350

        'Titulo
        Conf_Impresion.MostrarTitulo = True
        Conf_Impresion.TituloTamañoFuente = 20
        Conf_Impresion.TituloEspacio = 25
        Conf_Impresion.TituloPosision = 1

        'Direccion
        Conf_Impresion.MostrarDireccion = True
        Conf_Impresion.DireccionTamañoFuente = 12
        Conf_Impresion.DireccionEspacio = 20

        'Telefono
        Conf_Impresion.MostrarTelefono = True
        Conf_Impresion.TelefonoTamañoFuente = 12
        Conf_Impresion.TelefonoEspacio = 20

        'RFC
        Conf_Impresion.MostrarRfc = True
        Conf_Impresion.RfcTamañoFuente = 12
        Conf_Impresion.RfcEspacio = 20

        'Folio
        Conf_Impresion.MostrarFolio = True
        Conf_Impresion.FolioTamañoFuente = 12
        Conf_Impresion.FolioEspacio = 20

        'Cajero
        Conf_Impresion.MostrarCajero = True
        Conf_Impresion.CajeroTamañoFuente = 12
        Conf_Impresion.CajeroEspacio = 20

        'Fecha
        Conf_Impresion.MostrarFecha = True
        Conf_Impresion.FechaTamañoFuente = 12
        Conf_Impresion.FechaEspacio = 20

        'Linea
        Conf_Impresion.MostrarLinea = True
        Conf_Impresion.LineaTamaño = 12
        Conf_Impresion.LineaEspacio = 20

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
            DrawingText(dc, "PUNTO DE VENTA", Conf_Impresion.TituloTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.TituloEspacio
        End If

        '*** Direccion ***
        If (Conf_Impresion.MostrarDireccion) Then
            DrawingText(dc, "CALLE AMARGURA NO. 12345 CP:11430", Conf_Impresion.DireccionTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.DireccionEspacio

            DrawingText(dc, "COL PERDO JOSE MENDEZ TAMAULIPAS MEXICO", Conf_Impresion.DireccionTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.DireccionEspacio
        End If

        '*** Telefono ***
        If (Conf_Impresion.MostrarTelefono) Then
            DrawingText(dc, "TEl: 834 1031608", Conf_Impresion.TelefonoTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.TelefonoEspacio
        End If

        '*** RFC ***
        If (Conf_Impresion.MostrarRfc) Then
            DrawingText(dc, "RFC: XXXXXXXXX-XX", Conf_Impresion.RfcTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.RfcEspacio
        End If

        '*** FOLIO ***
        If (Conf_Impresion.MostrarFolio) Then
            DrawingText(dc, "No.: 0002560054", Conf_Impresion.FolioTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.FolioEspacio
        End If

        '*** CAJERO ***
        If (Conf_Impresion.MostrarCajero) Then
            DrawingText(dc, "Atendio: Juan Salvador Rangel", Conf_Impresion.CajeroTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.CajeroEspacio
        End If

        '*** FECHA ***
        If (Conf_Impresion.MostrarFecha) Then
            DrawingText(dc, DateTime.Now.ToString("MM-DD-YYYY"), Conf_Impresion.FechaTamañoFuente, p.X, p.Y)
            p.Y += Conf_Impresion.FechaEspacio
        End If


        '*** LINEA ***
        If (Conf_Impresion.MostrarFecha) Then
            dc.DrawLine(New Pen(Brushes.Black, Conf_Impresion.LineaTamaño), New Point(p.X, p.Y), New Point(Conf_Impresion.GeneralAncho - Conf_Impresion.GeneralMargen, p.Y))
            p.Y += Conf_Impresion.LineaEspacio
        End If



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

#Region " *** Handles methods*** "
    Sub buttons_click(sender As Object, e As RoutedEventArgs) Handles btn_regresar.Click
        Select Case sender.name
            Case "btn_regresar"
                navService.Source = New Uri("Views/Page_Configuracion.xaml", UriKind.Relative)
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

                Case "chb_rfc"
                    Conf_Impresion.MostrarRfc = CType(sender, CheckBox).IsChecked
                Case "chb_folio"
                    Conf_Impresion.MostrarFolio = CType(sender, CheckBox).IsChecked
                Case "chb_cajero"
                    Conf_Impresion.MostrarCajero = CType(sender, CheckBox).IsChecked
                Case "chb_fecha"
                    Conf_Impresion.MostrarFecha = CType(sender, CheckBox).IsChecked
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

        End Select

        DrawTicket()
    End Sub

#End Region


End Class
