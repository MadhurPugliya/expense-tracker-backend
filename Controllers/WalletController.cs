
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

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        #region Wallet
        
            [HttpGet]
            [Authorize]
        public async Task<IActionResult> GetAllWallets()
        {
            var wallets = await _walletService.GetAllWalletsAsync();
            return Ok(wallets);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetWalletById(int id)
        {
            var wallet = await _walletService.GetWalletByIdAsync(id);
            if (wallet == null)
                return NotFound(new { message = "Wallet not found" });

            return Ok(wallet);
        }

        [HttpPost("Wallet")]
        [Authorize]

        public async Task<IActionResult> CreateWallet([FromBody] WalletCreateDto walletCreateDto)
        {
            var wallet = await _walletService.CreateWalletAsync(walletCreateDto);
            return Ok(new { message = "Wallet created successfully", wallet });
        }      

        [HttpPut("UpdateWallet")]
        [Authorize]

        public async Task<IActionResult> UpdateWallet(int id, [FromBody] WalletUpdateDto walletDto)
        {
            var wallet = await _walletService.UpdateWalletAsync(id, walletDto);
            if(wallet == null)
            return NotFound(new { message = "Wallet not found" });
            return Ok(new { message = "Wallet updated successfully", wallet });
        }


        [HttpGet("type/{type}")]
        [Authorize]
        public async Task<IActionResult> GetWalletByType(WalletType type)
        {
            var wallet = await _walletService.GetWalletByTypeAsync(type);
            return Ok(wallet);
        }



    }
}
#endregion