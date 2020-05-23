Imports System.ComponentModel
Imports System.Data.SqlClient

Public Class Iniciando
    Dim worker As BackgroundWorker

    Public Sub New()
        InitializeComponent()

        worker = New BackgroundWorker
        worker.WorkerReportsProgress = True
        AddHandler worker.DoWork, AddressOf worker_work
        AddHandler worker.ProgressChanged, AddressOf worker_progressChanged

    End Sub

    Private Sub me_loaded_done() Handles Me.Loaded
        lblVersion.Text = xversion
        worker.RunWorkerAsync()
    End Sub
    Private Sub ComprobarConexion(conectado As Boolean)

        If (Not conectado) Then

            '****** MOSTRAR FORMULARIO CONFIGURAR CONEXION ******
            Me.Visibility = Visibility.Collapsed

            Dim conexionForm As New Frm_Conexion
            Dim xres = conexionForm.ShowDialog()
            If (xres) Then
                Mi_conexion = New Conexion_Sql
                Mi_conexion.Conectar()

                '****** CARGAR DATOS DE LA EMPRESA ******
                Dim params As New List(Of SqlParameter)
                params.Add(New SqlParameter("@xAlias", "OBTENER"))
                Using reader As SqlDataReader = Mi_conexion.Ejecutar_Procedimiento_dataReader("[Global].[sp_datos_empresa]", params)
                    If reader.Read Then
                        DatosEmpresa.Nombre = reader("nombre_comercial").ToString
                        DatosEmpresa.Ciudad = reader("ciudad").ToString
                        DatosEmpresa.Codigo_postal = reader("codigo_postal").ToString
                        DatosEmpresa.Colonia = reader("colonia").ToString
                        DatosEmpresa.Direccion = reader("direccion").ToString
                        DatosEmpresa.Estado = reader("estado").ToString
                        DatosEmpresa.Rfc = reader("rfc").ToString
                        DatosEmpresa.Telefono1 = reader("telefono1").ToString
                        Try
                            Dim datosImagen = CType(reader("logo"), Byte())
                            DatosEmpresa.CargarImagen(datosImagen)
                        Catch ex As Exception
                        End Try

                    End If
                End Using

                '****** MOSTRAR FOMULARIO LOGIN ******
                Dim xl As New Form_LogIn
                xl.Show()
            End If

        Else
            Mi_conexion = New Conexion_Sql
            Mi_conexion.Conectar()

            '****** CARGAR DATOS DE LA EMPRESA ******
            Dim params As New List(Of SqlParameter)
            params.Add(New SqlParameter("@xAlias", "OBTENER"))
            Using reader As SqlDataReader = Mi_conexion.Ejecutar_Procedimiento_dataReader("[Global].[sp_datos_empresa]", params)
                If reader.Read Then
                    DatosEmpresa.Nombre = reader("nombre_comercial").ToString
                    DatosEmpresa.Ciudad = reader("ciudad").ToString
                    DatosEmpresa.Codigo_postal = reader("codigo_postal").ToString
                    DatosEmpresa.Colonia = reader("colonia").ToString
                    DatosEmpresa.Direccion = reader("direccion").ToString
                    DatosEmpresa.Estado = reader("estado").ToString
                    DatosEmpresa.Rfc = reader("rfc").ToString
                    DatosEmpresa.Telefono1 = reader("telefono1").ToString
                    Try
                        Dim datosImagen = CType(reader("logo"), Byte())
                        DatosEmpresa.CargarImagen(datosImagen)
                    Catch ex As Exception
                    End Try

                End If
            End Using

            '****** MOSTRAR FOMULARIO LOGIN ******
            Dim xl As New Form_LogIn
            xl.Show()
        End If
        Me.Close()

    End Sub

    '********* Eventos BackGroundWorker *********
    Private Sub worker_work(sender As Object, e As DoWorkEventArgs)
        System.Threading.Thread.Sleep(2000)
        Dim conexion As New Conexion_Sql
        conexion.cerrarConexion()
        Dim ok = conexion.Conectar
        CType(sender, BackgroundWorker).ReportProgress(1, ok)
    End Sub
    Private Sub worker_progressChanged(sender As Object, e As ProgressChangedEventArgs)
        ComprobarConexion(CType(e.UserState, Boolean))
    End Sub
End Class
