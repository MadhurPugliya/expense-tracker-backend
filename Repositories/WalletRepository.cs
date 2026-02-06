using ExpenseTracker.Data;
using ExpenseTracker.Dtos;
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

        public async Task<List<Wallet>> GetAllAsync(int userId)
        {
            return await _context.Wallets.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<Wallet> GetByIdAsync(int id, int userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
        }

        public async Task<Wallet> CreateAsync(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<List<Wallet>> GetByTypeAsync(WalletType type, int userId)
        {
            return await _context.Wallets.Where(w => w.Type == type && w.UserId == userId).ToListAsync();
        }

        public async Task<Wallet> UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<List<Wallet>> GetWalletTotalsAsync(int userId)
        {
            return await _context.Wallets.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<bool> DeleteAsync(Wallet wallet)
        {
            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}