﻿Imports System.Threading

Public Class Form1

    Public Declare Function DeleteUrlCacheEntry Lib "wininet.dll" Alias "DeleteUrlCacheEntryA" (ByVal lpszUrlName As String) As Long

    Enum ShowCommands
        SW_HIDE = 0
        SW_SHOWNORMAL = 1
        SW_NORMAL = 1
        SW_SHOWMINIMIZED = 2
        SW_SHOWMAXIMIZED = 3
        SW_MAXIMIZE = 3
        SW_SHOWNOACTIVATE = 4
        SW_SHOW = 5
        SW_MINIMIZE = 6
        SW_SHOWMINNOACTIVE = 7
        SW_SHOWNA = 8
        SW_RESTORE = 9
        SW_SHOWDEFAULT = 10
        SW_FORCEMINIMIZE = 11
        SW_MAX = 11
    End Enum





    Dim webform As New web

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Top = 1
        webform.Hide()
        Mweb_Reg(webform.WebBrow)
        DGV_Reg(ssr_dgv)
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            webform.Close()
        Catch ex As Exception

        End Try
    End Sub




    Private Sub login_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles login.Click


        webform.WebBrow.Navigate("https://app.arukas.io")
        sleepdo(1000)
        While webform.WebBrow.IsBusy
            Application.DoEvents()
        End While

        '判断所处界面
        Try
            Dim i As System.Windows.Forms.HtmlElement
            For Each i In webform.WebBrow.Document.GetElementsByTagName("h2")
                If i.OuterText = "Login" Then
                    'MsgBox("非控制面板")
                    webform.Show()
                    MsgBox("请重新登录，然后再点击更新信息。")
                    Exit Sub
                ElseIf i.OuterText = "Apps" Then
                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try


        '查找信息
        Try
            Dim i As System.Windows.Forms.HtmlElement
            Dim a As System.Windows.Forms.HtmlElement
            For Each i In webform.WebBrow.Document.GetElementsByTagName("h2")
                For Each a In i.GetElementsByTagName("A")
                    log.Text += vbCrLf + "*********************" + vbCrLf + a.OuterText + "=" + Mid(webform.WebBrow.Url.ToString, 1, webform.WebBrow.Url.ToString.Length - 1) + a.GetAttribute("href")
                    DGV.Rows.Add()
                    DGV.Rows(DGV.Rows.Count - 1).Cells(0).Value = a.OuterText
                Next

            Next
        Catch


        End Try
    End Sub

    Dim webthread As Thread

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles reg.Click

        System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 2")
        Try
            webform.Close()

        Catch ex As Exception

        End Try
        webform = New web
        webform.Show()
        webform.WebBrow.Navigate("https://app.arukas.io/sign_up")
    End Sub



    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim str(100) As String
        Dim result As String = ""
        Dim i As Integer = 0
        Dim status As Boolean = False

        str(i) = "OpenSite|https://www.dou-bi.co/sszhfx/"
        i += 1
        str(i) = "While"
        i += 1
        str(i) = "Html_SetPoint|<td width=""18%"">"
        i += 1
        str(i) = "DGV_AddLine"
        i += 1
        str(i) = "Html_GetBetween|<strong>,</strong>"
        i += 1
        str(i) = "DGV_CellSet|5"
        i += 1
        str(i) = "Html_GetBetween|<td width=""15%"">,</td>"
        i += 1
        str(i) = "DGV_CellSet|1"
        i += 1
        str(i) = "Html_GetBetween|"">,</td>"
        i += 1
        str(i) = "DGV_CellSet|2"
        i += 1
        str(i) = "Html_GetBetween|"">,</td>"
        i += 1
        str(i) = "DGV_CellSet|3"
        i += 1
        str(i) = "Html_GetBetween|"">,</td>"
        i += 1
        str(i) = "DGV_CellSet|4"
        i += 1
        str(i) = "EndWhile"




        ProgressBar.Value = 0
        ProgressBar.Minimum = 0
        ProgressBar.Maximum = i
        For j = 0 To i
            status = Mweb_RunCmd(str(j), result)
            If status = False Then
                Exit Sub
            End If
            ProgressBar.Value = j
            Application.DoEvents()
        Next

    End Sub
End Class
