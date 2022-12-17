using Application.DTOs.User.Request;
using Application.Features.UsersInfo.Commands;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<Users>
    {
        Task<Users> GetUserFullInfo(UserLogin userLogin);
        Task<int> InsertUserInfo(UserInfo UserInfo);

    }
}
