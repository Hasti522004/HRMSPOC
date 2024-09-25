namespace HRMSPOC.API.Services.Interface
{
    public interface IUserOrganizationService
    {
        Task<Guid?> GetOrganizationIdByUserIdAsync(string userId);
    }
}
