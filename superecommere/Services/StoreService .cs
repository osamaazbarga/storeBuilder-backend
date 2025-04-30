using Microsoft.EntityFrameworkCore;
using superecommere.Data;
using superecommere.Models.Store;

namespace superecommere.Services
{
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext context;

        public StoreService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<TblStore> GetStoreBySubdomainAsync(string subdomain)
        {
            return await context.Stores.FirstOrDefaultAsync(s => s.Subdomain == subdomain);
        }

        public async Task<TblStore> GetStoreByCustomDomainAsync(string domain)
        {
            return await context.Stores.FirstOrDefaultAsync(s => s.CustomDomain == domain);
        }

        public async Task<TblStore> CreateStoreAsync(int userId, string name, string subdomain)
        {
            var newStore = new TblStore
            {
                Name = name,
                Subdomain = subdomain,
                OwnerUserId = userId,
                CreateDate = DateTime.UtcNow,
                DomainVerificationCode = Guid.NewGuid(),
                DomainVerificationStatus = "Unverified",
                IsActive = true
            };

            context.Stores.Add(newStore);
            await context.SaveChangesAsync();

            return newStore;
        }

        public async Task<TblStore> UpdateCustomDomainAsync(int storeId, string customDomain)
        {
            var store = await context.Stores.FindAsync(storeId);
            if (store == null) return null;

            store.CustomDomain = customDomain;
            store.DomainVerificationStatus = "Unverified";
            store.DomainVerificationCode = Guid.NewGuid();

            await context.SaveChangesAsync();
            return store;
        }

        public async Task<bool> VerifyCustomDomainAsync(int storeId)
        {
            var store = await context.Stores.FindAsync(storeId);
            if (store == null) return false;

            // Example: in production, check DNS TXT record
            store.DomainVerificationStatus = "Verified";

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateStoreAsync(int storeId)
        {
            var store = await context.Stores.FindAsync(storeId);
            if (store == null) return false;

            store.IsActive = false;
            await context.SaveChangesAsync();

            return true;
        }
    }
}
