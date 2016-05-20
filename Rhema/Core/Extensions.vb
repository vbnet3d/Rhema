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

Imports System.Text.RegularExpressions

Public Module RegexConvert

    <System.Runtime.CompilerServices.Extension>
    Public Function ToAlphaNumericOnly(input As String) As String
        Dim rgx As New Regex("[^a-zA-Z0-9]", RegexOptions.Compiled)
        Return rgx.Replace(input, "")
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToAlphaOnly(input As String) As String
        Dim rgx As New Regex("[^a-zA-Z]", RegexOptions.Compiled)
        Return rgx.Replace(input, "")
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToGreekLettersOnly(input As String) As String
        Return ParsingFunctions.RemoveDiacriticals(input)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToHebrewLettersOnly(input As String) As String
        Return ParsingFunctions.RemoveDiacriticals(input)
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToNumericOnly(input As String) As Integer
        Dim rgx As New Regex("[^0-9]", RegexOptions.Compiled)
        Dim result As String = rgx.Replace(input, "")
        Dim val As Integer
        If Integer.TryParse(result, val) Then
            Return val
        Else
            Return 0
        End If
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToNumericOrDecimal(input As String) As Decimal
        Dim rgx As New Regex("[^0-9\.]", RegexOptions.Compiled)
        Dim result As String = rgx.Replace(input, "")
        Dim val As Decimal
        If Decimal.TryParse(result, val) Then
            Return val
        Else
            Return 0.0D
        End If
    End Function

End Module

