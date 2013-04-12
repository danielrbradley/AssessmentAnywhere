namespace AssessmentAnywhere.Services.Assessments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Assessment : IAssessment
    {
        private readonly SortedSet<IAssessmentResult> results;

        public Assessment(Guid id)
            : this(id, string.Empty)
        {
        }

        public Assessment(Guid id, string name)
            : this(id, name, null, Enumerable.Empty<AssessmentResult>())
        {
        }

        public Assessment(Guid id, string name, decimal? totalMarks, IEnumerable<IAssessmentResult> results)
        {
            this.Id = id;
            this.Name = name;
            this.TotalMarks = totalMarks;
            this.results = new SortedSet<IAssessmentResult>(results);
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public decimal? TotalMarks { get; private set; }

        public IList<IAssessmentResult> Results
        {
            get
            {
                return this.results.ToList();
            }
        }

        public Guid AddCandidate(string surname, string forenames)
        {
            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new ArgumentException("Surname must have a value.", "surname");
            }

            if (string.IsNullOrWhiteSpace(forenames))
            {
                throw new ArgumentException("Forenames must have a value.", "forenames");
            }

            var assessmentResult = new AssessmentResult(surname, forenames);
            if (this.results.Contains(assessmentResult))
            {
                throw new UniqueConstraintException(string.Format("A candidate with name \"{0}, {1}\" alread exists.", surname, forenames));
            }

            this.results.Add(assessmentResult);
            return assessmentResult.Id;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must have a value.", "name");
            }

            this.Name = name;
        }

        public void SetTotalMarks(decimal? totalMarks)
        {
            if (totalMarks.HasValue)
            {
                if (totalMarks.Value < 1)
                {
                    throw new ArgumentOutOfRangeException("totalMarks", "Total marks value must be greater than zero.");
                }

                if (this.results.Any(r => r.Result > totalMarks))
                {
                    throw new ArgumentOutOfRangeException(
                        "totalMarks", "All results must be within than the total marks");
                }
            }

            this.TotalMarks = totalMarks;
        }

        public void SetCandidateNames(Guid id, string surname, string forenames)
        {
            if (string.IsNullOrWhiteSpace(surname))
            {
                throw new ArgumentException("Surname must have a value.", "surname");
            }

            if (string.IsNullOrWhiteSpace(forenames))
            {
                throw new ArgumentException("Forenames must have a value.", "forenames");
            }

            var existingResult = this.results.Single(r => r.Id == id);
            var newResult = new AssessmentResult(id, surname, forenames, existingResult.Result);

            if (this.results.Where(r => r.Id != existingResult.Id).Any(r => r.Surname == surname && r.Forenames == forenames))
            {
                throw new UniqueConstraintException(string.Format("A candidate with name \"{0}, {1}\" alread exists.", surname, forenames));
            }

            this.results.Remove(existingResult);
            this.results.Add(newResult);
        }

        public void SetCandidateResult(Guid id, decimal? result)
        {
            if (result.HasValue)
            {
                if (result < 0)
                {
                    throw new ArgumentOutOfRangeException("result", "Result be a positive number.");
                }

                if (this.TotalMarks.HasValue && result > this.TotalMarks)
                {
                    throw new ArgumentOutOfRangeException(
                        "result", "Result must be lower than or equal to the total marks.");
                }
            }

            var existingResult = this.results.Single(r => r.Id == id);
            var newResult = new AssessmentResult(id, existingResult.Surname, existingResult.Forenames, result);
            this.results.Remove(existingResult);
            this.results.Add(newResult);
        }

        public void RemoveResult(Guid id)
        {
            var result = this.results.Single(r => r.Id == id);
            this.results.Remove(result);
        }
    }
}
