using Microsoft.AspNetCore.Mvc;
using ResumeAnalyzerAI.Data;
using ResumeAnalyzerAI.Models;
using ResumeAnalyzerAI.Services;
using System.IO;
using System.Threading.Tasks;

namespace ResumeAnalyzerAI.Controllers
{
    public class ResumeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly OpenAIService _aiService;

        public ResumeController(OpenAIService aiService, AppDbContext context)
        {
            _aiService = aiService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ResumeInputModel model)
        {
            if (model.ResumeFile == null || string.IsNullOrEmpty(model.JobDescription))
            {
                ModelState.AddModelError("", "Please upload a resume and paste a job description.");
                return View();
            }

            // Save uploaded resume temporarily
            var filePath = Path.Combine(Path.GetTempPath(), model.ResumeFile.FileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ResumeFile.CopyToAsync(stream);
                }

                var textService = new TextExtractionService();
                string extractedText = textService.ExtractText(filePath);

                // Call OpenAI API
                var result = await _aiService.AnalyzeResumeAsync(extractedText, model.JobDescription);

                model.AnalysisResult = result;
                var record = new ResumeRecord
                {
                    ResumeText = extractedText,
                    JobDescription = model.JobDescription,
                    MatchPercentage = result.MatchPercentage,
                    MissingSkills = string.Join(",", result.MissingSkills), // Store as CSV
                    AISuggestions = result.AISuggestions
                };

                _context.ResumeRecords.Add(record);
                await _context.SaveChangesAsync();
            }
            finally
            {
                // Delete temporary file if it exists
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return View(model);
        }

    }
}
