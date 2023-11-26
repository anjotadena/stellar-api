using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace API.Controllers;

public class BuggyController : ApiBaseController
{
    public readonly StellarDbContext _context;
    
    public BuggyController(StellarDbContext context)
    {
        _context = context;    
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundResult()
    {
        var thing = _context.ProductBrands.Find(55);

        if (thing is null)
        {
            return NotFound();
        }

        return NotFound();
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        var thing = _context.ProductBrands.Find(55);
        var r = thing.ToString();

        return Ok();   
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest();   
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {
        return Ok();   
    }
}