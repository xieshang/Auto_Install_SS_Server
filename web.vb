Public Class web

    Private Sub web_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Mweb_Reg(WebBrow)
    End Sub

    Private Sub web_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Hide()
        e.Cancel = True
    End Sub
End Class