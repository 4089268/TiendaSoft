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
                    Dim SqlComand = New SqlCommand
                    SqlComand.CommandTimeout = 500
                    SqlComand.CommandType = CommandType.StoredProcedure
                    SqlComand.CommandText = "[Global].[sys_Inventario]"
                    SqlComand.Parameters.Clear()
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "CANTIDADACTUAL"))
                    SqlComand.Parameters.Add(New SqlClient.SqlParameter("@codigo", tb_search.Text))

                    SqlComand.Connection = Mi_conexion.conexion
                    Dim DataAdapter As New SqlDataAdapter(SqlComand)
                    Dim dataSet As New DataSet
                    DataAdapter.Fill(dataSet, "Resultado")

                    If (dataSet.Tables(0).Rows.Count > 0) Then
                        tb_ACantidad.Text = dataSet.Tables(0).Rows(0).Item("existencia")
                        lb_descripcion.Content = dataSet.Tables(0).Rows(0).Item("descripcion")
                        tb_nCantidad.Text = "0"
                        isOk = True
                    Else
                        tb_ACantidad.Text = "0"
                        lb_descripcion.Content = "--"
                        tb_nCantidad.Text = "0"
                        isOk = False
                    End If

                Else
                    tb_ACantidad.Text = "0"
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
                Dim SqlComand = New SqlCommand
                SqlComand.CommandTimeout = 500
                SqlComand.CommandType = CommandType.StoredProcedure
                SqlComand.CommandText = "[Global].[sys_Inventario]"
                SqlComand.Parameters.Clear()
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "AGREGAR"))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@id_operador", xOpererador))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@codigo", tb_search.Text))
                SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xCantidad", CInt(tb_nCantidad.Text)))


                SqlComand.Connection = Mi_conexion.conexion
                Dim DataAdapter As New SqlDataAdapter(SqlComand)
                Dim dataSet As New DataSet
                DataAdapter.Fill(dataSet, "Resultado")

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
