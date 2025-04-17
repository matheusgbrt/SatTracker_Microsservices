using Authentication.API.DTO;
using Authentication.Application.Interfaces;
using Authentication.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Authentication.API.Controllers
{
    [Route("/user")]
    public class UserRolesController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        //[Authorize(Roles = "ADMIN")]
        //[Consumes("application/json")]
        //[SwaggerResponse(StatusCodes.Status201Created, "Success", null, contentTypes: new[] { "application/json" })]
        //[SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        //[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerResponse(StatusCodes.Status404NotFound, "User/Role not found", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerResponse(StatusCodes.Status409Conflict, "User already has roles.", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerOperation(Summary = "Add roles to a user.", Tags = new[] { "User Control" })]
        //[HttpPost("{username}/roles")]
        //public async Task<IActionResult> InsertUserRoles(string username, [FromBody] List<RoleDTO> roleNames)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);
        //        var rolesNames = roleNames.Select(r => r.roleName.ToUpper()).ToList();
        //        await _userService.AddUserRoles(username, rolesNames);
        //        return Created();
        //    }
        //    catch (Exception ex) when (ex is UserNotFoundException || ex is RoleNotFoundException)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return Conflict(ex.Message);
        //    }
        //}

        //[Authorize(Roles = "ADMIN")]
        //[Consumes("application/json")]
        //[SwaggerResponse(StatusCodes.Status201Created, "Success", null, contentTypes: new[] { "application/json" })]
        //[SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        //[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerResponse(StatusCodes.Status404NotFound, "User/Role not found", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerOperation(Summary = "Add roles to a user.", Tags = new[] { "User Control" })]
        //[HttpPut("{username}/roles")]
        //public async Task<IActionResult> PutUserRoles(string username, [FromBody] List<RoleDTO> roleNames)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);
        //        var rolesNames = roleNames.Select(r => r.roleName.ToUpper()).ToList();
        //        await _userService.DeleteUserRoles(username);
        //        await _userService.AddUserRoles(username, rolesNames);
        //        return Created();
        //    }
        //    catch (Exception ex) when (ex is UserNotFoundException || ex is RoleNotFoundException)
        //    {
        //        return NotFound(ex.Message);
        //    }

        //}
        //[Authorize(Roles = "ADMIN")]
        //[Consumes("application/json")]
        //[SwaggerResponse(StatusCodes.Status201Created, "Success", null, contentTypes: new[] { "application/json" })]
        //[SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        //[SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerResponse(StatusCodes.Status404NotFound, "User/Role not found", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerResponse(StatusCodes.Status409Conflict, "User already has roles.", typeof(string), contentTypes: new[] { "text/plain" })]
        //[SwaggerOperation(Summary = "Add roles to a user.", Tags = new[] { "User Control" })]
        //[HttpPatch("{username}/roles")]
        //public async Task<IActionResult> PatchUserRoles(string username, [FromBody] List<RoleDTO> roleNames)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);
        //        var rolesNames = roleNames.Select(r => r.roleName.ToUpper()).ToList();
        //        await _userService.AddMissingUserRoles(username, rolesNames);
        //        return Created();
        //    }
        //    catch (Exception ex) when (ex is UserNotFoundException || ex is RoleNotFoundException)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return Conflict(ex.Message);
        //    }
        //}
    }
}
