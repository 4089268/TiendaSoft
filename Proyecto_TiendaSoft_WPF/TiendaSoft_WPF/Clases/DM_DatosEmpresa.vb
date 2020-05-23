Imports System.IO

Public Class DM_DatosEmpresa
    Public Property Nombre As String
    Public Property Razon_social As String
    Public Property Direccion As String
    Public Property Colonia As String
    Public Property Ciudad As String
    Public Property Estado As String
    Public Property Codigo_postal As String
    Public Property Rfc As String
    Public Property Telefono1 As String
    Public Property Telefono2 As String
    Public Property Pagina As String
    Public Property Email As String
    Public Property Firma1 As String
    Public Property Firma2 As String
    Public Property Logo As BitmapImage
    Public Property Inactivo As Boolean
    Public Sub New()
        Me.Nombre = "TIENDA_SOFT"
        Me.Razon_social = ""
        Me.Direccion = ""
        Me.Colonia = ""
        Me.Ciudad = ""
        Me.Estado = ""
        Me.Codigo_postal = ""
        Me.Rfc = ""
        Me.Telefono1 = ""
        Me.Telefono2 = ""
        Me.Pagina = ""
        Me.Email = ""
        Me.Firma1 = ""
        Me.Firma2 = ""
    End Sub

    Public Sub CargarImagen(img() As Byte)

        Dim bitmapImage As New BitmapImage
        Dim bytes As Byte() = img
        Dim ms As New System.IO.MemoryStream(bytes)
        bitmapImage.BeginInit()
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad
        bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat
        bitmapImage.StreamSource = ms
        bitmapImage.EndInit()

        Me.Logo = bitmapImage
    End Sub

End Class
