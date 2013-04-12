namespace AssessmentAnywhere.Services.AccountActivation
{
    using AssessmentAnywhere.Services.Users;

    public interface IAccountActivationService
    {
        IAccountActivation BeginActivation(IUser userAccount);

        CompleteActivationResult TryCompleteActivation(string username, string code);
    }
}
