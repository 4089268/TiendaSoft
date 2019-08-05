Imports System.Data.SqlClient
Imports System.Data

Class Page_inv_agregar

    Dim isOk As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(codigo As String)
        InitializeComponent()
        Me.tb_search.Text = codigo
    End Sub

    Sub buttons_click(sender As Object, e As RoutedEventArgs) Handles btn_search.Click, btn_agregar.Click
        Select Case sender.name
            Case "btn_search"
                Dim xform As New Frm_Busqueda
                If xform.ShowDialog Then
                    tb_search.Text = xform.codigoProducto
                End If

            Case "btn_agregar"
                Agregar_Inventario()
        End Select
    End Sub

    Private Sub cargar_Cantidad() Handles tb_search.TextChanged
        Try
            If (Mi_conexion.Conectar) Then
                If (tb_search.Text.Length > 0) Then
                    Dim params As New List(Of SqlParameter)
                    params.Add(New SqlParameter("@xAlias", "CANTIDADACTUAL"))
                    params.Add(New SqlParameter("@codigo", tb_search.Text))
                    Dim dataSet As New DataSet
                    Mi_conexion.Ejecutar_Procedimiento_dataAdapter("[Global].[sys_Inventario]", params).Fill(dataSet, "resultado")

                    If (dataSet.Tables(0).Rows.Count > 0) Then
                        tb_ACantidad.Text = dataSet.Tables(0).Rows(0).Item("existencia")
                        lb_descripcion.Tag = dataSet.Tables(0).Rows(0).Item("id_producto")
                        lb_descripcion.Content = dataSet.Tables(0).Rows(0).Item("descripcion")
                        tb_nCantidad.Text = "0"
                        isOk = True
                    Else
                        tb_ACantidad.Text = "0"
                        lb_descripcion.Tag = ""
                        lb_descripcion.Content = "--"
                        tb_nCantidad.Text = "0"
                        isOk = False
                    End If

                Else
                    tb_ACantidad.Text = "0"
                    lb_descripcion.Tag = ""
                    lb_descripcion.Content = "--"
                    tb_nCantidad.Text = "0"
                    isOk = False
                End If
            Else
                MessageBox.Show("Error al conectarse con la base de datos", "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Agregar_Inventario()
        Try
            If (Mi_conexion.Conectar And isOk) Then
                Dim params As New List(Of SqlParameter)
                params.Add(New SqlParameter("@xAlias", "AGREGAR"))
                params.Add(New SqlParameter("@id_operador", xOpererador))
                params.Add(New SqlParameter("@xidProducto", CType(lb_descripcion.Tag, Integer)))
                params.Add(New SqlParameter("@xCantidad", CInt(tb_nCantidad.Text)))
                Dim dataSet As New DataSet
                Mi_conexion.Ejecutar_Procedimiento_dataAdapter("[Global].[sys_Inventario]", params).Fill(dataSet, "resultado")
                MessageBox.Show(dataSet.Tables(0).Rows(0).Item("mensaje"), "", MessageBoxButton.OK, MessageBoxImage.Information)
                cargar_Cantidad()
            End If
        Catch ex As Exception
        End Try
    End Sub

    '' Validar que se tecleen numeros
    Private Sub validar_numbers(sender As Object, e As TextCompositionEventArgs) Handles tb_nCantidad.PreviewTextInput
        Try
            Dim regex As System.Text.RegularExpressions.Regex
            regex = New System.Text.RegularExpressions.Regex("[^0-9]+")

            e.Handled = regex.IsMatch(e.Text)
        Catch ex As Exception
        End Try

    End Sub

End Class