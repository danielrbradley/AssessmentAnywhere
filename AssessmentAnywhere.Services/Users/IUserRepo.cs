namespace AssessmentAnywhere.Services.Users
{
    public interface IUserRepo
    {
        IUser Create(string username, string password, string emailAddress);

        bool Exists(string username);

        bool EmailAddressExists(string emailAddress);

        IUser Open(string username);
    }
}