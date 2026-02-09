
public class UserService : IUserService
{

    private readonly List<User> _users = new List<User>();
    private static int _nextUserId = 1;
    public User CreateUser(CreateUserDTO userDTO)
    {
        var users = new User
        {
            Id = _nextUserId++,
            Name = userDTO.Name,
            Email = userDTO.Email,
            Password = userDTO.Password,
            Address = userDTO.Address
        };

        _users.Add(users);
        return users;
    }
    public User? GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public bool DeleteUser(int id)
    {
        var user = GetUserById(id);
        if (user == null)
        {
            return false;
        }

        _users.Remove(user);
        return true;
    }

    public User? UpdatePartialUser(int id, UpdateUserDTO user)
    {
        var userToUpdate = GetUserById(id);
        if (userToUpdate == null)
        {
            return null;
        }

        if (user.Name != null)
        {
            userToUpdate.Name = user.Name;
        }

        if (user.Address != null)
        {
            userToUpdate.Address = user.Address;
        }

        return userToUpdate;
    }

    public User? UpdateUser(int id, UpdateUserDTO user)
    {
        var userToUpdate = GetUserById(id);
        if (userToUpdate == null)
        {
            return null;
        }

        userToUpdate.Name = user.Name;
        userToUpdate.Address = user.Address;
        userToUpdate.Password = user.Password;

        return userToUpdate;
    }
}