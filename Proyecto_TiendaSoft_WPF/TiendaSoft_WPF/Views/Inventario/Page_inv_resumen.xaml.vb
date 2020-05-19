Imports System.Data
Imports System.Data.SqlClient

Class Page_inv_resumen
    Private formularioPadre As New Page_Inventario

    Public Sub New(x As Page_Inventario)
        InitializeComponent()
        Me.formularioPadre = x
    End Sub
    Private Sub MeLoadedDone() Handles Me.Loaded
        CargarDatos()
    End Sub

    Private Sub CargarDatos()
        Dim conexion As New Conexion_Sql
        If conexion.Conectar Then

            Dim datosTabla As DataTable = conexion.Ejecutar_query("exec [Global].[Sys_inventario] @xAlias = 'TODOS'")

            lbl_total.Text = "TOTAL PRODUCTOS:" & datosTabla.Rows.Count.ToString
            dg_productos.ItemsSource = datosTabla.DefaultView
        End If
    End Sub

    '*********** EVENTOS UI *********** 
    Private Sub BotonClick(sender As Object, e As RoutedEventArgs) Handles btn_agregar.MouseLeftButtonUp, btn_ajutar.MouseLeftButtonUp, btn_movimientos.MouseLeftButtonUp
        Try

            Dim itemSeleccionado As DataRowView = dg_productos.SelectedItem

            Select Case CType(sender, Border).Name
                Case "btn_agregar"
                    formularioPadre.navigationframe.Navigate(New Page_inv_agregar(itemSeleccionado("codigo").ToString))
                Case "btn_ajutar"
                    formularioPadre.navigationframe.Navigate(New Page_inv_remplazar(itemSeleccionado("codigo").ToString))
                Case "btn_movimientos"
                    formularioPadre.navigationframe.Navigate(New Page_inv_movimientos(itemSeleccionado("codigo").ToString))
            End Select

        Catch ex As Exception
            MessageBox.Show("hola")
        End Try

    End Sub

    Private Sub tb_search_search(sender As Object, e As KeyEventArgs) Handles tb_search.KeyDown
        Try
            If (e.Key = Key.Enter) Then
                If (tb_search.SearchText <> "") Then
                    Dim dataview As DataView = dg_productos.ItemsSource

                    Dim regex As New System.Text.RegularExpressions.Regex("[^0-9]+")
                    If (regex.IsMatch(tb_search.SearchText)) Then
                        dataview.RowFilter = String.Format("descripcion like '%{0}%'", tb_search.SearchText)
            Else
                        dataview.RowFilter = String.Format("codigo = '{0}'", tb_search.SearchText)
                    End If

                    dg_productos.ItemsSource = dataview
                    lbl_total.Text = "TOTAL PRODUCTOS:" & dataview.Table.Rows.Count
                Else
                        CargarDatos()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("error")
        End Try
    End Sub
End Class
