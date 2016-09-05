<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBible
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBible))
        Me.cmbBible = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbVerse = New System.Windows.Forms.ComboBox()
        Me.cmbChap = New System.Windows.Forms.ComboBox()
        Me.cmbBook = New System.Windows.Forms.ComboBox()
        Me.txt = New Rhema.Viewer()
        Me.SuspendLayout()
        '
        'cmbBible
        '
        Me.cmbBible.FormattingEnabled = True
        Me.cmbBible.Location = New System.Drawing.Point(44, 3)
        Me.cmbBible.Name = "cmbBible"
        Me.cmbBible.Size = New System.Drawing.Size(192, 21)
        Me.cmbBible.TabIndex = 18
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Bible:"
        '
        'cmbVerse
        '
        Me.cmbVerse.FormattingEnabled = True
        Me.cmbVerse.Location = New System.Drawing.Point(190, 30)
        Me.cmbVerse.Name = "cmbVerse"
        Me.cmbVerse.Size = New System.Drawing.Size(46, 21)
        Me.cmbVerse.TabIndex = 16
        '
        'cmbChap
        '
        Me.cmbChap.FormattingEnabled = True
        Me.cmbChap.Location = New System.Drawing.Point(131, 30)
        Me.cmbChap.Name = "cmbChap"
        Me.cmbChap.Size = New System.Drawing.Size(53, 21)
        Me.cmbChap.TabIndex = 15
        '
        'cmbBook
        '
        Me.cmbBook.FormattingEnabled = True
        Me.cmbBook.Location = New System.Drawing.Point(4, 30)
        Me.cmbBook.Name = "cmbBook"
        Me.cmbBook.Size = New System.Drawing.Size(121, 21)
        Me.cmbBook.TabIndex = 14
        '
        'txt
        '
        Me.txt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txt.Location = New System.Drawing.Point(4, 57)
        Me.txt.MinimumSize = New System.Drawing.Size(20, 20)
        Me.txt.Name = "txt"
        Me.txt.Size = New System.Drawing.Size(232, 295)
        Me.txt.TabIndex = 19
        '
        'frmBible
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(244, 364)
        Me.Controls.Add(Me.txt)
        Me.Controls.Add(Me.cmbBible)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbVerse)
        Me.Controls.Add(Me.cmbChap)
        Me.Controls.Add(Me.cmbBook)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmBible"
        Me.Text = "Bible"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbBible As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbVerse As ComboBox
    Friend WithEvents cmbChap As ComboBox
    Friend WithEvents cmbBook As ComboBox
    Friend WithEvents txt As Viewer
End Class
