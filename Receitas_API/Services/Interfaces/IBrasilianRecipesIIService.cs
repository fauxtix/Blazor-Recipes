using Receitas_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Receitas_API.Services.Interfaces
{
    public interface IBrasilianRecipesIIService
    {
        Task<IEnumerable<BrasilianRecipe.Receita>> GetAllRecipes();
        Task<BrasilianRecipe.Receita> GetById(int Id);
        Task<BrasilianRecipe.Receita> GetRecipes(int Id = 0, int option = 0);
    }
}