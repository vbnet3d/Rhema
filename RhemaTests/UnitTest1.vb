Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Rhema

<TestClass()> Public Class UnitTests
    Dim tr As Bible
    Dim strongs As Lexicon
    Dim fb As FullTextBible



    <TestMethod()> Public Sub TestWithinSearches()
        Assert.AreEqual(tr.GetReference(fb.Search("αλλος <WITHIN 25 WORDS> αυτου").ToArray).Count, 16)
        Assert.AreEqual(tr.GetReference(fb.Search("αλλος <WITHIN 250 WORDS> αυτου").ToArray).Count, 969)
        Assert.AreEqual(tr.GetReference(fb.Search("αλλος <WITHIN 5 WORDS> αυτου").ToArray).Count, 3)
    End Sub

End Class