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
using System.Linq;

namespace Application.Features.LoanApplicationsInfo.Queries
{
    public class GetApplicationLoanTypesQuery : IRequest<Response<List<LoanTypes>>>
    {
    }
    public class GetApplicationLoanTypesQueryHandler : IRequestHandler<GetApplicationLoanTypesQuery, Response<List<LoanTypes>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetApplicationLoanTypesQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response<List<LoanTypes>>> Handle(GetApplicationLoanTypesQuery request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("GetApplicationLoanTypesQuery", requestJson, DateTime.Now);
            try
            {
                var loanTypes = await _uow.LoanApplicationsRepository.GetApplicationLoanTypesAsync();
                return new Response<List<LoanTypes>>(loanTypes.ToList(), 200, "Success");
            }
            catch (Exception ex)
            {
                logObj.ResponseJson = JsonConvert.SerializeObject(ex.Message);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                Helper.LogException(ex, requestJson, responseJson, "GetApplicationLoanTypesQuery");
                return new Response<List<LoanTypes>>(null, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
    }
}
