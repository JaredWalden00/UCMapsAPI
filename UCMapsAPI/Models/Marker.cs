using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UCMapsAPI.Models;

namespace UCMapsAPI.Controllers
{
    public class Marker
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Lat { get; set; }
        public double Lng { get; set; }

        public User? User { get; set; }
        public int StillThereCount { get; set; } = 0;
        public int NotThereCount { get; set; } = 0;

        // Navigation property for votes
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
