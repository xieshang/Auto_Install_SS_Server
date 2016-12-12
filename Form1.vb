Imports System.Threading

Public Class Form1
    Dim webform As New web

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Top = 1
        webform.Hide()

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            webform.Close()
        Catch ex As Exception

        End Try
    End Sub

    Sub sleepdo(ByVal t As Long)

        For i = 0 To t - 1
            Thread.Sleep(1)
            Application.DoEvents()
        Next
    End Sub




    Dim webthread As Thread

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        webform.Show()

        webform.WebBrow.Navigate("https://app.arukas.io/")
        sleepdo(1000)
        While webform.WebBrow.IsBusy
            Application.DoEvents()
        End While
        For t = 0 To 1000
            Thread.Sleep(1)
            Application.DoEvents()
            Try
                If webform.WebBrow.DocumentTitle.IndexOf("Panel") > 0 Then
                    MsgBox("控制面板")
                Else
                    MsgBox("非控制面板")
                End If
                Exit For
            Catch

            End Try
        Next
        Try
            Dim i As System.Windows.Forms.HtmlElement
            Dim a As System.Windows.Forms.HtmlElement
            For Each i In webform.WebBrow.Document.GetElementsByTagName("h2")
                For Each a In i.GetElementsByTagName("A")
                    log.Text += vbCrLf + "********************************************" + vbCrLf + a.Name + "=" + webform.WebBrow.Url.ToString + a.GetAttribute("href")
                Next

            Next
        Catch


        End Try


    End Sub



    Sub openweb()



    End Sub


End Class
