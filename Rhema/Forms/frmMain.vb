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

Imports System.IO
Imports System.Reflection

Public Class frmMain
    Dim curBible As Bible
    Dim curFtBible As FullTextBible
    Dim ftBibles As New List(Of FullTextBible)
    Dim Lexicon As Lexicon
    Dim Bibles As New List(Of Bible)
    Dim b As Book
    Dim c As Chapter

    Dim _assembly As [Assembly]

    Dim ac As AutoCompleteStringCollection


    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        _assembly = [Assembly].GetExecutingAssembly()

        createBible()
    End Sub

    Private Sub createBible()
        Dim list() As String = _assembly.GetManifestResourceNames
        For Each s As String In list
            If s.Contains("bible") Then
                Using _s = New StreamReader(_assembly.GetManifestResourceStream(s))
                    Dim name As String = s.Substring(s.IndexOf(".") + 1, s.LastIndexOf(".") - s.IndexOf(".") - 1)
                    Dim fb As FullTextBible = BibleData.Load(_s, name)
                    Bibles.Add(fb.ToBible)
                    ftBibles.Add(fb)
                    cmbBible.Items.Add(fb.Name)
                End Using
            End If
        Next

        Dim d As New IO.DirectoryInfo(".\bibles")
        Dim files As IO.FileInfo() = d.GetFiles
        For Each f As IO.FileInfo In files
            If f.Extension.Contains("bible") Then
                If Not cmbBible.Items.Contains(f.Name.Replace(".bible", "")) Then
                    Dim fb As FullTextBible = BibleData.Load(f.FullName)
                    Bibles.Add(fb.ToBible)
                    ftBibles.Add(fb)
                    cmbBible.Items.Add(fb.Name)
                End If
            End If
        Next

        curBible = Bibles.Last
        curFtBible = ftBibles.Last
        cmbBible.Text = curBible.Name

        'ac = New AutoCompleteStringCollection
        'Dim strongs As Lexicon = Import.Lexicon(".\strongs.csv")
        'For Each e As LexicalEntry In strongs.Entry.Values
        '    ac.Add(e.simpleform)
        'Next
        'Me.GreekText1.AutoCompleteCustomSource = ac
        'Me.GreekText1.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        'Me.GreekText1.AutoCompleteSource = AutoCompleteSource.CustomSource
    End Sub

    Private Sub cmbBook_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBook.SelectedIndexChanged
        Dim i As Integer
        cmbChap.Items.Clear()
        b = curBible.Books(cmbBook.Text)
        For i = 1 To b.Chapters.Count
            cmbChap.Items.Add(i)
        Next
    End Sub

    Private Sub cmbChap_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbChap.SelectedIndexChanged
        Dim i As Integer
        cmbVerse.Items.Clear()
        For i = 1 To b.Chapters(cmbChap.SelectedIndex).Verse.Count
            cmbVerse.Items.Add(i)
        Next
    End Sub

    Private Sub cmbVerse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVerse.SelectedIndexChanged
        txt.Text = curBible.GetVerseText(cmbBook.Text, cmbChap.SelectedIndex + 1, cmbVerse.SelectedIndex + 1)
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim search As New System.Text.StringBuilder
        txtSearch.Text = ""
        Dim l As List(Of Verse) = curBible.GetReference(curFtBible.Search(GreekText1.Text).ToArray)
        Dim i As Integer
        For i = 0 To l.Count - 1
            search.Append(l(i).Book & " " & (l(i).Chapter + 1) & ":" & (l(i).Verse) & " " & l(i).RawText & vbCrLf)
        Next
        txtSearch.Text = search.ToString
        lblResults.Text = l.Count & " Result(s)"
    End Sub

    Private Sub cmbBible_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBible.SelectedIndexChanged
        For i As Integer = 0 To ftBibles.Count - 1
            If ftBibles(i).Name = cmbBible.Text Then
                curFtBible = ftBibles(i)
                curBible = Bibles(i)
                cmbBook.Items.Clear()
                cmbBook.Items.AddRange(curBible.BookList())
            End If
        Next
    End Sub
End Class
