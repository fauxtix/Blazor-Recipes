using Dapper;
using Microsoft.Extensions.Logging;
using Receitas_API.Models;
using Receitas_API.Services.Interfaces.DapperContext;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Receitas_API.Services.Interfaces;
using static Receitas_API.Models.RecipeDbModel;

namespace Receitas_API.Services.Implementations
{
    public class MyRecipesService : IMyRecipesService
    {
        private readonly IDapperContext _context;
        private readonly ILogger<MyRecipesService> _logger;

        public MyRecipesService(IDapperContext context, ILogger<MyRecipesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MyRecipe> GetRecipeById(int Id)
        {
            var p = new DynamicParameters();
            p.Add("RecipeID", Id, dbType: DbType.Int32);
            using (var conn = _context.CreateConnection())
            {
                var output = await conn.QueryFirstOrDefaultAsync<MyRecipe>("usp_GetMyRecipe_ById",
                    p,
                    commandType: CommandType.StoredProcedure);
                return output;
            }
        }

        public async Task<IEnumerable<MyRecipe>> GetAllRecipes()
        {
            try
            {
                using (var conn = _context.CreateConnection())
                {
                    var results = await conn.QueryAsync<MyRecipe>("usp_GetAll_MyRecipes",
                        commandType: CommandType.StoredProcedure);

                    return results.ToList();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return Enumerable.Empty<MyRecipe>();
            }
        }

        public async Task<int> Insert_Recipe(MyRecipe recipe)
        {
            int PkIdValue;

            var p = new DynamicParameters();
            p.Add("@Title", recipe.Title);
            p.Add("@Ingredients", recipe.Ingredients);
            p.Add("@Preparation", recipe.Preparation);
            p.Add("@OtherInfo", recipe.OtherInfo);
            p.Add("@RecipeUrl", recipe.RecipeUrl);
            p.Add("@ImageUrl", recipe.ImageUrl);
            p.Add("RecipeID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var conn = _context.CreateConnection())
            {
                conn.Open();
                try
                {
                    await conn.QueryAsync<int>("usp_Insert_MyRecipe", p, commandType: CommandType.StoredProcedure);
                    PkIdValue = p.Get<int>("RecipeID");
                    return PkIdValue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return 0;
                }
            }
        }

        public async Task Update_Recipe(MyRecipe recipe)
        {

            var p = new DynamicParameters();

            p.Add("@RecipeId", recipe.Id);
            p.Add("@Title", recipe.Title);
            p.Add("@Ingredients", recipe.Ingredients);
            p.Add("@Preparation", recipe.Preparation);
            p.Add("@OtherInfo", recipe.OtherInfo);
            p.Add("@RecipeUrl", recipe.RecipeUrl);
            p.Add("@ImageUrl", recipe.ImageUrl);

            using (var conn = _context.CreateConnection())
            {
                try
                {
                    await conn.ExecuteAsync("usp_Update_MyRecipe", p, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task Delete_Recipe(int id)
        {

            var p = new DynamicParameters();
            p.Add("@RecipeId", id, dbType: DbType.Int32);

            using (var conn = _context.CreateConnection())
            {
                try
                {
                    await conn.ExecuteAsync("usp_Delete_MyRecipe",
                        p, commandType:
                        CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}

