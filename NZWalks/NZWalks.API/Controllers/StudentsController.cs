using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{   
    //https://localhost:postnumber/api/students 
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET: https://localhost:postnumber/api/students 
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentNames = new string[] { "John", "hung", "Jane", "hehe", "siuuu" };
            return Ok(studentNames);
        }

    }
}
