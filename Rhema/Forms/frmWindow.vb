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

Imports System.Windows.Forms

Public Class frmWindow

    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click
        ' Create a new instance of the child form.
        Dim ChildForm As New frmMain
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        ChildForm.Text = "Search"

        ChildForm.Show()
    End Sub

    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private Sub frmWindow_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ' Create a new instance of the child form.
        Dim ChildForm As New frmMain
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        ChildForm.Text = "Search"

        ChildForm.Show()

        ' Create a new instance of the child form.
        Dim ChildForm2 As New frmBible
        ' Make it a child of this MDI form before showing it.
        ChildForm2.MdiParent = Me

        ChildForm2.Show()

        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub frmWindow_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If Not IsNothing(Me.ParentForm) Then
            Me.ParentForm.Close()
        End If
        Application.Exit()
    End Sub

    Private Sub FileMenu_Click(sender As Object, e As EventArgs) Handles FileMenu.Click

    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        ' Create a new instance of the child form.
        Dim ChildForm As New frmBible
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        ChildForm.Show()
    End Sub

    Private Sub HorizontalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub VerticalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub CascadeToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub


    Private Sub UnboundBibleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnboundBibleToolStripMenuItem.Click
        If ofd.ShowDialog = DialogResult.OK Then
            inputFile = ofd.FileName
            outputFile = ofd.SafeFileName.Replace(".txt", "")
            bibleFile = inputFile.Replace(outputFile, "book_names")
            Dim t As New Threading.Thread(AddressOf ImportUB)
            t.Start()
        End If
    End Sub

    Private inputFile As String
    Private outputFile As String
    Private bibleFile As String
    Private done As Boolean
    Private Sub ImportUB()
        done = False
        BibleData.ConvertUnboundToRhema(inputFile, String.Format("./bibles/{0}.bible", outputFile), bibleFile)
        done = True
    End Sub

    Private Sub frmWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub UI_Tick(sender As Object, e As EventArgs) Handles UI.Tick
        'If Not done Then
        '    lblStatus.Text = "Processing file."
        'End If
        If done Then
            lblStatus.Text = "Finishing importing file."
        End If
    End Sub
End Class
