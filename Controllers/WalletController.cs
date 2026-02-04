using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllWallets()
        {
            _logger.LogInformation("Getting all wallets");
            try
            {
                var wallets = await _walletService.GetAllWalletsAsync();
                _logger.LogInformation("Successfully retrieved {Count} wallets", wallets.Count);
                return Ok(wallets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wallets");
                return StatusCode(500, new { message = "Error retrieving wallets", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetWalletById(int id)
        {
            _logger.LogInformation("Getting wallet with ID: {WalletId}", id);
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid wallet ID provided: {WalletId}", id);
                    return BadRequest(new { message = "Invalid wallet ID" });
                }

                var wallet = await _walletService.GetWalletByIdAsync(id);
                if (wallet == null)
                {
                    _logger.LogWarning("Wallet not found with ID: {WalletId}", id);
                    return NotFound(new { message = "Wallet not found" });
                }

                _logger.LogInformation("Successfully retrieved wallet: {WalletName}", wallet.Name);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wallet with ID: {WalletId}", id);
                return StatusCode(500, new { message = "Error retrieving wallet", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWallet([FromBody] WalletCreateDto walletCreateDto)
        {
            _logger.LogInformation("Creating new wallet: {WalletName}", walletCreateDto?.Name);
            try
            {
                if (walletCreateDto == null)
                {
                    _logger.LogWarning("Wallet creation attempted with null data");
                    return BadRequest(new { message = "Wallet data is required" });
                }

                if (string.IsNullOrEmpty(walletCreateDto.Name))
                {
                    _logger.LogWarning("Wallet creation attempted without name");
                    return BadRequest(new { message = "Wallet name is required" });
                }

                if (walletCreateDto.Balance < 0)
                {
                    _logger.LogWarning("Wallet creation attempted with negative balance: {Balance}", walletCreateDto.Balance);
                    return BadRequest(new { message = "Balance cannot be negative" });
                }

                var wallet = await _walletService.CreateWalletAsync(walletCreateDto);
                _logger.LogInformation("Successfully created wallet: {WalletName} with ID: {WalletId}", wallet.Name, wallet.Id);
                return Ok(new { message = "Wallet created successfully", wallet });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating wallet: {WalletName}", walletCreateDto?.Name);
                return StatusCode(500, new { message = "Error creating wallet", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] WalletUpdateDto walletDto)
        {
            _logger.LogInformation("Updating wallet with ID: {WalletId}", id);
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid wallet ID for update: {WalletId}", id);
                    return BadRequest(new { message = "Invalid wallet ID" });
                }

                if (walletDto == null)
                {
                    _logger.LogWarning("Wallet update attempted with null data for ID: {WalletId}", id);
                    return BadRequest(new { message = "Wallet data is required" });
                }

                if (string.IsNullOrEmpty(walletDto.Name))
                {
                    _logger.LogWarning("Wallet update attempted without name for ID: {WalletId}", id);
                    return BadRequest(new { message = "Wallet name is required" });
                }

                if (walletDto.Balance < 0)
                {
                    _logger.LogWarning("Wallet update attempted with negative balance: {Balance} for ID: {WalletId}", walletDto.Balance, id);
                    return BadRequest(new { message = "Balance cannot be negative" });
                }

                var wallet = await _walletService.UpdateWalletAsync(id, walletDto);
                if (wallet == null)
                {
                    _logger.LogWarning("Wallet not found for update with ID: {WalletId}", id);
                    return NotFound(new { message = "Wallet not found" });
                }

                _logger.LogInformation("Successfully updated wallet: {WalletName} with ID: {WalletId}", wallet.Name, wallet.Id);
                return Ok(new { message = "Wallet updated successfully", wallet });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating wallet with ID: {WalletId}", id);
                return StatusCode(500, new { message = "Error updating wallet", details = ex.Message });
            }
        }

        [HttpGet("type/{type}")]
        [Authorize]
        public async Task<IActionResult> GetWalletByType(WalletType type)
        {
            _logger.LogInformation("Getting wallets by type: {WalletType}", type);
            try
            {
                var wallets = await _walletService.GetWalletByTypeAsync(type);
                _logger.LogInformation("Successfully retrieved {Count} wallets of type: {WalletType}", wallets?.Count ?? 0, type);
                return Ok(wallets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving wallets by type: {WalletType}", type);
                return StatusCode(500, new { message = "Error retrieving wallets by type", details = ex.Message });
            }
        }

        [HttpGet("totals")]
        [Authorize]
        public async Task<IActionResult> GetWalletTotals()
        {
            _logger.LogInformation("Calculating wallet totals");
            try
            {
                var totalBalance = await _walletService.GetWalletTotalsAsync();
                _logger.LogInformation("Successfully calculated totals - Cash: {CashTotal}, Bank: {BankTotal}, Grand: {GrandTotal}", 
                    totalBalance.CashTotal, totalBalance.BankTotal, totalBalance.GrandTotal);
                return Ok(totalBalance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating wallet totals");
                return StatusCode(500, new { message = "Error calculating wallet totals", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            _logger.LogInformation("Deleting wallet with ID: {WalletId}", id);
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid wallet ID for deletion: {WalletId}", id);
                    return BadRequest(new { message = "Invalid wallet ID" });
                }

                var result = await _walletService.DeleteWalletAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Wallet not found for deletion with ID: {WalletId}", id);
                    return NotFound(new { message = "Wallet not found" });
                }

                _logger.LogInformation("Successfully deleted wallet with ID: {WalletId}", id);
                return Ok(new { message = "Wallet deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting wallet with ID: {WalletId}", id);
                return StatusCode(500, new { message = "Error deleting wallet", details = ex.Message });
            }
        }
    }
}