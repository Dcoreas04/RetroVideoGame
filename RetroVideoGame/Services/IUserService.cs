
public interface IUserService
{
    User createUser(CreateUserDTO user);

    User? GetUserById(int id);
    User? updateUser(int id, UpdateUserDTO user);
    User? updatePartialUser(int id, UpdateUserDTO user);
    bool deleteUser(int id);
}