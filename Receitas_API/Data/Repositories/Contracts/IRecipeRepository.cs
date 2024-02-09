using Receitas_API.Data.Repositories.Base;
using static Receitas_API.Models.RecipeDbModel;

namespace Repository.Contracts
{
    public interface IRecipeRepository : IBaseRepository<Recipe>
    {
    }
}