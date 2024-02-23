namespace UsersAuthenticationJWT.Services.Users
{
    public interface IUserService
    {
        User GetUserName(string userName);

        bool CheckUserPassword(string userName, string password);
    }
}
