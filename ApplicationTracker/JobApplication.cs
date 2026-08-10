using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTracker
{
    // Represents one job application record in the tracker.
    public class JobApplication
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = "";
        public string JobTitle { get; set; } = "";
        public string JobLocation { get; set; } = "";
        public DateTime DateApplied { get; set; } 
        public string JobStatus { get; set; } = "";
        public string Notes { get; set; } = "";
    }
}
