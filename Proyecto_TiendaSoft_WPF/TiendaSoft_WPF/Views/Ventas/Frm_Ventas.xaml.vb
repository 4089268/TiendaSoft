Imports System.Data.SqlClient
Imports System.Data
Imports NHotkey.Wpf
Imports NHotkey

Class Frm_Ventas
    Property MyCommand As New RoutedCommand
    Property imp_cobrar As Decimal = 0
    Property PagoCredito As Boolean = False
    Property id_credito As Integer = -1

    'Dim ds As DataSet = New DataSet()

    Dim strCurrency As String = ""
    Dim acceptableKey As Boolean = False
    Dim xagranel As Boolean = False
    Dim si_graba As Boolean = False

    Dim xcmd As SqlCommand
    Dim xreader As SqlDataReader
    Dim frm_busqueda As Frm_Busqueda

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
        HotkeyManager.Current.AddOrReplace("SalidaDinero", Key.F7, Nothing, AddressOf btn_salidas)
        HotkeyManager.Current.AddOrReplace("EntradaDinero", Key.F8, Nothing, AddressOf btn_entradas)
        btn_nuevoTicketClick()

        ''***BOTON IMPRIMIR DESACTIVADO HASTA TERMINARLO
        btn_imprimir.IsEnabled = False
        btn_imprimir.Visibility = Windows.Visibility.Collapsed

    End Sub

    Private Sub limpiar_Campos()
        txt_desc.Text = ""
        txt_pre.Text = "0.00"
        txt_imp.Text = "0.00"
        txt_exis.Text = ""

        If txt_cant.Text.Length < 1 Then
            txt_cant.Text = "1"
        ElseIf txt_cant.Text = "0" Then
            txt_cant.Text = "1"
        End If

        txt_cant.IsEnabled = True
    End Sub

    Private Sub renombrarTickets()
        For i As Integer = 0 To (tc_tickets.Items.Count - 2)
            Dim xtab As TabItem = CType(tc_tickets.Items(i), TabItem)
            xtab.Header = "Ticket " & (i + 1)
            CType(xtab.Content, uc_frmTicket).index = i
        Next
    End Sub

    '********** EVENTOS UI **********
    Private Sub txt_codigo_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txt_codigo.TextChanged
        limpiar_Campos()

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
                If newCodigo = txt_codigo.Text Then
                    txt_codigo.Tag = listaProductos(0).id_producto
                    txt_desc.Text = listaProductos(0).descripcion
                    txt_pre.Text = listaProductos(0).precio_v
                    txt_pre.Tag = listaProductos(0).precio_c
                    txt_imp.Text = CDec(txt_pre.Text) * CDec(txt_cant.Text)
                    txt_exis.Text = listaProductos(0).existencia
                    xagranel = listaProductos(0).agranel
                    si_graba = True
                Else
                    txt_codigo.Text = newCodigo
                End If
            End If
            Mi_conexion.cerrarConexion()
        End If
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
    Private Sub cantida_textChanged() Handles txt_cant.ValueChanged
        Try
            txt_imp.Text = CDec(txt_pre.Text) * CDec(txt_cant.Text)
        Catch ex As Exception
        End Try
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
                            txt_imp.Text = (CDec(txt_pre.Text) * CDec(txt_cant.Text)).ToString("####0.00")
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

    Private Sub btn_nuevoTicketClick() Handles btn_nuevo.MouseLeftButtonUp
        If tc_tickets.Items.Count < 11 Then
            Dim x As New TabItem
            Dim frmTicket As New uc_frmTicket
            frmTicket.xFrmVentas = Me
            x.Header = "Ticket #"
            x.Content = frmTicket
            x.FontWeight = FontWeights.Medium
            tc_tickets.Items.Insert((tc_tickets.Items.Count - 1), x)
            renombrarTickets()
            tc_tickets.SelectedIndex = (tc_tickets.Items.Count - 2)
        End If
        tc_tickets.SelectedIndex = (tc_tickets.Items.Count - 2)
    End Sub
    Public Sub btn_cerrarTicketClick(i As Integer)
        tc_tickets.Items.RemoveAt(i)
        If tc_tickets.Items.Count <= 1 Then
            btn_nuevoTicketClick()
        Else
            tc_tickets.SelectedIndex = 0
        End If
        renombrarTickets()
    End Sub

    Private Sub btn_graba_Click(sender As Object, e As RoutedEventArgs) Handles btn_graba.Click
        Dim ok As Boolean = True
        If CDec(IIf(txt_pre.Text.Length > 0, txt_pre.Text, 0)) <= 0 And CDec(IIf(txt_cant.Text.Length > 0, txt_cant.Text, 0)) <= 0 Then
            ok = False
        End If

        If si_graba And ok And tc_tickets.SelectedIndex < (tc_tickets.Items.Count - 1) Then
            Dim itemTicket As New dataModel_ticketProduct
            itemTicket.idProducto = CType(txt_codigo.Tag, Integer)
            itemTicket.codigo = txt_codigo.Text
            itemTicket.descripción = txt_desc.Text
            itemTicket.precio = CType(txt_pre.Text, Double)
            itemTicket.cantidad = CType(txt_cant.Text, Integer)
            itemTicket.importe = (itemTicket.precio * itemTicket.cantidad)

            itemTicket.precio_c = CType(txt_pre.Tag, Double)
            itemTicket.existencia = CType(txt_exis.Text, Integer)

            CType(CType(tc_tickets.SelectedItem, TabItem).Content, uc_frmTicket).agregarProducto(itemTicket)

            limpiar_Campos()
            txt_codigo.Text = ""
            txt_codigo.Tag = ""
            txt_cant.Text = "1"
            si_graba = False
            txt_codigo.Focus()
        Else
            txt_codigo.Focus()
        End If
    End Sub

    Private Sub btn_busqueda_Click() Handles btn_busqueda.Click
        If IsNothing(frm_busqueda) Then
            modal.Visibility = Windows.Visibility.Visible
            frm_busqueda = New Frm_Busqueda
            If (frm_busqueda.ShowDialog()) Then
                Dim cp As String = frm_busqueda.codigoProducto
                txt_codigo.Text = cp
            End If
            modal.Visibility = Windows.Visibility.Collapsed
            frm_busqueda = Nothing
            txt_codigo.Focus()
        End If
    End Sub
    Private Sub btn_imprimir_Click(sender As Object, e As RoutedEventArgs) Handles btn_imprimir.Click
        'Dim printDialog As New PrintDialog()
        'If printDialog.ShowDialog() = True Then
        '    Dim paginator As New Paginador_Ticket(ds.Tables(0), New Typeface("Calibri"), 16, 40, New Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight))

        '    printDialog.PrintDocument(paginator, "Ticket")
        'End If
    End Sub
    Private Sub btn_entradas() Handles btn_entrada.Click
        If Me.IsLoaded Then
            Dim xForm As New Frm_SalidaDinero(1)
            xForm.ShowDialog()
        End If
    End Sub
    Private Sub btn_salidas() Handles btn_salida.Click
        If Me.IsLoaded Then
            Dim xForm As New Frm_SalidaDinero(2)
            xForm.ShowDialog()
        End If
    End Sub

End Class