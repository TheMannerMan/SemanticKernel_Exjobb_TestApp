using System;
using System.Collections;
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
		[Description("Enbart Tjänsten titel, t.ex. arkitekt, systemutvecklare, snickare. Obligatorisk")]
		public List<string> EmploymentTitle { get; set; }

		[Description("Anställningstyp. Tillsvidare,Tidsbegränsad,Projekt eller Praktik. Returnera null om okänd")]
		[JsonConverter(typeof(JsonStringEnumConverter))] // Ensures enum values are serialized/deserialized as strings (e.g. "Tillsvidare") instead of integers, which is required for correct LLM output parsing.
		public EmploymentType? EmploymentType { get; set; }              // T.ex. "Tillsvidare"

		[Description("Är anställningen provanställning? Svara true, false eller null om okänt.")]
		public bool? IsTrialEmployment { get; set; }              // T.ex. "Provanställning"
		
		[Description("Primär anställningsort. Ange samtliga om fler. Ange stad. Svara distans om distans. Returnera null om okänd")]
		public List<string> Location { get; set; }                    // T.ex. "Karlstad, Stockholm, Distans"

		[Description("Obligatoriska färdigheter för tjänsten, såsom erfarenhet inom specifika områden eller teknologier. Svara enbart med nyckelord. (JavaScript, SQL, MassTransit). Obligatoriskt. Särskilj från meriterande färdigheter")]
		public List<string> RequiredSkills { get; set; }        // ["SQL", "agilt arbete"]

		[Description("Meriterande färdigheter för tjänsten, såsom erfarenhet inom specifika områden eller teknologier. Svara enbart med nyckelord (JavaScript, SQL, MassTransit). Obligatoriskt. Särskilj från obligatoriska färdigheter")]
		public List<string> MeritingSkills { get; set; }        // ["RabbitMQ", "Azure DevOps"]

		[Description("Kontaktperson för annonsen. Returnera null om okänd. Obligatoriskt")]
		public ContactPerson? Contact { get; set; }

		[Description("Referensnummer för annonsen. Returnera null om okänd")]
		public string? ReferenceNumber { get; set; }

		[Description("Sista ansökningsdag. Returnera null om okänd. Svara i följande format YYYY-MM-DD. ")]
		public string? ApplicationDeadline { get; set; }
		
		[Description("Arbetsgivare. Obligatoriskt.")]
		public string Employer { get; set; }
	}

	public class ContactPerson
	{
		[Description("Namn på kontaktperson. Svara med Förnamn Efternamn. Returnera null om okänd.")]
		public string? Name { get; set; }
		[Description("Epost till kontaktperson. Returnera null om okänd.")]
		public string? Email { get; set; }
		[Description("Telefonnummer till kontaktperson. Returnera null om okänd. Om känt, returnera följande format. Exempel: +46-70-123-45-67")]
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
