namespace AssessmentAnywhere.Services.Users
{
    public interface IUser
    {
        string Username { get; }

        bool ValidatePassword(string passwordToTest);

        void ChangePassword(string newPassword, string existingPassword);
    }
}