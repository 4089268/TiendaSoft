Imports System
Imports System.Data
Imports System.IO
Imports System.Windows.Forms
Imports System.Collections.ObjectModel
Imports OfficeOpenXml

Class Page_Reportes

    Private ventasService As VentasService
    Public Property ListaVentas As ObservableCollection(Of Venta)
    Public Property FechaSeleccionada As Date = DateTime.Now

    Public Sub New()
        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()
        Me.DataContext = Me
        ventasService = New VentasService()
    End Sub

    Public Sub OnProcessClick(sender As Object, e As RoutedEventArgs)
        ListaVentas = New ObservableCollection(Of Venta)(Me.ventasService.ObtenerVentas(FechaSeleccionada, FechaSeleccionada))
        Me.myDatagrid.ItemsSource = Me.ListaVentas
    End Sub

    Public Sub OnExportClick(sender As Object, e As RoutedEventArgs)
        If IsNothing(ListaVentas) Then
            MessageBox.Show("No hay elementos para exportar")
            Return
        End If

        If Not ListaVentas.Any Then
            MessageBox.Show("No hay elementos para exportar")
            Return
        End If

        Try
            Dim filePath As String = Path.GetTempPath

            ' ask for the path to save the file
            Using dialog As New FolderBrowserDialog()
                If dialog.ShowDialog() <> System.Windows.Forms.DialogResult.OK Then
                    dialog.Dispose()
                    Return
                End If

                ' prepare the file path
                Dim selectedPath As String = dialog.SelectedPath
                Dim fileName = String.Format("ventas-{0}-{1}", FechaSeleccionada.ToString("yyyyMMdd"), DateTime.Now.Ticks)
                filePath = $"{dialog.SelectedPath}/{fileName}.xlsx"

                ' Prepare data
                Dim table As New DataTable("ventas")
                table.Columns.Add("Num Venta", GetType(Integer))
                table.Columns.Add("Fecha", GetType(String))
                table.Columns.Add("Hora", GetType(String))
                table.Columns.Add("Operador", GetType(String))
                table.Columns.Add("Estasus", GetType(Integer))
                table.Columns.Add("Total", GetType(Decimal))
                table.Columns.Add("Pago", GetType(Decimal))
                table.Columns.Add("Cambio", GetType(Decimal))
                table.Columns.Add("Num Session", GetType(Integer))
                table.Columns.Add("TipoCobro", GetType(String))

                For Each venta In ListaVentas
                    table.Rows.Add(
                        venta.IdVenta,
                        venta.Fecha.ToString("yyyy-MM-dd"),
                        venta.Fecha.ToString("HH:mm:ss"),
                        venta.Operador,
                        venta.IdEstasus,
                        venta.Total,
                        venta.Pago,
                        venta.Cambio,
                        venta.IdSession,
                         venta.TipoCobro
                    )
                Next

                ' make the file
                Using package As New ExcelPackage(New FileInfo(filePath))
                    Dim worksheet = package.Workbook.Worksheets.Add("ventas")
                    worksheet.Cells("A1").LoadFromDataTable(table, True, OfficeOpenXml.Table.TableStyles.Medium7)
                    package.Save()
                End Using
            End Using

            ' open the saved file
            If File.Exists(filePath) Then
                Process.Start(filePath)
            End If

        Catch ex As Exception
            MessageBox.Show(Me, $"Ocurrió un error durante la exportación: {ex.Message}", "Error", MessageBoxIcon.Error)
        End Try
    End Sub

End Class
