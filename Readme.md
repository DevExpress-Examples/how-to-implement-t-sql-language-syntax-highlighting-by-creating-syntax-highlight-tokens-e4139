<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128610522/18.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4139)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# How to implement T-SQL language syntax highlighting by creating Syntax Highlight Tokens


<p>This example illustrates how to implement simplified syntax highlighting for the T-SQL language by registering the <a href="http://documentation.devexpress.com/#CoreLibraries/clsDevExpressXtraRichEditServicesISyntaxHighlightServicetopic"><u>ISyntaxHighlightService</u></a>. Note that we do not use the <strong>DevExpress.CodeParser</strong> library in this example. Text is parsed into tokens (a list of <a href="http://documentation.devexpress.com/#CoreLibraries/clsDevExpressXtraRichEditAPINativeSyntaxHighlightTokentopic"><u>SyntaxHighlightToken Class</u></a> instances) manually in the <strong>CustomSyntaxHighlightService.ParseTokens()</strong> method. The resulting list is sorted (see <a href="https://www.devexpress.com/Support/Center/p/Q368517">Documentation - It is not mentioned that a list passed to the SubDocument.ApplySyntaxHighlight method should be sorted</a>) and passed to the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraRichEditAPINativeSubDocument_ApplySyntaxHighlighttopic"><u>SubDocument.ApplySyntaxHighlight Method</u></a>.</p><p><strong>See Also:</strong><br />
<a href="https://www.devexpress.com/Support/Center/p/E2993">Syntax highlighting for C# and VB code using DevExpress CodeParser and Syntax Highlight tokens</a> - Syntax highlighting for C# and VB code using DevExpress CodeParser and Syntax Highlight tokens</p>

<br/>


<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=how-to-implement-t-sql-language-syntax-highlighting-by-creating-syntax-highlight-tokens-e4139&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=how-to-implement-t-sql-language-syntax-highlighting-by-creating-syntax-highlight-tokens-e4139&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
