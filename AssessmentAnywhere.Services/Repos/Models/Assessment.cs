namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Assessment
    {
        private readonly Dictionary<Guid, AssessmentResult> results = new Dictionary<Guid, AssessmentResult>();

        public Assessment(Guid id)
            : this(id, string.Empty)
        {
        }

        public Assessment(Guid id, string name)
            : this(id, name, Enumerable.Empty<AssessmentResult>())
        {
        }

        public Assessment(Guid id, string name, IEnumerable<AssessmentResult> results)
        {
            Id = id;
            Name = name;
            foreach (var assessmentResult in results)
            {
                this.results.Add(assessmentResult.Id, assessmentResult);
            }
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public IList<AssessmentResult> Results
        {
            get
            {
                return this.results.Values.ToList();
            }
        }

        public Guid AddCandidate(string surname, string forenames)
        {
            var assessmentResult = new AssessmentResult(surname, forenames);
            this.results.Add(assessmentResult.Id, assessmentResult);
            return assessmentResult.Id;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetCandidateNames(Guid id, string surname, string forenames)
        {
            var result = results[id].Result;
            this.results[id] = new AssessmentResult(id, surname, forenames, result);
        }

        public void SetCandidateResult(Guid id, decimal? result)
        {
            var oldCandidate = results[id];
            this.results[id] = new AssessmentResult(id, oldCandidate.Surname, oldCandidate.Forenames, result);
        }

        public void RemoveResult(Guid id)
        {
            results.Remove(id);
        }
    }
}