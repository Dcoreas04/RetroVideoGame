
using Microsoft.AspNetCore.Mvc;

public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly List<User> _users;
    public UsersController()
    {
        
    }

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id}")]
    public User? getUserById(int id)
    {
        return _userService.GetUserById(id);
    }

    [HttpPost]
    public User createUser(CreateUserDTO user)
    {
        return _userService.createUser(user);
    }

    [HttpDelete("{id}")]
    public void deleteUser(int id)
    {
        _userService.deleteUser(id);
    }

    [HttpPut("{id}")]
    public User? updateUser(int id, UpdateUserDTO user)
    {
        return _userService.updateUser(id, user);
    }

    [HttpPatch("{id}")]
    public User? updatePartialUser(int id, UpdateUserDTO user)
    {
        return _userService.updatePartialUser(id, user);
    }
    
}