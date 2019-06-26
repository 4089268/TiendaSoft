﻿Imports System.Data.SqlClient
Imports System.Data

Public Class Frm_Busqueda

    Dim xBusqueda As New List(Of Cls_Busqueda)
    Dim xform As Frm_Ventas
    Dim xform2 As Page_inv_agregar

    Dim desde_frmVentas As Boolean = False
    Dim textboxDestino As TextBox = Nothing
    
    Sub New(xform As Frm_Ventas)
        InitializeComponent()
        Me.xform = xform
        desde_frmVentas = True
    End Sub

    Sub New(textbox As TextBox)
        InitializeComponent()
        textboxDestino = textbox
    End Sub

    Private Sub Frm_busqueda_Loaded(sender As Object, e As RoutedEventArgs) Handles MyBase.Loaded, MyBase.Loaded

        grd_productos.AutoGenerateColumns = False
        grd_productos.Columns.Clear()
        grd_productos.IsReadOnly = False
        grd_productos.CanUserAddRows = False


        Dim TextBoxColumn1 As New DataGridTextColumn
        TextBoxColumn1.Header = "Código"
        TextBoxColumn1.Binding = New Binding("codigo")
        TextBoxColumn1.Width = 120
        TextBoxColumn1.IsReadOnly = True

        Dim TextBoxColumn2 As New DataGridTextColumn
        TextBoxColumn2.Header = "Descripción"
        TextBoxColumn2.Binding = New Binding("descripcion")
        TextBoxColumn2.Width = New DataGridLength(1, DataGridLengthUnitType.Star) 'ajustable
        TextBoxColumn2.IsReadOnly = True

        Dim TextBoxColumn3 As New DataGridTextColumn
        TextBoxColumn3.Header = "Precio"
        TextBoxColumn3.Width = 80
        TextBoxColumn3.Binding = New Binding("precio_v")

        TextBoxColumn3.IsReadOnly = True


        Dim TextBoxColumn4 As New DataGridTextColumn
        TextBoxColumn4.Header = "Existencia"
        TextBoxColumn4.Width = 80
        TextBoxColumn4.Binding = New Binding("existencia")
        TextBoxColumn4.IsReadOnly = True


        grd_productos.Columns.Add(TextBoxColumn1)
        grd_productos.Columns.Add(TextBoxColumn2)
        grd_productos.Columns.Add(TextBoxColumn3)
        grd_productos.Columns.Add(TextBoxColumn4)





    End Sub

    Private Sub btn_buscar_Click(sender As Object, e As RoutedEventArgs) Handles btn_buscar.Click
        Dim cmd As New SqlCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "select codigo,descripcion,isnull(precio_v,0) as precio_v,isnull(existencia,0) as existencia from Opr_Productos where isnull(inactivo,0)=0 and descripcion like '%" + Me.txt_prod.Text + "%'"
        cmd.Connection = Mi_conexion.conexion
        cmd.Parameters.Clear()

        xBusqueda.Clear()
        Dim reader As SqlDataReader = cmd.ExecuteReader()
        Try
            If reader.HasRows Then
                While reader.Read()
                    Dim xRec As New Cls_Busqueda

                    xRec.codigo = IIf(IsDBNull(reader("codigo")), "", reader("codigo"))
                    xRec.descripcion = IIf(IsDBNull(reader("descripcion")), "", reader("descripcion"))
                    xRec.precio_v = IIf(IsDBNull(reader("precio_v")), 0, reader("precio_v"))
                    xRec.existencia = IIf(IsDBNull(reader("existencia")), 0, reader("existencia"))

                    xBusqueda.Add(xRec)
                End While
            End If

        Finally
            reader.Close()
            grd_productos.ItemsSource = xBusqueda.ToList

        End Try



    End Sub

    Private Sub grd_productos_KeyDown(sender As Object, e As KeyEventArgs) Handles grd_productos.KeyDown
        'If e.Key = Key.Enter Then 'MessageBox.Show("presione enter")'End If
    End Sub

    Private Sub grd_productos_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles grd_productos.MouseDoubleClick
        Try
            If (desde_frmVentas) Then
                Dim xvalor As Cls_Busqueda = grd_productos.SelectedItem
                xform.txt_codigo.Text = xvalor.codigo
                xform.txt_codigo.SelectAll()
                xform.txt_codigo.Focus()
                Me.Close()
            Else
                Dim xvalor As Cls_Busqueda = grd_productos.SelectedItem
                textboxDestino.Text = xvalor.codigo
                Me.Close()
            End If

            
        Catch ex As Exception
        End Try

    End Sub

    ''PRESIONAR ENTER PARA BUSCAR
    Private Sub txt_prod_enterClick(sender As Object, e As KeyEventArgs) Handles txt_prod.KeyUp
        If (e.Key = Key.Enter) Then
            btn_buscar_Click(sender, e)
        End If
    End Sub


    ''CARGAR LA IMAGEN CUANDO SE SELECIONA UN PRODUCTO
    Private Sub datagrid_selectionChanged() Handles grd_productos.SelectionChanged
        Try

            Dim codigo As String = grd_productos.SelectedItem.codigo

            Dim dataSet As New DataSet
            Mi_conexion.Ejecutar_Procedimiento_dataset("[Global].[sys_cargarImagen]", {"codigo"}, {codigo}).Fill(dataSet, "Resultado")

            ''CargarImagen
            Try
                Dim bitmapImage As New BitmapImage
                Dim bytes As Byte() = CType(dataSet.Tables(0).Rows(0).Item("foto1"), Byte())
                Dim ms As New System.IO.MemoryStream(bytes)
                bitmapImage.BeginInit()
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad
                bitmapImage.StreamSource = ms
                bitmapImage.EndInit()
                img1.Source = bitmapImage

            Catch ex As Exception
                img1.Source = New BitmapImage
            End Try

        Catch ex As Exception
            MessageBox.Show("ALGO SALIO MAL", "ERR", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try


    End Sub

End Class

Public Class Cls_Busqueda
    Public Property codigo As String
    Public Property descripcion As String
    Public Property precio_v As Decimal
    Public Property existencia As Decimal
End Class

