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
Imports System.Text.RegularExpressions

Public Module Evaluate
    Public Function [And](c1 As Condition, c2 As Condition) As Boolean
        If IsNothing(c1.Result) Then Return False
        If IsNothing(c2.Result) Then Return False
        If c1.Result.Success And c2.Result.Success Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function [Or](c1 As Condition, c2 As Condition) As Boolean
        If IsNothing(c1.Result) Then Return False
        If IsNothing(c2.Result) Then Return False
        If c1.Result.Success Or c2.Result.Success Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function [Not](c1 As Condition, c2 As Condition) As Boolean
        If IsNothing(c1.Result) Then Return False
        If IsNothing(c2.Result) Then Return False
        If c1.Result.Success And Not c2.Result.Success Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function [Xor](c1 As Condition, c2 As Condition) As Boolean
        If IsNothing(c1.Result) Then Return False
        If IsNothing(c2.Result) Then Return False
        If c1.Result.Success Xor c2.Result.Success Then
            Return True
        Else
            Return False
        End If
    End Function
End Module

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

Public Class Parsing
    Public Id As Integer
    Public Gender As String
    Public Number As String
    Public [Case] As String
    Public Tense As String
    Public Voice As String
    Public Mood As String
    Public Person As String

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim res As Boolean = False
        Dim p As Parsing = TryCast(obj, Parsing)
        If Not IsNothing(p) Then
            If Not IsNothing(Me.Gender) Then
                If Me.Gender = p.Gender Then
                    res = True
                Else
                    res = False
                End If
            End If
            If Not IsNothing(Me.Case) Then
                If Me.Case = p.Case Then
                    res = True
                Else
                    res = False
                End If
            End If
            If Not IsNothing(Me.Number) Then
                If Me.Number = p.Number Then
                    res = True
                Else
                    res = False
                End If
            End If
            If Not IsNothing(Me.Tense) Then
                If Me.Tense = p.Tense Then
                    res = True
                Else
                    res = False
                End If
            End If
            If Not IsNothing(Me.Voice) Then
                If Me.Voice = p.Voice Then
                    res = True
                Else
                    res = False
                End If
            End If
            If Not IsNothing(Me.Mood) Then
                If Me.Mood = p.Mood Then
                    res = True
                Else
                    res = False
                End If
            End If
            If Not IsNothing(Me.Person) Then
                If Me.Person = p.Person Then
                    res = True
                Else
                    res = False
                End If
            End If
        Else
            Return False
        End If


        Return res
    End Function
End Class

Public Class PartOfSpeech
    Public Type As String
    Public Id As Integer = -1
    Public Parsing As Parsing

    Public Sub New(s As String)
        s = s.Replace("[", "").Replace("]", "")
        Dim data() As String = s.Split(CType(" ", Char()))
        Type = data(0)
        For i = 1 To data.Length - 1
            If IsNumeric(data(i)) Then
                id = CInt(data(i))
            Else
                Parsing = New Parsing()
                Parsing.Id = Id
                'GNC for substantives or TVMPN for standard Greek verbs. Greek Participle parsing Is in the form of TVMGNC.
                If Type.ToUpper = "VERB" Then
                    Parsing.Tense = IIf(data(i)(0).ToString = "*", Nothing, data(i)(0).ToString).ToString
                    Parsing.Voice = IIf(data(i)(1).ToString = "*", Nothing, data(i)(1).ToString).ToString
                    Parsing.Mood = IIf(data(i)(2).ToString = "*", Nothing, data(i)(2).ToString).ToString
                    If data(i).Length = 6 Then
                        'Participles
                        Parsing.Gender = IIf(data(i)(3).ToString = "*", Nothing, data(i)(0).ToString).ToString
                        Parsing.Number = IIf(data(i)(4).ToString = "*", Nothing, data(i)(1).ToString).ToString
                        Parsing.Case = IIf(data(i)(5).ToString = "*", Nothing, data(i)(2).ToString).ToString
                    End If
                Else
                    Parsing.Gender = IIf(data(i)(0).ToString = "*", Nothing, data(i)(0).ToString).ToString
                    Parsing.Number = IIf(data(i)(1).ToString = "*", Nothing, data(i)(1).ToString).ToString
                    Parsing.Case = IIf(data(i)(2).ToString = "*", Nothing, data(i)(2).ToString).ToString
                End If
            End If
        Next
    End Sub
End Class

Public Class Token
    Implements IComparable(Of Token)

    Public Raw As String
    Public Index As Integer
    Public LastIndex As Integer
    Public Type As UnitType

    Public Function StrongsNumber() As String
        Return Raw.Replace("<", "").Replace(">", "")
    End Function

    Public Function PartOfSpeech() As PartOfSpeech
        Return New PartOfSpeech(Raw)
    End Function

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

Public Class Info
    Public Success As Boolean
    Public Conditions As New List(Of Condition)
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

Public Module ParsingFunctions
    Public Function RemoveDiacriticals(input As String) As String
        Dim decomposed As String = input.Normalize(NormalizationForm.FormD)
        Dim filtered As Char() = decomposed.Where(Function(c) Char.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark).ToArray()
        Dim newString As String = New [String](filtered)
        Return newString
    End Function
End Module