using KooliProjekt.Application.Infrastructure.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public abstract class ApiControllerBase : Controller
    {
        private static readonly JsonSerializerSettings _serializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore                
            };

        protected IActionResult Result(OperationResult result)
        {
            if (result == null)
            {
                return NotFound();
            }

            return SerializedResult(result);
        }

        protected IActionResult Result<T>(OperationResult<T> result)
        {
            if (result == null || (!result.HasErrors && result.Value == null))
            {
                return NotFound();
            }

            return SerializedResult(result);
        }

        private IActionResult SerializedResult(OperationResult result)
        {
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result, _serializerSettings),
                ContentType = "application/json",
                StatusCode = result.HasErrors
                    ? StatusCodes.Status400BadRequest
                    : StatusCodes.Status200OK
            };
        }
    }
}
