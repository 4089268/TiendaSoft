Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading

Public Class Inventarios_Bajos
    Property xdt As DataTable

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub me_loadedDonde() Handles Me.Loaded
        Dim dataT As DataTable = cargandoProductosBajos()

        datagrid.ItemsSource = dataT.DefaultView
        tb_total.Text = dataT.Rows.Count
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
