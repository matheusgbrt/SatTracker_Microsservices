using Authentication.API.DTO;
using Authentication.Application.Interfaces;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Authentication.API.Controllers
{
    [Route("/roles")]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;
        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(List<RoleDTO>), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "List all roles", Tags = new[] { "Role Control" })]
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(Role.ConvertToDTO(await _roleService.GetAllRoles()));
        }
        [Authorize(Roles = "ADMIN")]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status202Accepted, "Success", null, contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Forbidden", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Role already exists", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Create new role.", Tags = new[] { "Role Control" })]
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] RoleDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _roleService.CreateRole(dto.roleName.ToUpper());
                return Accepted();
            }
            catch (RoleAlreadyExistsException ex)
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
        [SwaggerResponse(StatusCodes.Status409Conflict, "Role is in use.", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Delete existing role.", Tags = new[] { "Role Control" })]
        [HttpDelete("{roleName}")]
        public async Task<IActionResult> Delete([FromRoute] string roleName)
        {
            try
            {
                await _roleService.DeleteRole(roleName.ToUpper());
                return NoContent();
            }
            catch (RoleNotFoundException ex)
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
