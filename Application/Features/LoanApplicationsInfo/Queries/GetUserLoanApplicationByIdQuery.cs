using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Application.Wrappers;
using Application.DTOs.LoanApplication.Response;

namespace Application.Features.LoanApplicationsInfo.Queries
{
    public class GetUserLoanApplicationByIdQuery : IRequest<Response<LoanApplicationResponse>>
    {
        public int UserId { get; set; }
        public int LoanId { get; set; }
    }
    public class GetUserLoanApplicationByIdQueryHandler : IRequestHandler<GetUserLoanApplicationByIdQuery, Response<LoanApplicationResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUserLoanApplicationByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response<LoanApplicationResponse>> Handle(GetUserLoanApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("GetUserLoanApplicationByIdQuery", requestJson, DateTime.Now);
            try
            {
                var id = await _uow.LoggingRepository.AddAsync(logObj);
                var loan = await _uow.LoanApplicationsRepository.GetUserLoanApplicationByIdAsync(request.UserId, request.LoanId);
                logObj.Id = id;
                logObj.ResponseJson = JsonConvert.SerializeObject(loan);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                return loan != null
                    ? new Response<LoanApplicationResponse>(loan, 200, "Success")
                    : new Response<LoanApplicationResponse>(null, 404, "Application Not Found");
            }
            catch (Exception ex)
            {
                logObj.ResponseJson = JsonConvert.SerializeObject(ex.Message);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                Helper.LogException(ex, requestJson, responseJson, "GetUserLoanApplicationByIdQuery");
                return new Response<LoanApplicationResponse>(null, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
    }
}
