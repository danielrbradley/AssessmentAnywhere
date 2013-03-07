namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Assessment
    {
        private readonly List<AssessmentResult> results = new List<AssessmentResult>();

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
            this.results.AddRange(results);
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public IEnumerable<AssessmentResult> Results
        {
            get
            {
                return this.results.AsEnumerable();
            }
        }

        public void AddCandidate(string candidateName)
        {
            this.results.Add(new AssessmentResult(candidateName));
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void ChangeCandidateName(string candidateName, string newCandidateName)
        {
            var candidateIndex = this.results.FindIndex(r => r.CandidateName == candidateName);
            var result = this.results[candidateIndex].Result;
            this.results[candidateIndex] = new AssessmentResult(newCandidateName, result);
        }

        public void SetCandidateResult(string candidateName, decimal? result)
        {
            var candidateIndex = this.results.FindIndex(r => r.CandidateName == candidateName);
            this.results[candidateIndex] = new AssessmentResult(candidateName, result);
        }
    }
}