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

Imports Microsoft.VisualBasic.FileIO

Public Module Import
    Public Function Lexicon(filename As String) As Lexicon
        Dim f As TextFieldParser = My.Computer.FileSystem.OpenTextFieldParser(filename, ",")
        Dim l As New Lexicon

        Do Until f.EndOfData
            Dim data() As String = f.ReadFields
            Dim e As New LexicalEntry
            e.ID = data(0)
            e.lemma = data(1)
            e.xlit = data(2)
            e.pronounce = data(3)
            e.description = data(4)
            e.PartOfSpeech = data(5)
            e.Language = data(6)
            e.simpleform = ParsingFunctions.RemoveDiacriticals(e.lemma)
            l.Entry.Add(e.ID, e)
        Loop

        f.Close()

        Return l
    End Function
End Module

Public Class LexicalEntry
    Public ID As String
    Public lemma As String
    Public xlit As String
    Public pronounce As String
    Public description As String
    Public PartOfSpeech As String
    Public Language As String
    Public simpleform As String
End Class

Public Class Lexicon
    Public Entry As New Dictionary(Of String, LexicalEntry)
    Public Name As String
End Class