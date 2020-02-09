using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyAPICore.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : MainController
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var values = new string[] { "value1", "value2" };

            if (values.Length < 5000)
                return BadRequest();

            return Ok(values);
        }

        // GET api/values/get-result
        [HttpGet("get-result")]
        public ActionResult GetResult()
        {
            var values = new string[] { "value1", "value2" };

            if (values.Length < 5000)
                return CustomResponse();

            return CustomResponse(values);
        }

        // GET api/values/get-values
        [HttpGet("get-values")]
        public IEnumerable<string> GetValues()
        {
            var values = new string[] { "value1", "value2" };

            if (values.Length < 5000)
                return null;

            return values;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        //[HttpGet]
        //[Route("{id:int}")]
        //[HttpGet, Route("{id:int}")]
        //[HttpGet("get-by-id/{id:int}")] api/values/get-by-id/5
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Post(Product product)
        {
            if (product.Id == 0) return BadRequest();

            // add database

            return CreatedAtAction("Post", product);
        }

        // POST api/values/product/from-query?id=10&name=Name&description=Description
        [HttpPost("product/from-query")]
        public ActionResult<Product> Post([FromQuery] int id, [FromQuery] string name, [FromQuery] string description)
        {
            if (id == 0) return BadRequest();
            if (name.Length < 5) return BadRequest();
            if (description.Length < 10) return BadRequest();

            var product = new Product
            {
                Id = id,
                Name = name,
                Description = description
            };

            return Ok(product);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] string value)
        {
        }

        // PUT api/values/product/5
        [HttpPut("product/{id:int}")]
        public void Put([FromRoute] int id, [FromForm] Product product)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = GetErrors()
            });
        }

        public bool ValidOperation()
        {
            return true;
        }

        protected string GetErrors()
        {
            return "";
        }
    }

    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
