<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main_Form
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
        Me.FromAccID = New System.Windows.Forms.ComboBox()
        Me.ToAccID = New System.Windows.Forms.ComboBox()
        Me.FromPeriod = New System.Windows.Forms.ComboBox()
        Me.FromYaear = New System.Windows.Forms.ComboBox()
        Me.ToPeriod = New System.Windows.Forms.ComboBox()
        Me.ToYear = New System.Windows.Forms.ComboBox()
        Me.shwbutton = New System.Windows.Forms.Button()
        Me.MainPanel = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.btnSelectModules = New System.Windows.Forms.Button()
        Me.txtSelectedModules = New System.Windows.Forms.TextBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.colSegmentName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFrom = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.MainPanel.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'FromAccID
        '
        Me.FromAccID.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FromAccID.FormattingEnabled = True
        Me.FromAccID.Location = New System.Drawing.Point(170, 120)
        Me.FromAccID.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.FromAccID.Name = "FromAccID"
        Me.FromAccID.Size = New System.Drawing.Size(166, 24)
        Me.FromAccID.TabIndex = 0
        '
        'ToAccID
        '
        Me.ToAccID.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToAccID.FormattingEnabled = True
        Me.ToAccID.Location = New System.Drawing.Point(521, 120)
        Me.ToAccID.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.ToAccID.Name = "ToAccID"
        Me.ToAccID.Size = New System.Drawing.Size(166, 24)
        Me.ToAccID.TabIndex = 1
        Me.ToAccID.Text = "ZZZ"
        '
        'FromPeriod
        '
        Me.FromPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FromPeriod.FormattingEnabled = True
        Me.FromPeriod.Location = New System.Drawing.Point(271, 89)
        Me.FromPeriod.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.FromPeriod.Name = "FromPeriod"
        Me.FromPeriod.Size = New System.Drawing.Size(65, 24)
        Me.FromPeriod.TabIndex = 3
        '
        'FromYaear
        '
        Me.FromYaear.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FromYaear.FormattingEnabled = True
        Me.FromYaear.Location = New System.Drawing.Point(170, 89)
        Me.FromYaear.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.FromYaear.Name = "FromYaear"
        Me.FromYaear.Size = New System.Drawing.Size(89, 24)
        Me.FromYaear.TabIndex = 2
        '
        'ToPeriod
        '
        Me.ToPeriod.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToPeriod.FormattingEnabled = True
        Me.ToPeriod.Location = New System.Drawing.Point(611, 89)
        Me.ToPeriod.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.ToPeriod.Name = "ToPeriod"
        Me.ToPeriod.Size = New System.Drawing.Size(75, 24)
        Me.ToPeriod.TabIndex = 5
        '
        'ToYear
        '
        Me.ToYear.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToYear.FormattingEnabled = True
        Me.ToYear.Location = New System.Drawing.Point(521, 89)
        Me.ToYear.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.ToYear.Name = "ToYear"
        Me.ToYear.Size = New System.Drawing.Size(71, 24)
        Me.ToYear.TabIndex = 4
        '
        'shwbutton
        '
        Me.shwbutton.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.shwbutton.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.shwbutton.Location = New System.Drawing.Point(306, 344)
        Me.shwbutton.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.shwbutton.Name = "shwbutton"
        Me.shwbutton.Size = New System.Drawing.Size(110, 32)
        Me.shwbutton.TabIndex = 6
        Me.shwbutton.Text = "Show Report"
        Me.shwbutton.UseVisualStyleBackColor = True
        '
        'MainPanel
        '
        Me.MainPanel.BackColor = System.Drawing.SystemColors.ControlLight
        Me.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MainPanel.Controls.Add(Me.Button1)
        Me.MainPanel.Controls.Add(Me.TextBox6)
        Me.MainPanel.Controls.Add(Me.btnSelectModules)
        Me.MainPanel.Controls.Add(Me.txtSelectedModules)
        Me.MainPanel.Controls.Add(Me.DataGridView1)
        Me.MainPanel.Controls.Add(Me.TextBox5)
        Me.MainPanel.Controls.Add(Me.TextBox4)
        Me.MainPanel.Controls.Add(Me.TextBox3)
        Me.MainPanel.Controls.Add(Me.TextBox2)
        Me.MainPanel.Controls.Add(Me.TextBox1)
        Me.MainPanel.Controls.Add(Me.shwbutton)
        Me.MainPanel.Controls.Add(Me.FromAccID)
        Me.MainPanel.Controls.Add(Me.ToPeriod)
        Me.MainPanel.Controls.Add(Me.ToAccID)
        Me.MainPanel.Controls.Add(Me.ToYear)
        Me.MainPanel.Controls.Add(Me.FromYaear)
        Me.MainPanel.Controls.Add(Me.FromPeriod)
        Me.MainPanel.Location = New System.Drawing.Point(27, 31)
        Me.MainPanel.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.MainPanel.Name = "MainPanel"
        Me.MainPanel.Padding = New System.Windows.Forms.Padding(17, 16, 17, 16)
        Me.MainPanel.Size = New System.Drawing.Size(721, 404)
        Me.MainPanel.TabIndex = 7
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Button1.Location = New System.Drawing.Point(648, 15)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(33, 32)
        Me.Button1.TabIndex = 16
        Me.Button1.Text = "⟳"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox6
        '
        Me.TextBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox6.Location = New System.Drawing.Point(21, 296)
        Me.TextBox6.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.Size = New System.Drawing.Size(142, 23)
        Me.TextBox6.TabIndex = 15
        Me.TextBox6.Text = "Source Modules"
        '
        'btnSelectModules
        '
        Me.btnSelectModules.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectModules.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.btnSelectModules.Location = New System.Drawing.Point(521, 293)
        Me.btnSelectModules.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnSelectModules.Name = "btnSelectModules"
        Me.btnSelectModules.Size = New System.Drawing.Size(58, 27)
        Me.btnSelectModules.TabIndex = 14
        Me.btnSelectModules.Text = "Modules"
        Me.btnSelectModules.UseVisualStyleBackColor = True
        '
        'txtSelectedModules
        '
        Me.txtSelectedModules.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSelectedModules.Location = New System.Drawing.Point(170, 296)
        Me.txtSelectedModules.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtSelectedModules.Name = "txtSelectedModules"
        Me.txtSelectedModules.Size = New System.Drawing.Size(344, 23)
        Me.txtSelectedModules.TabIndex = 13
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSegmentName, Me.colFrom, Me.colTo})
        Me.DataGridView1.Location = New System.Drawing.Point(21, 165)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 62
        Me.DataGridView1.RowTemplate.Height = 28
        Me.DataGridView1.Size = New System.Drawing.Size(665, 105)
        Me.DataGridView1.TabIndex = 12
        '
        'colSegmentName
        '
        Me.colSegmentName.HeaderText = "Segment Name"
        Me.colSegmentName.MinimumWidth = 8
        Me.colSegmentName.Name = "colSegmentName"
        Me.colSegmentName.Width = 150
        '
        'colFrom
        '
        Me.colFrom.HeaderText = "From"
        Me.colFrom.MinimumWidth = 8
        Me.colFrom.Name = "colFrom"
        Me.colFrom.Width = 150
        '
        'colTo
        '
        Me.colTo.HeaderText = "To"
        Me.colTo.MinimumWidth = 8
        Me.colTo.Name = "colTo"
        Me.colTo.Width = 150
        '
        'TextBox5
        '
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(371, 90)
        Me.TextBox5.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(142, 23)
        Me.TextBox5.TabIndex = 11
        Me.TextBox5.Text = "To Year/Period"
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(21, 91)
        Me.TextBox4.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(142, 23)
        Me.TextBox4.TabIndex = 10
        Me.TextBox4.Text = "From Year/Period"
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(371, 121)
        Me.TextBox3.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(142, 23)
        Me.TextBox3.TabIndex = 9
        Me.TextBox3.Text = "To Account ID"
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(21, 120)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(142, 23)
        Me.TextBox2.TabIndex = 8
        Me.TextBox2.Text = "From Account ID"
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.Maroon
        Me.TextBox1.Location = New System.Drawing.Point(253, 18)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(260, 25)
        Me.TextBox1.TabIndex = 7
        Me.TextBox1.Text = "General Ledger Report"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Main_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(773, 455)
        Me.Controls.Add(Me.MainPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Main_Form"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "General Ledger Report"
        Me.MainPanel.ResumeLayout(False)
        Me.MainPanel.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents FromAccID As ComboBox
    Friend WithEvents ToAccID As ComboBox
    Friend WithEvents FromPeriod As ComboBox
    Friend WithEvents FromYaear As ComboBox
    Friend WithEvents ToPeriod As ComboBox
    Friend WithEvents ToYear As ComboBox
    Friend WithEvents shwbutton As Button
    Friend WithEvents MainPanel As Panel
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents colSegmentName As DataGridViewTextBoxColumn
    Friend WithEvents colFrom As DataGridViewTextBoxColumn
    Friend WithEvents colTo As DataGridViewTextBoxColumn
    Friend WithEvents btnSelectModules As Button
    Friend WithEvents txtSelectedModules As TextBox
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents Button1 As Button
End Class
