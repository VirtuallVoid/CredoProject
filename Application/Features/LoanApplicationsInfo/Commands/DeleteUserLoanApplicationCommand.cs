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
    public class DeleteUserLoanApplicationCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
    public class DeleteUserLoanApplicationCommandHandler : IRequestHandler<DeleteUserLoanApplicationCommand, Response<int>>
    {
        private readonly IUnitOfWork _uow;

        public DeleteUserLoanApplicationCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<Response<int>> Handle(DeleteUserLoanApplicationCommand request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("DeleteUserLoanApplicationCommand", requestJson, DateTime.Now);
            try
            {
                var id = await _uow.LoggingRepository.AddAsync(logObj);
                var loanId = await _uow.LoanApplicationsRepository.DeleteUserLoanApplicationAsync(request.UserId, request.Id);
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
                Helper.LogException(ex, requestJson, responseJson, "DeleteUserLoanApplicationCommand");
                return new Response<int>(0, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
    }
}
