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

Imports System.IO
Imports System.Reflection

Public Module Globals
    Public FlexibleSearch As Boolean = False
End Module

Public Module Core

    Public ftBibles As New List(Of FullTextBible)
    Public Lexicon As Lexicon
    Public Bibles As New List(Of Bible)
    Public BibleList As New List(Of String)
    Public AutoCompleteList As New List(Of String)
    Public Jquery As String
    Public Scripts As String
    Public b As Book
    Public c As Chapter
    Dim _assembly As [Assembly]

    Public Sub Initialize()
        _assembly = [Assembly].GetExecutingAssembly()

        Dim rgx_any As String = "[\[][A-Za-z0-9Α-ῼ ]*[\]]"

        AutoCompleteList.Add("<WITHIN # WORDS>")
        AutoCompleteList.Add("<AND>")
        AutoCompleteList.Add("<OR>")
        AutoCompleteList.Add("<NOT>")
        AutoCompleteList.Add("<XOR>")
        AutoCompleteList.Add("<FOLLOWEDBY # WORDS>")
        AutoCompleteList.Add("<PRECEDEDBY # WORDS>")

        Dim list() As String = _assembly.GetManifestResourceNames
        For Each s As String In list
            If s.Contains("bible") Then
                Using _s = New StreamReader(_assembly.GetManifestResourceStream(s))
                    Dim name As String = s.Substring(s.IndexOf(".") + 1, s.LastIndexOf(".") - s.IndexOf(".") - 1)
                    Dim fb As FullTextBible = BibleData.Load(_s, name)
                    Bibles.Add(fb.ToBible)
                    ftBibles.Add(fb)
                    BibleList.Add(fb.Name)
                End Using
            ElseIf s.Contains(".js") AndAlso s.Contains("jquery") Then
                Using _s = New StreamReader(_assembly.GetManifestResourceStream(s))
                    Jquery = _s.ReadToEnd()
                End Using
            ElseIf s.Contains(".js") AndAlso s.Contains("script") Then
                Using _s = New StreamReader(_assembly.GetManifestResourceStream(s))
                    Scripts = _s.ReadToEnd()
                End Using
            End If
        Next

        Try
            Dim d As New IO.DirectoryInfo(".\bibles")
            Dim files As IO.FileInfo() = d.GetFiles
            For Each f As IO.FileInfo In files
                If f.Extension.Contains("bible") Then
                    If Not BibleList.Contains(f.Name.Replace(".bible", "")) Then
                        Dim fb As FullTextBible = BibleData.Load(f.FullName)
                        Bibles.Add(fb.ToBible)
                        ftBibles.Add(fb)
                        BibleList.Add(fb.Name)
                    End If
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try



    End Sub
End Module
