using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{id:int}")]
    public ActionResult<User> GetUserById(int id)
    {
        var user = _userService.GetUserById(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public ActionResult<User> CreateUser([FromBody] CreateUserDTO user)
    {
        var created = _userService.createUser(user);
        return Created($"/users/{created.Id}", created);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteUser(int id)
    {
        _userService.deleteUser(id);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public ActionResult<User> UpdateUser(int id, [FromBody] UpdateUserDTO user)
    {
        var updated = _userService.updateUser(id, user);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id:int}")]
    public ActionResult<User> UpdatePartialUser(int id, [FromBody] UpdateUserDTO user)
    {
        var updated = _userService.updatePartialUser(id, user);
        return updated is null ? NotFound() : Ok(updated);
    }
}
