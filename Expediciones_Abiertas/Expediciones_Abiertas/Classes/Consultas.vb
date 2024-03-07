Imports MySql.Data.MySqlClient

Public Class Consultas

    Public Function expedicionesAbiertas() As DataTable
        Dim consulta As New DataSet1TableAdapters.ExpedicionesTableAdapter
        Dim dt As DataTable = consulta.GetDataAbiertos
        Return dt
    End Function

    Public Function expedicionesCerradas() As DataTable
        Dim consulta As New DataSet1TableAdapters.ExpedicionesTableAdapter
        Dim dt As DataTable = consulta.GetDataCerrados
        Return dt
    End Function

    Public Function returndatospedidoSAP(pedidoOS As String) As DataTable
        Dim consulta As New DataSet1TableAdapters.ORDRTableAdapter
        Dim dt As DataTable = consulta.GetDataByPedidoOS(pedidoOS)
        Return dt
    End Function

    Public Function getCustomerOrderNo(Pedido As String) As String
        Dim consultas As New DataSet1TableAdapters.Detalle_PedidoTableAdapter
        Dim customerorderno As String = consultas.ObtenerCustomerOrderNO(Pedido)
        Return customerorderno
    End Function

    Public Function estadoUrgente(idExpedicion) As Boolean
        Dim consulta As New DataSet1TableAdapters.ExpedicionesTableAdapter
        Dim bool As Boolean = False
        Try
            bool = consulta.GetUrgencia(idExpedicion)
        Catch ex As Exception
            bool = False
        End Try

        Return bool
    End Function

    Public Sub urgenteFalse(id As Integer)
        Dim consulta As New DataSet1TableAdapters.ExpedicionesTableAdapter
        consulta.UpdateUrgenteFalse(id)

    End Sub

    Public Sub urgenteTrue(id As Integer)
        Dim consulta As New DataSet1TableAdapters.ExpedicionesTableAdapter
        consulta.UpdateUrgenteTrue(id)

    End Sub

    Public Function returndatoslineaspedidoSAP(DocEntry As String) As DataTable
        Dim consulta As New DataSet1TableAdapters.RDR1TableAdapter
        Dim dt As DataTable = consulta.GetDataByDocEntry(DocEntry)
        Return dt
    End Function

    Public Function clienteSAP(idClienteSAP As String) As String
        Dim a As String = ""
        Dim consultas As New DataSet1TableAdapters.QueriesTableAdapter
        a = consultas.ObtenerClienteSAP(idClienteSAP)
        Return a
    End Function

    Public Function clienteSAPPorDocNum(DocNum As String) As String
        Dim a As String = ""
        Dim consultas As New DataSet1TableAdapters.QueriesTableAdapter
        a = consultas.ObtenerClientePorDocNum(DocNum)
        Return a
    End Function

    Public Function estadoExpedicion(id As Integer) As Boolean
        Dim consultas As New DataSet1TableAdapters.QueriesTableAdapter
        Dim estado As Boolean = consultas.GetEstadoExpedicion(id)
        Return estado
    End Function

    Public Function tipoBulto() As DataTable
        Dim consulta As New DataSet1TableAdapters.OPKGTableAdapter
        Dim dt As DataTable = consulta.GetDataByTipoBulto
        Return dt
    End Function

    Public Function numBultos(idexpedicion As Integer) As DataTable
        Dim consulta As New DataSet1TableAdapters.BultosExpedicionTableAdapter
        Dim dt As DataTable = consulta.GetBultosbyIdExpedicion(idexpedicion)
        Return dt
    End Function

    Public Function contenidoExpedicion(idexpedicion As Integer) As DataTable
        Dim consultas As New DataSet1TableAdapters.ContenidoExpedicionTableAdapter
        Dim dt As DataTable = consultas.GetContenidoByidExpedicion(idexpedicion)
        Return dt
    End Function

    Public Function cerrarExpedicion(idexpedicion As Integer) As Boolean
        Dim consultas As New DataSet1TableAdapters.QueriesTableAdapter
        Dim resultado As Boolean = consultas.CerrarExpedicion(Now, My.Computer.Name, idexpedicion)
        Return resultado
    End Function

    Public Function abrirExpedicion(idexpedicion As Integer) As Boolean
        Dim consultas As New DataSet1TableAdapters.QueriesTableAdapter
        Dim resultado As Boolean = consultas.AbrirExpedicion(idexpedicion)
        Return resultado
    End Function

    Public Function lineasPedido(PedidoOS As String) As DataTable

        Dim dt As New DataTable
        Dim rd As MySqlDataReader
        Dim cmd As New MySqlCommand
        Dim conn As MySqlConnection
        Dim cs As String = "Database=production_system;Data Source=192.168.14.13;" _
        & "User Id=root;Password=_Datamars.13"

        conn = New MySqlConnection(cs)
        conn.Open()

        cmd.CommandText = "SELECT        production_item.number, component_set.name, production_item.note, component_set.sap_code, production_item.quantity
                           FROM          production_item 
                           INNER JOIN    component_set 
                           ON            production_item.component_set_id = component_set.id
                           WHERE        (production_item.number LIKE '" & PedidoOS & "%')"
        cmd.Connection = conn
        rd = cmd.ExecuteReader

        dt.Rows.Clear()
        dt.Load(rd)

        Return dt

    End Function

    Public Function lineasPedidoporSAP(idLineaPedidoSAP As String) As String

        Dim dt As New DataTable
        Dim rd As MySqlDataReader
        Dim cmd As New MySqlCommand
        Dim conn As MySqlConnection
        Dim cs As String = "Database=production_system;Data Source=192.168.14.13;" _
        & "User Id=root;Password=_Datamars.13"

        conn = New MySqlConnection(cs)
        conn.Open()

        cmd.CommandText = "SELECT        production_item.number
                           FROM          production_item 
                           WHERE        (production_item.order_item_id =  '" & idLineaPedidoSAP & "')"
        cmd.Connection = conn
        rd = cmd.ExecuteReader

        dt.Rows.Clear()
        dt.Load(rd)
        Dim lineapedido As String = dt(0)(0)
        Return lineapedido

    End Function

    Public Function ImporteTotal(DocNum As String) As String
        Dim consulta As New DataSet1TableAdapters.QueriesTableAdapter
        Dim importe As String = ""
        Try
            importe = consulta.GetImporteTotalPedido(DocNum)
        Catch ex As Exception

        End Try

        Return importe
    End Function

    Public Function importePedido(DocNum As String) As String
        Dim consulta As New DataSet1TableAdapters.ImporteTotalTableAdapter
        Dim dt As DataTable = consulta.GetImporteTotalExpedicion(DocNum)
        Return dt(0)(0)
    End Function

    Public Function returnidUnificarExpedicion() As Integer
        Dim consultas As New DataSet1TableAdapters.NotificarUnionExpedicionesTableAdapter
        Dim i As Integer = consultas.UltimoValordeId
        Return i
    End Function

    Public Function InsertarUnificarExpedicion(id As Integer, origen As String, fechaNotificado As DateTime, mensaje As String, leido As Boolean, asunto As String) As Integer
        Dim consultas As New DataSet1TableAdapters.NotificarUnionExpedicionesTableAdapter
        Dim i As Integer = consultas.InsertQuery(id, origen, fechaNotificado, mensaje, leido, asunto)
        Return i
    End Function

    Public Function ExpedicionesPendientesUnificar() As Integer
        Dim consultas As New DataSet1TableAdapters.NotificarUnionExpedicionesTableAdapter
        Dim i As Integer = consultas.ExpedicionesPendientesUnificar
        Return i
    End Function

    Public Function MoverlineadePedido(idNueva As Integer, idVieja As Integer, pedido As String) As Integer
        Dim consulta As New DataSet1TableAdapters.ContenidoExpedicionTableAdapter
        Dim i As Integer = consulta.ModificarExpedicionDeUnaLinea(idNueva, idVieja, pedido)
        Return i
    End Function

    Public Function obtenerMaquina(pedido As String) As String
        Dim dt As New DataTable
        Dim rd As MySqlDataReader
        Dim cmd As New MySqlCommand
        Dim conn As MySqlConnection
        Dim cs As String = "Database=production_system;Data Source=192.168.14.13;" _
        & "User Id=root;Password=_Datamars.13"

        conn = New MySqlConnection(cs)
        conn.Open()

        cmd.CommandText = "SELECT        production_machine.name
                           FROM          production_system.production_machine INNER JOIN production_system.assignment ON production_machine.id = assignment.machine_id
                           INNER JOIN    production_system.production_item ON assignment.production_item_id = production_item.id
                           WHERE        (production_item.number =  '" & pedido & "')"
        cmd.Connection = conn
        rd = cmd.ExecuteReader

        dt.Rows.Clear()
        dt.Load(rd)
        Dim maquina As String = dt(0)(0)
        Return maquina
    End Function


End Class
