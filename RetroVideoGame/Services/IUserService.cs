
public interface IUserService
{
    User CreateUser(CreateUserDTO user);

    User? GetUserById(int id);
    User? UpdateUser(int id, UpdateUserDTO user);
    User? UpdatePartialUser(int id, UpdateUserDTO user);
    bool DeleteUser(int id);
}