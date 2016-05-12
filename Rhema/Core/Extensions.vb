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
        Dim rgx As New Regex("[^Α-Ωα-ω]", RegexOptions.Compiled)
        Return rgx.Replace(input, "")
    End Function

    <System.Runtime.CompilerServices.Extension>
    Public Function ToHebrewLettersOnly(input As String) As String
        Dim rgx As New Regex("[^א-ת]", RegexOptions.Compiled)
        Return rgx.Replace(input, "")
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

