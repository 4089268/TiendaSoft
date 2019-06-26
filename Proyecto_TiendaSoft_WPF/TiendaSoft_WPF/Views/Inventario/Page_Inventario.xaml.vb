﻿Imports System.Data.SqlClient
Imports System.Data

Class Page_Inventario

    Private _mainWindow As MainWindow


    Private Sub rootLayout() Handles rotLayout.Loaded
        navigationframe.Navigate(New Page_inv_agregar)
    End Sub

    Private Sub btn_clicks(sender As Object, e As RoutedEventArgs) Handles btn_agregarInv.Click, btn_remplazarInv.Click, btn_bajosInv.Click, btn_MovimientoInv.Click, btn_ReporteInv.Click
        actividadButons()

        CType(sender, Button).IsEnabled = True

        Select Case sender.name
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
        End Select

    End Sub

    Private Sub actividadButons()
        btn_agregarinv.IsEnabled = True
    End Sub

    '' Abre la opcion de productos
    Sub editarProdcuto(p1 As String)
        _mainWindow.editarProducto(p1)
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(main As MainWindow)
        InitializeComponent()
        _mainWindow = main
    End Sub

End Class
