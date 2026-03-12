using D2LExtensionWebAPPSSR.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace D2LExtensionWebAPPSSR.Service
{
    public interface IAiStudyAdvisorService
    {
        Task<string> GetStudyRecommendationsAsync(IEnumerable<AssignmentItem> assignments);
    }

    public class OpenAiStudyAdvisorService : IAiStudyAdvisorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiStudyAdvisorService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["Groq:ApiKey"];
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GetStudyRecommendationsAsync(IEnumerable<AssignmentItem> assignments)
        {
            if (!assignments.Any()) return "<p>No assignments due this week. Great job staying ahead!</p>";

            var totalHours = assignments.Sum(a => a.EstimatedHours);
            var taskList = string.Join("\n", assignments.Select(a =>
                $"- {a.Title} ({a.CourseTitle}): Due {a.DueDate:ddd, MMM dd}, Est. Time: {a.EstimatedHours}h, Priority Score: {a.Priority}"));

            var prompt = $@"
                You are an expert academic advisor. The student has {totalHours} total hours of work due this week.
                Tasks:
                {taskList}
                
                Provide in clean HTML format (use <ul>, <li>, <strong>, <h3>, NO markdown blockticks):
                1. <h3>Recommended Execution Order</h3>: Sort logically by Priority Score and Due Date.
                2. <h3>Time Management Strategy</h3>: How to break down the {totalHours} hours.
                3. <h3>Burnout Warning</h3>: Highlight if any day is overloaded based on estimated hours.";

            var requestBody = new
            {
                model = "llama-3.1-8b-instant", // Updated Groq Model
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 600
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.groq.com/openai/v1/chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                string groqError = await response.Content.ReadAsStringAsync();
                throw new Exception($"GROQ API ERROR: {groqError}");
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            return result.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }
}