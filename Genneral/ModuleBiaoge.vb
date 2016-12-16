Module ModuleBiaoge


    Private Delegate Sub RecieveRefreshMethodDelegate(ByVal [text1] As String, ByVal [text2] As String) '声明委托
    Private Delegate Sub DataGridViewAdd(ByVal db As DataGridView, ByVal [text2] As String) '声明委托
    Private Delegate Sub DataGridViewClear(ByVal db As DataGridView) '声明委托



    ''' <summary>
    ''' 格式化表格
    ''' </summary>
    ''' <param name="db">表格控件</param>
    ''' <param name="x">横向个数</param>
    ''' <param name="y">纵向个数</param>
    ''' <param name="xname">横向说明字符串：
    ''' 若为空，表示说明按0开始标记；非空，则会校验长度是否正确
    ''' 格式:说明1,宽1|说明2,宽2……</param>
    ''' <remarks></remarks>
    Public Sub ModuleBiaoge_ReBuild(ByVal db As DataGridView, ByVal x As Long, ByVal y As Long, ByVal xname As String)



        Dim tname() As String
        Dim dbwith As Long = 0

        If xname <> "" Then
            tname = xname.Split("|")
            If tname.Length <> x Then
                MsgBox("表格横向说明个数错误")
                Exit Sub
            End If
        Else
            ReDim tname(x - 1)
            For i = 0 To x - 1
                tname(i) = i
            Next
        End If
        db.BeginInvoke(New DataGridViewClear(AddressOf ModuleBiaoge_Clear), db)

        For i = 0 To x - 1
            db.BeginInvoke(New DataGridViewAdd(AddressOf ModuleBiaoge_AddColumns), db, tname(i))
            dbwith += 20
        Next

        db.BeginInvoke(New DataGridViewAdd(AddressOf ModuleBiaoge_Resize), db, (dbwith + db.RowHeadersWidth + 10).ToString)


        db.Invoke(New DataGridViewAdd(AddressOf ModuleBiaoge_AddRows), db, y.ToString)
    End Sub

    Private Sub ModuleBiaoge_Resize(ByVal db As DataGridView, ByVal s As String)
        db.Width = Val(s)
    End Sub

    Public Sub ModuleBiaoge_Clear(ByVal db As DataGridView)

        db.Columns.Clear()
        db.Rows.Clear()
    End Sub

    Private Sub ModuleBiaoge_AddRows(ByVal db As DataGridView, ByVal s As String)
        Dim count As Integer = Val(s)
        For i = 0 To count - 1
            db.Rows.Add()
            db.Rows(i).HeaderCell.Value = i + 1
        Next
    End Sub

    Private Sub ModuleBiaoge_AddColumns(ByVal db As DataGridView, ByVal name As String)

        db.Columns.Add(name, name)
        With db.Columns(db.Columns.Count - 1)
            .Width = 20
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .ReadOnly = True
        End With
    End Sub


End Module



