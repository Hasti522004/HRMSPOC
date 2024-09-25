namespace HRMSPOC.API.Repositories.Interfaces
{
    public interface IUserOrganizationRepository
    {
        Task<Guid?> GetOrganizationIdByUserIdAsync(string userId);
    }
}
