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

    public class PaginationExceededException : Exception
    {
        public PaginationExceededException(int current, int max) : base($"The max take must be {max}. Current: {current}")
        { 
        }
    }
}
