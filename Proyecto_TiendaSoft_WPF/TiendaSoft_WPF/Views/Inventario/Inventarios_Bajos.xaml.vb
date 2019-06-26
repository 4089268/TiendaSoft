Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading

Public Class Inventarios_Bajos
    Property xdt As DataTable

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub me_loadedDonde() Handles Me.Loaded

        datagrid.AutoGenerateColumns = False
        datagrid.IsReadOnly = True

        Dim c1 As New DataGridTextColumn
        c1.Header = "PRODUCTO"
        c1.Binding = New Binding("descripcion")
        c1.Width = 300
        datagrid.Columns.Add(c1)

        Dim c2 As New DataGridTextColumn
        c2.Header = "EXISTENCIA"
        c2.Binding = New Binding("existencia")
        c2.Width = 170
        datagrid.Columns.Add(c2)

        Me.xdt = cargandoProductosBajos()

        If Me.xdt.Rows.Count > 0 Then
            datagrid.ItemsSource = xdt.DefaultView
        Else
            Me.Close()
        End If

    End Sub

    Function cargandoProductosBajos() As DataTable
        Dim xdt As New DataTable

        Dim SqlComand = New SqlCommand
        SqlComand.CommandTimeout = 500
        SqlComand.CommandType = CommandType.StoredProcedure
        SqlComand.CommandText = "[Global].[sys_Inventario]"
        SqlComand.Parameters.Clear()
        SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "BAJOS"))

        SqlComand.Connection = Mi_conexion.conexion

        Dim xds As New DataSet
        Dim DataAdapter = New SqlDataAdapter(SqlComand)
        DataAdapter.Fill(xds, "Resultado")
        Try
            xdt = xds.Tables(0)
        Catch ex As Exception
        End Try

        Return xdt

    End Function

    Private Sub me_close() Handles btn_aceotar.Click
        Me.Close()
    End Sub

End Class
