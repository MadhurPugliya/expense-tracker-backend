using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;

        public WalletRepository(AppDbContext context)
        {
            _context = context;
        }

            public async Task<List<Wallet>> GetAllAsync()
        {
            return await _context.Wallets.ToListAsync();
        }

                public async Task<Wallet> GetByIdAsync(int id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Wallet> CreateAsync(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<List<Wallet>> GetByTypeAsync(WalletType type)
        {
             return await _context.Wallets.Where(w => w.Type == type).ToListAsync();
        }

        public async Task<Wallet> UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }


    }

}
