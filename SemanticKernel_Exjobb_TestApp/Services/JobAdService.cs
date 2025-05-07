using Microsoft.Extensions.Logging;
using SemanticKernel_Exjobb_TestApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SemanticKernel_Exjobb_TestApp.Services
{
	public class JobAdService
	{
		private readonly HttpClient _httpClient;
		//private readonly ILogger<JobService> _logger;

		public JobAdService(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient();
		}

		public async Task<JobAdApiData?> FetchJobAsync(string jobId)
		{
			var url = $"https://jobsearch.api.jobtechdev.se/ad/{jobId}";
			try
			{
				var response = await _httpClient.GetAsync(url);
				response.EnsureSuccessStatusCode();

				var json = await response.Content.ReadAsStringAsync();
				//_logger.LogInformation("Svar från Job API:\n{Json}", json);
				JobAdApiData jd = JsonSerializer.Deserialize<JobAdApiData>(json);
				return jd;
				
			}
			catch (HttpRequestException ex)
			{
				//_logger.LogError(ex, "Fel vid anrop till API: {Message}", ex.Message);
				return null;
			}
		}
	}
}
