using System.Windows.Forms;
using DevExpress.Office.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Services;

namespace RichEditSyntaxSample
{
    public partial class Form1 : XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            richEditControl1.Options.Search.RegExResultMaxGuaranteedLength = 500;
            richEditControl1.ReplaceService<ISyntaxHighlightService>(new CustomSyntaxHighlightService(richEditControl1.Document));
            richEditControl1.LoadDocument("CarsXtraScheduling.sql");
            richEditControl1.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Draft;
            richEditControl1.Document.Sections[0].Page.Width = Units.InchesToDocumentsF(80f);
            richEditControl1.Document.DefaultCharacterProperties.FontName = "Courier New";
        }
    }
}
  