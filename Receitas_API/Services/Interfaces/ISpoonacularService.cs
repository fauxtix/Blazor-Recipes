using Receitas_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Receitas_API.Services.Interfaces
{
    public interface ISpoonacularService
    {
        Task<List<Recipes.MyArray>> GetRecipeDetails(int Id);
        Task<RecipeInformation.RecipeInfo> GetRecipeInformation(int Id);
        Task<CountriesCuisines.Root> GetRecipeTitles(string regionName);
    }
}