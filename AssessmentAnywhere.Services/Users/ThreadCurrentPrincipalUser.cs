namespace AssessmentAnywhere.Services.Users
{
    public static class ThreadCurrentPrincipalUser
    {
        public static IUser OpenCurrentUser(this IUserRepo userRepo)
        {
            return userRepo.Open(System.Threading.Thread.CurrentPrincipal.Identity.Name);
        }
    }
}
