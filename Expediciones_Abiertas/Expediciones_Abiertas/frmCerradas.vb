Public Class frmCerradas
    Dim queries As New Consultas
    Dim dtExpedicionesAbiertas As New DataTable
    Dim cmbGlobal As New ComboBox
    Dim iGlobal As Integer = 0
    Dim AnchuraGlobal As Integer = 0
    Dim dtFinalAnterior As New DataTable
    Dim dtFiltro As New DataTable
    Dim indexColumna As Integer = 0
    Dim dtAntesFiltrar As New DataTable

    Private Sub frmAbiertas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Me.MinimumSize = New System.Drawing.Size(Me.Width, Me.Height)
        'Me.MaximumSize = New System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        'Me.AutoSize = True
        'Me.AutoSizeMode = AutoSizeMode.GrowAndShrink
        cargarGrid()
        rellenarExpediciones()
        Dim desde As String = DateTimePicker1.Value.ToString.Substring(0, DateTimePicker1.Value.ToString.LastIndexOf(" "))
        dtAntesFiltrar.DefaultView.RowFilter = "[Fecha Cierre] >= '" & desde & "'"
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
        dtFinal.Columns.Add("Fecha Cierre")
        dtFinal.Columns.Add("Importe")
        dtFinal.Columns.Add("Sobre")
        dtFinal.Columns.Add("Caja Pequeña")
        dtFinal.Columns.Add("Caja Mediana")
        dtFinal.Columns.Add("Caja Grande")
        dtFinal.Columns.Add("Caja Cole")

        'Tabla TipoBulto
        dtTipoBultos = queries.tipoBulto

        'Obtenemos el listado de expediciones abiertas
        dtExpedicionesAbiertas = queries.expedicionesCerradas
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
                'Si solo hay un cliente se añade
                If dtclientes.Rows.Count = 1 Then
                    r("Cliente") = dtclientes(0)(0)
                Else 'Sino se deja en blanco
                    r("Cliente") = ""
                End If
                r("Dirección") = row("direccion")
                Try
                    r("Fecha Cierre") = row("fechaCierre").ToString.Substring(0, row("fechaCierre").ToString.LastIndexOf(" "))
                Catch ex As Exception
                    r("Fecha Cierre") = ""
                End Try


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
        dtFiltro = dtFinal
        dtAntesFiltrar = dtFinal

    End Sub

    Private Sub rellenarExpediciones()
        Cursor.Current = Cursors.WaitCursor
        Dim dtExpediciones As New DataTable
        dtExpediciones.Columns.Add("Nº Expedición")
        dtExpediciones.Columns.Add("Cliente")
        dtExpediciones.Columns.Add("Dirección")
        dtExpediciones.Columns.Add("Fecha Cierre")
        dtExpediciones.Columns.Add("Importe")
        dtExpediciones.Columns.Add("Sobre")
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
                r("Fecha Cierre") = row("Fecha Cierre")
                Try
                    r("Importe") = importetotatExpedicion(row("Nº Expedición"))
                Catch ex As Exception
                    r("Importe") = ""
                End Try

                r("Sobre") = row("Sobre")
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
        dtAntesFiltrar = dtExpediciones

        'Limpiamos Controles
        panelFiltros.Controls.Clear()

        'Generamos los combobox
        crearComboBox(dtExpediciones)

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

    Private Sub rellenarPedidosExpedicion()
        Cursor.Current = Cursors.WaitCursor
        Dim dtPedidos As New DataTable
        dtPedidos.Columns.Add("Pedido OS")
        dtPedidos.Columns.Add("Pedido SAP")
        dtPedidos.Columns.Add("Cliente")
        dtPedidos.Columns.Add("Importe")

        Dim comprobar As Boolean = False
        Dim numExp As String = dgvExpediciones.Rows(dgvExpediciones.SelectedCells(0).RowIndex).Cells("Nº Expedición").Value

        For Each row As DataRow In dtFinalAnterior.Rows

            If row("Nº Expedición") = numExp Then
                Dim r As DataRow = dtPedidos.NewRow

                r("Pedido OS") = row("Pedido OS")
                r("Pedido SAP") = row("Pedido SAP")
                r("Importe") = CType(queries.importePedido(row("Pedido SAP")), Double)

                dtPedidos.Rows.Add(r)

            End If
        Next

        For Each row As DataRow In dtPedidos.Rows
            Dim dt As DataTable = queries.returndatospedidoSAP(row("Pedido OS"))
            row("Cliente") = dt(0)("CardName")

        Next

        dgvPedidos.DataSource = Nothing
        dgvPedidos.DataSource = dtPedidos

        dgvPedidos.AutoResizeColumns()

        ' Configure the details DataGridView so that its columns automatically
        ' adjust their widths when the data changes.
        dgvPedidos.AutoSizeColumnsMode =
        DataGridViewAutoSizeColumnsMode.AllCells

        'rellenarPedidosExpedicion()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub rellenarListadoPedidosExpedicion()
        Cursor.Current = Cursors.WaitCursor
        Dim dtListadoPedidoExpedicion As New DataTable
        dtListadoPedidoExpedicion.Columns.Add("Línea Pedido")
        dtListadoPedidoExpedicion.Columns.Add("Nombre")
        dtListadoPedidoExpedicion.Columns.Add("Referencia")
        dtListadoPedidoExpedicion.Columns.Add("Cliente Final")
        dtListadoPedidoExpedicion.Columns.Add("Customer Order No")
        dtListadoPedidoExpedicion.Columns.Add("Cantidad")

        Dim numExp As String = dgvExpediciones.Rows(dgvExpediciones.SelectedCells(0).RowIndex).Cells("Nº Expedición").Value

        Dim dt As DataTable = queries.contenidoExpedicion(numExp)
        Dim pedido As String = dgvPedidos.Rows(dgvPedidos.SelectedCells(0).RowIndex).Cells("Pedido OS").Value
        For Each row As DataRow In dt.Rows
            If row("codLineaPedido").ToString.Contains(pedido) Then
                Dim r As DataRow = dtListadoPedidoExpedicion.NewRow
                Dim dtLinea As DataTable = queries.lineasPedido(row("codLineaPedido"))
                Dim notas As String() = dtLinea(0)("note").ToString.Split(vbLf)
                For Each nota As String In notas
                    If nota.StartsWith("U_END") Then
                        r("Cliente Final") = nota.Substring(15, nota.Length - 15)
                    ElseIf nota.StartsWith("U_CUSTOR") Then
                        r("Customer Order No") = nota.Substring(13, nota.Length - 13)
                    End If
                Next
                r("Línea Pedido") = dtLinea(0)("number")
                r("Nombre") = dtLinea(0)("name")
                r("Referencia") = dtLinea(0)("sap_code")
                r("Cantidad") = CType(dtLinea(0)("Quantity"), Integer)
                dtListadoPedidoExpedicion.Rows.Add(r)
            End If
        Next

        dgvLineasPedido.DataSource = Nothing
        dgvLineasPedido.DataSource = dtListadoPedidoExpedicion

        dgvLineasPedido.AutoResizeColumns()

        ' Configure the details DataGridView so that its columns automatically
        ' adjust their widths when the data changes.
        dgvLineasPedido.AutoSizeColumnsMode =
        DataGridViewAutoSizeColumnsMode.AllCells
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub crearComboBox(dtPanel As DataTable)

        Dim pos As Integer = dgvExpediciones.RowHeadersWidth

        For Each c As DataColumn In dtPanel.Columns
            If c.ColumnName = "Sobre" Or c.ColumnName = "Fecha" Or c.ColumnName = "Caja Pequeña" Or c.ColumnName = "Caja Mediana" Or c.ColumnName = "Caja Grande" Or c.ColumnName = "Caja Cole" Or c.ColumnName = "Importe" Then
                Continue For
            End If
            Dim cmb As New ComboBox
            cmb.Name = c.ColumnName

            cmb.Visible = True

            If panelFiltros.Controls.Count = 0 Then
                cmb.Margin = New Padding() With {.Left = pos, .Top = 0, .Bottom = 0, .Right = 0}
            Else
                cmb.Margin = New Padding() With {.Left = 0, .Top = 0, .Bottom = 0, .Right = 0}
            End If

            cmb.Text = cmb.Name
            AddHandler cmb.TextChanged, AddressOf cmbFiltros_TextChanged
            AddHandler cmb.SelectedValueChanged, AddressOf cmbFiltros_TextChanged
            AddHandler cmb.DropDown, AddressOf cmb_DropDown

            cmbGlobal = Nothing
            cmbGlobal = cmb

            añadirCMB()
            ordenarCMB()
        Next

    End Sub

    Private Sub cmbFiltros_TextChanged(sender As Object, e As EventArgs)

        Dim str As String = ""
        dtFiltro = dgvExpediciones.DataSource

        For Each c As Control In panelFiltros.Controls
            Dim cmb As ComboBox = c
            If cmb.Text = "" Or cmb.Text = cmb.Name Then Continue For
            str &= "[" & cmb.Name & "] like '%" & cmb.Text & "%' AND "
        Next

        'Si se borra el filtro vuelve a cargar la tabla original y deja el nombre del cmb
        If str = "" Then
            dgvExpediciones.DataSource = dtAntesFiltrar
            'modificarAnchuraDelDGV()
            'dtFinal = dtFinal
            For Each c As Control In panelFiltros.Controls
                Dim cmb As ComboBox = c
                If cmb.Text = "" Then
                    cmb.Text = c.Name
                End If
            Next
            Exit Sub
        End If
        Try
            Dim filas() As DataRow = dtFinalAnterior.Select(str.Substring(0, str.Length - 4))
            If filas.Count = 0 Then
                MessageBox.Show("No se han encontrado resultados", "ATENCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                dtFiltro = filas.CopyToDataTable
            End If

            dgvExpediciones.DataSource = dtFiltro
            'modificarAnchuraDelDGV()

        Catch ex As Exception
            MsgBox("Solo puede filtrarse las cadenas de texto")
            Exit Sub
        End Try


    End Sub

    Private Sub anchuraCMB()
        If panelFiltros.InvokeRequired Then
            Me.panelFiltros.Invoke(New MethodInvoker(AddressOf anchuraCMB))
        Else
            Me.panelFiltros.Controls(iGlobal).Width = dgvExpediciones.Columns(iGlobal).Width
        End If
    End Sub

    Sub cmb_DropDown(Sender As Object, e As EventArgs)

        Dim cmb As ComboBox = Sender
        dtFiltro = dgvExpediciones.DataSource
        cmb.Items.Clear()

        Dim t As DataTable = New DataView(dtFiltro).ToTable(True, cmb.Name)

        For Each r As DataRow In t.Rows
            cmb.Items.Add(r(0))
        Next

        Dim anchura As Integer = Sender.DropDownWidth

        For Each ite As String In cmb.Items
            Dim newWidth As Integer = TextRenderer.MeasureText(cmb.GetItemText(ite), cmb.Font).Width
            If newWidth > anchura Then
                anchura = newWidth + 50
            End If
        Next

        cmb.DropDownWidth = anchura

    End Sub

    Private Sub añadirCMB()
        If panelFiltros.InvokeRequired Then
            Me.panelFiltros.Invoke(New MethodInvoker(AddressOf añadirCMB))
        Else
            Me.panelFiltros.Controls.Add(cmbGlobal)
        End If
    End Sub


    Private Sub ordenarCMB()
        If cmbGlobal.InvokeRequired Then
            Me.cmbGlobal.Invoke(New MethodInvoker(AddressOf añadirCMB))
        Else
            Me.cmbGlobal.Sorted = True
        End If
    End Sub


    Private Sub dgvExpediciones_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvExpediciones.CellDoubleClick
        If dgvExpediciones.SelectedCells(0).ColumnIndex <> 0 Then
            Exit Sub
        End If

        'Comprobacion por problemas de actualizacion en tiempo real
        Dim bool As Boolean = queries.estadoExpedicion(dgvExpediciones.Rows(dgvExpediciones.SelectedCells(0).RowIndex).Cells("Nº Expedición").Value)
        If bool = False Then
            MsgBox("La expedición ya ha sido abierta en otro equipo")
            cargarGrid()
            rellenarExpediciones()
            Exit Sub
        End If

        Dim respuesta As String = MsgBox("Está seguro de que quieres abrir la expedición " & dgvExpediciones.Rows(dgvExpediciones.SelectedCells(0).RowIndex).Cells("Nº Expedición").Value & "", vbYesNo)

        If respuesta = 6 Then
            Dim idexpedicion As Integer = dgvExpediciones.Rows(dgvExpediciones.SelectedCells(0).RowIndex).Cells("Nº Expedición").Value
            Dim b As Boolean = queries.abrirExpedicion(idexpedicion)
            If b = True Then
                MsgBox("La expedición " & idexpedicion & " se ha abierto correctamente")

                cargarGrid()
                rellenarExpediciones()

                dgvPedidos.DataSource = Nothing
                dgvLineasPedido.DataSource = Nothing
            Else
                MsgBox("Error al abrir la expedición. Llamad a informática")
            End If

        End If

    End Sub

    Private Sub dgvExpediciones_CellClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles dgvExpediciones.CellClick

        'dgvExpediciones.Rows(dgvExpediciones.SelectedCells(0).RowIndex).Selected = True
        rellenarPedidosExpedicion()
        dgvLineasPedido.DataSource = Nothing
    End Sub

    Private Sub dgvPedidos_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPedidos.CellClick
        'dgvPedidos.Rows(dgvPedidos.SelectedCells(0).RowIndex).Selected = True
        rellenarListadoPedidosExpedicion()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Dim desde As String = DateTimePicker1.Value.ToString.Substring(0, DateTimePicker1.Value.ToString.LastIndexOf(" "))
        Dim hasta As String = DateTimePicker2.Value.ToString.Substring(0, DateTimePicker2.Value.ToString.LastIndexOf(" "))
        dtAntesFiltrar.DefaultView.RowFilter = "[Fecha Cierre] >= '" & desde & "' AND [Fecha Cierre] <= '" & hasta & "'"
    End Sub

    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        Dim desde As String = DateTimePicker1.Value.ToString.Substring(0, DateTimePicker1.Value.ToString.LastIndexOf(" "))
        Dim hasta As String = DateTimePicker2.Value.ToString.Substring(0, DateTimePicker2.Value.ToString.LastIndexOf(" "))
        dtAntesFiltrar.DefaultView.RowFilter = "[Fecha Cierre] >= '" & desde & "' AND [Fecha Cierre] <= '" & hasta & "'"
    End Sub
End Class