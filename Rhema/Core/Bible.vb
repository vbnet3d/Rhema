Imports System.Globalization
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.FileIO

Public Class Condition
    Public Type As String
    Public X As String
    Public Y As String
    Public Options As New List(Of String)
End Class

Public Module Parsing
    Public Function RemoveDiacriticals(input As String) As String
        Dim decomposed As String = input.Normalize(NormalizationForm.FormD)
        Dim filtered As Char() = decomposed.Where(Function(c) Char.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark).ToArray()
        Dim newString As String = New [String](filtered)
        Return newString
    End Function
End Module

Public Class Reference
    Public Book As String
    Public StartChapter As Integer
    Public EndChapter As Integer
    Public StartVerse As Integer
    Public EndVerse As Integer

    Public Sub AddRange(chapter As Integer, verse As Integer)
        If StartChapter = 0 And StartVerse = 0 Then
            StartChapter = chapter
            StartVerse = verse
        Else
            If chapter = StartChapter Then
                If StartVerse > verse Then
                    EndVerse = StartVerse
                    StartVerse = verse
                Else
                    EndVerse = verse
                End If
                If EndChapter = 0 Then
                    EndChapter = chapter
                End If
            Else
                If chapter > StartChapter Then
                    EndChapter = chapter
                Else
                    EndChapter = StartChapter
                    StartChapter = chapter
                End If
            End If
        End If

    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        Try
            Dim r As Reference = TryCast(obj, Reference)

            If r.Book = Me.Book AndAlso r.StartChapter = Me.StartChapter _
                                AndAlso r.StartVerse = Me.StartVerse _
                                AndAlso Me.EndChapter = r.EndChapter _
                                AndAlso Me.EndVerse = r.EndVerse Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Overrides Function ToString() As String
        Return String.Format("{0} {1}:{2}{3}", Book,
                             StartChapter, StartVerse,
                             IIf(StartChapter <> EndChapter,
                                 String.Format("-{0}:{1}", EndChapter, EndVerse),
                                 IIf(StartVerse <> EndVerse,
                                     String.Format("-{0}", EndVerse), "")))
    End Function
End Class

Public Class FullTextBible
    Public Text As New List(Of word)

    Public Const WITHIN As String = "[\[\]\(\)\{\}, \u0040-\u4040A-Za-z0-9]*<[Ww][Ii][Tt][Hh][Ii][Nn][A-Za-z0-9 ]*>[\[\]\(\)\{\}, \u0040-\u4040A-Za-z0-9]*"
    Public Const [OR] As String = "<[Oo][Rr]>"
    Public Const [AND] As String = "<[Aa][Nn][Dd]>"
    Public Const [XOR] As String = "<[Xx][Oo][Rr]>"
    Public Const [NOT] As String = "<[Nn][Oo][Tt]>"

    Private WithinRegex As New Regex(WITHIN, RegexOptions.Compiled)
    Private OrRegex As New Regex([OR], RegexOptions.Compiled)
    Private AndRegex As New Regex([AND], RegexOptions.Compiled)
    Private XorRegex As New Regex([XOR], RegexOptions.Compiled)
    Private NotRegex As New Regex([NOT], RegexOptions.Compiled)

    Public Function ParseCommandBlock(cmd As String) As Condition
        Dim c As New Condition
        Dim info() As String

        Try
            If cmd.Contains("<") Then
                cmd = cmd.Replace("  ", " ")

                c.X = cmd.Substring(0, cmd.IndexOf("<")).Trim
                c.Y = cmd.Substring(cmd.IndexOf(">") + 1).Trim
                info = cmd.Substring(cmd.IndexOf("<") + 1, cmd.IndexOf(">") - cmd.IndexOf("<") - 1).Trim().Split(" ")
                c.Type = info(0)
                For t As Integer = 1 To info.Length - 1
                    c.Options.Add(info(t))
                Next
            Else
                c.Type = "SIMPLE"
                c.X = cmd
            End If
        Catch ex As Exception
            Throw New Exception("Invalid command syntax in '" & cmd & "'", ex)
        End Try

        Return c
    End Function

    Private Function Parse(cmd As String) As List(Of List(Of Condition))
        Dim l As New List(Of Condition)
        Dim c_groups As New List(Of List(Of Condition))

        Dim sets() As String = OrRegex.Split(cmd)

        For Each s As String In sets
            Dim cm As MatchCollection = WithinRegex.Matches(s)
            For Each m As Match In cm
                l.Add(ParseCommandBlock(m.Value))
            Next

            If l.Count = 0 Then
                Dim c As New Condition
                c.Type = "SIMPLE"
                c.X = s.Trim
                l.Add(c)
            End If

            c_groups.Add(l)
        Next

        Return c_groups
    End Function

    Private Function FuncWithin(x As String, y As String, dist As Integer, unit As String) As Integer
        Dim count As Integer

        Select Case unit
            Case "Words"

        End Select

        Return count
    End Function

    Public Function Search(command As String) As List(Of Reference)
        Dim l As New List(Of Reference)
        Dim condition_groups As List(Of List(Of Condition)) = Parse(command)
        'Dim c As Condition = ParseCommandBlock(command)

        'DEFINITIONS
        '#########################
        'COMMANDS
        '*************************
        'Begin and end with < and >
        '*************************
        'x <WITHIN int WORDS> y 'Is word x within int words of y, either direction?
        'x <OR> y  'either x or y may be true
        'x <AND> y 'x and y must be true
        'x <NOT> y 'x must be true and y false
        'x <XOR> y 'only one of x/y may be true
        'x <FOLLOWED BY int WORDS> y
        'x <PRECEDED BY int WORDS> y

        ' TODO: Change this to loop through all conditions in a group while on a specific word index
        ' And then evaluate the result of all the group conditions together before adding the pieces in.
        For Each group As List(Of Condition) In condition_groups
            For Each c As Condition In group
                For i As Integer = 0 To Me.Text.Count - 1
                    If c.Type = "WITHIN" Then
                        If Me.Text(i)._Text = c.X Then
                            For b As Integer = i To (i - c.Options(0)) Step -1
                                If Me.Text(b)._Text = c.Y Then
                                    Dim r As New Reference
                                    r.Book = Me.Text(i).Book
                                    r.AddRange(Me.Text(i).Chapter, Me.Text(i).Verse)
                                    r.AddRange(Me.Text(b).Chapter, Me.Text(b).Verse)

                                    If Not l.Contains(r) Then
                                        l.Add(r)
                                    End If
                                    Exit For
                                End If
                                If b = 0 Then
                                    Exit For
                                End If
                            Next
                            For f As Integer = i To (i + c.Options(0))
                                If Me.Text(f)._Text = c.Y Then
                                    Dim r As New Reference
                                    r.Book = Me.Text(i).Book
                                    r.AddRange(Me.Text(i).Chapter, Me.Text(i).Verse)
                                    r.AddRange(Me.Text(f).Chapter, Me.Text(f).Verse)

                                    If Not l.Contains(r) Then
                                        l.Add(r)
                                    End If
                                    Exit For
                                End If
                                If f = Me.Text.Count - 1 Then
                                    Exit For
                                End If
                            Next
                        ElseIf Me.Text(i)._Text = c.Y Then
                            For b As Integer = i To (i - c.Options(0)) Step -1
                                If Me.Text(b)._Text = c.X Then
                                    Dim r As New Reference
                                    r.Book = Me.Text(i).Book
                                    r.AddRange(Me.Text(i).Chapter, Me.Text(i).Verse)
                                    r.AddRange(Me.Text(b).Chapter, Me.Text(b).Verse)

                                    If Not l.Contains(r) Then
                                        l.Add(r)
                                    End If
                                    Exit For
                                End If
                                If b = 0 Then
                                    Exit For
                                End If
                            Next
                            For f As Integer = i To (i + c.Options(0))
                                If Me.Text(f)._Text = c.X Then
                                    Dim r As New Reference
                                    r.Book = Me.Text(i).Book
                                    r.AddRange(Me.Text(i).Chapter, Me.Text(i).Verse)
                                    r.AddRange(Me.Text(f).Chapter, Me.Text(f).Verse)

                                    If Not l.Contains(r) Then
                                        l.Add(r)
                                    End If
                                    Exit For
                                End If
                                If f = Me.Text.Count - 1 Then
                                    Exit For
                                End If
                            Next
                        End If
                    ElseIf c.Type = "SIMPLE" Then
                        If Me.Text(i)._Text = c.X Then
                            Dim r As New Reference
                            r.Book = Me.Text(i).Book
                            r.StartChapter = Me.Text(i).Chapter
                            r.StartVerse = Me.Text(i).Verse
                            r.EndChapter = Me.Text(i).Chapter
                            r.EndVerse = Me.Text(i).Verse
                            l.Add(r)
                        End If
                    End If
                Next
            Next
        Next

        Return l
    End Function


End Class

Public Module Import
    Public Function Lexicon(filename As String) As Lexicon
        Dim f As TextFieldParser = My.Computer.FileSystem.OpenTextFieldParser(filename, ",")
        Dim l As New Lexicon

        Do Until f.EndOfData
            Dim data() As String = f.ReadFields
            Dim e As New LexicalEntry
            e.ID = data(0)
            e.lemma = data(1)
            e.xlit = data(2)
            e.pronounce = data(3)
            e.description = data(4)
            e.PartOfSpeech = data(5)
            e.Language = data(6)
            e.simpleform = Parsing.RemoveDiacriticals(e.lemma)
            l.Entry.Add(e.ID, e)
        Loop

        f.Close()

        Return l
    End Function
End Module

Public Class LexicalEntry
    Public ID As String
    Public lemma As String
    Public xlit As String
    Public pronounce As String
    Public description As String
    Public PartOfSpeech As String
    Public Language As String
    Public simpleform As String
End Class

Public Class Lexicon
    Public Entry As New Dictionary(Of String, LexicalEntry)
    Public Name As String
End Class

Public Class Bible
    Public Book As New Dictionary(Of String, Book)
    Public BibleParser As UnicodeBibleParser
    Public Name As String
    Public GUID As System.Guid
    'Public Lexicon As New WordBase
    Public Status As String = ""

    Public Function GetReference(ref() As Reference) As List(Of VerseParser)
        Dim l As New List(Of VerseParser)
        For Each r As Reference In ref
            l.AddRange(Me.GetReference(r))
        Next
        Return l
    End Function

    Public Function GetReference(ref As Reference) As VerseParser()
        'Try
        Dim v As New List(Of VerseParser)
        Dim vr As Integer

        Dim b As Book = Me.Book(BibleParser.ReverseList(ref.Book))
        For c As Integer = ref.StartChapter To ref.EndChapter
            If ref.StartChapter = ref.EndChapter Then
                For vr = ref.StartVerse To ref.EndVerse
                    v.Add(b.Chapter(c - 1).Verse(vr - 2))
                Next
            Else
                If c = ref.StartChapter Then
                    For vr = ref.StartVerse To b.Chapter(c - 1).Verse.Count - 1
                        v.Add(b.Chapter(c - 1).Verse(vr - 2))
                    Next
                Else
                    For vr = 1 To ref.EndVerse
                        v.Add(b.Chapter(c - 1).Verse(vr - 2))
                    Next
                End If

            End If
        Next

        Return v.ToArray
        'Catch ex As Exception
        '    Throw New Exception("Reference not found: " & ref.ToString, ex)
        '    Return Nothing
        'End Try
    End Function

    Public Function ToFullText() As FullTextBible
        Dim s As New System.Text.StringBuilder
        Dim f As New FullTextBible
        Dim index As Integer

        For Each b As Book In Me.Book.Values
            For i As Integer = 0 To b.Chapter.Count - 1
                For Each v As VerseParser In b.Chapter(i).Verse

                    For Each w As word In v.Words
                        index += 1
                        w.Book = b.Name
                        w.Chapter = i + 1
                        w.Verse = v.Verse + 1

                        f.Text.Add(w)
                    Next
                Next
            Next
        Next

        Return f
    End Function

    Public ReadOnly Property BookList As String()
        Get
            Dim s As New List(Of String)
            Dim b As Book
            For Each b In Book.Values
                s.Add(b.Name)
            Next
            Return s.ToArray
        End Get
    End Property

    Public Function Parse(search As String) As String()
        Dim phrases As New List(Of String)

        search = ".*" & Regex.Escape(search).Replace("\*", ".*") & ".*"

        phrases.Add(search)

        Return phrases.ToArray
    End Function

    Public Function Search(ByVal phrase As String, Optional ByVal start_book As String = "", Optional ByVal start_chapter As Integer = 0, Optional ByVal end_book As String = "", Optional ByVal end_chapter As Integer = 0) As List(Of VerseParser)
        Dim list As New List(Of VerseParser)

        ' Get Search range
        Dim b As List(Of Book)
        If start_book = "" Then
            b = Me.Book.Values.ToList
        Else
            If end_book = "" Then
                b = New List(Of Book)
                b.Add(Me.Book(BibleParser.ReverseList(start_book)))
            Else
                b = New List(Of Book)
                Dim l As Book
                Dim adding As Boolean = False
                For Each l In Me.Book.Values.ToList
                    If l.Name = start_book Then
                        adding = True
                    End If
                    If adding Then
                        b.Add(l)
                    End If
                    If l.Name = end_book Then
                        adding = False
                        Exit For
                    End If
                Next
            End If
        End If
        Dim c As New List(Of Chapter)
        Dim i As Integer = start_chapter
        Dim cnt As Integer = 1
        Dim last_book As Boolean = False
        Dim t As Book
        For Each t In b
            While i < t.Chapter.Count - 1
                If last_book Then
                    If i >= end_chapter Then
                        Exit While
                    End If
                End If
                c.Add(t.Chapter(i))
                i += 1
                If cnt = b.Count And end_chapter <> 0 Then
                    last_book = True
                End If
            End While
            i = 0
            cnt += 1
        Next

        'Perform search
        Try
            'Get all possible search phrases from parser
            Dim phrases As String() = Parse(phrase)
            Dim searches As New List(Of Regex)

            For Each p As String In phrases
                Dim r As New Regex(p, RegexOptions.Compiled)
                searches.Add(r)
            Next

            Dim ch As Chapter
            For Each ch In c
                Dim v As VerseParser
                For Each v In ch.Verse
                    For Each r As Regex In searches
                        If r.Match(v.RawText).Success Then
                            list.Add(v)
                            Exit For
                        End If
                    Next
                Next
            Next
            Return list
        Catch ex As Exception
            Return list
        End Try
    End Function

    Public Function GetVerseText(ByVal book As String, ByVal chapter As Integer, ByVal verse As Integer, Optional ByVal numberOfVerses As Integer = 1000) As String
        Status = "Getting Verse"
        Dim b As Book = Me.Book(BibleParser.ReverseList(book))
        Dim c As Chapter = b.Chapter(chapter - 1)
        Dim s As String = ""
        Dim i As Integer
        If verse + numberOfVerses - 1 > c.Verse.Count Then
            numberOfVerses = c.Verse.Count - verse
        End If
        For i = (verse - 1) To (verse + numberOfVerses - 1)
            s &= (i + 1) & " " & c.Verse(i).RawText & vbCrLf
        Next
        Status = "Waiting"
        Return s
    End Function

    Public Sub New(ByVal name As String, ByVal file As String)
        Status = "Generating"
        GUID = Guid.NewGuid
        BibleParser = New UnicodeBibleParser(file)
        name = name
        Dim i As Integer
        For i = 0 To BibleParser.Data.Rows.Count - 1
            If Not Book.ContainsKey(BibleParser.Data.Rows(i).Item(0)) Then
                Dim bk As New Book(BibleParser.Data.Rows(i).Item(0), BibleParser)
                Book.Add(bk.Code, bk)
            End If
            If Not Book(BibleParser.Data.Rows(i).Item(0)).Chapter.Count >= BibleParser.Data.Rows(i).Item(1) Then
                Dim c As New Chapter
                Book(BibleParser.Data.Rows(i).Item(0)).Chapter.Add(c)
            End If
            Dim v As New VerseParser(BibleParser.Data.Rows(i).Item(5))
            v.Book = Book(BibleParser.Data.Rows(i).Item(0)).Name
            v.Chapter = BibleParser.Data.Rows(i).Item(1) - 1
            v.Verse = Book(BibleParser.Data.Rows(i).Item(0)).Chapter(BibleParser.Data.Rows(i).Item(1) - 1).Verse.Count + 1
            Book(BibleParser.Data.Rows(i).Item(0)).Chapter(BibleParser.Data.Rows(i).Item(1) - 1).Verse.Add(v)

            'Dim x As Integer
            'For x = 0 To v.Words.Count - 1
            '    If Not Lexicon.wordlist.ContainsKey(v.Words(x)._Text) Then
            '        Lexicon.wordlist.Add(v.Words(x)._Text, v.Words(x))
            '    End If
            'Next
        Next




        'Status = "Building Lexicon"
        'Me.Lexicon.BuildLexicon()
        Status = "Waiting"
    End Sub
End Class

Public Class Book
    Public Name As String
    Public Code As String
    Public Chapter As New List(Of Chapter)
    Public Sub New(ByVal _name As String, ByVal b As UnicodeBibleParser)
        Name = b.BookList(_name)
        Code = _name
    End Sub
End Class

Public Class Chapter
    Public Verse As New List(Of VerseParser)
End Class

Public Class UnicodeBibleParser
    Private _file As String
    Private _data As New DataTable
    Private _booklist As New Dictionary(Of String, String)
    Private _reverselist As New Dictionary(Of String, String)

    Public ReadOnly Property BookList As Dictionary(Of String, String)
        Get
            Return _booklist
        End Get
    End Property

    Public ReadOnly Property ReverseList As Dictionary(Of String, String)
        Get
            Return _reverselist
        End Get
    End Property

    Public ReadOnly Property Data As DataTable
        Get
            Return _data
        End Get
    End Property

    Public Function Filter(ByVal field As String, ByVal filter_text As String) As DataTable
        Dim d As New DataTable
        'sample o * kai *


        Return d
    End Function

    Public Sub New(ByVal Filename As String)
        Try
            Dim r As IO.StreamReader = IO.File.OpenText(Filename)
            Dim data() As String
            Dim i As Integer

            Do Until r.Peek = -1
                Dim s As String = r.ReadLine
                If Not s.StartsWith("#") Then 'Comment line
                    s = s.Trim(ControlChars.Tab)
                    's = s.Replace(ControlChars.Tab & ControlChars.Tab, ControlChars.Tab)

                    data = s.Split(ControlChars.Tab)

                    _data.Rows.Add(data)
                Else
                    If s.Contains("columns") Then
                        s = s.Replace("#columns", "")
                        s = s.Trim(ControlChars.Tab)
                        's = s.Replace(ControlChars.Tab & ControlChars.Tab, ControlChars.Tab)

                        data = s.Split(ControlChars.Tab)
                        For i = 0 To data.Length - 1
                            _data.Columns.Add(data(i))
                        Next
                    End If
                End If
            Loop

            r.Close()
            r.Dispose()
            r = Nothing


            If IO.File.Exists(".\book_names.txt") Then
                Dim bb As IO.StreamReader = IO.File.OpenText(".\book_names.txt")
                Do Until bb.Peek = -1
                    Dim s() As String = bb.ReadLine.Split(ControlChars.Tab)
                    _booklist.Add(s(0), s(1))
                    _reverselist.Add(s(1), s(0))
                Loop
                bb.Close()
                bb.Dispose()
                bb = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class


Public Class VerseParser
    Public Words As List(Of word)
    Public RawText As String = ""
    Public Book As String
    Public Chapter As Integer
    Public Verse As Integer


    Public Sub New(ByVal Text As String)
        Words = New List(Of word)
        Dim lines As List(Of String) = SplitText(Text)
        Dim s As String
        For Each s In lines
            Dim w As String = ""
            Dim parsing As String = ""
            Dim strongs() As String = {""}
            Dim d() As String = s.Split(" ")
            If d.Length > 1 Then
                ReDim strongs(d.Length - 3)
                parsing = d(d.Length - 1)
            End If
            w = d(0)


            Dim i As Integer
            For i = 1 To d.Length - 2
                strongs(i - 1) = d(i)
            Next

            Dim wd As New word(w, strongs, parsing)
            RawText &= " " & wd._Text
            RawText = RawText.Trim
            Words.Add(wd)
        Next



    End Sub

    Private Function SplitText(ByVal text As String) As List(Of String)
        Dim s As New List(Of String)
        Dim i As Integer

        While text <> ""
            Dim t As String = ""
            text = text.TrimStart(" ")

            While text(0) = "{"  'Variant readings - for the time these are elided
                text = text.Substring(text.IndexOf("}") + 1)
                text = text.TrimStart(" ")
                If text = "" Then Exit While
            End While
            If text = "" Then Exit While

            text = text.TrimStart(" ")
            t = text.Substring(0, text.IndexOf(" "))
            text = text.Substring(text.IndexOf(" "))
            For i = 0 To text.Length - 1
                If AscW(text(i)) > 900 Or text(i) = "{" Then
                    Exit For
                End If
            Next
            t &= text.Substring(0, i)
            t = t.Trim

            If i = text.Length - 1 Then
                text = ""
            End If
            s.Add(t)
            text = text.Substring(i)
        End While

        Return s
    End Function
End Class