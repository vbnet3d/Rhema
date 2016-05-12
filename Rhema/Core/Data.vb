Imports fastJSON

Public Module BibleData

    Public Sub Save(ByVal l As Lexicon)
        IO.File.WriteAllText(String.Format(".\lexicons\{0}.lexicon", l.Name), JSON.ToJSON(l))
    End Sub

    Public Sub Save(ByVal b As Bible)
        IO.File.WriteAllText(String.Format(".\bibles\{0}.bible", b.Name), JSON.ToJSON(b))
    End Sub
End Module
