using JobData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobData.Dtos
{
    public class PagingResponse<T>(IEnumerable<T> data, int totalCount)
    {
        public IEnumerable<T> Data { get; set; } = data;

        public int TotalCount { get; set; } = totalCount;
    }
}
