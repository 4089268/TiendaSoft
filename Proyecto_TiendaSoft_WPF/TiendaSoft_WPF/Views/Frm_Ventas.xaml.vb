Imports System
Imports System.Data.SqlClient
Imports System.Data
Imports SUT.PrintEngine.Utils
Imports System.Collections.Generic

Imports System.IO
Imports System.Text
Imports System.Windows

Imports System.Windows.Markup
Imports System.Xaml
Imports Point.Point
Imports System.Globalization

Class Frm_Ventas

    Property imp_cobrar As Decimal = 0

    Dim ds As DataSet = New DataSet()
    Dim strCurrency As String = ""
    Dim acceptableKey As Boolean = False
    Dim xagranel As Boolean = False
    Dim si_graba As Boolean = False


    Private Sub txt_codigo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_codigo.KeyDown

        If e.Key = Key.OemPlus _
            OrElse e.Key = Key.OemMinus _
            OrElse (e.Key = Key.Enter) _
            OrElse (e.Key = Key.Escape) _
            OrElse (e.Key = Key.Tab) _
            OrElse (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) _
            OrElse (e.Key >= Key.D0 And e.Key <= Key.D9) Then

            If e.Key = Key.OemPlus Then
                txt_cant.Text = CStr(CDec(txt_cant.Text) + 1)
                e.Handled = True
            End If

            If e.Key = Key.OemMinus Then
                txt_cant.Text = CStr(IIf(CDec(txt_cant.Text) > 1, CDec(txt_cant.Text) - 1, CDec(txt_cant.Text)))
                e.Handled = True
            End If

            If e.Key = Key.Enter Then


                Dim xcod As String = TryCast(sender.text, String)
                Dim cn As New SqlConnection(My.Settings.TiendaSoftConnectionString)
                Dim cmd As New SqlCommand()


                cn.Open()
                cmd.CommandType = CommandType.Text
                cmd.CommandText = "select id_producto,codigo,descripcion,isnull(precio_v,0) as precio_v,isnull(precio_c,0) as precio_c,isnull(existencia,0) as existencia, agranel from Opr_Productos where isnull(inactivo,0)=0 and codigo='" + xcod + "'"
                cmd.Connection = cn
                cmd.Parameters.Clear()
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    si_graba = False
                    If reader.HasRows Then
                        Do While reader.Read()
                            txt_codigo.Tag = reader("id_producto")
                            'txt_desc.Text = reader("descripcion")
                            txt_desc.Content = reader("descripcion")
                            txt_pre.Text = reader("precio_v")
                            txt_pre.Tag = reader("precio_c")
                            txt_imp.Text = CDec(txt_pre.Text) * CDec(txt_cant.Text)
                            txt_exis.Text = reader("existencia")
                            xagranel = reader("agranel")
                            si_graba = True
                        Loop

                    Else
                        'MessageBox.Show("El código capturado, no existe en el Inventario")
                        si_graba = False
                    End If
                    reader.Close()
                Finally
                    cn.Close()
                    'si_graba = False
                End Try


                If Not xagranel And si_graba Then
                    Me.btn_graba_Click(sender, e)
                    'Me.btn_registrar(sender, e)
                    txt_cant.IsEnabled = xagranel
                Else
                    txt_cant.IsEnabled = xagranel
                    Dim request = New TraversalRequest(FocusNavigationDirection.Next)
                    request.Wrapped = True
                    DirectCast(sender, TextBox).MoveFocus(request)
                    'txt_cant.Focus()
                End If



                e.Handled = True




            End If




            '    e.Key = Key.Tab
            '    'Me.SelectNextControl(Me.ActiveControl, True, True, True, False)
            '    'Forms.SendKeys.Send("{TAB}")
            '    '            acceptableKey = False
            '    'Keyboard.send()



            'End If

            '            if (char.IsNumber(e.KeyChar) == false)
            '            {
            '                e.Handled = true;
            '            }
            '//backspace working code
            '            if (e.KeyChar == (char)Keys.Back)
            '            {
            '                e.Handled = false;
            '            }


        Else
            'acceptableKey = False
            e.Handled = True

        End If


        ' End If

        '    If (e.Key >= Forms.Keys.D0 And e.KeyCode <= Keys.D9) _
        'OrElse (e.KeyCode = Keys.Up) _
        'OrElse (e.KeyCode = Keys.Down) _
        'OrElse (e.KeyCode = Keys.Enter) _
        'OrElse (e.KeyCode = Keys.Escape) _
        'OrElse (e.KeyCode >= Keys.NumPad0 And e.KeyCode <= Keys.NumPad9) _
        '    OrElse (e.KeyCode >= Keys.F1) OrElse (e.KeyCode >= Keys.F2) OrElse e.KeyCode = Keys.Back Then

        '        If e.KeyCode = Keys.Up Then
        '            txt_cant.Text = CStr(CDec(txt_cant.Text) + 1)
        '        End If

        '        If e.KeyCode = Keys.Down Then
        '            txt_cant.Text = CStr(IIf(CDec(txt_cant.Text) > 1, CDec(txt_cant.Text) - 1, CDec(txt_cant.Text)))
        '        End If



        '        If e.KeyCode = Keys.Escape Then
        '            btn_salir_Click(sender, e)
        '        End If

        '        If e.KeyCode = Keys.F1 Then
        '            btn_busca_Click(sender, e)
        '        End If

        '        If e.KeyCode = Keys.F2 Then
        '            btn_comun_Click(sender, e)
        '            'btn_busca_Click(sender, e)
        '        End If

        '        If e.KeyCode = Keys.Enter Then
        '            SendKeys.Send("{TAB}")
        '            acceptableKey = False
        '        Else
        '            acceptableKey = True
        '        End If
        '    Else
        '        acceptableKey = False
        '    End If

    End Sub

    'Private Sub grd_prod_KeyDown(sender As Object, e As KeyEventArgs) Handles grd_prod.KeyDown

    '    If e.Key = Key.Up Then

    '        Dim txtBox As TextBox = e.Source
    '        If txtBox.Name = "txt_codigo" Then

    '            txt_cant.Text = CStr(CDec(txt_cant.Text) + 1)


    '        End If
    '    End If

    '    'if(txtBox != null)
    '    '{
    '    '    this.txtBlock.Text = txtBox.Text;
    '    '    this.txtBlock.Background = new SolidColorBrush(Colors.LightGray);
    '    '}


    'End Sub

    'Private Sub txt_codigo_LostFocus(sender As Object, e As RoutedEventArgs) Handles txt_codigo.LostFocus

    '    Dim xcod As String = TryCast(sender.text, String)
    '    Dim cn As New SqlConnection(My.Settings.TiendaSoftConnectionString)
    '    Dim cmd As New SqlCommand()


    '    cn.Open()
    '    cmd.CommandType = CommandType.Text
    '    cmd.CommandText = "select id_producto,codigo,descripcion,isnull(precio_v,0) as precio_v,isnull(precio_c,0) as precio_c,isnull(existencia,0) as existencia, agranel from Opr_Productos where isnull(inactivo,0)=0 and codigo='" + xcod + "'"
    '    cmd.Connection = cn
    '    cmd.Parameters.Clear()
    '    Try
    '        Dim reader As SqlDataReader = cmd.ExecuteReader()
    '        si_graba = False
    '        If reader.HasRows Then
    '            Do While reader.Read()
    '                txt_codigo.Tag = reader("id_producto")
    '                'txt_desc.Text = reader("descripcion")
    '                txt_desc.Content = reader("descripcion")
    '                txt_pre.Text = reader("precio_v")
    '                txt_pre.Tag = reader("precio_c")
    '                txt_imp.Text = CDec(txt_pre.Text) * CDec(txt_cant.Text)
    '                txt_exis.Text = reader("existencia")
    '                xagranel = reader("agranel")
    '                si_graba = True
    '            Loop

    '        Else
    '            'MessageBox.Show("El código capturado, no existe en el Inventario")
    '            si_graba = False
    '        End If
    '        reader.Close()
    '    Finally
    '        cn.Close()
    '        'si_graba = False
    '    End Try


    '    If Not xagranel And si_graba Then
    '        'Me.btn_registrar(sender, e)
    '        txt_cant.IsEnabled = xagranel
    '    Else
    '        txt_cant.IsEnabled = xagranel
    '        txt_cant.Focus()
    '    End If


    'End Sub


    'Private Sub txt_desc_PreviewKeyDown(sender As Object, e As KeyEventArgs) Handles txt_desc.PreviewKeyDown

    '    If e.Key = Key.OemPlus OrElse e.Key = Key.OemMinus OrElse (e.Key >= Key.D0 And e.Key <= Key.D9) Then


    '        If e.Key = Key.OemPlus Then
    '            txt_cant.Text = CStr(CDec(txt_cant.Text) + 1)
    '        End If

    '        If e.Key = Key.OemMinus Then
    '            txt_cant.Text = CStr(IIf(CDec(txt_cant.Text) > 1, CDec(txt_cant.Text) - 1, CDec(txt_cant.Text)))
    '        End If

    '    Else
    '        'acceptableKey = False
    '        e.Handled = False

    '    End If


    'End Sub

    Private Sub txt_codigo_GotFocus(sender As Object, e As RoutedEventArgs) Handles txt_codigo.GotFocus
        txt_desc.Content = ""
        txt_pre.Text = "0.00"
        txt_imp.Text = "0.00"
        txt_cant.IsEnabled = False
        txt_codigo.SelectAll()
    End Sub

    Private Sub btn_graba_Click(sender As Object, e As RoutedEventArgs) Handles btn_graba.Click
        If si_graba And CDec(IIf(txt_pre.Text.Length > 0, txt_pre.Text, 0)) > 0 And CDec(IIf(txt_cant.Text.Length > 0, txt_cant.Text, 0)) > 0 Then

            Dim dr As DataRow

            'dr = ds.Tables(tab_venta.SelectedIndex).NewRow()
            dr = ds.Tables(0).NewRow()
            dr("id_producto") = txt_codigo.Tag
            dr("codigo") = txt_codigo.Text
            dr("descripcion") = txt_desc.Content
            dr("precio_v") = txt_pre.Text
            dr("precio_c") = txt_pre.Tag
            dr("cantidad") = txt_cant.Text
            dr("importe") = txt_pre.Text * txt_cant.Text
            dr("existencia") = txt_exis.Text

            'Dim mis_rows = ds.Tables(tab_venta.SelectedIndex).Select("codigo=" + "'" + txt_codigo.Text + "'")
            Dim mis_rows = ds.Tables(0).Select("codigo=" + "'" + txt_codigo.Text + "'")
            If mis_rows.Count > 0 Then
                For Each rows In mis_rows
                    rows.Item("cantidad") = rows.Item("cantidad") + dr("cantidad")
                    rows.Item("importe") = rows.Item("cantidad") * rows.Item("precio_v")
                Next
            Else
                'ds.Tables(tab_venta.SelectedIndex).Rows.Add(dr)
                ds.Tables(0).Rows.Add(dr)

            End If

            Me.Despliega_Totales()
            'grd_venta.Refresh()

            txt_codigo.Text = ""
            txt_codigo.Tag = ""
            txt_desc.Content = ""
            txt_pre.Text = ""
            txt_cant.Text = "1"
            txt_imp.Text = ""
            txt_exis.Text = ""


            grd_venta.ItemsSource = ds.Tables(0).DefaultView

            'grd_venta1.DataMember = ds.Tables.Item(tab_venta.SelectedIndex).TableName
            'grd_venta..DataMember = ds.Tables(tab_venta.SelectedIndex).TableName
            'SendKeys.Send("{TAB}")
            'SendKeys.Send = Enter
            si_graba = False
            txt_codigo.Focus()
        Else
            txt_codigo.Focus()

        End If

    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("id_producto", GetType(Int32)))
        dt.Columns.Add(New DataColumn("codigo", GetType(String)))
        dt.Columns.Add(New DataColumn("descripcion", GetType(String)))
        dt.Columns.Add(New DataColumn("precio_v", GetType(Decimal)))
        dt.Columns.Add(New DataColumn("precio_c", GetType(Decimal)))
        dt.Columns.Add(New DataColumn("cantidad", GetType(Decimal)))
        dt.Columns.Add(New DataColumn("importe", GetType(Decimal)))
        dt.Columns.Add(New DataColumn("existencia", GetType(Decimal)))

        ds.Tables.Add(dt)

        grd_venta.AutoGenerateColumns = False
        grd_venta.Columns.Clear()
        grd_venta.IsReadOnly = False
        grd_venta.CanUserAddRows = False


        Dim TextBoxColumn1 As New DataGridTextColumn
        TextBoxColumn1.Header = "Código"
        TextBoxColumn1.Binding = New Binding("codigo")
        TextBoxColumn1.Width = 139
        TextBoxColumn1.IsReadOnly = True

        Dim TextBoxColumn2 As New DataGridTextColumn
        TextBoxColumn2.Header = "Descripción"
        TextBoxColumn2.Binding = New Binding("descripcion")
        TextBoxColumn2.Width = New DataGridLength(1, DataGridLengthUnitType.Star) 'ajustable
        TextBoxColumn2.IsReadOnly = True

        Dim TextBoxColumn3 As New DataGridTextColumn
        TextBoxColumn3.Header = "Precio"
        TextBoxColumn3.Width = 89
        TextBoxColumn3.Binding = New Binding("precio_v")
        TextBoxColumn3.IsReadOnly = True

        Dim TextBoxColumn4 As New DataGridTextColumn
        TextBoxColumn4.Header = "Cant"
        TextBoxColumn4.Binding = New Binding("cantidad")
        TextBoxColumn4.Width = 89
        TextBoxColumn4.IsReadOnly = True

        Dim TextBoxColumn5 As New DataGridTextColumn
        TextBoxColumn5.Header = "Importe"
        TextBoxColumn5.Binding = New Binding("importe")
        TextBoxColumn5.Width = 89
        TextBoxColumn5.IsReadOnly = True


        Dim TextBoxColumn6 As New DataGridTextColumn
        TextBoxColumn6.Header = "Existencia"
        TextBoxColumn6.Width = 89
        TextBoxColumn6.Binding = New Binding("existencia")
        TextBoxColumn6.IsReadOnly = True


        grd_venta.Columns.Add(TextBoxColumn1)
        grd_venta.Columns.Add(TextBoxColumn2)
        grd_venta.Columns.Add(TextBoxColumn3)
        grd_venta.Columns.Add(TextBoxColumn4)
        grd_venta.Columns.Add(TextBoxColumn5)
        grd_venta.Columns.Add(TextBoxColumn6)


        grd_venta.ItemsSource = ds.Tables(0).DefaultView



    End Sub

    Private Sub Despliega_Totales()
        Dim xart As Decimal = 0
        Dim xtot As Decimal = 0

        'For i = 0 To ds.Tables(tab_venta.SelectedIndex).Rows.Count - 1
        For i = 0 To ds.Tables(0).Rows.Count - 1
            'xart = xart + ds.Tables(tab_venta.SelectedIndex).Rows(i).Item("cantidad")
            'xtot = xtot + ds.Tables(tab_venta.SelectedIndex).Rows(i).Item("importe")
            Try
                xart = xart + ds.Tables(0).Rows(i).Item("cantidad")
                xtot = xtot + ds.Tables(0).Rows(i).Item("importe")
            Catch
            End Try

        Next
        lbl_articulos.Content = xart
        lbl_total.Content = FormatCurrency(xtot, 2)
        Me.imp_cobrar = xtot

    End Sub

    Private Sub btn_cobrar_Click(sender As Object, e As RoutedEventArgs) Handles btn_cobrar.Click

        Dim xform As New Frm_Cobrar(Me.imp_cobrar)
        'xform.ShowDialog()
        Dim result = xform.ShowDialog()
        If result = True Then
            'If xform.ShowDialog() = DialogResult.OK Then
            'MessageBox.Show("Dio Aceptar a grabar...")
            'If ds.Tables(tab_venta.SelectedIndex).Rows.Count > 0 Then
            Dim cn As New SqlConnection(My.Settings.TiendaSoftConnectionString)
            Dim cmd As New SqlCommand()

            Dim xid_prod As Int32 = 0
            Dim xpre1 As Decimal = 0
            Dim xpre2 As Decimal = 0
            Dim xcant As Decimal = 0
            Dim ximpo As Decimal = 0
            Dim xexis As Decimal = 0

            Dim dss As New DataSet
            dss.DataSetName = "VENTAS"

            Dim dts As New DataTable
            dts.Columns.Add(New DataColumn("fecha", GetType(DateTime)))
            dts.Columns.Add(New DataColumn("id_operador", GetType(Integer)))
            dts.Columns.Add(New DataColumn("total", GetType(Decimal)))
            dts.Columns.Add(New DataColumn("pago", GetType(Decimal)))
            dts.Columns.Add(New DataColumn("cambio", GetType(Decimal)))
            dts.TableName = "TMP_VENTAS"
            dss.Tables.Add(dts)

            Dim dr1 As DataRow
            dr1 = dss.Tables(0).NewRow()
            dr1("fecha") = Date.Now()
            'dr1("id_operador") = _Id_Operador
            dr1("id_operador") = 1
            dr1("total") = imp_pagar
            dr1("pago") = imp_pago
            dr1("cambio") = imp_cambio
            dts.Rows.Add(dr1)


            Dim dt2 As DataTable
            'dt2 = ds.Tables(tab_venta.SelectedIndex).Copy()
            dt2 = ds.Tables(0).Copy()

            dt2.TableName = "TMP_DETVENTAS"
            dss.Tables.Add(dt2)

            Dim xxml As String = dss.GetXml()

            cn.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_guarda_xml"

            cmd.Connection = cn
            cmd.Parameters.Clear()

            cmd.Parameters.AddWithValue("@cAlias", "VENTAS")
            cmd.Parameters.AddWithValue("@cAccion", "1")
            cmd.Parameters.AddWithValue("@xmlTables", dss.GetXml())

            Dim sw_grabo As Boolean = True
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                sw_grabo = False
                MessageBox.Show("!Error al grabar..." + Chr(13) + ex.Message)
            Finally
                cn.Close()
            End Try

            'Dim dsLog As DataSet = Variables.Current.usp_Guarda_XML("Tmp_Ventas".ToUpper, "1", dss)

            'Dim dsLog As DataSet = clsCallProcedures.usp_Guarda_XML(Me.TableName.ToUpper, CurrentAccion.ToString.ToUpper, Me.DataEnvironment)
            'clsDataLayer.Browse(dsLog)
            'tLog = dsLog.Tables(0).Copy
            'Dim oFrmLog As New clsControles.frmShowTransactLog(tLog)
            'lContinuar = oFrmLog.SinErrores

            'For j = 0 To ds.Tables(tab_venta.SelectedIndex).Rows.Count - 1
            '    xid_prod = ds.Tables(tab_venta.SelectedIndex).Rows(j).Item("id_producto")
            '    xpre1 = ds.Tables(tab_venta.SelectedIndex).Rows(j).Item("precio")
            '    xcant = ds.Tables(tab_venta.SelectedIndex).Rows(j).Item("cantidad")
            '    ximpo = ds.Tables(tab_venta.SelectedIndex).Rows(j).Item("importe")
            '    xpre2 = ds.Tables(tab_venta.SelectedIndex).Rows(j).Item("precio2")
            '    xexis = ds.Tables(tab_venta.SelectedIndex).Rows(j).Item("existencia")

            '    cn.Open()
            '    cmd.CommandType = CommandType.Text
            '    cmd.CommandText = "Insert Into dbo.Tmp_DetVentas (id_producto,precio_v,precio_c, cantidad, " & _
            '                        "importe, existencia) values (" + xid_prod.ToString + "," + _
            '                        xpre1.ToString + "," + xpre2.ToString + "," + xcant.ToString + "," + _
            '                        ximpo.ToString + "," + xexis.ToString + ")"

            '    cmd.Connection = cn
            '    cmd.Parameters.Clear()

            '    Try
            '        cmd.ExecuteNonQuery()
            '    Catch ex As Exception
            '        sw_grabo = False
            '        MessageBox.Show("!Error al grabar..." + Chr(13) + ex.Message)
            '    Finally
            '        cn.Close()
            '    End Try
            'Next j

            If sw_grabo Then
                ds.Tables(0).Rows.Clear()
                'If tab_venta.SelectedIndex = 0 Then
                'If tab_venta.SelectedIndex = 0 Then

                '    ds.Tables(tab_venta.SelectedIndex).Rows.Clear()
                'Else

                '    Dim dt As DataTable = ds.Tables(tab_venta.SelectedIndex)
                '    ds.Tables.Remove(dt)

                '    Dim xval_ant As Int16 = tab_venta.SelectedIndex

                '    Dim pg As TabPage = tab_venta.TabPages(tab_venta.SelectedIndex)
                '    tab_venta.TabPages.Remove(pg)

                '    tab_venta.SelectedIndex = xval_ant - 1

                'End If
                Me.Despliega_Totales()
            End If
            'End If

            'clsDataLayer.Browse(ds)
            'lbl_articulos.Text = xart
            'lbl_total.Text = FormatCurrency(xtot, 2)
            'Me.imp_cobrar = xtot
            'MessageBox.Show("cobre")
        End If


    End Sub

    Private Sub btn_imprimir_Click(sender As Object, e As RoutedEventArgs) Handles btn_imprimir.Click
        
        Dim printDialog As New PrintDialog()
        If printDialog.ShowDialog() = True Then
            Dim paginator As New Paginador_Ticket(ds.Tables(0), New Typeface("Calibri"), 16, 40, New Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight))

            printDialog.PrintDocument(paginator, "Ticket")
        End If


    End Sub

    Private Sub txt_cant_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_cant.KeyDown

        If (e.Key >= Key.D0 And e.Key <= Key.D9) OrElse (e.Key = Key.Enter) _
                OrElse (e.Key = Key.Up) OrElse (e.Key = Key.Down) _
                OrElse (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) _
                OrElse e.Key = Key.Back Then

            If e.Key = Key.Up Then
                txt_cant.Text = CStr(CDec(txt_cant.Text) + 1)
            End If

            If e.Key = Key.Down Then
                txt_cant.Text = CStr(IIf(CDec(txt_cant.Text) > 1, CDec(txt_cant.Text) - 1, CDec(txt_cant.Text)))
            End If

            If e.Key = Key.Enter Then

                acceptableKey = False
                Me.btn_graba_Click(sender, e)

            Else
                acceptableKey = True
            End If
        Else
            e.Handled = True
            acceptableKey = False
        End If


    End Sub



    Private Sub grd_venta_UnloadingRow(sender As Object, e As DataGridRowEventArgs) Handles grd_venta.UnloadingRow
        'MessageBox.Show("unloading row")
        Me.Despliega_Totales()
    End Sub

    Private Sub btn_busqueda_Click(sender As Object, e As RoutedEventArgs) Handles btn_busqueda.Click
        Dim xform As New Frm_Busqueda(Me)
        Dim result = xform.ShowDialog()
        If result = True Then


        End If
    End Sub
End Class


Public Class Paginador_Ticket

    Inherits DocumentPaginator

    Private dt As DataTable

    Private typeface As Typeface
    Private fontSize As Double
    Private margin As Double
    Private _pageSize As Size


    Private rowsPerPage As Integer
    Private _pageCount As Integer


    Public Overrides Property PageSize() As Size
        Get
            Return _pageSize
        End Get
        Set(ByVal value As Size)
            _pageSize = value
            PaginateData()
        End Set
    End Property


    Public Sub New(ByVal dt As DataTable, ByVal typeface As Typeface, ByVal fontSize As Double, ByVal margin As Double, ByVal pageSize As Size)
        Me.dt = dt
        Me.typeface = typeface
        Me.fontSize = fontSize
        Me.margin = margin
        Me._pageSize = New Size(pageSize.Width, pageSize.Height + 80)
        ''Me._pageSize = pageSize
        PaginateData()
    End Sub

    Private Function GetFormattedText(ByVal text As String) As FormattedText
        Return GetFormattedText(text, typeface)
    End Function

    Private Function GetFormattedText(ByVal text As String, ByVal typeface As Typeface) As FormattedText
        Return New FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black)
    End Function

    Private Sub PaginateData()
        Dim text As FormattedText = GetFormattedText("A")
        rowsPerPage = CInt((_pageSize.Height - margin * 2) / text.Height)
        rowsPerPage -= 1
        _pageCount = CInt(Math.Ceiling(CDbl(dt.Rows.Count) / rowsPerPage))
    End Sub

    Public Overrides ReadOnly Property IsPageCountValid() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property PageCount() As Integer
        Get
            Return _pageCount
        End Get
    End Property

    Public Overrides ReadOnly Property Source() As IDocumentPaginatorSource
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides Function GetPage(ByVal pageNumber As Integer) As DocumentPage

        'Create a test string for the purposes of measurement.        
        Dim text As FormattedText = GetFormattedText("A")

        Dim col1_X As Double = margin
        Dim col2_X As Double = col1_X + 95
        Dim col3_X As Double = col1_X + 95 + 300


        ' Calculate the range of rows that fits on this page.        
        Dim minRow As Integer = pageNumber * rowsPerPage
        Dim maxRow As Integer = minRow + rowsPerPage

        ' Create the visual for the page.        
        Dim visual As New DrawingVisual()

        ' Set the position to the top-left corner of the printable area.        
        Dim point As New Windows.Point(margin, margin)

        ' Print the column values.        
        Using dc As DrawingContext = visual.RenderOpen()

            Dim columnHeaderTypeface1 As New Typeface(typeface.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal)

            Dim header As New Header_Ticket
            Dim headertemplate As String = System.Windows.Markup.XamlWriter.Save(header)
            Dim headerElement As FrameworkElement = CType(System.Windows.Markup.XamlReader.Parse(headertemplate), FrameworkElement)

            UpdateSize(headerElement, 2000)

            dc.DrawRectangle(New VisualBrush(headerElement), Nothing, New Rect(point.X, point.Y, point.X + headerElement.ActualWidth, point.Y + headerElement.ActualHeight))

            point.Y += headerElement.ActualHeight

            ' Draw the column headers.            
            Dim columnHeaderTypeface As New Typeface(typeface.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal)
            point.X = col1_X
            text = GetFormattedText("CODIGO", columnHeaderTypeface)
            dc.DrawText(text, point)

            text = GetFormattedText("DESCRIPCIÓN", columnHeaderTypeface)
            point.X = col2_X
            dc.DrawText(text, point)

            text = GetFormattedText("PRECIO", columnHeaderTypeface)
            point.X = col3_X
            dc.DrawText(text, point)


            ' Draw the line underneath.            
            point.Y += text.Height

            dc.DrawLine(New Pen(Brushes.Black, 2), New Windows.Point(margin, point.Y + text.Height), New Windows.Point(_pageSize.Width - margin, point.Y + text.Height))

            point.Y += text.Height

            ' Draw the column values.            
            For i As Integer = minRow To maxRow - 1

                ' Check for the end of the last (half-filled) page.                
                If i > (dt.Rows.Count - 1) Then
                    Exit For
                End If

                point.X = col1_X
                text = GetFormattedText(dt.Rows(i)("codigo").ToString())
                dc.DrawText(text, point)

                ' Add second column.                
                text = GetFormattedText(dt.Rows(i)("descripcion").ToString())
                point.X = col2_X
                dc.DrawText(text, point)
                'point.Y += text.Height

                ' Agrega la tercer columna.                
                text = GetFormattedText( _
                        String.Format("{1, 10:n2}", "", dt.Rows(i)("precio_v")))

                point.X = col3_X
                dc.DrawText(text, point)
                point.Y += text.Height


            Next
        End Using
        Return New DocumentPage(visual, PageSize, New Rect(PageSize), New Rect(PageSize))
    End Function

    Private Sub UpdateSize(headerElement As FrameworkElement, availableWidth As Integer)
        Dim vbox As New Viewbox
        vbox.Child = headerElement

        vbox.Measure(New Size(availableWidth, 2000))
        vbox.Arrange(New Rect(0, 0, availableWidth, 2000))
        vbox.UpdateLayout()

    End Sub

End Class


