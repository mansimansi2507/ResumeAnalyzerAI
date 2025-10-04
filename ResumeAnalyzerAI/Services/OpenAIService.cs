using OpenAI;
using OpenAI.Chat;
using ResumeAnalyzerAI.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ResumeAnalyzerAI.Services
{
    public class OpenAIService
    {
        private readonly OpenAIClient _client;

        public OpenAIService(IConfiguration config)
        {
            var apiKey = config["OpenAI:ApiKey"];
            _client = new OpenAIClient(apiKey);
        }

        public async Task<AnalysisResultModel> AnalyzeResumeAsync(string resumeText, string jobDescription)
        {
            var chatClient = _client.GetChatClient("gpt-4o-mini");

            var prompt = $@"
You are an expert HR assistant.
Compare this resume with the job description and provide:
1. Match percentage (0-100)
2. Missing skills
3. Suggestions for improvement

Resume:
{resumeText}

Job Description:
{jobDescription}

Return the result as JSON in this format:
{{ ""MatchPercentage"": number, ""MissingSkills"": [""skill1"", ""skill2""], ""AISuggestions"": ""text"" }}";

            var response = await chatClient.CompleteChatAsync(
                new UserChatMessage(prompt)
            );

            var content = response.Value.Content[0].Text;

            // Extract only JSON object from AI response
            var match = Regex.Match(content, @"\{.*\}", RegexOptions.Singleline);
            if (match.Success)
            {
                try
                {
                    var result = JsonSerializer.Deserialize<AnalysisResultModel>(match.Value.Trim());
                    return result ?? new AnalysisResultModel
                    {
                        MatchPercentage = 0,
                        MissingSkills = Array.Empty<string>(),
                        AISuggestions = "AI analysis could not be generated."
                    };
                }
                catch
                {
                    return new AnalysisResultModel
                    {
                        MatchPercentage = 0,
                        MissingSkills = Array.Empty<string>(),
                        AISuggestions = "AI analysis could not be generated."
                    };
                }
            }

            // Fallback if no JSON found
            return new AnalysisResultModel
            {
                MatchPercentage = 0,
                MissingSkills = Array.Empty<string>(),
                AISuggestions = "AI analysis could not be generated."
            };
        }
    }
}
