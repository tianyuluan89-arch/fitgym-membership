using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitGym.Models;
using FitGym.Services;

namespace FitGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly ITokenService _tokenService;

        public AuthController(IMemberService memberService, ITokenService tokenService)
        {
            _memberService = memberService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Register a new member and return an authentication token.
        /// POST: api/auth/register
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<string> Register(RegisterRequest request)
        {
            try
            {
                var member = _memberService.RegisterMember(
                    request.FullName, request.Email, request.Phone, request.DateOfBirth);
                var token = _tokenService.CreateToken(member.Id, "Member");
                return Ok(new { token });
            }
            catch (InvalidOperationException ex) when (ex.Message == "DUPLICATE_EMAIL")
            {
                return Conflict(new { error = "Email is already registered." });
            }
        }

        /// <summary>
        /// Login and return an authentication token (simple credentials check for the course).
        /// POST: api/auth/login
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<string> Login(LoginRequest request)
        {
            // Simplified login for the course; real password hashing is out of scope
            var token = _tokenService.CreateToken(request.UserId, "Member");
            return Ok(new { token });
        }
    }

    public class RegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class LoginRequest
    {
        public int UserId { get; set; }
    }
}
