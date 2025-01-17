﻿using JobData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Business.Business
{
    public interface IJobTrackerToolBusiness
    {
        StringBuilder CsvCreateAll(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles);
        StringBuilder CsvCreateSelected(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles);
        byte[] PdfCreate(Guid jobProfileId, IEnumerable<EmployerProfile> employerProfiles);

        StringBuilder ExcelParse();

    }
}
