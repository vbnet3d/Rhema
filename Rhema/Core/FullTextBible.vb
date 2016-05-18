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

Public Class FullTextBible
    Public Text As New List(Of word)
    Public Name As String

    '****************************
    'REGEX Pattern Definitions
    '****************************
    Public Const WITHIN As String = "[\[\]\(\)\{\}, \u0040-\u4040A-Za-z0-9]*<[Ww][Ii][Tt][Hh][Ii][Nn][A-Za-z0-9 ]*>[\[\]\(\)\{\}, \u0040-\u4040A-Za-z0-9]*"
    Public Const [OR] As String = "<[Oo][Rr]>"
    Public Const [AND] As String = "<[Aa][Nn][Dd]>"
    Public Const [XOR] As String = "<[Xx][Oo][Rr]>"
    Public Const [NOT] As String = "<[Nn][Oo][Tt]>"
    Public Const STRONGS As String = "<[GH][0-9]*>"
    '*****************************

    '****************************
    'REGEX Object Definitions
    '****************************
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
        'Form: x <command_name options> y
        'Notes: x and y may be WORDS or other conditions.
        '*************************
        'x <WITHIN distance WORDS> y    Is word <x> within <distance> words of <y>, either direction?
        'x <OR> y                       Either <x> or <y> may be true
        'x <AND> y                      <x> and <y> must be true
        'x <NOT> y 'x must be true and y false
        'x <XOR> y 'only one of x/y may be true
        'x <FOLLOWED BY int WORDS> y
        'x <PRECEDED BY int WORDS> y
        '**************************
        '##########################
        'PARTS OF SPEECH
        '**************************
        'Form: [part_name id(numeric, optional) parsing(optional)]
        'Notes: Matching ids are used when you want to select different words with the basic parsing
        '       i.e. A Nom. Masc. Sg. Article and Nom. Masc. Sg. Noun, without specifying exactly what
        '       forms to return. This allows the user to search for all forms of a syntactic structure.
        '       Parsing is in the form of G(ender)N(umber)C(ase) for substantives or 
        '       T(ense)V(oice)M(ood)P(erson)N(umber) for standard Greek verbs. Greek Participle parsing
        '       Is in the form of T(ense)V(oice)M(ood)G(ender)N(umber)C(ase). Hebrew parsing will be
        '       added later.
        'TODO: Add Hebrew parsing
        'Parsing Options:
        '   Gender: M(asculine), F(eminine), N(euter)
        '   Number: S(ingular), P(lural), D(ual)
        '   Case:   N(ominative), D(ative), G(enitive), A(ccusative), V(ocative)
        '   Tense:  P(resent), A(orist), X(Perfect), I(mperfect), F(uture), Y(Pluperfect)
        '   Voice:  A(ctive), M(iddle), P(assive)
        '   Mood:   I(ndicative), D(Imperative), (I)N(finitive), P(articiple), S(ubjunctive), O(ptative)
        '   Person: 1, 2, 3
        '   [Hebrew Specific]
        '   Stem: Qal, Niphal, Hiphil, Hophal, Hithpael, etc... 
        '**************************
        '[ANY id parsing]           Matches any word unit
        '[ARTICLE id parsing]       Matches any article
        '[SUBSTANTIVE id parsing]   Matches any substantive word unit
        '[NOUN id parsing]          Matches any noun
        '[ANYPRONOUN id parsing]    Matches any pronoun
        '[PRONOUN id parsing]       Matches only regular pronouns
        '[REFLEXIVE id parsing]     Matches only reflexive pronouns
        '[INDEFINITE id parsing]    Matches only indefinite pronouns
        '[NUMBER id parsing]        Matches only indeclinable numbers
        '[PARTICLE id parsing]      Matches only particles
        '[VERB id parsing]          Matches only verbs
        '**************************
        '##########################
        'Strong's Number
        '**************************
        'Form: <G123>


        ' TODO: Change this to loop through all conditions in a group while on a specific word index
        ' And then evaluate the result of all the group conditions together before adding the pieces in.
        For Each group As List(Of Condition) In condition_groups
            For Each c As Condition In group
                For i As Integer = 0 To Me.Text.Count - 1
                    If c.Type = "WITHIN" Then
                        If Me.Text(i)._Text = c.X Then
                            For b As Integer = i To (i - CInt(c.Options(0))) Step -1
                                If Me.Text(b)._Text = c.Y Then
                                    Dim r As New Reference
                                    r.Book = Me.Text(i).Book
                                    r.AddRange(Me.Text(i).Chapter, Me.Text(i).Verse)
                                    r.AddRange(Me.Text(b).Chapter, Me.Text(b).Verse)
                                    c.Result = True
                                    If Not l.Contains(r) Then
                                        l.Add(r)
                                    End If
                                    Exit For
                                End If
                                If b = 0 Then
                                    Exit For
                                End If
                            Next
                            For f As Integer = i To (i + CInt(c.Options(0)))
                                If Me.Text(f)._Text = c.Y Then
                                    Dim r As New Reference
                                    r.Book = Me.Text(i).Book
                                    r.AddRange(Me.Text(i).Chapter, Me.Text(i).Verse)
                                    r.AddRange(Me.Text(f).Chapter, Me.Text(f).Verse)
                                    c.Result = True
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
                            For b As Integer = i To (i - CInt(c.Options(0))) Step -1
                                If Me.Text(b)._Text = c.X Then
                                    Dim r As New Reference
                                    r.Book = Me.Text(i).Book
                                    r.AddRange(Me.Text(i).Chapter, Me.Text(i).Verse)
                                    r.AddRange(Me.Text(b).Chapter, Me.Text(b).Verse)
                                    c.Result = True
                                    If Not l.Contains(r) Then
                                        l.Add(r)
                                    End If
                                    Exit For
                                End If
                                If b = 0 Then
                                    Exit For
                                End If
                            Next
                            For f As Integer = i To (i + CInt(c.Options(0)))
                                If Me.Text(f)._Text = c.X Then
                                    Dim r As New Reference
                                    r.Book = Me.Text(i).Book
                                    r.AddRange(Me.Text(i).Chapter, Me.Text(i).Verse)
                                    r.AddRange(Me.Text(f).Chapter, Me.Text(f).Verse)
                                    c.Result = True
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
                            c.Result = True
                        End If
                    End If
                Next
            Next
        Next

        Return l
    End Function


End Class
