using Authentication.API.DTO;
using Authentication.Application.Interfaces;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace Authentication.API.Controllers
{
    [Route("/")]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {

        private readonly IAuthenticationService _authenticationService = authenticationService;
        [AllowAnonymous]
        [Consumes("application/json")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success", typeof(AuthenticationResponseDTO), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation failed", typeof(ValidationProblemDetails), contentTypes: new[] { "application/json" })]
        [SwaggerResponse(StatusCodes.Status404NotFound, "User not found", typeof(string), contentTypes: new[] { "text/plain" })]
        [SwaggerOperation(Summary = "Authentication method", Tags = new[] { "Authentication" })]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequestDTO form)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                User user = await _authenticationService.Authenticate(form.login, form.password);
                var token = _authenticationService.GenerateJWT(user);
                return Ok(new AuthenticationResponseDTO { token = token, expires = DateTime.UtcNow.AddHours(2) });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                {
                    return Unauthorized(ex.Message);
                }
            }
        }
    }
}
