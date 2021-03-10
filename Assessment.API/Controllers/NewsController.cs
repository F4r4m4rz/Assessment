using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository repository;

        public NewsController(INewsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route(nameof(Get))]
        public async Task<IActionResult> Get(int id)
        {
            var news = await repository.GetAsync(id);
            if (news == null)
                return NotFound(new { Error = "News could not be found", Id = id });

            return Ok(news);
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var news = await repository.GetAllAsync();
            if (news == null || news.Count() == 0)
                return NotFound("No news could be found");

            return Ok(news);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Add([FromBody] News news)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await repository.Add(news);
            return Ok(news.Id);
        }
    }
}
