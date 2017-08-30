Public Class UserControl1
    Public Property cantidad As Int16 = 1
    Public Property nombre As String = ""
    Public Property codigo As Int64 = 0

    Public Sub update_ui()
        xNombre.Content = nombre
        xCant.Value = cantidad
    End Sub

    Private Sub cantidad_upadte() Handles xCant.EditValueChanged
        Try
            cantidad = CInt(xCant.Value)
        Catch ex As Exception
        End Try
    End Sub

End Class
