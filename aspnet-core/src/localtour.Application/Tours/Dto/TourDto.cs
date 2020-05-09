﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace localtour.Tours.Dto
{
    [AutoMapFrom(typeof(CreateOrEditTourDto))]
    public class TourDto : EntityDto
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public long? UserId { get; set; }
    }
}
