﻿'The MIT License (MIT)

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

#Region "WORD"

'#######################################################################################
'WORD class - represents an individual word with all its parsing and lexical information
Public Class word
    Public _Text As String
    Public _Gender As String
    Public _Number As String
    Public _Type As String
    Public _Case As String
    Public _LexicalForm As String
    Public _Tense As String
    Public _Voice As String
    Public _Mood As String
    Public _Indeclinable As Boolean
    Public _Strongs() As String
    Public _Person As String
    Public StrongsNumber As List(Of String)
    Public Book As String
    Public Chapter As Integer
    Public Verse As Integer

    Public Sub New()
        StrongsNumber = New List(Of String)
    End Sub

    Public Function Parsing() As Parsing
        Dim p As New Parsing
        p.Gender = Me._Gender
        p.Number = Me._Number
        p.Case = Me._Case
        p.Tense = Me._Tense
        p.Voice = Me._Voice
        p.Mood = Me._Mood
        p.Person = Me._Person
        Return p
    End Function

    Public Sub New(ByVal text As String, ByVal strongs() As String, ByVal parsing As String)
        _Text = text
        _Strongs = strongs
        Dim i As Integer
        If Not IsNothing(_Strongs) Then
            For i = 0 To _Strongs.Length - 1
                StrongsNumber.Add(_Strongs(i))
            Next
        End If

        If parsing = "N-PRI" Or _Text.Contains("ιησου") Or _Text.Contains("χριστ") Then
            _Type = "Proper Name"
            _Indeclinable = True
            _Text = Char.ToUpper(_Text(0)) & _Text.Substring(1)
        ElseIf parsing = "CONJ" Then
            _Type = "Conjunction"
        ElseIf parsing = "PREP" Then
            _Type = "Preposition"
        ElseIf parsing.StartsWith("T-") Then
            _Type = "Article"
            parsing = parsing.Replace("T-", "")
        ElseIf parsing.StartsWith("I-") Then
            _Type = "Indefinite Pronoun"
            parsing = parsing.Replace("I-", "")
        ElseIf parsing.StartsWith("PRT") Then
            _Type = "Particle"
        ElseIf parsing.StartsWith("P-") Then
            _Type = "Pronoun"
            _Person = parsing.Substring(2, 1)
            _Case = parsing.Substring(3, 1)
            _Number = parsing.Substring(4, 1)
        ElseIf parsing.Contains("HEB") Then
            _Type = "Hebraic"
        ElseIf parsing.StartsWith("N-") Then
            If parsing.Contains("OI") Then
                _Type = "Indeclinable Noun"
                _Indeclinable = True
            ElseIf parsing.Contains("LI") Then
                _Type = "Letter"
            Else
                _Type = "Noun"
            End If
            parsing = parsing.Replace("N-", "")
        ElseIf parsing.StartsWith("A-") Then
            _Type = "Adjective"
            parsing = parsing.Replace("A-", "")
        ElseIf parsing.StartsWith("V-") Then
            'V-AAI-3S
            parsing = parsing.Replace("V-", "")
            If IsNumeric(parsing.Substring(0, 1)) Then
                parsing = parsing.Substring(1)
            End If
            _Tense = parsing.Substring(0, 1)
            _Voice = parsing.Substring(1, 1)
            _Mood = parsing.Substring(2, 1)
            If _Mood = "P" Then 'Participle
                _Type = "Participle"
                _Case = parsing.Substring(4, 1)
                _Number = parsing.Substring(5, 1)
                _Gender = parsing.Substring(6, 1)
            ElseIf _Mood = "N" Then 'Infinitive
                _Type = "Infinitive"
            Else
                _Type = "Verb"
                _Person = parsing.Substring(4, 1)
                _Number = parsing.Substring(5, 1)
            End If
        End If

        If _Type = "Article" Or _Type = "Noun" Or _Type = "Adjective" Or _Type = "Indefinite Pronoun" Then
            _Case = parsing.Substring(0, 1)
            _Number = parsing.Substring(1, 1)
            _Gender = parsing.Substring(2, 1)
        End If
    End Sub
End Class
'#########################################################################################

#End Region