Public Class Frm_Cobrar

    Property imp_cobro As Decimal = 0

    Dim strCurrency As String = ""
    Dim acceptableKey As Boolean = False

    Sub New(ximp As Decimal)

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        Me.imp_cobro = ximp

        Me.txt_importe.Text = "Importe.:" + FormatCurrency(Me.imp_cobro, 2)
        Me.txt_paga.Text = "0.00"

        Me.txt_paga.SelectAll()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btn_cancelar.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_cobrar.Click

        If CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) >= CDec(Me.imp_cobro) Then
            'Me.txt_cambio.Text = FormatCurrency(CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) - CDec(Me.imp_cobrar), 2)
            'btn_cobrar.Enabled = True
            'Dim xcobro As New cobrar

            imp_pagar = CDec(Me.imp_cobro)
            imp_pago = CDec(Me.txt_paga.Text)
            imp_cambio = CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) - CDec(Me.imp_cobro)

            '            Dim x1, x2, x3 As Decimal
            '           x1 = CDec(Me.imp_cobro)
            '          x2 = CDec(Me.txt_paga.Text)
            '         x3 = CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) - CDec(Me.imp_cobro)

            '.imp_cobrar = x1
            'forPDV.imp_pago = x2
            'forPDV.imp_cambio = x3


            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        Else
            'btn_cobrar.Enabled = False
            MessageBox.Show("!Error!, La cantidad a pagar debe ser mayor o igual que el importe.")


            'sender.handled = False
        End If


    End Sub

    'Private Sub txt_paga_Validated(sender As Object, e As EventArgs) Handles txt_paga.Validated
    '    If CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) >= CDec(Me.imp_cobro) Then
    '        Me.txt_cambio.Text = FormatCurrency(CDec(IIf(txt_paga.Text.Length > 0, txt_paga.Text, 0)) - CDec(Me.imp_cobro), 2)
    '        'btn_cobrar.Enabled = True
    '        'Else
    '        '    'btn_cobrar.Enabled = False
    '        '    MessageBox.Show("!Error!, La cantidad a pagar debe ser mayor o igual que el importe.")
    '    End If
    'End Sub

    'Private Sub txt_paga_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txt_paga.KeyPress
    '    Acepta_NumerosConFrac(Me.txt_paga, e)
    'End Sub

    Private Sub txt_paga_KeyDown(sender As Object, e As KeyEventArgs) Handles txt_paga.KeyDown
        If (e.Key >= Key.D0 And e.Key <= Key.D9) OrElse (e.Key = Key.Enter) OrElse (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) OrElse e.Key = Key.Back Then
            If e.Key = Key.Enter Then

                Dim request = New TraversalRequest(FocusNavigationDirection.Next)
                request.Wrapped = True
                DirectCast(sender, TextBox).MoveFocus(request)


                'SendKeys.Send("{TAB}")
                'acceptableKey = False
            Else
                'acceptableKey = True
            End If
        Else
            'acceptableKey = False
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
    End Sub

    Private Sub btn_cobrar_Click(sender As Object, e As RoutedEventArgs) Handles btn_cobrar.Click




    End Sub
End Class
