namespace AssessmentAnywhere.Services.AssessmentIndex
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    internal class AssessmentIndex : IAssessmentIndex
    {
        private static readonly ConcurrentDictionary<string, SortedSet<IAssessment>> UserDictionaries = new ConcurrentDictionary<string, SortedSet<IAssessment>>();

        private string CurrentUsername
        {
            get
            {
                return System.Threading.Thread.CurrentPrincipal.Identity.Name;
            }
        }

        private SortedSet<IAssessment> Assessments
        {
            get
            {
                return UserDictionaries.GetOrAdd(this.CurrentUsername, new SortedSet<IAssessment>());
            }
        }

        public IResultPage Search(int skip, int top)
        {
            return new ResultPage(this.Assessments.Skip(skip).Take(top).ToArray(), this.Assessments.Count);
        }

        public bool ContainsName(string name)
        {
            return Assessments.Contains(new Assessment(Guid.Empty, name));
        }

        public void Set(Guid assessmentId, string name)
        {
            Assessments.RemoveWhere(a => a.Id == assessmentId);
            Assessments.Add(new Assessment(assessmentId, name));
        }

        public void Delete(Guid assessmentId)
        {
            Assessments.RemoveWhere(a => a.Id == assessmentId);
        }
    }
}