using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserProfile.BAL.Repository.Contract;
using UserProfile.DAL.Models;
using UserProfile.DAL.Models.Dtos;
using Microsoft.Identity.Client;
using System.Data;
using System.Net;
using System.Diagnostics.Metrics;

namespace UserProfile.BAL.Repository.Services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _config;
        private readonly UserProfileDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountRepository(UserProfileDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _roleManager = roleManager;
        }
        #region For SignUp
        public async Task<ResponseModel> SignUp([FromBody] RegisterDto regObj, string role)
        {
            ResponseModel responseModel = new ResponseModel();
            if (await _userManager.Users.AnyAsync(x => x.Email == regObj.UserName) || await _userManager.Users.AnyAsync(x => x.UserName == regObj.UserName))
            {
                responseModel.Message = regObj.UserName + "already Exist!!";
                responseModel.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return responseModel;
            }
            // Add the User in the Database
            var user = new IdentityUser
            {
                UserName = regObj.UserName,
                Email = regObj.Email,
                PasswordHash = regObj.Password,
                PhoneNumber = regObj.MobileNumber,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            // Check Role Exist or Not
            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, regObj.Password);
                if (result.Succeeded)
                {
                    // Add role to the user
                    await _userManager.FindByEmailAsync(regObj.Email);
                    await _userManager.AddToRoleAsync(user, role);
                    responseModel.Message = "User Created SuccessFully!!";
                    responseModel.StatusCode = System.Net.HttpStatusCode.Created;
                    responseModel.Data = user;
                }
                else
                {
                    responseModel.Message = "User Created Failed!!";
                    responseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    responseModel.Data = user;
                }
            }
            else
            {
                responseModel.Message = "This Role doesn't Exist!!";
                responseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                responseModel.Data = role;
            }
            return responseModel;
        }
        #endregion

        #region For Login Purpose
        public async Task<ResponseModel> Login([FromBody] LoginDto loginObj)
        {
            ResponseModel responseModel = new ResponseModel();
            var user = await _userManager.Users.AnyAsync(x => x.UserName == loginObj.UserName);
            if (user != true)
            {
                responseModel.Message = "User is Not Found!! Token is Not Created!!";
                responseModel.StatusCode = System.Net.HttpStatusCode.BadRequest;
            }
                var result = await _signInManager.PasswordSignInAsync(loginObj.UserName,loginObj.Password,true,false);
                if(result.Succeeded)
                {
                var actualUser = await _userManager.FindByNameAsync(loginObj.UserName);
                var isinRole = await _userManager.GetRolesAsync(actualUser);
                if (isinRole != null)
                {
                    var token = GenerateToken(loginObj,isinRole);
                    responseModel.Message = "Token is Created!!";
                    responseModel.StatusCode = System.Net.HttpStatusCode.Created;
                    responseModel.Data = token;
                }
            }
            return responseModel;
        }
        #endregion

        #region To Generate Token
        public string GenerateToken(LoginDto user,IList<string> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //#endregion
        public static List<UserAddress> ListofUserAddress(List<UserAddressDto> userAddressDtos)
        {
            List<UserAddress> ListuserAddresses = new List<UserAddress>();
            if (userAddressDtos.Count != 0)
            {
                for (int i = 0; i < userAddressDtos.Count(); i++)
                {
                    UserAddress useraddress = new UserAddress()
                    {
                        Address = userAddressDtos[i].Address,
                        City = userAddressDtos[i].City,
                        State = userAddressDtos[i].State,
                        Country = userAddressDtos[i].Country,
                        Pin = userAddressDtos[i].Pin,
                        Uid = userAddressDtos[i].Uid,
                    };
                    ListuserAddresses.Add(useraddress);
                }
            }
            return ListuserAddresses;
        }
        #endregion

        #region Save User and Its Address
        public async Task<ResponseModel> AddAddressForUsers([FromBody] UserDto user)
        {
            ResponseModel response = new ResponseModel();
            if (user != null)
            {
                User u = new User()
                {
                    Name = user.Name,
                    Email = user.Email,
                    MobileNo = user.MobileNo,
                    Gender = user.Gender,
                    DOB = user.DOB.ToString("MM/dd/yyyy"),
                    Qualification = user.Qualification,
                    Addresses = ListofUserAddress(user.Addresses),
                };
                _db.Add(u);
                int n=await _db.SaveChangesAsync();
                response.StatusCode = HttpStatusCode.Created;
                response.Message = "User Created SuccessFully!!";
                response.Data = u;
            }
            return response;
        }
        #endregion

        #region Get User Profile With Addresses
        public async Task<ResponseModel> GetUserProfileWithAddress([FromBody] int Uid)
        {
            ResponseModel response = new ResponseModel();
            if (Uid != 0)
            {
                var query = await (from user in _db.Users
                                   join uad in _db.UserAddresses on user.Uid equals uad.Uid
                                   select new
                                   {
                                       Uid = uad.Uid,
                                       Address = uad.Address,
                                       city = uad.City,
                                       state = uad.State,
                                       country = uad.Country,
                                       pin = uad.Pin
                                   }).ToListAsync();
                var useraddress = query.ToList().Where(f => f.Uid == Uid);
                if (useraddress != null && useraddress.Count()!=0)
                {
                    response.StatusCode = HttpStatusCode.Found;
                    response.Message = "User Address Found!!";
                    response.Data = useraddress;
                }
            }
            return response;
        }
        #endregion
    }
}
