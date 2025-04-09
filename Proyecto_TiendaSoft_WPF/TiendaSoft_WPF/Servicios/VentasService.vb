Imports System.Data
Imports System.Data.SqlClient

Public Class VentasService

    Public Function ObtenerVentas(fecha1 As Date, fecha2 As Date) As IEnumerable(Of Venta)
        Dim responseList = New List(Of Venta)
        If (Mi_conexion.Conectar) Then
            Dim query = "SELECT * FROM [dbo].[vw_ventas] WHERE convert(varchar(8), fecha, 112) between @f1 and @f2"
            Dim sqlCommand = New SqlCommand(query, Mi_conexion.conexion)
            sqlCommand.Parameters.AddWithValue("@f1", fecha1.ToString("yyyyMMdd"))
            sqlCommand.Parameters.AddWithValue("@f2", fecha2.ToString("yyyyMMdd"))
            Dim dataReaer As SqlDataReader = sqlCommand.ExecuteReader()
            While dataReaer.Read
                responseList.Add(Venta.FromDataReader(dataReaer))
            End While
            sqlCommand.Dispose()
        Else
            Throw New Exception("No se pudo conectar con el servidor")
        End If
        Return responseList
    End Function

End Class
