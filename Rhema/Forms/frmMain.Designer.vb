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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblResults = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbBible = New System.Windows.Forms.ComboBox()
        Me.cmbStart = New System.Windows.Forms.ComboBox()
        Me.cmbEnd = New System.Windows.Forms.ComboBox()
        Me.txtSearch = New Rhema.Viewer()
        Me.GreekText1 = New Rhema.GreekText()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(493, 33)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 26)
        Me.Button1.TabIndex = 7
        Me.Button1.Text = "Search"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'lblResults
        '
        Me.lblResults.AutoSize = True
        Me.lblResults.Location = New System.Drawing.Point(487, 11)
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
        'cmbStart
        '
        Me.cmbStart.FormattingEnabled = True
        Me.cmbStart.Location = New System.Drawing.Point(241, 6)
        Me.cmbStart.Name = "cmbStart"
        Me.cmbStart.Size = New System.Drawing.Size(121, 21)
        Me.cmbStart.TabIndex = 13
        '
        'cmbEnd
        '
        Me.cmbEnd.FormattingEnabled = True
        Me.cmbEnd.Location = New System.Drawing.Point(366, 6)
        Me.cmbEnd.Name = "cmbEnd"
        Me.cmbEnd.Size = New System.Drawing.Size(121, 21)
        Me.cmbEnd.TabIndex = 14
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(8, 70)
        Me.txtSearch.MinimumSize = New System.Drawing.Size(20, 20)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(559, 267)
        Me.txtSearch.TabIndex = 15
        '
        'GreekText1
        '
        Me.GreekText1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GreekText1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GreekText1.Location = New System.Drawing.Point(7, 33)
        Me.GreekText1.Name = "GreekText1"
        Me.GreekText1.Size = New System.Drawing.Size(480, 26)
        Me.GreekText1.TabIndex = 6
        '
        'frmMain
        '
        Me.AcceptButton = Me.Button1
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(580, 352)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.cmbEnd)
        Me.Controls.Add(Me.cmbStart)
        Me.Controls.Add(Me.cmbBible)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblResults)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GreekText1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "Rhema"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GreekText1 As Rhema.GreekText
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblResults As System.Windows.Forms.Label
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbBible As ComboBox
    Friend WithEvents cmbStart As ComboBox
    Friend WithEvents cmbEnd As ComboBox
    Friend WithEvents txtSearch As Viewer
End Class
