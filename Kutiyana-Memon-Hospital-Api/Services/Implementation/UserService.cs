using AutoMapper;
using BCrypt.Net;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        //public async Task<IEnumerable<UserDto>> GetAllAsync()
        //{
        //    var users = await _uow.Users.GetAllAsync();
        //    return users.Select(u => new UserDto
        //    {
        //        Id = u.Id,
        //        GlobalId = u.GlobalId,
        //        FirstName = u.FirstName,
        //        LastName = u.LastName,
        //        UserName = u.UserName,
        //        Email = u.Email,
        //        RoleId = u.RoleId,
        //        ImageUrl = u.ImageUrl,
        //    });
        //}

        //public async Task<UserDto> GetByIdAsync(int id)
        //{
        //    var user = await _uow.Users.GetByIdAsync(id);
        //    return user == null ? null : new UserDto
        //    {
        //        Id = user.Id,
        //        GlobalId = user.GlobalId,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        UserName = user.UserName,
        //        ImageUrl = user.ImageUrl,
        //        Email = user.Email,
        //        RoleId = user.RoleId
        //    };
        //}

        //public async Task<UserDto> GetByGlobalIdAsync(Guid globalId)
        //{
        //    var user = await _uow.Users.GetByGlobalIdAsync(globalId);
        //    return user == null ? null : new UserDto
        //    {
        //        Id = user.Id,
        //        GlobalId = user.GlobalId,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        UserName = user.UserName,
        //        ImageUrl = user.ImageUrl,
        //        Email = user.Email,
        //        RoleId = user.RoleId,
        //        CreatedBy = user.CreatedBy,
        //        ModifiedBy = user.ModifiedBy
        //    };
        //}

        //public async Task<UserDto> CreateOrUpdateAsync(CreateUserDto dto)
        //{
        //    string? imagePath = null;

        //    if (dto.Image != null && dto.Image.Length > 0)
        //    {
        //        var folderPath = Path.Combine("wwwroot", "uploads", "users");
        //        Directory.CreateDirectory(folderPath);
        //        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
        //        var filePath = Path.Combine(folderPath, fileName);

        //        using var stream = new FileStream(filePath, FileMode.Create);
        //        await dto.Image.CopyToAsync(stream);

        //        imagePath = $"/uploads/users/{fileName}";
        //    }

        //    if (!dto.GlobalId.HasValue || dto.GlobalId == Guid.Empty)
        //    {
        //        var newUser = new User
        //        {
        //            FirstName = dto.FirstName,
        //            LastName = dto.LastName,
        //            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        //            Email = dto.Email,            
        //            UserName = dto.UserName,
        //            RoleId = dto.RoleId,
        //            ImageUrl = imagePath
        //        };

        //        await _uow.Users.AddAsync(newUser);
        //        await _uow.SaveChangesAsync();

        //        return new UserDto
        //        {
        //            Id = newUser.Id,
        //            GlobalId = newUser.GlobalId,
        //            FirstName = newUser.FirstName,
        //            LastName = newUser.LastName,
        //            Email = newUser.Email,
        //            UserName = newUser.UserName,
        //            RoleId = newUser.RoleId,
        //            ImageUrl = newUser.ImageUrl
        //        };
        //    }
        //    else
        //    {
        //        var existingUser = await _uow.Users.GetByGlobalIdAsync(dto.GlobalId.Value);
        //        if (existingUser == null)
        //            throw new Exception("User not found");

        //        existingUser.FirstName = dto.FirstName;
        //        existingUser.LastName = dto.LastName;
        //        existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        //        existingUser.Email = dto.Email;             
        //        existingUser.UserName = dto.UserName;
        //        existingUser.RoleId = dto.RoleId;

        //        if (imagePath != null)
        //            existingUser.ImageUrl = imagePath;

        //        _uow.Users.Update(existingUser);
        //        await _uow.SaveChangesAsync();

        //        return new UserDto
        //        {
        //            Id = existingUser.Id,
        //            GlobalId = existingUser.GlobalId,
        //            FirstName = existingUser.FirstName,
        //            LastName = existingUser.LastName,
        //            Email = existingUser.Email,
        //            UserName = existingUser.UserName,
        //            RoleId = existingUser.RoleId,
        //            ImageUrl = existingUser.ImageUrl
        //        };
        //    }
        //}

        //public async Task DeleteAsync(int id)
        //{
        //    var user = await _uow.Users.GetByIdAsync(id);
        //    if (user != null)
        //    {
        //        _uow.Users.Delete(user);
        //        await _uow.SaveChangesAsync();
        //    }
        //}
    }
}
