using ExpenseTracker.Dtos;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories.interfaces;

namespace ExpenseTracker.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<List<WalletResponseDto>> GetAllWalletsAsync(int userId)
        {
            var wallets = await _walletRepository.GetAllAsync(userId);
            return wallets.Select(w => new WalletResponseDto
            {
                Id = w.Id,
                Name = w.Name,
                Balance = w.Balance,
                Type = w.Type
            }).ToList();
        }

        public async Task<WalletResponseDto> GetWalletByIdAsync(int id, int userId)
        {
            var wallet = await _walletRepository.GetByIdAsync(id, userId);
            if (wallet == null) return null;

            return new WalletResponseDto
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Balance = wallet.Balance,
                Type = wallet.Type
            };
        }

        public async Task<WalletResponseDto> CreateWalletAsync(WalletCreateDto walletDto, int userId)
        {
            var wallet = new Wallet
            {
                Name = walletDto.Name,
                Balance = walletDto.Balance,
                Type = walletDto.Type,
                UserId = userId
            };

            var createdWallet = await _walletRepository.CreateAsync(wallet);
            return new WalletResponseDto
            {
                Id = createdWallet.Id,
                Name = createdWallet.Name,
                Balance = createdWallet.Balance,
                Type = createdWallet.Type
            };
        }

        public async Task<WalletResponseDto> UpdateWalletAsync(int id, WalletUpdateDto walletDto, int userId)
        {
            var existingWallet = await _walletRepository.GetByIdAsync(id, userId);
            if (existingWallet == null) return null;

            existingWallet.Name = walletDto.Name;
            existingWallet.Balance = walletDto.Balance;
            existingWallet.Type = walletDto.Type;

            var updatedWallet = await _walletRepository.UpdateAsync(existingWallet);

            return new WalletResponseDto
            {
                Id = updatedWallet.Id,
                Name = updatedWallet.Name,
                Balance = updatedWallet.Balance,
                Type = updatedWallet.Type
            };
        }

        public async Task<List<WalletResponseDto>> GetWalletByTypeAsync(WalletType type, int userId)
        {
            var wallets = await _walletRepository.GetByTypeAsync(type, userId);
            if (wallets == null) return null;

            return wallets.Select(w => new WalletResponseDto
            {
                Id = w.Id,
                Name = w.Name,
                Balance = w.Balance,
                Type = w.Type
            }).ToList();
        }

        public async Task<WalletTotalDto> GetWalletTotalsAsync(int userId)
        {
            var wallets = await _walletRepository.GetWalletTotalsAsync(userId);

            var cashTotal = wallets.Where(w => w.Type == WalletType.Cash).Sum(w => w.Balance);
            var bankTotal = wallets.Where(w => w.Type == WalletType.Bank).Sum(w => w.Balance);

            return new WalletTotalDto
            {
                CashTotal = cashTotal,
                BankTotal = bankTotal,
                GrandTotal = cashTotal + bankTotal
            };
        }

        public async Task<bool> DeleteWalletAsync(int id, int userId)
        {
            var wallet = await _walletRepository.GetByIdAsync(id, userId);
            if (wallet == null) return false;

            return await _walletRepository.DeleteAsync(wallet);
        }
    }
}