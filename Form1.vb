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
        loadfilelist(DataGridView1, Application.StartupPath + "\rules")
        加密.SelectedIndex = 2
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            webform.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click_2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        saveSSRconfig()
    End Sub

#Region ""

    Private Sub login_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles login.Click
        Dim i As System.Windows.Forms.HtmlElement
        Dim a As System.Windows.Forms.HtmlElement

        DGV.Rows.Clear()
        webform.WebBrow.Navigate("https://app.arukas.io")
        WaitForPageLoad("div")



        '判断所处界面
        Try
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
            For Each i In webform.WebBrow.Document.GetElementsByTagName("h2")

                For Each a In i.GetElementsByTagName("A")

                    DGV.Rows.Add()
                    DGV.Rows(DGV.Rows.Count - 1).Cells(0).Value = a.OuterText
                    DGV.Rows(DGV.Rows.Count - 1).Cells(1).Value = a.GetAttribute("href")

                Next

            Next
        Catch


        End Try

        ''读取app页面信息
        For k = 0 To DGV.Rows.Count - 1
            Dim url As String = "https://app.arukas.io" + DGV.Rows(k).Cells(1).Value
            webform.WebBrow.Navigate(url)
            WaitForPageLoad("ul")
            For Each i In webform.WebBrow.Document.GetElementsByTagName("ul")
                If i.GetAttribute("classname") = "list-unstyled c-list-compact" Then
                    Dim item(0) As String
                    Dim rep_str As String = Mid(i.OuterHtml, i.OuterHtml.IndexOf("<li>"))
                    While rep_str.IndexOf("href=") > 0
                        item(item.Length - 1) = Mid(rep_str, rep_str.IndexOf(""">http://") + """>http://".Length + 1, rep_str.IndexOf("/tcp") - rep_str.IndexOf(""">http://") - "/tcp)".Length - 4)
                        item(item.Length - 1) = item(item.Length - 1).Replace(vbCrLf, "").Replace(vbLf, "").Replace("</a>", "").Replace(" ", "").Replace("<span>", "").Replace(")", "")
                        rep_str = Mid(rep_str, rep_str.IndexOf("</li>") + "</li>".Length)
                        Dim port As String = item(item.Length - 1).Split("(")(1)
                        If port <> "80" And port <> "22" And port <> "21" Then
                            log.Text += vbCrLf + item(item.Length - 1)
                            DGV_SSR.Rows.Add()
                            DGV_SSR.Rows(DGV_SSR.Rows.Count - 1).Cells(0).Value = item(item.Length - 1).Split(":")(0)
                            DGV_SSR.Rows(DGV_SSR.Rows.Count - 1).Cells(1).Value = item(item.Length - 1).Split(":")(1).Split("(")(0)
                            DGV_SSR.Rows(DGV_SSR.Rows.Count - 1).Cells(2).Value = item(item.Length - 1).Split(":")(1).Split("(")(1)

                        End If

                        ReDim Preserve item(item.Length)
                    End While

                End If
            Next
        Next


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





#End Region


#Region "采集"

    Sub loadfilelist(ByVal dg As DataGridView, ByVal dir_addr As String)
        If FileIO.FileSystem.DirectoryExists(dir_addr) = False Then
            IO.Directory.CreateDirectory(dir_addr)
        End If
        For Each f In IO.Directory.GetFiles(dir_addr)
            dg.Rows.Add(f)
        Next
    End Sub


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


    Sub startthread()
        Dim str As String
        Try
            For i = 0 To DataGridView1.Rows.Count - 1
                str = IO.File.ReadAllText(DataGridView1.Rows(i).Cells(0).Value)
                Me.BeginInvoke(New Dgt_str(AddressOf RunCmd), str)
                Thread.Sleep(10000)
                Mweb_free()
            Next
        Catch ex As Exception

        End Try
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
            ProBar.Minimum = 0
            ProBar.Maximum = i
            ProBar.Value = 1
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

#End Region


#Region "SSR"
    Sub saveSSRconfig()
        If FileIO.FileSystem.FileExists(Application.StartupPath + "\gui-config.json") = False Then
            MsgBox("软件未与SSR放在同一目录下")
            Exit Sub
        End If
        Dim cfg As String = FileIO.FileSystem.ReadAllText(Application.StartupPath + "\gui-config.json")
        'FileIO.FileSystem.DeleteFile(Application.StartupPath + "\gui-config.json")
        Dim cfgnew As String = ""
        Dim sercfg As String = ""
        cfgnew = Mid(cfg, 1, cfg.IndexOf("[") + 1)
        cfgnew += Mid(cfg, cfg.IndexOf("]"))

        '保存采集的数据
        If ssr_dgv.RowCount > 0 Then
            For i = 0 To ssr_dgv.RowCount - 1
                sercfg += "{" + vbLf + """remarks"" : """"," + vbLf _
                 + """server"" : """ + ssr_dgv.Rows(i).Cells(0).Value + """," + vbLf _
                 + """server_port"" : " + ssr_dgv.Rows(i).Cells(1).Value + "," + vbLf _
                 + """server_udp_port"" : 0," + vbLf _
                 + """password"" : """ + ssr_dgv.Rows(i).Cells(2).Value + """," + vbLf _
                 + """method"" : """ + ssr_dgv.Rows(i).Cells(3).Value + """," + vbLf _
                 + """obfs"" : ""plain”"," + vbLf _
                 + """obfsparam"" : """"," + vbLf _
                 + """remarks_base64"" : """"," + vbLf _
                 + """group"" : """"," + vbLf _
                 + """udp_over_tcp"" : false," + vbLf _
                 + """protocol"" : ""origin""," + vbLf _
                 + """enable"" : true," + vbLf _
                 + "}"

                If i <> ssr_dgv.RowCount - 1 Then
                    sercfg += ","
                End If
            Next
        End If


        '保存自建服务器数据
        '保存采集的数据
        If DGV_SSR.RowCount > 0 Then
            For i = 0 To DGV_SSR.RowCount - 1
                sercfg += "{" + vbLf + """remarks"" : """"," + vbLf _
                 + """server"" : """ + DGV_SSR.Rows(i).Cells(0).Value + """," + vbLf _
                 + """server_port"" : " + DGV_SSR.Rows(i).Cells(1).Value + "," + vbLf _
                 + """server_udp_port"" : 0," + vbLf _
                 + """password"" : """ + 密码.Text + """," + vbLf _
                 + """method"" : """ + 加密.Text + """," + vbLf _
                 + """obfs"" : ""plain”"," + vbLf _
                 + """obfsparam"" : """"," + vbLf _
                 + """remarks_base64"" : """"," + vbLf _
                 + """group"" : """"," + vbLf _
                 + """udp_over_tcp"" : false," + vbLf _
                 + """protocol"" : ""origin""," + vbLf _
                 + """enable"" : true," + vbLf _
                 + "}"

                If i <> DGV_SSR.RowCount - 1 Then
                    sercfg += ","
                End If
            Next
        End If


        '写入文件
        cfgnew = Mid(cfg, 1, cfg.IndexOf("[") + 1)
        cfgnew += sercfg + Mid(cfg, cfg.IndexOf("],"))

        FileIO.FileSystem.WriteAllText(Application.StartupPath + "\gui-config.json", cfgnew, False)
    End Sub



#End Region

End Class
