using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Dtos
{
    public class CheckpointStringDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? TourId { get; set; }
        public PublicRequestDto PublicRequest { get; set; }
    }
}
