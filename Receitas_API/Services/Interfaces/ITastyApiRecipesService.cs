using Receitas_API.Models;
using System.Threading.Tasks;

namespace Receitas_API.Services.Interfaces
{
    public interface ITastyApiRecipesService
    {
        Task<TastyRoot> GetRecipes(string query, int recordCount = 100, int option = 0);
        Task<RecipeTags> GetRecipesTags();
        Task<string> Translate(string sourcetext);
    }
}