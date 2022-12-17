using Application.DTOs.LoanApplication.Request;
using Application.DTOs.User.Request;
using Application.Features.LoanApplicationsInfo.Commands;
using Application.Features.UsersInfo.Commands;
using Application.Features.UsersInfo.Queries;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<UserLogin, LoginCustomerCommand>().ReverseMap();
            CreateMap<UserLogin, GetUserInfoQuery>().ReverseMap();
            CreateMap<UserInfo, RegisterUserQuery>().ReverseMap();
            CreateMap<LoanApplicationRequest, InsertUserLoanApplicationCommand>().ReverseMap();
            CreateMap<LoanApplicationRequest, UpdateUserLoanApplicationCommand>().ReverseMap();
        }
    }
}
