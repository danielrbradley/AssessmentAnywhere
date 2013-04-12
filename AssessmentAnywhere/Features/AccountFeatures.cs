namespace AssessmentAnywhere.Features
{
    public class AccountFeatures
    {
        private readonly bool requireActivation;

        public AccountFeatures(bool requireActivation)
        {
            this.requireActivation = requireActivation;
        }

        public bool RequireActivation
        {
            get
            {
                return this.requireActivation;
            }
        }
    }
}
