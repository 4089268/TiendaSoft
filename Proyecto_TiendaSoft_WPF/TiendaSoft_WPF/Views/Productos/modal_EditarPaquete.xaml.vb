Imports System.Data.SqlClient
Imports System.Data
Imports System.Xml
Imports System.IO

Public Class modal_EditarPaquete
    Dim dataTable As DataTable
    Property xml As String = ""

    Private Sub RootLayout_loaded() Handles rootLayout.Loaded
        CargarUI()
        CargarDatos()
    End Sub

    Private Sub CargarUI()
        datagrid1.Columns.Clear()
        datagrid1.IsReadOnly = True
        datagrid1.AutoGenerateColumns = False
        datagrid1.RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Collapsed

        Dim TextBoxColumn1 As New DataGridTextColumn
        TextBoxColumn1.Header = "producto"
        TextBoxColumn1.Binding = New Binding("descripcion")
        TextBoxColumn1.Width = New DataGridLength(250, DataGridLengthUnitType.Pixel)

        datagrid1.Columns.Add(TextBoxColumn1)

    End Sub

    Private Sub CargarDatos()
        Dim SqlComand1 = New SqlCommand
        SqlComand1.CommandTimeout = 500
        SqlComand1.CommandType = CommandType.StoredProcedure
        SqlComand1.CommandText = "[Global].[Sys_Productos]"
        SqlComand1.Parameters.Clear()
        SqlComand1.Parameters.Add(New SqlClient.SqlParameter("@cAlias", "CATALAGOCOMPONENTES"))

        SqlComand1.Connection = Mi_conexion.conexion
        Dim DataAdapter1 As New SqlDataAdapter(SqlComand1)
        Dim dataSet1 As New DataSet
        DataAdapter1.Fill(dataSet1, "Resultado")

        dataTable = dataSet1.Tables(0)
        datagrid1.ItemsSource = dataTable.DefaultView

        If (xml.Length > 0) Then
            CargarXml()
        End If
    End Sub

    Private Sub buttonsClick(sender As Object, e As Windows.RoutedEventArgs) Handles agregar.Click, limpiar.Click
        Select Case sender.name
            Case "agregar"
                Try
                    Dim row As DataRowView = datagrid1.SelectedItem
                    Dim datos As DataTable = CType(datagrid1.ItemsSource, DataView).Table.Copy
                    datos.Rows.RemoveAt(datagrid1.SelectedIndex)
                    datagrid1.ItemsSource = datos.DefaultView

                    Dim control As New UserControl1
                    control.nombre = row.Item("descripcion").ToString
                    control.codigo = row.Item("codigo")
                    control.update_ui()
                    datosStackPanel.Children.Add(control)

                Catch ex As Exception
                End Try
            Case "limpiar"
                datagrid1.ItemsSource = dataTable.DefaultView
                datosStackPanel.Children.Clear()
        End Select
    End Sub

    Private Sub guardar_Click(sender As Object, e As RoutedEventArgs) Handles guardar.Click
        If datosStackPanel.Children.Count > 0 Then
            Dim xmlGenerado As String = "<paquete>"

            For Each row As UserControl1 In datosStackPanel.Children
                xmlGenerado = xmlGenerado & "<ingrediente><codigo>" & row.codigo & "</codigo><cantidad>" & row.cantidad & "</cantidad></ingrediente>"
            Next
            xmlGenerado = xmlGenerado & "</paquete>"
            xml = xmlGenerado
            Me.DialogResult = True
        Else
            Me.DialogResult = False
        End If
    End Sub

    Private Sub cancelar_Click(sender As Object, e As RoutedEventArgs) Handles cancelar.Click
        DialogResult = False
    End Sub

    Private Sub CargarXml()
        Dim listaProductos As New List(Of productoPaquete)
        Using reader As XmlReader = XmlReader.Create(New StringReader(xml))
            Try
                While (reader.Read())
                    If (reader.NodeType = XmlNodeType.Element And reader.Name = "ingrediente") Then
                        Dim producto As New productoPaquete
                        Dim x As Boolean = True
                        While x
                            reader.Read()
                            If (reader.NodeType = XmlNodeType.Element And reader.Name = "codigo") Then
                                producto.codigo = reader.ReadElementContentAsString()
                            End If

                            If (reader.NodeType = XmlNodeType.Element And reader.Name = "cantidad") Then
                                producto.cantidad = CInt(reader.ReadElementContentAsString())
                                x = False
                            End If
                        End While
                        listaProductos.Add(producto)
                    End If
                End While
            Catch ex As Exception
            End Try
        End Using

        Dim datos As DataTable = CType(datagrid1.ItemsSource, DataView).Table.Copy
        Dim indexAEliminar As New List(Of Int32)

        For Each x As productoPaquete In listaProductos
            Dim nombre As String = ""
            Dim index As Integer = 0

            For Each row In datos.Rows
                If (row.item("codigo").ToString = x.codigo) Then
                    nombre = row.item("descripcion")
                    indexAEliminar.Add(index)
                End If
                index += 1
            Next

            Dim control As New UserControl1
            control.nombre = nombre
            control.codigo = x.codigo
            control.cantidad = x.cantidad
            control.update_ui()
            datosStackPanel.Children.Add(control)
        Next

        indexAEliminar = OrdernarListaMayorMenor(indexAEliminar)
        For Each index As Integer In indexAEliminar
            datos.Rows.RemoveAt(index)
        Next
        datagrid1.ItemsSource = datos.DefaultView
    End Sub

    Private Function OrdernarListaMayorMenor(lista As List(Of Integer)) As List(Of Integer)
        For i As Integer = 0 To (lista.Count - 2)
            For j As Integer = i + 1 To (lista.Count - 1)
                Dim c As Integer = 0
                If (lista(i) < lista(j)) Then
                    c = lista(j)
                    lista(j) = lista(i)
                    lista(i) = c
                End If
            Next j
        Next i
        Return lista
    End Function
End Class
