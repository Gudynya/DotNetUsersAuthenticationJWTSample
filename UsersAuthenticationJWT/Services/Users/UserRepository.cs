using UsersAuthenticationJWT.Entities;
using UsersAuthenticationJWT.Services.Storage;
using UsersAuthenticationJWT.Services.Users.Exceptions;

namespace UsersAuthenticationJWT.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IStorageService StorageService;

        public UserRepository(IStorageService storageService)
        {
            this.StorageService = storageService;
        }

        public async Task<IEnumerable<User>> GetUsersAsync(
            Func<User, bool> predicate, 
            int skip,
            int take,
            CancellationToken cancellationToken = default)
        {
            if (predicate == null) 
            {
                return this.StorageService.GetUsersStorage().Values.OrderBy(x => x.Id).Skip(skip).Take(take).AsEnumerable();
            }
            
            return this.StorageService.GetUsersStorage().Values
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .Where(predicate)
                .AsEnumerable();
        }

        public async Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            this.StorageService.GetUsersStorage().TryGetValue(id, out var user);
            return user;
        }

        public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user.Id == default)
            {
                user.Id = Guid.NewGuid();
                this.StorageService.GetUsersStorage().TryAdd(user.Id, user);
                return user;
            }

            throw new UserServiceException("To add a new user, the Id must be default value");
            
        }

        public async Task UpdateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            if (user.Id == default)
            {
                throw new UserServiceException("To update a new user, the Id must not be default value");
            }
            
            if (this.StorageService.GetUsersStorage().TryGetValue(user.Id, out _))
            {
                this.StorageService.GetUsersStorage()[user.Id] = user;
            }
        }

        public async Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (this.StorageService.GetUsersStorage().Remove(id, out var _))
            {
                throw new UserNotFoundException(id);
            }
        }
    }
}
