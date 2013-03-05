﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssessmentAnywhere.Services.Services;

namespace AssessmentAnywhere.Models.Stats
{
    public class AssessmentStatisticsModel
    {
        public Guid AssessmentId { get; set; }
        public List<AssessmentStatistic> Statistics { get; set; }

        public Dictionary<string, int> GradeCounts { get; set; }
    }
}