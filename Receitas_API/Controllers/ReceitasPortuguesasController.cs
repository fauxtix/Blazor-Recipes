﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace Receitas_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitasPortuguesasController : ControllerBase
    {

        public ReceitasPortuguesasController()
        {
            
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
