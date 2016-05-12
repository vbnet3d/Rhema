Imports System.Threading

Public Class frmMain
    Dim tr As Bible
    Dim b As Book
    Dim c As Chapter
    Dim fb As FullTextBible
    Dim ac As AutoCompleteStringCollection


    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Text = "Loading data..."
        createBible()
    End Sub

    Private Sub createBible()
        tr = New Bible("Textus Receptus", ".\greek_textus_receptus_parsed_utf8.txt")

        cmbBook.Items.AddRange(tr.BookList)

        Me.Text = "GNT Explorer"

        ac = New AutoCompleteStringCollection
        Dim strongs As Lexicon = Import.Lexicon(".\strongs.csv")
        For Each e As LexicalEntry In strongs.Entry.Values
            ac.Add(e.simpleform)
        Next
        Me.GreekText1.AutoCompleteCustomSource = ac
        Me.GreekText1.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Me.GreekText1.AutoCompleteSource = AutoCompleteSource.CustomSource

        fb = tr.ToFullText
        Dim refs As Reference() = (fb.Search("αλλος <WITHIN 25 Words> αυτου").ToArray())
        For Each r As Reference In refs
            Dim verses() As VerseParser = tr.GetReference(r)
            Dim txt As New System.Text.StringBuilder(r.ToString & " ")
            For Each v As VerseParser In verses
                txt.AppendLine(v.RawText)
            Next
            ListBox1.Items.Add(txt.ToString)
        Next

    End Sub

    Private Sub cmbBook_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbBook.SelectedIndexChanged
        Dim i As Integer
        cmbChap.Items.Clear()
        b = tr.Book(tr.BibleParser.ReverseList(cmbBook.Text))
        For i = 1 To b.Chapter.Count
            cmbChap.Items.Add(i)
        Next

    End Sub

    Private Sub cmbChap_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbChap.SelectedIndexChanged
        Dim i As Integer
        cmbVerse.Items.Clear()
        For i = 1 To b.Chapter(cmbChap.SelectedIndex).Verse.Count
            cmbVerse.Items.Add(i)
        Next
    End Sub

    Private Sub cmbVerse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbVerse.SelectedIndexChanged
        txt.Text = tr.GetVerseText(cmbBook.Text, cmbChap.SelectedIndex + 1, cmbVerse.SelectedIndex + 1, 5)
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        txtSearch.Text = ""
        Dim l As List(Of VerseParser) = tr.GetReference(fb.Search(GreekText1.Text).ToArray)
        Dim i As Integer
        For i = 0 To l.Count - 1
            txtSearch.Text &= l(i).Book & " " & (l(i).Chapter + 1) & ":" & (l(i).Verse) & " " & l(i).RawText & vbCrLf
        Next
        lblResults.Text = l.Count & " Result(s)"
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Text = tr.Status
    End Sub
End Class
