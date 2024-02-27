namespace UsersAuthenticationJWT.Services.Users.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid value) :
            base($"User with id {value} not found.")
        { }
    }

    public class UserServiceException : Exception
    {
        public UserServiceException(string message) : base(message)
        { }
    }
}
