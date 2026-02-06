using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Dtos;
using ExpenseTracker.Models;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IFileLoggerService _logger;

        public WalletController(IWalletService walletService, IFileLoggerService logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WalletResponseDto>>> GetAllWallets()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                await _logger.LogInformationAsync("Getting all wallets", new { UserId = userId });
                
                var wallets = await _walletService.GetAllWalletsAsync(userId);
                
                await _logger.LogInformationAsync("Successfully retrieved wallets", new { Count = wallets.Count, UserId = userId });
                return Ok(wallets);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error retrieving wallets", ex);
                return StatusCode(500, new { message = "Error retrieving wallets" });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<WalletResponseDto>> GetWalletById(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (id <= 0)
                {
                    await _logger.LogWarningAsync("Invalid wallet ID provided", new { WalletId = id, UserId = userId });
                    return BadRequest(new { message = "Invalid wallet ID" });
                }

                var wallet = await _walletService.GetWalletByIdAsync(id, userId);
                if (wallet == null)
                {
                    await _logger.LogWarningAsync("Wallet not found", new { WalletId = id, UserId = userId });
                    return NotFound();
                }

                await _logger.LogInformationAsync("Successfully retrieved wallet", new { WalletId = id, UserId = userId });
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error retrieving wallet", ex, new { WalletId = id });
                return StatusCode(500, new { message = "Error retrieving wallet" });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WalletResponseDto>> CreateWallet([FromBody] WalletCreateDto walletCreateDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (walletCreateDto == null || string.IsNullOrEmpty(walletCreateDto.Name))
                {
                    await _logger.LogWarningAsync("Invalid wallet data", new { UserId = userId });
                    return BadRequest(new { message = "Wallet name is required" });
                }

                if (walletCreateDto.Balance < 0)
                {
                    await _logger.LogWarningAsync("Negative balance attempted", new { Balance = walletCreateDto.Balance, UserId = userId });
                    return BadRequest(new { message = "Balance cannot be negative" });
                }

                var wallet = await _walletService.CreateWalletAsync(walletCreateDto, userId);
                
                await _logger.LogInformationAsync("Wallet created successfully", new { WalletId = wallet.Id, UserId = userId });
                return CreatedAtAction(nameof(GetWalletById), new { id = wallet.Id }, wallet);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error creating wallet", ex, new { WalletName = walletCreateDto?.Name });
                return StatusCode(500, new { message = "Error creating wallet" });
            }
        }

        [HttpPut("UpdateWallet/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] WalletUpdateDto walletDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (id <= 0 || walletDto == null || string.IsNullOrEmpty(walletDto.Name))
                {
                    await _logger.LogWarningAsync("Invalid update data", new { WalletId = id, UserId = userId });
                    return BadRequest(new { message = "Valid wallet data is required" });
                }

                if (walletDto.Balance < 0)
                {
                    await _logger.LogWarningAsync("Negative balance in update", new { Balance = walletDto.Balance, WalletId = id, UserId = userId });
                    return BadRequest(new { message = "Balance cannot be negative" });
                }

                var wallet = await _walletService.UpdateWalletAsync(id, walletDto, userId);
                if (wallet == null)
                {
                    await _logger.LogWarningAsync("Wallet not found for update", new { WalletId = id, UserId = userId });
                    return NotFound();
                }

                await _logger.LogInformationAsync("Wallet updated successfully", new { WalletId = id, UserId = userId });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error updating wallet", ex, new { WalletId = id });
                return StatusCode(500, new { message = "Error updating wallet" });
            }
        }

        [HttpGet("type/{type}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WalletResponseDto>>> GetWalletByType(WalletType type)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var wallets = await _walletService.GetWalletByTypeAsync(type, userId);
                
                await _logger.LogInformationAsync("Retrieved wallets by type", new { Type = type, Count = wallets?.Count ?? 0, UserId = userId });
                return Ok(wallets);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error retrieving wallets by type", ex, new { Type = type });
                return StatusCode(500, new { message = "Error retrieving wallets by type" });
            }
        }

        [HttpGet("totals")]
        [Authorize]
        public async Task<ActionResult<WalletTotalDto>> GetWalletTotals()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                var totalBalance = await _walletService.GetWalletTotalsAsync(userId);
                
                await _logger.LogInformationAsync("Calculated wallet totals", new { 
                    CashTotal = totalBalance.CashTotal, 
                    BankTotal = totalBalance.BankTotal, 
                    GrandTotal = totalBalance.GrandTotal,
                    UserId = userId 
                });
                return Ok(totalBalance);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error calculating wallet totals", ex);
                return StatusCode(500, new { message = "Error calculating wallet totals" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                if (id <= 0)
                {
                    await _logger.LogWarningAsync("Invalid wallet ID for deletion", new { WalletId = id, UserId = userId });
                    return BadRequest(new { message = "Invalid wallet ID" });
                }

                var result = await _walletService.DeleteWalletAsync(id, userId);
                if (!result)
                {
                    await _logger.LogWarningAsync("Wallet not found for deletion", new { WalletId = id, UserId = userId });
                    return NotFound();
                }

                await _logger.LogInformationAsync("Wallet deleted successfully", new { WalletId = id, UserId = userId });
                return NoContent();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error deleting wallet", ex, new { WalletId = id });
                return StatusCode(500, new { message = "Error deleting wallet" });
            }
        }
    }
}