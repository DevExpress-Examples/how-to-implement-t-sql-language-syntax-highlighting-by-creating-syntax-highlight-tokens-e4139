Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Office.Utils
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Services
Imports System.Linq

Namespace RichEditSyntaxSample
	Public Class CustomSyntaxHighlightService
		Implements ISyntaxHighlightService

		#Region "#parsetokens"
		Private ReadOnly document As Document
		Private defaultSettings As New SyntaxHighlightProperties() With {.ForeColor = Color.Black}
		Private keywordSettings As New SyntaxHighlightProperties() With {.ForeColor = Color.Blue}
		Private stringSettings As New SyntaxHighlightProperties() With {.ForeColor = Color.Green}

		Private keywords() As String = { "INSERT", "SELECT", "CREATE", "TABLE", "USE", "IDENTITY", "ON", "OFF", "NOT", "NULL", "WITH", "SET" }

		Public Sub New(ByVal document As Document)
			Me.document = document
		End Sub

		Private Function ParseTokens() As List(Of SyntaxHighlightToken)
			Dim tokens As New List(Of SyntaxHighlightToken)()
			Dim ranges() As DocumentRange = Nothing
			' search for quotation marks
			ranges = document.FindAll("'", SearchOptions.None)
			For i As Integer = 0 To (ranges.Length \ 2) - 1
				tokens.Add(New SyntaxHighlightToken(ranges(i * 2).Start.ToInt(), ranges(i * 2 + 1).Start.ToInt() - ranges(i * 2).Start.ToInt() + 1, stringSettings))
			Next i
			' search for keywords
			For i As Integer = 0 To keywords.Length - 1
				ranges = document.FindAll(keywords(i), SearchOptions.CaseSensitive Or SearchOptions.WholeWord)

				For j As Integer = 0 To ranges.Length - 1
					If Not IsRangeInTokens(ranges(j), tokens) Then
						tokens.Add(New SyntaxHighlightToken(ranges(j).Start.ToInt(), ranges(j).Length, keywordSettings))
					End If
				Next j
			Next i
			' order tokens by their start position
			tokens.Sort(New SyntaxHighlightTokenComparer())
			' fill in gaps in document coverage
			AddPlainTextTokens(tokens)
			Return tokens
		End Function

		Private Sub AddPlainTextTokens(ByVal tokens As List(Of SyntaxHighlightToken))
			Dim count As Integer = tokens.Count
			If count = 0 Then
				tokens.Add(New SyntaxHighlightToken(0, document.Range.End.ToInt(), defaultSettings))
				Return
			End If
			tokens.Insert(0, New SyntaxHighlightToken(0, tokens(0).Start, defaultSettings))
			For i As Integer = 1 To count - 1
				tokens.Insert(i * 2, New SyntaxHighlightToken(tokens(i * 2 - 1).End, tokens(i * 2).Start - tokens(i * 2 - 1).End, defaultSettings))
			Next i
			tokens.Add(New SyntaxHighlightToken(tokens(count * 2 - 1).End, document.Range.End.ToInt() - tokens(count * 2 - 1).End, defaultSettings))
		End Sub

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
		#End Region ' #parsetokens

		#Region "#ISyntaxHighlightServiceMembers"
		Public Sub ForceExecute() Implements ISyntaxHighlightService.ForceExecute
			Execute()
		End Sub
		Public Sub Execute() Implements ISyntaxHighlightService.Execute
			document.ApplySyntaxHighlight(ParseTokens())
		End Sub
		#End Region ' #ISyntaxHighlightServiceMembers
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

