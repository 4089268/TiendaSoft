
Class Page_Configuracion

    Dim navService As NavigationService
    Dim xMainWindow As MainWindow
    Public Sub New(xm As MainWindow)
        InitializeComponent()
        Me.xMainWindow = xm
    End Sub

    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        navService = Me.NavigationService
    End Sub

    Sub buttonsClick(sender As Object, e As RoutedEventArgs) Handles btn_cajeros.Click, btn_Opciones.Click, btn_ticket.Click, btn_datosEmp.Click, btn_respaldos.Click
        Select Case sender.name
            Case "btn_cajeros"
                navService.Navigate(New Page_conf_cajeros(xMainWindow))

            Case "btn_Opciones"
                pruebaImpresion()

            Case "btn_ticket"
                navService.Source = New Uri("Views/Configuracion/Page_conf_ticket.xaml", UriKind.Relative)

            Case "btn_datosEmp"
                navService.Navigate(New Page_conf_DatosEmpreza(xMainWindow))

            Case "btn_respaldos"
                navService.Navigate(New Page_conf_BaseDatos(xMainWindow))

        End Select

    End Sub

    Private Sub pruebaImpresion()

        'Dim dataTable As New System.Data.DataTable

        'dataTable.Columns.Add("codigo", GetType(String))
        'dataTable.Columns.Add("Descripcion", GetType(String))
        'dataTable.Columns.Add("Precio", GetType(String))

        'dataTable.Rows.Add("000001", "CocaCola", "$12.02")
        'dataTable.Rows.Add("000002", "PepsiCola", "$13.02")
        'dataTable.Rows.Add("000003", "1Kg Tortillas", "$17.00")
        'dataTable.Rows.Add("000004", "Tomates", "$20.00")

        'Dim columnWidths As New List(Of Double)
        'columnWidths.Add(200)
        'columnWidths.Add(250)
        'columnWidths.Add(80)


        'Dim PrintControlViewModel As IPrintControlViewModel
        'PrintControlViewModel = PrintControlFactory.Create(dataTable, columnWidths)
        'PrintControlViewModel.ShowPrintPreview()

    End Sub

End Class

