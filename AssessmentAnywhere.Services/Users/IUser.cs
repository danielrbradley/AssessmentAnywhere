namespace AssessmentAnywhere.Services.Users
{
    public interface IUser
    {
        string Username { get; }

        string EmailAddress { get; }

        bool IsActive { get; }

        bool ValidatePassword(string passwordToTest);

        void ChangePassword(string newPassword, string existingPassword);

        void SetEmailAddress(string updatedEmailAddress);

        void Activate();
    }
}