Public Class PedidoOS
    Public Property id As String
    Public Property number As String
    Public Property status As String

    Public Property customer As BusinessPartner

    Public Property content As List(Of PedidoOS)
End Class
