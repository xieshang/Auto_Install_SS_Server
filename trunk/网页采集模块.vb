Imports System.Threading

Module 网页采集模块
    Dim wbrow As WebBrowser
    Dim dgv As DataGridView
    Dim webhtml As String = ""
    Dim webresult As String = ""



    Public Sub Mweb_Reg(ByVal wb As WebBrowser)
        wbrow = wb
        wbrow.Navigate("about:blank")
    End Sub



    ''' <summary>
    ''' 运行采集指令
    ''' </summary>
    ''' <param name="cmdstr">指令名称</param>
    ''' <param name="result">采集结果</param>
    ''' <returns>执行结果</returns>
    ''' <remarks></remarks>
    Public Function Mweb_RunCmd(ByVal cmdstr As String, ByRef result As String) As Boolean
        Mweb_RunCmd = True
        Dim config() As String
        Dim cmd As String = cmdstr.Split("|")(0).ToUpper
        If cmdstr.Split("|").Length > 1 Then
            config = cmdstr.Split("|")(1).Split(",")
        End If

        Try


            Select Case cmd
                Case "OpenSite".ToUpper                         '打开新网页
                    Mweb_RunCmd = Mweb_OpenSite(config(0))

                Case "Html_SetPoint".ToUpper                    '设置开始采集点
                    Mweb_RunCmd = Mweb_Html_SetPoint(config(0))

                Case "Html_GetBetween".ToUpper                  '获取关键词之间的内容
                    webresult = Mweb_Html_GetBetween(config(0), config(1))






                Case "DGV_AddLine".ToUpper
                    DGV_AddRow()

                Case "DGV_CellSet".ToUpper
                    Mweb_RunCmd = DGV_CellSet(config(0))
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
            wbrow.Navigate(url)
            sleepdo(1000)
            While wbrow.IsBusy
                Thread.Sleep(10)
                Application.DoEvents()
            End While
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function Mweb_Html_SetPoint(ByVal str As String) As Boolean
        If webhtml = "" Or webhtml = Nothing Then
            webhtml = wbrow.DocumentText
        End If
        If webhtml.IndexOf(str) >= 0 Then
            webhtml = Mid(webhtml, webhtml.IndexOf(str) + str.Length + 1)
            Return True
        End If
        Return False
    End Function

    Public Function Mweb_Html_GetBetween(ByVal str_start As String, ByVal str_end As String) As String
        If webhtml = "" Or webhtml = Nothing Then
            webhtml = wbrow.DocumentText
        End If
        If webhtml = "" Then
            Return ""
        End If
        Mweb_Html_SetPoint(str_start)
        Dim len As Long = webhtml.IndexOf(str_end)
        Mweb_Html_GetBetween = Mid(webhtml, 1, len)

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


    Public Sub DGV_AddRow()
        dgv.Rows.Add()
    End Sub

    Public Function DGV_CellSet(ByVal index As String) As Boolean
        Dim dex As Integer = index
        DGV_CellSet = True
        Try
            dgv.Rows(dgv.Rows.Count - 1).Cells(dex - 1).Value = webresult
        Catch ex As Exception
            DGV_CellSet = False
        End Try

    End Function

#End Region



#Region "通用"

    Sub sleepdo(ByVal t As Long)

        For i = 0 To t - 1
            Thread.Sleep(1)
            Application.DoEvents()
        Next
    End Sub
#End Region

End Module

