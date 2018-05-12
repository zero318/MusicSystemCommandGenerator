Public Class MainForm
    Public Shared TempArray1 As String()()
    Public Shared TempArray2 As String()()
    Public Shared TempArray3 As String()()
    Public Shared TempArray4() As String
    Public Shared TempArray5() As String
    Public Shared TempArray6() As String
    Public Shared IndexTracker2 As Integer
    Public Shared MusicControlBook As New WrittenBook
    Public Shared AddedMultipartPages As Integer
    Public Shared TempMultipartPages As Integer
    Public Shared AddedPages As Integer
    Public Shared SongNameArray As String()
    Public Shared SongIDArray As String()
    Public Shared SongLengthArray As Integer()
    Public Shared NamespaceName As String
    Public Sub GenerateCommandButton_Click(sender As Object, e As EventArgs) Handles GenerateCommandButton.Click
        NamespaceName = NamespaceBox.Text
        Dim RawPathString As String = ResourcePackLocation.Text & "\" & ResourcePackName.Text & "\assets\" & NamespaceBox.Text & "\sounds\"
        Dim SoundFiles() As String = IO.Directory.GetFiles(RawPathString, "*", IO.SearchOption.AllDirectories)
        Dim NestingFolderLayers As Integer = 0
        Dim DistinctStrings As New Dictionary(Of String, Integer)
        For i As Integer = 0 To SoundFiles.Length - 1
            SoundFiles(i) = SoundFiles(i).Substring(RawPathString.Length)
            If SoundFiles(i).Split("\"c).Count > NestingFolderLayers Then
                NestingFolderLayers = SoundFiles(i).Split("\".ToCharArray).Count
            End If
        Next
        Dim SoundFileParts(SoundFiles.Count - 1)() As String
        For i As Integer = 0 To SoundFiles.Length - 1
            SoundFileParts(i) = SoundFiles(i).Split("\"c)
        Next
        DistinctStrings.Add("/sounds", 0)
        Dim IndexTracker As Integer = 0
        Dim ParentTracker As Integer = 0
        Dim TempString As String
        For i As Integer = 0 To SoundFileParts.GetLength(0) - 1
            ParentTracker = 0
            For j As Integer = 0 To SoundFileParts(i).Length - 1
                TempString = "/" & ParentTracker & "/" & SoundFileParts(i)(j)
                If Not DistinctStrings.ContainsKey(TempString) Then
                    IndexTracker += 1
                    DistinctStrings.Add(TempString, IndexTracker)
                End If
                If Not TempString.EndsWith(".ogg") Then
                    ParentTracker = DistinctStrings.Item(TempString)
                End If
            Next
        Next
        Dim SearchResultArray(IndexTracker) As Integer
        Dim TempInt As Integer
        Dim Temp As List(Of String())() = {New List(Of String()), New List(Of String()), New List(Of String())}
        For i As Integer = 0 To IndexTracker
            SearchResultArray(i) = GetIndexes(String.Join("", DistinctStrings.Keys.ToArray), "/" & i & "/").ToArray.Length
            TempInt = i
            If SearchResultArray(i) > 0 Then
                Temp(0).Add(DistinctStrings.Where(Function(x) x.Key.Contains("/" & TempInt & "/")).Select(Function(x) x.Key.Remove(("/" & TempInt & "/").Length - 1)).ToArray)
                Temp(1).Add(DistinctStrings.Where(Function(x) x.Key.Contains("/" & TempInt & "/")).Select(Function(x) x.Key.Substring(("/" & TempInt & "/").Length - 1)).ToArray)
                Temp(2).Add(DistinctStrings.Where(Function(x) x.Key.Contains("/" & TempInt & "/")).Select(Function(x) x.Value.ToString).ToArray)
            End If
        Next
        Dim DataArray(Temp.ToArray.Length - 1)()() As String
        For i As Integer = 0 To Temp.ToArray.Length - 1
            For j As Integer = 0 To Temp(i).ToArray.Length - 1
                For k As Integer = 0 To Temp(i)(j).ToArray.Length - 1
                    DataArray(i) = Temp(i).ToArray
                Next
            Next
        Next
        Dim DelimitedDataPath As String = ResourcePackLocation.Text & "\" & ResourcePackName.Text & "\assets\" & NamespaceBox.Text
        Dim SongIDs As New List(Of String())
        Dim SongLengths As New List(Of String())
        SongIDs = GetDelimitedData(DelimitedDataPath, "SoundIDs.txt")
        SongLengths = GetDelimitedData(DelimitedDataPath, "SoundLengths.txt")
        Array.Resize(SongNameArray, SongIDs.ToArray.Length)
        Array.Resize(SongIDArray, SongIDs.ToArray.Length)
        Array.Resize(SongLengthArray, SongLengths.ToArray.Length)
        For i As Integer = 0 To SongIDs.ToArray.Length - 1
            SongNameArray(i) = SongIDs(i)(0)
            SongIDArray(i) = SongIDs(i)(1)
            SongLengthArray(i) = CType(SongLengths(i)(1), Integer)
        Next
        TempArray1 = DataArray(0).ToArray
        TempArray2 = DataArray(1).ToArray
        TempArray3 = DataArray(2).ToArray
        Array.Resize(TempArray4, TempArray1.Length)
        Array.Resize(TempArray5, TempArray2.Length)
        Array.Resize(TempArray6, TempArray3.Length)
        For i As Integer = 0 To TempArray1.Length - 1
            TempArray4(i) = TempArray1(i)(0)
        Next
        For i As Integer = 0 To TempArray2.Length - 1
            TempArray5(i) = String.Join("", TempArray2(i))
        Next
        For i As Integer = 0 To TempArray3.Length - 1
            TempArray6(i) = "/" & String.Join("/", TempArray3(i))
        Next
        With MusicControlBook.Pages(0).Lines(0).Text_Components(0)
            .RawText = "Music Control Book"
            .TextColor = JSON_Format2.TextColors.Dark_Purple
            .Format = JSON_Format2.Formatting.Bold_Italic
        End With
        MusicControlBook.Pages(0).FilledLines += 1
        With MusicControlBook.Pages(0).Lines(1).Text_Components(0)
            .RawText = "Table of Contents:"
            .TextColor = JSON_Format2.TextColors.Black
            .Format = JSON_Format2.Formatting.None
        End With
        MusicControlBook.Pages(0).FilledLines += 1
        With MusicControlBook.Pages(0).Lines(2).Text_Components(0)
            .RawText = "Clear Override"
            .TextColor = JSON_Format2.TextColors.Red
            .Format = JSON_Format2.Formatting.Underlined
            With .ClickEvent
                .Type = JSON_Format2.ClickEvents.RunCommand
                .Value = CType(0, String)
            End With
        End With
        MusicControlBook.Pages(0).FilledLines += 1
        Dim PageTracker As Integer = 0
        With MusicControlBook
            For i As Integer = 0 To TempArray1(0).Length - 1
                With .Pages(0)
                    .PageName = "/MainPage"
                    .PageNumber = PageTracker
                    .ParentID = -1
                    .ParentPage = -1
                    .Contents.Add(TempArray3(PageTracker)(i))
                    With .Lines(3 + i).Text_Components(0)
                        .RawText = TempArray2(PageTracker)(i)
                        .TextColor = JSON_Format2.TextColors.Dark_Blue
                        .Format = JSON_Format2.Formatting.None
                        With .ClickEvent
                            .Type = JSON_Format2.ClickEvents.ChangePage
                            .Value = -1
                        End With
                    End With
                    .FilledLines += 1
                End With
            Next
            For i As Integer = 0 To .Pages(0).Lines.Length - 5
                .AddPage(.Pages(0).PageNumber, .Pages(0).Contents(i), TempArray2(0)(i))
                AddedPages = 0
                For Each Content As Integer In .Pages(i + 1 + IndexTracker2).Contents
                    If Array.IndexOf(TempArray4, "/" & Content) <> -1 Then
                        AddedPages += 1
                        .AddPage(.Pages(i + 1 + IndexTracker2).PageNumber, .Pages(i + 1 + IndexTracker2).Contents(Array.IndexOf(.Pages(i + 1 + IndexTracker2).Contents.ToArray, Content)), "")
                        If Page.Multipart = True Then
                            For j As Integer = 0 To Page.MultipartPageTotal - 1
                                AddedPages += 1
                                .AddPage(.Pages(i + 1 + IndexTracker2).PageNumber, .Pages(i + 1 + IndexTracker2).Contents(Array.IndexOf(.Pages(i + 1 + IndexTracker2).Contents.ToArray, Content)), "")
                                AddedMultipartPages += 1
                            Next
                        End If
                    Else
                        If Page.Multipart = True Then
                            For j As Integer = 0 To Page.MultipartPageTotal - 1
                                AddedPages += 1
                                .AddPage(.Pages(i + 1 + IndexTracker2).ParentPage, .Pages(i + 1 + IndexTracker2).ParentID, "") 'Contents(Array.IndexOf(.Pages(i + 1 + IndexTracker2).Contents.ToArray, Content)), "")
                                AddedMultipartPages += 1
                            Next
                        End If
                    End If
                Next
                IndexTracker2 += AddedPages
                TempMultipartPages = AddedMultipartPages
            Next
        End With
        BookCommandBox.Text = MusicControlBook.FinalizeBookCommand(BookName.Text, AuthorName.Text)
    End Sub

    Private Function GetDelimitedData(ByVal FilePath As String, ByVal FileName As String) As List(Of String())
        Dim TempListOfArray As New List(Of String())
        Using MyReader As New FileIO.TextFieldParser(FilePath & "\" & FileName)
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim currentRow As String()
            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()
                    TempListOfArray.Add(currentRow)
                Catch ex As FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                End Try
            End While
        End Using
        Return TempListOfArray
    End Function

    Private Function GetIndexes(ByVal SearchWithinThis As String, ByVal SearchForThis As String) As List(Of Integer)
        'Screw it, let's just rip a function off the internet.
        Dim Result As New List(Of Integer)
        Dim i As Integer = SearchWithinThis.IndexOf(SearchForThis)
        While (i <> -1)
            Result.Add(i)
            i = SearchWithinThis.IndexOf(SearchForThis, i + 1)
        End While
        Return Result
    End Function

    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        BookCommandBox.Size = New Size(Size.Width - 40, (Size.Height - BookCommandBox.Location.Y) - 50)
    End Sub
End Class



Public Class WrittenBook
    Public Pages As Page() = {New Page}
    Public Sub New()
    End Sub
    Public Sub New(ByVal PagesArray As Page())
        Pages = PagesArray
    End Sub
    Public Sub AddPage(ByVal ParentPage As Integer, ByVal ParentID As Integer, ByVal Name As String)
        Array.Resize(Pages, Pages.Length + 1)
        Pages(Pages.Length - 1) = New Page(ParentPage, ParentID, Name)
    End Sub
    Public Function FinalizeBookCommand(ByVal BookNameString As String, ByVal AuthorNameString As String) As String
        Dim CommandString As String = ""
        Dim IsBold As Boolean = False
        Dim IsItalic As Boolean = False
        Dim IsUnderlined As Boolean = False
        Dim IsStrikethrough As Boolean = False
        Dim IsObfuscated As Boolean = False
        Dim HasClickEvent As Integer = 0
        Dim IsMultipart As Boolean = 0
        CommandString &= "/give @p minecraft:written_book{title:" & BookNameString & ",author:" & AuthorNameString & ",pages:["
        For i As Integer = 0 To Pages.Length - 1
            CommandString &= """[\""\"","
            IsMultipart = False
            If Pages(i).Lines(13).Text_Components.Length > 1 Then
                IsMultipart = True
            End If
            For j As Integer = 0 To Pages(i).Lines.Length - 1
                For k As Integer = 0 To Pages(i).Lines(j).Text_Components.Length - 1
                    With Pages(i).Lines(j).Text_Components(k)
                        If .RawText <> Nothing Then
                            If .RawText.EndsWith(".ogg") Then
                                CommandString &= "{\""text\"":\""" & .RawText.Remove(.RawText.Length - 4)
                            Else
                                CommandString &= "{\""text\"":\""" & .RawText
                            End If
                            If (j < 13) AndAlso (k = 0) Then
                                CommandString &= "\\n"
                            End If
                            CommandString &= "\"""
                            If (.TextColor <> Nothing) OrElse (.Format <> Nothing) OrElse (.ClickEvent.Type <> 0) Then
                                CommandString &= ","
                            End If
                        End If
                        If .TextColor <> Nothing Then
                            CommandString &= "\""color\"":\"""
                            Select Case .TextColor
                                Case = JSON_Format2.TextColors.Blue
                                    CommandString &= "blue"
                                Case = JSON_Format2.TextColors.None
                                    CommandString &= "none"
                                Case = JSON_Format2.TextColors.Dark_Purple
                                    CommandString &= "dark_purple"
                                Case = JSON_Format2.TextColors.Red
                                    CommandString &= "red"
                                Case = JSON_Format2.TextColors.Aqua
                                    CommandString &= "aqua"
                                Case = JSON_Format2.TextColors.Black
                                    CommandString &= "black"
                                Case = JSON_Format2.TextColors.Dark_Aqua
                                    CommandString &= "dark_aqua"
                                Case = JSON_Format2.TextColors.Dark_Blue
                                    CommandString &= "dark_blue"
                                Case = JSON_Format2.TextColors.Dark_Gray
                                    CommandString &= "dark_gray"
                                Case = JSON_Format2.TextColors.Dark_Green
                                    CommandString &= "dark_green"
                                Case = JSON_Format2.TextColors.Dark_Red
                                    CommandString &= "dark_red"
                                Case = JSON_Format2.TextColors.Gold
                                    CommandString &= "gold"
                                Case = JSON_Format2.TextColors.Gray
                                    CommandString &= "gray"
                                Case = JSON_Format2.TextColors.Green
                                    CommandString &= "green"
                                Case = JSON_Format2.TextColors.Purple
                                    CommandString &= "purple"
                                Case = JSON_Format2.TextColors.White
                                    CommandString &= "white"
                                Case = JSON_Format2.TextColors.Yellow
                                    CommandString &= "yellow"
                            End Select
                            CommandString &= "\"""
                            If (.Format <> Nothing) OrElse (.ClickEvent.Type <> 0) Then
                                CommandString &= ","
                            End If
                        End If
                        If .Format <> Nothing Then
                            IsBold = False
                            IsItalic = False
                            IsUnderlined = False
                            IsStrikethrough = False
                            IsObfuscated = False
                            If (((.Format Mod 16) Mod 8) Mod 4) Mod 2 = 1 Then
                                IsBold = True
                            End If
                            If (((.Format Mod 16) Mod 8) Mod 4) \ 2 = 1 Then
                                IsItalic = True
                            End If
                            If ((.Format Mod 16) Mod 8) \ 4 = 1 Then
                                IsUnderlined = True
                            End If
                            If (.Format Mod 16) \ 8 = 1 Then
                                IsStrikethrough = True
                            End If
                            If .Format \ 16 = 1 Then
                                IsObfuscated = True
                            End If
                            If IsBold = True Then
                                CommandString &= "\""bold\"":true"
                                If IsItalic OrElse IsUnderlined OrElse IsStrikethrough OrElse IsObfuscated Then
                                    CommandString &= ","
                                End If
                            End If
                            If IsItalic = True Then
                                CommandString &= "\""italic\"":true"
                                If IsUnderlined OrElse IsStrikethrough OrElse IsObfuscated Then
                                    CommandString &= ","
                                End If
                            End If
                            If IsUnderlined = True Then
                                CommandString &= "\""underlined\"":true"
                                If IsStrikethrough OrElse IsObfuscated Then
                                    CommandString &= ","
                                End If
                            End If
                            If IsStrikethrough = True Then
                                CommandString &= "\""strikethrough\"":true"
                                If IsObfuscated Then
                                    CommandString &= ","
                                End If
                            End If
                            If IsObfuscated = True Then
                                CommandString &= "\""obfuscated\"":true"
                            End If
                            If .ClickEvent.Type <> 0 Then
                                CommandString &= ","
                            End If
                        End If
                        If .ClickEvent.Type <> 0 Then
                            With .ClickEvent
                                HasClickEvent = 0
                                Select Case .Type
                                    Case = JSON_Format2.ClickEvents.ChangePage
                                        HasClickEvent = 1
                                    Case = JSON_Format2.ClickEvents.RunCommand
                                        HasClickEvent = 2
                                    Case = JSON_Format2.ClickEvents.None
                                        HasClickEvent = 0
                                End Select
                                If HasClickEvent <> 0 Then
                                    CommandString &= "\""clickEvent\"":{\""action\"":\"""
                                    Select Case HasClickEvent
                                        Case = 1
                                            CommandString &= "change_page"
                                        Case = 2
                                            CommandString &= "run_command"
                                    End Select
                                    CommandString &= "\"",\""value\"":"
                                    Select Case HasClickEvent
                                        Case = 1
                                            CommandString &= "\""" & (.Value + 1) & "\"""
                                        Case = 2
                                            CommandString &= "\""/trigger MO set " & .Value & "\"""
                                    End Select
                                End If
                                CommandString &= "}"
                            End With
                        End If
                        If .RawText <> Nothing Then
                            CommandString &= "}"
                        End If
                    End With
                    If IsMultipart = True Then
                        If j < 13 Then
                            CommandString &= ","
                            If (Pages(i).FilledLines <> 14) AndAlso (j <> 12) AndAlso ((Pages(i).FilledLines - 3) < j) Then
                                CommandString &= "{\""text\"":\""\\n\""}"
                            End If
                        Else
                            If k <> 2 Then
                                CommandString &= ","
                            End If
                        End If
                    Else
                        If (Pages(i).FilledLines - 1) > j Then
                            CommandString &= ","
                        End If
                    End If
                Next
            Next
            CommandString &= "]"""
            If Pages(i).PageNumber <> (Pages.Length - 1) Then
                CommandString &= ","
            End If
        Next
        CommandString &= "]}"
        Return CommandString
    End Function
End Class



Public Class Page
    Shared NextPageNumber As Integer = 0
    Public Lines As JSON_Text() = {New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text}
    Public PageNumber As Integer
    Public ParentPage As Integer
    Public ParentID As Integer
    Public PageName As String
    Public Contents As List(Of Integer)
    Public MyMultipart As Boolean
    Public MyMultipartPageNumber As Integer
    Public MyMultipartPageTotal As Integer
    Public Shared Multipart As Boolean
    Public Shared MultipartPageNumber As Integer
    Public Shared MultipartPageTotal As Integer
    Public Shared MultipartInteger As Integer
    Public FilledLines As Integer
    Public Sub New()
        PageNumber = NextPageNumber
        NextPageNumber += 1
        Contents = New List(Of Integer)
    End Sub
    Public Sub New(ByVal ParentPageNumber As Integer, ByVal Parent_ID As Integer, ByVal Name As String)
        FilledLines = 0
        PageNumber = NextPageNumber
        ParentID = Parent_ID
        NextPageNumber += 1
        ParentPage = ParentPageNumber
        Contents = New List(Of Integer)
        If Multipart = False Then
            Contents = MainForm.TempArray3(PageNumber - MainForm.AddedMultipartPages).ToList.ConvertAll(Function(str) Integer.Parse(str))
        Else
            Dim TempArray As Integer() = {0}
            Array.Resize(TempArray, MainForm.MusicControlBook.Pages(PageNumber - 1).Contents.ToArray.Length - 12)
            Array.ConstrainedCopy(MainForm.MusicControlBook.Pages(PageNumber - 1).Contents.ToArray, 12, TempArray, 0, MainForm.MusicControlBook.Pages(PageNumber - 1).Contents.ToArray.Length - 12)
            Contents = TempArray.ToList
        End If
        With MainForm.MusicControlBook
            If Name <> "" Then
                PageName = Name
            Else
                If Multipart = False Then
                    PageName = MainForm.TempArray2(ParentPage - MainForm.TempMultipartPages)(Array.IndexOf(MainForm.TempArray3(ParentPage - MainForm.TempMultipartPages), CType(.Pages(ParentPage).Contents(Array.IndexOf(.Pages(ParentPage).Contents.ToArray, ParentID)), String)))
                Else
                    PageName = .Pages(PageNumber - 1).PageName
                End If
            End If
        End With
        If (Contents.ToArray.Length - 1) > 12 Then
            Multipart = True
            If MultipartPageNumber = 0 Then
                MultipartPageTotal = (Contents.ToArray.Length) \ 12
            End If
        End If
        MyMultipart = Multipart
        MyMultipartPageNumber = MultipartPageNumber
        MyMultipartPageTotal = MultipartPageTotal
        With MainForm.MusicControlBook
            If ParentPage <> 0 Then
                If Multipart = False Then
                    With .Pages(ParentPage).Lines(Array.IndexOf(.Pages(ParentPage).Contents.ToArray, ParentID) + 1).Text_Components(0).ClickEvent
                        .Type = JSON_Format2.ClickEvents.ChangePage
                        .Value = PageNumber
                    End With
                Else
                    If MultipartPageNumber = 0 Then
                        With .Pages(ParentPage).Lines(Array.IndexOf(.Pages(ParentPage).Contents.ToArray, ParentID) + 1).Text_Components(0).ClickEvent
                            .Type = JSON_Format2.ClickEvents.ChangePage
                            .Value = PageNumber
                        End With
                    End If
                End If
            Else
                If Multipart = False Then
                    With .Pages(ParentPage).Lines(Array.IndexOf(.Pages(ParentPage).Contents.ToArray, ParentID) + 3).Text_Components(0).ClickEvent
                        .Type = JSON_Format2.ClickEvents.ChangePage
                        .Value = PageNumber
                    End With
                Else
                    If MultipartPageNumber = 0 Then
                        With .Pages(ParentPage).Lines(Array.IndexOf(.Pages(ParentPage).Contents.ToArray, ParentID) + 3).Text_Components(0).ClickEvent
                            .Type = JSON_Format2.ClickEvents.ChangePage
                            .Value = PageNumber
                        End With
                    End If
                End If
            End If
        End With
        With Lines(0).Text_Components(0)
            .RawText = MainForm.MusicControlBook.Pages(ParentPage).PageName & "/.."
            .TextColor = JSON_Format2.TextColors.Dark_Blue
            .Format = JSON_Format2.Formatting.None
            With .ClickEvent
                .Type = JSON_Format2.ClickEvents.ChangePage
                .Value = ParentPage
            End With
            FilledLines += 1
        End With
        Dim TempString As String = ""
        Dim TempString2 As String = ""
        Dim ParentPageLoop As Integer = 0
        For i As Integer = 0 To Contents.ToArray.Length - 1
            TempString = ""
            TempString2 = ""
            If (Multipart = True) AndAlso (i = 0) Then
                Array.Resize(Lines(13).Text_Components, 3)
                For j As Integer = 0 To 2
                    Lines(13).Text_Components(j) = New JSON_Format2()
                    With Lines(13).Text_Components(j)
                        Select Case j
                            Case 0
                                If MultipartPageNumber <> 0 Then
                                    .RawText = "<Back"
                                    .TextColor = JSON_Format2.TextColors.Dark_Blue
                                    .Format = JSON_Format2.Formatting.None
                                    With .ClickEvent
                                        .Type = JSON_Format2.ClickEvents.ChangePage
                                        .Value = PageNumber - 1
                                    End With
                                Else
                                    .RawText = "     "
                                    .TextColor = JSON_Format2.TextColors.None
                                    .Format = JSON_Format2.Formatting.None
                                End If
                            Case 1
                                .RawText = " Page " & MultipartPageNumber & "/" & MultipartPageTotal & " "
                                .TextColor = JSON_Format2.TextColors.Black
                                .Format = JSON_Format2.Formatting.None
                            Case 2
                                If MultipartPageNumber <> MultipartPageTotal Then
                                    .RawText = "Next>"
                                    .TextColor = JSON_Format2.TextColors.Dark_Blue
                                    .Format = JSON_Format2.Formatting.None
                                    With .ClickEvent
                                        .Type = JSON_Format2.ClickEvents.ChangePage
                                        .Value = PageNumber + 1
                                    End With
                                Else
                                    .RawText = "     "
                                    .TextColor = JSON_Format2.TextColors.None
                                    .Format = JSON_Format2.Formatting.None
                                End If
                        End Select
                    End With
                Next
                FilledLines += 1
            End If
            If ((Contents.ToArray.Length - 1) + MultipartInteger) <= 12 Then
                With Lines(i + 1).Text_Components(0)
                    If Multipart = False Then
                        .RawText = MainForm.TempArray2(PageNumber - MainForm.AddedMultipartPages)(i)
                    Else
                        .RawText = MainForm.TempArray2(PageNumber - (MainForm.AddedMultipartPages + MultipartPageNumber))(i + (12 * MultipartPageNumber))
                    End If
                    .TextColor = JSON_Format2.TextColors.Blue
                    .Format = JSON_Format2.Formatting.None
                    If .RawText.EndsWith(".ogg") Then
                        TempString = .RawText.Remove(.RawText.Length - 4).Substring(1)
                        With .ClickEvent
                            .Type = JSON_Format2.ClickEvents.RunCommand
                            ParentPageLoop = ParentPage
                            TempString2 = "." & PageName.Substring(1) & "." & TempString
                            If TempString2.StartsWith("._") = True Then
                                TempString2 = "." & TempString2.Substring(2)
                            End If
                            While ParentPageLoop <> 0
                                TempString2 = "." & MainForm.MusicControlBook.Pages(ParentPageLoop).PageName.Substring(1) & TempString2
                                If TempString2.StartsWith("._") = True Then
                                    TempString2 = "." & TempString2.Substring(2)
                                End If
                                ParentPageLoop = MainForm.MusicControlBook.Pages(ParentPageLoop).ParentPage
                            End While
                            TempString2 = MainForm.NamespaceName & ":" & TempString2.Substring(1)
                            .Value = MainForm.SongIDArray(Array.IndexOf(MainForm.SongNameArray, TempString2))
                        End With
                    End If
                End With
                FilledLines += 1
            Else
                If i <= 11 Then
                    With Lines(i + 1).Text_Components(0)
                        .RawText = MainForm.TempArray2(PageNumber - MainForm.AddedMultipartPages)(i)
                        .TextColor = JSON_Format2.TextColors.Blue
                        .Format = JSON_Format2.Formatting.None
                        If .RawText.EndsWith(".ogg") Then
                            TempString = .RawText.Remove(.RawText.Length - 4).Substring(1)
                            With .ClickEvent
                                .Type = JSON_Format2.ClickEvents.RunCommand
                                ParentPageLoop = ParentPage
                                TempString2 = "." & PageName.Substring(1) & "." & TempString
                                While ParentPageLoop <> 0
                                    TempString2 = "." & MainForm.MusicControlBook.Pages(ParentPageLoop).PageName.Substring(1) & TempString2
                                    ParentPageLoop = MainForm.MusicControlBook.Pages(ParentPageLoop).ParentPage
                                End While
                                TempString2 = MainForm.NamespaceName & ":" & TempString2.Substring(1)
                                .Value = MainForm.SongIDArray(Array.IndexOf(MainForm.SongNameArray, TempString2))
                            End With
                        End If
                    End With
                    FilledLines += 1
                End If
            End If
        Next
        If Multipart = True Then
            MultipartPageNumber += 1
        End If
        If MultipartPageNumber > MultipartPageTotal Then
            Multipart = False
            MultipartPageNumber = 0
            MultipartPageTotal = 0
        End If
        If Multipart = False Then
            MultipartInteger = 0
        Else
            MultipartInteger = 1
        End If
    End Sub
    Public Sub New(ByVal LinesArray As JSON_Text())
        PageNumber = NextPageNumber
        NextPageNumber += 1
        Lines = LinesArray
        Contents = New List(Of Integer)
    End Sub
    Public Sub New(ByVal LinesArray As JSON_Text(), ByVal Number As Integer, ByVal Parent As Integer)
        Lines = LinesArray
        PageNumber = Number
        ParentPage = Parent
        Contents = New List(Of Integer)
    End Sub
End Class



Public Class JSON_Text
    Public Text_Components As JSON_Format2() = {New JSON_Format2}
    Public Sub New()
    End Sub
    Public Sub New(ByVal Components As JSON_Format2())
        Text_Components = Components
    End Sub
End Class



Public Class JSON_Format2
    Public RawText As String
    Public TextColor As TextColors
    Public Format As Formatting
    Public ClickEvent As ClickEventData
    Public Sub New()
        RawText = ""
        TextColor = New TextColors
        Format = New Formatting
        ClickEvent = New ClickEventData()
    End Sub
    Public Sub New(ByVal TextInput As String, ByVal InputColor As TextColors, ByVal InputFormat As Formatting, ByVal InputClickEvent As ClickEventData)
        RawText = TextInput
        TextColor = InputColor
        Format = InputFormat
        ClickEvent = InputClickEvent
    End Sub
    Public Enum TextColors
        None = 0
        Black = 1
        Dark_Blue = 2
        Dark_Green = 3
        Dark_Aqua = 4
        Dark_Red = 5
        Dark_Purple = 6
        Gold = 7
        Gray = 8
        Dark_Gray = 9
        Blue = 10
        Green = 11
        Aqua = 12
        Red = 13
        Purple = 14
        Yellow = 15
        White = 16
    End Enum
    Public Enum Formatting
        None = 0
        Bold = 1
        Italic = 2
        Bold_Italic = 3
        Underlined = 4
        Bold_Underlined = 5
        Italic_Underlined = 6
        Bold_Italic_Underlined = 7
        Strikethrough = 8
        Bold_Strikethrough = 9
        Italic_Strikethrough = 10
        Bold_Italic_Strikethrough = 11
        Underlined_Strikethrough = 12
        Bold_Underlined_Strikethrough = 13
        Italic_Underlined_Strikethrough = 14
        Bold_Italic_Underlined_Strikethrough = 15
        Obfuscated = 16
        Bold_Obfuscated = 17
        Italic_Obfuscated = 18
        Bold_Italic_Obfuscated = 19
        Underlined_Obfuscated = 20
        Bold_Underlined_Obfuscated = 21
        Italic_Underlined_Obfuscated = 22
        Bold_Italic_Underlined_Obfuscated = 23
        Strikethrough_Obfuscated = 24
        Bold_Strikethrough_Obfuscated = 25
        Italic_Strikethrough_Obfuscated = 26
        Bold_Italic_Strikethrough_Obfuscated = 27
        Underlined_Strikethrough_Obfuscated = 28
        Bold_Underlined_Strikethrough_Obfuscated = 29
        Italic_Underlined_Strikethrough_Obfuscated = 30
        Bold_Italic_Underlined_Strikethrough_Obfuscated = 31
    End Enum
    Public Structure ClickEventData
        Public Type As ClickEvents
        Public Value As String
        Public Sub New(ByVal InputType As ClickEvents, ByVal InputValue As String)
            Type = InputType
            Value = InputValue
        End Sub
    End Structure
    Public Enum ClickEvents
        None = 0
        ChangePage = 1
        RunCommand = 2
    End Enum
End Class