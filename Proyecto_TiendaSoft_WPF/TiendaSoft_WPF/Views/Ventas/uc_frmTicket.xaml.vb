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
            If x.codigo = p.codigo Then
                x.cantidad += p.cantidad
                x.importe = x.cantidad * x.precio
                encontrado = True
            End If
        Next
        If Not encontrado Then
            listaProductos.Add(p)
        End If

        calcularTotal()
        dg_venta.ItemsSource = Nothing
        dg_venta.ItemsSource = listaProductos
    End Sub

    Public Sub calcularTotal()
        Dim totl As Double = 0
        For Each item In listaProductos
            totl += item.importe
        Next
        total = totl
        lbl_total.Content = total.ToString("$ ###0.00")
        lbl_articulos.Content = listaProductos.Count
    End Sub

    ''*************  EVENTOS UI *************
    Private Sub grd_venta_UnloadingRow(sender As Object, e As DataGridRowEventArgs) Handles dg_venta.UnloadingRow
        calcularTotal()
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

    Private Sub Dg_venta_KeyUp(sender As Object, e As KeyEventArgs) Handles dg_venta.KeyUp
        If e.Key = Key.Delete Then
            Try
                listaProductos.Remove(CType(dg_venta.SelectedItem, dataModel_ticketProduct))
                dg_venta.ItemsSource = Nothing
                dg_venta.ItemsSource = listaProductos
            Catch ex As Exception
            End Try
        End If
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
    Property esComponente As Boolean = False
End Class