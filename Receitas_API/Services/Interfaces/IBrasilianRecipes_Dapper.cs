using Receitas_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Receitas_API.Services.Interfaces
{
    public interface IBrasilianRecipes_Dapper
    {
        Task CreateDbDataFromApi();
        Task Update_RecipeTable();

        int Insert_Recipe(RecipeDbModel.Recipe recipe);
        void Update_Recipe(RecipeDbModel.Recipe recipe);

        Task<List<RecipeDbModel.Recipe>> GetAllRecipes();

        RecipeDbModel.Recipe GetRecipeById(int Id);

        List<RecipeDbModel.Ingredient> GetIngredientsById(int Id);
        List<RecipeDbModel.Preparation> GetPreparationById(int Id);
        List<RecipeDbModel.Other> GetOtherById(int Id);
        void Delete_Recipe(int id);
        Task<IEnumerable<BrasilianEqualNames>> GetEqualRecipesNames();
    }
}