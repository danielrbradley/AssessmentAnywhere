namespace AssessmentAnywhere.Services.Repos
{
    using System.Collections.Generic;
    using System.Linq;

    using AssessmentAnywhere.Services.Repos.Models;

    public class CandidateRepo
    {
        public List<string> CandidateNames
        {
            get
            {
                var registerRepo = new RegistersRepo();
                List<Register> registers = registerRepo.GetRegisters();

                var names = new List<string>();

                if (registers == null)
                {
                    return new List<string>();
                }

                foreach (var candidate in from register in registers 
                                          where register != null && register.Candidates != null 
                                          from candidate in register.Candidates 
                                          where candidate != null && !string.IsNullOrEmpty(candidate.Name) 
                                          where !names.Contains(candidate.Name) 
                                          select candidate)
                {
                    names.Add(candidate.Name);
                }

                return names;
            }
        }
    }
}
