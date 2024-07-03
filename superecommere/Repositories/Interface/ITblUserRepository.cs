using superecommere.Models.Domain;

namespace superecommere.Repositories.Interface
{
    public interface ITblUserRepository
    {
        Task<TblUser> CreateAsync(TblUser User);
        Task<TblUser> RemoveAsync(TblUser User);

    }
}
