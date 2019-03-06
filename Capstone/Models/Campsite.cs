using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Campsite
    {
        public int SiteId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public byte Accessible { get; set; }
        public int MaxRvLength { get; set; }
        public byte Utilities { get; set; }
        public decimal DailyFee { get; set; }

    }
}
