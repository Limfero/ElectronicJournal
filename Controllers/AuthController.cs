using ElectronicJournal.Domain.Helpers;
using ElectronicJournal.Domain.Response;
using ElectronicJournal.Domain.ViewModels;
using ElectronicJournal.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ElectronicJournal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController
    {
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;

        public AuthController(IStudentService studentService, ITeacherService teacherService)
        {
            _studentService = studentService;
            _teacherService = teacherService;
        }

        [HttpPost]
        [Route("login")]
        public IResult Login([FromBody] AuthResponse response)
        {
            var teacher = _teacherService.GetAllTeachers().Data.FirstOrDefault(teacher => teacher.Login == response.UserName);
            var student = _studentService.GetAllStudent().Data.FirstOrDefault(teacher => teacher.Login == response.UserName);

            var user = response.UserName[0] == 'T' ? new UserViewModel { Name = teacher.Login, Id = teacher.Id.ToString(), Role = (int)teacher.Role } : new UserViewModel { Name = student.Login, Id = student.Id.ToString(), Role = (int)Domain.Enum.Role.Student };

            if (user == null) 
                return Results.Unauthorized();

            var claims = new List<Claim>
            {
                new("userName", user.Name),
                new("userId", user.Id),
                new("userRole", user.Role.ToString()),
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var jwtResponse = new
            {
                userName = user.Name,
                token = encodedJwt,
                userId = user.Id,
                userRole = user.Role,
            };

            return Results.Json(jwtResponse);
        }
    }
}
