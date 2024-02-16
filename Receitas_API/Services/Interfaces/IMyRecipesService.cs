using Receitas_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Receitas_API.Services.Interfaces
{
    public interface IMyRecipesService
    {
        Task Delete_Recipe(int id);
        Task<IEnumerable<MyRecipe>> GetAllRecipes();
        Task<MyRecipe> GetRecipeById(int Id);
        Task<int> Insert_Recipe(MyRecipe recipe);
        Task Update_Recipe(MyRecipe recipe);
    }
}