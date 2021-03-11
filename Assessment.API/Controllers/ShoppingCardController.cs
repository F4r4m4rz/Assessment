using Assessment.Data.Exceptions;
using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Assessment.API.Controllers
{
    [ApiController]
    [Authorize(Roles = UserRole.Costumer + "," + UserRole.Admin)]
    [Route("api/[controller]")]
    public class ShoppingCardController : ControllerBase
    {
        private readonly IShoppingCardRepository repository;
        private readonly UserManager<IdentityUser> userManager;

        public ShoppingCardController(IShoppingCardRepository repository, UserManager<IdentityUser> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(Add))]
        public async Task<IActionResult> Add([FromBody] ShoppingCardEntry entry)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            await repository.AddToActiveShoppingCard(user.Id, entry);
            return Ok(entry.Id);
        }

        [HttpGet]
        [Route(nameof(Get))]
        public async Task<IActionResult> Get(int cardId)
        {
            var card = await repository.GetShoppingCard(cardId);
            if (card == null)
                NotFound($"No card found. Id: {cardId}");

            return Ok(card);
        }
        
        [HttpGet]
        [Route(nameof(GetUserActiveCard))]
        public async Task<IActionResult> GetUserActiveCard()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var card = await repository.GetActiveShoppingCard(user.Id);

            if (card == null)
                return NotFound(new
                {
                    Message = "No active shopping card found",
                    User = user
                });

            return Ok(card);
        }

        [HttpPost]
        [Route(nameof(Checkout))]
        public async Task<IActionResult> Checkout(int cardId)
        {
            ShoppingCard card;
            try
            {
                card = await repository.CheckOutShoppingCard(cardId);
            }
            catch (EntryNotFoundException ex)
            {
                return BadRequest(new
                {
                    Error = ex.Message,
                    CardId = cardId
                });
            }
            
            return Ok(card);
        }

        [HttpGet]
        [Route(nameof(History))]
        public async Task<IActionResult> History()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            return await History(user.Id);
        }

        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        [Route(nameof(History))]
        public async Task<IActionResult> History(string userId)
        {
            try
            {
                return Ok(await repository.ShoppingCardHistory(userId));
            }
            catch (EntryNotFoundException ex)
            {
                return NotFound(new
                {
                    Error = ex.Message
                });
            }
        }
    }
}
