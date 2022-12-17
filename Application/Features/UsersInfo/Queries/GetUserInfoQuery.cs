using Application.DTOs.User.Request;
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

namespace Application.Features.UsersInfo.Queries
{
    public class GetUserInfoQuery : IRequest<Response<Users>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, Response<Users>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUserInfoQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response<Users>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("GetUserInfoQuery", requestJson, DateTime.Now);
            try
            {
                var postObj = _mapper.Map<UserLogin>(request);
                var user = await _uow.UserRepository.GetUserFullInfo(postObj);
                var id = await _uow.LoggingRepository.AddAsync(logObj);
                logObj.Id = id;
                logObj.ResponseJson = JsonConvert.SerializeObject(user);
                await _uow.LoggingRepository.UpdateAsync(logObj);

                return user != null 
                    ? new Response<Users>(user, 200, "Success") 
                    : new Response<Users>(null, 404, "User Not Found");
            }
            catch (Exception ex)
            {
                logObj.ResponseJson = JsonConvert.SerializeObject(ex.Message);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                Helper.LogException(ex, requestJson, responseJson, "GetUserInfoQuery");
                return new Response<Users>(null, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
    }
}
