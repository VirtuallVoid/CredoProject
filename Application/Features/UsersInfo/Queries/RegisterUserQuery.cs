using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Threading;
using Application.Wrappers;
using Application.DTOs.User.Request;

namespace Application.Features.UsersInfo.Queries
{
    public class RegisterUserQuery : IRequest<Response<int>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PersonalNumber { get; set; }
        public DateTime BirthDate { get; set; }
    }
    public class RegisterUserQueryHandler : IRequestHandler<RegisterUserQuery, Response<int>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RegisterUserQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(RegisterUserQuery request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("RegisterUserQuery", requestJson, DateTime.Now);
            try
            {
                var logId = await _uow.LoggingRepository.AddAsync(logObj);
                var userInfo = _mapper.Map<UserInfo>(request);
                int id = await _uow.UserRepository.InsertUserInfo(userInfo);
                logObj.Id = logId;
                logObj.ResponseJson = JsonConvert.SerializeObject(id);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                return id != 0
                    ? new Response<int>(id, 200, "Inserted Successfuly")
                    : new Response<int>(id, 400, "Insert Operation Failed");
            }
            catch (Exception ex)
            {
                logObj.ResponseJson = JsonConvert.SerializeObject(ex.Message);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                Helper.LogException(ex, requestJson, responseJson, "RegisterUserQuery");
                return new Response<int>(0, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
    }
}
