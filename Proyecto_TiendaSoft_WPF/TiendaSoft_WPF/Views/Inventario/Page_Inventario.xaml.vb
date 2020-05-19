Imports System.Data.SqlClient
Imports System.Data

Class Page_Inventario

    Private _mainWindow As MainWindow

    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(main As MainWindow)
        InitializeComponent()
        _mainWindow = main
    End Sub

    Private Sub rootLayout() Handles rotLayout.Loaded
        'navigationframe.Navigate(New Page_inv_agregar)
        'btn_agregarInv.IsEnabled = False
        navigationframe.Navigate(New Page_inv_reporteInv(Me))
        btn_ReporteInv.IsEnabled = False
        'navigationframe.Navigate(New Page_inv_resumen(Me))
        'btn_resumen.IsEnabled = False
    End Sub
    Private Sub actividadButons()
        btn_agregarInv.IsEnabled = True
        btn_bajaInv.IsEnabled = True
        btn_remplazarInv.IsEnabled = True
        btn_bajosInv.IsEnabled = True
        btn_ReporteInv.IsEnabled = True
        btn_MovimientoInv.IsEnabled = True
        btn_resumen.IsEnabled = True
    End Sub

    '********** EVENTOS UI **********
    Private Sub btn_clicks(sender As Object, e As RoutedEventArgs) Handles btn_agregarInv.Click, btn_remplazarInv.Click, btn_bajosInv.Click, btn_MovimientoInv.Click, btn_ReporteInv.Click, btn_bajaInv.Click, btn_resumen.Click
        actividadButons()
        CType(sender, Button).IsEnabled = False
        Select Case sender.name
            Case "btn_bajaInv"
                navigationframe.Navigate(New Page_inv_darBaja)

            Case "btn_agregarInv"
                navigationframe.Navigate(New Page_inv_agregar)

            Case "btn_remplazarInv"
                navigationframe.Navigate(New Page_inv_remplazar)

            Case "btn_bajosInv"
                navigationframe.Navigate(New Page_inv_bajos)

            Case "btn_ReporteInv"
                navigationframe.Navigate(New Page_inv_reporteInv(Me))

            Case "btn_MovimientoInv"
                navigationframe.Navigate(New Page_inv_movimientos)

            Case "btn_resumen"
                navigationframe.Navigate(New Page_inv_resumen(Me))

        End Select

    End Sub

    '' Abre la opcion de productos
    Sub editarProdcuto(p1 As String)
        _mainWindow.editarProducto(p1)
    End Sub

End Class
