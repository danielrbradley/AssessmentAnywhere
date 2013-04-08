namespace AssessmentAnywhere.Services.AccountActivation
{
    public interface IAccountActivationRepo
    {
        IAccountActivation CreateOrReplace(string username);

        bool Contains(string username);

        IAccountActivation Open(string username);
    }
}
