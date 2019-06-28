Imports System.Data.SqlClient
Imports System.Data
Imports NHotkey.Wpf
Imports NHotkey

Class Frm_Ventas
    Property MyCommand As New RoutedCommand
    Property imp_cobrar As Decimal = 0
    Property PagoCredito As Boolean = False
    Property id_credito As Integer = -1

    Dim ds As DataSet = New DataSet()
    Dim strCurrency As String = ""
    Dim acceptableKey As Boolean = False
    Dim xagranel As Boolean = False
    Dim si_graba As Boolean = False

    Dim xcmd As SqlCommand
    Dim xreader As SqlDataReader

    Public Shared CustomRoutedCommand As New RoutedCommand()

    Private Sub ExecutedCustomCommand(ByVal sender As Object, ByVal e As ExecutedRoutedEventArgs)
        MessageBox.Show("Custom Command Executed")
    End Sub

    Private Sub CanExecuteCustomCommand(ByVal sender As Object, ByVal e As CanExecuteRoutedEventArgs)
        Dim target As Control = TryCast(e.Source, Control)

        If target IsNot Nothing Then
            e.CanExecute = True
        Else
            e.CanExecute = False
        End If
    End Sub

    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = "."

        HotkeyManager.Current.AddOrReplace("Buscar", Key.F1, Nothing, AddressOf btn_busqueda_Click)
        HotkeyManager.Current.AddOrReplace("BorrarProduct", Key.F6, Nothing, AddressOf btn_borrar_articulo)
        HotkeyManager.Current.AddOrReplace("SalidaDinero", Key.F7, Nothing, AddressOf btn_salidas)
        HotkeyManager.Current.AddOrReplace("EntradaDinero", Key.F8, Nothing, AddressOf btn_entradas)
        HotkeyManager.Current.AddOrReplace("Cobrar", Key.F10, Nothing, AddressOf btn_cobrar_Click)

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
        TextBoxColumn2.Width = New DataGridLength(1, DataGridLengthUnitType.Star)
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


        ''***BOTON IMPRIMIR DESACTIVADO HASTA TERMINARLO
        btn_imprimir.IsEnabled = False
        btn_imprimir.Visibility = Windows.Visibility.Collapsed

    End Sub

    Private Sub Despliega_Totales()
        Dim xart As Decimal = 0
        Dim xtot As Decimal = 0

        For i = 0 To ds.Tables(0).Rows.Count - 1
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

    '********** EVENTOS UI **********
    Private Sub txt_codigo_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txt_codigo.TextChanged
        If Mi_conexion.Conectar Then
            Dim newCodigo = ""
            txt_desc.Text = ""
            txt_pre.Text = ""
            txt_pre.Tag = ""
            txt_exis.Text = ""

            Dim listaProductos As New List(Of itemProducto)

            Dim xcod As String = TryCast(sender.text, String)
            xcmd = New SqlCommand()
            xcmd.CommandType = CommandType.Text
            xcmd.CommandText = "EXEC [Global].[Sys_Productos] @cAlias = 'BUSQUEDARAPIDA', @Descripcion = '" & xcod & "'"
            xcmd.Connection = Mi_conexion.conexion
            xcmd.Parameters.Clear()
            Try
                xreader = xcmd.ExecuteReader()
                si_graba = False
                If xreader.HasRows Then
                    Do While xreader.Read()
                        Dim prod As New itemProducto
                        prod.id_producto = CType(xreader("id_producto"), Int64)
                        prod.codigo = xreader("codigo")
                        prod.descripcion = xreader("descripcion")
                        prod.agranel = CType(xreader("agranel"), Boolean)
                        prod.precio_c = CType(xreader("precio_c"), Double)
                        prod.precio_v = CType(xreader("precio_v"), Double)
                        prod.usaInventario = CType(xreader("usaInventario"), Boolean)
                        prod.existencia = CType(xreader("existencia"), Double)
                        prod.minimo = CType(xreader("minimo"), Double)
                        prod.id_familia = CType(xreader("id_familia"), Integer)
                        prod.tipoProducto = CType(xreader("tipoProducto"), Integer)
                        listaProductos.Add(prod)
                    Loop

                    If listaProductos.Count = 1 Then
                        txt_codigo.Tag = listaProductos(0).id_producto
                        txt_desc.Text = listaProductos(0).descripcion
                        txt_pre.Text = listaProductos(0).precio_v
                        txt_pre.Tag = listaProductos(0).precio_c
                        txt_imp.Text = CDec(txt_pre.Text) * CDec(txt_cant.Text)
                        txt_exis.Text = listaProductos(0).existencia
                        xagranel = listaProductos(0).agranel
                        si_graba = True
                    Else
                        Dim frm As New Frm_seleccionarProducto(listaProductos)
                        If (frm.ShowDialog) Then
                            newCodigo = frm.nuevoCodigo
                        Else
                            txt_codigo.Text = ""
                        End If
                    End If
                Else
                    si_graba = False
                End If
                xreader.Close()
            Catch
            End Try

            If newCodigo.Length > 0 Then
                txt_codigo.Text = newCodigo
            End If
            Mi_conexion.cerrarConexion()
        End If
    End Sub
    Private Sub txt_codigo_GotFocus(sender As Object, e As RoutedEventArgs) Handles txt_codigo.GotFocus
        txt_desc.Text = ""
        txt_pre.Text = "0.00"
        txt_imp.Text = "0.00"
        txt_cant.Text = "0"
        txt_cant.IsEnabled = True
        txt_codigo.SelectAll()
    End Sub
    Private Sub txt_codigo_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_codigo.KeyDown
        If e.Key = Key.OemPlus _
            OrElse e.Key = Key.Add _
            OrElse e.Key = Key.Subtract _
            OrElse e.Key = Key.OemMinus _
            OrElse (e.Key = Key.Enter) _
            OrElse (e.Key = Key.Escape) _
            OrElse (e.Key = Key.Tab) _
            OrElse (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) _
            OrElse (e.Key >= Key.D0 And e.Key <= Key.D9) Then

            If (e.Key = Key.OemPlus Or e.Key = Key.Add) Then

                If txt_cant.Text.Length > 0 Then
                    txt_cant.Text = CStr(CDec(txt_cant.Text) + 1)
                Else
                    txt_cant.Text = "1"
                End If
                e.Handled = True
            End If

            If (e.Key = Key.OemMinus Or e.Key = Key.Subtract) Then
                If txt_cant.Text.Length > 0 Then
                    txt_cant.Text = CStr(IIf(CDec(txt_cant.Text) > 1, CDec(txt_cant.Text) - 1, CDec(txt_cant.Text)))
                Else
                    txt_cant.Text = "1"
                End If

                e.Handled = True
            End If

            If (e.Key = Key.Enter) Then
                If Not xagranel And si_graba Then
                    Me.btn_graba_Click(sender, e)
                Else
                    Dim request = New TraversalRequest(FocusNavigationDirection.Next)
                    request.Wrapped = True
                    DirectCast(sender, TextBox).MoveFocus(request)
                End If
                e.Handled = True
            End If
        Else
            e.Handled = True
        End If
    End Sub


    Private Sub btn_cobrar_Click() Handles btn_cobrar.Click

        Dim xform As New Frm_Cobrar(Me.imp_cobrar, Me)

        Dim result = xform.ShowDialog()
        If result = True And Mi_conexion.Conectar Then

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
            dts.Columns.Add(New DataColumn("tipoCobro", GetType(Integer)))
            dts.TableName = "TMP_VENTAS"
            dss.Tables.Add(dts)

            Dim dr1 As DataRow
            dr1 = dss.Tables(0).NewRow()
            dr1("fecha") = Date.Now()
            dr1("id_operador") = xOpererador
            dr1("total") = imp_pagar

            If PagoCredito Then
                dr1("pago") = imp_cobrar
                dr1("cambio") = 0
                dr1("tipoCobro") = 2
                dts.Rows.Add(dr1)
            Else
                dr1("pago") = imp_pago
                dr1("cambio") = imp_cambio
                dr1("tipoCobro") = 1
                dts.Rows.Add(dr1)
            End If

            Dim dt2 As DataTable
            dt2 = ds.Tables(0).Copy()
            dt2.TableName = "TMP_DETVENTAS"
            dss.Tables.Add(dt2)

            Dim xxml As String = dss.GetXml()
            Dim cmd As New SqlCommand()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "dbo.usp_guarda_xml"
            cmd.Connection = Mi_conexion.conexion
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@cAlias", "VENTAS")
            cmd.Parameters.AddWithValue("@cAccion", "1")
            cmd.Parameters.AddWithValue("@xmlTables", dss.GetXml())
            cmd.Parameters.AddWithValue("@xidCredito", id_credito)


            Dim sw_grabo As Boolean = True
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                sw_grabo = False
                MessageBox.Show("!Error al grabar..." + Chr(13) + ex.Message)
            End Try

            If sw_grabo Then
                ds.Tables(0).Rows.Clear()
                Me.Despliega_Totales()
            End If

            Mi_conexion.cerrarConexion()
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
        Me.Despliega_Totales()
    End Sub

    Private Sub btn_busqueda_Click() Handles btn_busqueda.Click
        modal.Visibility = Windows.Visibility.Visible
        Dim xform As New Frm_Busqueda(Me)
        Dim result = xform.ShowDialog()
        If result = True Then
        End If
        modal.Visibility = Windows.Visibility.Collapsed
    End Sub

    Private Sub btn_entradas() Handles btn_entrada.Click
        Dim xForm As New Frm_SalidaDinero(1)
        xForm.ShowDialog()
    End Sub

    Private Sub btn_salidas() Handles btn_salida.Click
        Dim xForm As New Frm_SalidaDinero(2)
        xForm.ShowDialog()
    End Sub

    Private Sub btn_enter_click(sender As Object, e As RoutedEventArgs) Handles btn_enter.Click
        If Mi_conexion.Conectar Then
            Try
                Dim xcod As String = TryCast(txt_codigo.Text, String)
                Dim cmd As New SqlCommand()

                cmd.CommandType = CommandType.Text
                cmd.CommandText = "select id_producto,codigo,descripcion,isnull(precio_v,0) as precio_v,isnull(precio_c,0) as precio_c,isnull(existencia,0) as existencia, agranel from Opr_Productos where isnull(inactivo,0)=0 and codigo='" + xcod + "'"
                cmd.Connection = Mi_conexion.conexion
                cmd.Parameters.Clear()
                Try
                    Dim reader As SqlDataReader = cmd.ExecuteReader()
                    si_graba = False
                    If reader.HasRows Then
                        Do While reader.Read()
                            txt_codigo.Tag = reader("id_producto")
                            txt_desc.Text = reader("descripcion")
                            txt_pre.Text = reader("precio_v")
                            txt_pre.Tag = reader("precio_c")
                            txt_imp.Text = CDec(txt_pre.Text) * CDec(txt_cant.Text)
                            txt_exis.Text = reader("existencia")
                            xagranel = reader("agranel")
                            si_graba = True
                        Loop
                    Else
                        si_graba = False
                    End If
                    reader.Close()
                Catch er As Exception
                End Try

                If Not xagranel And si_graba Then
                    Me.btn_graba_Click(sender, e)
                    txt_cant.IsEnabled = True
                Else
                    txt_cant.IsEnabled = True
                    Dim request = New TraversalRequest(FocusNavigationDirection.Next)
                    request.Wrapped = True
                    DirectCast(sender, TextBox).MoveFocus(request)
                End If
                e.Handled = True
            Catch ex As Exception
            Finally
                Mi_conexion.cerrarConexion()
            End Try
        End If
    End Sub

    Private Sub btn_graba_Click(sender As Object, e As RoutedEventArgs) Handles btn_graba.Click
        If si_graba And CDec(IIf(txt_pre.Text.Length > 0, txt_pre.Text, 0)) > 0 And CDec(IIf(txt_cant.Text.Length > 0, txt_cant.Text, 0)) > 0 Then

            Dim dr As DataRow

            dr = ds.Tables(0).NewRow()
            dr("id_producto") = txt_codigo.Tag
            dr("codigo") = txt_codigo.Text
            dr("descripcion") = txt_desc.Text
            dr("precio_v") = txt_pre.Text
            dr("precio_c") = txt_pre.Tag
            dr("cantidad") = txt_cant.Text
            dr("importe") = txt_pre.Text * txt_cant.Text
            dr("existencia") = txt_exis.Text

            Dim mis_rows = ds.Tables(0).Select("codigo=" + "'" + txt_codigo.Text + "'")
            If mis_rows.Count > 0 Then
                For Each rows In mis_rows
                    rows.Item("cantidad") = rows.Item("cantidad") + dr("cantidad")
                    rows.Item("importe") = rows.Item("cantidad") * rows.Item("precio_v")
                Next
            Else
                ds.Tables(0).Rows.Add(dr)
            End If

            Despliega_Totales()
            txt_codigo.Text = ""
            txt_codigo.Tag = ""
            txt_desc.Text = ""
            txt_pre.Text = ""
            txt_cant.Text = "1"
            txt_imp.Text = ""
            txt_exis.Text = ""

            grd_venta.ItemsSource = ds.Tables(0).DefaultView
            si_graba = False
            txt_codigo.Focus()
        Else
            txt_codigo.Focus()
        End If

    End Sub

    Private Sub btn_borrar_articulo() Handles btn_borrar.Click
        Try
            If (grd_venta.SelectedIndex <> -1) Then
                Dim index As Integer = grd_venta.SelectedIndex
                Dim datos As DataView = grd_venta.ItemsSource
                datos.Item(index).Delete()
                grd_venta.ItemsSource = datos
            Else
                MessageBox.Show("Seleccione un producto para removerlo", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Seleccione un producto para removerlo", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub validar_CantidadNumeros(sender As Object, e As TextCompositionEventArgs) Handles txt_cant.PreviewTextInput
        Try
            Dim regex As New System.Text.RegularExpressions.Regex("[^0-9]+")

            e.Handled = regex.IsMatch(e.Text)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub cantida_textChanged() Handles txt_cant.TextChanged
        Try
            txt_imp.Text = CDec(txt_pre.Text) * CDec(txt_cant.Text)
        Catch ex As Exception
        End Try
    End Sub

End Class