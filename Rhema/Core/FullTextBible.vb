Option Explicit On
Option Strict On
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

Imports System.Text.RegularExpressions

Public Class FullTextBible
    Public Text As New List(Of word)
    Public Name As String
    Public Language As Language
    Private Code As String
    Private infoList As New List(Of Info)

    '****************************
    'REGEX Pattern Definitions
    '****************************
    Public Const WITHIN As String = "[\[\]\(\)\{\}, \u0040-\u4040A-Za-z0-9]*<[WFP][IOR][TLE][A-Za-z0-9 ]*>[\[\]\(\)\{\}, \u0040-\u4040A-Za-z0-9]*"
    Public Const [OR] As String = "<[Oo][Rr]>"
    Public Const [AND] As String = "<[Aa][Nn][Dd]>"
    Public Const [XOR] As String = "<[Xx][Oo][Rr]>"
    Public Const [NOT] As String = "<[Nn][Oo][Tt]>"
    Public Const STRONGS As String = "<[GH][0-9]*>"
    '*****************************

    '****************************
    'REGEX Object Definitions
    '****************************
    Private CommandRegex As New Regex("<[WFP][IOR][TLE][A-Za-z0-9 ]*>", RegexOptions.Compiled)
    Private WithinRegex As New Regex(WITHIN, RegexOptions.Compiled)
    Private OrRegex As New Regex([OR], RegexOptions.Compiled)
    Private AndRegex As New Regex([AND], RegexOptions.Compiled)
    Private XorRegex As New Regex([XOR], RegexOptions.Compiled)
    Private NotRegex As New Regex([NOT], RegexOptions.Compiled)
    Private StrongsRegex As New Regex(STRONGS, RegexOptions.Compiled)

    '****************************

    Public Function ToBible() As Bible
        Dim b As New Bible(Me.Name)
        For Each w As word In Me.Text
            If Not b.Books.ContainsKey(w.Book) Then
                Dim bk As New Book(w.Book)
                b.Books.Add(w.Book, bk)
            End If
            If Not b.Books(w.Book).Chapters.Count >= w.Chapter Then
                b.Books(w.Book).Chapters.Add(New Chapter)
            End If

            Dim foundVerse As Boolean = False
            For i As Integer = 0 To b.Books(w.Book).Chapters(w.Chapter - 1).Verse.Count - 1
                Dim v As Verse = b.Books(w.Book).Chapters(w.Chapter - 1).Verse(i)
                If b.Books(w.Book).Chapters(w.Chapter - 1).Verse(i).Chapter = w.Chapter _
                                        And b.Books(w.Book).Chapters(w.Chapter - 1).Verse(i).Verse = w.Verse Then
                    b.Books(w.Book).Chapters(w.Chapter - 1).Verse(i).Words.Add(w)
                    b.Books(w.Book).Chapters(w.Chapter - 1).Verse(i).Text.AppendFormat("{0} ", w._Text)
                    foundVerse = True
                    Exit For
                End If
            Next

            If Not foundVerse Then
                Dim v As New Verse
                v.Words.Add(w)
                v.Text.AppendFormat("{0} ", w._Text)
                v.Book = w.Book
                v.Chapter = w.Chapter
                v.Verse = w.Verse
                b.Books(w.Book).Chapters(w.Chapter - 1).Verse.Add(v)
            End If

        Next
        Return b
    End Function

    Public Function ParseCommandBlock(cmd As String) As Condition
        Dim c As New Condition
        Dim info() As String

        Try
            If cmd.Contains("<") Then
                cmd = cmd.Replace("  ", " ")

                c.X = cmd.Substring(0, cmd.IndexOf("<")).Trim
                c.Y = cmd.Substring(cmd.IndexOf(">") + 1).Trim
                info = cmd.Substring(cmd.IndexOf("<") + 1, cmd.IndexOf(">") - cmd.IndexOf("<") - 1).Trim().Split(CType(" ", Char()))
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

    Public TokenDefinitions As New List(Of Regex)

    Private Function Tokenize(s As String) As Token()
        If TokenDefinitions.Count <= 0 Then
            TokenDefinitions.Add(New Regex("<[^GH][A-Za-z0-9 ]*>", RegexOptions.Compiled))
            TokenDefinitions.Add(New Regex("<[GH][0-9]*>", RegexOptions.Compiled))
            TokenDefinitions.Add(New Regex("[\[][A-Za-z0-9 \*]*[\]]", RegexOptions.Compiled))
            TokenDefinitions.Add(New Regex("\.\.\.", RegexOptions.Compiled))
            TokenDefinitions.Add(New Regex("[A-Za-z0-9Α-ῼ]*", RegexOptions.Compiled))
        End If

        Dim t As New List(Of Token)

        Dim curType As UnitType = UnitType.Command
        For Each d As Regex In TokenDefinitions
            Dim m As MatchCollection = d.Matches(s)
            For Each match As Match In m
                If Not match.Value = "" Then
                    'Disallow repeated tokenizing... first come first serve tokenizing.
                    s = s.Remove(match.Index, match.Length)
                    s = s.Insert(match.Index, "".PadRight(match.Length, CChar("#")))

                    Dim token As New Token
                    token.Raw = match.Value
                    token.Index = match.Index
                    token.LastIndex = match.Length + match.Index
                    token.Type = curType
                    t.Add(token)
                End If
            Next
            Dim tp As UnitType = CType(CType(curType, Integer) + 1, UnitType)
            If System.Enum.IsDefined(GetType(UnitType), tp) Then
                curType = tp
            Else
                curType = UnitType.Literal
            End If
        Next

        t.Sort()

        Return t.ToArray
    End Function

    Private Function Unitize(tokens As Token()) As Unit()
        Dim u As New List(Of Unit)
        Dim curUnit As New Unit

        For Each t As Token In tokens
            If t.Type = UnitType.Command Then
                If curUnit.Tokens.Count > 0 Then
                    u.Add(curUnit)
                    curUnit = New Unit
                End If
                curUnit.Tokens.Add(t)
                curUnit.Type = t.Type
                u.Add(curUnit)
                curUnit = New Unit
            Else
                If curUnit.Tokens.Count = 0 Then
                    curUnit.Type = t.Type
                End If
                curUnit.Tokens.Add(t)
                If curUnit.Type <> t.Type Then
                    curUnit.Type = UnitType.Complex
                End If
            End If
        Next
        If curUnit.Tokens.Count > 0 Then
            u.Add(curUnit)
        End If

        Return u.ToArray
    End Function

    Private Function GetCondition(s As String) As Condition
        Dim con As New Condition
        s = s.Replace("<", "").Replace(">", "")
        Select Case s(0).ToString
            Case "W"
                con.Type = "WITHIN"
            Case "F"
                con.Type = "FOLLOWEDBY"
            Case "P"
                con.Type = "PRECEDEDBY"
        End Select
        Dim data() As String = s.Split(CType(" ", Char()))
        For Each d As String In data
            If Not d = con.Type Then
                con.Options.Add(d)
            End If
        Next
        Return con
    End Function

    Public Function Search(s As String) As List(Of Reference)
        Dim refs As New List(Of Reference)

        Dim conditions As Condition() = curFtBible.Search(Unitize(Tokenize(s)))

        For i As Integer = 0 To Me.Text.Count - 1
            For Each c As Condition In conditions
                If c.Type = "SIMPLE" Then
                    c.Result = FuncSimple(i, c.Unit1)
                Else
                    Dim b As Boolean = c.Type = "WITHIN" Or c.Type = "PRECEDEDBY"
                    Dim f As Boolean = c.Type = "WITHIN" Or c.Type = "FOLLOWEDBY"
                    c.Result = FuncProximity(i, c.Unit1, c.Unit2, CInt(c.Options(0)), f, b)
                End If
            Next

            If Search(conditions) Then
                For Each c As Condition In conditions
                    refs.AddRange(c.Result.References)
                Next
            End If
        Next

        Return refs
    End Function

    Public Function Search(units As Unit()) As Condition()
        Try
            Dim c As New List(Of Condition)
            For i As Integer = 0 To units.Count - 1
                If Not units(i).Used Then
                    If units(i).Type <> UnitType.Command Then
                        If units.Count > i + 1 Then
                            If CommandRegex.IsMatch(units(i + 1).Tokens.First.Raw) Then
                                'SKIP ME
                            Else
                                'ADD ME
                                Dim con As New Condition
                                con.Type = "SIMPLE"
                                con.Unit1 = units(i)
                                c.Add(con)
                                units(i).Used = True
                            End If
                        Else
                            ' End of the line - SIMPLE
                            Dim con As New Condition
                            con.Type = "SIMPLE"
                            con.Unit1 = units(i)
                            units(i).Used = True
                            c.Add(con)
                        End If
                    Else
                        If CommandRegex.IsMatch(units(i).Tokens.First.Raw) Then
                            Dim con As Condition = GetCondition(units(i).Tokens.First.Raw)

                            con.Unit1 = units(i - 1)
                            con.Unit2 = units(i + 1)
                            units(i - 1).Used = True
                            units(i + 1).Used = True
                            c.Add(con)
                        Else
                            Dim con As New Condition
                            units(i).Used = True
                            con.Type = units(i).Tokens.First.Raw.Replace("<", "").Replace(">", "")
                            c.Add(con)
                        End If
                    End If
                End If
            Next


            Return c.ToArray
        Catch ex As Exception
            Throw New Exception("Invalid search syntax", ex)
            Return Nothing
        End Try
    End Function

    Public Function Search(conditions As Condition()) As Boolean
        Dim status As Boolean = True

        If conditions.Length = 1 Then
            If Not IsNothing(conditions(0).Result) Then
                Return conditions(0).Result.Success
            Else
                Return False
            End If
        End If

            For i As Integer = 0 To conditions.Length - 1
            Select Case conditions(i).Type.ToUpper
                Case "AND"
                    status = Evaluate.And(conditions(i - 1), conditions(i + 1))
                Case "OR"
                    status = Evaluate.Or(conditions(i - 1), conditions(i + 1))
                Case "NOT"
                    status = Evaluate.Not(conditions(i - 1), conditions(i + 1))
                Case "XOR"
                    status = Evaluate.Xor(conditions(i - 1), conditions(i + 1))
            End Select
            If Not status Then
                Exit For
            End If
        Next

        Return status
    End Function

    Private Function Match(t As Token, w As word, Optional ids As Dictionary(Of Integer, Parsing) = Nothing) As Boolean
        If t.Type = UnitType.Literal Then
            If t.Raw = w._Text Then
                Return True
            Else
                Return False
            End If
        ElseIf t.Type = UnitType.Strongs Then
            If w.StrongsNumber.Contains(t.StrongsNumber) Then
                Return True
            Else
                Return False
            End If
        ElseIf t.Type = UnitType.PartOfSpeech Then
                Dim p As PartOfSpeech = t.PartOfSpeech
                Dim found As Boolean = False
            'TODO: Add "wildcard" types
            If w._Type.ToUpper Like p.Type & "*" Then
                found = True
                If Not IsNothing(ids) AndAlso ids.ContainsKey(p.Id) Then
                    If Not IsNothing(p.Parsing) Then
                        found = p.Parsing.Equals(w.Parsing) And w.Parsing.Equals(ids(p.Id))
                    Else
                        found = w.Parsing.Equals(ids(p.Id))
                    End If
                Else
                    If Not IsNothing(p.Parsing) Then
                        found = p.Parsing.Equals(w.Parsing)
                    End If
                End If
            End If
            Return found
            Else
                Return False
        End If
    End Function

    Private Function FuncSimple(index As Integer, u As Unit) As Result
        Dim res As New Result

        Dim refs As New Reference
        Dim found As Boolean

        Dim ids As New Dictionary(Of Integer, Parsing)

        If Match(u.Tokens.First, Me.Text(index), CType(IIf(ids.Count > 0, ids, Nothing), Dictionary(Of Integer, Parsing))) Then
            found = True
            Dim p As PartOfSpeech = u.Tokens.First.PartOfSpeech
            If Not ids.ContainsKey(p.Id) Then
                ids.Add(p.Id, Me.Text(index).Parsing)
            End If
            refs.AddRange(Me.Text(index).Chapter, Me.Text(index).Verse)
            refs.Book = Me.Text(index).Book

            Dim wordIndex As Integer = 1
            For i As Integer = 1 To u.Tokens.Count - 1
                Dim flexibleSearch As Boolean = Globals.FlexibleSearch

                If (wordIndex + index) < Me.Text.Count Then
                    If u.Tokens(i).Type = UnitType.Expansion Then
                        flexibleSearch = True
                    Else
                        If Not Match(u.Tokens(i), Me.Text(index + wordIndex), CType(IIf(ids.Count > 0, ids, Nothing), Dictionary(Of Integer, Parsing))) Then
                            If flexibleSearch Then 'TODO: fix Flexible Search...
                                For x As Integer = wordIndex + 1 To wordIndex + 3
                                    If Match(u.Tokens(i), Me.Text(index + x), CType(IIf(ids.Count > 0, ids, Nothing), Dictionary(Of Integer, Parsing))) Then
                                        found = True
                                        refs.AddRange(Me.Text(index + wordIndex).Chapter, Me.Text(index + wordIndex).Verse)
                                        wordIndex = x
                                        Exit For
                                    End If
                                Next
                                If Not found Then
                                    Exit For
                                End If
                            Else
                                found = False
                                Exit For
                            End If
                        Else
                            refs.AddRange(Me.Text(index + wordIndex).Chapter, Me.Text(index + wordIndex).Verse)
                        End If
                        wordIndex += 1
                    End If

                End If
            Next
        End If

        If found Then
            res.Success = True
            res.References.Add(refs)
        End If

        Return res
    End Function

    Private Function FuncProximity(index As Integer, x As Unit, y As Unit, dist As Integer, Optional forward As Boolean = True, Optional backward As Boolean = True, Optional unit As String = "WORDS") As Result
        Dim res As Result = FuncSimple(index, x)

        If res.Success Then
            Dim found_f As Boolean = False
            Dim found_b As Boolean = False
            For i = 1 To dist
                If forward Then

                    If (index + i) < Me.Text.Count Then
                        Dim tmp As Result = FuncSimple(index + i, y)
                        If tmp.Success Then
                            res.References.First.EndChapter = tmp.References.First.StartChapter
                            res.References.First.EndVerse = tmp.References.First.StartVerse
                            found_f = True
                            Exit For
                        End If
                    End If
                End If
                If backward Then
                    If (index - i) > 0 Then
                        Dim tmp As Result = FuncSimple(index - i, y)
                        If tmp.Success Then
                            res.References.First.EndChapter = tmp.References.First.StartChapter
                            res.References.First.EndVerse = tmp.References.First.StartVerse
                            found_b = True
                            Exit For
                        End If
                    End If
                End If
            Next
            If Not found_f And forward Then
                If Not found_b Then
                    res.Success = False
                End If
            End If
            If Not found_b And backward Then
                If Not found_f Then
                    res.Success = False
                End If
            End If
        End If


        Return res
    End Function


End Class
