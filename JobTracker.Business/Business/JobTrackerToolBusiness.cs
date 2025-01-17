using iTextSharp.text;
using iTextSharp.text.pdf;
using JobData.Entities;
using System.Text;

namespace JobTracker.Business.Business
{
    public class JobTrackerToolBusiness : IJobTrackerToolBusiness
    {
        public StringBuilder CsvCreateAll(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles)
        {
            if (!employerProfiles.Any())
            {
                throw new ArgumentException("No employers found in csv creation.");
            }

            var csv = new StringBuilder();
            csv.AppendLine("Job Profile " + jobProfileId);
            csv.AppendLine("Id,Name,Title,Address,City,State,Zip,Phone,Email,Website, Action, ActionResult, ResultDate, ResultLatestUpdate, DetailUpdate, DetailDate, DetailLatestUpdate");

            foreach (var profile in employerProfiles)
            {
                csv.AppendLine($"{profile.Id},{profile.Name},{profile.Title},{profile.Address},{profile.City}, {profile.State},{profile.Zip}," +
                    $"{profile.Phone},{profile.Email},{profile.Website}, {profile.Result.Action}, {profile.Result.ActionResult}, {profile.Result.Date}, " +
                    $"{profile.Result.LatestUpdate},{profile.Detail.Updates}, {profile.Detail.Date}, {profile.Detail.LatestUpdate}");
            }

            return csv;
        }

        public StringBuilder CsvCreateSelected(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles)
        {
            if (!employerProfiles.Any())
            {
                throw new ArgumentException("No employers found in csv creation.");
            }

            var csv = new StringBuilder();
            csv.AppendLine("Job Profile " + jobProfileId);
            csv.AppendLine("Id,Name,Title,Address,City,State,Zip,Phone,Email,Website");

            foreach (var profile in employerProfiles)
            {
                csv.AppendLine($"{profile.Id},{profile.Name},{profile.Title},{profile.Address},{profile.City}, {profile.State},{profile.Zip}," +
                    $"{profile.Phone},{profile.Email},{profile.Website}");
            }

            return csv;
        }

        public StringBuilder ExcelParse()
        {
            throw new NotImplementedException();
        }

        public byte[] PdfCreate(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles)
        {
            if (!employerProfiles.Any())
            {
                throw new ArgumentException("No employers found in pdf creation.");
            }

            using (var memoryStream = new MemoryStream())
            {
                var document = new Document();
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                foreach (var profile in employerProfiles)
                {
                    document.Add(new Paragraph($"Id: {profile.Id}"));
                    document.Add(new Paragraph($"Name: {profile.Name}"));
                    document.Add(new Paragraph($"Title: {profile.Title}"));
                    document.Add(new Paragraph($"Address: {profile.Address}"));
                    document.Add(new Paragraph($"City: {profile.City}"));
                    document.Add(new Paragraph($"State: {profile.State}"));
                    document.Add(new Paragraph($"Zip: {profile.Zip}"));
                    document.Add(new Paragraph($"Phone: {profile.Phone}"));
                    document.Add(new Paragraph($"Email: {profile.Email}"));
                    document.Add(new Paragraph($"Website: {profile.Website}"));
                    document.Add(new Paragraph("\n"));
                }

                document.Close();
                return memoryStream.ToArray();
            }
        }
    }
}
