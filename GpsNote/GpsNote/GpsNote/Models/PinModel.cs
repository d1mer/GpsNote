using SQLite;

namespace GpsNote.Models
{
    public class PinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [Unique]
        public string Label { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Owner { get; set; }
        public bool IsEnable { get; set; }
    }
}