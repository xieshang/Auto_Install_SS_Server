Imports System.IO.Ports

Module ModuleCom

    Dim com_list As ComboBox
    Dim com As SerialPort

    Dim bt As Button


    Public Sub ModuleCom_Reg(ByVal s As SerialPort, ByVal list As ComboBox, ByVal b As Button)
        com_list = list
        AddHandler com_list.SelectedIndexChanged, AddressOf com_list_SelectedIndexChanged
        AddHandler com_list.DropDown, AddressOf com_list_dropdown

        bt = b
        AddHandler bt.Click, AddressOf bt_click

        com = s
    End Sub


    Public Sub com_list_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ModuleCom_CloseSerialPorts(com, bt)
        ModuleCom_OpenSerialPorts(com, com_list.Text, com.BaudRate, bt)
    End Sub


    Public Sub com_list_dropdown(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ModuleCom_GetAvaliableSerialPorts()
    End Sub


    Public Sub bt_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ModuleCom_OpenSerialPorts(com, com_list.Text, com.BaudRate, bt)
    End Sub


    Public Sub ModuleCom_GetAvaliableSerialPorts()
        com_list.Items.Clear()
        Dim portNames() As String = SerialPort.GetPortNames()
        For i = 0 To portNames.Length - 1
            com_list.Items.Add(portNames(i))
        Next
    End Sub

    Public Sub ModuleCom_CloseSerialPorts(ByVal com As SerialPort, ByVal bt As Button)
        com.Close()
        bt.Text = "打开"
    End Sub

    Public Function ModuleCom_OpenSerialPorts(ByVal com As SerialPort, ByVal comport As String, ByVal rate As String, ByVal bt As Button) As Boolean
        If comport = "" Or rate = "" Then
            bt.Text = "打开"
            Return False
        End If
        If bt.Text = "打开" Then
            com.Close()
            com.PortName = comport
            com.BaudRate = rate
            Try
                com.Open()
                bt.Text = "关闭"
            Catch ex As Exception
                    MsgBox("打开【" + comport + "】失败！")
            End Try
        Else
            bt.Text = "打开"
            Try
                com.Close()
            Catch ex As Exception

            End Try
        End If
        If bt.Text = "关闭" Then
            Return True
        Else

            Return False
        End If

    End Function


    Public Sub ModuleCom_Sendbytes(ByVal com As SerialPort, ByVal data() As Byte)
        If com.IsOpen = True Then
            LogDebug("发送:" + ModuleGenneral_BytetoHEXString(data))
            com.Write(data, 0, data.Length)
        End If
    End Sub

    Public Sub ModuleCom_RTS(ByVal c As Boolean)
        If com.IsOpen = True Then
            com.RtsEnable = c
        End If
    End Sub

    Public Sub ModuleCom_DTR(ByVal c As Boolean)
        If com.IsOpen = True Then
            com.DtrEnable = c
        End If
    End Sub

End Module




