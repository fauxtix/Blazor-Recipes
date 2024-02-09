using Microsoft.AspNetCore.Mvc;
using Receitas_API.Models;
using System.Threading.Tasks;


namespace Receitas_API.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeListController : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<Receita> GetData(string action, string id)
        {
            CustomHttpClient client = new CustomHttpClient();
            return await client.GetJsonAsync<Receita>("afrodite.json");
        }
    }
}
