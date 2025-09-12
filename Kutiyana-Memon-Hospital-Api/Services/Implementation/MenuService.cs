using AutoMapper;
using BCrypt.Net;
using Kutiyana_Memon_Hospital_Api.API.Entities;
using Kutiyana_Memon_Hospital_Api.API.Services.Interfaces;
using Kutiyana_Memon_Hospital_Api.API.UnitOfWork.Interfaces;
using Kutiyana_Memon_Hospital_Api.DTOs.Request;
using Kutiyana_Memon_Hospital_Api.DTOs.Response;
using Kutiyana_Memon_Hospital_Api.Http;

namespace Kutiyana_Memon_Hospital_Api.API.Services.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public MenuService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ResponseModel<MenuResponse>> SaveMenuAsync(MenuRequest request)
        {
            try
            {
                Module menu;

                // ✅ Agar GlobalId empty ho → create mode
                if (request.GlobalId == Guid.Empty)
                {
                    menu = _mapper.Map<Module>(request);
                    menu.GlobalId = Guid.NewGuid();

                    // ✅ Ensure top-level module ParentId is null
                    menu.ParentId = request.ParentId == 0 ? null : request.ParentId;

                    await _uow.menuRepository.AddAsync(menu);
                    await _uow.SaveChangesAsync();

                    return new ResponseModel<MenuResponse>
                    {
                        Result = _mapper.Map<MenuResponse>(menu),
                        Message = "Menu created successfully.",
                        HttpStatusCode = 201
                    };
                }

                // ✅ Agar GlobalId mila hai → update mode
                menu = await _uow.menuRepository
                    .FirstOrDefaultAsync(m => m.GlobalId == request.GlobalId);

                if (menu == null)
                {
                    return new ResponseModel<MenuResponse>
                    {
                        Result = null,
                        Message = "Menu not found with the given GlobalId.",
                        HttpStatusCode = 404
                    };
                }

                // 🔹 Update fields
                menu.Name = request.Name ?? menu.Name;
                menu.ParentId = (request.ParentId == 0) ? null : request.ParentId;
                menu.Url = request.Url ?? menu.Url;
                menu.Icon = request.Icon ?? menu.Icon;
                menu.OrderNo = request.OrderNo != 0 ? request.OrderNo : menu.OrderNo;
                menu.IsActive = request.IsActive;

                _uow.menuRepository.Update(menu);
                await _uow.SaveChangesAsync();

                return new ResponseModel<MenuResponse>
                {
                    Result = _mapper.Map<MenuResponse>(menu),
                    Message = "Menu updated successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<MenuResponse>
                {
                    Result = null,
                    Message = $"Error saving menu: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }


        public async Task<ResponseModel<MenuResponse>> GetMenuByIdAsync(int id)
        {
            var menu = await _uow.menuRepository
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (menu == null)
            {
                return new ResponseModel<MenuResponse>
                {
                    Result = null,
                    Message = "Menu not found with the given Id.",
                    HttpStatusCode = 404
                };
            }

            return new ResponseModel<MenuResponse>
            {
                Result = _mapper.Map<MenuResponse>(menu),
                Message = "Menu fetched successfully.",
                HttpStatusCode = 200
            };
        }

        public async Task<ResponseModel<List<MenuResponse>>> GetAllMenusAsync()
        {
            var menus = await _uow.menuRepository.GetAllAsync();

            // Sirf active & not deleted
            var activeMenus = menus
                .Where(m => !m.IsDeleted && m.IsActive)
                .OrderBy(m => m.OrderNo)
                .ToList();

            if (!activeMenus.Any())
            {
                return new ResponseModel<List<MenuResponse>>
                {
                    Result = null,
                    Message = "No menus found.",
                    HttpStatusCode = 404
                };
            }

            // ✅ Sirf root menus (ParentId == null) bhejna
            var rootMenus = activeMenus
                .Where(m => m.ParentId == null)
                .ToList();

            return new ResponseModel<List<MenuResponse>>
            {
                Result = _mapper.Map<List<MenuResponse>>(rootMenus),
                Message = "Menus fetched successfully.",
                HttpStatusCode = 200
            };
        }


        public async Task<ResponseModel<bool>> DeleteMenuAsync(int id)
        {
            try
            {
                var menu = await _uow.menuRepository.GetByIdAsync(id);

                if (menu == null || menu.IsDeleted)
                {
                    return new ResponseModel<bool>
                    {
                        Result = false,
                        Message = "Menu not found with the given Id.",
                        HttpStatusCode = 404
                    };
                }

                // Soft delete + inactive
                menu.IsDeleted = true;
                menu.IsActive = false; // 👈 yahan inactive bhi kar diya

                _uow.menuRepository.Update(menu);
                await _uow.SaveChangesAsync();

                return new ResponseModel<bool>
                {
                    Result = true,
                    Message = "Menu deleted successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>
                {
                    Result = false,
                    Message = $"Error deleting menu: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }



    }
}
