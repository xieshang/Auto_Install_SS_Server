Imports System.Threading

Module 网页采集模块
    Public Delegate Sub Dgt_none()
    Public Delegate Sub Dgt_int(ByVal t As Integer)
    Public Delegate Function Dgt_r_str(ByVal t As String) As Boolean
    Public Delegate Function Dgt_r_str2(ByVal t1 As String, ByVal t2 As String) As Boolean


    Dim wbrow As WebBrowser
    Dim dgv As DataGridView
    Dim webhtml As String = ""
    Dim webresult As String = ""
    Dim ProgBar As New ProgressBar
    Dim webThread As Thread


    Public Sub Mweb_Reg(ByVal wb As WebBrowser, Optional ByVal bar As ProgressBar = Nothing)
        wbrow = wb
        ProgBar = bar
    End Sub








    Public Function Mweb_ThreadIsAlive() As Boolean
        Mweb_ThreadIsAlive = webThread.IsAlive
    End Function



    ''' <summary>
    ''' 运行采集指令
    ''' </summary>
    ''' <param name="cmdstr">指令名称</param>
    ''' <param name="result">采集结果</param>
    ''' <returns>执行结果</returns>
    ''' <remarks></remarks>
    Public Function Mweb_RunCmd(ByVal cmdstr As String, ByRef result As String) As Boolean
        Mweb_RunCmd = True
        Dim config(5) As String
        Dim cmd As String = cmdstr.Split("|")(0).ToUpper
        If cmdstr.Split("|").Length > 1 Then
            config = cmdstr.Split("|")(1).Split(",")
        End If

        Try

            LogDebug("******************* " + cmdstr + " *******************")

            Select Case cmd
                Case "OpenSite".ToUpper                         '打开新网页
                    Mweb_RunCmd = Mweb_OpenSite(config(0))
                    'Mweb_RunCmd = ProgBar.Invoke(New Dgt_r_str(AddressOf Mweb_OpenSite), config(0))

                Case "Html_SetPoint".ToUpper                    '设置开始采集点
                    Mweb_RunCmd = Mweb_Html_SetPoint(config(0))
                    'Mweb_RunCmd = ProgBar.Invoke(New Dgt_r_str(AddressOf Mweb_Html_SetPoint), config(0))

                Case "Html_GetBetween".ToUpper                  '获取关键词之间的内容
                    webresult = Mweb_Html_GetBetween(config(0), config(1))
                    'Mweb_RunCmd = ProgBar.Invoke(New Dgt_r_str2(AddressOf Mweb_Html_GetBetween), config(0), config(1))





                Case "DGV_AddLine".ToUpper
                    DGV_AddRow()
                    'Mweb_RunCmd = ProgBar.Invoke(New Dgt_int(AddressOf DGV_AddRow))

                Case "DGV_CellSet".ToUpper
                    Mweb_RunCmd = DGV_CellSet(config(0))
                    'Mweb_RunCmd = ProgBar.Invoke(New Dgt_r_str(AddressOf DGV_CellSet), config(0))
            End Select


        Catch ex As Exception

        End Try
    End Function

    Public Sub Mweb_free()
        webhtml = ""
    End Sub


#Region "HTML指令"
    Public Function Mweb_ClassName(ByVal result As String, ByVal heard As String, Optional ByVal name As String = "", Optional ByVal id As String = "") As Boolean
        If webhtml = "" Or webhtml = Nothing Then
            webhtml = wbrow.DocumentText
        End If
        Mweb_ClassName = False

        Dim i As System.Windows.Forms.HtmlElement
        For Each i In wbrow.Document.GetElementsByTagName(heard)
            If (name <> "" And i.Name = name) Or (id <> "" And i.Id = id) Then
                result = i.OuterText
                Return True
            End If
        Next
    End Function

    Public Function Mweb_OpenSite(ByVal url As String) As Boolean
        Try
            LogDebug("尝试打开网页:" + url)
            webhtml = ""
            pageready = False
            wbrow.Navigate(url)
            'sleepdo(3000)
            ''Thread.Sleep(500)
            'While pageready = False
            '    Thread.Sleep(10)
            '    Application.DoEvents()
            'End While
            WaitForPageLoad()

            LogDebug("【打开成功】")
            webhtml = wbrow.DocumentText
            Return True
        Catch ex As Exception
            LogDebug("【打开失败】")
            Return False
        End Try


    End Function

    Public Function Mweb_Html_SetPoint(ByVal str As String) As Boolean
        If webhtml = "" Or webhtml = Nothing Then
            webhtml = wbrow.DocumentText
        End If
        LogDebug("设置HTML缓存起始点:" + str)
        If webhtml.IndexOf(str) >= 0 Then
            webhtml = Mid(webhtml, webhtml.IndexOf(str) + str.Length + 1)
            LogDebug("【找到起始点】")
            Return True
        End If
        LogDebug("【未找到起始点】")
        Return False
    End Function

    Public Function Mweb_Html_GetBetween(ByVal str_start As String, ByVal str_end As String) As String
        LogDebug("采集内容 介于:" + str_start + "_" + str_end)
        If webhtml = "" Or webhtml = Nothing Then
            webhtml = wbrow.DocumentText
        End If
        If webhtml = "" Then
            Return ""
        End If
        Mweb_Html_SetPoint(str_start)
        Dim len As Long = webhtml.IndexOf(str_end)
        Mweb_Html_GetBetween = Mid(webhtml, 1, len)
        LogDebug("【截取内容】:" + Mweb_Html_GetBetween)
        Mweb_Html_SetPoint(str_end)
    End Function

    Public Function Mweb_Html_GetHtml() As String
        Return webhtml
    End Function

#End Region


#Region "表格"


    Public Sub DGV_Reg(ByVal ndgv As DataGridView)
        dgv = ndgv
    End Sub

    Public Sub DGV_Clear()
        Try
            dgv.Rows.Clear()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub DGV_AddRow()
        Try
            dgv.Rows.Add()
        Catch ex As Exception

        End Try
    End Sub

    Public Function DGV_CellSet(ByVal index As String) As Boolean
        Dim dex As Integer = index
        DGV_CellSet = True
        Try
            dgv.Rows(dgv.Rows.Count - 1).Cells(dex - 1).Value = webresult
        Catch ex As Exception
            DGV_CellSet = False
        End Try
        If webresult = "rc4-md5" Then
            Thread.Sleep(1)
        End If
        webresult = ""

    End Function

#End Region



#Region "通用"

    Sub sleepdo(ByVal t As Long)

        For i = 0 To t - 1
            Thread.Sleep(1)
            'Application.DoEvents()
        Next
    End Sub
#End Region

#Region "接口"
    Sub UpdateProgBar(ByVal t As Integer)
        ProgBar.Value = t
    End Sub
#End Region



    Function FindHtmlElement(ByVal FindText As String, ByVal doc As HtmlDocument, ByVal cTagName As String, ByVal cGetAttribute As String, Optional ByVal StrictMatching As Boolean = False) As HtmlElement
        'cTagName：检索具有指定 html 标记的元素，标记需要输入完整的，缺省时查找所有。
        '例如：<input class="button" type="submit" value=提交 style="cursor:hand">，不能只输入"i"，需要输入"input"
        'cGetAttribute ：比较的属性类型，取值为：Id、InnerText、Name、title、classname、value、
        'Id、InnerText可以通过GetAttribute获取，也可以通过HtmlElement.Id、HtmlElement.InnerText获取，所以代码简化为用GetAttribute获取。
        'doc：Webbrowser1.Document
        'GetAttribute("classname")   '例如显示class="commonTable"的值commonTable
        'StrictMatching：True严格匹配FindText

        Dim i, k As Integer

        FindHtmlElement = Nothing

        For i = 0 To doc.All.Count - 1
            If InStr(doc.All.Item(i).GetAttribute(cGetAttribute), FindText) > 0 _
                And (Not StrictMatching Or InStr(FindText, doc.All.Item(i).GetAttribute(cGetAttribute)) > 0) And (cTagName = "" Or LCase(cTagName) = LCase(doc.All.Item(i).TagName)) Then
                FindHtmlElement = doc.All.Item(i)
                Exit Function
            End If
        Next

        For k = 0 To doc.Window.Frames.Count - 1
            For i = 0 To doc.Window.Frames.Item(k).Document.All.Count - 1
                If InStr(doc.Window.Frames.Item(k).Document.All.Item(i).GetAttribute(cGetAttribute), FindText) > 0 And (Not StrictMatching Or InStr(FindText, doc.Window.Frames.Item(k).Document.All.Item(i).GetAttribute(cGetAttribute)) > 0) And (cTagName = "" Or LCase(cTagName) = LCase(doc.Window.Frames.Item(k).Document.All.Item(i).TagName)) Then
                    FindHtmlElement = doc.Window.Frames.Item(k).Document.All.Item(i)
                    Exit Function
                End If
            Next

            '递归调用
            If FindHtmlElement Is Nothing Then
                FindHtmlElement = FindHtmlElement(FindText, doc.Window.Frames.Item(k).Document, cTagName, cGetAttribute, StrictMatching)
            End If
        Next

    End Function


#Region "Page Loading Functions"
    Private Property pageready As Boolean = False
    Public Sub WaitForPageLoad(Optional ByVal str As String = "")
        AddHandler wbrow.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        While Not pageready
            Application.DoEvents()
        End While
        '等待动态加载完成
        If str <> "" Then
            While wbrow.Document.GetElementsByTagName(str).Count = 0
                Application.DoEvents()
            End While
        End If
        pageready = False
    End Sub

    Private Sub PageWaiter(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        If wbrow.ReadyState = WebBrowserReadyState.Complete Then
            pageready = True
            RemoveHandler wbrow.DocumentCompleted, New WebBrowserDocumentCompletedEventHandler(AddressOf PageWaiter)
        End If
    End Sub

#End Region
End Module

