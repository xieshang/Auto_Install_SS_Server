''' <summary>
''' 数据类型模块，包含数据类型，及数据类型转换接口
''' </summary>
''' <remarks></remarks>
Module ModuleDataType

    Structure TYPESTU
        Dim type As String
        Dim decode As String
    End Structure


    Dim types() As TYPESTU



    Public Function ModuleDataType_GetTypeList() As String
        Dim result As String = ""


        'Dim stack As New Stack(Of Enum_DataType)
        'Dim popped As Enum_DataType
        'For Each i In [Enum].GetValues(GetType(Enum_DataType))
        '    stack.Push(i)
        '    popped = stack.Pop()
        '    result += popped.ToString
        '    If i + 1 <> stack.Count - 1 Then
        '        result += "|"
        '    End If
        'Next
        If FileIO.FileSystem.FileExists(Application.StartupPath + "/datatype.cfg") = False Then
            Dim path As String = "datatype.cfg" '文件释放路径
            Dim resources As System.Resources.ResourceManager = My.Resources.ResourceManager
            Dim b() As Byte = resources.GetObject("datatype_cfg")
            Dim s As IO.Stream
            Try
                s = IO.File.Create(path)
                s.Write(b, 0, b.Length)
                s.Close()
                'MessageBox.Show("资源释放成功")
            Catch ex As Exception
                'MessageBox.Show("资源释放失败！Result=" + ex.Message)
            End Try
        End If
        Dim str() As String = ReadFile(Application.StartupPath + "/datatype.cfg").Split(vbCrLf)
        ReDim types(str.Length - 1)


        For i = 0 To str.Length - 1
            str(i) = str(i).Replace(vbLf, "")
            types(i).type = str(i).Split(":")(0)
            types(i).decode = str(i).Split(":")(1)
            result += types(i).type + "|"
        Next



        'For Each i In [Enum].GetValues(GetType(Enum_DataType))
        '    result += [Enum].GetName(GetType(Enum_DataType), i) + "|"
        'Next

        Return result
    End Function
    Private Function ZToFSAll(ByVal ZT As Long) As String
        '全存样式
        '从天，时，分，秒整合为秒
        Dim T, S, F, M As Integer '天，时，分，秒
        Dim T1, S1 As Integer '天，时，分，秒
        Dim M1 As String
        T = ZT \ 86400
        T1 = ZT Mod 86400
        S = T1 \ 3600
        S1 = T1 Mod 3600
        F = S1 \ 60
        M = S1 Mod 60
        If M < 10 Then
            M1 = "0" & M
        Else
            M1 = M
        End If
        ZToFSAll = T & "天" & S & "小时" & F & "分钟" & M1 & "秒"
    End Function


    Private Function ZToFSQS(ByVal ZT As Long) As String
        '缺损样式
        '从秒分解为天，时，分，秒
        Dim T, S, F, M As Integer '天，时，分，秒
        Dim T1, S1 As Integer '天，时，分，秒
        Dim T2, S2, F2, M2 As String '天，时，分，秒
        T = ZT \ 86400
        T1 = ZT Mod 86400
        S = T1 \ 3600
        S1 = T1 Mod 3600
        F = S1 \ 60
        M = S1 Mod 60
        If T = 0 Then
            T2 = ""
        Else
            T2 = T & "天"
        End If
        If S = 0 Then
            S2 = ""
        Else
            S2 = S & "小时"
        End If
        If F = 0 Then
            F2 = ""
        Else
            F2 = F & "分钟"
        End If
        M2 = M & "秒"
        ZToFSQS = T2 & S2 & F2 & M2
    End Function
    Private Function FSoZT(ByVal T As Integer, ByVal S As Int16, ByVal F As Int16, ByVal M As Int16) As Long
        '从天，时，分，秒分解为秒
        'T， S， F， M分别为天，时，分，秒
        FSoZT = T * 86400 + S * 3600 + F * 60 + M
    End Function
    Private Function MsTohhmmss(ByVal str As String)
        MsTohhmmss = ""


    End Function
    ''' <summary>
    ''' 倒置
    ''' </summary>
    ''' <param name="str">源数据</param>
    ''' <param name="cmd">TRUE:加密；FALSE:解密；</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function roll(ByVal str As String, Optional ByVal cmd As Boolean = True) As String
        roll = ""
        If str.Length Mod 2 Then
            MsgBox("roll()-输入数据必须为2的整数倍！")
            logger.Error("roll()-" + "字符串长度错误" + str)
        End If
        For i = 0 To (str.Length / 2) - 1
            roll = Mid(str, i * 2 + 1, 2) + roll
        Next
    End Function

    ''' <summary>
    ''' 字符串转HEX
    ''' </summary>
    ''' <param name="str">源数据</param>
    ''' <param name="cmd">TRUE:加密；FALSE:解密；</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function strtohex(ByVal str As String, Optional ByVal cmd As Boolean = True) As String
        strtohex = ""
        If cmd = False Then
            '解
            strtohex = Format(CLng(str), "X2")
            If strtohex.Length Mod 2 <> 0 Then
                strtohex = "0" + strtohex
            End If
            strtohex = roll(strtohex, False)
        Else
            '加
            If str.Length Mod 2 Then
                MsgBox("strtohex()-输入数据必须为2的整数倍！")
                logger.Error("roll()-" + "字符串长度错误" + str)
            End If
            str = roll(str)

            Dim i As Integer
            For i = 0 To str.Length - 1
                If Mid(str, i + 1, 1) <> "0" Then
                    Exit For
                End If
            Next
            Dim t As String = Mid(str, i + 1, str.Length - i)
            If t.Length > 10 Then
                MsgBox("数据超出64，不予以解析。")
                Return str
            End If
            strtohex = CLng("&h" + str)
        End If

    End Function

    ''' <summary>
    ''' 字符串转IP
    ''' 例如：C0(192) A8(168) 0(0) 1(0)
    ''' 转换为(字符串)192.168.0.1
    ''' </summary>
    ''' <param name="str">源数据</param>
    ''' <param name="cmd">TRUE:加密；FALSE:解密；</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function strtoip(ByVal str As String, Optional ByVal cmd As Boolean = True) As String
        strtoip = ""
        If str.Length <> 4 Then
            logger.Error("strtoip()-" + "字符串长度错误" + str)
            MsgBox("strtoip()-IP地址长度不正确！")
            Return str
        End If
        strtoip = CLng("&h" + Mid(str, 1, 2)) + "." + CLng("&h" + Mid(str, 3, 2)) + "." + CLng("&h" + Mid(str, 5, 2)) + "." + CLng("&h" + Mid(str, 7, 2))
    End Function


    Public Function FindTypeDecode(ByVal s As String) As String
        FindTypeDecode = ""

        For i = 0 To types.Length - 1
            If s = types(i).type Then Return types(i).decode
        Next
        Return ""
    End Function



    ''' <summary>
    ''' 内存转换为指定格式数据
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="t"></param>
    ''' <remarks></remarks>
    Public Function ModuleDataType_MemeryToAny(ByVal data As String, ByVal t As String) As String
        Dim decodes() As String = FindTypeDecode(t).Split("|")
        If decodes.Length = 1 And decodes(0) = "" Then Return data

        Dim str As String = data

        For i = 0 To decodes.Length - 1
            Select Case decodes(i)
                Case "bytetobcd"
                Case "roll"
                    str = roll(str)
                Case "hex"
                    str = strtohex(str)
                Case "MsTohhmmss"
                    str = ZToFSQS(str / 1000)
                Case "STohhmmss"
                    str = ZToFSQS(str)
                Case "IP"
                    str = strtoip(str)



            End Select


        Next

        Return str
    End Function


    Public Function ModuleDataType_AnyToMemery(ByVal str As String, ByVal t As String) As String
        Dim decodes() As String = FindTypeDecode(t).Split("|")
        If decodes.Length = 1 And decodes(0) = "" Then Return str
        
        For i = 0 To decodes.Length - 1
            Select Case decodes(decodes.Length - 1 - i)
                Case "bytetobcd"
                Case "roll"
                    str = roll(str, False)
                Case "hex"
                    str = strtohex(str, False)
                Case "MsTohhmmss"
                    str = ZToFSQS(str / 1000)
                Case "STohhmmss"
                    str = ZToFSQS(str)
                Case "IP"
                    str = strtoip(str, False)

            End Select
        Next

        Return str


    End Function


End Module
