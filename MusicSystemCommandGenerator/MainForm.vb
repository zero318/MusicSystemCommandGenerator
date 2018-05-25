Public Class MainForm
    Public Shared TempArray1 As String()()
    Public Shared HierarchyStrings As String()()
    Public Shared HierarchyIDs As String()()
    Public Shared HierarchyParentIDs() As String
    Public Shared IndexTracker2 As Integer
    Public Shared MusicControlBook As WrittenBook
    Public Shared AddedMultipartPages As Integer
    Public Shared TempMultipartPages As Integer
    Public Shared AddedPages As Integer
    Public Shared SongNameArray As String()
    Public Shared SongIDArray As String()
    Public Shared SongLengthArray As Integer()
    Public Shared NamespaceName As String
    Public Shared CharWidthList As List(Of String())
    Public Shared PackName As String
    Public Shared IsProtoPack As String = ""
    Private Sub BookCommandBox_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles BookCommandBox.KeyPress
        If e.KeyChar = Convert.ToChar(1) Then
            DirectCast(sender, TextBox).SelectAll()
            e.Handled = True
        End If
    End Sub
    Public Sub GenerateCommandButton_Click(sender As Object, e As EventArgs) Handles GenerateCommandButton.Click
        TempArray1 = Nothing
        HierarchyStrings = Nothing
        HierarchyIDs = Nothing
        HierarchyParentIDs = Nothing
        IndexTracker2 = Nothing
        MusicControlBook = Nothing
        MusicControlBook = New WrittenBook
        AddedMultipartPages = Nothing
        TempMultipartPages = Nothing
        AddedPages = Nothing
        SongNameArray = Nothing
        SongIDArray = Nothing
        SongLengthArray = Nothing
        NamespaceName = Nothing
        CharWidthList = Nothing
        CharWidthList = New List(Of String())
        PackName = Nothing
        IsProtoPack = Nothing
        BookCommandBox.Text = ""
        NamespaceName = NamespaceBox.Text
        PackName = ResourcePackName.Text
        '========================================
        'This section reads from the variables on the form and stores the sound files and folders to a variable.
        '========================================
        Dim RawPathString As String = ResourcePackLocation.Text & "\" & ResourcePackName.Text & "\assets\" & NamespaceBox.Text & "\sounds\"
        Dim SoundFiles() As String = IO.Directory.GetFiles(RawPathString, "*", IO.SearchOption.AllDirectories)
        For i As Integer = 0 To SoundFiles.Length - 1
            SoundFiles(i) = SoundFiles(i).Substring(RawPathString.Length)
        Next
        Dim SoundFileParts(SoundFiles.Count - 1)() As String
        For i As Integer = 0 To SoundFiles.Length - 1
            SoundFileParts(i) = SoundFiles(i).Split("\"c)
        Next
        '========================================
        'This section takes the raw data about the files/folders and parses it into a dictionary that can supposedly be queried.
        '========================================
        Dim DistinctStrings As New Dictionary(Of String, Integer)
        DistinctStrings.Add("/sounds", 0)
        Dim IndexTracker As Integer = 0
        Dim ParentTracker As Integer = 0
        Dim TempString As String
        For i As Integer = 0 To SoundFileParts.GetLength(0) - 1
            ParentTracker = 0
            For j As Integer = 0 To SoundFileParts(i).Length - 1
                TempString = "/" & ParentTracker & "/" & SoundFileParts(i)(j)
                If (Not DistinctStrings.ContainsKey(TempString)) AndAlso (Not TempString.EndsWith("intro.ogg")) Then
                    IndexTracker += 1
                    DistinctStrings.Add(TempString, IndexTracker)
                End If
                If Not TempString.EndsWith(".ogg") Then
                    ParentTracker = DistinctStrings.Item(TempString)
                End If
            Next
        Next
        '========================================
        'However, the dictionary flat out refused to work like I thought it should, so I reparse it into 
        '========================================
        Dim SearchResultArray(IndexTracker) As Integer
        Dim TempIterator As Integer
        Dim DictionaryListArray As List(Of String())() = {New List(Of String()), New List(Of String()), New List(Of String())}
        For i As Integer = 0 To IndexTracker
            'This function is merely something I got off the internet. TBH I have no idea how it works.
            SearchResultArray(i) = GetIndexes(String.Join("", DistinctStrings.Keys.ToArray), "/" & i & "/").ToArray.Length
            'This variable only exists because the next three lines give a warning when I try to use i directly. Something about iteration variables in lambda expressions having unexpected results.
            TempIterator = i
            If SearchResultArray(i) > 0 Then
                DictionaryListArray(0).Add(DistinctStrings.Where(Function(x) x.Key.Contains("/" & TempIterator & "/")).Select(Function(x) x.Key.Remove(("/" & TempIterator & "/").Length - 1)).ToArray)
                DictionaryListArray(1).Add(DistinctStrings.Where(Function(x) x.Key.Contains("/" & TempIterator & "/")).Select(Function(x) x.Key.Substring(("/" & TempIterator & "/").Length - 1)).ToArray)
                DictionaryListArray(2).Add(DistinctStrings.Where(Function(x) x.Key.Contains("/" & TempIterator & "/")).Select(Function(x) x.Value.ToString).ToArray)
            End If
        Next
        Dim DataArray(DictionaryListArray.ToArray.Length - 1)()() As String
        For i As Integer = 0 To DictionaryListArray.ToArray.Length - 1
            For j As Integer = 0 To DictionaryListArray(i).ToArray.Length - 1
                For k As Integer = 0 To DictionaryListArray(i)(j).ToArray.Length - 1
                    DataArray(i) = DictionaryListArray(i).ToArray
                Next
            Next
        Next
        '========================================
        'This section reads some external data files with some information about the sound files.
        'I'd like to replace this with some code that can actually generate IDs and read the lengths from the files, but I'm not really sure how to do that.
        '========================================
        Dim DelimitedDataPath As String = ResourcePackLocation.Text & "\" & ResourcePackName.Text & "\assets\" & NamespaceBox.Text
        Dim SongIDs As New List(Of String())
        Dim SongLengths As New List(Of String())
        Dim CharTempList As New List(Of String())
        SongIDs = GetDelimitedData(DelimitedDataPath, "SoundIDs.txt", True)
        SongLengths = GetDelimitedData(DelimitedDataPath, "SoundLengths.txt", True)
        CharTempList = GetDelimitedData(DelimitedDataPath, "CharWidths.txt", False)
        IsProtoPack = (GetDelimitedData(DelimitedDataPath, "IsFinalPack.txt", True).ToArray)(0)(0)
        Dim CharTempArray As String() = {}
        Array.Resize(CharTempArray, CharTempList.ToArray.Length)
        Dim CharWidthArray As String() = {}
        Array.Resize(CharWidthArray, CharTempList.ToArray.Length)
        For i As Integer = 0 To CharTempList.ToArray.Length - 1
            CharTempArray(i) = CharTempList(i).ToArray(0)
            CharWidthArray(i) = CharTempList(i).ToArray(1)
        Next
        CharWidthList.Add(CharTempArray)
        CharWidthList.Add(CharWidthArray)
        Array.Resize(SongNameArray, SongIDs.ToArray.Length)
        Array.Resize(SongIDArray, SongIDs.ToArray.Length)
        Array.Resize(SongLengthArray, SongLengths.ToArray.Length)
        For i As Integer = 0 To SongIDs.ToArray.Length - 1
            SongNameArray(i) = SongIDs(i)(0)
            SongIDArray(i) = SongIDs(i)(1)
            SongLengthArray(i) = CType(SongLengths(i)(1), Integer)
        Next
        '========================================
        'This section parses the dictionary replacement array into an easier to use format.
        '========================================
        TempArray1 = DataArray(0).ToArray
        'This array contains the strings.
        HierarchyStrings = DataArray(1).ToArray
        'This array contains the hierarchy IDs that the strings correspond to.
        HierarchyIDs = DataArray(2).ToArray
        Array.Resize(HierarchyParentIDs, TempArray1.Length)
        'This loop removes the duplicate entries from TempArray1 to reduce the dimensions of that array.
        For i As Integer = 0 To TempArray1.Length - 1
            'This array contains information about which IDs are folders and which hierarchy IDs are children of those folders.
            HierarchyParentIDs(i) = TempArray1(i)(0)
        Next
        '========================================
        'This section starts generating the ContentIDs of the written book.
        'To be specific, it adds the headers to the first page.
        '========================================
        With MusicControlBook.Pages(0).Lines(0).Text_Components(0)
            .RawText = "Music Control Book"
            .TextColor = JSON_Format2.TextColors.Dark_Purple
            .Format = JSON_Format2.Formatting.Bold_Italic
        End With
        MusicControlBook.Pages(0).FilledLines += 1
        With MusicControlBook.Pages(0).Lines(1).Text_Components(0)
            .RawText = "Table of ContentIDs:"
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
        '========================================
        'This section starts the main loop by filling in the folders of the first page.
        '========================================
        MusicControlBook.Pages(0).FilledLines += 1
        Dim PageTracker As Integer = 0
        With MusicControlBook
            For i As Integer = 0 To TempArray1(0).Length - 1
                With .Pages(0)
                    .PageName = "/MainPage"
                    .PageNumber = PageTracker
                    .ParentID = -1
                    .ParentPage = -1
                    .ContentIDs.Add(HierarchyIDs(PageTracker)(i))
                    With .Lines(3 + i).Text_Components(0)
                        .RawText = HierarchyStrings(PageTracker)(i)
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
            '========================================
            'This loop section is the main section of the program.
            'It iterates through the folders of the first page and generates new pages for any subfolders.
            'If the contents of a folder cannot fit on a single page, it also generates more pages for that folder until the contents will fit.
            '========================================
            For i As Integer = 0 To .Pages(0).Lines.Length - 5 'For each folder on the main page...
                .AddPage(.Pages(0).PageNumber, .Pages(0).ContentIDs(i), HierarchyStrings(0)(i))
                AddedPages = 0
                For Each ContentID As Integer In .Pages(i + 1 + IndexTracker2).ContentIDs 'For all of the IDs on this page...
                    If Array.IndexOf(HierarchyParentIDs, "/" & ContentID) <> -1 Then 'If this ID is a folder...
                        AddedPages += 1
                        .AddPage(.Pages(i + 1 + IndexTracker2).PageNumber, .Pages(i + 1 + IndexTracker2).ContentIDs(Array.IndexOf(.Pages(i + 1 + IndexTracker2).ContentIDs.ToArray, ContentID)), "")
                        If Page.Multipart = True Then 'If this page can't hold all of the contents...
                            For j As Integer = 0 To Page.MultipartPageTotal - 1
                                AddedPages += 1
                                .AddPage(.Pages(i + 1 + IndexTracker2).PageNumber, .Pages(i + 1 + IndexTracker2).ContentIDs(Array.IndexOf(.Pages(i + 1 + IndexTracker2).ContentIDs.ToArray, ContentID)), "")
                                AddedMultipartPages += 1
                            Next
                        End If
                    Else 'If this ID is not a folder...
                        If Page.Multipart = True Then 'If this page can't hold all of the contents...
                            For j As Integer = 0 To Page.MultipartPageTotal - 1
                                AddedPages += 1
                                .AddPage(.Pages(i + 1 + IndexTracker2).ParentPage, .Pages(i + 1 + IndexTracker2).ParentID, "")
                                AddedMultipartPages += 1
                            Next
                        End If
                    End If
                Next
                IndexTracker2 += AddedPages
                TempMultipartPages = AddedMultipartPages
            Next
        End With
        '========================================
        'This function runs at the very end of the program.
        'It compiles all of the book contents into a JSON command and displays it on the form.
        '========================================
        BookCommandBox.Text = MusicControlBook.FinalizeBookCommand(BookName.Text, AuthorName.Text)
        BookCommandBox.Focus()
    End Sub

    Private Function GetDelimitedData(ByVal FilePath As String, ByVal FileName As String, ByVal EnclosingQuotes As Boolean) As List(Of String())
        '========================================
        'I got this function off the internet to read data from some comma delimited text files.
        'Currently it doesn't do very much except retrieve the SoundIDs.
        '========================================
        Dim TempListOfArray As New List(Of String())
        Using MyReader As New FileIO.TextFieldParser(FilePath & "\" & FileName)
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            MyReader.HasFieldsEnclosedInQuotes = EnclosingQuotes
            Dim currentRow As String()
            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()
                    If currentRow(0) = "" Then
                        currentRow(0) = " "
                    End If
                    If currentRow(0) = "comma" Then
                        currentRow(0) = ","
                    End If
                    TempListOfArray.Add(currentRow)
                Catch ex As FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message & "is not valid and will be skipped.")
                End Try
            End While
        End Using
        Return TempListOfArray
    End Function

    Private Function GetIndexes(ByVal SearchWithinThis As String, ByVal SearchForThis As String) As List(Of Integer)
        '========================================
        'Screw it, let's just rip a function off the internet.
        'TBH, I don't even remember what this function is doing. ^_^;
        '========================================
        Dim Result As New List(Of Integer)
        Dim i As Integer = SearchWithinThis.IndexOf(SearchForThis)
        While (i <> -1)
            Result.Add(i)
            i = SearchWithinThis.IndexOf(SearchForThis, i + 1)
        End While
        Return Result
    End Function

    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        '========================================
        'This function merely makes the output textbox as large as possible when the form is resized.
        'I didn't realize how stupidly large the command would get. XD
        '========================================
        BookCommandBox.Size = New Size(Size.Width - 40, (Size.Height - BookCommandBox.Location.Y) - 50)
    End Sub
End Class



Public Class WrittenBook
    '========================================
    'This class implements the main written book structure.
    'A written book can contain an arbitrary number of pages.
    '========================================
    Public Pages As Page() = {New Page}
    Public Sub New() 'Yay default constructor. XD
    End Sub
    Public Sub AddPage(ByVal ParentPage As Integer, ByVal ParentID As Integer, ByVal Name As String)
        '========================================
        'This subroutine extends the array of pages and then generates a new page.
        'This runs repeatedly during the main loop of the program.
        '========================================
        Array.Resize(Pages, Pages.Length + 1)
        Pages(Pages.Length - 1) = New Page(ParentPage, ParentID, Name)
    End Sub
    Public Function GetPixelWidthOfString(ByVal InputString As String) As Integer
        Dim PixelWidth As Integer = 0
        Dim CharArray As Char() = InputString.ToCharArray()
        For i As Integer = 0 To CharArray.Length - 1
            PixelWidth += CType(MainForm.CharWidthList(1).ToArray(Array.IndexOf(MainForm.CharWidthList(0), CType(CharArray(i), String))), Integer)
            If i <> CharArray.Length - 1 Then
                PixelWidth += 1
            End If
        Next
        Return PixelWidth
    End Function
    Public Function TrimStringToBookWidth(ByVal InputString As String) As String
        Dim TrimmedString As String = ""
        Dim PixelWidth As Integer = 0
        Dim CharArray As Char() = InputString.ToCharArray()
        For i As Integer = 0 To CharArray.Length - 1
            PixelWidth += CType(MainForm.CharWidthList(1).ToArray(Array.IndexOf(MainForm.CharWidthList(0), CType(CharArray(i), String))), Integer)
            If i <> CharArray.Length - 1 Then
                PixelWidth += 1
            End If
            If PixelWidth <= 110 Then
                TrimmedString &= CharArray(i)
            End If
        Next
        TrimmedString &= "..."
        Return TrimmedString
    End Function
    Public Function FinalizeBookCommand(ByVal BookNameString As String, ByVal AuthorNameString As String) As String
        '========================================
        'This function reads the written book structure and generates the command to create the book.
        'This is a messy blob, but it's doing the job. I'm not sure how to optimize it since it's mostly just implementing the written book JSON format.
        '========================================
        Dim PackVersionNumber As String = ""
        Dim TempCharArray As Char() = MainForm.PackName.ToCharArray()
        For i As Integer = 0 To TempCharArray.Length - 1
            If Char.IsDigit(TempCharArray(i)) Then
                PackVersionNumber &= CType(TempCharArray(i), String)
            End If
        Next
        '========================================
        Dim BookVersionNumber As String = "2"
        '========================================
        Dim CommandString As String = ""
        Dim IsBold As Boolean = False
        Dim IsItalic As Boolean = False
        Dim IsUnderlined As Boolean = False
        Dim IsStrikethrough As Boolean = False
        Dim IsObfuscated As Boolean = False
        Dim HasClickEvent As Integer = 0
        Dim HasHoverEvent As Integer = 0
        Dim IsMultipart As Boolean = 0
        Dim TempText As String = ""
        CommandString &= "/give @p minecraft:written_book{title:" & BookNameString & ",author:" & AuthorNameString & ",display:{Lore:[" & """Version: " & BookVersionNumber & """, ""Pack: " & PackVersionNumber & MainForm.IsProtoPack & """]}" & ",pages:["
        For i As Integer = 0 To Pages.Length - 1
            CommandString &= """[\""\"","
            IsMultipart = False
            If Pages(i).Lines(13).Text_Components.Length > 1 Then
                IsMultipart = True
            End If
            For j As Integer = 0 To Pages(i).Lines.Length - 1
                For k As Integer = 0 To Pages(i).Lines(j).Text_Components.Length - 1
                    With Pages(i).Lines(j).Text_Components(k)
                        TempText = ""
                        If .RawText <> Nothing Then
                            If .RawText.EndsWith(".ogg") Then
                                .RawText = .RawText.Remove(.RawText.Length - 4)
                            End If
                            'If .RawText.EndsWith(".ogg") Then
                            '    CommandString &= "{\""text\"":\""" & .RawText.Remove(.RawText.Length - 4)
                            'Else
                            '    CommandString &= "{\""text\"":\""" & .RawText
                            'End If
                            If GetPixelWidthOfString(.RawText) > 116 Then
                                TempText = .RawText
                                With .HoverEvent
                                    .Type = JSON_Format2.HoverEvents.ShowText
                                    .Value = TempText
                                End With
                                .RawText = TrimStringToBookWidth(.RawText)
                            End If
                            CommandString &= "{\""text\"":\""" & .RawText
                            If (j < 13) AndAlso (k = 0) Then
                                CommandString &= "\\n"
                            End If
                            'If (j < 13) AndAlso (k = (Pages(i).Lines(j).Text_Components.Length - 1)) Then
                            '    CommandString &= "\\n"
                            'End If
                            CommandString &= "\"""
                            If (.TextColor <> Nothing) OrElse (.Format <> Nothing) OrElse (.ClickEvent.Type <> 0) OrElse (.HoverEvent.Type <> 0) Then
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
                            If (.Format <> Nothing) OrElse (.ClickEvent.Type <> 0) OrElse (.HoverEvent.Type <> 0) Then
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
                            If (.ClickEvent.Type <> 0) OrElse (.HoverEvent.Type <> 0) Then
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
                            If .HoverEvent.Type <> 0 Then
                                CommandString &= ","
                            End If
                        End If
                        If .HoverEvent.Type <> 0 Then
                            With .HoverEvent
                                HasHoverEvent = 0
                                Select Case .Type
                                    Case = JSON_Format2.HoverEvents.ShowText
                                        HasHoverEvent = 1
                                    Case = JSON_Format2.HoverEvents.None
                                        HasHoverEvent = 0
                                End Select
                                If HasHoverEvent <> 0 Then
                                    CommandString &= "\""hoverEvent\"":{\""action\"":\"""
                                    Select Case HasHoverEvent
                                        Case = 1
                                            CommandString &= "show_text"
                                    End Select
                                    CommandString &= "\"",\""value\"":"
                                    Select Case HasHoverEvent
                                        Case = 1
                                            CommandString &= "\""" & .Value & "\"""
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
    '========================================
    'This class implements the structure of each page of a written book and is the messiest part of the code.
    'Each page of a written book can only be 14 lines long, so each page contains an array of 14 instances of the JSON_Text class.
    '========================================
    'This variable keeps track of what the page number of the next page should be.
    Shared NextPageNumber As Integer = 0
    'This is the array of JSON_Text instances. The first instance will be a link to the parent folder on all pages except page 1.
    Public Lines As JSON_Text() = {New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text, New JSON_Text}
    'This variable contains the index number of this page.
    Public PageNumber As Integer
    'This variable stores the index number of the page that contains the parent folder. This is used as part of the link on line 1.
    Public ParentPage As Integer
    'This variable stores the ID of the parent folder.
    Public ParentID As Integer
    'This variable contains the hierarchy string of this folder.
    Public PageName As String
    'This array contains all of the hierarchy IDs within the page(s) corresponding to this folder. If the page is a multipart page, this array may be longer than 13.
    Public ContentIDs As List(Of Integer)
    'These variables keep track of information regarding how many extra pages should be generated to accomodate lines that can't fit on a single page.
    Public Shared Multipart As Boolean
    Public Shared MultipartPageNumber As Integer
    Public Shared MultipartPageTotal As Integer
    Public Shared MultipartInteger As Integer
    'This variable keeps track of how many lines need to be filled with newline characters when the JSON is actually generated. That way the multipage controls are always at the bottom of the page while keeping the command string as small as possible.
    Public FilledLines As Integer
    Public Sub New()
        Multipart = 0
        MultipartPageNumber = 0
        MultipartPageTotal = 0
        MultipartInteger = 0
        NextPageNumber = 0
        '========================================
        'This constructor is only used as part of the generation of the main page.
        '========================================
        PageNumber = NextPageNumber
        NextPageNumber += 1
        ContentIDs = New List(Of Integer)
    End Sub
    Public Sub New(ByVal ParentPageNumber As Integer, ByVal Parent_ID As Integer, ByVal Name As String)
        '========================================
        'The big knot of code.
        '========================================
        FilledLines = 0
        PageNumber = NextPageNumber
        ParentID = Parent_ID
        NextPageNumber += 1
        ParentPage = ParentPageNumber
        ContentIDs = New List(Of Integer)
        If Multipart = False Then 'If this is the first page of this folder...
            ContentIDs = MainForm.HierarchyIDs(PageNumber - MainForm.AddedMultipartPages).ToList.ConvertAll(Function(str) Integer.Parse(str)) '...copy the hieracrhy IDs corresponding to this folder.
        Else 'If this is not the first page of this folder...
            Dim TempArray As Integer() = {0}
            Array.Resize(TempArray, MainForm.MusicControlBook.Pages(PageNumber - 1).ContentIDs.ToArray.Length - 12)
            Array.ConstrainedCopy(MainForm.MusicControlBook.Pages(PageNumber - 1).ContentIDs.ToArray, 12, TempArray, 0, MainForm.MusicControlBook.Pages(PageNumber - 1).ContentIDs.ToArray.Length - 12)
            ContentIDs = TempArray.ToList '...copy the leftover hierarchy IDs from the previous page of this folder.
        End If
        With MainForm.MusicControlBook
            If Name <> "" Then 'If I directly passed the name of the name of this page (usually no)
                PageName = Name
            Else
                If Multipart = False Then 'If this is the first page of this folder...
                    PageName = MainForm.HierarchyStrings(ParentPage - MainForm.TempMultipartPages)(Array.IndexOf(MainForm.HierarchyIDs(ParentPage - MainForm.TempMultipartPages), CType(.Pages(ParentPage).ContentIDs(Array.IndexOf(.Pages(ParentPage).ContentIDs.ToArray, ParentID)), String))) '...copy the name from the hierarchy name array at the index that corresponds to the parent ID in the parent page.
                Else 'If this is not the first page of this folder...
                    PageName = .Pages(PageNumber - 1).PageName '...copy the name of the previous page of this folder.
                End If
            End If
        End With
        If (ContentIDs.ToArray.Length - 1) > 12 Then 'If the contents of this folder can't fit on one page...
            Multipart = True
            If MultipartPageNumber = 0 Then 'Only run this code once per multipart page...
                MultipartPageTotal = (ContentIDs.ToArray.Length) \ 12
            End If
        End If
        With MainForm.MusicControlBook
            If ParentPage <> 0 Then 'If this folder is not directly a subfolder of the main page...
                If Multipart = False Then '...and if this folder does not have multiple pages...
                    With .Pages(ParentPage).Lines(Array.IndexOf(.Pages(ParentPage).ContentIDs.ToArray, ParentID) + 1).Text_Components(0).ClickEvent
                        .Type = JSON_Format2.ClickEvents.ChangePage
                        .Value = PageNumber '...set the link of the parent ID line to this page.
                    End With
                Else '...and if this folder has multiple pages...
                    If MultipartPageNumber = 0 Then '...and if this is the first page of this folder...
                        With .Pages(ParentPage).Lines(Array.IndexOf(.Pages(ParentPage).ContentIDs.ToArray, ParentID) + 1).Text_Components(0).ClickEvent
                            .Type = JSON_Format2.ClickEvents.ChangePage
                            .Value = PageNumber '...set the link of the parent ID line to this page.
                        End With
                    End If
                End If
            Else 'If this folder is directly a subfolder of the main page...
                If Multipart = False Then '...and if this folder does not have multiple pages...
                    With .Pages(ParentPage).Lines(Array.IndexOf(.Pages(ParentPage).ContentIDs.ToArray, ParentID) + 3).Text_Components(0).ClickEvent
                        .Type = JSON_Format2.ClickEvents.ChangePage
                        .Value = PageNumber '...set the link of the parent ID line to this page.
                    End With
                Else '...and if this folder has multiple pages...
                    If MultipartPageNumber = 0 Then '...and if this is the first page of this folder...
                        With .Pages(ParentPage).Lines(Array.IndexOf(.Pages(ParentPage).ContentIDs.ToArray, ParentID) + 3).Text_Components(0).ClickEvent
                            .Type = JSON_Format2.ClickEvents.ChangePage
                            .Value = PageNumber '...set the link of the parent ID line to this page.
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
                .Value = ParentPage 'Set the link of line 1 on this page to the parent page.
            End With
            FilledLines += 1
        End With
        Dim TempString As String = ""
        Dim TempString2 As String = ""
        Dim ParentPageLoop As Integer = 0
        For i As Integer = 0 To ContentIDs.ToArray.Length - 1 'For each hierarchy ID in this folder...
            TempString = ""
            TempString2 = ""
            If (Multipart = True) AndAlso (i = 0) Then 'If this folder has multiple pages...
                Array.Resize(Lines(13).Text_Components, 3) 'Create text components for the multipage buttons.
                For j As Integer = 0 To 2 'For each multipage button text component...
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
                                    .RawText = "<Back"
                                    .TextColor = JSON_Format2.TextColors.Black
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
                                    .RawText = "Next>"
                                    .TextColor = JSON_Format2.TextColors.Black
                                    .Format = JSON_Format2.Formatting.None
                                End If
                        End Select
                    End With
                Next
                FilledLines += 1
                'Array.Resize(Lines(0).Text_Components, 4) 'Create text components for the multipage buttons.
                'For j As Integer = 1 To 3 'For each multipage button text component...
                '    Lines(0).Text_Components(j) = New JSON_Format2()
                '    With Lines(0).Text_Components(j)
                '        Select Case j
                '            Case 1
                '                If MultipartPageNumber <> 0 Then
                '                    .RawText = "   <"
                '                    .TextColor = JSON_Format2.TextColors.Dark_Blue
                '                    .Format = JSON_Format2.Formatting.None
                '                    With .ClickEvent
                '                        .Type = JSON_Format2.ClickEvents.ChangePage
                '                        .Value = PageNumber - 1
                '                    End With
                '                Else
                '                    .RawText = "    "
                '                    .TextColor = JSON_Format2.TextColors.None
                '                    .Format = JSON_Format2.Formatting.None
                '                End If
                '            Case 2
                '                .RawText = " " & MultipartPageNumber & "/" & MultipartPageTotal & " "
                '                .TextColor = JSON_Format2.TextColors.Black
                '                .Format = JSON_Format2.Formatting.None
                '            Case 3
                '                If MultipartPageNumber <> MultipartPageTotal Then
                '                    .RawText = ">"
                '                    .TextColor = JSON_Format2.TextColors.Dark_Blue
                '                    .Format = JSON_Format2.Formatting.None
                '                    With .ClickEvent
                '                        .Type = JSON_Format2.ClickEvents.ChangePage
                '                        .Value = PageNumber + 1
                '                    End With
                '                Else
                '                    .RawText = " "
                '                    .TextColor = JSON_Format2.TextColors.None
                '                    .Format = JSON_Format2.Formatting.None
                '                End If
                '        End Select
                '    End With
                'Next
                'FilledLines += 1
            End If
            If ((ContentIDs.ToArray.Length - 1) + MultipartInteger) <= 12 Then 'If all of the hierarchy IDs of this page will fit on the available lines...
                With Lines(i + 1).Text_Components(0) 'With the next line of the page...
                    If Multipart = False Then 'If this folder does not have multiple pages...
                        .RawText = MainForm.HierarchyStrings(PageNumber - MainForm.AddedMultipartPages)(i) 'Get the hierarchy string of this hierarchy ID.
                    Else 'If this folder has multiple pages...
                        .RawText = MainForm.HierarchyStrings(PageNumber - (MainForm.AddedMultipartPages + MultipartPageNumber))(i + (12 * MultipartPageNumber)) 'Get the hierarchy string of this hierarchy ID.
                    End If
                    .TextColor = JSON_Format2.TextColors.Dark_Blue
                    .Format = JSON_Format2.Formatting.None
                    If .RawText.EndsWith(".ogg") Then 'If this hierarchy ID is a sound file...
                        .TextColor = JSON_Format2.TextColors.Blue
                        TempString = .RawText.Remove(.RawText.Length - 4).Substring(1)
                        With .ClickEvent 'Set the value of the sound override command to the ID of the appropriate sound.
                            .Type = JSON_Format2.ClickEvents.RunCommand
                            ParentPageLoop = ParentPage
                            TempString2 = "." & PageName.Substring(1) & "." & TempString
                            If TempString2.StartsWith("._") = True Then 'If the name of this folder contains an underscore to process correctly...
                                TempString2 = "." & TempString2.Substring(2) '...don't display the underscore.
                            End If
                            While ParentPageLoop <> 0 'Build the string of parent pages until the main page is reached...
                                TempString2 = "." & MainForm.MusicControlBook.Pages(ParentPageLoop).PageName.Substring(1) & TempString2
                                If TempString2.StartsWith("._") = True Then 'If the name of this parent folder contains an underscore to process correctly...
                                    TempString2 = "." & TempString2.Substring(2) '...don't display the underscore.
                                End If
                                ParentPageLoop = MainForm.MusicControlBook.Pages(ParentPageLoop).ParentPage
                            End While
                            TempString2 = MainForm.NamespaceName & ":" & TempString2.Substring(1) 'Create a string matching Minecraft's version of my sound file names.
                            .Value = MainForm.SongIDArray(Array.IndexOf(MainForm.SongNameArray, TempString2)) 'Retrieve the numeric sound ID corresponding to this sound name.
                        End With
                    End If
                End With
                FilledLines += 1
            Else 'If all of the hierarchy IDs of this page will not fit on the available lines...
                If i <= 11 Then 'Do not overwrite the multipage control line.
                    With Lines(i + 1).Text_Components(0) 'With the next line of the page...
                        .RawText = MainForm.HierarchyStrings(PageNumber - MainForm.AddedMultipartPages)(i)
                        .TextColor = JSON_Format2.TextColors.Dark_Blue
                        .Format = JSON_Format2.Formatting.None
                        If .RawText.EndsWith(".ogg") Then 'If this hierarchy ID is a sound file...
                            .TextColor = JSON_Format2.TextColors.Blue
                            TempString = .RawText.Remove(.RawText.Length - 4).Substring(1)
                            With .ClickEvent 'Set the value of the sound override command to the ID of the appropriate sound.
                                .Type = JSON_Format2.ClickEvents.RunCommand
                                ParentPageLoop = ParentPage
                                TempString2 = "." & PageName.Substring(1) & "." & TempString
                                If TempString2.StartsWith("._") = True Then 'If the name of this folder contains an underscore to process correctly...
                                    TempString2 = "." & TempString2.Substring(2) '...don't display the underscore.
                                End If
                                While ParentPageLoop <> 0 'Build the string of parent pages until the main page is reached...
                                    TempString2 = "." & MainForm.MusicControlBook.Pages(ParentPageLoop).PageName.Substring(1) & TempString2
                                    If TempString2.StartsWith("._") = True Then 'If the name of this parent folder contains an underscore to process correctly...
                                        TempString2 = "." & TempString2.Substring(2) '...don't display the underscore.
                                    End If
                                    ParentPageLoop = MainForm.MusicControlBook.Pages(ParentPageLoop).ParentPage
                                End While
                                TempString2 = MainForm.NamespaceName & ":" & TempString2.Substring(1) 'Create a string matching Minecraft's version of my sound file names.
                                .Value = MainForm.SongIDArray(Array.IndexOf(MainForm.SongNameArray, TempString2)) 'Retrieve the numeric sound ID corresponding to this sound name.
                            End With
                        End If
                    End With
                    FilledLines += 1
                End If
            End If
        Next
        If Multipart = True Then 'If this folder has multiple pages...
            MultipartPageNumber += 1
        End If
        If MultipartPageNumber > MultipartPageTotal Then 'If this was the last page of this folder...
            Multipart = False
            MultipartPageNumber = 0
            MultipartPageTotal = 0
        End If
        If Multipart = False Then 'If this folder does not have any pages remaining...
            MultipartInteger = 0 'Set the offset to 0.
        Else 'If this folder has pages remaining...
            MultipartInteger = 1 'Set the offset to 1.
        End If
    End Sub
    'Public Sub New(ByVal LinesArray As JSON_Text())
    '    PageNumber = NextPageNumber
    '    NextPageNumber += 1
    '    Lines = LinesArray
    '    ContentIDs = New List(Of Integer)
    'End Sub
    'Public Sub New(ByVal LinesArray As JSON_Text(), ByVal Number As Integer, ByVal Parent As Integer)
    '    Lines = LinesArray
    '    PageNumber = Number
    '    ParentPage = Parent
    '    ContentIDs = New List(Of Integer)
    'End Sub
End Class



Public Class JSON_Text
    '========================================
    'This class merely acts as an instantiatable container for a page line.
    '========================================
    Public Text_Components As JSON_Format2() = {New JSON_Format2}
    Public Sub New()
    End Sub
    Public Sub New(ByVal Components As JSON_Format2())
        Text_Components = Components
    End Sub
End Class



Public Class JSON_Format2
    '========================================
    'This class merely acts as an instantiatable container for a text component.
    '========================================
    Public RawText As String
    Public TextColor As TextColors
    Public Format As Formatting
    Public ClickEvent As ClickEventData
    Public HoverEvent As HoverEventData
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
        '========================================
        'This structure contains the data corresponding to the two types of click events necessary for this program.
        'It probably doesn't need to be implemented like this, but I was messing around with what a structure is.
        '========================================
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
    Public Structure HoverEventData
        '========================================
        '========================================
        Public Type As HoverEvents
        Public Value As String
        Public Sub New(ByVal InputType As HoverEvents, ByVal InputValue As String)
            Value = InputValue
        End Sub
    End Structure
    Public Enum HoverEvents
        None = 0
        ShowText = 1
    End Enum
End Class