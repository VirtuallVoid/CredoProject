using Application.DTOs.User.Request;
using Application.Helpers;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UsersInfo.Commands
{
    public class LoginCustomerCommand : IRequest<Response<string>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginCustomerCommandHandler : IRequestHandler<LoginCustomerCommand, Response<string>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IOptions<JWTSettings> _jwtSettings;

        public LoginCustomerCommandHandler(IUnitOfWork uow, IMapper mapper, IOptions<JWTSettings> JWTSettings)
        {
            _uow = uow;
            _jwtSettings = JWTSettings;
            _mapper = mapper;
        }
        public async Task<Response<string>> Handle(LoginCustomerCommand request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("LoginCustomerCommand", requestJson, DateTime.Now);
            try
            {
                var postObj = _mapper.Map<UserLogin>(request);
                var user = Authenticate(postObj);
                var id = await _uow.LoggingRepository.AddAsync(logObj);
                if (user != null)
                {
                    var token = Generate(user);
                    logObj.Id = id;
                    logObj.ResponseJson = JsonConvert.SerializeObject(token);
                    await _uow.LoggingRepository.UpdateAsync(logObj);
                    return new Response<string>(token, 200, "Success");
                }
                return new Response<string>(null, 404, "User Not Found");
            }
            catch (Exception ex)
            {
                logObj.ResponseJson = JsonConvert.SerializeObject(ex.Message);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                Helper.LogException(ex, requestJson, responseJson, "LoginCustomerCommand");
                return new Response<string>(null, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
        private Users Authenticate(UserLogin userLogin)
        {
            var currentUser = _uow.UserRepository.GetUserFullInfo(userLogin);
            if (currentUser != null)
            {
                return currentUser.Result;
            }

            return null;
        }

        private string Generate(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes($"{_jwtSettings.Value.Key}"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName),
                //new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken($"{_jwtSettings.Value.Issuer}",
              $"{_jwtSettings.Value.Audience}",
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
