using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories.interfaces
{
    public interface IWalletRepository
    {
         Task<List<Wallet>> GetAllAsync(int userId);
         Task<Wallet> GetByIdAsync(int id, int userId);
         Task<List<Wallet>> GetByTypeAsync(WalletType type, int userId);
         Task<Wallet> CreateAsync(Wallet wallet);
         Task<Wallet> UpdateAsync(Wallet wallet);
         Task<List<Wallet>> GetWalletTotalsAsync(int userId);
         Task<bool> DeleteAsync(Wallet wallet);
    }
}