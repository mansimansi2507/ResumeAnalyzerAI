using Microsoft.AspNetCore.Http;

namespace ResumeAnalyzerAI.Models
{
    public class ResumeInputModel
    {
        public IFormFile ResumeFile { get; set; }
        public string JobDescription { get; set; }
        public AnalysisResultModel AnalysisResult { get; set; }
    }

    public class AnalysisResultModel
    {
        public int MatchPercentage { get; set; }
        public string[] MissingSkills { get; set; }
        public string AISuggestions { get; set; }
    }
}
