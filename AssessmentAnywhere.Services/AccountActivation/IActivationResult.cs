namespace AssessmentAnywhere.Services.AccountActivation
{
    public interface IActivationResult
    {
        bool Success { get; }

        string ErrorMessage { get; }

        string Username { get; }
    }
}