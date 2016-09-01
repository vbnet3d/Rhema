<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.txt = New System.Windows.Forms.TextBox()
        Me.cmbBook = New System.Windows.Forms.ComboBox()
        Me.cmbChap = New System.Windows.Forms.ComboBox()
        Me.cmbVerse = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblResults = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbBible = New System.Windows.Forms.ComboBox()
        Me.GreekText1 = New Rhema.GreekText()
        Me.SuspendLayout()
        '
        'txt
        '
        Me.txt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txt.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt.Location = New System.Drawing.Point(3, 86)
        Me.txt.Multiline = True
        Me.txt.Name = "txt"
        Me.txt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt.Size = New System.Drawing.Size(232, 254)
        Me.txt.TabIndex = 0
        '
        'cmbBook
        '
        Me.cmbBook.FormattingEnabled = True
        Me.cmbBook.Location = New System.Drawing.Point(3, 53)
        Me.cmbBook.Name = "cmbBook"
        Me.cmbBook.Size = New System.Drawing.Size(121, 21)
        Me.cmbBook.TabIndex = 1
        '
        'cmbChap
        '
        Me.cmbChap.FormattingEnabled = True
        Me.cmbChap.Location = New System.Drawing.Point(130, 53)
        Me.cmbChap.Name = "cmbChap"
        Me.cmbChap.Size = New System.Drawing.Size(53, 21)
        Me.cmbChap.TabIndex = 2
        '
        'cmbVerse
        '
        Me.cmbVerse.FormattingEnabled = True
        Me.cmbVerse.Location = New System.Drawing.Point(189, 53)
        Me.cmbVerse.Name = "cmbVerse"
        Me.cmbVerse.Size = New System.Drawing.Size(46, 21)
        Me.cmbVerse.TabIndex = 3
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(596, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 20)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "Search"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(244, 52)
        Me.txtSearch.Multiline = True
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSearch.Size = New System.Drawing.Size(427, 288)
        Me.txtSearch.TabIndex = 8
        '
        'lblResults
        '
        Me.lblResults.AutoSize = True
        Me.lblResults.Location = New System.Drawing.Point(374, 27)
        Me.lblResults.Name = "lblResults"
        Me.lblResults.Size = New System.Drawing.Size(0, 13)
        Me.lblResults.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Bible:"
        '
        'cmbBible
        '
        Me.cmbBible.FormattingEnabled = True
        Me.cmbBible.Location = New System.Drawing.Point(43, 6)
        Me.cmbBible.Name = "cmbBible"
        Me.cmbBible.Size = New System.Drawing.Size(192, 21)
        Me.cmbBible.TabIndex = 12
        '
        'GreekText1
        '
        Me.GreekText1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GreekText1.Location = New System.Drawing.Point(244, 2)
        Me.GreekText1.Name = "GreekText1"
        Me.GreekText1.Size = New System.Drawing.Size(346, 26)
        Me.GreekText1.TabIndex = 6
        '
        'frmMain
        '
        Me.AcceptButton = Me.Button1
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(683, 352)
        Me.Controls.Add(Me.cmbBible)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblResults)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GreekText1)
        Me.Controls.Add(Me.cmbVerse)
        Me.Controls.Add(Me.cmbChap)
        Me.Controls.Add(Me.cmbBook)
        Me.Controls.Add(Me.txt)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "Rhema"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txt As System.Windows.Forms.TextBox
    Friend WithEvents cmbBook As System.Windows.Forms.ComboBox
    Friend WithEvents cmbChap As System.Windows.Forms.ComboBox
    Friend WithEvents cmbVerse As System.Windows.Forms.ComboBox
    Friend WithEvents GreekText1 As Rhema.GreekText
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblResults As System.Windows.Forms.Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbBible As ComboBox
End Class
