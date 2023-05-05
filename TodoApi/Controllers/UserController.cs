using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TodoApi.Data;
using TodoApi.Data.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApi.Models;
using AutoMapper;
using TodoApi.Dtos;

namespace TodoApi.Controllers
{
  [Route("api/user")]
  [ApiController]
  public class UserContoller : ControllerBase
  {
    private readonly IUserRepository repository;
    private readonly IOptions<AppSettings> appSettings;
    private readonly IMapper mapper;
    public UserContoller(IUserRepository repository, IOptions<AppSettings> appSettings, IMapper mapper)
    {
      this.repository = repository;
      this.appSettings = appSettings;
      this.mapper = mapper;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult authenticate([FromBody] AuthenticatieModel model)
    {
      var user = repository.login(model.username, model.password);

      if (user == null)
      {
        return BadRequest(new { message = "username or password is incorrect" });
      }

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(appSettings.Value.secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.role)
                }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);
      return Ok(new
      {
        id = user.id,
        username = user.username,
        firstName = user.firstName,
        lastName = user.lastName,
        email = user.email,
        role = user.role,
        Token = tokenString
      });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult register([FromForm] RegisterDto model)
    {
      var user = mapper.Map<User>(model);
      try
      {
        repository.register(user, model.password);
        return Ok(new
        {
          id = user.id,
          username = user.username,
          firstName = user.firstName,
          lastName = user.lastName,
          email = user.email,
          role = user.role
        });
      }
      catch (AppException ex)
      {
        return BadRequest(new
        {
          message = ex.Message
        });
      }
    }
  }
}