using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfile.DAL.Models;
using UserProfile.DAL.Models.Dtos;

namespace UserProfile.BAL.Repository.Contract
{
    public interface IAccountRepository
    {
       public Task<ResponseModel> SignUp(RegisterDto regObj,string role);
       public Task<ResponseModel> Login(LoginDto loginObj);
       public Task<ResponseModel> AddAddressForUsers(UserDto user);
       public Task<ResponseModel> GetUserProfileWithAddress(int Uid);
    }
}
