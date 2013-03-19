namespace AssessmentAnywhere.Services.Repos.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Assessment
    {
        private readonly SortedSet<AssessmentResult> results;

        public Assessment(Guid id)
            : this(id, string.Empty)
        {
        }

        public Assessment(Guid id, string name)
            : this(id, name, null, Enumerable.Empty<AssessmentResult>())
        {
        }

        public Assessment(Guid id, string name, decimal? totalMarks, IEnumerable<AssessmentResult> results)
        {
            Id = id;
            Name = name;
            TotalMarks = totalMarks;
            this.results = new SortedSet<AssessmentResult>(results);
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public decimal? TotalMarks { get; private set; }

        public IList<AssessmentResult> Results
        {
            get
            {
                return this.results.ToList();
            }
        }

        public Guid AddCandidate(string surname, string forenames)
        {
            var assessmentResult = new AssessmentResult(surname, forenames);
            this.results.Add(assessmentResult);
            return assessmentResult.Id;
        }

        public void SetName(string name)
        {
            this.Name = name;
        }

        public void SetTotalMarks(decimal? totalMarks)
        {
            this.TotalMarks = totalMarks;
        }

        public void SetCandidateNames(Guid id, string surname, string forenames)
        {
            var existingResult = results.Single(r => r.Id == id);
            var newResult = new AssessmentResult(id, surname, forenames, existingResult.Result);
            this.results.Remove(existingResult);
            this.results.Add(newResult);
        }

        public void SetCandidateResult(Guid id, decimal? result)
        {
            var existingResult = results.Single(r => r.Id == id);
            var newResult = new AssessmentResult(id, existingResult.Surname, existingResult.Forenames, result);
            this.results.Remove(existingResult);
            this.results.Add(newResult);
        }

        public void RemoveResult(Guid id)
        {
            var result = results.Single(r => r.Id == id);
            results.Remove(result);
        }
    }
}