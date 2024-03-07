Public Class MoverLineasPedido
    Dim queries As New Consultas
    Dim dtFinalAnterior As New DataTable
    Private Sub MoverLineasPedido_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cargarGrid()
        rellenarExpediciones()
    End Sub

    Private Sub cargarGrid()
        Dim dtFinal As New DataTable
        Dim dtExpedicionesAbiertas As New DataTable
        Dim dtTipoBultos As New DataTable
        Dim dtContenidoExpedicion As New DataTable
        Dim dtNumBultos As New DataTable
        Dim sobre, cajap, cajam, cajag, cajac As New Integer
        dtFinal.Columns.Add("Nº Expedición")
        dtFinal.Columns.Add("Pedido OS")
        dtFinal.Columns.Add("Pedido SAP")
        dtFinal.Columns.Add("Cliente")
        dtFinal.Columns.Add("Dirección")
        dtFinal.Columns.Add("Fecha")
        dtFinal.Columns.Add("Importe")
        dtFinal.Columns.Add("Urgente", GetType(Boolean))
        dtFinal.Columns.Add("Sobre")
        dtFinal.Columns.Add("Caja Pequeña")
        dtFinal.Columns.Add("Caja Mediana")
        dtFinal.Columns.Add("Caja Grande")
        dtFinal.Columns.Add("Caja Cole")

        'Tabla TipoBulto
        dtTipoBultos = queries.tipoBulto

        'Obtenemos el listado de expediciones abiertas
        dtExpedicionesAbiertas = queries.expedicionesAbiertas
        dtExpedicionesAbiertas.Columns("idClienteSAP").MaxLength = 100

        Dim dtclientes As New DataTable
        dtclientes.Columns.Add("Cliente")

        'Por cada expedicion obtenemos los datos que queremos
        For Each row As DataRow In dtExpedicionesAbiertas.Rows

            'Comprobar Cliente
            Try
                'Obtenemos todo su contenido para saber el pedido de SAP
                Dim dt As DataTable = queries.contenidoExpedicion(row("id"))
                'Por cada una de las lineas de contenido comprobamos su cliente
                For Each fila As DataRow In dt.Rows
                    Dim r As DataRow = dtclientes.NewRow
                    Dim docnum As String = fila("pedidoSAP")
                    Dim cliente As String = queries.clienteSAPPorDocNum(docnum)
                    'Este for es para comprobar que no añadimos un cliente repetido a la tabla
                    Dim compruebaCliente As Boolean = False
                    For Each fila2 As DataRow In dtclientes.Rows
                        If fila2("Cliente") = cliente Then
                            compruebaCliente = True
                        End If
                    Next
                    If compruebaCliente = True Then
                        compruebaCliente = False
                        Continue For
                    End If
                    r("Cliente") = cliente
                    dtclientes.Rows.Add(r)
                Next

                'row("idClienteSAP") = queries.clienteSAP(row("idClienteSAP"))
            Catch ex As Exception

            End Try

            dtNumBultos = queries.numBultos(row("id"))

            'obtenemos el numero de bultos que hay para cada tipo
            For Each rbultos As DataRow In dtNumBultos.Rows
                Select Case rbultos("idTipoBulto")
                    Case 1 'SOBRE
                        sobre += 1
                    Case 2 'CAJA PEQUEÑA
                        cajap += 1
                    Case 3 'CAJA MEDIANA
                        cajam += 1
                    Case 4 'CAJA COLES
                        cajac += 1
                    Case 5 'CAJA GRANDE
                        cajag += 1
                End Select
            Next

            dtContenidoExpedicion = queries.contenidoExpedicion(row("id"))
            Dim dtauxiliar As New DataTable
            dtauxiliar.Columns.Add("Pedido")
            dtauxiliar.Columns.Add("PedidoSAP")
            Dim pedido As String = ""

            'Obtengo todos los pedido distintos que tenemos para una expedicion
            For Each rcontenido As DataRow In dtContenidoExpedicion.Rows
                Dim raux As DataRow = dtauxiliar.NewRow

                'comprobacion inicial
                If pedido = "" Then
                    Dim a As Integer = rcontenido("codLineaPedido").LastIndexOf("-")
                    pedido = rcontenido("codLineaPedido").Substring(0, a)
                    raux("Pedido") = pedido
                    raux("PedidoSAP") = rcontenido("pedidoSAP")
                    dtauxiliar.Rows.Add(raux)
                    Continue For
                End If

                'Para saber cuando cambio de pedido
                Dim b As Integer = rcontenido("codLineaPedido").LastIndexOf("-")
                If pedido = rcontenido("codLineaPedido").Substring(0, b) Then
                    Continue For
                Else
                    pedido = rcontenido("codLineaPedido").Substring(0, b)
                    raux("Pedido") = pedido
                    raux("PedidoSAP") = rcontenido("pedidoSAP")
                    dtauxiliar.Rows.Add(raux)
                    Continue For
                End If
            Next

            dtFinal.PrimaryKey = Nothing

            'Ahora tengo que añadir a la tabla final una linea por cada pedido distinto para una misma expedicion
            For Each rauxiliar As DataRow In dtauxiliar.Rows
                Dim r As DataRow = dtFinal.NewRow

                'obtenemos el numero de bultos para una expedicion
                r("Nº Expedición") = row("id")
                If dtclientes.Rows.Count = 1 Then
                    r("Cliente") = dtclientes(0)(0)
                Else 'Sino se deja en blanco
                    r("Cliente") = ""
                End If
                r("Dirección") = row("direccion")
                r("Fecha") = row("fecha").ToString.Substring(0, row("Fecha").ToString.LastIndexOf(" "))

                r("Sobre") = sobre
                r("Caja Pequeña") = cajap
                r("Caja Mediana") = cajam
                r("Caja Grande") = cajag
                r("Caja Cole") = cajac

                r("Pedido OS") = rauxiliar("Pedido")
                r("Pedido SAP") = rauxiliar("PedidoSAP")
                Try
                    r("Importe") = CType(queries.ImporteTotal(rauxiliar("PedidoSAP")), Double)
                Catch ex As Exception
                    r("Importe") = ""
                End Try

                r("Urgente") = queries.estadoUrgente(row("id"))
                dtFinal.Rows.Add(r)

            Next
            dtclientes.Rows.Clear()
            sobre = 0
            cajap = 0
            cajam = 0
            cajag = 0
            cajac = 0

        Next

        dtFinalAnterior = dtFinal

    End Sub


    Private Sub rellenarExpediciones()
        Cursor.Current = Cursors.WaitCursor
        Dim dtExpediciones As New DataTable
        dtExpediciones.Columns.Add("Nº Expedición")
        dtExpediciones.Columns.Add("Cliente")
        dtExpediciones.Columns.Add("Dirección")
        dtExpediciones.Columns.Add("Fecha")
        dtExpediciones.Columns.Add("Importe")
        dtExpediciones.Columns.Add("Sobre")
        dtExpediciones.Columns.Add("Urgente", GetType(Boolean))
        dtExpediciones.Columns.Add("Caja Pequeña")
        dtExpediciones.Columns.Add("Caja Mediana")
        dtExpediciones.Columns.Add("Caja Grande")
        dtExpediciones.Columns.Add("Caja Cole")

        Dim comprobar As Boolean = False

        For Each row As DataRow In dtFinalAnterior.Rows
            Dim r As DataRow = dtExpediciones.NewRow
            For Each fila As DataRow In dtExpediciones.Rows
                If row("Nº Expedición") = fila("Nº Expedición") Then
                    comprobar = True
                    Exit For
                End If

            Next
            If comprobar = False Then
                r("Nº Expedición") = row("Nº Expedición")
                r("Cliente") = row("Cliente")
                r("Dirección") = row("Dirección")
                r("Fecha") = row("Fecha")
                Try
                    r("Importe") = importetotatExpedicion(row("Nº Expedición"))
                Catch ex As Exception
                    r("Importe") = ""
                End Try

                r("Sobre") = row("Sobre")
                r("Urgente") = row("Urgente")
                r("Caja Pequeña") = row("Caja Pequeña")
                r("Caja Mediana") = row("Caja Mediana")
                r("Caja Grande") = row("Caja Grande")
                r("Caja Cole") = row("Caja Cole")
                dtExpediciones.Rows.Add(r)
            Else
                comprobar = False
            End If

        Next

        dgvExpediciones1.DataSource = dtExpediciones
        Dim dtexpediciones2 As DataTable = dtExpediciones.Copy
        dgvExpediciones2.DataSource = dtexpediciones2

        For Each column As DataGridViewColumn In dgvExpediciones1.Columns
            If column.Name = "Sobre" Or column.Name = "Caja Pequeña" Or column.Name = "Caja Mediana" Or column.Name = "Caja Grande" Or column.Name = "Caja Cole" Then
                column.Visible = False
            End If
        Next

        For Each column As DataGridViewColumn In dgvExpediciones2.Columns
            If column.Name = "Sobre" Or column.Name = "Caja Pequeña" Or column.Name = "Caja Mediana" Or column.Name = "Caja Grande" Or column.Name = "Caja Cole" Then
                column.Visible = False
            End If
        Next

        Cursor.Current = Cursors.Default
    End Sub

    Private Function importetotatExpedicion(expedicion As Integer) As String
        Dim dt As DataTable = queries.contenidoExpedicion(expedicion)
        Dim dtpedido As New DataTable
        dtpedido.Columns.Add("Pedido")
        dtpedido.Columns.Add("PedidoSAP")

        Dim suma As Double = 0
        Dim b As Boolean = False
        For Each row As DataRow In dt.Rows
            Dim a As Integer = row("codLineaPedido").ToString.LastIndexOf("-")
            Dim pedido As String = row("codLineaPedido").ToString.Substring(0, a)
            If dt.Rows.IndexOf(row) = 0 Then
                Dim r As DataRow = dtpedido.NewRow
                a = row("codLineaPedido").ToString.LastIndexOf("-")
                pedido = row("codLineaPedido").ToString.Substring(0, a)
                r("Pedido") = pedido
                r("PedidoSAP") = row("pedidoSAP")
                dtpedido.Rows.Add(r)
                Continue For
            End If
            b = False
            For Each fila As DataRow In dtpedido.Rows

                If fila("Pedido") = pedido Then
                    b = True
                End If
            Next
            If b = True Then
                Continue For
            End If
            Dim ra As DataRow = dtpedido.NewRow
            ra("Pedido") = pedido
            ra("PedidoSAP") = row("pedidoSAP")
            dtpedido.Rows.Add(ra)

        Next

        For Each row2 As DataRow In dtpedido.Rows
            suma += CType(queries.importePedido(row2("PedidoSAP")), Double)
        Next
        Return suma
    End Function

    Private Sub dgvExpediciones1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvExpediciones1.CellDoubleClick
        For Each row As DataGridViewRow In dgvExpediciones1.SelectedRows
            Dim idexpedicion As Integer = dgvExpediciones1.SelectedRows(0).Cells("Nº Expedición").Value
            Try
                If idexpedicion = dgvExpediciones2.SelectedRows(0).Cells("Nº Expedición").Value Then
                    dgvPedidos1.DataSource = Nothing
                    Exit Sub
                End If
            Catch ex As Exception

            End Try

            dgvPedidos1.DataSource = rellenarListadoPedidosExpedicion(idexpedicion)
            dgvPedidos1.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        Next
    End Sub

    Private Sub dgvExpediciones2_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvExpediciones2.CellDoubleClick
        For Each row As DataGridViewRow In dgvExpediciones1.SelectedRows
            Dim idexpedicion As Integer = dgvExpediciones2.SelectedRows(0).Cells("Nº Expedición").Value

            Try
                If idexpedicion = dgvExpediciones1.SelectedRows(0).Cells("Nº Expedición").Value Then
                    dgvPedidos2.DataSource = Nothing
                    Exit Sub
                End If
            Catch ex As Exception

            End Try

            dgvPedidos2.DataSource = rellenarListadoPedidosExpedicion(idexpedicion)
            dgvPedidos2.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        Next
    End Sub

    Private Function rellenarListadoPedidosExpedicion(numExp As Integer) As DataTable
        Cursor.Current = Cursors.WaitCursor
        Dim dtListadoPedidoExpedicion As New DataTable
        dtListadoPedidoExpedicion.Columns.Add("Línea Pedido")
        dtListadoPedidoExpedicion.Columns.Add("Nombre")
        dtListadoPedidoExpedicion.Columns.Add("Referencia")
        dtListadoPedidoExpedicion.Columns.Add("Cantidad")

        Dim dt As DataTable = queries.contenidoExpedicion(numExp)

        For Each row As DataRow In dt.Rows
            Dim r As DataRow = dtListadoPedidoExpedicion.NewRow
            Dim dtLinea As DataTable = queries.lineasPedido(row("codLineaPedido"))
            r("Línea Pedido") = dtLinea(0)("number")
            r("Nombre") = dtLinea(0)("name")
            r("Referencia") = dtLinea(0)("sap_code")
            r("Cantidad") = CType(dtLinea(0)("Quantity"), Integer)
            dtListadoPedidoExpedicion.Rows.Add(r)
        Next

        Return dtListadoPedidoExpedicion
        Cursor.Current = Cursors.Default
    End Function

    Private Sub dgvPedidos1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPedidos1.CellDoubleClick
        Dim idexpedicion1, idexpedicion2 As Integer
        Dim lineapedido1 As String

        idexpedicion1 = dgvExpediciones1.SelectedRows(0).Cells("Nº Expedición").Value
        idexpedicion2 = dgvExpediciones2.SelectedRows(0).Cells("Nº Expedición").Value

        For Each row As DataGridViewRow In dgvPedidos1.SelectedRows
            lineapedido1 = row.Cells("Línea Pedido").Value
            Dim resultado As Integer = queries.MoverlineadePedido(idexpedicion2, idexpedicion1, lineapedido1)
        Next

        'rellenamos los datos para ver el cambio

        dgvPedidos1.DataSource = rellenarListadoPedidosExpedicion(idexpedicion1)
        dgvPedidos2.DataSource = rellenarListadoPedidosExpedicion(idexpedicion2)

    End Sub

    Private Sub dgvPedidos2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPedidos2.CellDoubleClick
        Dim idexpedicion1, idexpedicion2 As Integer
        Dim lineapedido2 As String
        idexpedicion1 = dgvExpediciones1.SelectedRows(0).Cells("Nº Expedición").Value
        idexpedicion2 = dgvExpediciones2.SelectedRows(0).Cells("Nº Expedición").Value

        For Each row As DataGridViewRow In dgvPedidos2.SelectedRows
            lineapedido2 = row.Cells("Línea Pedido").Value
            Dim resultado As Integer = queries.MoverlineadePedido(idexpedicion1, idexpedicion2, lineapedido2)
            'Cambiar la asignacion de ese pedido por la idexpedicion2
        Next

        'rellenamos los datos para ver el cambio

        dgvPedidos1.DataSource = rellenarListadoPedidosExpedicion(idexpedicion1)
        dgvPedidos2.DataSource = rellenarListadoPedidosExpedicion(idexpedicion2)
    End Sub

End Class