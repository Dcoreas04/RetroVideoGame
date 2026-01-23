
public class UserService : IUserService
{

    private readonly List<User> _users = new List<User>();
    public User createUser(CreateUserDTO userDTO)
    {
        var users = new User
        {
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

    public bool deleteUser(int id)
    {
        var user = GetUserById(id);
        if (user == null)
        {
            return false;
        }

        _users.Remove(user);
        return true;
    }

    public User? updatePartialUser(int id, UpdateUserDTO user)
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

    public User? updateUser(int id, UpdateUserDTO user)
    {
        var userToUpdate = GetUserById(id);
        if (userToUpdate == null)
        {
            return null;
        }

        userToUpdate.Name = user.Name;
        userToUpdate.Address = user.Address;

        return userToUpdate;
    }
}