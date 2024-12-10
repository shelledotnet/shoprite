using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopRite.Application.Dto;
using ShopRite.Application.Services.Interfaces;
using ShopRite.Application;
using System.Net;
using ShopRite.Application.Dto.Category;
using ShopRite.Host.Filter;

namespace ShopRite.Host.Controllers
{
    //[Produces("application/json", "application/xml")]  //output formatter Media type: Accept header
    //[Consumes("application/json")] //input-formatter Media type: content-type header
    [Route("api/[controller]")]
    [ServiceFilter(typeof(RequestAuthActionFilterAttribute))]
    [ApiController]
    //using primary constructor 
    public class CategoryController(ICategoryService categoryService, IMapper mapper) : ControllerBase
    {
        
            /// <summary>
            /// Get all categorys
            /// </summary>
            /// <returns>A List of categorys</returns>
            /// <response code="200">A List of categorys</response>
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<IEnumerable<GetCategory>>))]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ServiceFailedResponse))]
            [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ServiceFailedResponse))]
            [HttpGet("", Name = "category"), AllowAnonymous]
            public async Task<IActionResult> GetCategorys()
            {

                try
                {

                    ServiceResponse<IEnumerable<GetCategory>> response = await categoryService.GetAllAsync();


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
                    return StatusCode(500, new ServiceFailedResponse(HttpStatusCode.InternalServerError, false, "error getting category this time"));
                }


            }


            /// <summary>
            /// Get a category
            /// </summary>
            /// <returns>A single category</returns>
            /// <response code="200">a category</response>
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<GetCategory>))]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ServiceFailedResponse))]
            [HttpGet("{id:guid}", Name = "categoryById"), AllowAnonymous]
            public async Task<IActionResult> GetCategorysById([FromRoute] CategoryById categoryById)
            {

                try
                {

                    ServiceResponse<GetCategory> response = await categoryService.GetByIdAsync(categoryById.id);


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

                    // _logger.LogError($"GetCategorysById: {ex}");
                    return StatusCode(500, new ServiceFailedResponse(HttpStatusCode.InternalServerError, false, "error getting category this time"));
                }


            }



            /// <summary>
            /// Create category
            /// </summary>
            /// <returns> category</returns>
            /// <response code="201">Book</response>
            [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ServiceFailedResponse))]
            [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ServiceResponse<string>))]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceFailedResponse))]
            [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ServiceFailedResponse))]
            [HttpPost("", Name = "addcategory")]
            public async Task<IActionResult> Createcategory([FromBody] CreateCategory createCategory)
            {
                #region ModelState


                #endregion
                try
                {


                    ServiceResponse<GetCategory?> response = await categoryService.AddAsync(createCategory);

                    return response.Code switch
                    {
                        HttpStatusCode.Created => CreatedAtRoute("categoryById", new CategoryById { id = response.Data.Id }, response),
                        HttpStatusCode.NotFound => NotFound(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.BadRequest => BadRequest(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.Conflict => Conflict(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.InternalServerError => StatusCode(500, mapper.Map<ServiceFailedResponse>(response)),

                        _ => StatusCode(422, mapper.Map<ServiceFailedResponse>(response)),
                    };


                }
                catch (Exception ex)
                {

                    // _logger.LogError($"Createcategory: {ex}");
                    return StatusCode(500, new ServiceFailedResponse(HttpStatusCode.InternalServerError, false, "error creating category this time"));

                }





            }


            /// <summary>
            /// Update category
            /// </summary>
            /// <returns> category</returns>
            /// <response code="204">Book</response>
            [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ServiceFailedResponse))]
            [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ServiceResponse<string>))]
            [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ServiceFailedResponse))]
            [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ServiceFailedResponse))]
            [HttpPut("", Name = "updatecategory")]
            public async Task<IActionResult> Updatecategory([FromBody] UpdateCategory updatecategory)
            {
                #region ModelState


                #endregion
                try
                {


                    ServiceResponse<string> response = await categoryService.UpdateAsync(updatecategory);

                    return response.Code switch
                    {
                        HttpStatusCode.NoContent => NoContent(),
                        HttpStatusCode.NotFound => NotFound(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.BadRequest => BadRequest(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.Conflict => Conflict(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.InternalServerError => StatusCode(500, mapper.Map<ServiceFailedResponse>(response)),

                        _ => StatusCode(422, mapper.Map<ServiceFailedResponse>(response)),
                    };


                }
                catch (Exception ex)
                {

                    // _logger.LogError($"Createcategory: {ex}");
                    return StatusCode(500, new ServiceFailedResponse(HttpStatusCode.InternalServerError, false, "error creating category this time"));

                }

            }

            /// <summary>
            /// Delete a category
            /// </summary>
            /// <returns>Delete categoryt</returns>
            /// <response code="204">a category</response>
            [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<GetCategory>))]
            [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ServiceFailedResponse))]
            [HttpDelete("{id:guid}", Name = "deletecategoryById"), AllowAnonymous]
            public async Task<IActionResult> Delete([FromRoute] CategoryById categoryById)
            {

                try
                {

                    ServiceResponse<string> response = await categoryService.DeleteAsync(categoryById.id);


                    return response.Code switch
                    {
                        HttpStatusCode.NoContent => NoContent(),
                        HttpStatusCode.NotFound => NotFound(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.BadRequest => BadRequest(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.Conflict => Conflict(mapper.Map<ServiceFailedResponse>(response)),
                        HttpStatusCode.InternalServerError => StatusCode(500, mapper.Map<ServiceFailedResponse>(response)),

                        _ => StatusCode(422, mapper.Map<ServiceFailedResponse>(response)),
                    };


                }
                catch (Exception ex)
                {

                    // _logger.LogError($"GetCategorysById: {ex}");
                    return StatusCode(500, new ServiceFailedResponse(HttpStatusCode.InternalServerError, false, "error getting category this time"));
                }


            }


        }
    
}
