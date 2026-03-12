using D2LExtensionWebAPPSSR.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace D2LExtensionWebAPPSSR.Service
{
    public interface IStaticReportGeneratorService
    {
        // Now returns both the name and full physical path
        Task<(string FileName, string FilePath)> GenerateStaticHtmlAsync(Guid reportId, DateTime start, DateTime end, IEnumerable<AssignmentItem> assignments, string aiAdvice);
    }

    public class StaticReportGeneratorService : IStaticReportGeneratorService
    {
        private readonly IWebHostEnvironment _env;

        public StaticReportGeneratorService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<(string FileName, string FilePath)> GenerateStaticHtmlAsync(Guid reportId, DateTime start, DateTime end, IEnumerable<AssignmentItem> assignments, string aiAdvice)
        {
            var reportsFolder = Path.Combine(_env.WebRootPath, "reports");
            if (!Directory.Exists(reportsFolder)) Directory.CreateDirectory(reportsFolder);

            var counts = new int[7];
            foreach (var a in assignments)
            {
                int dayIndex = (int)a.DueDate.DayOfWeek - 1;
                if (dayIndex < 0) dayIndex = 6;
                counts[dayIndex]++;
            }
            string chartDataJson = JsonSerializer.Serialize(counts);

            var tableRows = new StringBuilder();
            foreach (var item in assignments.OrderBy(a => a.DueDate))
            {
                tableRows.Append($@"
                    <tr>
                        <td><strong>{item.Title}</strong></td>
                        <td>{item.CourseTitle}</td>
                        <td>{item.DueDate:ddd, MMM dd h:mm tt}</td>
                        <td>{item.EstimatedHours} h</td>
                        <td>{item.Priority}</td>
                    </tr>");
            }

            string htmlContent = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Weekly Study Plan</title>
                <style>
                    body {{ background-color: #f8fafc; font-family: sans-serif; padding: 20px; }}
                    .report-wrapper {{ max-width: 800px; margin: 2rem auto; }}
                    .card {{ background: #fff; border: 1px solid #e2e8f0; border-radius: 8px; padding: 20px; margin-bottom: 20px; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1); }}
                    .table-custom {{ width: 100%; border-collapse: collapse; margin-top: 15px; }}
                    .table-custom th, .table-custom td {{ padding: 12px; border-bottom: 1px solid #e2e8f0; text-align: left; }}
                    .table-custom th {{ background-color: #f8fafc; }}
                    .ai-box {{ background: #f0fdf4; border-left: 4px solid #22c55e; }}
                    h2, h4 {{ color: #1e293b; }}
                </style>
            </head>
            <body>
                <div class='report-wrapper'>
                    <h2 class='mb-4'>Weekly Analysis: {start:MMM dd} - {end:MMM dd}</h2>

                    <div class='card ai-box'>
                        <h4>AI Study Advisor</h4>
                        <div class='mt-3'>
                            {aiAdvice}
                        </div>
                    </div>

                    <div class='card'>
                        <h4>Workload Heatmap</h4>
                        <canvas id='workloadChart' height='100'></canvas>
                    </div>

                    <div class='card'>
                        <h4>Pending Assignments</h4>
                        <table class='table-custom'>
                            <thead>
                                <tr><th>Task</th><th>Course</th><th>Due Date</th><th>Est. Hours</th><th>Priority Score</th></tr>
                            </thead>
                            <tbody>
                                {tableRows}
                            </tbody>
                        </table>
                    </div>
                </div>

                <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
                <script>
                    document.addEventListener('DOMContentLoaded', function () {{
                        const ctx = document.getElementById('workloadChart').getContext('2d');
                        const chartData = {chartDataJson};

                        new Chart(ctx, {{
                            type: 'bar',
                            data: {{
                                labels: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'],
                                datasets: [{{ label: 'Assignments Due', data: chartData, backgroundColor: '#3b82f6', borderRadius: 4 }}]
                            }},
                            options: {{ scales: {{ y: {{ beginAtZero: true, ticks: {{ stepSize: 1 }} }} }} }}
                        }});
                    }});
                </script>
            </body>
            </html>";

            var fileName = $"{reportId}.html";
            var filePath = Path.Combine(reportsFolder, fileName);
            await File.WriteAllTextAsync(filePath, htmlContent);
            return (fileName, filePath);
        }
    }
}