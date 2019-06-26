Imports System.Data
Imports System.Data.SqlClient

Class Page_inv_bajos
    Private Sub rootLayout_loaded() Handles rootLayout.Loaded
        Cargar_Datos()
    End Sub

    Private Sub Cargar_Datos()
        Try
            If (Mi_conexion.Conectar) Then
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[sys_Inventario]"
                SqlComand.Parameters.Clear()
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "BAJOS"))

                SqlComand.Connection = Mi_conexion.conexion
                Dim DataAdapter As New SqlDataAdapter(SqlComand)
                Dim dataSet As New DataSet
                DataAdapter.Fill(dataSet, "Resultado")


                For Each row As DataRow In dataSet.Tables(0).Rows
                    agregar_fila(row.Item("codigo").ToString,
                                    row.Item("descripcion").ToString,
                                    row.Item("precio_v").ToString,
                                    row.Item("existencia").ToString,
                                    row.Item("minimo").ToString,
                                    row.Item("familia").ToString)
                Next


            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub agregar_fila(codigo As String, descripcion As String, precio As String, existencia As String, minimo As String, familia As String)

        Dim myStackPanel As New StackPanel
        myStackPanel.Orientation = Orientation.Horizontal
        myStackPanel.Margin = New Thickness(2, 5, 0, 2)

        myStackPanel.Children.Add(create_label(codigo, 220))
        myStackPanel.Children.Add(create_label(descripcion, 340))
        myStackPanel.Children.Add(create_label(precio, 150))
        myStackPanel.Children.Add(create_label(existencia, 150))
        myStackPanel.Children.Add(create_label(minimo, 150))
        myStackPanel.Children.Add(create_label(familia, 200))

        sp_datos.Children.Add(myStackPanel)

    End Sub

    Private Function create_label(contenido As String, ancho As Integer) As textbox

        Dim mylabel As New TextBox
        mylabel.FontSize = 14
        mylabel.FontWeight = FontWeights.Medium
        mylabel.Text = contenido
        mylabel.HorizontalAlignment = Windows.HorizontalAlignment.Left
        mylabel.HorizontalContentAlignment = Windows.HorizontalAlignment.Left
        mylabel.VerticalAlignment = Windows.VerticalAlignment.Center
        mylabel.VerticalContentAlignment = Windows.VerticalAlignment.Center
        mylabel.Width = ancho
        mylabel.IsReadOnly = True
        mylabel.BorderThickness = New Thickness(0, 0, 0, 0)
        Return mylabel

    End Function

End Class
