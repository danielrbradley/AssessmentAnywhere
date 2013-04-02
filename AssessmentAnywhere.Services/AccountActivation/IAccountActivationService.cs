namespace AssessmentAnywhere.Services.AccountActivation
{
    using AssessmentAnywhere.Services.Users;

    public interface IAccountActivationService
    {
        void BeginActivation(IUser userAccount);

        CompleteActivationResult TryCompleteActivation(string emailAddress, string code);
    }
}
