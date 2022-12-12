Imports System.Collections.Generic
Imports System.Drawing
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Services
Imports System.Linq
Imports System.Text.RegularExpressions

Namespace RichEditSyntaxSample

    Public Class CustomSyntaxHighlightService
        Implements ISyntaxHighlightService

        Private ReadOnly document As Document

        Private _keywords As Regex

        Private _quotedString As Regex = New Regex("'([^']|'')*'")

        Private _commentedString As Regex = New Regex("(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)")

        Public Sub New(ByVal document As Document)
            Me.document = document
            Dim keywords As String() = {"INSERT", "SELECT", "CREATE", "TABLE", "USE", "IDENTITY", "ON", "OFF", "NOT", "NULL", "WITH", "SET", "GO", "DECLARE", "EXECUTE", "NVARCHAR", "FROM", "INTO", "VALUES", "WHERE", "AND"}
            _keywords = New Regex("\b(" & String.Join("|", keywords.[Select](Function(w) Regex.Escape(w))) & ")\b")
        End Sub

        Public Sub ForceExecute() Implements ISyntaxHighlightService.ForceExecute
            Execute()
        End Sub

        Public Sub Execute() Implements ISyntaxHighlightService.Execute
            Dim tSqltokens As List(Of SyntaxHighlightToken) = ParseTokens()
            document.ApplySyntaxHighlight(tSqltokens)
        End Sub

        Private Function ParseTokens() As List(Of SyntaxHighlightToken)
            Dim tokens As List(Of SyntaxHighlightToken) = New List(Of SyntaxHighlightToken)()
            ' search for quoted strings
            Dim ranges As DocumentRange() = TryCast(document.FindAll(_quotedString).GetAsFrozen(), DocumentRange())
            For i As Integer = 0 To ranges.Length - 1
                tokens.Add(CreateToken(ranges(i).Start.ToInt(), ranges(i).End.ToInt(), Color.Red))
            Next

            'Extract all keywords
            ranges = TryCast(document.FindAll(_keywords).GetAsFrozen(), DocumentRange())
            For j As Integer = 0 To ranges.Length - 1
                If Not IsRangeInTokens(ranges(j), tokens) Then tokens.Add(CreateToken(ranges(j).Start.ToInt(), ranges(j).End.ToInt(), Color.Blue))
            Next

            'Find all comments
            ranges = TryCast(document.FindAll(_commentedString).GetAsFrozen(), DocumentRange())
            For j As Integer = 0 To ranges.Length - 1
                If Not IsRangeInTokens(ranges(j), tokens) Then tokens.Add(CreateToken(ranges(j).Start.ToInt(), ranges(j).End.ToInt(), Color.Green))
            Next

            ' order tokens by their start position
            tokens.Sort(New SyntaxHighlightTokenComparer())
            ' fill in gaps in document coverage
            tokens = CombineWithPlainTextTokens(tokens)
            Return tokens
        End Function

        'Parse the remaining text into tokens:
        Private Function CombineWithPlainTextTokens(ByVal tokens As List(Of SyntaxHighlightToken)) As List(Of SyntaxHighlightToken)
            Dim result As List(Of SyntaxHighlightToken) = New List(Of SyntaxHighlightToken)(tokens.Count * 2 + 1)
            Dim documentStart As Integer = document.Range.Start.ToInt()
            Dim documentEnd As Integer = document.Range.End.ToInt()
            If tokens.Count = 0 Then
                result.Add(CreateToken(documentStart, documentEnd, Color.Black))
            Else
                Dim firstToken As SyntaxHighlightToken = tokens(0)
                If documentStart < firstToken.Start Then result.Add(CreateToken(documentStart, firstToken.Start, Color.Black))
                result.Add(firstToken)
                For i As Integer = 1 To tokens.Count - 1
                    Dim token As SyntaxHighlightToken = tokens(i)
                    Dim prevToken As SyntaxHighlightToken = tokens(i - 1)
                    If prevToken.End <> token.Start Then result.Add(CreateToken(prevToken.End, token.Start, Color.Black))
                    result.Add(token)
                Next

                Dim lastToken As SyntaxHighlightToken = tokens(tokens.Count - 1)
                If documentEnd > lastToken.End Then result.Add(CreateToken(lastToken.End, documentEnd, Color.Black))
            End If

            Return result
        End Function

        'Create a token from the retrieved range and specify its forecolor
        Private Function CreateToken(ByVal start As Integer, ByVal [end] As Integer, ByVal foreColor As Color) As SyntaxHighlightToken
            Dim properties As SyntaxHighlightProperties = New SyntaxHighlightProperties()
            properties.ForeColor = foreColor
            Return New SyntaxHighlightToken(start, [end] - start, properties)
        End Function

        'Check whether tokens intersect each other
        Private Function IsRangeInTokens(ByVal range As DocumentRange, ByVal tokens As List(Of SyntaxHighlightToken)) As Boolean
            Return tokens.Any(Function(t) IsIntersect(range, t))
        End Function

        Private Function IsIntersect(ByVal range As DocumentRange, ByVal token As SyntaxHighlightToken) As Boolean
            Dim start As Integer = range.Start.ToInt()
            If start >= token.Start AndAlso start < token.End Then Return True
            Dim [end] As Integer = range.End.ToInt() - 1
            If [end] >= token.Start AndAlso [end] < token.End Then Return True
            If start < token.Start AndAlso [end] >= token.End Then Return True
            Return False
        End Function
    End Class

    'Compare token's initial positions to sort them
    Public Class SyntaxHighlightTokenComparer
        Implements IComparer(Of SyntaxHighlightToken)

        Public Function Compare(ByVal x As SyntaxHighlightToken, ByVal y As SyntaxHighlightToken) As Integer Implements IComparer(Of SyntaxHighlightToken).Compare
            Return x.Start - y.Start
        End Function
    End Class
End Namespace
