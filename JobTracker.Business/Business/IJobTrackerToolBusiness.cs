
using JobData.Common;
using JobData.Entities;
using System.Text;

namespace JobTracker.Business.Business
{
    public interface IJobTrackerToolBusiness
    {
        StringBuilder DownloadCsv(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles, DownloadOptions downloadOptions);
        byte[] DownloadPdf(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles, DownloadOptions downloadOptions);
        byte[] CreateZipFile(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles, DownloadOptions downloadOptions);
        byte[] ExportEmployerProfilesToExcel(IEnumerable<EmployerProfile> employerProfiles);
        List<EmployerProfile> UploadParsing(Stream stream, Guid jobProfileId);

    }
}
