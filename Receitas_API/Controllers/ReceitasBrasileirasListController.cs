using Microsoft.AspNetCore.Mvc;
using Receitas_API.Data;
using Receitas_API.Models;
using System.Threading.Tasks;


namespace Receitas_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitasBrasileirasListController : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<Receita> GetData(string action, string id)
        {
            CustomHttpClient client = new CustomHttpClient();
            return await client.GetJsonAsync<Receita>("afrodite.json");
        }
    }
}
