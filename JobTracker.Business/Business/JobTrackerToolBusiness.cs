using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using JobData.Common;
using JobData.Entities;
using System.IO.Compression;
using System.Text;

namespace JobTracker.Business.Business
{
    public class JobTrackerToolBusiness : IJobTrackerToolBusiness
    {
        public StringBuilder ExcelParse()
        {
            throw new NotImplementedException();
        }

        //public StringBuilder CreateZipFile(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //        {
        //            var csvContent = DownloadContent(jobProfileId, employerProfiles, downloadOptions).ToString();
        //            var csvFile = archive.CreateEntry($"{jobProfileId}_employerProfiles.csv");
        //            using (var entryStream = csvFile.Open())
        //            using (var streamWriter = new StreamWriter(entryStream))
        //            {
        //                streamWriter.Write(csvContent);
        //            }

        //            var pdfContent = GeneratePdfContent(employerProfiles);
        //            var pdfFile = archive.CreateEntry($"{jobProfileId}_employerProfiles.pdf");
        //            using (var entryStream = pdfFile.Open())
        //            {
        //                entryStream.Write(pdfContent, 0, pdfContent.Length);
        //            }
        //        }
        //    }
        //}

        public StringBuilder DownloadCsv(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles, DownloadOptions downloadOptions)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Job Profile " + jobProfileId);

            if (downloadOptions.Include == DownloadType.Include)
            {
                csv.AppendLine("Id,Name,Title,Address,City,State,Zip,Phone,Email,Website,Action,ActionResult,ResultDate,ResultLatestUpdate,DetailUpdate,DetailDate,DetailLatestUpdate");

                foreach (var profile in employerProfiles)
                {
                    csv.AppendLine($"{profile.Id},{profile.Name},{profile.Title},{profile.Address},{profile.City},{profile.State},{profile.Zip},{profile.Phone},{profile.Email},{profile.Website},{profile.Result?.Action},{profile.Result?.ActionResult},{profile.Result?.Date},{profile.Result?.LatestUpdate},{profile.Detail?.Updates},{profile.Detail?.Date},{profile.Detail?.LatestUpdate}");
                }
            }
            else
            {
                csv.AppendLine("Id,Name,Title,Address,City,State,Zip,Phone,Email,Website");

                foreach (var profile in employerProfiles)
                {
                    csv.AppendLine($"{profile.Id},{profile.Name},{profile.Title},{profile.Address},{profile.City}, {profile.State},{profile.Zip}," +
                        $"{profile.Phone},{profile.Email},{profile.Website}");
                }
            }
            
            return csv;
        }

        public byte[] DownloadPdf(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles, DownloadOptions downloadOptions)
        {
            if (!employerProfiles.Any())
            {
                throw new ArgumentException("No employers found in pdf creation.");
            }

            using (var memoryStream = new MemoryStream())
            {
                var pdfWriter = new PdfWriter(memoryStream);
                var pdfDocument = new PdfDocument(pdfWriter);
                var document = new Document(pdfDocument);

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
