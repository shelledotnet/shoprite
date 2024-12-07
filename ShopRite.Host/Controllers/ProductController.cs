using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopRite.Application;
using ShopRite.Application.Dto;
using ShopRite.Application.Dto.Product;
using ShopRite.Application.Services.Interfaces;
using System.Net;

namespace ShopRite.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //using primary constructor 
    public class ProductController(IProductService productService, IMapper mapper) : ControllerBase
    {

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>A List of products</returns>
        /// <response code="200">A List of products</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<GetProduct>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ServiceFailedResponse))]
        [HttpGet("get-all-products", Name = "products"), AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {

            try
            {

                ServiceResponse<IEnumerable<GetProduct>> response = await productService.GetAllAsync();


                return response.Code switch
                {
                    HttpStatusCode.OK => Ok(response),
                    HttpStatusCode.NotFound => NotFound(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.BadRequest => BadRequest(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.Conflict => Conflict(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.InternalServerError => StatusCode(500, mapper.Map<ServiceFailedResponse>(response)),

                    _ => StatusCode(422, mapper.Map<ServiceFailedResponse>(response)),
                };


            }
            catch (Exception ex)
            {

                // _logger.LogError($"GetBooks: {ex}");
                return StatusCode(500, new ServiceFailedResponse(HttpStatusCode.InternalServerError, false, "error getting product this time"));
            }


        }


        /// <summary>
        /// Get a product
        /// </summary>
        /// <returns>A single product</returns>
        /// <response code="200">a product</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<GetProduct>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ServiceFailedResponse))]
        [HttpGet("get-product-by-id/{id}", Name = "productById"), AllowAnonymous]
        public async Task<IActionResult> GetProductsById([FromRoute] ProductById productById)
        {

            try
            {

                ServiceResponse<GetProduct> response = await productService.GetByIdAsync(productById.Id);


                return response.Code switch
                {
                    HttpStatusCode.OK => Ok(response),
                    HttpStatusCode.NotFound => NotFound(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.BadRequest => BadRequest(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.Conflict => Conflict(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.InternalServerError => StatusCode(500, mapper.Map<ServiceFailedResponse>(response)),

                    _ => StatusCode(422, mapper.Map<ServiceFailedResponse>(response)),
                };


            }
            catch (Exception ex)
            {

                // _logger.LogError($"GetProductsById: {ex}");
                return StatusCode(500, new ServiceFailedResponse(HttpStatusCode.InternalServerError, false, "error getting product this time"));
            }


        }



        /// <summary>
        /// Create product
        /// </summary>
        /// <returns> product</returns>
        /// <response code="201">Book</response>
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ServiceFailedResponse))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ServiceResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceFailedResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ServiceFailedResponse))]
        [HttpPost()]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProduct createProduct)
        {
            #region ModelState


            #endregion
            try
            {


                ServiceResponse<GetProduct?> response = await productService.AddAsync(createProduct);

                return response.Code switch
                {
                    HttpStatusCode.Created => CreatedAtRoute("productById", new ProductById { Id = response.Data.Id }, response),
                    HttpStatusCode.NotFound => NotFound(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.BadRequest => BadRequest(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.Conflict => Conflict(mapper.Map<ServiceFailedResponse>(response)),
                    HttpStatusCode.InternalServerError => StatusCode(500, mapper.Map<ServiceFailedResponse>(response)),

                    _ => StatusCode(422, mapper.Map<ServiceFailedResponse>(response)),
                };


            }
            catch (Exception ex)
            {

                // _logger.LogError($"CreateProduct: {ex}");
                return StatusCode(500, new ServiceFailedResponse(HttpStatusCode.InternalServerError, false, "error creating product this time"));

            }





        }


    }
}
