using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ErrorController : ApiBaseController
{
    [Route("errors/{code}")]
    public IActionResult Error(int code)
    {
        return new ObjectResult(new ApiResponse(code)); 
    }
}