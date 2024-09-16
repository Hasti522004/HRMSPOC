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
        public async Task CreateOrganizationAsync(Organization organization)
        {
            await _organizationRepository.AddAsync(organization);
        }

        public async Task DeleteOrganizationAsync(int id)
        {
            await _organizationRepository.DeleteAsync(id);
        }

        public async Task<Organization> GetOrganizationByIdAsync(int id)
        {
            return await _organizationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Organization>> GetOrganizationsAsync()
        {
            return await _organizationRepository.GetAllAsync();
        }

        public async Task UpdateOrganizationAsync(Organization organization)
        {
            await _organizationRepository.UpdateAsync(organization);
        }
    }
}
