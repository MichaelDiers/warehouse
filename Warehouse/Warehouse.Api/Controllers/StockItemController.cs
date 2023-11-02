namespace Warehouse.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Warehouse.Api.Contracts.StockItems;
    using Warehouse.Api.Extensions;
    using Warehouse.Api.Models.StockItems;

    /// <summary>
    ///     The controller for manipulating stock items.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class StockItemController : ControllerBase
    {
        /// <summary>
        ///     The business logic for handling stock items.
        /// </summary>
        private readonly IStockItemService stockItemService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StockItemController" /> class.
        /// </summary>
        /// <param name="stockItemService">The business logic for handling stock items.</param>
        public StockItemController(IStockItemService stockItemService)
        {
            this.stockItemService = stockItemService;
        }

        // DELETE api/<StockItemController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: api/<StockItemController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[]
            {
                "value1",
                "value2"
            };
        }

        // GET api/<StockItemController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        ///     Posts the specified stock item.
        /// </summary>
        /// <param name="createStockItem">The stock item to be created.</param>
        /// <returns>The created stock item.</returns>
        [HttpPost]
        public async Task<IStockItem> Post([FromBody] CreateStockItem createStockItem)
        {
            return await this.stockItemService.CreateAsync(
                createStockItem,
                this.User.Claims.RequiredId());
        }

        // PUT api/<StockItemController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
