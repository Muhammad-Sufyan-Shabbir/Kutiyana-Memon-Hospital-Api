using AutoMapper;
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Helpers;
using Kutiyana_Memon_Hospital_Api.Http;
using Kutiyana_Memon_Hospital_Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            IUnitOfWork uow,
            IMapper mapper,
            IFunctionRepository functionRepository,
            IPasswordHasher<User> passwordHasher)
        {
            _uow = uow;
            _mapper = mapper;
            _functionRepository = functionRepository;
            _passwordHasher = passwordHasher;
        }

        // ✅ CREATE / UPDATE USER
        public async Task<ResponseModel<ApplicationUserResponse>> SaveUserAsync(ApplicationUserRequest request)
        {
            try
            {
                User user;

                // --- CREATE NEW USER ---
                if (request.GlobalId == Guid.Empty)
                {
                    // Duplicate email check
                    var emailExists = await _uow.userRepository
                        .GetQueryable()
                        .AnyAsync(u => u.Email == request.Email && !u.IsDeleted);

                    if (emailExists)
                    {
                        return new ResponseModel<ApplicationUserResponse>
                        {
                            Result = null,
                            Message = "Email already exists.",
                            HttpStatusCode = 400
                        };
                    }

                    // Duplicate username check
                    var usernameExists = await _uow.userRepository
                        .GetQueryable()
                        .AnyAsync(u => u.UserName == request.UserName && !u.IsDeleted);

                    if (usernameExists)
                    {
                        return new ResponseModel<ApplicationUserResponse>
                        {
                            Result = null,
                            Message = "Username already exists.",
                            HttpStatusCode = 400
                        };
                    }

                    // Map and set properties
                    user = _mapper.Map<User>(request);
                    user.GlobalId = Guid.NewGuid();

                    // Hash password using Identity
                    var (hash, salt) = PasswordHelper.HashPassword(request.Password);
                    user.PasswordHash = hash;
                    user.PasswordSalt = salt; 

                    await _uow.userRepository.AddAsync(user);
                    await _uow.SaveChangesAsync();

                    return new ResponseModel<ApplicationUserResponse>
                    {
                        Result = _mapper.Map<ApplicationUserResponse>(user),
                        Message = "User created successfully.",
                        HttpStatusCode = 201
                    };
                }

                // --- UPDATE EXISTING USER ---
                user = await _uow.userRepository
                    .GetQueryable()
                    .FirstOrDefaultAsync(u => u.GlobalId == request.GlobalId && !u.IsDeleted);

                if (user == null)
                {
                    return new ResponseModel<ApplicationUserResponse>
                    {
                        Result = null,
                        Message = "User not found with the given GlobalId.",
                        HttpStatusCode = 404
                    };
                }

                // Email conflict check
                if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
                {
                    var emailExists = await _uow.userRepository
                        .GetQueryable()
                        .AnyAsync(u => u.Email == request.Email && u.GlobalId != request.GlobalId && !u.IsDeleted);

                    if (emailExists)
                    {
                        return new ResponseModel<ApplicationUserResponse>
                        {
                            Result = null,
                            Message = "Email already exists for another user.",
                            HttpStatusCode = 400
                        };
                    }
                }

                // Update fields
                user.FirstName = request.FirstName ?? user.FirstName;
                user.LastName = request.LastName ?? user.LastName;
                user.UserName = request.UserName ?? user.UserName;
                user.Email = request.Email ?? user.Email;
                user.Phone = request.Phone ?? user.Phone;
                user.CompanyId = request.CompanyId;
                user.RoleId = request.RoleId;
                user.Department = request.Department ?? user.Department;
                user.Designation = request.Designation ?? user.Designation;
                user.Address = request.Address ?? user.Address;
                user.JoiningDate = request.JoiningDate ?? user.JoiningDate;
                user.IsActive = request.IsActive ?? user.IsActive;

                if (!string.IsNullOrWhiteSpace(request.Password))
                {
                    var (hash, salt) = PasswordHelper.HashPassword(request.Password);
                    user.PasswordHash = hash;
                    user.PasswordSalt = salt;
                }

                _uow.userRepository.Update(user);
                await _uow.SaveChangesAsync();

                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = _mapper.Map<ApplicationUserResponse>(user),
                    Message = "User updated successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = null,
                    Message = $"Error saving user: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<object>> GetUserByIdAsync(int userId)
        {
            try
            {
                var userDetail = await _functionRepository.GetUserByIdAsync(userId);

                return new ResponseModel<object>
                {
                    Result = userDetail,
                    Message = userDetail == null ? "User not found." : "User retrieved successfully.",
                    HttpStatusCode = userDetail == null ? 404 : 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Result = null,
                    Message = $"Error fetching role: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<object>> GetAllUsersAsync()
        {
            try
            {
                var users = await _functionRepository.GetAllUsersAsync();

                return new ResponseModel<object>
                {
                    Result = users,
                    Message = users == null ? "No users found." : "Users retrieved successfully.",
                    HttpStatusCode = users == null ? 404 : 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<object>
                {
                    Result = null,
                    Message = $"Error fetching users: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }


        public async Task<ResponseModel<bool>> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await _uow.userRepository.GetQueryable()
                    .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

                if (user == null)
                {
                    return new ResponseModel<bool>
                    {
                        Result = false,
                        Message = "User not found.",
                        HttpStatusCode = 404
                    };
                }

                // Soft delete
                user.IsDeleted = true;
                user.ModifiedOn = DateTime.UtcNow; 
                _uow.userRepository.Update(user);
                await _uow.SaveChangesAsync();

                return new ResponseModel<bool>
                {
                    Result = true,
                    Message = "User deleted successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Result = false,
                    Message = $"Error deleting user: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }



    }
}
