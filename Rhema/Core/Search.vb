Option Explicit On
Option Strict On

Imports System.Globalization
Imports System.Text

Public Class Condition
    Public Type As String
    Public X As String
    Public Y As String
    Public Options As New List(Of String)
End Class

Public Module Parsing
    Public Function RemoveDiacriticals(input As String) As String
        Dim decomposed As String = input.Normalize(NormalizationForm.FormD)
        Dim filtered As Char() = decomposed.Where(Function(c) Char.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark).ToArray()
        Dim newString As String = New [String](filtered)
        Return newString
    End Function
End Module