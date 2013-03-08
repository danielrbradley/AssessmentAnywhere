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

        public IEnumerable<AssessmentResult> Results
        {
            get
            {
                return this.results.Values;
            }
        }

        public Guid AddCandidate(string candidateName)
        {
            var assessmentResult = new AssessmentResult(candidateName);
            this.results.Add(assessmentResult.Id, assessmentResult);
            return assessmentResult.Id;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetCandidateName(Guid id, string newCandidateName)
        {
            var result = results[id].Result;
            this.results[id] = new AssessmentResult(id, newCandidateName, result);
        }

        public void SetCandidateResult(Guid id, decimal? result)
        {
            var candidateName = results[id].CandidateName;
            this.results[id] = new AssessmentResult(id, candidateName, result);
        }

        public void RemoveResult(Guid id)
        {
            results.Remove(id);
        }
    }
}