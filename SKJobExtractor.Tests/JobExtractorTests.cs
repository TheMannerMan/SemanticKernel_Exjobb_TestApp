using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using SemanticKernel_Exjobb_TestApp.Models;
using SemanticKernel_Exjobb_TestApp.Models.ResponseModels;
using SemanticKernel_Exjobb_TestApp.Services;
using System.Text.Json;
using Shouldly;

namespace SKJobExtractor.Tests
{
	public class JobExtractorTests
	{
		private readonly Kernel _kernel;
		private readonly JobAdService _jobAdService;
		private readonly string _jobDescription;


		public JobExtractorTests()
		{
			var config = new ConfigurationBuilder()
			.AddUserSecrets<JobExtractorTests>()
			.Build();

			var kernelBuilder = Kernel.CreateBuilder();

			kernelBuilder.AddAzureOpenAIChatCompletion(
				config["Model"] ?? throw new InvalidOperationException("Missing model"),
				config["Endpoint"] ?? throw new InvalidOperationException("Missing endpoint"),
				config["Key"] ?? throw new InvalidOperationException("Missing key")
			);

			_kernel = kernelBuilder.Build();

			var services = new ServiceCollection();
			services.AddHttpClient();
			services.AddSingleton<JobAdService>();
			var provider = services.BuildServiceProvider();

			var jobAdService = provider.GetRequiredService<JobAdService>();
			var job = jobAdService.FetchJobAsync("29676073").Result;
			_jobDescription = job?.description?.text ?? throw new InvalidOperationException("Jobbtexten kunde inte hämtas");

		}

		[Fact]
		public async Task JobExtraction_ShouldSucceed()
		{
			int successCount = 0;
			const int iterations = 1;

			for (int i = 0; i < iterations; i++)
			{
				var result = await _kernel.InvokePromptAsync(_jobDescription, new KernelArguments(new AzureOpenAIPromptExecutionSettings
				{
					ResponseFormat = typeof(JobExtract)
				}));

				var extract = JsonSerializer.Deserialize<JobExtract>(result.ToString());

				if (IsExtractionSuccessful(extract))
					successCount++;
			}

			successCount.ShouldBeGreaterThanOrEqualTo(1);
		}

		private bool IsExtractionSuccessful(JobExtract? extract)
		{
			if (extract == null) return false;

			return extract.Employer.Contains("Quest") == true;
			return false;

		}
	}
}
