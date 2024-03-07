Imports PedidosAbiertosFinal
Public Class frmExpediciones
    Dim childform As New List(Of Form)
    Private Sub ABIERTOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ABIERTOSToolStripMenuItem.Click
        Cursor = Cursors.WaitCursor

        ActiveMdiChild.Close()

        Dim fc As New frmAbiertas

        With fc
            .MdiParent = Me
            .Dock = DockStyle.Fill
            .BringToFront()
            .Show()
        End With
        Cursor = Cursors.Default
    End Sub

    Private Sub frmExpediciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Cursor = Cursors.WaitCursor

        Dim fc As New frmAbiertas

        With fc
            .MdiParent = Me
            .Dock = DockStyle.Fill
            .StartPosition = FormStartPosition.CenterScreen
            .BringToFront()
            .Show()
        End With
        Cursor = Cursors.Default
    End Sub

    Private Sub CERRADOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CERRADOSToolStripMenuItem.Click
        Cursor = Cursors.WaitCursor

        ActiveMdiChild.Close()

        Dim fc As New frmCerradas

        With fc
            .MdiParent = Me
            .Dock = DockStyle.Fill
            .StartPosition = FormStartPosition.CenterScreen
            .BringToFront()
            .Show()
        End With
        Cursor = Cursors.Default
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        Cursor = Cursors.WaitCursor
        System.Diagnostics.Process.Start(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "PedidosAbiertosFinal.exe"))
        Cursor = Cursors.Default
        'Cursor = Cursors.WaitCursor

        'ActiveMdiChild.Close()

        'Dim fc As New PedidosAbiertosFinal.Form1

        'With fc

        '    .MdiParent = Me
        '    .StartPosition = FormStartPosition.CenterScreen
        '    .BringToFront()
        '    .Show()

        'End With

        'Cursor = Cursors.Default
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        Cursor = Cursors.WaitCursor

        Dim fc As New AñadirExpediciones

        With fc
            .MdiParent = Me
            .StartPosition = FormStartPosition.CenterScreen
            .BringToFront()
            .Show()
        End With
        Cursor = Cursors.Default
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        Cursor = Cursors.WaitCursor


        Dim fc As New UnificarExpediciones

        With fc
            .MdiParent = Me
            .StartPosition = FormStartPosition.CenterScreen
            .BringToFront()
            .Show()
        End With
        Cursor = Cursors.Default
    End Sub

    Private Sub ToolStripMenuItem5_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem5.Click
        Cursor = Cursors.WaitCursor


        Dim fc As New MoverLineasPedido

        With fc
            .MdiParent = Me
            .StartPosition = FormStartPosition.CenterScreen
            .BringToFront()
            .Show()
        End With
        Cursor = Cursors.Default
    End Sub
End Class
