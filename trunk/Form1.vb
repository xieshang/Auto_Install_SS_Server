Imports System.Threading

Public Class Form1
    Public Delegate Sub Dgt_str(ByVal t As String)

    Dim webform As New web
    Dim mythread As Thread


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






    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Top = 1
        webform.Show()
        webform.Hide()
        Mweb_Reg(webform.WebBrow, ProBar)
        DGV_Reg(ssr_dgv)
        'Control.CheckForIllegalCrossThreadCalls = False '允许多线程调用控件

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            webform.Close()
        Catch ex As Exception

        End Try
    End Sub




    Private Sub login_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles login.Click


        webform.WebBrow.Navigate("https://app.arukas.io")
        WaitForPageLoad()

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


#Region "采集"
    Private Sub DataGridView1_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DataGridView1.DragEnter
        e.Effect = DragDropEffects.Link '接受拖放数据，启用拖放效果
    End Sub

    Private Sub DataGridView1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DataGridView1.DragDrop
        Dim tdgv As DataGridView = sender
        Dim filename As String = e.Data.GetData(DataFormats.FileDrop)(0)
        tdgv.Rows.Add(filename, "")
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunRules.Click

        DGV_Clear()
        mythread = New Thread(AddressOf startthread)
        mythread.Start()
    End Sub

#End Region

















    Sub startthread()
        Dim str As String
        For i = 0 To DataGridView1.Rows.Count - 1
            str = ReadFile(DataGridView1.Rows(i).Cells(0).Value)
            Me.BeginInvoke(New Dgt_str(AddressOf RunCmd), str)
        Next
        mythread.Abort()
    End Sub



    Public Sub RunCmd(ByVal cmd As String)

        Dim cmdlist() As String = cmd.Split(vbCrLf)
        Dim i As Integer = 0
        For i = 0 To cmdlist.Length - 1
            cmdlist(i) = Trim(cmdlist(i).Replace(vbCr, "").Replace(vbLf, ""))
        Next
        'cmdlist(i) = "OpenSite|https://www.dou-bi.co/sszhfx/"
        'i += 1
        'cmdlist(i) = "For|0"
        'i += 1
        'cmdlist(i) = "Html_SetPoint|<td width=""18%"">"
        'i += 1
        'cmdlist(i) = "DGV_AddLine"
        'i += 1
        'cmdlist(i) = "Html_GetBetween|<strong>,</strong>"
        'i += 1
        'cmdlist(i) = "DGV_CellSet|5"
        'i += 1
        'cmdlist(i) = "Html_GetBetween|<td width=""15%"">,</td>"
        'i += 1
        'cmdlist(i) = "DGV_CellSet|1"
        'i += 1
        'cmdlist(i) = "Html_GetBetween|"">,</td>"
        'i += 1
        'cmdlist(i) = "DGV_CellSet|2"
        'i += 1
        'cmdlist(i) = "Html_GetBetween|"">,</td>"
        'i += 1
        'cmdlist(i) = "DGV_CellSet|3"
        'i += 1
        'cmdlist(i) = "Html_GetBetween|"">,</td>"
        'i += 1
        'cmdlist(i) = "DGV_CellSet|4"
        'i += 1
        'cmdlist(i) = "EndFor"
        'ReDim Preserve cmdlist(i)

        If ProBar IsNot Nothing Then
            ProBar.Value = 0
            ProBar.Minimum = 0
            ProBar.Maximum = i
        End If
        i = cmdlist.Length
        Dim result As String = ""
        Dim forstart, forend, trytime As Integer
        Dim status As Boolean = False

        For j = 0 To i
            If cmdlist(j).Split("|")(0).ToUpper = "For".ToUpper Then
                forstart = j
                trytime = cmdlist(j).Split("|")(1)
            Else
                status = Mweb_RunCmd(cmdlist(j), result)
                If status = False Then
                    ProBar.Value = ProBar.Maximum
                    Exit Sub
                End If
                If ProBar IsNot Nothing Then
                    ProBar.Value = j
                End If
                If forstart <> 0 Then
                    If forend = 0 Then
                        If cmdlist(j).Split("|")(0).ToUpper = "EndFor".ToUpper Then
                            forend = j
                            j = forstart
                        End If
                    ElseIf j = forend - 1 Then
                        If trytime <> 0 Then
                            If trytime > 1 Then
                                trytime -= 1
                            Else
                                forstart = 0
                                j = forend + 1
                            End If
                        Else
                            j = forstart
                        End If
                    End If
                End If
            End If

            Application.DoEvents()
        Next
    End Sub

End Class
