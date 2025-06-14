namespace UCMapsAPI.Models.DTO
{
    public class UpdateMarkerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int UserId { get; set; }
    }
}
