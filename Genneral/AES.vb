
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO

Public Class AES

    Private AES_IV As String = "593840@274837103" '加密IV 
    Private AES_Key As String = "sj58f4k82h@#49&$hdje826dh38smw81" '加密key
    Private AES_Length As Integer = 256 '加密的長度預設用256
    Private AES_Result_Type As String = "HEX" '回傳的格式

    Public Sub New()
        Me.AES_Constructor()
    End Sub



    Public Sub AES_Constructor()
        Me.AES_Set_Length(256)
        Me.AES_Set_IV("593840@274837103")
        Me.AES_Set_Key("sj58f4k82h@#49&$hdje826dh38smw81")
        Me.AES_Set_Result_Type("HEX")
    End Sub

    ''' <summary>
    ''' 設定AES 加密長度
    ''' </summary>
    ''' <param name="InputLength">加密的長度 只接受128 196 256 如果設定其他的，將自動預設為256</param>
    ''' <remarks></remarks>
    Public Sub AES_Set_Length(ByVal InputLength As Integer)
        If InputLength = 128 Or InputLength = 196 Or InputLength = 256 Then
            Me.AES_Length = InputLength
        Else
            Me.AES_Length = 256
        End If
    End Sub
    ''' <summary>
    ''' 設定AES加密後回傳的結果類型
    ''' </summary>
    ''' <param name="InputType">BASE64 回傳base64加密後的結果 HEX 回傳轉16進位的格式</param>
    ''' <remarks></remarks>
    Public Sub AES_Set_Result_Type(ByVal InputType As String)
        If InputType = "BASE64" Or InputType = "HEX" Then
            Me.AES_Result_Type = InputType
        Else
            Me.AES_Result_Type = "HEX"
        End If
    End Sub

    Public Function AES_Get_Result_Type()
        Return Me.AES_Result_Type
    End Function

    Public Function AES_Get_Length()
        Return Me.AES_Length.ToString
    End Function
    ''' <summary>
    ''' 設定AES 加密的IV直請固定帶入16字元
    ''' </summary>
    ''' <param name="InputIV">要設定IV值</param>
    ''' <remarks></remarks>
    Public Sub AES_Set_IV(ByVal InputIV As String)
        If InputIV.Length <> 16 Then
            Me.AES_IV = "1234567890123456"
        Else
            Me.AES_IV = InputIV
        End If
    End Sub
    ''' <summary>
    ''' 取得AES設定的Key 值
    ''' </summary>
    ''' <returns>目前使用的AES Key值</returns>
    ''' <remarks></remarks>
    Public Function AES_Get_IV()
        Return Me.AES_IV
    End Function
    ''' <summary>
    ''' 設定AES 加密的Key值 AES128 請設定16字元 AES192 請設定24字元 AES256 請設定32字元
    ''' </summary>
    ''' <param name="InputKey"></param>
    ''' <remarks></remarks>
    Public Sub AES_Set_Key(ByVal InputKey As String)
        If InputKey.Length <> 16 Then
            Me.AES_Key = "12345678901234567890123456789012"
        Else
            Me.AES_Key = InputKey
        End If
    End Sub
    ''' <summary>
    ''' 取得AES 的Key值設定
    ''' </summary>
    ''' <returns>目前使用的AES Key值</returns>
    ''' <remarks></remarks>
    Public Function AES_Get_Key()
        Return Me.AES_Key
    End Function

    ''' <summary>
    ''' AES 加密函数
    ''' </summary>
    ''' <param name="strContent">要加密的内容</param>
    ''' <returns>加密後的結果字串</returns>
    ''' <remarks>會依照AES_Get_Result_Type的設定回傳不一樣的結果</remarks>
    Public Function AES_Encrypt(ByVal strContent As String) As String

        Try
            Dim AES As New RijndaelManaged()
            AES.KeySize = 256
            AES.Key = Encoding.UTF8.GetBytes(Me.AES_Get_Key())
            AES.IV = Encoding.UTF8.GetBytes(Me.AES_Get_IV())
            AES.Mode = CipherMode.CBC
            AES.Padding = PaddingMode.PKCS7

            Dim return_str As String = ""
            Dim encrypt = AES.CreateEncryptor()
            Dim bytes As Byte() = Encoding.GetEncoding("utf-8").GetBytes(strContent)
            Dim stream As New MemoryStream()
            Dim stream2 As New CryptoStream(stream, encrypt, CryptoStreamMode.Write)

            stream2.Write(bytes, 0, bytes.Length)
            stream2.FlushFinalBlock()


            'MessageBox.Show()
            If Me.AES_Get_Result_Type() = "BASE64" Then
                Dim base64String As String = System.Convert.ToBase64String(stream.ToArray, 0, stream.ToArray.Length)
                return_str = base64String.ToString()
            Else
                Dim sb As New StringBuilder()
                For Each B As Byte In stream.ToArray
                    sb.AppendFormat("{0:X2}", B)
                Next
                return_str = sb.ToString()
            End If

            stream.Close()
            Return return_str

        Catch generatedExceptionName As Exception
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' AES 256 解密函数
    ''' </summary>
    ''' <param name="strContent">要解密的内容</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AES_Decrypt(ByVal strContent As String) As String
        Try
            Dim AES As New RijndaelManaged()

            AES.KeySize = 256
            AES.Key = Encoding.UTF8.GetBytes(Me.AES_Get_Key())
            AES.IV = Encoding.UTF8.GetBytes(Me.AES_Get_IV())
            AES.Mode = CipherMode.CBC
            AES.Padding = PaddingMode.PKCS7

            Dim decrypt = AES.CreateDecryptor()
            Dim return_str As String
            Dim stream As New MemoryStream()
            Dim stream2 As New CryptoStream(stream, decrypt, CryptoStreamMode.Write)

            If Me.AES_Get_Result_Type() = "BASE64" Then
                Dim buffer As Byte()
                buffer = Convert.FromBase64String(strContent)
                stream2.Write(buffer, 0, buffer.Length)
                stream2.FlushFinalBlock()
                return_str = Encoding.GetEncoding("utf-8").GetString(stream.ToArray())
                stream.Close()
            Else

                Dim temp As Integer = (strContent.Length / 2) - 1

                Dim buffer(temp) As Byte
                For X As Integer = 0 To (strContent.Length / 2) - 1 Step 1
                    Dim i As Integer = (Convert.ToInt32(strContent.Substring(X * 2, 2), 16))
                    buffer(X) = CByte(i)
                Next

                stream2.Write(buffer, 0, buffer.Length)
                stream2.FlushFinalBlock()
                return_str = Encoding.GetEncoding("utf-8").GetString(stream.ToArray())
                stream.Close()
            End If
            Return return_str


        Catch generatedExceptionName As Exception
            Return ""
        End Try
    End Function
End Class
