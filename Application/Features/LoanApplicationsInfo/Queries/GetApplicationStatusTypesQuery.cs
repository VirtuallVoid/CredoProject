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

namespace Application.Features.StatusApplicationsInfo.Queries
{
    public class GetApplicationStatusTypesQuery : IRequest<Response<List<StatusTypes>>>
    {
    }
    public class GetApplicationStatusTypesQueryHandler : IRequestHandler<GetApplicationStatusTypesQuery, Response<List<StatusTypes>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetApplicationStatusTypesQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<Response<List<StatusTypes>>> Handle(GetApplicationStatusTypesQuery request, CancellationToken cancellationToken)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = string.Empty;
            var logObj = new LogTable("GetApplicationStatusTypesQuery", requestJson, DateTime.Now);
            try
            {
                var StatusTypes = await _uow.LoanApplicationsRepository.GetApplicationStatusTypesAsync();
                return new Response<List<StatusTypes>>(StatusTypes.ToList(), 200, "Success");
            }
            catch (Exception ex)
            {
                logObj.ResponseJson = JsonConvert.SerializeObject(ex.Message);
                await _uow.LoggingRepository.UpdateAsync(logObj);
                Helper.LogException(ex, requestJson, responseJson, "GetApplicationStatusTypesQuery");
                return new Response<List<StatusTypes>>(null, 400, "მოხდა განუსაზღვრელი შეცდომა");
            }
        }
    }
}
