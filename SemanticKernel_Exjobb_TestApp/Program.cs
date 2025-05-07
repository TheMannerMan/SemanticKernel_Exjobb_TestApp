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

//const int 

var service = app.Services.GetRequiredService<JobAdService>();

JobAdApiData? job = await service.FetchJobAsync("29676073");

if (job == null)
{
	Console.WriteLine("Job not found");
	return;
}

else
{
	Console.WriteLine("Job found");
}

string jobDescription = job.description.text;

var result = await kernel.InvokePromptAsync(jobDescription, new KernelArguments(new AzureOpenAIPromptExecutionSettings
{
	ResponseFormat = typeof(JobExtract)
}));


JobExtract jobExtract = JsonSerializer.Deserialize<JobExtract>(result.ToString());

//var testtest = result.GetValue<JobExtract>();

//var formattedJson = JsonSerializer.Serialize(testtest, new JsonSerializerOptions
//{
//	WriteIndented = true
//});

//Console.WriteLine(formattedJson);

//PrintProperties(jobExtract);

PrintJobExtract(jobExtract);

static void PrintJobExtract(JobExtract job)
{
	Console.WriteLine("Titel:");
	job.EmploymentTitle?.ForEach(title => Console.WriteLine($"- {title}"));

	Console.WriteLine($"Anställningstyp: {job.EmploymentType}");
	if (job.IsTrialEmployment)
	{
		Console.WriteLine("Innehåller provanställning");
	}

	Console.WriteLine($"Plats: {job.Location}");

	Console.WriteLine("Verktyg och teknologier:");
	job.ToolsAndTechnologies?.ForEach(tool => Console.WriteLine($"- {tool}"));

	Console.WriteLine("Kravkompetenser:");
	job.RequiredSkills?.ForEach(skill => Console.WriteLine($"- {skill}"));

	Console.WriteLine("Meriterande kompetenser:");
	job.MeritingSkills?.ForEach(skill => Console.WriteLine($"- {skill}"));

	Console.WriteLine("Personliga egenskaper:");
	job.SoftSkills?.ForEach(skill => Console.WriteLine($"- {skill}"));

	Console.WriteLine($"Erfarenhetsnivå: {job.ExperienceLevel}");

	Console.WriteLine($"Arbetsgivare: {job.Employer}");
	Console.WriteLine($"Referensnummer: {job.ReferenceNumber}");
	Console.WriteLine($"Sista ansökningsdag: {job.ApplicationDeadline:yyyy-MM-dd}");

	if (job.Contact != null)
	{
		Console.WriteLine("Kontaktperson:");
		Console.WriteLine($"- Namn: {job.Contact.Name}");
		Console.WriteLine($"- E-post: {job.Contact.Email}");
		Console.WriteLine($"- Telefon: {job.Contact.Phone}");
	}
}

Console.WriteLine();
