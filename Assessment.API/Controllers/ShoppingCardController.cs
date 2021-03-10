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
    [Authorize(Roles = UserRole.Costumer)]
    [Route("api/[controller]")]
    public class ShoppingCardController : ControllerBase
    {
        private readonly IShoppingCardRepository repository;
        private readonly UserManager<User> userManager;

        public ShoppingCardController(IShoppingCardRepository repository, UserManager<User> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(Add))]
        public async Task<IActionResult> Add([FromBody] ShoppingCardEntry entry)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            await repository.AddToActiveShoppingCard(user.Id, entry);
            return Ok();
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
            return Ok(card);
        }
    }
}
