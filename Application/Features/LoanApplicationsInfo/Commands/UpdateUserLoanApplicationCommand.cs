using Application.DTOs.LoanApplication.Request;
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

namespace Application.Features.LoanApplicationsInfo.Commands
{
    public class UpdateUserLoanApplicationCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int LoanTypeId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime LoanPeriod { get; set; }
        public int StatusId { get; set; }
    }
    public class UpdateUserLoanApplicationCommandHandler : IRequestHandler<UpdateUserLoanApplicationCommand, Response<int>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateUserLoanApplicationCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(UpdateUserLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("UpdateUserLoanApplicationCommand", requestJson, DateTime.Now);
            try
            {
                var id = await _uow.LoggingRepository.AddAsync(logObj);
                var postObj = _mapper.Map<LoanApplicationRequest>(request);
                var loanId = await _uow.LoanApplicationsRepository.UpdateUserLoanApplicationAsync(postObj);
                logObj.Id = id;
                logObj.ResponseJson = JsonConvert.SerializeObject(loanId);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                return loanId != 0
                    ? new Response<int>(loanId, 200, "Success")
                    : new Response<int>(loanId, 400, "Failure");
            }
            catch (Exception ex)
            {
                logObj.ResponseJson = JsonConvert.SerializeObject(ex.Message);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                Helper.LogException(ex, requestJson, responseJson, "UpdateUserLoanApplicationCommand");
                return new Response<int>(0, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
    }
}
