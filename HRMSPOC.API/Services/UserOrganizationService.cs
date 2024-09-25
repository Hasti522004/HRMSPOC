using HRMSPOC.API.Repositories.Interfaces;
using HRMSPOC.API.Services.Interface;

namespace HRMSPOC.API.Services
{
    public class UserOrganizationService : IUserOrganizationService
    {
        private readonly IUserOrganizationRepository _userOrganizationRepository;

        public UserOrganizationService(IUserOrganizationRepository userOrganizationRepository)
        {
            _userOrganizationRepository = userOrganizationRepository;
        }

        public async Task<Guid?> GetOrganizationIdByUserIdAsync(string userId)
        {
            return await _userOrganizationRepository.GetOrganizationIdByUserIdAsync(userId);
        }
    }
}
