'The MIT License (MIT)

'Copyright(c) 2016 David Dzimianski

'Permission Is hereby granted, free Of charge, to any person obtaining a copy
'of this software And associated documentation files (the "Software"), to deal
'in the Software without restriction, including without limitation the rights
'to use, copy, modify, merge, publish, distribute, sublicense, And/Or sell
'copies of the Software, And to permit persons to whom the Software Is
'furnished to do so, subject to the following conditions:

'The above copyright notice And this permission notice shall be included In all
'copies Or substantial portions of the Software.

'THE SOFTWARE Is PROVIDED "AS IS", WITHOUT WARRANTY Of ANY KIND, EXPRESS Or
'IMPLIED, INCLUDING BUT Not LIMITED To THE WARRANTIES Of MERCHANTABILITY,
'FITNESS FOR A PARTICULAR PURPOSE And NONINFRINGEMENT. IN NO EVENT SHALL THE
'AUTHORS Or COPYRIGHT HOLDERS BE LIABLE For ANY CLAIM, DAMAGES Or OTHER
'LIABILITY, WHETHER In AN ACTION Of CONTRACT, TORT Or OTHERWISE, ARISING FROM,
'OUT OF Or IN CONNECTION WITH THE SOFTWARE Or THE USE Or OTHER DEALINGS IN THE
'SOFTWARE.

Option Explicit On
Option Strict On

Public Class Book
    Public Property Chapters As New List(Of Chapter)
    Public Property Name As String
    Public Sub New(Name As String)
        Me.Name = Name
    End Sub
End Class

Public Class Chapter
    Public Verse As New List(Of Verse)
End Class

Public Class Verse
    Public Words As New List(Of word)
    Public Property Text As New System.Text.StringBuilder
    Public ReadOnly Property RawText As String
        Get
            Return Text.ToString
        End Get
    End Property
    Public Book As String
    Public Chapter As Integer
    Public Verse As Integer
End Class

Public Class Bible
    Public Property Books As New Dictionary(Of String, Book)
    Public Property Name As String

    Public Sub New(name As String)
        Me.Name = name
    End Sub

    Public Function GetReference(ref() As Reference) As List(Of Verse)
        Dim l As New List(Of Verse)
        For Each r As Reference In ref
            l.AddRange(Me.GetReference(r))
        Next
        Return l
    End Function

    Public Function GetReference(ref As Reference) As Verse()
        'Try
        Dim v As New List(Of Verse)

        Dim b As Book = Me.Books(ref.Book)
        For c As Integer = ref.StartChapter To ref.EndChapter
            If ref.StartChapter = ref.EndChapter Then
                For Each vs As Verse In b.Chapters(c - 1).Verse
                    If vs.Verse >= ref.StartVerse And vs.Verse <= ref.EndVerse Then
                        v.Add(vs)
                    End If
                Next
            Else
                If c = ref.StartChapter Then
                    For Each vs As Verse In b.Chapters(c - 1).Verse
                        If vs.Verse >= ref.StartVerse Then
                            v.Add(vs)
                        End If
                    Next
                Else
                    For Each vs As Verse In b.Chapters(c - 1).Verse
                        If vs.Verse <= ref.EndVerse Then
                            v.Add(vs)
                        End If
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

    Public ReadOnly Property BookList As String()
        Get
            Dim s As New List(Of String)
            Dim b As Book
            For Each b In Books.Values
                s.Add(b.Name)
            Next
            Return s.ToArray
        End Get
    End Property

    Public Function GetVerseText(ByVal book As String, ByVal chapter As Integer, ByVal verse As Integer, Optional ByVal numberOfVerses As Integer = 1000) As String
        Dim b As Book = Me.Books(book)
        Dim c As Chapter = b.Chapters(chapter - 1)
        Dim s As String = ""
        Dim i As Integer
        If verse + numberOfVerses - 1 > c.Verse.Count Then
            numberOfVerses = c.Verse.Count - verse
        End If
        For i = (verse - 1) To (verse + numberOfVerses - 1)
            s &= (i + 1) & " " & c.Verse(i).RawText & vbCrLf
        Next
        Return s
    End Function
End Class

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