using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductInventoryManagement.Entities;
using ProductInventoryManagement.Exceptions;
using ProductInventoryManagement.Model;
using ProductInventoryManagement.Repositories;

namespace ProductInventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryManagementController : ControllerBase
    {
        private readonly IProductInventoryManagementRepository _productInventoryManagementRepository;
        private readonly ILogger<ProductInventoryManagementController> _logger;
        string NotFoundError = "Not Found Error: The product with the given Id was not found in the inventory.";
        string NullObjecError = "Null Object Error: The product cannot be null";
        string NullParameterError = "Null Parameter Error: The filter or paging parameters cannot be null";
        string InvalidIdError = "Invalid Id Error: The provided Id does not match the product's Id";
        string DuplicationError = "Duplication Error: The product with same name and brand already exists in the inventory.";
        public ProductInventoryManagementController(IProductInventoryManagementRepository productInventoryManagementRepository, ILogger<ProductInventoryManagementController> logger)
        {
            _productInventoryManagementRepository = productInventoryManagementRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all the products from the inventory.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllProductsFromInventory")]
        [HttpGet]
        public IActionResult GetAllProductsFromInventory()
        {
            var products = _productInventoryManagementRepository.GetAllProductsFromInventory();
            return Ok(products);
        }

        /// <summary>
        /// Gets the product for the given product Id.
        /// </summary>
        /// <param name="id">Id for the product</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Not Found Error: The product not found in the inventory.</exception>
        [Route("GetProductFromInventoryById")]
        [HttpGet]
        public IActionResult GetProductFromInventoryById([FromQuery]int id)
        {
            var product = _productInventoryManagementRepository.GetProductFromInventoryById(id);
            if (product == null)
            {
                _logger.LogError(NotFoundError);
                throw new NotFoundException(NotFoundError);
            }
            return Ok(product);
        }

        /// <summary>
        /// Gets the products from the inventory using pagination
        /// </summary>
        /// <param name="productInventoryManagementPagingParameters">Null Parameter Error: The filter or paging parameters cannot be null</param>
        /// <returns></returns>
        [Route("GetProductsFromInventoryUsingPaging")]
        [HttpGet]
        public IActionResult GetProductsFromInventoryUsingPaging([FromQuery] ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters)
        {
            if (productInventoryManagementPagingParameters == null)
            {
                _logger.LogError(NullParameterError);
                throw new ValidationException(NullParameterError);
            }
            var products = _productInventoryManagementRepository.GetProductsFromInventoryUsingPaging(productInventoryManagementPagingParameters);
            return Ok(products);
        }

       
        /// <summary>
        /// Gets the list of products based on given filter and page parameters
        /// </summary>
        /// <param name="productInventoryMangementFilter"></param>
        /// <param name="productInventoryManagementPagingParameters"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException">Null Parameter Error: The filter or paging parameters cannot be null</exception>
        [Route("GetProductsFromInventoryUsingFilter")]
        [HttpGet]
        public IActionResult GetProductsFromInventoryUsingFilter([FromQuery] ProductInventoryMangementFilter productInventoryMangementFilter, [FromQuery] ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters)
        {
            if (productInventoryMangementFilter == null || productInventoryManagementPagingParameters == null)
            {
                _logger.LogError(NullParameterError);
                throw new ValidationException(NullParameterError);
            }
            var products = _productInventoryManagementRepository.GetProductsFromInventoryUsingFilter(productInventoryMangementFilter, productInventoryManagementPagingParameters);
            return Ok(products);
        }

        /// <summary>
        /// Adds product to the inventory.Product name and brand should be unique.
        /// </summary>
        /// <param name="product">Object with Id,Name,Brand and Price</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">1) Duplication Error: The product with name and brand already exists in the inventory.
        /// 2) Null Object Error: The product cannot be null </exception>
        [Route("AddProductToInventory")]
        [HttpPost]
        public IActionResult AddProductToInventory([FromBody] Product product)
        {
            if (product == null)
            {
                _logger.LogError(NullObjecError);
                throw new ValidationException(NullObjecError);
            }
            var existingProducts = _productInventoryManagementRepository.GetAllProductsFromInventory();
            foreach(var existingProduct in existingProducts)
            {
                if(string.Equals(existingProduct.Name,product.Name,StringComparison.OrdinalIgnoreCase) && 
                    string.Equals(existingProduct.Brand,product.Brand,StringComparison.OrdinalIgnoreCase)) {
                    _logger.LogError(DuplicationError);
                    throw new ValidationException(DuplicationError);
                }
            }
            _productInventoryManagementRepository.AddProductToInventory(product);
            return CreatedAtAction(nameof(GetProductFromInventoryById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update/Modify the product properties
        /// </summary>
        /// <param name="id">Id for the product</param>
        /// <param name="product">Object with Id,Name,Brand and Price</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"> 1) Null Object Error: The product cannot be null
        /// 2) Invalid Id Error: The product id does not match.</exception>
        /// <exception cref="NotFoundException">Not Found Error: The product not found in the inventory.</exception>
        [Route("UpdateProductInsideInventory")]
        [HttpPut]
        public IActionResult UpdateProductInsideInventory([FromQuery]int id, [FromBody] Product product)
        {
            if (product == null)
            {
                _logger.LogError(NullObjecError);
                throw new ValidationException(NullObjecError);
            }
            if(id!=product.Id)
            {
                _logger.LogError(InvalidIdError);
                throw new ValidationException(InvalidIdError);
            }
            var existingProduct = _productInventoryManagementRepository.GetProductFromInventoryById(id);
            if (existingProduct == null)
            {
                _logger.LogError(NotFoundError);
                throw new NotFoundException(NotFoundError);
            }
            _productInventoryManagementRepository.UpdateProductInsideInventory(product);
            return NoContent();
        }

        /// <summary>
        /// Deletes the product from the inventory.
        /// </summary>
        /// <param name="id">Id for the product</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Not Found Error: The product not found in the inventory.</exception>
        [Route("DeleteProductFromInventory")]
        [HttpDelete]
        public IActionResult DeleteProductFromInventory([FromQuery] int id)
        {
            var existingProduct = _productInventoryManagementRepository.GetProductFromInventoryById(id);
            if (existingProduct == null)
            {
                _logger.LogError(NotFoundError);
                throw new NotFoundException(NotFoundError);
            }
            _productInventoryManagementRepository.DeleteProductFromInventory(id);
            return NoContent();
        }
    }
}
