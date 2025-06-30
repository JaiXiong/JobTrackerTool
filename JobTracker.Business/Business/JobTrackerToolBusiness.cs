
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using JobData.Common;
using JobData.Entities;
using JobData.Migrations;
using JobTracker.Business.Services;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;
using Utils.Operations;
using Verifiable.Core.Cryptography;

namespace JobTracker.Business.Business
{
    public class JobTrackerToolBusiness : IJobTrackerToolBusiness
    {
        private readonly ILogger<JobTrackerToolService> _logger;
        private ResxFormat _resx;
        ResourceManager _resourceManager;

        public JobTrackerToolBusiness(ILogger<JobTrackerToolService> logger)
        {
            _resourceManager = new ResourceManager("JobTracker.Business.JobTrackerBusinessErrors", typeof(JobTrackerToolService).Assembly);
            _resx = new ResxFormat(_resourceManager);
            _logger = logger;
        }
        public StringBuilder ExcelParse()
        {
            throw new NotImplementedException();
        }

        public byte[] CreateZipFile(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles, DownloadOptions downloadOptions)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var csvContent = DownloadCsv(jobProfileId, employerProfiles, downloadOptions).ToString();
                    var csvFile = archive.CreateEntry($"{jobProfileId}_employerProfiles.csv");
                    using (var entryStream = csvFile.Open())
                    using (var streamWriter = new StreamWriter(entryStream))
                    {
                        streamWriter.Write(csvContent);
                    }

                    var pdfContent = DownloadPdf(jobProfileId, employerProfiles, downloadOptions);
                    var pdfFile = archive.CreateEntry($"{jobProfileId}_employerProfiles.pdf");
                    using (var entryStream = pdfFile.Open())
                    {
                        entryStream.Write(pdfContent, 0, pdfContent.Length);
                    }
                }
                return memoryStream.ToArray();
            }
        }

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

        public List<EmployerProfile> UploadParsing(Stream fileStream, Guid jobProfileId)
        {
            var employerProfiles = new List<EmployerProfile>();
            var columnMapping = new Dictionary<string, int>();

            using (var reader = new StreamReader(fileStream))
            {
                string line;
                bool isHeader = true;

                while ((line = reader.ReadLine()) != null)
                {

                    var values = line.Split(',');

                    if (isHeader)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            columnMapping[values[i].Trim().ToLower()] = i;
                        }
                        isHeader = false;
                    }
                    else
                    {
                        try
                        {
                            var employerProfile = new EmployerProfile
                            {
                                Id = Guid.NewGuid(),
                                JobProfileId = jobProfileId,
                                Date = DateTime.Parse(values[columnMapping["date"]]),
                                LatestUpdate = DateTime.Parse(values[columnMapping["latestupdate"]]),
                                Name = values[columnMapping["name"]],
                                Title = values[columnMapping["title"]],
                                Address = values[columnMapping["address"]],
                                City = values[columnMapping["city"]],
                                State = values[columnMapping["state"]],
                                Zip = values[columnMapping["zip"]],
                                Phone = values[columnMapping["phone"]],
                                Email = values[columnMapping["email"]],
                                Website = values[columnMapping["website"]],
                                Result = new JobAction
                                {
                                    Id = Guid.NewGuid(),
                                    EmployerProfileId = Guid.Parse(values[columnMapping["id"]]),
                                    Action = values[columnMapping["action"]],
                                    ActionResult = values[columnMapping["actionresult"]],
                                    Date = DateTime.Parse(values[columnMapping["resultdate"]]),
                                    LatestUpdate = DateTime.Parse(values[columnMapping["resultlatestupdate"]])
                                },
                                Detail = new Detail
                                {
                                    Id = Guid.NewGuid(),
                                    EmployerProfileId = Guid.Parse(values[columnMapping["id"]]),
                                    Comments = values[columnMapping["detailcomments"]],
                                    Updates = values[columnMapping["detailupdate"]],
                                    Date = DateTime.Parse(values[columnMapping["detaildate"]]),
                                    LatestUpdate = DateTime.Parse(values[columnMapping["detaillatestupdate"]])
                                }
                            };
                            employerProfiles.Add(employerProfile);
                        }
                        catch (BusinessException ex)
                        {
                            _logger.LogError(ex, "An error occured while persing upload");
                            throw new BusinessException(_resx.Create("ParsingFailed"));
                        }
                    }

                }
            }
            return employerProfiles;
        }

        public byte[] ExportEmployerProfilesToExcel(IEnumerable<EmployerProfile> employerProfiles)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("EmployerProfiles");

            //Add headers
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "JobProfileId";
            worksheet.Cell(1, 3).Value = "Name";
            worksheet.Cell(1, 4).Value = "Title";
            worksheet.Cell(1, 5).Value = "Address";
            worksheet.Cell(1, 6).Value = "City";
            worksheet.Cell(1, 7).Value = "State";
            worksheet.Cell(1, 8).Value = "Zip";
            worksheet.Cell(1, 9).Value = "Phone";
            worksheet.Cell(1, 10).Value = "Email";
            worksheet.Cell(1, 11).Value = "Website";

            //Add Data
            int row = 2;
            foreach (var profile in employerProfiles)
            {
                worksheet.Cell(row, 1).Value = profile.Id.ToString();
                worksheet.Cell(row, 2).Value = profile.JobProfileId.ToString();
                worksheet.Cell(row, 3).Value = profile.Name;
                worksheet.Cell(row, 4).Value = profile.Title;
                worksheet.Cell(row, 5).Value = profile.Address;
                worksheet.Cell(row, 6).Value = profile.City;
                worksheet.Cell(row, 7).Value = profile.State;
                worksheet.Cell(row, 8).Value = profile.Zip;
                worksheet.Cell(row, 9).Value = profile.Phone;
                worksheet.Cell(row, 10).Value = profile.Email;
                worksheet.Cell(row, 11).Value = profile.Website;
                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        //public async Task<bool> VerifyNewEmail(string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        throw new ArgumentNullException(nameof(email), _resx.Create("EmailCannotBeNullOrEmpty"));
        //    }
        //    // Here you would typically check if the email already exists in your database
        //    // For demonstration purposes, we will assume the email is valid and return true
        //    var result = await _emailServices.VerifyEmail(email);
        //    if (result.Success)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        throw new BusinessException(_resx.Create("EmailAlreadyExists"));
        //    }
        //    //if (result.Success)
        //    //{
        //    //    return CreatedAtAction(nameof(CreateJobProfile), new { id = jobProfile.Id }, jobProfile);
        //    //}
        //    //else
        //    //{
        //    //    return BadRequest(new { result.Message, result.Errors });
        //    //}
        //    return true;
        //}
    }
}
