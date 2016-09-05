Public Class Viewer
    Inherits WebBrowser

    Private Header As String = String.Format("<html><head><script type='text/javascript'>{0}</script></head><body><div id='info'></div>", Jquery)
    Private Footer As String = "</body></html>"
    Private Script As String = String.Format("<script type='text/javascript'>{0}</script>", Scripts)
    Private _content As String

    Public Overrides Property Text As String
        Get
            Return _content
        End Get
        Set(value As String)
            value = value.Replace(ControlChars.NewLine, "<br/>")
            _content = value
            Me.UpdateWB(value)
        End Set
    End Property

    Public Sub UpdateWB(content As String)
        'Me.ScriptErrorsSuppressed = True
        Me.DocumentText = String.Format("{0}{1}{2}{3}", Header, content, Script, Footer)
    End Sub

End Class
