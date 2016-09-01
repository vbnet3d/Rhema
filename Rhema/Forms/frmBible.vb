Public Class frmBible
    Public Property curBible As Bible
    Public Property curFtBible As FullTextBible
    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        cmbBible.Items.AddRange(BibleList.ToArray)
        cmbBible.Text = BibleList.Last
        curBible = Bibles.Last
        curFtBible = ftBibles.Last
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