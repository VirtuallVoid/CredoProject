using Application.DTOs.User.Request;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Application.Wrappers;
using System.Linq;

namespace Application.Features.LoanApplicationsInfo.Queries
{
    public class GetUserLoanApplicationsQuery : IRequest<Response<List<LoanApplications>>>
    {
        public int UserId{ get; set; }
    }
    public class GetUserLoanApplicationsQueryHandler : IRequestHandler<GetUserLoanApplicationsQuery, Response<List<LoanApplications>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUserLoanApplicationsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response<List<LoanApplications>>> Handle(GetUserLoanApplicationsQuery request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("GetUserLoanApplicationsQuery", requestJson, DateTime.Now);
            try
            {
                var id = await _uow.LoggingRepository.AddAsync(logObj);
                var loan = await _uow.LoanApplicationsRepository.GetUserLoanApplicationsAsync(request.UserId);
                var result = loan.ToList<LoanApplications>();
                logObj.Id = id;
                logObj.ResponseJson = JsonConvert.SerializeObject(result);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                return result.Count != 0 
                    ? new Response<List<LoanApplications>>(result, 200, "Success") 
                    : new Response<List<LoanApplications>>(null, 404, "User Does Not Have Any Application");
            }
            catch (Exception ex)
            {
                logObj.ResponseJson = JsonConvert.SerializeObject(ex.Message);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                Helper.LogException(ex, requestJson, responseJson, "GetUserLoanApplicationsQuery");
                return new Response<List<LoanApplications>>(null, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
    }
}
