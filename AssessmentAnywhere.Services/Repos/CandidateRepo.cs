namespace AssessmentAnywhere.Services.Repos
{
    using System.Collections.Generic;
    using System.Linq;

    public class CandidateRepo
    {
        public List<string> CandidateNames
        {
            get
            {
                var assessmentsRepo = new AssessmentsRepo();
                var assessments = assessmentsRepo.QueryAssessments();

                return assessments.SelectMany(a => a.Results.Select(r => string.Concat(r.Surname, ", ", r.Forenames))).Distinct().ToList();
            }
        }
    }
}
