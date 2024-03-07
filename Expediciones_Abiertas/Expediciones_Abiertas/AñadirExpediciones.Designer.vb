<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AñadirExpediciones
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.dgvExpedicion = New System.Windows.Forms.DataGridView()
        Me.PedidoOS = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.nudLineas = New System.Windows.Forms.NumericUpDown()
        Me.btnAñadirExpedicion = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.dgvExpedicion, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLineas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(630, 188)
        Me.SplitContainer1.SplitterDistance = 25
        Me.SplitContainer1.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Maroon
        Me.Label1.Location = New System.Drawing.Point(3, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(133, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "AÑADIR EXPEDICIÓN"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.dgvExpedicion)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.nudLineas)
        Me.SplitContainer2.Panel2.Controls.Add(Me.btnAñadirExpedicion)
        Me.SplitContainer2.Size = New System.Drawing.Size(630, 159)
        Me.SplitContainer2.SplitterDistance = 124
        Me.SplitContainer2.TabIndex = 0
        '
        'dgvExpedicion
        '
        Me.dgvExpedicion.AllowUserToAddRows = False
        Me.dgvExpedicion.AllowUserToDeleteRows = False
        Me.dgvExpedicion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvExpedicion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvExpedicion.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.PedidoOS})
        Me.dgvExpedicion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvExpedicion.Location = New System.Drawing.Point(0, 0)
        Me.dgvExpedicion.Name = "dgvExpedicion"
        Me.dgvExpedicion.Size = New System.Drawing.Size(630, 124)
        Me.dgvExpedicion.TabIndex = 0
        '
        'PedidoOS
        '
        Me.PedidoOS.HeaderText = "Pedido OS"
        Me.PedidoOS.MaxInputLength = 20
        Me.PedidoOS.Name = "PedidoOS"
        '
        'nudLineas
        '
        Me.nudLineas.Location = New System.Drawing.Point(13, 5)
        Me.nudLineas.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudLineas.Name = "nudLineas"
        Me.nudLineas.Size = New System.Drawing.Size(120, 20)
        Me.nudLineas.TabIndex = 1
        Me.nudLineas.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'btnAñadirExpedicion
        '
        Me.btnAñadirExpedicion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAñadirExpedicion.ForeColor = System.Drawing.Color.Maroon
        Me.btnAñadirExpedicion.Location = New System.Drawing.Point(278, 3)
        Me.btnAñadirExpedicion.Name = "btnAñadirExpedicion"
        Me.btnAñadirExpedicion.Size = New System.Drawing.Size(75, 23)
        Me.btnAñadirExpedicion.TabIndex = 0
        Me.btnAñadirExpedicion.Text = "Añadir"
        Me.btnAñadirExpedicion.UseVisualStyleBackColor = True
        '
        'AñadirExpediciones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 188)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "AñadirExpediciones"
        Me.Text = "Añadir Expediciones"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.dgvExpedicion, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLineas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Label1 As Label
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents dgvExpedicion As DataGridView
    Friend WithEvents btnAñadirExpedicion As Button
    Friend WithEvents nudLineas As NumericUpDown
    Friend WithEvents PedidoOS As DataGridViewTextBoxColumn
End Class
