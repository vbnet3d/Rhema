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

Imports System.Text.RegularExpressions

Public Module BibleData
    Dim StrongsRegex As New Regex("^[GH][0-9]*$", RegexOptions.Compiled)
    Dim VarRegex As New Regex("{.*}", RegexOptions.Compiled)
    Dim separator As Char = ControlChars.Tab

    Public Function Load(filename As String) As FullTextBible
        Dim b As New FullTextBible
        b.Name = IO.Path.GetFileNameWithoutExtension(filename)
        Dim contents() As String = IO.File.ReadAllLines(filename)

        For Each s As String In contents
            Dim w As New word
            Dim data As String() = s.Split(separator)
            If Not data(0) = "Book" Then
                'Strongs	Gender	Number	Case	Tense	Mood	Voice	Person	PartOfSpeech
                w.Book = data(0)
                w.Chapter = Convert.ToInt32(data(1))
                w.Verse = Convert.ToInt32(data(2))
                w._Text = data(3)
                w.StrongsNumber = data(4)
                w._Strongs = {data(4)}
                w._Gender = data(5)
                w._Number = data(6)
                w._Case = data(7)
                w._Tense = data(8)
                w._Mood = data(9)
                w._Voice = data(10)
                w._Person = data(11)
                w._Type = data(12)
                b.Text.Add(w)
            End If
        Next


        Return b
    End Function

    Public Sub ConvertUnboundToRhema(inputfilename As String, outputfilename As String, Optional CodeToNameFile As String = Nothing)
        Dim _booklist As New Dictionary(Of String, String)
        If Not IsNothing(CodeToNameFile) Then
            If IO.File.Exists(CodeToNameFile) Then
                Dim bb As IO.StreamReader = IO.File.OpenText(CodeToNameFile)
                Do Until bb.Peek = -1
                    Dim s() As String = bb.ReadLine.Split(ControlChars.Tab)
                    _booklist.Add(s(0), s(1))
                Loop
                bb.Close()
                bb.Dispose()
                bb = Nothing
            End If
        End If

        Using r As IO.StreamReader = IO.File.OpenText(inputfilename)
            Using w As IO.StreamWriter = IO.File.CreateText(outputfilename)
                w.WriteLine(String.Format("Book{0}Chapter{0}Verse{0}Word{0}Strongs{0}Gender{0}Number{0}Case{0}Tense{0}Mood{0}Voice{0}Person{0}PartOfSpeech", separator))
                While Not r.Peek = -1

                    Dim line As String = r.ReadLine
                    Try
                        If Not line.StartsWith("#") Then
                            Dim data() As String = line.Split(ControlChars.Tab)
                            Dim entry As New EntryData
                            If _booklist.ContainsKey(data(0)) Then
                                entry.Book = _booklist(data(0))
                            Else
                                entry.Book = data(0)
                            End If
                            entry.Chapter = CInt(data(1))
                            entry.Verse = CInt(data(2))
                            entry.Word = ""
                            data(5) = VarRegex.Replace(data(5), "")

                            Dim tokens() As String = data(5).Split(CType(" ", Char()))
                            For Each t As String In tokens
                                If t <> "" Then
                                    If AscW(t(0)) >= 300 Then
                                        If entry.Word <> "" Then
                                            w.WriteLine(String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}",
                                                                      separator, entry.Book,
                                                                      entry.Chapter, entry.Verse,
                                                                      entry.Word, entry.Strongs,
                                                                      entry.Gender, entry.Number,
                                                                      entry.Case, entry.Tense,
                                                                      entry.Mood, entry.Voice,
                                                                      entry.Person, entry.Part))
                                        End If

                                        entry.Strongs = ""
                                        entry.Case = ""
                                        entry.Gender = ""
                                        entry.Number = ""
                                        entry.Tense = ""
                                        entry.Mood = ""
                                        entry.Voice = ""
                                        entry.Person = ""
                                        entry.Part = ""

                                        If t.Contains("-") Then
                                            Dim info() As String = t.Split(CType("-", Char()))
                                            entry.Word = info(0)
                                            If info.Length > 1 Then
                                                Parse(t, entry)
                                            End If
                                        Else
                                            entry.Word = t
                                        End If

                                    ElseIf StrongsRegex.IsMatch(t) Then
                                        entry.Strongs = t
                                    Else
                                        Parse(t, entry)
                                    End If
                                End If


                            Next
                        End If
                    Catch ex As Exception

                    End Try
                End While
                w.Close()
                r.Close()
            End Using
        End Using
    End Sub

    Private Sub Parse(Parsing As String, ByVal entry As EntryData)
        If entry.Strongs = "" Then
            Parsing = Parsing.Replace("--", "-")
            Dim parts() As String = Parsing.Split(CType("-", Char()))

            If parts(1).StartsWith("N") Then
                If parts(1) = "N" Then
                    entry.Part = "Proper Name"
                    entry.Word = entry.Word.Replace("*", "")
                    entry.Word = Char.ToUpper(entry.Word(0)) & entry.Word.Substring(1)
                Else
                    entry.Part = "Noun"
                End If
            ElseIf parts(1).StartsWith("RA") Then
                entry.Part = "Article"
            ElseIf parts(1).StartsWith("RD") Then
                entry.Part = "Demonstrative Pronount"
            ElseIf parts(1).StartsWith("RI") Then
                entry.Part = "Indefinite Pronoun"
            ElseIf parts(1).StartsWith("RP") Then
                entry.Part = "Personal Pronoun"
            ElseIf parts(1).StartsWith("RR") Then
                entry.Part = "Relative Pronoun"
            ElseIf parts(1).StartsWith("V") Then
                entry.Tense = parts(2).Substring(0, 1)
                entry.Voice = parts(2).Substring(1, 1)
                entry.Mood = parts(2).Substring(2, 1)
                If entry.Mood = "P" Then 'Participle
                    entry.Part = "Participle"
                    entry.Case = parts(2).Substring(3, 1)
                    entry.Number = parts(2).Substring(4, 1)
                    entry.Gender = parts(2).Substring(5, 1)
                ElseIf entry.Mood = "N" Then 'Infinitive
                    entry.Part = "Infinitive"
                Else
                    entry.Part = "Verb"
                    entry.Person = parts(2).Substring(3, 1)
                    entry.Number = parts(2).Substring(4, 1)
                End If
            ElseIf parts(1) = "P" Then
                entry.Part = "Preposition"
            ElseIf parts(1) = "C" Then
                entry.Part = "Conjunction"
            ElseIf parts(1) = "D" Then
                entry.Part = "Adverb"
            ElseIf parts(1) = "I" Then
                entry.Part = "Interjection"
            ElseIf parts(1) = "M" Then
                entry.Part = "Indeclinable Number"
            ElseIf parts(1) = "X" Then
                entry.Part = "Particle"
            ElseIf parts(1).StartsWith("A") Then
                entry.Part = "Adjective"
            End If

            If entry.Part = "Article" Or entry.Part = "Noun" Or entry.Part = "Adjective" Or entry.Part.Contains("Pronoun") Then
                entry.Case = parts(2).Substring(0, 1)
                entry.Number = parts(2).Substring(1, 1)
                entry.Gender = parts(2).Substring(2, 1)
            End If
        Else
            If Parsing = "N-PRI" Or entry.Word.Contains("ιησου") Or entry.Word.Contains("χριστ") Then
                entry.Part = "Proper Name"
                entry.Word = Char.ToUpper(entry.Word(0)) & entry.Word.Substring(1)
            ElseIf Parsing = "CONJ" Then
                entry.Part = "Conjunction"
            ElseIf Parsing = "PREP" Then
                entry.Part = "Preposition"
            ElseIf Parsing.StartsWith("T-") Then
                entry.Part = "Article"
                Parsing = Parsing.Replace("T-", "")
            ElseIf Parsing.StartsWith("I-") Then
                entry.Part = "Indefinite Pronoun"
                Parsing = Parsing.Replace("I-", "")
            ElseIf Parsing.StartsWith("PRT") Then
                entry.Part = "Particle"
            ElseIf Parsing.StartsWith("P-") Then
                entry.Part = "Pronoun"
                entry.Person = Parsing.Substring(2, 1)
                entry.Case = Parsing.Substring(3, 1)
                entry.Number = Parsing.Substring(4, 1)
            ElseIf Parsing.Contains("HEB") Then
                entry.Part = "Hebraic"
            ElseIf Parsing.StartsWith("N-") Then
                If Parsing.Contains("OI") Then
                    entry.Part = "Indeclinable Noun"
                ElseIf Parsing.Contains("LI") Then
                    entry.Part = "Letter"
                Else
                    entry.Part = "Noun"
                End If
                Parsing = Parsing.Replace("N-", "")
            ElseIf Parsing.StartsWith("A-") Then
                entry.Part = "Adjective"
                Parsing = Parsing.Replace("A-", "")
            ElseIf Parsing.StartsWith("V-") Then
                'V-AAI-3S
                Parsing = Parsing.Replace("V-", "")
                If IsNumeric(Parsing.Substring(0, 1)) Then
                    Parsing = Parsing.Substring(1)
                End If
                entry.Tense = Parsing.Substring(0, 1)
                entry.Voice = Parsing.Substring(1, 1)
                entry.Mood = Parsing.Substring(2, 1)
                If entry.Mood = "P" Then 'Participle
                    entry.Part = "Participle"
                    entry.Case = Parsing.Substring(4, 1)
                    entry.Number = Parsing.Substring(5, 1)
                    entry.Gender = Parsing.Substring(6, 1)
                ElseIf entry.Mood = "N" Then 'Infinitive
                    entry.Part = "Infinitive"
                Else
                    entry.Part = "Verb"
                    entry.Person = Parsing.Substring(4, 1)
                    entry.Number = Parsing.Substring(5, 1)
                End If
            End If

            If entry.Part = "Article" Or entry.Part = "Noun" Or entry.Part = "Adjective" Or entry.Part = "Indefinite Pronoun" Then
                entry.Case = Parsing.Substring(0, 1)
                entry.Number = Parsing.Substring(1, 1)
                entry.Gender = Parsing.Substring(2, 1)
            End If
        End If
    End Sub
End Module

Public Class EntryData
    Public Book As String
    Public Chapter As Integer
    Public Verse As Integer
    Public Word As String
    Public Strongs As String
    Public Gender As String
    Public Number As String
    Public [Case] As String
    Public Tense As String
    Public Voice As String
    Public Mood As String
    Public Person As String
    Public Part As String
End Class
