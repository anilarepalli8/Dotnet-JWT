using AutoMapper;
using JWT_Demo.DTO;
using JWT_Demo.HelperMethods;
using JWT_Demo.Models;
using JWT_Demo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankRepository _bankRepository;
        private readonly Jwthelper _jwthelper;
        public BankController(IBankRepository bankRepository,Jwthelper jwthelper)
        {
            _bankRepository = bankRepository;
            _jwthelper = jwthelper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> userRegistration(RegisterDto registerDto)
        {
            bool flag = _bankRepository.register(registerDto);
            return flag? Ok("Registration Successful"): BadRequest("Registration Failed");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> userLogin(LoginDto loginDto)
        {
            User user = _bankRepository.login(loginDto.Username,loginDto.Password);
            if (user == null)
                return Unauthorized("Invalid username or password");

             var token = _jwthelper.GenerateJwtToken(user.UserId);
            return Ok(new
            {
                Message = "Login Successful",
                Token = token
            });
        }


        [HttpGet("details")]
        [Authorize] 
        public async Task<IActionResult> userDetails()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            string userId = _jwthelper.ValidateJwtToken(token);

            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

            User user = _bankRepository.getDetails(userId);
            return user != null ? Ok(user) : NotFound("User not found");
        }

        [HttpPost("deposit")]
        [Authorize]
        public async Task<IActionResult> deposit([FromBody] DepositWithdrawDto depositWithdrawDto)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            string userId = _jwthelper.ValidateJwtToken(token);

            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

            decimal balance = _bankRepository.deposit(userId, depositWithdrawDto.Amount);
            return Ok(new { Balance = balance });
        }

        [HttpPost("withdraw")]
        [Authorize]
        public async Task<IActionResult> withdraw([FromBody] DepositWithdrawDto depositWithdrawDto)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            string userId = _jwthelper.ValidateJwtToken(token);

            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

            decimal balance = _bankRepository.withdraw(userId, depositWithdrawDto.Amount);
            return Ok(new { Balance = balance });
        }

        [HttpPost("transfer")]
        [Authorize]
        public async Task<IActionResult> transfer([FromBody] TransferDto transferDto)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            string userId = _jwthelper.ValidateJwtToken(token);

            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

            decimal balance = _bankRepository.transfer(userId, transferDto.Amount, transferDto.ReceiverAccountNumber);
            return Ok(new { Balance = balance });
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized("Invalid or missing token");

            var token = authHeader.Substring("Bearer ".Length).Trim();
            _jwthelper.RevokeToken(token);

            return Ok(new { Message = "Logout successful" });
        }


    }
}
