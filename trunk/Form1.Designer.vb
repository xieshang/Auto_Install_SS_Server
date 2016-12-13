<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
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

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.log = New System.Windows.Forms.RichTextBox()
        Me.reg = New System.Windows.Forms.Button()
        Me.login = New System.Windows.Forms.Button()
        Me.DGV = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.Public_SS = New System.Windows.Forms.TabPage()
        Me.arukas_io = New System.Windows.Forms.TabPage()
        CType(Me.DGV, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.arukas_io.SuspendLayout()
        Me.SuspendLayout()
        '
        'log
        '
        Me.log.Location = New System.Drawing.Point(1, 243)
        Me.log.Name = "log"
        Me.log.Size = New System.Drawing.Size(286, 139)
        Me.log.TabIndex = 0
        Me.log.Text = ""
        '
        'reg
        '
        Me.reg.Location = New System.Drawing.Point(15, 4)
        Me.reg.Name = "reg"
        Me.reg.Size = New System.Drawing.Size(75, 23)
        Me.reg.TabIndex = 1
        Me.reg.Text = "注册"
        Me.reg.UseVisualStyleBackColor = True
        '
        'login
        '
        Me.login.Location = New System.Drawing.Point(96, 5)
        Me.login.Name = "login"
        Me.login.Size = New System.Drawing.Size(75, 23)
        Me.login.TabIndex = 2
        Me.login.Text = "读取信息"
        Me.login.UseVisualStyleBackColor = True
        '
        'DGV
        '
        Me.DGV.AllowUserToAddRows = False
        Me.DGV.AllowUserToDeleteRows = False
        Me.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.DGV.Location = New System.Drawing.Point(3, 34)
        Me.DGV.Name = "DGV"
        Me.DGV.ReadOnly = True
        Me.DGV.RowHeadersWidth = 4
        Me.DGV.RowTemplate.Height = 27
        Me.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DGV.Size = New System.Drawing.Size(277, 176)
        Me.DGV.TabIndex = 3
        '
        'Column1
        '
        Me.Column1.HeaderText = "应用名称"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 200
        '
        'Column2
        '
        Me.Column2.HeaderText = "端口"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 60
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.Public_SS)
        Me.TabControl1.Controls.Add(Me.arukas_io)
        Me.TabControl1.Location = New System.Drawing.Point(1, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(290, 241)
        Me.TabControl1.TabIndex = 4
        '
        'Public_SS
        '
        Me.Public_SS.Location = New System.Drawing.Point(4, 25)
        Me.Public_SS.Name = "Public_SS"
        Me.Public_SS.Padding = New System.Windows.Forms.Padding(3)
        Me.Public_SS.Size = New System.Drawing.Size(282, 212)
        Me.Public_SS.TabIndex = 0
        Me.Public_SS.Text = "公布SS"
        Me.Public_SS.UseVisualStyleBackColor = True
        '
        'arukas_io
        '
        Me.arukas_io.Controls.Add(Me.DGV)
        Me.arukas_io.Controls.Add(Me.reg)
        Me.arukas_io.Controls.Add(Me.login)
        Me.arukas_io.Location = New System.Drawing.Point(4, 25)
        Me.arukas_io.Name = "arukas_io"
        Me.arukas_io.Padding = New System.Windows.Forms.Padding(3)
        Me.arukas_io.Size = New System.Drawing.Size(282, 212)
        Me.arukas_io.TabIndex = 1
        Me.arukas_io.Text = "arukas.io"
        Me.arukas_io.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(288, 382)
        Me.Controls.Add(Me.log)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "arukas.io Tools"
        CType(Me.DGV, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.arukas_io.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents log As System.Windows.Forms.RichTextBox
    Friend WithEvents reg As System.Windows.Forms.Button
    Friend WithEvents login As System.Windows.Forms.Button
    Friend WithEvents DGV As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents Public_SS As System.Windows.Forms.TabPage
    Friend WithEvents arukas_io As System.Windows.Forms.TabPage

End Class
