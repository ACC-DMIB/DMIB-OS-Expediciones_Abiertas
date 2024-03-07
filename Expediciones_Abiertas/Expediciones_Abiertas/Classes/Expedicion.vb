Public Class Expedicion
    Public idExpedicion As Integer
    Public idClienteSAP As String
    Public idClienteOS As String
    Public direccion As String
    Public estado As Boolean
    Public pedidos As New DataTable

    Sub New()
        Me.idExpedicion = 0
        Me.idClienteSAP = ""
        Me.idClienteOS = ""
        Me.direccion = ""
        Me.estado = False
        Me.pedidos.Columns.Add("codLineaPedido")
        Me.pedidos.Columns.Add("pedidoSAP")
    End Sub

    Sub New(idExpedicion As Integer, idClienteSAP As String, idClienteOS As String, direccion As String, estado As Boolean, listaPedidos As DataTable)
        Me.idExpedicion = idExpedicion
        Me.idClienteSAP = idClienteSAP
        Me.idClienteOS = idClienteOS
        Me.direccion = direccion
        Me.estado = estado
        Me.pedidos = listaPedidos
    End Sub
End Class
