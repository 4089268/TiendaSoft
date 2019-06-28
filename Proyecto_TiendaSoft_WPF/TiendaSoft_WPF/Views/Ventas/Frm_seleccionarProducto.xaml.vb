Public Class Frm_seleccionarProducto
    Dim lstProductos As New List(Of itemProducto)
    Dim productoSeleccionado As itemProducto = Nothing
    Property nuevoCodigo As String = ""

    Public Sub New(xlista As List(Of itemProducto))
        InitializeComponent()
        Me.lstProductos = xlista
    End Sub

    Private Sub me_loaded_done() Handles Me.Loaded
        dg_productos.ItemsSource = lstProductos
    End Sub

    '********* EVENTOS UI '*********
    Private Sub dg_productos_rowSelection(sender As Object, e As RoutedEventArgs) Handles dg_productos.SelectionChanged
        productoSeleccionado = CType(dg_productos.SelectedItem, itemProducto)
    End Sub

    Private Sub btn_seleccionar() Handles btn_select.Click, dg_productos.MouseDoubleClick
        If (Not IsNothing(productoSeleccionado)) Then
            nuevoCodigo = productoSeleccionado.codigo
            Me.DialogResult = True
        End If
    End Sub

    Private Sub btn_cancelar() Handles bt_cancelar.Click
        Me.DialogResult = False
    End Sub

End Class
