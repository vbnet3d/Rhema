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

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        cmbBible.Items.AddRange(BibleList.ToArray)
        cmbBible.Text = BibleList.Last
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

    Private Sub frmMain_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        If Not IsNothing(Me.ParentForm) Then
            Me.ParentForm.Close()
        End If
    End Sub
End Class
