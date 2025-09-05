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
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CompanyService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        } 

        public async Task<ResponseModel<CompanyResponse>> SaveCompanyAsync(CompanyRequest request)
        {
            try
            {
                Company company;

                if (request.GlobalId == Guid.Empty)
                {
                    company = _mapper.Map<Company>(request);
                    company.GlobalId = Guid.NewGuid();

                    await _uow.companyRepository.AddAsync(company);
                    await _uow.SaveChangesAsync();

                    return new ResponseModel<CompanyResponse>
                    {
                        Result = _mapper.Map<CompanyResponse>(company),
                        Message = "Company created successfully.",
                        HttpStatusCode = 201
                    };
                }

                company = await _uow.companyRepository
                    .FirstOrDefaultAsync(c => c.GlobalId == request.GlobalId);

                if (company == null)
                    return new ResponseModel<CompanyResponse>
                    {
                        Result = null,
                        Message = "Company not found with the given GlobalId.",
                        HttpStatusCode = 404
                    };

                company.Name = request.Name ?? company.Name;
                company.Address = request.Address ?? company.Address;
                company.City = request.City ?? company.City;
                company.ContactInfo = request.ContactInfo ?? company.ContactInfo;
                company.IsActive = request.IsActive;

                _uow.companyRepository.Update(company);
                await _uow.SaveChangesAsync();

                return new ResponseModel<CompanyResponse>
                {
                    Result = _mapper.Map<CompanyResponse>(company),
                    Message = "Company updated successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<CompanyResponse>
                {
                    Result = null,
                    Message = $"Error saving company: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<CompanyResponse>> GetCompanyByIdAsync(int id)
        {
            var company = await _uow.companyRepository.GetByIdAsync(id);

            if (company == null)
            {
                return new ResponseModel<CompanyResponse>
                {
                    Result = null,
                    Message = "Company not found with the given Id.",
                    HttpStatusCode = 404
                };
            }

            return new ResponseModel<CompanyResponse>
            {
                Result = _mapper.Map<CompanyResponse>(company),
                Message = "Company fetched successfully.",
                HttpStatusCode = 200
            };
        }

        public async Task<ResponseModel<bool>> DeleteCompanyAsync(int id)
        {
            try
            {
                var company = await _uow.companyRepository.GetByIdAsync(id);

                if (company == null)
                    return new ResponseModel<bool> { Result = false, Message = "Company not found.", HttpStatusCode = 404 };

                _uow.companyRepository.Delete(company);
                await _uow.SaveChangesAsync();

                return new ResponseModel<bool> { Result = true, Message = "Company deleted successfully.", HttpStatusCode = 200 };
            }
            catch (Exception ex)
            {
                return new ResponseModel<bool> { Result = false, Message = $"Error deleting company: {ex.Message}", HttpStatusCode = 500 };
            }
        }

        public async Task<ResponseModel<IEnumerable<CompanyResponse>>> GetAllCompaniesAsync()
        {
            try
            {
                var companies = await _uow.companyRepository.GetAllAsync();
                return new ResponseModel<IEnumerable<CompanyResponse>>
                {
                    Result = _mapper.Map<IEnumerable<CompanyResponse>>(companies),
                    Message = "Companies retrieved successfully.",
                    HttpStatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel<IEnumerable<CompanyResponse>>
                {
                    Result = null,
                    Message = $"Error retrieving companies: {ex.Message}",
                    HttpStatusCode = 500
                };
            }
        }
    }
}
