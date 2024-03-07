Imports RestSharp
Public Class AñadirExpediciones

    Dim queries As New Consultas
    Private Sub nudLineas_ValueChanged(sender As Object, e As EventArgs) Handles nudLineas.ValueChanged
        While dgvExpedicion.Rows.Count <> nudLineas.Value
            If dgvExpedicion.Rows.Count < nudLineas.Value Then
                dgvExpedicion.Rows.Add()
            ElseIf dgvExpedicion.Rows.Count > nudLineas.Value Then
                dgvExpedicion.Rows.RemoveAt(dgvExpedicion.Rows.Count - 1)
            End If

        End While
    End Sub

    Private Sub btnAñadirExpedicion_Click(sender As Object, e As EventArgs) Handles btnAñadirExpedicion.Click
        Dim expedicion As New Expedicion
        Dim dt As DataTable = dgvExpedicion.DataSource


        For Each row As DataGridViewRow In dgvExpedicion.Rows
            Dim pedido As String = row.Cells(0).Value
            expedicion.idClienteOS = returnidOS(pedido)
            Dim dtORDR As DataTable = queries.returndatospedidoSAP(pedido)
            Dim docEntry As String = dtORDR(0)("DocEntry")
            Dim dtRDR1 As DataTable = queries.returndatoslineaspedidoSAP(docEntry)
            expedicion.direccion = dtORDR(0)("ShipToCode")
            expedicion.idClienteSAP = dtORDR(0)("CardCode")

            For Each fila As DataRow In dtRDR1.Rows
                Dim r As DataRow = expedicion.pedidos.NewRow
                r("codLineaPedido") = queries.lineasPedidoporSAP(fila("U_IDORDERITEM"))
                r("pedidoSAP") = dtORDR(0)("DocNum")
                expedicion.pedidos.Rows.Add(r)
            Next

            'Faltaria añadir la parte de insertar los datos en la bbdd

        Next

    End Sub

    Private Function returnidOS(pedidoOSS As String) As String
        getToken()
        Dim client As New RestClient("https://api.datamars.com/ordering/v1/orders?number=$eq(" & pedidoOSS & ")&subsidiary.id=00000000-0000-0000-0000-000000000006")
        Dim request As New RestRequest(Method.GET)
        request.AddHeader("Authorization", "bearer " & myToken.access_token & "")
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
        Dim response As IRestResponse = client.Execute(request)
        Dim pedidoOS As PedidoOS = Newtonsoft.Json.JsonConvert.DeserializeObject(Of PedidoOS)(response.Content).content(0)

        Dim id As String = pedidoOS.customer.id
        Return id

    End Function

    Private Sub AñadirExpediciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class