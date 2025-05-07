using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticKernel_Exjobb_TestApp.Models
{
	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class ApplicationDetails
	{
		public object information { get; set; }
		public string reference { get; set; }
		public object email { get; set; }
		public bool via_af { get; set; }
		public string url { get; set; }
		public object other { get; set; }
	}

	public class Description
	{
		public string text { get; set; }
		public string text_formatted { get; set; }
		public object company_information { get; set; }
		public object needs { get; set; }
		public object requirements { get; set; }
		public string conditions { get; set; }
	}

	public class Duration
	{
		public string concept_id { get; set; }
		public string label { get; set; }
		public string legacy_ams_taxonomy_id { get; set; }
	}

	public class Employer
	{
		public object phone_number { get; set; }
		public object email { get; set; }
		public string url { get; set; }
		public string organization_number { get; set; }
		public string name { get; set; }
		public string workplace { get; set; }
	}

	public class EmploymentType
	{
		public string concept_id { get; set; }
		public string label { get; set; }
		public string legacy_ams_taxonomy_id { get; set; }
	}

	public class MustHave
	{
		public List<object> skills { get; set; }
		public List<object> languages { get; set; }
		public List<WorkExperience> work_experiences { get; set; }
		public List<object> education { get; set; }
		public List<object> education_level { get; set; }
	}

	public class NiceToHave
	{
		public List<object> skills { get; set; }
		public List<object> languages { get; set; }
		public List<object> work_experiences { get; set; }
		public List<object> education { get; set; }
		public List<object> education_level { get; set; }
	}

	public class Occupation
	{
		public string concept_id { get; set; }
		public string label { get; set; }
		public string legacy_ams_taxonomy_id { get; set; }
	}

	public class OccupationField
	{
		public string concept_id { get; set; }
		public string label { get; set; }
		public string legacy_ams_taxonomy_id { get; set; }
	}

	public class OccupationGroup
	{
		public string concept_id { get; set; }
		public string label { get; set; }
		public string legacy_ams_taxonomy_id { get; set; }
	}

	public class JobAdApiData
	{
		public string id { get; set; }
		public string external_id { get; set; }
		public object original_id { get; set; }
		public string label { get; set; }
		public string webpage_url { get; set; }
		public string logo_url { get; set; }
		public string headline { get; set; }
		public DateTime application_deadline { get; set; }
		public int number_of_vacancies { get; set; }
		public Description description { get; set; }
		public EmploymentType employment_type { get; set; }
		public SalaryType salary_type { get; set; }
		public string salary_description { get; set; }
		public Duration duration { get; set; }
		public WorkingHoursType working_hours_type { get; set; }
		public ScopeOfWork scope_of_work { get; set; }
		public object access { get; set; }
		public Employer employer { get; set; }
		public ApplicationDetails application_details { get; set; }
		public bool experience_required { get; set; }
		public bool access_to_own_car { get; set; }
		public bool driving_license_required { get; set; }
		public object driving_license { get; set; }
		public Occupation occupation { get; set; }
		public OccupationGroup occupation_group { get; set; }
		public OccupationField occupation_field { get; set; }
		public WorkplaceAddress workplace_address { get; set; }
		public MustHave must_have { get; set; }
		public NiceToHave nice_to_have { get; set; }
		public List<object> application_contacts { get; set; }
		public DateTime publication_date { get; set; }
		public DateTime last_publication_date { get; set; }
		public bool removed { get; set; }
		public object removed_date { get; set; }
		public string source_type { get; set; }
		public long timestamp { get; set; }
	}

	public class SalaryType
	{
		public string concept_id { get; set; }
		public string label { get; set; }
		public string legacy_ams_taxonomy_id { get; set; }
	}

	public class ScopeOfWork
	{
		public int min { get; set; }
		public int max { get; set; }
	}

	public class WorkExperience
	{
		public int weight { get; set; }
		public string concept_id { get; set; }
		public string label { get; set; }
		public string legacy_ams_taxonomy_id { get; set; }
	}

	public class WorkingHoursType
	{
		public string concept_id { get; set; }
		public string label { get; set; }
		public string legacy_ams_taxonomy_id { get; set; }
	}

	public class WorkplaceAddress
	{
		public string municipality { get; set; }
		public string municipality_code { get; set; }
		public string municipality_concept_id { get; set; }
		public string region { get; set; }
		public string region_code { get; set; }
		public string region_concept_id { get; set; }
		public string country { get; set; }
		public string country_code { get; set; }
		public string country_concept_id { get; set; }
		public object street_address { get; set; }
		public object postcode { get; set; }
		public object city { get; set; }
		public List<double> coordinates { get; set; }
	}


}
