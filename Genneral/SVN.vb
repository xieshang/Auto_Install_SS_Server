Imports System.Threading

Class SVNTasksClass
    Public fileurl As String
    Public diraddr As String
    Public Sub SVNGetFileTread()
        SVN_GetFile(fileurl, diraddr)
    End Sub
End Class

Module SVN

    Dim svn_username As String
    Dim svn_password As String
    Dim svn_sever As String

    Dim SVN_Thread(0) As Thread
    Dim SVN_ThreadNum As Integer = 0

    Dim SVNThreadCheck_Thread(0) As Thread
    Dim SVNThreadCheck_ThreadNum As Integer = 0

    Public Sub SVN_Init(ByVal server As String, ByVal name As String, ByVal pass As String)
        svn_username = name
        svn_password = pass
        svn_sever = server.Replace(vbLf, "")
    End Sub

    Public Sub SVN_GetFile(ByVal url As String, ByVal dir As String)
        Dim str = RunCMD("svn export " + url + " " + dir + " --force" + " --no-auth-cache" + " --username " + svn_username + " --password " + svn_password, 20)
    End Sub

    Private Sub SVN_StartGetFile()

    End Sub

    Public Function SVN_GetServerIP() As String
        Return svn_sever
    End Function

    Public Function SVN_CheckGetFileThreadIsOver() As Boolean
        SVN_CheckGetFileThreadIsOver = True
        If SVN_Thread.Length <> 1 Then
            For i = 0 To SVN_Thread.Length - 2
                If SVN_Thread(i).IsAlive = True Then
                    SVN_CheckGetFileThreadIsOver = False
                    Exit For
                End If
            Next
        End If
    End Function

    Public Sub SVN_WaitGetFileThread(ByVal num As Integer)
        If num >= SVN_ThreadNum Then
            Exit Sub
        End If
        If SVN_Thread(num) Is Nothing Then
            Exit Sub
        End If
        SVN_Thread(num).Join()
    End Sub


    Public Function SVN_GetFileThread(ByVal url As String, ByVal dir As String) As Integer
        '判断进程全部结束
        Dim i As Integer

        If SVN_Thread.Length <> 1 Then
            For i = 0 To SVN_Thread.Length - 2
                If SVN_Thread(i).IsAlive = True Then
                    Exit For
                End If
            Next

            If i = SVN_Thread.Length - 1 Then
                SVN_ThreadNum = 0
            End If

        End If

        Dim Tasks As New SVNTasksClass()
        ReDim Preserve SVN_Thread(SVN_ThreadNum + 1)
        SVN_Thread(SVN_ThreadNum) = New Thread(AddressOf Tasks.SVNGetFileTread)
        Tasks.fileurl = url
        Tasks.diraddr = dir
        SVN_Thread(SVN_ThreadNum).Start()

        SVN_ThreadNum += 1
        Return SVN_ThreadNum - 1
    End Function


    Public Function SVN_Login(ByVal url As String) As String
        SVN_Login = RunCMD("svn list  -R " + url + " --no-auth-cache" + " --username " + svn_username + " --password " + svn_password, 20)

    End Function


    ''' <summary>
    ''' 获取SVN服务器的文件列表，支持文件夹深度设置
    ''' </summary>
    ''' <param name="url">服务器地址</param>
    ''' <param name="level">读取文件夹深度，默认为0全部读取</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SVN_GetList(ByVal url As String, Optional ByVal timeout As Integer = 20, Optional ByVal level As Integer = 0) As String


        SVN_GetList = RunCMD("svn list  -R " + url + " --no-auth-cache" + " --username " + svn_username + " --password " + svn_password, timeout)
        If level = 0 Then Return SVN_GetList
        Dim str() As String = SVN_GetList.Split(vbCrLf)
        Dim result(0) As String
        For i = 0 To str.Length - 1
            str(i) = Trim(str(i).Replace(vbLf, ""))
            If str(i).IndexOf("config.ini") <> -1 Then
                If str(i).Split("/").Length = level + 1 Then
                    ReDim Preserve result(result.Length)
                    result(result.Length - 2) = str(i)
                End If
            End If
        Next
        SVN_GetList = ""
        For i = 0 To result.Length - 2
            SVN_GetList += result(i) + vbCrLf
        Next

    End Function



End Module
