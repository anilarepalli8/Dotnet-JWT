using JWT_Demo.CustomException;
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

        public BankController(IBankRepository bankRepository, Jwthelper jwthelper)
        {
            _bankRepository = bankRepository;
            _jwthelper = jwthelper;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return _bankRepository.Register(registerDto)
                ? Ok("Registration Successful")
                : BadRequest("Registration Failed");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = _bankRepository.Login(loginDto.Username, loginDto.Password);
            if (user == null) return Unauthorized("Invalid username or password");
              
            var token = _jwthelper.GenerateJwtToken(user.UserId);
            return Ok(new { Message = "Login Successful", Token = token });
        }

        [HttpGet("details")]
        public IActionResult UserDetails()
        {
            var userId = _jwthelper.GetUserIdFromToken(HttpContext);
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

            var user = _bankRepository.GetDetails(userId);
            return user != null ? Ok(user) : NotFound("User not found");
        }

        [HttpPost("deposit"), Authorize]
        public IActionResult Deposit([FromBody] DepositWithdrawDto dto)
        {
            try
            {
                var userId = _jwthelper.GetUserIdFromToken(HttpContext);
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

                var balance = _bankRepository.Deposit(userId, dto.Amount);
                return Ok(new { Message = "Deposit Successful", Balance = balance });
            }
            catch(Error e) { return NotFound(e.Message); }
        }

        [HttpPost("withdraw"), Authorize]
        public IActionResult Withdraw([FromBody] DepositWithdrawDto dto)
        {
            try
            {
                var userId = _jwthelper.GetUserIdFromToken(HttpContext);
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

                var balance = _bankRepository.Withdraw(userId, dto.Amount);
                return Ok(new { Message = "Withdraw Successful", Balance = balance });
            }
            catch(Error e) { return NotFound(e.Message); }
        }

        [HttpPost("transfer"), Authorize]
        public IActionResult Transfer([FromBody] TransferDto dto)
        {
            try
            {
                var userId = _jwthelper.GetUserIdFromToken(HttpContext);
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

                var balance = _bankRepository.Transfer(userId, dto.Amount, dto.ReceiverAccountNumber);
                return Ok(new { Message = "Transfer Successful", Balance = balance });
            }
            catch(Error e) { return NotFound(e.Message); }
        }

        [HttpGet("transactions"), Authorize]
        public IActionResult GetTransactions()
        {
            try
            {
                var userId = _jwthelper.GetUserIdFromToken(HttpContext);
                if (string.IsNullOrEmpty(userId)) return Unauthorized("Invalid or missing token");

                var transactions = _bankRepository.GetHistory(userId);
                return transactions != null ? Ok(transactions) : NotFound("No transactions done by you");
            }
            catch(Error e) { return NotFound(e.Message); }
        }

        [HttpPost("logout"), Authorize]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            _jwthelper.RevokeToken(token);
            return Ok(new { Message = "Logout successful" });
        }
    }
}


