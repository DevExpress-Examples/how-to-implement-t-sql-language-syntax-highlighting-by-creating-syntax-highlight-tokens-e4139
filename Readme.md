<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128610522/19.1.6%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4139)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# How to implement T-SQL language syntax highlighting by creating Syntax Highlight Tokens


<p>This example illustrates how to implement simplified syntax highlighting for the T-SQL language by registering the <a href="http://documentation.devexpress.com/#CoreLibraries/clsDevExpressXtraRichEditServicesISyntaxHighlightServicetopic"><u>ISyntaxHighlightService</u></a>. Note that we do not use the <strong>DevExpress.CodeParser</strong> library in this example. Text is parsed into tokens (a list of <a href="http://documentation.devexpress.com/#CoreLibraries/clsDevExpressXtraRichEditAPINativeSyntaxHighlightTokentopic"><u>SyntaxHighlightToken Class</u></a> instances) manually in the <strong>CustomSyntaxHighlightService.ParseTokens()</strong> method. </p> 
<p>Note that we recommend using the <a href="https://docs.devexpress.com/OfficeFileAPI/DevExpress.XtraRichEdit.API.Native.DocumentRangeExtensions.GetAsFrozen(DevExpress.XtraRichEdit.API.Native.DocumentRange)">GetAsFrozen</a> method to improve performance on retriving document ranges for tokens.</p>
<p> Note that the token list should be sorted by the token start position.  
</p><p><strong>See Also:</strong><br />
<a href="https://www.devexpress.com/Support/Center/p/E2993">Syntax highlighting for C# and VB code using DevExpress CodeParser and Syntax Highlight tokens</a> - Syntax highlighting for C# and VB code using DevExpress CodeParser and Syntax Highlight tokens</p>

<br/>


