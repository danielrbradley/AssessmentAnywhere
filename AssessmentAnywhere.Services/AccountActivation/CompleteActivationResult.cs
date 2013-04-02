namespace AssessmentAnywhere.Services.AccountActivation
{
    public enum CompleteActivationResult
    {
        /// <summary>
        /// Account successfully activated.
        /// </summary>
        Success,

        /// <summary>
        /// Account activation code expired.
        /// </summary>
        Expired,

        /// <summary>
        /// Account activation not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// Account activation code incorrect.
        /// </summary>
        IncorrectCode,
    }
}
