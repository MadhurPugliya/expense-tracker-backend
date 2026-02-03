using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories.interfaces
{
    public interface IWalletRepository
    {
         Task<List<Wallet>> GetAllAsync();
         Task<Wallet> GetByIdAsync(int id);

         Task<List<Wallet>> GetByTypeAsync(WalletType type);
         Task<Wallet> CreateAsync(Wallet wallet);

         Task<Wallet> UpdateAsync(Wallet wallet);
        
    }
}