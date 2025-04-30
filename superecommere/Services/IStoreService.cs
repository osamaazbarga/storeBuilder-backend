using superecommere.Models.Store;

namespace superecommere.Services
{
    public interface IStoreService
    {
        Task<TblStore> GetStoreBySubdomainAsync(string subdomain);
        Task<TblStore> GetStoreByCustomDomainAsync(string domain);
        Task<bool> VerifyCustomDomainAsync(int userId);
        Task<TblStore> CreateStoreAsync(int userId, string name, string subdomain);
    }
}
