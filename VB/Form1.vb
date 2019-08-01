Imports System.Collections.Generic
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Office.Utils
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraRichEdit.Services

Namespace RichEditSyntaxSample
	Partial Public Class Form1
		Inherits Form

		Public Sub New()
			InitializeComponent()
			richEditControl1.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Draft
			richEditControl1.Options.Search.RegExResultMaxGuaranteedLength = 500
			richEditControl1.ReplaceService(Of ISyntaxHighlightService)(New CustomSyntaxHighlightService(richEditControl1.Document))
			richEditControl1.LoadDocument("CarsXtraScheduling.sql")
			richEditControl1.Document.Sections(0).Page.Width = Units.InchesToDocumentsF(80F)
			richEditControl1.Document.DefaultCharacterProperties.FontName = "Courier New"
		End Sub
	End Class
End Namespace
