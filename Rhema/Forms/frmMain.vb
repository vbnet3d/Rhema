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

Public Class frmMain
    Public Property curBible As Bible
    Public Property curFtBible As FullTextBible
    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        'curFtBible.Search2("εν αρχη ην ο λογος <OR> [ARTICLE 1 **S] [NOUN 1] και [NOUN 1]") ' <OR> αυτος <WITHIN 5 WORDS> ανθρωπος <OR> [ARTICLE 1] [NOUN 1] και [NOUN 1]
        cmbBible.Items.AddRange(BibleList.ToArray)
        cmbBible.Text = BibleList.Last
        curBible = Bibles.Last
        curFtBible = ftBibles.Last
        'Me.Text = "Converting TR"
        'BibleData.ConvertUnboundToRhema("greek_textus_receptus_parsed_utf8.txt", "./bibles/Textus Receptus.bible", "book_names.txt")
        ''Me.Text = "Converting LXX"
        ''BibleData.ConvertUnboundToRhema("lxx_a_parsing_unaccented_utf8.txt", "./bibles/LXX.bible", "book_names.txt")
        ''Application.Exit()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim search As New System.Text.StringBuilder

        Dim l As List(Of Verse) = curBible.GetReference(curFtBible.Search(GreekText1.Text, cmbStart.SelectedIndex, cmbEnd.SelectedIndex).ToArray)
        Dim i As Integer
        For i = 0 To l.Count - 1
            search.Append(l(i).Book & " " & (l(i).Chapter) & ":" & (l(i).Verse) & " " & l(i).RawText & vbCrLf)
        Next
        txtSearch.Text = search.ToString()

        Me.Text = String.Format("Search - {0} : {1} Result(s)", GreekText1.Text, l.Count)

    End Sub

    Private Sub cmbBible_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbBible.SelectedIndexChanged
        For i As Integer = 0 To ftBibles.Count - 1
            If ftBibles(i).Name = cmbBible.Text Then
                curFtBible = ftBibles(i)
                curBible = Bibles(i)
            End If
        Next


        cmbStart.Items.Clear()
        cmbEnd.Items.Clear()

        For Each b As String In curBible.BookList
            cmbStart.Items.Add(b)
            cmbEnd.Items.Add(b)
        Next

        cmbStart.SelectedIndex = 0
        cmbEnd.SelectedIndex = cmbEnd.Items.Count - 1

    End Sub


End Class
