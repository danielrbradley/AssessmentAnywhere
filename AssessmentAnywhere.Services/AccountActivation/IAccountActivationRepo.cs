namespace AssessmentAnywhere.Services.AccountActivation
{
    public interface IAccountActivationRepo
    {
        IAccountActivation CreateOrReplace(string emailAddress, string username);

        bool Contains(string emailAddress);

        IAccountActivation Open(string emailAddress);
    }
}
