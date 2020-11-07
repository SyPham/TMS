using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Data.Models;
using Data.ViewModel.Line;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Service.Dto;
using Service.Helpers;
using Service.Interface;


namespace TMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IOCService _oCService;
        private readonly ILineService _lineService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService,
            IConfiguration configuration,
            IOCService oCService,
            ILineService lineService,
            IMapper mapper)
        {
            _authService = authService;
            _configuration = configuration;
            _oCService = oCService;
            _mapper = mapper;
            _lineService = lineService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _authService.FindByNameAsync(userForRegisterDto.Username) != null)
                return BadRequest("Username already exists");

            var user = _mapper.Map<User>(userForRegisterDto);
            var createdUser = await _authService.Register(user, userForRegisterDto.Password);
            return CreatedAtRoute("GetUser", new { controller = "User", id = createdUser.ID }, userForRegisterDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        
        {
            var user = await _authService.FindByNameAsync(userForLoginDto.Username);
            if (user == null)
                return NotFound();

            var result = await _authService
                .Login(userForLoginDto.Username, userForLoginDto.Password);
            if (result == null)
                return NotFound();
            var subscribeLine = new bool();
            if (!user.AccessTokenLineNotify.IsNullOrEmpty())
            {
               // await _lineService.SendWithSticker(new MessageParams {Message = $"Hi {user.Username}! Welcome to Task Management System!", Token = user.AccessTokenLineNotify, StickerPackageId = "2", StickerId = "41" });
                subscribeLine = true;
            }
            var userprofile = new UserProfileDto()
            {
                User = new UserForReturnLogin
                {
                    Username = user.Username,
                    Role = user.RoleID,
                    ID = user.ID,
                    OCLevel = user.LevelOC,
                    ListOCs = await _oCService.ListOCIDofUser(user.OCID),
                    IsLeader = user.isLeader,
                    image = user.ImageBase64,
                    SubscribeLine = subscribeLine,
                    Systems = user.UserSystems.Where(x=>x.Status == true).Select(x=>x.SystemID).ToList()
                },
                //Menus = JsonConvert.SerializeObject(await _authService.GetMenusAsync(user.Role))
            };
            var token = GenerateJwtToken(result);
            if (userForLoginDto.SystemCode == 0)
            {
                return Ok(new
                {
                    token,
                    user = userprofile
                });
            }
            await this._authService.SignInHistory(new SignInHistory
            {
                Token = token,
                UserID = user.ID,
                Username = user.Username,
                SystemCode = userForLoginDto.SystemCode,
                Host = $"{Request.HttpContext.Connection.RemoteIpAddress.ToString()}:{Request.HttpContext.Connection.RemotePort.ToString()}",
                LoginTime = DateTime.Now,
                CreatedDate = DateTime.Now
            });
            return Ok(new
            {
                token,
                user = userprofile
            });

        }
        
        [HttpGet("GetSignInHistory")]
        public async Task<IActionResult> GetSignInHistory()
        {
            var res = await this._authService.GetSignInHistory();
            return Ok(res);

        }
        [HttpPut("SignOut")]
        public async Task<IActionResult> SignOut([FromBody] SignInHistory signInHistory)
        {
            if (signInHistory.SystemCode == 0)
            {
                return Ok(true);
            }
            var res = await this._authService.SignInHistory(signInHistory);
            return Ok(res);

        }
        private int checkRole(int role, int level)
        {
            if (role == 1) return 1;
            else if (role != 1 && level >= 1 && level <= 2) return 2;
            else return 3;
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] UserForLoginDto userForLoginDto)
        {
            return Ok(await _authService.Edit(userForLoginDto.Username));

        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = _authService.Authenticate(model, ipAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _authService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = _authService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        public enum ClaimTypeEnum
        {
            OCID,
            Role
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypeEnum.OCID.ToString(), user.OCID.ToSafetyString(),ClaimTypeEnum.OCID.ToString()),
                new Claim(ClaimTypeEnum.Role.ToString(), user.RoleID.ToSafetyString(),ClaimTypeEnum.Role.ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}