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
		public EmploymentType? EmploymentType { get; set; }              // T.ex. "Tillsvidare"
		public bool? IsTrialEmployment { get; set; }              // T.ex. "Provanställning"
		public List<string> JobLocation { get; set; }                    // T.ex. "Karlstad, Stockholm, Distans"

		public List<string> RequiredSkills { get; set; }        // ["SQL", "agilt arbete"]
	
		public List<string> MeritingSkills { get; set; }        // ["RabbitMQ", "Azure DevOps"]

		public ContactPerson? Contact { get; set; }

		public string? ReferenceNumber { get; set; }

		public string?  ApplicationDeadline { get; set; }         // "2025-04-28"

		public string Employer { get; set; }
	}

	public class ContactPerson
	{
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
	}
	public enum EmploymentType
	{
		Tillsvidare,
		Tidsbegränsad,
		Projekt,
		Praktik
	}
}
