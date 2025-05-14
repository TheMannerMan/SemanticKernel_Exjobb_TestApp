using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using SemanticKernel_Exjobb_TestApp.Models;
using SemanticKernel_Exjobb_TestApp.Models.ResponseModels;
using SemanticKernel_Exjobb_TestApp.Services;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<JobAdService>();
var app = builder.Build();

IConfiguration _config = new ConfigurationBuilder()
	.AddUserSecrets<Program>()
	.Build();

var kernelBuilder = Kernel.CreateBuilder();
string deploymentModel = _config["Model"] ?? throw new InvalidOperationException("LLM model is required");
string endpoint = _config["Endpoint"] ?? throw new InvalidOperationException("Endpoint required");
string apiKey = _config["Key"] ?? throw new InvalidOperationException("Key is required");

kernelBuilder.AddAzureOpenAIChatCompletion(deploymentModel, endpoint, apiKey);

Kernel kernel = kernelBuilder.Build();

var service = app.Services.GetRequiredService<JobAdService>();

// NOTE: Replace the ID below with your own job ad ID from the Swedish Public Employment Service (Arbetsförmedlingen).
// The ID can be found in the URL of a specific ad, e.g., https://arbetsformedlingen.se/platsbanken/annonser/29652183
JobAdApiData? job = await service.FetchJobAsync("29652183");

if (job == null)
{
	Console.WriteLine("Job not found");
	return;
}

string jobDescription = job.description.text;

for (int i = 1; i <= 3; i++)
{
	var result = await kernel.InvokePromptAsync(jobDescription, new KernelArguments(new AzureOpenAIPromptExecutionSettings
	{
		ResponseFormat = typeof(JobExtract),
		}));

	JobExtract jobExtract = JsonSerializer.Deserialize<JobExtract>(result.ToString());

	// Spara resultat till textfil
	string fileName = $"iteration1_test{i}.txt";
	using var writer = new StreamWriter(fileName);
	PrintJobExtract(jobExtract, writer);

	Console.WriteLine($"Test {i} sparat i {fileName}");
}

static void PrintJobExtract(JobExtract job, TextWriter writer)
{
	writer.WriteLine("Titel:");
	job.EmploymentTitle?.ForEach(title => writer.WriteLine($"- {title}"));

	writer.WriteLine($"Anställningstyp: {job.EmploymentType}");

	writer.Write("Innehåller provanställning: ");
	writer.WriteLine(job?.IsTrialEmployment.HasValue == true ? (job.IsTrialEmployment.Value ? "Ja" : "Nej") : "Okänt");

	writer.WriteLine("Plats:");
	job.JobLocation?.ForEach(location => writer.WriteLine($"- {location}"));

	writer.WriteLine("Kravkompetenser:");
	job.RequiredSkills?.ForEach(skill => writer.WriteLine($"- {skill}"));

	writer.WriteLine("Meriterande kompetenser:");
	job.MeritingSkills?.ForEach(skill => writer.WriteLine($"- {skill}"));

	writer.WriteLine($"Arbetsgivare: {job.Employer}");
	writer.WriteLine($"Referensnummer: {job.ReferenceNumber}");
	writer.WriteLine($"Sista ansökningsdag: {job.ApplicationDeadline}");

	if (job.Contact != null)
	{
		writer.WriteLine("Kontaktperson:");
		writer.WriteLine($"- Namn: {job.Contact.Name}");
		writer.WriteLine($"- E-post: {job.Contact.Email}");
		writer.WriteLine($"- Telefon: {job.Contact.Phone}");
	}
}