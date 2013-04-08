namespace AssessmentAnywhere.Services.AccountActivation
{
    using System;

    public interface IAccountActivation
    {
        string ActivationCode { get; }

        string Username { get; }

        DateTime Expiry { get; }

        ActivationState State { get; }

        bool TryComplete();
    }
}
