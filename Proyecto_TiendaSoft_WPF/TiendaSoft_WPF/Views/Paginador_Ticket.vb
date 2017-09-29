Imports System.Data

Public Class Paginador_Ticket
    Inherits DocumentPaginator

    Private dt As DataTable

    Private typeface As Typeface
    Private fontSize As Double
    Private margin As Double
    Private _pageSize As Size

    Private rowsPerPage As Integer
    Private _pageCount As Integer


    Public Overrides Property PageSize() As Size
        Get
            Return _pageSize
        End Get
        Set(ByVal value As Size)
            _pageSize = value
            PaginateData()
        End Set
    End Property


    Public Sub New(ByVal dt As DataTable, ByVal typeface As Typeface, ByVal fontSize As Double, ByVal margin As Double, ByVal pageSize As Size)
        Me.dt = dt
        Me.typeface = typeface
        Me.fontSize = fontSize
        Me.margin = margin
        Me._pageSize = New Size(pageSize.Width, pageSize.Height + 80)
        PaginateData()
    End Sub

    Public Sub New(ByVal typeface As Typeface, ByVal fontSize As Double, ByVal margin As Double, ByVal pageSize As Size)
        Dim xdt As New DataTable
        Dim dr As DataRow

        Dim dc1 As New DataColumn("Name", Type.GetType("System.String"))
        Dim dc2 As New DataColumn("codigo", Type.GetType("System.String"))
        Dim dc3 As New DataColumn("descripcion", Type.GetType("System.String"))
        Dim dc4 As New DataColumn("precio_v", Type.GetType("System.String"))
        Dim dc5 As New DataColumn("precio_c", Type.GetType("System.String"))
        Dim dc6 As New DataColumn("cantidad", Type.GetType("System.String"))
        Dim dc7 As New DataColumn("importe", Type.GetType("System.String"))
        Dim dc8 As New DataColumn("existencia", Type.GetType("System.String"))

        xdt.Columns.Add(dc1)
        xdt.Columns.Add(dc2)
        xdt.Columns.Add(dc3)
        xdt.Columns.Add(dc4)
        xdt.Columns.Add(dc5)
        xdt.Columns.Add(dc6)
        xdt.Columns.Add(dc7)
        xdt.Columns.Add(dc8)

        dr = xdt.NewRow
        dr("Name") = "CocaCola"
        dr("codigo") = "0000001"
        dr("descripcion") = "Cocacola1"
        dr("precio_v") = "12.50"
        dr("precio_c") = "10.50"
        dr("cantidad") = "2"
        dr("importe") = "25.00"
        dr("existencia") = "10"
        dt.Rows.Add(dr)

        dr = xdt.NewRow
        dr("Name") = "Papas Sabritas"
        dr("codigo") = "0000002"
        dr("descripcion") = "Papas Sabritas Descripcion"
        dr("precio_v") = "11.50"
        dr("precio_c") = "8.50"
        dr("cantidad") = "1"
        dr("importe") = "11.50"
        dr("existencia") = "12"
        dt.Rows.Add(dr)

        Me.dt = xdt
        Me.typeface = typeface
        Me.fontSize = fontSize
        Me.margin = margin
        Me._pageSize = New Size(pageSize.Width, pageSize.Height + 80)
        PaginateData()
    End Sub

    Private Function GetFormattedText(ByVal text As String) As FormattedText
        Return GetFormattedText(text, typeface)
    End Function

    Private Function GetFormattedText(ByVal text As String, ByVal typeface As Typeface) As FormattedText
        Return New FormattedText(text, Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, fontSize, Brushes.Black)
    End Function

    Private Sub PaginateData()
        Dim text As FormattedText = GetFormattedText("A")
        rowsPerPage = CInt((_pageSize.Height - margin * 2) / text.Height)
        rowsPerPage -= 1
        _pageCount = CInt(Math.Ceiling(CDbl(dt.Rows.Count) / rowsPerPage))
    End Sub

    Public Overrides ReadOnly Property IsPageCountValid() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property PageCount() As Integer
        Get
            Return _pageCount
        End Get
    End Property

    Public Overrides ReadOnly Property Source() As IDocumentPaginatorSource
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides Function GetPage(ByVal pageNumber As Integer) As DocumentPage

        'Create a test string for the purposes of measurement.        
        Dim text As FormattedText = GetFormattedText("A")

        Dim col1_X As Double = margin
        Dim col2_X As Double = col1_X + 95
        Dim col3_X As Double = col1_X + 95 + 300


        ' Calculate the range of rows that fits on this page.        
        Dim minRow As Integer = pageNumber * rowsPerPage
        Dim maxRow As Integer = minRow + rowsPerPage

        ' Create the visual for the page.        
        Dim visual As New DrawingVisual()

        ' Set the position to the top-left corner of the printable area.        
        Dim point As New Windows.Point(margin, margin)

        ' Print the column values.        
        Using dc As DrawingContext = visual.RenderOpen()

            Dim columnHeaderTypeface1 As New Typeface(typeface.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal)

            Dim header As New Header_Ticket
            Dim headertemplate As String = System.Windows.Markup.XamlWriter.Save(header)
            Dim headerElement As FrameworkElement = CType(System.Windows.Markup.XamlReader.Parse(headertemplate), FrameworkElement)

            UpdateSize(headerElement, 2000)

            dc.DrawRectangle(New VisualBrush(headerElement), Nothing, New Rect(point.X, point.Y, point.X + headerElement.ActualWidth, point.Y + headerElement.ActualHeight))

            point.Y += headerElement.ActualHeight

            ' Draw the column headers.            
            Dim columnHeaderTypeface As New Typeface(typeface.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal)
            point.X = col1_X
            text = GetFormattedText("CODIGO", columnHeaderTypeface)
            dc.DrawText(text, point)

            text = GetFormattedText("DESCRIPCIÓN", columnHeaderTypeface)
            point.X = col2_X
            dc.DrawText(text, point)

            text = GetFormattedText("PRECIO", columnHeaderTypeface)
            point.X = col3_X
            dc.DrawText(text, point)


            ' Draw the line underneath.            
            point.Y += text.Height

            dc.DrawLine(New Pen(Brushes.Black, 2), New Windows.Point(margin, point.Y + text.Height), New Windows.Point(_pageSize.Width - margin, point.Y + text.Height))

            point.Y += text.Height

            ' Draw the column values.            
            For i As Integer = minRow To maxRow - 1

                ' Check for the end of the last (half-filled) page.                
                If i > (dt.Rows.Count - 1) Then
                    Exit For
                End If

                point.X = col1_X
                text = GetFormattedText(dt.Rows(i)("codigo").ToString())
                dc.DrawText(text, point)

                ' Add second column.                
                text = GetFormattedText(dt.Rows(i)("descripcion").ToString())
                point.X = col2_X
                dc.DrawText(text, point)
                'point.Y += text.Height

                ' Agrega la tercer columna.                
                text = GetFormattedText( _
                        String.Format("{1, 10:n2}", "", dt.Rows(i)("precio_v")))

                point.X = col3_X
                dc.DrawText(text, point)
                point.Y += text.Height


            Next
        End Using
        Return New DocumentPage(visual, PageSize, New Rect(PageSize), New Rect(PageSize))
    End Function

    Private Sub UpdateSize(headerElement As FrameworkElement, availableWidth As Integer)
        Dim vbox As New Viewbox
        vbox.Child = headerElement

        vbox.Measure(New Size(availableWidth, 2000))
        vbox.Arrange(New Rect(0, 0, availableWidth, 2000))
        vbox.UpdateLayout()

    End Sub
End Class
