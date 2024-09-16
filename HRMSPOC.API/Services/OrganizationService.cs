using HRMSPOC.API.Models;
using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services.Interface;

namespace HRMSPOC.API.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }
        public async Task CreateEmployeeAsync(Organization organization)
        {
            await _organizationRepository.AddAsync(organization);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _organizationRepository.DeleteAsync(id);
        }

        public async Task<Organization> GetEmployeeByIdAsync(int id)
        {
            return await _organizationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Organization>> GetEmployeesAsync()
        {
            return await _organizationRepository.GetAllAsync();
        }

        public async Task UpdateEmployeeAsync(Organization organization)
        {
            await _organizationRepository.UpdateAsync(organization);
        }
    }
}
