using System;

namespace ResumeAnalyzerAI.Models
{
    public class ResumeRecord
    {
        public int Id { get; set; } // Primary Key

        public string ResumeText { get; set; }
        public string JobDescription { get; set; }

        public int MatchPercentage { get; set; }
        public string MissingSkills { get; set; } // Stored as comma-separated or JSON
        public string AISuggestions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
