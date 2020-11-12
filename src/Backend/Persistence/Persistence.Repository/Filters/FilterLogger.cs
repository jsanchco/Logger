using System;

namespace Persistence.Repository.Filters
{
    public class FilterLogger
    {
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 10;

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
