using superecommere.Data;
using superecommere.Models.Domain;
using superecommere.Repositories.Interface;

namespace superecommere.Repositories.Implementation
{
    public class TblUserRepository:ITblUserRepository
    {


        private readonly ApplicationDbContext _superEcommereDbContext;

        public TblUserRepository(ApplicationDbContext superEcommereDbContext)
        {
            _superEcommereDbContext = superEcommereDbContext;
        }
        public async Task<TblUser> CreateAsync(TblUser user)
        {
            await _superEcommereDbContext.Users.AddAsync(user);
            await _superEcommereDbContext.SaveChangesAsync();

            return user;
        }

        public async Task<TblUser> RemoveAsync(TblUser user)
        {
            _superEcommereDbContext.Users.Remove(user);
            await _superEcommereDbContext.SaveChangesAsync();
            return user;
        }

        public void UpdateAsync()
        {
            _superEcommereDbContext.SaveChangesAsync();
        }
    }
}
