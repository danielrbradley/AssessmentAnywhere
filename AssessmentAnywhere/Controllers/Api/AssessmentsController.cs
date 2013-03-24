namespace AssessmentAnywhere.Controllers.Api
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using AssessmentAnywhere.Models.Api.Assessments;
    using AssessmentAnywhere.Services.AssessmentIndex;
    using AssessmentAnywhere.Services.Assessments;

    public class AssessmentsController : ApiController
    {
        private readonly IAssessmentIndex assessmentIndex;

        private readonly IAssessmentsRepo assessmentsRepo;

        public AssessmentsController()
            : this(new AssessmentsRepo(), new AssessmentIndex())
        {
        }

        public AssessmentsController(IAssessmentsRepo assessmentsRepo, IAssessmentIndex assessmentIndex)
        {
            this.assessmentsRepo = assessmentsRepo;
            this.assessmentIndex = assessmentIndex;
        }

        // GET api/assessments
        public GetModel Get()
        {
            var searchResult = this.assessmentIndex.Search(0, 100);
            return new GetModel
                       {
                           Results = searchResult.Results.Select(r => new Result { Id = r.Id, Name = r.Name }).ToList(),
                           TotalCount = searchResult.TotalCount
                       };
        }

        // POST api/assessments
        public void Post([FromBody]string name)
        {
            var assessment = this.assessmentsRepo.Create();
            assessment.SetName(name);
            this.assessmentIndex.Set(assessment.Id, name);
        }

        // DELETE api/assessments/5
        public void Delete(Guid id)
        {
            this.assessmentIndex.Delete(id);
            this.assessmentsRepo.Delete(id);
        }
    }
}
