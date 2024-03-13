﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Dtos
{
    public class StakeholdersCoordinateDto
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public StakeholdersCoordinateDto() { }

        public StakeholdersCoordinateDto(decimal latitude, decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            StakeholdersCoordinateDto other = (StakeholdersCoordinateDto)obj;
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }
    }
}
