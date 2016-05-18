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

Public Enum Language
    English = 0
    Greek = 1
    Hebrew = 2
End Enum

Public Class GreekText
    Inherits TextBox

    Public Language As Language
    Private ld As New Dictionary(Of Char, Char)

    Public Sub New()
        ld.Add(ChrW(65), ChrW(913))
        ld.Add(ChrW(66), ChrW(914))
        ld.Add(ChrW(71), ChrW(915))
        ld.Add(ChrW(68), ChrW(916))
        ld.Add(ChrW(69), ChrW(917))
        ld.Add(ChrW(90), ChrW(918))
        ld.Add(ChrW(72), ChrW(919))
        ld.Add(ChrW(81), ChrW(920))
        ld.Add(ChrW(73), ChrW(921))
        ld.Add(ChrW(76), ChrW(923))
        ld.Add(ChrW(77), ChrW(924))
        ld.Add(ChrW(78), ChrW(925))
        ld.Add(ChrW(88), ChrW(926))
        ld.Add(ChrW(79), ChrW(927))
        ld.Add(ChrW(80), ChrW(928))
        ld.Add(ChrW(82), ChrW(929))
        ld.Add(ChrW(83), ChrW(931))
        ld.Add(ChrW(84), ChrW(932))
        ld.Add(ChrW(85), ChrW(933))
        ld.Add(ChrW(89), ChrW(933))
        ld.Add(ChrW(70), ChrW(934))
        ld.Add(ChrW(67), ChrW(935))
        ld.Add(ChrW(74), ChrW(936))
        ld.Add(ChrW(87), ChrW(937))
        ld.Add(ChrW(97), ChrW(945))
        ld.Add(ChrW(98), ChrW(946))
        ld.Add(ChrW(103), ChrW(947))
        ld.Add(ChrW(100), ChrW(948))
        ld.Add(ChrW(101), ChrW(949))
        ld.Add(ChrW(122), ChrW(950))
        ld.Add(ChrW(104), ChrW(951))
        ld.Add(ChrW(113), ChrW(952))
        ld.Add(ChrW(105), ChrW(953))
        ld.Add(ChrW(107), ChrW(954))
        ld.Add(ChrW(108), ChrW(955))
        ld.Add(ChrW(109), ChrW(956))
        ld.Add(ChrW(110), ChrW(957))
        ld.Add(ChrW(120), ChrW(958))
        ld.Add(ChrW(111), ChrW(959))
        ld.Add(ChrW(112), ChrW(960))
        ld.Add(ChrW(114), ChrW(961))
        ld.Add(ChrW(115), ChrW(963))
        ld.Add(ChrW(118), ChrW(962))
        ld.Add(ChrW(116), ChrW(964))
        ld.Add(ChrW(117), ChrW(965))
        ld.Add(ChrW(121), ChrW(965))
        ld.Add(ChrW(102), ChrW(966))
        ld.Add(ChrW(99), ChrW(967))
        ld.Add(ChrW(106), ChrW(968))
        ld.Add(ChrW(119), ChrW(969))

    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Dim p As Integer = Me.SelectionStart
        Dim skip As Boolean
        Dim i As Integer
        For i = p To 1 Step -1
            If Me.Text(i - 1) = "<" Or Me.Text(i - 1) = "[" Then
                skip = True
                Exit For
            End If
        Next
        For c As Integer = i To p - 1
            If Me.Text(c) = ">" Or Me.Text(c) = "]" Then
                skip = False
                Exit For
            End If
        Next

        If Not skip Then
            If ld.ContainsKey(e.KeyChar) Then
                e.KeyChar = ld(e.KeyChar)
            End If
        Else
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If

        MyBase.OnKeyPress(e)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Dim p As Integer = Me.SelectionStart

        Dim words As String() = Me.Text.Split(CType(" ", Char()))

        For i As Integer = 0 To words.Length - 1
            words(i) = words(i).Replace("ς", "σ")
            If words(i).EndsWith("σ") Then
                words(i) = words(i).Substring(0, words(i).Length - 1) & "ς"
            End If
        Next
        Me.Text = String.Join(" ", words)

        Me.SelectionStart = p
        'MyBase.OnTextChanged(e)
    End Sub
End Class
