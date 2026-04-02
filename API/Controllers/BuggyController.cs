using System;
using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public ActionResult GetUnauthorized ()
    {
        return Unauthorized();
    }
    [HttpGet("badrequest")]
    public ActionResult GetBadRequest ()
    {
        return BadRequest("This is a bad request");
    }
    [HttpGet("notfound")]
    public ActionResult GetNotFound ()
    {
        return NotFound();
    }
    [HttpGet("internalerror")]
    public ActionResult GetInternalServerError ()
    {
        throw new Exception("This is an internal server error");
    }
    [HttpPost("validationerror")]
    public ActionResult GetValidationError (CreateProductDto  product)
    {
       return Ok();
    }
}

