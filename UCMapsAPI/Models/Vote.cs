using System.ComponentModel.DataAnnotations.Schema;
using UCMapsAPI.Controllers;

namespace UCMapsAPI.Models
{
    public class Vote
    {
        public int Id { get; set; }

        public int MarkerId { get; set; }
        [ForeignKey(nameof(MarkerId))]
        public Marker Marker { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public bool IsStillThere { get; set; }
    }

}
