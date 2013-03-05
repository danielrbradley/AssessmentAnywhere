namespace AssessmentAnywhere.Services.Repos
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AssessmentAnywhere.Services.Model;

    public class RegistersRepo
    {
        private static readonly Dictionary<Guid, Register> Registers = new Dictionary<Guid, Register>();

        public Register Create()
        {
            var newId = Guid.NewGuid();
            var register = new Register { Id = newId };
            Registers.Add(newId, register);
            return register;
        }

        public Register Open(Guid registerId)
        {
            return Registers[registerId];
        }

        public List<Register> GetRegisters()
        {
            return Registers.Values.ToList();
        }
        
    }
}
