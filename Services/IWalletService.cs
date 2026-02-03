using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public interface IWalletService
    {
                Task<List<WalletResponseDto>> GetAllWalletsAsync();
     
        Task<WalletResponseDto> GetWalletByIdAsync(int id);

        Task<WalletResponseDto> CreateWalletAsync(WalletCreateDto walletDto);

        Task<List<WalletResponseDto>> GetWalletByTypeAsync(WalletType type);

        Task<WalletResponseDto> UpdateWalletAsync(int id, WalletUpdateDto walletDto);
    }
}