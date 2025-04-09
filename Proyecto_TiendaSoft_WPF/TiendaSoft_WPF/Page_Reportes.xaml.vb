Imports System.Collections.ObjectModel

Class Page_Reportes

    Private ventasService As VentasService
    Public Property ListaVentas As ObservableCollection(Of Venta)

    Public Sub New()
        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ventasService = New VentasService()
        datePicker1.SelectedDate = DateTime.Now
    End Sub

    Public Sub OnProcessClick(sender As Object, e As RoutedEventArgs)
        Dim targetDate As Date = datePicker1.SelectedDate
        ListaVentas = New ObservableCollection(Of Venta)(Me.ventasService.ObtenerVentas(targetDate, targetDate))
        Me.myDatagrid.ItemsSource = Me.ListaVentas
    End Sub

    Public Sub OnExportClick(sender As Object, e As RoutedEventArgs)
        MessageBox.Show("Export data click!")
    End Sub

End Class
