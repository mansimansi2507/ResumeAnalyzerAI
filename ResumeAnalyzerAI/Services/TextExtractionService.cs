using System.IO;
using UglyToad.PdfPig;
using Xceed.Words.NET;

namespace ResumeAnalyzerAI.Services
{
    public class TextExtractionService
    {
        public string ExtractText(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLower();
            if (ext == ".pdf")
            {
                using var pdf = PdfDocument.Open(filePath);
                var text = string.Join("\n", pdf.GetPages().Select(p => p.Text));
                return text;
            }
            else if (ext == ".docx")
            {
                using var doc = DocX.Load(filePath);
                return doc.Text;
            }
            else
            {
                throw new Exception("Unsupported file format.");
            }
        }
    }
}
