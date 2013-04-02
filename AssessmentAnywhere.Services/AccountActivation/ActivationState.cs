namespace AssessmentAnywhere.Services.AccountActivation
{
    public enum ActivationState
    {
        /// <summary>
        /// The account is pending user activation.
        /// </summary>
        Pending,

        /// <summary>
        /// The activation was not activated in time.
        /// </summary>
        Expired,

        /// <summary>
        /// The account has been activated.
        /// </summary>
        Complete,
    }
}
