using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Dtos
{
    public class EncounterStatisticsStringDto
    {
        public string EncounterId { get; set; } 
        public int ActiveCount { get; set; }
        public int CompletedCount { get; set; }
        public int AbandonedCount { get; set; }
    }
}
