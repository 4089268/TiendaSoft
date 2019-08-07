Public Class UserControl1
    Public Property cantidad As Integer = 1
    Public Property nombre As String = ""
    Public Property id_producto As String = ""

    Public Sub update_ui()
        xNombre.Content = nombre
        xCant.Value = cantidad
    End Sub

    Private Sub cantidad_upadte() Handles xCant.EditValueChanged
        cantidad = CInt(xCant.Value)
    End Sub
End Class
