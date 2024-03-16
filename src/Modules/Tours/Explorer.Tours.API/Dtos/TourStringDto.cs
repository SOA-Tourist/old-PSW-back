using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Explorer.Tours.API.Dtos
{

    public class TourStringDto
    {
        // Changed type from long to string for UUID compatibility
        public string Id { get; set; }
        public long AuthorId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Difficult Difficult { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; set; }

        public double Price { get; set; }
        public string Tags { get; set; }
        public double Distance { get; set; }
        public DateTime PublishTime { get; set; }
        public DateTime ArchiveTime { get; set; }

        public List<TravelTimeAndMethodDto> TravelTimeAndMethod { get; set; } = new List<TravelTimeAndMethodDto>();
        public List<EquipmentDto> TourEquipment { get; set; } = new List<EquipmentDto>();
    }
}
