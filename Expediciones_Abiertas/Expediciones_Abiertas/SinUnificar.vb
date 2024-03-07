Public Class SinUnificar

    Private Sub SinUnificar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim query As New DataSet1TableAdapters.NotificarUnionExpedicionesTableAdapter
        Dim dt As DataTable = query.GetDataByNoLeidos
        dgvSinLeer.DataSource = dt

        For Each column As DataGridViewColumn In dgvSinLeer.Columns
            column.Visible = False
        Next

        dgvSinLeer.Columns("id").Visible = True
        dgvSinLeer.Columns("mensaje").Visible = True
    End Sub

    Private Sub dgvSinLeer_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSinLeer.CellClick
        RichTextBox1.Text = ""
        Dim mensaje As String = dgvSinLeer.Rows(dgvSinLeer.SelectedCells(0).RowIndex).Cells("mensaje").Value
        RichTextBox1.Text = mensaje
    End Sub
End Class