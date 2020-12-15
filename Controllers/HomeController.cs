using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public IConfiguration _configuration;

        public HomeController(
            IConfiguration config, 
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            _configuration = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {

            //Login Functionality
            var user = await _userManager.FindByNameAsync(username);

            if(user != null)
            {
                // sign in
                var SignInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (SignInResult.Succeeded)
                {

                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("UserName", user.UserName),
                    new Claim("Email", user.Email)};

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                    return RedirectToAction("index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }

            return RedirectToAction("LoginFailed");
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult LoginFailed()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            //var useId = new Guid.NewGuid().ToString(username);
            //Register Functionality
            var user = new IdentityUser
            {
                UserName = username,
                Email = "",
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                //generation of email token

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Home", new {userId = user.Id, code}, Request.Scheme, Request.Host.ToString());

                await _emailService.SendAsync("test@test.com", "email verify", $"<a href=\"{link}\">Verify Email</a>", true);

                return RedirectToAction("EmailVerification");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {

                return View();
            }
            return BadRequest();
            
        }
        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
