Imports RestSharp
Module networkCommands

    Public urlDEV_MAIN As String
    Public urlASSIGNMENT As String = "/tracking/v1/assignments"
    Public urlASSIGNMENT_OPTION As String = "/tracking/v1/assignment_options_query"
    Public urlASSIGNMENT_REQUEST As String = "/tracking/v1/assignment_requests"
    Public urlTRANSPONDER_TYPE As String = "/tracking/v1/transponder_types"
    Public urlTRANSPONDER_TECH As String = "/tracking/v1/transponder_technologies"
    Public urlTRANSPONDER_APP As String = "/tracking/v1/transponder_applications"
    Public urlTRACKING As String = "/tracking/v1/trackings"
    Public urlUIDINFO As String = "/tracking/v1/unique_identifiers"
    Public urlWORKORDER As String = "/tracking/v1/work_orders"
    Public urlSALEORDER As String = "/tracking/v1/sales_orders"
    Public urlCONTAINNER As String = "/tracking/v1/containers"
    Public urlTRANSPONDER As String = "/tracking/v1/transponders"

    Public urlMANUFACTURER_CODE As String = "/tracking/v1/manufacturer_codes"
    Public urlPARTNER As String = "/tracking/v1/partners"
    Public urlMACHINE As String = "/tracking/v1/machines"
    Public urlPRODUCT As String = "/tracking/v1/products"

    Public myToken As Token

    Public Sub getToken()

        Dim client As New RestClient("https://login.microsoftonline.com/5d7ef17f-affc-45f4-a3ce-3379dfb2c5ef/oauth2/token")
        Dim request As New RestRequest(Method.POST)
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded")
        request.AddHeader("Cache-Control", "no-cache")


        'Dim Client_ID As String = "4ea45c95-ae13-4139-bd26-0a119fe20fc6"


        'TEST
        Dim client_ID As String = "8c6b25ce-6e8d-4912-9d1a-a96d5c2a8b3a"
        Dim Client_Secret As String = "LFuZLe%2BV4VxmexdHTN0T1sXfegWHkxv31wQeQbHbUsA%3D" '"lIyNaga5OXxN1Bvnc8BxkURkDtrgZrnoYpHxApIR5tk="

        'Try
        '    If TRACKING_SYSTEM___Uploader.chkLive.Checked Then
        '        myToken = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Token)(IO.File.ReadAllText(My.Application.Info.DirectoryPath & "\myTokenLive.json"))
        '    Else
        '        myToken = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Token)(IO.File.ReadAllText(My.Application.Info.DirectoryPath & "\myTokenTest.json"))
        '    End If

        'Catch ex As Exception
        '    myToken = Nothing
        'End Try


        'If Not myToken Is Nothing Then
        '    Dim expiry As Date = New Date(1970, 1, 1, 0, 0, 0).AddSeconds(Int64.Parse(myToken.expires_on))
        '    If expiry.AddMinutes(-20) < Now.ToUniversalTime Then
        '        Exit Sub
        '    End If

        'End If



        'LIVE
        client = New RestClient("https://login.microsoftonline.com/5d7ef17f-affc-45f4-a3ce-3379dfb2c5ef/oauth2/token")

        client_ID = "4ca5738a-a27a-4052-b833-50a77e3606fe"
        Client_Secret = "1vwHhm3Roqua4Z1RgpOoPs1mHOegUpbp3BR0SXwPOsc="

        request.AddParameter("undefined", "grant_type=client_credentials&client_id=" & client_ID & "&client_secret=" & Client_Secret & "&resource=https%3A%2F%2Fgraph.windows.net%2F", ParameterType.RequestBody)

        Dim response As IRestResponse = client.Execute(request)
        'txt_token.Text = response.Content

        myToken = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Token)(response.Content)

        myToken.expiration_date = New Date(1970, 1, 1, 0, 0, 0).AddSeconds(Int64.Parse(myToken.expires_on)).ToLocalTime



    End Sub

    Public Function sendWebRequest(address As String) As IRestResponse

        Dim client As New RestClient(address)

        Dim request As New RestRequest(Method.GET)
        request.AddHeader("Authorization", "Bearer " & myToken.access_token & "")
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
        Dim response As IRestResponse = client.Execute(request)

        Return response


    End Function


    Public Function sendJSONfile(address As String, path As String) As IRestResponse

        Dim JSON As String
        Dim jsonFile As String
        Dim client As RestClient

        jsonFile = path
        JSON = IO.File.ReadAllText(path)
        client = New RestClient(address)

        Dim request As New RestRequest(Method.POST)
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12

        request.AddHeader("Content-Type", "application/json")
        request.AddHeader("Authorization", "Bearer " & myToken.access_token & "")
        request.AddHeader("Accept", "application/json")
        request.AddParameter("application/json", JSON, ParameterType.RequestBody)

        Dim response As IRestResponse = client.Execute(request)
        Return response

    End Function

    Public Function sendJSONString(address As String, json As String) As IRestResponse

        Dim client As RestClient

        client = New RestClient(address)

        Dim request As New RestRequest(Method.POST)
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12

        request.AddHeader("Content-Type", "application/json")
        request.AddHeader("Authorization", "Bearer " & myToken.access_token & "")
        request.AddHeader("Accept", "application/json")
        request.AddParameter("application/json", json, ParameterType.RequestBody)

        Dim response As IRestResponse = client.Execute(request)
        Return response

    End Function

    Public Sub sendJSONfilePATCH(address As String, path As String)

        Dim JSON As String
        Dim jsonFile As String
        Dim client As RestClient

        jsonFile = path
        JSON = IO.File.ReadAllText(path)
        client = New RestClient(address)

        Dim request As New RestRequest(Method.PATCH)
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12

        request.AddHeader("Content-Type", "application/json")
        request.AddHeader("Authorization", "Bearer " & myToken.access_token & "")
        request.AddHeader("Accept", "application/json")
        request.AddParameter("application/json", JSON, ParameterType.RequestBody)

        Dim response As IRestResponse = client.Execute(request)
        MsgBox(response.Content)

    End Sub


    Public Function sendJSONstringPATCH(address As String, json As String) As IRestResponse

        Dim client As RestClient

        client = New RestClient(address)

        Dim request As New RestRequest(Method.PATCH)
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12

        request.AddHeader("Content-Type", "application/json")
        request.AddHeader("Authorization", "Bearer " & myToken.access_token & "")
        request.AddHeader("Accept", "application/json")
        request.AddParameter("application/json", json, ParameterType.RequestBody)

        Dim response As IRestResponse = client.Execute(request)
        Return response

    End Function

    Public Function getDatosPedido(pedido As String) As PedidoOS

        Dim client As New RestClient("https://api.datamars.com/ordering/v1/orders?search=" & pedido)
        Dim request As New RestRequest(Method.GET)
        request.AddHeader("Authorization", "bearer " & myToken.access_token & "")
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
        Dim response As IRestResponse = client.Execute(request)

        Dim pedidoOS As PedidoOS = Newtonsoft.Json.JsonConvert.DeserializeObject(Of PedidoOS)(response.Content).content(0)

        Return pedidoOS

    End Function


    Public Function getListaPedido(fechaMin As DateTime, fechaMax As DateTime) As List(Of PedidoOS)


        Dim listaPedidos As New List(Of PedidoOS)

        Dim i As Integer = 0
        Dim pagina As PaginaPedidos

        Dim client As New RestClient("https://api.datamars.com/ordering/v1/orders?sort=createdAt,desc&page=" & i & "&createdAfter=" & fechaMin.ToString("yyyy-MM-dd") & "T22:00:00Z&createdBefore=" & fechaMax.ToString("yyyy-MM-dd") & "T21:59:59Z&subsidiary.id=00000000-0000-0000-0000-000000000006&size=1000")
        Dim request As New RestRequest(Method.GET)
        request.AddHeader("Authorization", "bearer " & myToken.access_token & "")
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Tls12
        Dim response As IRestResponse = client.Execute(request)

        Do

            pagina = Newtonsoft.Json.JsonConvert.DeserializeObject(Of PaginaPedidos)(response.Content)

            'For Each p As PedidoOS In pagina.content
            listaPedidos.AddRange(pagina.content)
            'Next
            i += 1
            If pagina.last Then Exit Do
        Loop

        Return listaPedidos

    End Function

End Module
