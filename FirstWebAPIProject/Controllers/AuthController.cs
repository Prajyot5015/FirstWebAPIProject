using FirstWebAPIProject.Model.DTO;
using FirstWebAPIProject.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDTO.Username,
                Email = registerDTO.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerDTO.Password);
            
            if(identityResult.Succeeded)
            {
                // Add Roles to this User

                if(registerDTO.Roles != null && registerDTO.Roles.Any())
                {
                   identityResult =  await userManager.AddToRolesAsync(identityUser,  registerDTO.Roles);
                    
                    if(identityResult.Succeeded)
                    {
                        return Ok("Registration Successful Please login");
                    }
                }

            }
            return BadRequest("Something Went Wrong");
        
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Username);

            if(user != null)
            {
              var checkPassResult =  await userManager.CheckPasswordAsync(user, loginDTO.Password);

                if(checkPassResult)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    if(roles != null)
                    {
                        // Create Token

                       var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };
                        
                        return Ok(response);
                    }
                }
            }
            return BadRequest("Username or Password is incorrect");
        }
    
    }
}
