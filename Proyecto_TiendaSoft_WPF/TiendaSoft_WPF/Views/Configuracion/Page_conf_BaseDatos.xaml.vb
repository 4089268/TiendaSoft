Imports System.Data

Class Page_conf_BaseDatos
    Dim navService As NavigationService
    Dim xMainWindow As MainWindow
    Dim datosConfiguracion As DataTable
    Dim rutaNueva = ""

    Public Sub New(m As MainWindow)
        InitializeComponent()
        Me.xMainWindow = m
        CargarDatos()
        Me.DataContext = datosConfiguracion
    End Sub

    Sub rootlayout_onLoaded() Handles rootLayout.Loaded
        navService = Me.NavigationService
    End Sub
    Private Sub CargarDatos()
        If (Mi_conexion.Conectar) Then
            Try
                Dim q As String = "SELECT valor as RUTA_RESPALDO FROM [Global].[opr_configuracion] WHERE parametro = 'RUTA_RESPALDO' "
                datosConfiguracion = Mi_conexion.Ejecutar_query(q)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
                BotonRegresar_Click()
            End Try
        Else
            MessageBox.Show("No se pudo conectar a la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
        End If

    End Sub
    Private Sub GuardarConfiguracion()
        If (Mi_conexion.Conectar) Then
            Try
                Dim xquery As String = String.Format("UPDATE [Global].[opr_configuracion] SET valor = '{0}' WHERE parametro = 'RUTA_RESPALDO' ", rutaNueva)
                Mi_conexion.Ejecutar_query(xquery)
                MessageBox.Show("Configuracion Guardada", "", MessageBoxButton.OK, MessageBoxImage.Information)
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        Else
            MessageBox.Show("No se pudo conectar a la base de datos", "", MessageBoxButton.OK, MessageBoxImage.Error)
        End If

    End Sub

    '********** EVENTOS UI **********
    Private Sub BotonRegresar_Click() Handles btn_regresar.Click
        navService.Navigate(New Page_Configuracion(xMainWindow))
    End Sub
    Private Sub Boton_Ruta_Click() Handles btn_rutaRespaldo.Click

        Dim FolderDialog As New System.Windows.Forms.FolderBrowserDialog
        If FolderDialog.ShowDialog = Forms.DialogResult.OK Then
            Me.tb_rutaRes.Text = FolderDialog.SelectedPath & "\"
        End If

    End Sub

    Private Sub Boton_GuardarCambios_Click() Handles btn_Guardar.MouseLeftButtonUp

        '*** Validar si la ruta existe
        If tb_rutaRes.Text.Length > 2 Then
            Dim tmpRuta As String = tb_rutaRes.Text
            If tmpRuta.ToArray(tmpRuta.ToArray.Count - 1) <> "\" Then
                tmpRuta = tmpRuta & "\"
            End If
            rutaNueva = tmpRuta
            Dim xf As New System.IO.DirectoryInfo(rutaNueva)

            If Not xf.Exists Then
                Dim res = MessageBox.Show("La ruta no existe o no es accesible, ¿Desea guardar los cambios de todas formas?.", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error)
                If (res = MessageBoxResult.Yes) Then
                    GuardarConfiguracion()
                End If
                tb_rutaRes.Focus()
                Return
            End If

            GuardarConfiguracion()
        Else
            tb_rutaRes.Focus()
        End If

    End Sub
End Class
