using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("errors/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ApiBaseController
{
    public IActionResult Error(int code)
    {
        Console.WriteLine("ERROR REDIRECT!");
        Console.WriteLine(code);
        return new ObjectResult(new ApiResponse(code)); 
    }
}