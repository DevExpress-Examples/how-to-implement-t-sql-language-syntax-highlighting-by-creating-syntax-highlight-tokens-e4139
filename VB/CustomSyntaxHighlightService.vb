Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Office.Utils
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Services
Imports System.Linq
Imports System.Text.RegularExpressions

Namespace RichEditSyntaxSample
	Public Class CustomSyntaxHighlightService
		Implements ISyntaxHighlightService

		Private ReadOnly document As Document
		Private keywordSettings As New SyntaxHighlightProperties() With {.ForeColor = Color.Blue}
		Private stringSettings As New SyntaxHighlightProperties() With {.ForeColor = Color.Red}
        Private commentSettings As New SyntaxHighlightProperties() With {.ForeColor = Color.Green}

        Private _keywords As Regex
        Private _quotedString As New Regex("'([^']|'')*'")
        Private _commentString As Regex

        Public Sub New(ByVal document As Document)
			Me.document = document
			Dim keywords() As String = { "INSERT", "SELECT", "CREATE", "TABLE", "USE", "IDENTITY", "ON", "OFF", "NOT", "NULL", "WITH", "SET", "GO", "DECLARE", "EXECUTE", "NVARCHAR", "FROM", "INTO", "VALUES" }
            Me._keywords = New Regex("\b(" & String.Join("|", keywords.Select(Function(w) Regex.Escape(w))) & ")\b")
            Me._commentString = New Regex("(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)")
        End Sub

        Public Sub ForceExecute() Implements ISyntaxHighlightService.ForceExecute
            Execute()
        End Sub

        Private Sub Execute() Implements ISyntaxHighlightService.Execute
            document.ApplySyntaxHighlight(ParseTokens())
        End Sub

        Private Function ParseTokens() As List(Of SyntaxHighlightToken)
            Dim tokens As New List(Of SyntaxHighlightToken)()
            Dim ranges() As DocumentRange = Nothing

            ' search for quotation marks
            ranges = document.FindAll(_quotedString)
            For i As Integer = 0 To ranges.Length - 1
                tokens.Add(New SyntaxHighlightToken(ranges(i).Start.ToInt(), ranges(i).Length, stringSettings))
            Next i

            ranges = document.FindAll(_keywords)
            For j As Integer = 0 To ranges.Length - 1
                If Not IsRangeInTokens(ranges(j), tokens) Then
                    tokens.Add(New SyntaxHighlightToken(ranges(j).Start.ToInt(), ranges(j).Length, keywordSettings))
                End If
            Next j

            ranges = document.FindAll(_commentString)
            For j As Integer = 0 To ranges.Length - 1
                If Not IsRangeInTokens(ranges(j), tokens) Then
                    tokens.Add(New SyntaxHighlightToken(ranges(j).Start.ToInt(), ranges(j).Length, commentSettings))
                End If
            Next j

            ' order tokens by their start position
            tokens.Sort(New SyntaxHighlightTokenComparer())
            ' fill in gaps in document coverage
            tokens = CombineWithPlainTextTokens(tokens)
            Return tokens
        End Function
        Private Function CombineWithPlainTextTokens(ByVal tokens As List(Of SyntaxHighlightToken)) As List(Of SyntaxHighlightToken)
			Dim result As New List(Of SyntaxHighlightToken)(tokens.Count * 2 + 1)
			Dim documentStart As Integer = Me.document.Range.Start.ToInt()
			Dim documentEnd As Integer = Me.document.Range.End.ToInt()
			If tokens.Count = 0 Then
				result.Add(CreateToken(documentStart, documentEnd, Color.Black))
			Else
				Dim firstToken As SyntaxHighlightToken = tokens(0)
				If documentStart < firstToken.Start Then
					result.Add(CreateToken(documentStart, firstToken.Start, Color.Black))
				End If
				result.Add(firstToken)
				For i As Integer = 1 To tokens.Count - 1
					Dim token As SyntaxHighlightToken = tokens(i)
					Dim prevToken As SyntaxHighlightToken = tokens(i - 1)
					If prevToken.End <> token.Start Then
						result.Add(CreateToken(prevToken.End, token.Start, Color.Black))
					End If
					result.Add(token)
				Next i
				Dim lastToken As SyntaxHighlightToken = tokens(tokens.Count - 1)
				If documentEnd > lastToken.End Then
					result.Add(CreateToken(lastToken.End, documentEnd, Color.Black))
				End If
			End If
			Return result
		End Function
		Private Function CreateToken(ByVal start As Integer, ByVal [end] As Integer, ByVal foreColor As Color) As SyntaxHighlightToken
			Dim properties As New SyntaxHighlightProperties()
			properties.ForeColor = foreColor
			Return New SyntaxHighlightToken(start, [end] - start, properties)
		End Function

		Private Function IsRangeInTokens(ByVal range As DocumentRange, ByVal tokens As List(Of SyntaxHighlightToken)) As Boolean
			Return tokens.Any(Function(t) IsIntersect(range, t))
		End Function
		Private Function IsIntersect(ByVal range As DocumentRange, ByVal token As SyntaxHighlightToken) As Boolean
			Dim start As Integer = range.Start.ToInt()
			If start >= token.Start AndAlso start < token.End Then
				Return True
			End If
			Dim [end] As Integer = range.End.ToInt() - 1
			If [end] >= token.Start AndAlso [end] < token.End Then
				Return True
			End If
			If start < token.Start AndAlso [end] >= token.End Then
				Return True
			End If
			Return False
		End Function

	End Class

	#Region "#SyntaxHighlightTokenComparer"
	Public Class SyntaxHighlightTokenComparer
		Implements IComparer(Of SyntaxHighlightToken)

		Public Function Compare(ByVal x As SyntaxHighlightToken, ByVal y As SyntaxHighlightToken) As Integer Implements IComparer(Of SyntaxHighlightToken).Compare
			Return x.Start - y.Start
		End Function
	End Class
	#End Region ' #SyntaxHighlightTokenComparer
End Namespace

