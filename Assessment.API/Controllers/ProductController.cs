using Assessment.Data.Interfaces;
using Assessment.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var products = await repository.GetAllAsync();
            if (products == null || products.Count() == 0)
                return NotFound("No product could be found");

            return Ok(products);
        }

        [HttpGet]
        [Route(nameof(Filter))]
        public async Task<IActionResult> Filter(string filter)
        {
            var products = await repository.GetAllFilterdAsync(filter);
            if (products == null || products.Count() == 0)
                return NotFound("No product could be found");

            return Ok(products);
        }

        [HttpGet]
        [Route("Pagination")]
        public async Task<IActionResult> GetPaginated(int page, int perPage)
        {
            if (page < 1 || perPage < 1)
                return BadRequest("Page and number of items per page should be greater than 0");

            var products = await repository.GetPaginatedAsync(page, perPage);
            return Ok(products);
        }

        [HttpGet]
        [Route(nameof(Get))]
        public async Task<IActionResult> Get(int id)
        {
            var product = await repository.GetAsync(id);
            if (product == null)
                return NotFound($"Product not found. Id: {id}");

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Admin)]
        public IActionResult Add([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(product);
            return Ok(product.Id);
        }

        [HttpDelete]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await repository.GetAsync(id);
            if (product == null)
                return BadRequest($"Product not found. Id: {id}");

            repository.Remove(id);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = UserRole.Admin)]
        public IActionResult Update([FromBody] Product product)
        {
            if (product.Id == 0)
                ModelState.AddModelError("Id", "Not provided");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Update(product);
            return Ok(product.Id);
        }
    }
}
