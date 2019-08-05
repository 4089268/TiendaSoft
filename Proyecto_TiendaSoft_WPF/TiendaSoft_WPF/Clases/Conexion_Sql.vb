Imports System.Data.SqlClient
Imports System.Data

Public Class Conexion_Sql
    Property _conectionString As String = ""
    Property conexion As SqlConnection

    Public Sub New()
        _conectionString = "Server=" & My.Settings.Server & ";Database=" & strBasedeDatos & ";User Id=" & My.Settings.user & "; Password=" & My.Settings.password & ";"
    End Sub

    Function Conectar() As Boolean
        Try
            conexion = New SqlConnection(_conectionString)
            conexion.Open()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Sub cerrarConexion()
        Try
            conexion.Close()
            conexion = Nothing
        Catch ex As Exception
        End Try
    End Sub

    Function Ejecutar_Procedimiento(procedureName As String, param As String(), valores As String()) As SqlDataReader
        Try
            If (param.Length <> valores.Length) Then
                MessageBox.Show("Arreglos de Parametros y valores tienen diferentes tamano", "Alert", MessageBoxButton.OK, MessageBoxImage.Information)
            End If

            Dim xSqlCommand = New SqlCommand
            xSqlCommand.CommandTimeout = 500
            xSqlCommand.CommandType = CommandType.StoredProcedure
            xSqlCommand.CommandText = procedureName
            xSqlCommand.Parameters.Clear()
            Dim i As Integer = 0
            For Each parm As String In param
                xSqlCommand.Parameters.Add(New SqlClient.SqlParameter("@" & parm, valores(i)))
                i = i + 1
            Next
            xSqlCommand.Connection = conexion
            Dim reader As SqlDataReader = xSqlCommand.ExecuteReader()
            Return reader
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Console.WriteLine(ex)
            Return Nothing
        End Try
    End Function

    Function Ejecutar_Procedimiento_dataAdapter(procedureName As String, param As String(), valores As String()) As SqlDataAdapter
        If (param.Length <> valores.Length) Then
            MessageBox.Show("Arreglos de Parametros y valores tienen diferentes tamano", "Alert", MessageBoxButton.OK, MessageBoxImage.Information)
        End If

        Dim xSqlCommand = New SqlCommand
        xSqlCommand.CommandTimeout = 500
        xSqlCommand.CommandType = CommandType.StoredProcedure
        xSqlCommand.CommandText = procedureName
        xSqlCommand.Parameters.Clear()
        Dim i As Integer = 0
        For Each parm As String In param
            xSqlCommand.Parameters.Add(New SqlClient.SqlParameter("@" & parm, valores(i)))
            i = i + 1
        Next
        xSqlCommand.Connection = conexion
        Dim dataAdapter As New SqlDataAdapter(xSqlCommand)
        Return dataAdapter
    End Function

    Function Ejecutar_Procedimiento_dataAdapter(procedureName As String, params As List(Of SqlParameter)) As SqlDataAdapter
        Dim xSqlCommand = New SqlCommand
        xSqlCommand.CommandTimeout = 500
        xSqlCommand.CommandType = CommandType.StoredProcedure
        xSqlCommand.CommandText = procedureName
        xSqlCommand.Parameters.Clear()

        For Each parm As SqlParameter In params
            xSqlCommand.Parameters.Add(parm)
        Next
        xSqlCommand.Connection = conexion
        Dim dataAdapter As New SqlDataAdapter(xSqlCommand)
        Return dataAdapter
    End Function

    Function Ejecutar_Procedimiento_dataReader(procedureName As String, params As List(Of SqlParameter)) As SqlDataReader
        Dim xsqlReader As SqlDataReader = Nothing
        Try
            Dim xSqlCommand = New SqlCommand
            xSqlCommand.CommandTimeout = 1000
            xSqlCommand.CommandType = CommandType.StoredProcedure
            xSqlCommand.CommandText = procedureName
            xSqlCommand.Parameters.Clear()
            For Each sqlp In params
                xSqlCommand.Parameters.Add(sqlp)
            Next
            xSqlCommand.Connection = conexion
            xsqlReader = xSqlCommand.ExecuteReader

        Catch ex As Exception
            MessageBox.Show("Algo salio mal al ejecutar el procedimiento almacenado :(")
        End Try
        Return xsqlReader
    End Function

    Function Ejecutar_query(xqu As String) As DataTable
        Dim xSqlCommand = New SqlCommand
        xSqlCommand.CommandTimeout = 500
        xSqlCommand.CommandText = xqu
        xSqlCommand.Connection = conexion
        Dim reader As SqlDataReader = xSqlCommand.ExecuteReader()

        Dim xdataTable As New DataTable
        xdataTable.Load(reader)
        Return xdataTable

    End Function

End Class
