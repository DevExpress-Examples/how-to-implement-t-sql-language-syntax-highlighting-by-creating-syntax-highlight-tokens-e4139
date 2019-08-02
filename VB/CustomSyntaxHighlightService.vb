﻿Imports System.Collections.Generic
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

		Private _keywords As Regex
		Private _quotedString As New Regex("'([^']|'')*'")
		Private _commentedString As New Regex("(/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*+/)")

		Public Sub New(ByVal document As Document)
			Me.document = document
			Dim keywords() As String = { "INSERT", "SELECT", "CREATE", "TABLE", "USE", "IDENTITY", "ON", "OFF", "NOT", "NULL", "WITH", "SET", "GO", "DECLARE", "EXECUTE", "NVARCHAR", "FROM", "INTO", "VALUES", "WHERE", "AND" }
			Me._keywords = New Regex("\b(" & String.Join("|", keywords.Select(Function(w) Regex.Escape(w))) & ")\b")
		End Sub
		Public Sub ForceExecute()
			Execute()
		End Sub
		Public Sub Execute()
			Dim tSqltokens As List(Of SyntaxHighlightToken) = ParseTokens()
			document.ApplySyntaxHighlight(tSqltokens)
		End Sub

		Private Function ParseTokens() As List(Of SyntaxHighlightToken)
			Dim tokens As New List(Of SyntaxHighlightToken)()
			Dim ranges() As DocumentRange = Nothing

			' search for quoted strings
			ranges = document.FindAll(_quotedString)
			For i As Integer = 0 To ranges.Length - 1
				tokens.Add(CreateToken(ranges(i).Start.ToInt(),ranges(i).End.ToInt(), Color.Red))
			Next i

			'Extract all keywords
			ranges = document.FindAll(_keywords)
			For j As Integer = 0 To ranges.Length - 1
				If Not IsRangeInTokens(ranges(j), tokens) Then
					tokens.Add(CreateToken(ranges(j).Start.ToInt(), ranges(j).End.ToInt(), Color.Blue))
				End If
			Next j

			'Find all comments
			ranges = document.FindAll(_commentedString)
			For j As Integer = 0 To ranges.Length - 1
				If Not IsRangeInTokens(ranges(j), tokens) Then
					tokens.Add(CreateToken(ranges(j).Start.ToInt(), ranges(j).End.ToInt(), Color.Green))
				End If
			Next j

			' order tokens by their start position
			tokens.Sort(New SyntaxHighlightTokenComparer())

			' fill in gaps in document coverage
			tokens = CombineWithPlainTextTokens(tokens)
			Return tokens
		End Function

		'Parse the remaining text into tokens:
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

		'Create a token from the retrieved range and specify its forecolor
		Private Function CreateToken(ByVal start As Integer, ByVal [end] As Integer, ByVal foreColor As Color) As SyntaxHighlightToken
			Dim properties As New SyntaxHighlightProperties()
			properties.ForeColor = foreColor
			Return New SyntaxHighlightToken(start, [end] - start, properties)
		End Function

		'Check whether tokens intersect each other
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

	'Compare token's initial positions to sort them
	Public Class SyntaxHighlightTokenComparer
		Implements IComparer(Of SyntaxHighlightToken)

		Public Function Compare(ByVal x As SyntaxHighlightToken, ByVal y As SyntaxHighlightToken) As Integer Implements IComparer(Of SyntaxHighlightToken).Compare
			Return x.Start - y.Start
		End Function
	End Class
End Namespace

