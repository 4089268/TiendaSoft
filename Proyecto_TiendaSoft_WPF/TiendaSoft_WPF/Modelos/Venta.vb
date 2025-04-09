Imports System
Imports System.Data

Public Class Venta

    Public Property IdVenta As Integer
    Public Property Fecha As Date
    Public Property IdOperador As Integer
    Public Property Operador As String
    Public Property IdEstasus As Integer
    Public Property Total As Decimal
    Public Property Pago As Decimal
    Public Property Cambio As Decimal
    Public Property IdSession As Integer
    Public Property IdTipoCobro As Integer
    Public Property TipoCobro As String

    Shared Function FromDataReader(reader As IDataReader) As Venta
        Dim newObject As New Venta With {
            .IdVenta = If(IsDBNull(reader("id_venta")), 0, Convert.ToInt32(reader("id_venta"))),
            .Fecha = If(IsDBNull(reader("fecha")), Date.MinValue, Convert.ToDateTime(reader("fecha"))),
            .IdOperador = If(IsDBNull(reader("id_operador")), 0, Convert.ToInt32(reader("id_operador"))),
            .Operador = If(IsDBNull(reader("operador")), "", Convert.ToString(reader("operador"))),
            .IdEstasus = If(IsDBNull(reader("id_estatus")), 0, Convert.ToInt32(reader("id_estatus"))),
            .Total = If(IsDBNull(reader("total")), 0D, Convert.ToDecimal(reader("total"))),
            .Pago = If(IsDBNull(reader("pago")), 0D, Convert.ToDecimal(reader("pago"))),
            .Cambio = If(IsDBNull(reader("cambio")), 0D, Convert.ToDecimal(reader("cambio"))),
            .IdSession = If(IsDBNull(reader("id_sesion")), 0, Convert.ToInt32(reader("id_sesion"))),
            .IdTipoCobro = If(IsDBNull(reader("idTipoCobro")), 0, Convert.ToInt32(reader("idTipoCobro"))),
            .TipoCobro = If(IsDBNull(reader("tipoCobro")), "", Convert.ToString(reader("tipoCobro")))
        }
        Return newObject
    End Function

End Class
