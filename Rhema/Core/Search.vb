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
Imports Rhema

Public Module Search


    Public TokenDefinitions As New List(Of Regex)

    Public Function Search(units As Unit()) As Boolean
        'TODO: build search string as trues/falses with And, Or, Xor, and Not
        Dim s As String = "True Or True"
        Dim script As String = <script>
                                   If <%= s %> Then
                                        Return True
                                   Else
                                        Return False
                                   End If
                               </script>.ToString
        Return CBool(Compiler.Run(script))
    End Function
    Public Function Tokenize(s As String) As Token()
        If TokenDefinitions.Count <= 0 Then
            TokenDefinitions.Add(New Regex("<[^GH][A-Za-z0-9 ]*>", RegexOptions.Compiled))
            TokenDefinitions.Add(New Regex("<[GH][0-9]*>", RegexOptions.Compiled))
            TokenDefinitions.Add(New Regex("[\[][A-Za-z0-9 ]*[\]]", RegexOptions.Compiled))
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

    Public Function Unitize(tokens As Token()) As Unit()
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

End Module

Public Class Token
    Implements IComparable(Of Token)

    Public Raw As String
    Public Index As Integer
    Public LastIndex As Integer
    Public Type As UnitType

    Public Function CompareTo(other As Token) As Integer Implements IComparable(Of Token).CompareTo
        Return Me.Index.CompareTo(other.Index)
    End Function
End Class

Public Class LiteralToken
    Inherits Token
End Class

Public Class StrongsToken
    Inherits Token
    Public StrongsNumber As String
End Class

Public Class PartOfSpeechToken
    Inherits Token
    Public PartOfSpeech As String
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
    Public Y As String 'TODO: Replace with Unit
    Public Options As New List(Of String)
    Public Result As Boolean
End Class

Public Module Parsing
    Public Function RemoveDiacriticals(input As String) As String
        Dim decomposed As String = input.Normalize(NormalizationForm.FormD)
        Dim filtered As Char() = decomposed.Where(Function(c) Char.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark).ToArray()
        Dim newString As String = New [String](filtered)
        Return newString
    End Function
End Module