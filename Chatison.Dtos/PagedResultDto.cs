using System;
using System.Collections.Generic;

namespace Chatison.Dtos
{
    public class PagedResultDto<T>
    {
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public int TotalRecordsFiltered { get; set; }
        public double PageCount { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> ResultSet { get; set; }

        public void UpdatePageCount()
        {
            if (TotalRecords > PageSize)
            {
                PageCount = Math.Ceiling(Convert.ToDouble(TotalRecords) / Convert.ToDouble(PageSize));
            }
        }
    }
}
