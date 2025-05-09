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

JobAdApiData? job = await service.FetchJobAsync("29676134");

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


#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var result = await kernel.InvokePromptAsync(jobDescription, new KernelArguments(new AzureOpenAIPromptExecutionSettings
{
	ResponseFormat = typeof(JobExtract),
	Temperature = 0.0,
	ChatSystemPrompt = @"Extrahera informationen från jobbannonsen och svara på svenska för följande: tjänstens titel, anställningstyp, plats och erfarenhetsnivå. 
För ansvarsområden och personliga egenskaper, översätt till svenska om det finns en tydlig och naturlig motsvarighet. Presentera varje ansvarsområde och personlig egenskap som ett separat nyckelord i respektive lista.
Behåll namn på tekniker (t.ex. C#, .NET, Azure Service Bus), arbetsgivare och eventuella personnamn i originalspråket. Extrahera varje teknik och färdighet (både krav och meriterande) som ett separat nyckelord i respektive lista.

Exempel på extraherade nyckelord för kravkompetenser: ['C#', '.NET 6+', 'Docker', 'SQL'].
Exempel på översättning och extrahering av ansvarsområden: 'Software development' -> ['Mjukvaruutveckling'], 'Agile team collaboration' -> ['Agilt samarbete'].
Exempel på översättning och extrahering av personliga egenskaper: 'Analytical skills' -> ['Analytisk'], 'Team player' -> ['Samarbetsorienterad'].
Returnera resultatet i det JSON-format som definieras av JobExtract-klassen."
}));
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.


JobExtract jobExtract = JsonSerializer.Deserialize<JobExtract>(result.ToString());


PrintJobExtract(jobExtract);

static void PrintJobExtract(JobExtract job)
{
	Console.WriteLine("Titel:");
	job.EmploymentTitle?.ForEach(title => Console.WriteLine($"- {title}"));

	Console.WriteLine($"Anställningstyp: {job.EmploymentType}");

	Console.Write("Innehåller provanställning: ");
	if (job?.IsTrialEmployment.HasValue == true)
	{
		Console.WriteLine(job.IsTrialEmployment.Value ? "Ja" : "Nej");
	}
	else
	{
		Console.WriteLine("Okänt");
	}

	Console.WriteLine($"Plats:");
	job.Location?.ForEach(location => Console.WriteLine($"- {location}"));

	Console.WriteLine("Ansvarsområden:");
	job.JobResponsiblities?.ForEach(tool => Console.WriteLine($"- {tool}"));

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
