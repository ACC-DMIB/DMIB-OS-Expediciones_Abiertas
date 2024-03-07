Public Class UnificarExpediciones

    Dim queries As New Consultas
    Dim dtFinalAnterior As New DataTable
    Private Sub UnificarExpediciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cargarGrid()
        rellenarExpediciones()

        For Each c As DataGridViewColumn In dgvExpediciones.Columns
            dgvAgrupar.Columns.Add(TryCast(c.Clone(), DataGridViewColumn))
        Next

        Button2.Text = queries.ExpedicionesPendientesUnificar

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


        'Modificamos el valor del cliente por el obtenido en SAP
        'Por cada expedicion que hemos obtenido
        'For Each row As DataRow In dtExpedicionesAbiertas.Rows


        'Next

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

                'Si solo hay un cliente se añade
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

        dgvExpediciones.DataSource = dtExpediciones

        dgvExpediciones.AutoResizeColumns()

        ' Configure the details DataGridView so that its columns automatically
        ' adjust their widths when the data changes.
        dgvExpediciones.AutoSizeColumnsMode =
        DataGridViewAutoSizeColumnsMode.AllCells
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

    Private Sub dgvExpediciones_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvExpediciones.CellDoubleClick
        For Each r As DataGridViewRow In dgvExpediciones.SelectedRows
            For Each row As DataGridViewRow In dgvAgrupar.Rows
                If row.Cells("Nº Expedición").Value = r.Cells(0).Value Then
                    Exit Sub
                End If
            Next
            Dim index As Integer = dgvAgrupar.Rows.Add(TryCast(r.Clone(), DataGridViewRow))

            For Each o As DataGridViewCell In r.Cells

                dgvAgrupar.Rows(index).Cells(o.ColumnIndex).Value = o.Value
                If o.ColumnIndex = 1 Then
                    cbDirecciones.Items.Add(o.Value)
                End If
            Next
        Next


    End Sub


    Private Sub dgvAgrupar_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAgrupar.CellDoubleClick
        For Each r As DataGridViewRow In dgvAgrupar.SelectedRows
            Dim valor As String = dgvAgrupar.SelectedRows(0).Cells("Dirección").Value
            For Each c In cbDirecciones.Items
                If c = valor Then

                    cbDirecciones.Items.Remove(c)
                    If cbDirecciones.Text = "" Then
                        Button1.Enabled = False
                    End If
                    Exit For
                End If
            Next
        Next

        dgvAgrupar.Rows.RemoveAt(dgvAgrupar.SelectedRows(0).Index)
        If dgvAgrupar.Rows.Count = 0 Then
            cbDirecciones.Text = ""
            Button1.Enabled = False
        End If
    End Sub

    Private Sub cbDirecciones_SelectedValueChanged(sender As Object, e As EventArgs) Handles cbDirecciones.SelectedValueChanged
        If cbDirecciones.Text <> "" Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim mensaje As String
        Dim pc As String = My.Computer.Name

        mensaje = " == DIRECCIÓN FINAL: " & cbDirecciones.Text & " ==" & vbCrLf & vbCrLf
        For Each row As DataGridViewRow In dgvAgrupar.Rows
            mensaje = mensaje & row.Cells("Nº Expedición").Value & ": " & row.Cells("Dirección").Value & vbCrLf & vbCrLf
            Dim dtlineas As DataTable = queries.contenidoExpedicion(row.Cells("Nº Expedición").Value)
            For Each r As DataRow In dtlineas.Rows
                mensaje = mensaje & vbTab & r("codLineaPedido") & " | " & row.Cells("Cliente").Value & vbCrLf
            Next
            mensaje = mensaje & vbCrLf
        Next

        Try
            Dim id As Integer = queries.returnidUnificarExpedicion + 1
            queries.InsertarUnificarExpedicion(id, pc, Now, mensaje, False, "PEDIDOS AGRUPADOS")
            MsgBox("NOTIFICACIÓN DE UNIÓN COMPLETADA")
            Button2.Text = queries.ExpedicionesPendientesUnificar
        Catch ex As Exception
            MsgBox("Ha ocurrido un error al notificar la unión de las expediciones: " & ex.Message)
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Cursor = Cursors.WaitCursor


        Dim fc As New SinUnificar

        With fc
            .StartPosition = FormStartPosition.CenterScreen
            .BringToFront()
            .Show()
        End With
        Cursor = Cursors.Default
    End Sub
End Class