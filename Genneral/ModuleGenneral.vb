

Imports System.IO
Imports System.Web
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography

Module ModuleGenneral
    Declare Function GetFileVersionInfo& Lib "Version"  Alias "GetFileVersionInfoA" (ByVal FileName$,   ByVal dwHandle&, ByVal cbBuff&, ByVal lpvData$)
    Declare Sub hmemcpy Lib "Kernel32" Alias "RtlMoveMemory" (ByVal hpvDest As Object, ByVal hpvSource As Object, ByVal cbBytes&)
    Structure FileInfo
        Dim wLength As Integer
        Dim wValueLength As Integer
        Dim szKey() As String

        Dim dwSignature As Long
        Dim dwStrucVersion As Long
        Dim dwFileVersionMS As Long
        Dim dwFileVersionLS As Long
    End Structure
    Function HIWORD(ByVal x As Long) As Integer
        HIWORD = x / &HFFFF&
        ' High 16 bits contain Major revision number.
    End Function
    Function LOWORD(ByVal x As Long) As Integer
        LOWORD = x And &HFFFF&
        ' Low 16 bits contain Minor revision number.
    End Function
    Public Function GetFileVersion(ByVal path As String) As String
        GetFileVersion = ""
        Dim FileVer As String
        Dim dwHandle&, BufSize&, lpvData$, R&
        Dim x As FileInfo
        ReDim x.szKey(15)

        lpvData$ = Space$(BufSize&)
        R& = GetFileVersionInfo(path, dwHandle&, BufSize&, lpvData$)
        hmemcpy(x, lpvData$, Len(x))

        FileVer = Trim$(Str$(HIWORD(x.dwFileVersionMS))) + "."
        FileVer = FileVer + Trim$(Str$(LOWORD(x.dwFileVersionMS))) + "."
        FileVer = FileVer + Trim$(Str$(HIWORD(x.dwFileVersionLS))) + "."
        FileVer = FileVer + Trim$(Str$(LOWORD(x.dwFileVersionLS)))
    End Function




    Private Declare Function SendMessageAsLong Lib "user32" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Const EM_GETLINECOUNT = 186
    Const EM_LINEINDEX = &HBB


    Public introduce_with As Integer = 21


    Dim debuglog As TextBox
    Dim debugfile As String
    Public logger As log4net.ILog

    Private Delegate Sub RecieveRefreshMethodDelegate(ByVal [text] As String) '声明委托
    Private RecieveRefresh As New RecieveRefreshMethodDelegate(AddressOf ModuleGenneral_DebugLogAdd) '定义一个委托实例


    '共享方法：注册文件关联
    Function RegMyFileType(ByVal FileType As String, ByVal ShellName As String, ByVal DefIcon As String) As Boolean
        Try
            '注册文件类型
            My.Computer.Registry.ClassesRoot.CreateSubKey(FileType)
            My.Computer.Registry.SetValue("HKEY_CLASSES_ROOT\" & FileType, String.Empty, ShellName)
            '注册文件类型打开程序
            My.Computer.Registry.ClassesRoot.CreateSubKey(ShellName)
            My.Computer.Registry.SetValue("HKEY_CLASSES_ROOT\" & ShellName, String.Empty, ShellName)
            '文件类型默认图标
            My.Computer.Registry.ClassesRoot.CreateSubKey(ShellName & "\DefaultIcon")
            My.Computer.Registry.SetValue("HKEY_CLASSES_ROOT\" & ShellName & "\DefaultIcon", String.Empty, DefIcon)
            '注册打开方式
            My.Computer.Registry.ClassesRoot.CreateSubKey(ShellName & "\shell\open\command")
            My.Computer.Registry.SetValue("HKEY_CLASSES_ROOT\" & ShellName & "\shell\open\command", String.Empty, My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe " + """" + "%1" + """")
            Return True
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return False
        End Try
    End Function
    '注销文件关联
    Function UnRegMyFileType(ByVal FileType As String, ByVal ExeName As String) As Boolean
        On Error Resume Next
        My.Computer.Registry.ClassesRoot.DeleteSubKeyTree(FileType)
        My.Computer.Registry.ClassesRoot.DeleteSubKeyTree(ExeName)
    End Function

    Public Function Getcurversion(ByVal filepath As String) As String
        Try
            Getcurversion = FileVersionInfo.GetVersionInfo(filepath).FileVersion.ToString
            Return Getcurversion
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Sub TextMouseEnter(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim text As TextBox = sender

        Dim lCount As Integer
        lCount = SendMessageAsLong(text.Handle, EM_GETLINECOUNT, 0, 0)

        text.BringToFront()
        If text.Height <> (lCount - 1) * 12 + introduce_with Then
            text.Height = (lCount - 1) * 12 + introduce_with
        End If

    End Sub

    Public Sub TextMouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim text As TextBox = sender

        text.Height = introduce_with
    End Sub







    Public Function LoadFileNameFromFolder(ByVal vbFilePath As String) As String()
        Dim result(0) As String

        Dim Stack(0) As String, m, i, n, p As Integer, muluming(0) As String, wenjianming(0) As String
        Stack(0) = vbFilePath
        m = 1
        Do While m > 0
            Try
                If My.Computer.FileSystem.DirectoryExists(Stack(m - 1)) Then
                    列目录(Stack(m - 1), muluming, n)
                    列文件(Stack(m - 1), wenjianming, p)
                    m -= 1
                    ReDim Preserve Stack(m + n - 1)
                    For i = 0 To n - 1
                        Stack(m) = muluming(i)
                        m += 1
                    Next
                    For i = 0 To p - 1
                        ReDim Preserve result(result.Length)
                        result(result.Length - 2) = wenjianming(i)
                    Next
                End If
            Catch ex As Exception
            End Try
        Loop
        Return result
    End Function
    Public Sub 列目录(ByVal vbFilePath As String, ByRef muluming() As String, ByRef n As Integer)
        n = 0
        If My.Computer.FileSystem.DirectoryExists(vbFilePath) Then
            '       My.Computer.FileSystem.GetDirectories(vbFilePath)
            Dim i As Integer = 0
            ReDim muluming(i)
            For Each foundDirectory As String In My.Computer.FileSystem.GetDirectories(vbFilePath)
                ReDim Preserve muluming(i)
                muluming(i) = foundDirectory
                n += 1
                i += 1
            Next
        End If
    End Sub
    Public Sub 列文件(ByVal vbFilePath As String, ByRef wenjianming() As String, ByRef n As Integer)
        n = 0
        If My.Computer.FileSystem.DirectoryExists(vbFilePath) Then
            Dim i As Integer = 0
            ReDim wenjianming(i)
            For Each foundfile In My.Computer.FileSystem.GetFiles(vbFilePath, FileIO.SearchOption.SearchTopLevelOnly, "*.*") '获得指定目录下的文件名
                ReDim Preserve wenjianming(i)
                wenjianming(i) = foundfile
                n += 1
                i += 1
            Next
        End If
    End Sub






    Public Sub ModuleGenneral_DebugTextReg(ByVal tbox As TextBox)
        debuglog = tbox

    End Sub

    ''' <summary>
    ''' 输出调试信息
    ''' </summary>
    ''' <param name="s">内容</param>
    ''' <param name="newline">是否换行</param>
    ''' <remarks></remarks>
    Public Sub ModuleGenneral_DebugPrint(ByVal s As String, ByVal newline As Boolean)
        If newline = True Then
            s += vbCrLf
        End If
        debuglog.BeginInvoke(RecieveRefresh, s)
        LogDebug(s)
    End Sub


    Sub ModuleGenneral_DebugLogAdd(ByVal str As String)
        debuglog.Text = "[" + Format(Now(), "H:mm:ss") + "]" + str + debuglog.Text
    End Sub

    ''' <summary>
    ''' hex(字符串)转byte()
    ''' </summary>
    ''' <param name="str">需要转换的hex(字符串)</param>
    ''' <returns>转换后的byte()值</returns>
    ''' <remarks>
    ''' dim result() = ModuleGenneral_HEXtoByte("12345678")
    ''' </remarks>
    Public Function ModuleGenneral_HEXtoByte(ByVal str As String) As Byte()

        Dim rt(1) As Byte
        Dim t As String = ""
        str = str.Replace(" ", "")

        If str.Length Mod 2 = 0 Then
            ReDim rt(str.Length \ 2 - 1)
            For intP = 0 To Len(str) \ 2 - 1
                rt(intP) = Val("&h" + Mid(str, intP * 2 + 1, 2))
            Next intP
        End If

        Return rt
    End Function


    Public Function ModuleGenneral_BytetoHEX(ByVal b() As Byte, Optional ByVal 倒置 As Boolean = False, Optional ByVal offset As Long = 0, Optional ByVal len As Long = 0) As String
        Dim t As Long = 0

        If len = 0 Then
            len = b.Length
        End If

        For i = offset To offset + len - 1
            If 倒置 = True Then
                t += b(len + offset * 2 - 1 - i)
            Else
                t += b(i)
            End If


        Next
        Return t.ToString
    End Function



    Public Function ModuleGenneral_BytetoHEXString(ByVal b() As Byte, Optional ByVal offset As Long = 0, Optional ByVal len As Long = 0) As String
        Dim t As String
        Dim rt As String = ""
        If len = 0 Then
            len = b.Length
        End If
        For i = offset To len - 1
            t = Hex(b(i))
            If b(i) < &H10 Then
                t = "0" + t
            End If
            rt += t + " "
        Next

        Return rt
    End Function


    Public Function ModuleGenneral_ByteAdd(ByVal data() As Byte, ByVal index As Long, ByVal len As Long) As UShort
        Dim sum As Long = 0

        For i = index To len + index - 1
            sum = sum + data(i)
            If sum > &H10000 Then
                sum = sum And &HFFFF
            End If
        Next
        ModuleGenneral_ByteAdd = sum Mod &H10000
        Return ModuleGenneral_ByteAdd
    End Function



    ''' <summary>
    ''' 将字符串加入到DataGridViewComboBoxCell中
    ''' </summary>
    ''' <param name="cb">DataGridViewComboBoxCell</param>
    ''' <param name="s">下拉内容以|间隔</param>
    ''' <remarks></remarks>
    Public Sub ModuleGenneral_ComboBoxCell(ByVal cb As DataGridViewComboBoxCell, ByVal s As String)
        Dim str() As String = s.Split("|")
        cb.DropDownWidth = 50
        cb.Items.Clear()
        For i = 0 To str.Length - 1
            If str(i) <> "" Then
                cb.Items.Add(str(i))
            End If
        Next

    End Sub


    Public Function bytesToString(ByRef byteArr() As Byte, Optional ByRef ArrLen As Integer = 0, Optional ByRef ArrBegin As Integer = 0) As String
        On Error GoTo ErrExcuted
        If ArrLen = 0 Then
            ArrLen = byteArr.Length
        End If
        '确保长度
        System.Diagnostics.Debug.Assert((UBound(byteArr) - LBound(byteArr) - ArrBegin + 1) >= ArrLen, "")

        Dim arrId As Integer
        Dim relStr As String = ""
        Dim tmp As Integer
        For arrId = ArrBegin To ArrBegin + ArrLen - 1
            If byteArr(arrId) > 0 Then

                'change by haoyujie 2004-09-22
                If byteArr(arrId) <= 128 Then '对于普通字符，直接转换
                    relStr = relStr & Chr(byteArr(arrId))

                Else '与下一元素组合起来是一个汉字
                    tmp = 256.0# * byteArr(arrId) + byteArr(arrId + 1)
                    relStr = relStr & Chr(tmp)
                    arrId = arrId + 1
                End If

            Else '如果遇到'\0'
                Exit For
            End If
        Next
        bytesToString = relStr

        Exit Function
ErrExcuted:
        'DefaultErrorExcute()
    End Function

    Public Function ListDirFilesName(ByVal list As ComboBox, ByVal addr As String) As String
        Dim result As String = ""

        If Dir(addr) = "" Then
            Return result
        End If

        Dim d As New System.IO.DirectoryInfo(addr) '这里是你的文件夹路径
        Dim f As System.IO.FileInfo

        list.Items.Clear()
        For Each f In d.GetFiles
            list.Items.Add(f.Name)
        Next

        Return result
    End Function


    Public Function ListDirName(ByVal list As ComboBox, ByVal dir As String) As String
        Dim result As String = ""

        Dim d As New System.IO.DirectoryInfo(dir) '这里是你的文件夹路径
        Dim f As System.IO.DirectoryInfo

        list.Items.Clear()
        For Each f In d.GetDirectories
            list.Items.Add(f.Name)
        Next

        Return result
    End Function


    Public Function ReadFile(ByVal addr As String) As String
        ReadFile = ""
        If Dir(addr) = "" Then

            MsgBox("""" + addr + """" + "文件未找到！")
            Return ReadFile
        End If

        Try

            Dim fp As New System.IO.StreamReader(addr, System.Text.Encoding.GetEncoding("GB2312"))


            ReadFile = fp.ReadToEnd
            fp.Close()
        Catch ex As Exception
            MsgBox("打开文件:" + addr + "出错！")
        End Try

        Return ReadFile
    End Function


    Public Function WriteFile(ByVal addr As String, ByVal txt As String) As Boolean
        Dim fp As New System.IO.StreamWriter(addr, False, System.Text.Encoding.GetEncoding("GB2312"))
        Try
            fp.WriteLine(txt)
            fp.Close()

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function


    Public Function FindKeyword(ByVal str() As String, ByVal key As String) As String
        FindKeyword = ""

        For i = 0 To str.Length - 1
            If str(i).IndexOf(key) < 0 Then
            Else
                FindKeyword = Mid(str(i), str(i).IndexOf(key) + key.Length + 1, str(i).Length - str(i).IndexOf(key) + key.Length)
                Return FindKeyword
            End If
        Next
    End Function



    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32
    Private Declare Function GetPrivateProfileSection Lib "kernel32" Alias "GetPrivateProfileSectionA" (ByVal lpAppName As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    'Private Declare Function GetPrivateProfileSectionNames Lib "kernel32.dll" Alias "GetPrivateProfileSectionNamesA" (ByVal lpReturnedString As StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    <DllImport("kernel32.dll")> _
    Function GetPrivateProfileSectionNames( _
    ByVal lpszReturnBuffer As IntPtr, ByVal nSize As System.Int32, ByVal lpFileName As String) As System.Int32
    End Function







    Public Function GetINI(ByVal Section As String, ByVal AppName As String, ByVal iniFilePath As String, Optional ByVal lpDefault As String = "") As String
        Dim Str As String = ""
        Str = LSet(Str, 256)
        GetPrivateProfileString(Section, AppName, lpDefault, Str, Len(Str), iniFilePath)
        Str = Microsoft.VisualBasic.Left(Str, InStr(Str, Chr(0)) - 1)
        Str = Str.Replace("\n", vbCrLf)
        Return Str
    End Function

    ''' <summary>
    ''' 获取ini文件指定父框架内所有的子关键词及参数
    ''' </summary>
    ''' <param name="strSection">父框架名称</param>
    ''' <param name="strIniFile">文件路径</param>
    ''' <returns>返回以 N=D的方式</returns>
    ''' <remarks></remarks>
    Public Function GetInfoSection(ByVal strSection As String, ByVal strIniFile As String) As String()
        Dim strReturn As String = New String("   ", 1000)
        Dim strTmp As String
        Dim nStart As Integer, nEnd As Integer
        Dim sArray() As String
        sArray = New String() {"", ""}

        Call GetPrivateProfileSection(strSection, strReturn, Len(strReturn), strIniFile)

        strTmp = strReturn
        Dim k As Integer = 0
        Do While strTmp <> ""
            nStart = nEnd + 1
            nEnd = InStr(nStart, strReturn, vbNullChar)
            strTmp = Mid$(strReturn, nStart, nEnd - nStart)
            If Len(strTmp) > 0 Then
                sArray(k) = strTmp
                sArray(k) = sArray(k).Replace("\n", vbCrLf)
                ReDim Preserve sArray(UBound(sArray) + 1)
                k = k + 1
            End If
        Loop

        Dim i As Integer = 0

        While i <> sArray.Length
            If sArray(i) Is Nothing Then
                i = 0
                ReDim Preserve sArray(UBound(sArray) - 1)
            Else
                i += 1
            End If
        End While

        GetInfoSection = sArray
    End Function


    ''' <summary>
    ''' 获取ini所有的Section
    ''' </summary>
    ''' <param name="strIniFile">文件路径</param>
    ''' <returns>返回以 N=D的方式</returns>
    ''' <remarks></remarks>
    Public Function GetInfoSectionNames(ByVal strIniFile As String) As String()

        Dim ptr As IntPtr = Marshal.StringToHGlobalAnsi(New String(vbNullChar, 1024))
        Dim len As Int32 = GetPrivateProfileSectionNames(ptr, 1024, strIniFile)
        Dim buff As String = Marshal.PtrToStringAnsi(ptr, len)
        Marshal.FreeHGlobal(ptr)
        Dim s As String() = Split(buff, vbNullChar)
        ReDim Preserve s(s.Length - 1)
        Return s
    End Function


    Public Function SetINI(ByVal iniFilePath As String, ByVal Section As String, ByVal AppName As String, ByVal lpString As String) As Boolean
        Try
            WritePrivateProfileString(Section, AppName, lpString, iniFilePath)
        Catch exception1 As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function RunCMD(ByVal Commands As String, Optional ByVal TimeOutSencond As Integer = 3 * 60) As String '执行命令行函数
        Dim myProcess As New Process()
        Dim myProcessStartInfo As New ProcessStartInfo("cmd.exe")
        LogInfo("cmd运行:" + Commands)
        myProcessStartInfo.UseShellExecute = False
        myProcessStartInfo.RedirectStandardOutput = True
        myProcessStartInfo.CreateNoWindow = True
        myProcessStartInfo.Arguments = "/c " & Commands
        myProcess.StartInfo = myProcessStartInfo
        myProcess.Start()
        myProcess.WaitForExit(TimeOutSencond * 1000)
        If myProcess.HasExited = False Then
            myProcess.Kill()
            myProcess.Close()
            LogInfo(vbCrLf + "cmd运行结果:超时")
            Return "超时"
        End If
        Dim myStreamReader As IO.StreamReader = myProcess.StandardOutput
        Dim myString As String = myStreamReader.ReadToEnd()
        myProcess.Close()
        LogInfo(vbCrLf + "cmd运行结果:" + myString)
        Return myString
    End Function



    Private _process As Process = Nothing
    Private callbacklog As TextBox

    Public Sub RunCMD_CallBack(ByVal cmd As String, ByRef pback As TextBox)
        pback.HideSelection = False
        callbacklog = pback

        Dim psi As New ProcessStartInfo()
        psi.FileName = "cmd.exe"
        psi.Arguments = " /c " + cmd
        psi.UseShellExecute = False
        psi.RedirectStandardOutput = True
        psi.CreateNoWindow = True
        _process = New Process()
        _process.StartInfo = psi
        ' 定义接收消息的Handler
        AddHandler _process.OutputDataReceived, New DataReceivedEventHandler(AddressOf Process1_OutputDataReceived)
        _process.Start()
        ' 开始接收
        _process.BeginOutputReadLine()
    End Sub


    Private Delegate Sub AddMessageHandler(ByVal msg As String)
    Private Sub Process1_OutputDataReceived(ByVal sender As Object, ByVal e As System.Diagnostics.DataReceivedEventArgs)
        Dim handler As AddMessageHandler = Function(msg As String)
                                               callbacklog.Text += msg
                                               callbacklog.[Select](callbacklog.Text.Length - 1, 0)
                                               callbacklog.ScrollToCaret()
                                               Return ""
                                           End Function

        If callbacklog.InvokeRequired Then
            callbacklog.Invoke(handler, e.Data)
        End If
    End Sub








    Function ModuleGenneral_Xor(ByVal data() As Byte, Optional ByVal offset As Integer = 0, Optional ByVal len As Integer = 0) As Byte
        If len = 0 Then
            len = data.Length
        End If
        Dim result As Byte = data(0)
        For i = 1 To len - 1
            result = result Xor data(i)
        Next

        Return result

    End Function




    Function CompVer(ByVal v1 As String, ByVal v2 As String) As Long
        Dim a1() As String
        Dim a2() As String
        Dim i As Long

        a1 = Split(v1, ".")
        a2 = Split(v2, ".")
        For i = 0 To 3
            Select Case Math.Sign(CInt(a1(i)) - CInt(a2(i)))
                Case -1 : GoTo LT
                Case 0
                Case 1 : GoTo GT
            End Select
        Next

        CompVer = 0
        Exit Function
LT:
        CompVer = -1
        Exit Function
GT:
        CompVer = 1
    End Function













    Public Function ChangeFileText(ByVal file As String, ByVal key As String, ByVal value As String) As Boolean
        Dim str As String = ""
        str = ReadFile(file)
        If str = "" Then Return False
        Dim index As Integer = 0
        Dim tstr() As String = str.Split(vbCrLf)
        For i = 0 To tstr.Length - 1
            tstr(i) = tstr(i).Replace(vbLf, "").Replace(vbTab, " ").Replace("  ", " ")
            index = -1
            If tstr(i).IndexOf(key + "=") <> -1 And tstr(i).IndexOf(key + "=") = tstr(i).LastIndexOf(key + "=") Then
                index = tstr(i).IndexOf(key + "=")
                tstr(i) = Mid(tstr(i), 1, index + (key + "=").Length)
            ElseIf tstr(i).IndexOf(key + " =") <> -1 And tstr(i).IndexOf(key + " =") = tstr(i).LastIndexOf(key + " =") Then
                index = tstr(i).IndexOf(key + " =")
                tstr(i) = Mid(tstr(i), 1, index + (key + " =").Length)
                'ElseIf tstr(i).IndexOf(key + vbTab + "=") <> -1 And tstr(i).IndexOf(key + vbTab + "=") = tstr(i).LastIndexOf(key + vbTab + "=") Then
                '    index = tstr(i).IndexOf(key + vbTab + "=")
                '    tstr(i) = Mid(tstr(i), 1, index + (key + vbTab + "=").Length)
                'ElseIf tstr(i).IndexOf(key + vbTab) <> -1 And tstr(i).IndexOf(key + vbTab) = tstr(i).LastIndexOf(key + vbTab) Then
                '    index = tstr(i).IndexOf(key + vbTab)
                '    tstr(i) = Mid(tstr(i), 1, index + (key + vbTab).Length)
            ElseIf tstr(i).IndexOf(key + " ") <> -1 And tstr(i).IndexOf(key + " ") = tstr(i).LastIndexOf(key + " ") Then
                index = tstr(i).IndexOf(key + " ")
                tstr(i) = Mid(tstr(i), 1, index + (key + " ").Length)
            End If
            If index <> -1 Then
                tstr(i) = tstr(i) + " " + value
                Exit For
            End If
        Next
        If index = -1 Then
            MsgBox("查找关键字【" + key + "】失败！" + vbCrLf + "文件:" + file)
            Return False
        End If

        str = ""
        For i = 0 To tstr.Length - 1
            str += tstr(i)
            If i + 1 <> tstr.Length - 2 Then
                str += vbCr
            End If
        Next

        WriteFile(file, str)
        Return True
    End Function




    '''''''''''''''''''''''''''''''''''''''log4net''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub LogInit()
        'Get the logger as named in the configuration file.
        logger = log4net.LogManager.GetLogger("CBBTools")
    End Sub

    Public Sub LogDebug(ByVal str As String)
        If logger Is Nothing Then
            LogInit()
        End If
        Try
            logger.Debug(str)
        Catch ex As Exception
            logger.Error("LogDebug() - " & ex.Message)
        End Try
    End Sub

    Public Sub LogInfo(ByVal str As String)
        If logger Is Nothing Then
            LogInit()
        End If
        Try
            logger.Info(str)
        Catch ex As Exception
            logger.Error("LogInfo() - " & ex.Message)
        End Try
    End Sub

    Public Sub LogErr(ByVal str As String)
        Try
            logger.Error(str)
        Catch ex As Exception
            logger.Error("LogErr() - " & ex.Message)
        End Try
    End Sub





End Module



