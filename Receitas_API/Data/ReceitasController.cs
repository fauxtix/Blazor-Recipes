using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Receitas_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receitas_API.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitasController : ControllerBase
    {
        private List<Receita> receitas = new List<Receita>();
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReceitasController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult<List<Receita>> GetReceitas()
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;

            try
            {
                var file = System.IO.File.ReadAllText(contentRootPath + @"/Data/afrodite.json");
                var output = JsonConvert.DeserializeObject<List<Receita>>(file).ToList();
                return output;
            }
            catch (System.Exception ex)
            {
                throw new ApplicationException($"Erro no controller:\r\n{ex.Message}");
            }
        }
    }
}
