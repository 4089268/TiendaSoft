Imports System.Data
Imports System.Data.SqlClient
Imports NHotkey.Wpf

Public Class uc_frmTicket
    Property total As Double = 0
    Property index As Integer = 0
    Property xFrmVentas As Frm_Ventas = Nothing
    Private listaProductos As New List(Of dataModel_ticketProduct)

    Public Sub me_loaded_done() Handles Me.Loaded
        HotkeyManager.Current.AddOrReplace("Nuevoticket", Key.F10, Nothing, AddressOf btn_cobrar_Click)
    End Sub

    Public Sub agregarProducto(p As dataModel_ticketProduct)
        Dim encontrado As Boolean = False
        For Each x In listaProductos
            If x.codigo = p.codigo And Not x.esComponente Then
                x.cantidad += p.cantidad
                x.importe = x.cantidad * x.precio
                encontrado = True
            End If
        Next
        If Not encontrado Then
            listaProductos.Add(p)
            If p.tipoProducto = 2 Then
                Try
                    If Mi_conexion.Conectar Then
                        Dim params As New List(Of SqlParameter)
                        params.Add(New SqlParameter("@cAlias", "OBTENERCOMPONENTES"))
                        params.Add(New SqlParameter("@codigo", p.codigo))

                        Using dReader As SqlDataReader = Mi_conexion.Ejecutar_Procedimiento_dataReader("[Global].[Sys_Productos]", params)
                            While dReader.Read
                                Dim x As New dataModel_ticketProduct
                                x.idProducto = dReader.Item("id_producto")
                                x.codigo = dReader.Item("codigo")
                                x.descripción = "   |__ " & dReader.Item("descripcion")
                                x.precio_c = dReader.Item("precio_c")
                                x.precio = dReader.Item("precio_v")
                                x.cantidad = CType(dReader.Item("cantidadxPaquete"), Integer) * p.cantidad
                                x.existencia = dReader.Item("existencia")
                                x.tipoProducto = dReader.Item("tipoProducto")
                                x.importe = CType(dReader.Item("precio_v"), Integer) * x.cantidad
                                x.esComponente = True
                                x.codigoPaquete = dReader.Item("codigoPaquete")
                                listaProductos.Add(x)
                            End While
                        End Using
                        Mi_conexion.cerrarConexion()
                    End If
                Catch ex As Exception
                    MessageBox.Show("", "Error al cargar los componentes del paquete.", MessageBoxButton.OK, MessageBoxImage.Error)
                End Try
            End If
        Else
            If p.tipoProducto = 2 Then
                Try
                    If Mi_conexion.Conectar Then
                        Dim params As New List(Of SqlParameter)
                        params.Add(New SqlParameter("@cAlias", "OBTENERCOMPONENTES"))
                        params.Add(New SqlParameter("@codigo", p.codigo))

                        Using dReader As SqlDataReader = Mi_conexion.Ejecutar_Procedimiento_dataReader("[Global].[Sys_Productos]", params)
                            While dReader.Read
                                For Each x In listaProductos
                                    If x.codigo = dReader.Item("codigo") Then
                                        x.cantidad += CType(dReader.Item("cantidadxPaquete"), Integer) * p.cantidad
                                        x.importe = CType(dReader.Item("precio_v"), Integer) * x.cantidad
                                    End If
                                Next
                            End While
                        End Using
                        Mi_conexion.cerrarConexion()
                    End If
                Catch ex As Exception
                    MessageBox.Show("", "Error al cargar los componentes del paquete.", MessageBoxButton.OK, MessageBoxImage.Error)
                End Try
            End If
        End If

        calcularTotal()
        dg_venta.ItemsSource = Nothing
        dg_venta.ItemsSource = listaProductos
    End Sub

    Public Sub calcularTotal()
        Dim xcount As Integer = 0
        Dim totl As Double = 0
        For Each item In listaProductos
            If Not item.esComponente Then
                totl += item.importe
                xcount += item.cantidad
            End If
        Next
        total = totl
        lbl_total.Content = total.ToString("$ ###0.00")
        lbl_articulos.Content = xcount
    End Sub

    ''*************  EVENTOS UI *************
    Private Sub grd_venta_UnloadingRow(sender As Object, e As DataGridRowEventArgs) Handles dg_venta.UnloadingRow
        calcularTotal()
    End Sub
    Private Sub dg_venta_rowLoaded(sender As Object, e As DataGridRowEventArgs) Handles dg_venta.LoadingRow
        If e.Row.Item.esComponente Then
            e.Row.Style = CType(FindResource("componente"), Style)
        End If
    End Sub

    Private Sub btn_cobrar_Click() Handles btn_cobrar.Click
        If Me.IsLoaded Then
            Dim xform As New Frm_Cobrar(total)
            If xform.ShowDialog Then
                If Mi_conexion.Conectar Then
                    Dim xid_prod As Integer = 0
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

                    If xform.pagoConCredito Then
                        dr1("pago") = xform.imp_cobro
                        dr1("cambio") = 0
                        dr1("tipoCobro") = 2
                        dts.Rows.Add(dr1)
                    Else
                        dr1("pago") = imp_pago
                        dr1("cambio") = imp_cambio
                        dr1("tipoCobro") = 1
                        dts.Rows.Add(dr1)
                    End If

                    Dim dt2 As New DataTable
                    dt2.Columns.Add(New DataColumn("id_producto", GetType(Integer)))
                    dt2.Columns.Add(New DataColumn("codigo", GetType(String)))
                    dt2.Columns.Add(New DataColumn("descripcion", GetType(String)))
                    dt2.Columns.Add(New DataColumn("precio_v", GetType(Decimal)))
                    dt2.Columns.Add(New DataColumn("precio_c", GetType(Decimal)))
                    dt2.Columns.Add(New DataColumn("cantidad", GetType(Decimal)))
                    dt2.Columns.Add(New DataColumn("importe", GetType(Decimal)))
                    dt2.Columns.Add(New DataColumn("existencia", GetType(Decimal)))
                    For Each product In listaProductos
                        If Not product.esComponente Then
                            Dim xrow = dt2.NewRow
                            xrow.Item("id_producto") = product.idProducto
                            xrow.Item("codigo") = product.codigo
                            xrow.Item("descripcion") = product.descripción
                            xrow.Item("precio_v") = product.precio
                            xrow.Item("precio_c") = product.precio_c
                            xrow.Item("cantidad") = product.cantidad
                            xrow.Item("importe") = product.importe
                            xrow.Item("existencia") = product.existencia
                            dt2.Rows.Add(xrow)
                        End If
                    Next
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
                    cmd.Parameters.AddWithValue("@xidCredito", xform.idCuentaCredito)

                    Try
                        cmd.ExecuteNonQuery()
                        cerrarTicket()
                    Catch ex As Exception
                        MessageBox.Show("!Error al grabar..." + Chr(13) + ex.Message)
                    End Try
                    Mi_conexion.cerrarConexion()
                End If
            End If
        End If
    End Sub

    Private Sub cerrarTicket() Handles btn_cancelar.Click
        If Not IsNothing(xFrmVentas) Then
            xFrmVentas.btn_cerrarTicketClick(index)
        End If
    End Sub

    ''** BORRAR PRODUCTO **
    Private Sub Dg_venta_KeyUp(sender As Object, e As KeyEventArgs) Handles dg_venta.KeyUp
        Try
            If e.Key = Key.Delete And Not CType(dg_venta.SelectedItem, dataModel_ticketProduct).esComponente Then
                If CType(dg_venta.SelectedItem, dataModel_ticketProduct).tipoProducto = 2 Then
                    Dim tmp_list As New List(Of dataModel_ticketProduct)
                    tmp_list = listaProductos.ToList
                    For Each xitem In tmp_list
                        If xitem.codigoPaquete = CType(dg_venta.SelectedItem, dataModel_ticketProduct).codigo Then
                            listaProductos.Remove(xitem)
                        End If
                    Next

                End If
                listaProductos.Remove(CType(dg_venta.SelectedItem, dataModel_ticketProduct))
                dg_venta.ItemsSource = Nothing
                dg_venta.ItemsSource = listaProductos
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class

Public Class dataModel_ticketProduct
    Property idProducto As Integer = 0
    Property codigo As String = ""
    Property descripción As String = ""
    Property precio_c As Double = 0
    Property precio As Double = 0
    Property cantidad As Integer = 0
    Property importe As Double = 0
    Property existencia As Integer = 0
    Property tipoProducto As Integer = 0
    Property esComponente As Boolean = False
    Property codigoPaquete As String = ""
End Class