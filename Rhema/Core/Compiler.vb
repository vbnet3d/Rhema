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

Imports System.CodeDom.Compiler
Imports System.Reflection

Module Compiler
    Private VB As New VBCodeProvider



    ''' <summary>
    ''' Runs the specified source code. This is for single scripts.
    ''' </summary>
    ''' <param name="src">The source.</param>
    ''' <returns></returns>
    ''' <remarks>You may use '\n' for new lines in your script text if inlining is required. Use '\r' if you want to replace vbCrLfs in a string.</remarks>
    Public Function Run(ByVal src As String) As Object
        Dim results As CompilerResults

        Dim source As String = <code>
                        Imports System
                        Imports Microsoft.VisualBasic
                        Imports <%= GetType(IScript).Namespace %>
                        Class Script
                            Implements IScript
                            Public Function Run() As Object Implements IScript.Run
                                <%= src %>
                            End Function
                        End Class
            </code>.ToString

        'Compile script
        results = CompileScript(source, Nothing, VB)

        Dim err As CompilerError

        'Add each error as a listview item with its line number
        For Each err In results.Errors
            Debug.Print(String.Format("{0} Line: {1}", err.ErrorText, err.Line), "scripting.log")
        Next

        If results.Errors.Count = 0 Then
            Dim scriptObj As IScript = CType(results.CompiledAssembly.CreateInstance("Script"), IScript)
            Return scriptObj.Run()
        Else
            Return Nothing
        End If
    End Function

    Private Function CompileScript(ByVal Source As String, ByVal Reference As String, ByVal Provider As CodeDomProvider) As CompilerResults
        Dim params As New CompilerParameters()
        Dim results As CompilerResults

        'Configure parameters
        With params
            .GenerateExecutable = False
            .GenerateInMemory = True
            .IncludeDebugInformation = False
            If Not Reference Is Nothing AndAlso Reference.Length <> 0 Then _
       .ReferencedAssemblies.Add(Reference)
            .ReferencedAssemblies.Add("System.Windows.Forms.dll")
            .ReferencedAssemblies.Add("System.dll")
            .ReferencedAssemblies.Add("Microsoft.VisualBasic.dll")
            .ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location)
        End With

        'Compile
        results = Provider.CompileAssemblyFromSource(params, Source)

        Return results
    End Function
End Module

Public Interface IScript
    Function Run() As Object
End Interface