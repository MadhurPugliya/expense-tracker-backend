using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public interface IWalletService
    {
        Task<List<WalletResponseDto>> GetAllWalletsAsync(int userId);
        Task<WalletResponseDto> GetWalletByIdAsync(int id, int userId);
        Task<WalletResponseDto> CreateWalletAsync(WalletCreateDto walletDto, int userId);
        Task<List<WalletResponseDto>> GetWalletByTypeAsync(WalletType type, int userId);
        Task<WalletResponseDto> UpdateWalletAsync(int id, WalletUpdateDto walletDto, int userId);
        Task<WalletTotalDto> GetWalletTotalsAsync(int userId);
        Task<bool> DeleteWalletAsync(int id, int userId);
    }
}