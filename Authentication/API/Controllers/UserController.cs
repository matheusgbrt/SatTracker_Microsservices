using Authentication.API.DTO;
using Authentication.Application.Interfaces;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Authentication.API.Controllers
{
    [Route("/user")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(User), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Get user information.", Tags = new[] { "User Control" })]
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser([FromRoute] string username)
        {
            try
            {
                User? user = await _userService.GetUserByUsername(username) ?? throw new UserNotFoundException(username);

                return Ok(user.ConvertToDTO());
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Success", null, contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status409Conflict, "User already exists", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Create new user.", Tags = ["User Control"],OperationId ="1")]
        [HttpPost()]
        public async Task<IActionResult> CreateUser([FromBody] AuthenticationRequestDTO form)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _userService.CreateUser(form.login, form.password);

                return Accepted();
            }
            catch (UserAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
        }


        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Success", null, contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found.", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Delete existing user.", Tags = new[] { "User Control" })]
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string username)
        {
            try
            {
                await _userService.DeleteUser(username);
                return NoContent();
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Success", null, contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not found.", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Alter user info.", Tags = new[] { "User Control" })]
        [HttpPatch("{username}")]
        public async Task<IActionResult> PatchUser([FromBody] UserUpdateDTO userUpdateDTO, [FromRoute] string username)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                await _userService.PatchUser(username, userUpdateDTO.password);
                return Accepted();
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", null, contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User/Role not found", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status409Conflict, "User already has roles.", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Add roles to a user.", Tags = new[] { "User Role Control" })]
        [HttpPost("{username}/roles")]
        public async Task<IActionResult> InsertUserRoles(string username, [FromBody] List<RoleDTO> roleNames)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var rolesNames = roleNames.Select(r => r.roleName.ToUpper()).ToList();
                await _userService.AddUserRoles(username, rolesNames);
                return Created();
            }
            catch (Exception ex) when (ex is UserNotFoundException || ex is RoleNotFoundException)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Success", null, contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User/Role not found", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Add roles to a user.", Tags = new[] { "User Role Control" })]
        [HttpPut("{username}/roles")]
        public async Task<IActionResult> PutUserRoles(string username, [FromBody] List<RoleDTO> roleNames)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var rolesNames = roleNames.Select(r => r.roleName.ToUpper()).ToList();
                await _userService.DeleteUserRoles(username);
                await _userService.AddUserRoles(username, rolesNames);
                return Accepted();
            }
            catch (Exception ex) when (ex is UserNotFoundException || ex is RoleNotFoundException)
            {
                return NotFound(ex.Message);
            }

        }
        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status201Created, "Success", null, contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User/Role not found", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status409Conflict, "User already has roles.", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Add roles to a user.", Tags = new[] { "User Role Control" })]
        [HttpPatch("{username}/roles")]
        public async Task<IActionResult> PatchUserRoles(string username, [FromBody] List<RoleDTO> roleNames)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var rolesNames = roleNames.Select(r => r.roleName.ToUpper()).ToList();
                await _userService.AddMissingUserRoles(username, rolesNames);
                return Created();
            }
            catch (Exception ex) when (ex is UserNotFoundException || ex is RoleNotFoundException)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

    }
}
