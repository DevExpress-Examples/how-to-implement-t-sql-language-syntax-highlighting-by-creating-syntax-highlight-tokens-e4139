<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128610522/19.1.6%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4139)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# How to Use Tokens to Implement T-SQL Language Syntax Highlight

The RichEditControl allows you to create a custom `ISyntaxHighlightService` implementation to display text in different colors and fonts according to the category of syntax sub-elements. These include keywords, comments, control-flow statements, variables, and other elements. This example describes how to highlight the T-SQL syntax. Note that we do not use the <strong>DevExpress.CodeParser</strong> library in this example.

# Implementation Details

* Create a `ISyntaxHighlightService` implementation.

* Search for keywords or specific symbols and convert them into tokens. Check whether the tokens intersect. If not, add them to the token collection.

* Specify a token’s format options in the [SyntaxHighlightToken](https://docs.devexpress.com/OfficeFileAPI/DevExpress.XtraRichEdit.API.Native.SyntaxHighlightToken) object constructor.

* Convert the remaining text into tokens and add them to the collection. Sort the collection by their position in the text.

    **Tip:** We recommend that you use the [GetAsFrozen](https://docs.devexpress.com/OfficeFileAPI/DevExpress.XtraRichEdit.API.Native.DocumentRangeExtensions.GetAsFrozen(DevExpress.XtraRichEdit.API.Native.DocumentRange)) method to improve performance during syntax highlight.

* Call [SubDocument.ApplySyntaxHighlight](https://docs.devexpress.com/OfficeFileAPI/DevExpress.XtraRichEdit.API.Native.SubDocument.ApplySyntaxHighlight(System.Collections.Generic.List-DevExpress.XtraRichEdit.API.Native.SyntaxHighlightToken-)) within the [ISyntaxHighlightService.Execute](https://docs.devexpress.com/OfficeFileAPI/DevExpress.XtraRichEdit.Services.ISyntaxHighlightService.Execute) method to enable syntax highlighting.

* Register the created implementation in the main class.

# Documentation

* [How to: Highlight Document Syntax in the Rich Text Editor](https://docs.devexpress.com/WindowsForms/12107/controls-and-libraries/rich-text-editor/examples/automation/how-to-highlight-document-syntax)

# More Examples

* [Rich Text Editor for WinForms - Implement ISyntaxHighlightService to Highlight C# and VB Code Syntax](https://github.com/DevExpress-Examples/rich-text-editor-highlight-syntax)