Option Explicit On
Option Strict On

Imports System.Text.RegularExpressions

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
                            For b As Integer = i To (i - CInt(c.Options(0))) Step -1
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
                            For f As Integer = i To (i + CInt(c.Options(0)))
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
                            For b As Integer = i To (i - CInt(c.Options(0))) Step -1
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
                            For f As Integer = i To (i + CInt(c.Options(0)))
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
