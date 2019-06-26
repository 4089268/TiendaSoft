Imports System.Data.SqlClient
Imports System.Data

Public Class Frm_Cobrar

    Property imp_cobro As Decimal = 0
    Property cuentaCredito As Integer = 0

    Private pagoConCredito As Boolean = False

    Dim strCurrency As String = ""
    Dim acceptableKey As Boolean = False
    Dim xform As Frm_Ventas


    Sub New(ximp As Decimal, xf As Frm_Ventas)
        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        xform = xf
        Me.imp_cobro = ximp
        Me.txt_importe.Text = "Importe.:" + FormatCurrency(Me.imp_cobro, 2)
        Me.txt_paga.Text = "0.00"
        Me.txt_paga.SelectAll()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btn_cancelar.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub btns_click(sender As Object, e As RoutedEventArgs) Handles btn_efectivo.Click, btn_credito.Click
        Select Case sender.name
            Case "btn_efectivo"
                btn_efectivo.IsEnabled = False
                btn_credito.IsEnabled = True
                grid_Efectivo.Visibility = Windows.Visibility.Visible
                grid_Credito.Visibility = Windows.Visibility.Collapsed
                pagoConCredito = False

            Case "btn_credito"
                btn_efectivo.IsEnabled = True
                btn_credito.IsEnabled = False
                pagoConCredito = True
                grid_Efectivo.Visibility = Windows.Visibility.Collapsed
                grid_Credito.Visibility = Windows.Visibility.Visible

        End Select

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_cobrar.Click
        If pagoConCredito Then
            If CType(tb_NombreCred.Content, String).Length > 1 Then
                xform.PagoCredito = True
                xform.id_credito = CType(tb_cuenta.Text, Integer)
                imp_pagar = imp_cobro
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                MessageBox.Show("", "CUENTA NO VALIDA", MessageBoxButton.OK, MessageBoxImage.Information)
            End If
        Else
            If (imp_cobro > 0) Then
                If CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) >= CDec(Me.imp_cobro) Then
                    imp_pagar = CDec(Me.imp_cobro)
                    imp_pago = CDec(Me.txt_paga.Text)
                    imp_cambio = CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) - CDec(Me.imp_cobro)
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                Else
                    MessageBox.Show("!Error!", "La cantidad a pagar debe ser mayor o igual que el importe.", MessageBoxButton.OK, MessageBoxImage.Error)
                End If
            Else
                MessageBox.Show("!Error!", "No se a seleccionado ningun producto para cobrar", MessageBoxButton.OK, MessageBoxImage.Error)
            End If
        End If
    End Sub

    Private Sub txt_paga_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_paga.KeyDown
        If (e.Key >= Key.D0 And e.Key <= Key.D9) OrElse (e.Key = Key.Enter) OrElse (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) OrElse e.Key = Key.Back Then
            If e.Key = Key.Enter Then

                Dim request = New TraversalRequest(FocusNavigationDirection.Next)
                request.Wrapped = True
                DirectCast(sender, TextBox).MoveFocus(request)
            End If
        End If
    End Sub

    Private Sub txt_paga_LostFocus(sender As Object, e As RoutedEventArgs) Handles txt_paga.LostFocus
        If CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) >= CDec(Me.imp_cobro) Then
            Me.txt_cambio.Text = FormatCurrency(CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) - CDec(Me.imp_cobro), 2)
            'btn_cobrar.Enabled = True
            'Else
            '    'btn_cobrar.Enabled = False
            '    MessageBox.Show("!Error!, La cantidad a pagar debe ser mayor o igual que el importe.")
        End If
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.txt_paga.SelectAll()
        Me.txt_paga.Focus()
        pagoConCredito = False
    End Sub

    'validar numeros    
    Private Sub validar_numbers(sender As Object, e As TextCompositionEventArgs) Handles txt_paga.PreviewTextInput
        Try
            Dim regex As System.Text.RegularExpressions.Regex
            regex = New System.Text.RegularExpressions.Regex("[^0-9]+")
            regex = New System.Text.RegularExpressions.Regex("([^0-9,]+).([^0-9]{2}+)")
            e.Handled = regex.IsMatch(e.Text)
        Catch ex As Exception
        End Try
    End Sub

    'Mostrar modal para buscar una cuenta
    Private Sub BuscarCuenta_click() Handles btn_buscarCuenta.Click
        Me.modal.Visibility = Windows.Visibility.Visible

        Dim xm As New Cuentas_BuscarCuentas(Me)
        xm.ShowDialog()
        If xm.DialogResult = True Then
            tb_cuenta.Text = cuentaCredito
        End If
        Me.modal.Visibility = Windows.Visibility.Collapsed
    End Sub
    Private Sub BuscarCuenta() Handles tb_cuenta.TextChanged
        tb_NombreCred.Content = ""

        If tb_cuenta.Text.Length > 0 And Mi_conexion.Conectar Then
            Dim SqlComand = New SqlCommand
            SqlComand.CommandTimeout = 500
            SqlComand.CommandType = CommandType.StoredProcedure
            SqlComand.CommandText = "[Global].[Sys_PCuentas]"
            SqlComand.Parameters.Clear()
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xAlias", "BUSQUEDAID"))
            SqlComand.Parameters.Add(New SqlClient.SqlParameter("@xid_cuenta", CType(tb_cuenta.Text, Integer)))

            SqlComand.Connection = Mi_conexion.conexion

            Dim dataSet As New DataSet
            Dim DataAdapter = New SqlDataAdapter(SqlComand)
            DataAdapter.Fill(dataSet, "Resultado")

            Try
                tb_NombreCred.Content = "" & dataSet.Tables(0).Rows(0).Item("nombre")
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub validarNumeros(sender As Object, e As TextCompositionEventArgs) Handles tb_cuenta.PreviewTextInput
        Try
            Dim regex As New System.Text.RegularExpressions.Regex("[^0-9]+")
            e.Handled = regex.IsMatch(e.Text)
        Catch ex As Exception
        End Try
    End Sub

End Class
