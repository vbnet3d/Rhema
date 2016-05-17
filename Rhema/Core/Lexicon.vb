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
            e.simpleform = Parsing.RemoveDiacriticals(e.lemma)
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