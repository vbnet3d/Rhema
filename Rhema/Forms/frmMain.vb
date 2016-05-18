Option Explicit On
Option Strict On

Public Class frmMain
    Dim curBible As Bible
    Dim b As Book
    Dim c As Chapter
    Dim fb As FullTextBible
    Dim ac As AutoCompleteStringCollection


    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Text = "Loading data..."
        createBible()
    End Sub

    Private Sub createBible()
        'curBible = New Bible("LXX", ".\lxx_a_parsing_unaccented_utf8.txt")
        ' ConvertUnboundToRhema(".\lxx_a_parsing_unaccented_utf8.txt", ".\bibles\LXX.txt", ".\book_names.txt")
        'tr = New Bible("Textus Receptus", ".\greek_textus_receptus_parsed_utf8.txt")
        'tr.Merge(lxx)
        'BibleData.Save(tr)
        ConvertUnboundToRhema(".\greek_textus_receptus_parsed_utf8.txt", ".\bibles\TR.txt", ".\book_names.txt")
        'cmbBook.Items.AddRange(tr.BookList)

        Me.Text = "Rhema"

        fb = BibleData.Load(".\bibles\LXX.bible")

        ac = New AutoCompleteStringCollection
        Dim strongs As Lexicon = Import.Lexicon(".\strongs.csv")
        For Each e As LexicalEntry In strongs.Entry.Values
            ac.Add(e.simpleform)
        Next
        Me.GreekText1.AutoCompleteCustomSource = ac
        Me.GreekText1.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Me.GreekText1.AutoCompleteSource = AutoCompleteSource.CustomSource
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
        txt.Text = curBible.GetVerseText(cmbBook.Text, cmbChap.SelectedIndex + 1, cmbVerse.SelectedIndex + 1, 5)
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim search As New System.Text.StringBuilder
        txtSearch.Text = ""
        Dim l As List(Of VerseParser) = curBible.GetReference(fb.Search(GreekText1.Text).ToArray)
        Dim i As Integer
        For i = 0 To l.Count - 1
            search.Append(l(i).Book & " " & (l(i).Chapter + 1) & ":" & (l(i).Verse) & " " & l(i).RawText & vbCrLf)
        Next
        txtSearch.Text = search.ToString
        lblResults.Text = l.Count & " Result(s)"
    End Sub
End Class
