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

Imports System.Globalization
Imports System.Text

Public Class Result
    Public Success As Boolean = False
    Public References As New List(Of Reference)
    Public Overrides Function ToString() As String
        Dim str As New StringBuilder()

        For Each r As Reference In References
            str.Append(r.ToString & ",")
        Next

        Return str.ToString.Trim(CType(",", Char()))
    End Function
End Class

Public Class Token
    Implements IComparable(Of Token)

    Public Raw As String
    Public Index As Integer
    Public LastIndex As Integer
    Public Type As UnitType

    Public Function CompareTo(other As Token) As Integer Implements IComparable(Of Token).CompareTo
        Return Me.Index.CompareTo(other.Index)
    End Function

    Public Overrides Function ToString() As String
        Return Raw
    End Function
End Class

Public Enum UnitType As Integer
    Command = 0             'Search command (WITHIN, FOLLOWEDBY, PRECEDEDBY, AND, OR, NOT, XOR)
    Strongs = 1             'Strong's number, which returns all forms of the word
    PartOfSpeech = 2        'Parts of speech, i.e Nouns, Verbs, Articles, etc...
    Expansion = 3           'Use ellipses (...). allows to introduce flexibility into the search (+/- 3 tokens between defined tokens)
    Literal = 4             'Literal word/string to search for
    Complex = 5             'Combination of types
End Enum

' A Unit is a block of 1-n tokens, divided by search commands.
' Examples:
' Single Unit Search:   (αυτος και ανθρωπος)
' 3-Unit Search:     (αυτος) <WITHIN 5 WORDS> (ανθρωπος)
Public Class Unit
    Public Tokens As New List(Of Token)
    Public Type As UnitType
    Public Used As Boolean = False
    Public Overrides Function ToString() As String
        Dim s As New StringBuilder
        For Each t As Token In Tokens
            s.Append(t.Raw & " ")
        Next
        Return s.ToString.Trim
    End Function
End Class

Public Class Condition
    Public Type As String
    Public X As String 'TODO: Replace with Unit
    Public Unit1 As Unit
    Public Unit2 As Unit
    Public Y As String 'TODO: Replace with Unit
    Public Options As New List(Of String)
    Public Result As Result

    Public Overrides Function ToString() As String
        Dim str As New StringBuilder(Type)

        For Each o As String In Options
            str.Append(" " & o)
        Next
        If Not IsNothing(Unit1) Then
            str.Append(" x: " & Unit1.ToString)
        End If
        If Not IsNothing(Unit2) Then
            str.Append(" y: " & Unit2.ToString)
        End If
        If Not IsNothing(Result) Then
            str.Append(" result: " & Result.ToString)
        End If

        Return str.ToString
    End Function
End Class

Public Module Parsing
    Public Function RemoveDiacriticals(input As String) As String
        Dim decomposed As String = input.Normalize(NormalizationForm.FormD)
        Dim filtered As Char() = decomposed.Where(Function(c) Char.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark).ToArray()
        Dim newString As String = New [String](filtered)
        Return newString
    End Function
End Module