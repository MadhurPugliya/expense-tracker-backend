using ExpenseTracker.Dtos;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories.interfaces;

namespace ExpenseTracker.Services{
    public class WalletService : IWalletService{
        
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<List<WalletResponseDto>> GetAllWalletsAsync()
        {
            var wallets = await _walletRepository.GetAllAsync();
            return wallets.Select(w => new WalletResponseDto
            {
                Id = w.Id,
                Name = w.Name,
                Balance = w.Balance,
                Type = w.Type
            }).ToList();
        }

        public async Task<WalletResponseDto> GetWalletByIdAsync(int id)
        {
            var wallet = await _walletRepository.GetByIdAsync(id);
            if(wallet == null) return null;

            return new WalletResponseDto
            {
                Id = wallet.Id,
                Name = wallet.Name,
                Balance = wallet.Balance,
                Type = wallet.Type
            };
        }

        public async Task<WalletResponseDto> CreateWalletAsync(WalletCreateDto walletDto)
        {
            var wallet = new Wallet
            {
                Name = walletDto.Name,
                Balance = walletDto.Balance,
                Type = walletDto.Type
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

        public async Task<WalletResponseDto> UpdateWalletAsync(int id, WalletUpdateDto walletDto)
        {
            var existingWallet = await _walletRepository.GetByIdAsync(id);
            if(existingWallet == null) return null;

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

        public async Task<List<WalletResponseDto>> GetWalletByTypeAsync(WalletType type)
        {
            var wallets = await _walletRepository.GetByTypeAsync(type);
               if(wallets == null) return null;

            return wallets.Select(w => new WalletResponseDto
            {
                Id = w.Id,
                Name = w.Name,
                Balance = w.Balance,
                Type = w.Type
            }).ToList();
        }
        
    }
}