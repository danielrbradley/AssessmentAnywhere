using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AssessmentAnywhere.Models.Stats;
using AssessmentAnywhere.Services.Services;

namespace AssessmentAnywhere.Controllers
{
    public class StatisticsController : Controller
    {
        
        public ActionResult ShowStatistics(Guid id)
        {
            var model = new AssessmentStatisticsModel
                {
                    AssessmentId = id,
                    Statistics = GetStats(id),
                    GradeCounts = GetGradeCounts(id)
                };
            return View(model);
        }

        public ActionResult PieChart(Dictionary<string, int> values)
        {
            return View(values);
        }


        private List<AssessmentStatistic> GetStats(Guid assessmentId)
        {
            return new Services.Services.AssessmentGradesService().GetStatsForAssessment(assessmentId, false);
            //var result = new List<AssessmentStatistic>
            //    {
            //        new AssessmentStatistic{StatisticName = "Min", StatisticValue = "23"}, 
            //        new AssessmentStatistic{StatisticName = "Max", StatisticValue = "87"},
            //        new AssessmentStatistic{StatisticName = "Average", StatisticValue = "52"}, 
            //    };
            //return result;
        }

        private Dictionary<string, int> GetGradeCounts(Guid assessmentId)
        {
            return new Services.Services.AssessmentGradesService().GetGradeCounts(assessmentId);
            //var result = new Dictionary<string, int>
            //    {
            //        {"A", 16},
            //        {"B", 22},
            //        {"C", 40},
            //        {"D", 22},
            //        {"E", 10}
            //    };
            //return result;
        }

    }
}
