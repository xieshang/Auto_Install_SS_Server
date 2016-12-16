<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class web
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
        Me.WebBrow = New System.Windows.Forms.WebBrowser()
        Me.SuspendLayout()
        '
        'WebBrow
        '
        Me.WebBrow.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WebBrow.Location = New System.Drawing.Point(1, 1)
        Me.WebBrow.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrow.Name = "WebBrow"
        Me.WebBrow.ScriptErrorsSuppressed = True
        Me.WebBrow.Size = New System.Drawing.Size(1187, 613)
        Me.WebBrow.TabIndex = 3
        '
        'web
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1191, 616)
        Me.Controls.Add(Me.WebBrow)
        Me.Name = "web"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "web"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents WebBrow As System.Windows.Forms.WebBrowser
End Class
