Imports System.Data.SqlClient

Module MiModulo
    Public Const strApp As String = "POINT"
    Public Const strBasedeDatos As String = "PDV"
    Public xversion As String = "2.1.32"
    Public DatosEmpresa As New DM_DatosEmpresa

    Public xOpererador As Int32 = 0
    Public xNombreUsuario As String = ""
    Public xOpciones As String = ""
    Public xAdmin As Boolean = False

    Public Mi_conexion As Conexion_Sql

#Region " Variables Ventas"
    Public imp_cambio As Double = 0
    Public imp_pagar As Double = 0
    Public imp_pago As Double = 0
#End Region

End Module
