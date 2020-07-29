# How to implement T-SQL language syntax highlighting by creating Syntax Highlight Tokens


<p>This example illustrates how to implement simplified syntax highlighting for the T-SQL language by registering the <a href="http://documentation.devexpress.com/#CoreLibraries/clsDevExpressXtraRichEditServicesISyntaxHighlightServicetopic"><u>ISyntaxHighlightService</u></a>. Note that we do not use the <strong>DevExpress.CodeParser</strong> library in this example. Text is parsed into tokens (a list of <a href="http://documentation.devexpress.com/#CoreLibraries/clsDevExpressXtraRichEditAPINativeSyntaxHighlightTokentopic"><u>SyntaxHighlightToken Class</u></a> instances) manually in the <strong>CustomSyntaxHighlightService.ParseTokens()</strong> method. </p> 
<p>Note that we recommend using the <a href="https://docs.devexpress.com/OfficeFileAPI/DevExpress.XtraRichEdit.API.Native.DocumentRangeExtensions.GetAsFrozen(DevExpress.XtraRichEdit.API.Native.DocumentRange)">GetAsFrozen</a> method to improve performance on retriving document ranges for tokens.</p>
<p> Note that the token list should be sorted by the token start position.  
</p><p><strong>See Also:</strong><br />
<a href="https://www.devexpress.com/Support/Center/p/E2993">Syntax highlighting for C# and VB code using DevExpress CodeParser and Syntax Highlight tokens</a> - Syntax highlighting for C# and VB code using DevExpress CodeParser and Syntax Highlight tokens</p>

<br/>


