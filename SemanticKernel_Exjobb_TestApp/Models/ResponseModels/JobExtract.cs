using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SemanticKernel_Exjobb_TestApp.Models.ResponseModels
{

	public class JobExtract
	{
		// Job adds can have multiple titles. Thereby we have a list of titles.
		public List<string> EmploymentTitle { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))] // Ensures enum values are serialized/deserialized as strings (e.g. "Tillsvidare") instead of integers, which is required for correct LLM output parsing.
		public EmploymentType EmploymentType { get; set; }              // T.ex. "Tillsvidare"

		public bool IsTrialEmployment { get; set; }              // T.ex. "Provanställning"

		public string Location { get; set; }                    // T.ex. "Karlstad, Stockholm, Distans"

		public List<string> ToolsAndTechnologies { get; set; }  // ["C#", ".NET", "Blazor"]

		public List<string> RequiredSkills { get; set; }        // ["SQL", "agilt arbete"]
	
		public List<string> MeritingSkills { get; set; }        // ["RabbitMQ", "Azure DevOps"]

		public List<string> SoftSkills { get; set; } // ["samarbetsorienterad", "problemlösande"]

		[JsonConverter(typeof(JsonStringEnumConverter))] // Ensures enum values are serialized/deserialized as strings (e.g. "Mid") instead of integers, which is required for correct LLM output parsing.
		public ExperienceLevel ExperienceLevel { get; set; } // Junior, Mid, Senior

		public ContactPerson Contact { get; set; }

		public string ReferenceNumber { get; set; }

		public DateTime ApplicationDeadline { get; set; }         // "2025-04-28"

		public string Employer { get; set; }
	}

	public class ContactPerson
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
	}

	public enum ExperienceLevel
	{
		Junior,
		Mid,
		Senior
	}

	public enum EmploymentType
	{
		Tillsvidare,
		Tidsbegränsad,
		Projekt,
		Praktik
	}

	/*public class JobExtract2
	{
		public string EmploymentTitle { get; set; }
		public string EmploymentType { get; set; }              // T.ex. "Tillsvidare"
		[Description("Location of the employment")]
		public string Location { get; set; }                    // T.ex. "Örebro eller Kista"
		[Description("Tools and technologies used in the employment")]
		public List<string> ToolsAndTechnologies { get; set; }  // ["C#", ".NET", "Blazor"]

		[Description($"Only type the skill. Ex. \"SQL\", \"Agilt arbete\"")]
		public List<string> RequiredSkills { get; set; }        // ["SQL", "agilt arbete"]
		[Description($"Only type the skill. Ex. \"SQL\", \"Agilt arbete\"")]
		public List<string> MeritingSkills { get; set; }        // ["RabbitMQ", "Azure DevOps"]
		public string ApplicationDeadline { get; set; }         // "2025-04-28"
																//public ContactPerson2 Contact { get; set; }
	}*/
}
