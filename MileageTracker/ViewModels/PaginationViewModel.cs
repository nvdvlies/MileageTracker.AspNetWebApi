using System;
using System.Collections.Generic;

namespace MileageTracker.ViewModels {
    public class PaginationViewModel {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public IEnumerable<Object> Items { get; set; }
    }
}