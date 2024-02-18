using Dapper;
using Microsoft.Extensions.Logging;
using Receitas_API.Data;
using Receitas_API.Models;
using Receitas_API.Services.Interfaces;
using Receitas_API.Services.Interfaces.DapperContext;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Receitas_API.Services.Implementations
{
    public class BrasilianRecipes_Dapper : IBrasilianRecipes_Dapper
    {
        private readonly IDapperContext _context;
        private readonly ILogger<BrasilianRecipes_Dapper> _logger;
        public BrasilianRecipes_Dapper(ILogger<BrasilianRecipes_Dapper> logger, IDapperContext context)
        {
            _logger = logger;
            _context = context;
        }
        public RecipeDbModel.Recipe GetRecipeById(int Id)
        {
            var p = new DynamicParameters();
            p.Add("RecipeID", Id, dbType: DbType.Int32);
            using (var conn = _context.CreateConnection())
            {
                var output = conn.Query<RecipeDbModel.Recipe>("usp_GetRecipe_ById", p, commandType: CommandType.StoredProcedure);
                return output.SingleOrDefault();
            }
        }

        public List<RecipeDbModel.Ingredient> GetIngredientsById(int Id)
        {
            var p = new DynamicParameters();
            p.Add("RecipeID", Id, dbType: DbType.Int32);
            using (var conn = _context.CreateConnection())
            {
                var output = conn.Query<RecipeDbModel.Ingredient>("usp_GetIngredients_ByRecipeId", p, commandType: CommandType.StoredProcedure);
                return output.ToList();
            }
        }

        public List<RecipeDbModel.Preparation> GetPreparationById(int Id)
        {
            var p = new DynamicParameters();
            p.Add("RecipeID", Id, dbType: DbType.Int32);
            using (var conn = _context.CreateConnection())
            {
                var output = conn.Query<RecipeDbModel.Preparation>("usp_GetPreparation_ByRecipeId", p, commandType: CommandType.StoredProcedure);
                return output.ToList();
            }
        }

        public List<RecipeDbModel.Other> GetOtherById(int Id)
        {
            var p = new DynamicParameters();
            p.Add("RecipeID", Id, dbType: DbType.Int32);
            using (var conn = _context.CreateConnection())
            {
                var output = conn.Query<RecipeDbModel.Other>("usp_GetOther_ByRecipeId", p, commandType: CommandType.StoredProcedure);
                return output.ToList();
            }
        }
        public async Task<List<RecipeDbModel.Recipe>> GetAllRecipes()
        {
            try
            {
                using (var conn = _context.CreateConnection())
                {
                    var results = await conn.QueryAsync<RecipeDbModel.Recipe>("usp_GetAllRecipes",
                        commandType: CommandType.StoredProcedure);

                    return results.ToList();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new List<RecipeDbModel.Recipe>();
            }
        }

        public int Insert_Recipe(RecipeDbModel.Recipe recipe)
        {
            int PkIdValue;

            var p = new DynamicParameters();
            p.Add("Description", recipe.Description);
            p.Add("RecipeID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using (var conn = _context.CreateConnection())
            {
                conn.Open();
                try
                {
                    conn.Query<int>("usp_Insert_Recipe", p, commandType: CommandType.StoredProcedure);
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

        public void Update_Recipe(RecipeDbModel.Recipe recipe)
        {

            var p = new DynamicParameters();
            p.Add("Description", recipe.Description);
            p.Add("Id", recipe.Id, dbType: DbType.Int32);

            using (var conn = _context.CreateConnection())
            {
                try
                {
                    conn.Execute("usp_Update_Recipe", p, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
        public void Delete_Recipe(int id)
        {

            var p = new DynamicParameters();
            p.Add("Id", id, dbType: DbType.Int32);

            using (var conn = _context.CreateConnection())
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        conn.Execute("usp_Delete_Other", p, commandType: CommandType.StoredProcedure, transaction: tran);
                        conn.Execute("usp_Delete_Preparation", p, commandType: CommandType.StoredProcedure, transaction: tran);
                        conn.Execute("usp_Delete_Ingredient", p, commandType: CommandType.StoredProcedure, transaction: tran);
                        conn.Execute("usp_Delete_Recipe", p, commandType: CommandType.StoredProcedure, transaction: tran);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        _logger.LogError(ex.Message);
                    }
                }
            }
        }

        public async Task CreateDbDataFromApi()
        {
            RestClient client = new RestClient("https://localhost:44355/api/");  // Recipes API
            var request = new RestRequest("receitas", method: Method.Get);
            request.AddHeader("Accept", "application/json");
            var response = (await client.GetAsync<List<Receita>>(request)).ToList();
            var OrderedRecipes = response.OrderBy(p => p.Nome).ToList();
            var spName = "";
            foreach (var recipe in OrderedRecipes)
            {
                if (recipe.Secao[0].Conteudo.Count == 0)
                    continue;

                int PkIdValue;
                string Description = recipe.Nome;

                var p = new DynamicParameters();
                p.Add("Description", recipe.Nome);
                p.Add("RecipeID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var conn = _context.CreateConnection())
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        await conn.QueryAsync<int>("usp_Insert_Recipe", p, commandType: CommandType.StoredProcedure, transaction: transaction);
                        PkIdValue = p.Get<int>("RecipeID");

                        foreach (var operacao in recipe.Secao)
                        {
                            // Ingredientes, Modo de Preparo, ...
                            var opName = operacao.Nome.Trim();
                            var conteudo = operacao.Conteudo.ToList();
                            if (!string.IsNullOrEmpty(opName) && operacao.Conteudo.Count > 0)
                            {
                                if (opName == "Ingredientes")
                                    spName = "usp_Insert_Ingredient";
                                else if (opName == "Modo de Preparo")
                                    spName = "usp_Insert_Preparation";
                                else
                                    spName = "usp_Insert_Other";
                            }
                            foreach (var descricao in conteudo)
                            {
                                var par = new DynamicParameters();
                                par.Add("Description", descricao);
                                par.Add("RecipeID", PkIdValue, dbType: DbType.Int32);

                                await conn.ExecuteAsync(spName, par, commandType: CommandType.StoredProcedure, transaction: transaction);
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task Update_RecipeTable()
        {
            var allRecipes = await GetAllRecipes();
            foreach (var recipe in allRecipes)
            {
                var recipeID = recipe.Id;
                var ingredients = GetIngredientsById(recipeID);
                var preparation = GetPreparationById(recipeID);
                var other = GetOtherById(recipeID);
                StringBuilder sbIngredients = new();
                StringBuilder sbPreparation = new();
                StringBuilder sbOther = new();
                if (ingredients != null && ingredients.Any())
                {
                    foreach (var ingredient in ingredients)
                    {
                        sbIngredients.AppendLine(ingredient.Description);
                    }
                }
                if (preparation != null && preparation.Count() > 0)
                {
                    foreach (var prep in preparation)
                    {
                        sbPreparation.AppendLine(prep.Description);
                    }
                }
                if (other != null && other.Count() > 0)
                {
                    foreach (var _other in other)
                    {
                        sbOther.AppendLine(_other.Description);
                    }
                }

                StringBuilder sbUpdate = new StringBuilder();
                sbUpdate.Append("UPDATE Recipe ");
                sbUpdate.Append($"SET Ingredients = '{sbIngredients.ToString()}', ");
                sbUpdate.Append($"Preparation = '{sbPreparation.ToString()}', ");
                sbUpdate.Append($"Other = '{sbOther.ToString()}' ");
                sbUpdate.Append($"WHERE Id = {recipeID}");
                using (var conn = _context.CreateConnection())
                {
                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        await conn.ExecuteAsync(sbUpdate.ToString(), transaction: transaction);
                        transaction.Commit();
                        Console.WriteLine(recipeID);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<IEnumerable<BrasilianEqualNames>> GetEqualRecipesNames()
        {
            using (var conn = _context.CreateConnection())
            {
                try
                {
                    var result = await conn.QueryAsync<BrasilianEqualNames>("usp_GetRecipesSameName", commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }
        public async Task<IEnumerable<RecipeDbModel.Recipe>> GetRecipesWithNoIngredients()
        {
            using (var conn = _context.CreateConnection())
            {
                try
                {
                    var result = await conn.QueryAsync<RecipeDbModel.Recipe>("GetRecipesWithNoIngredients", commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<IEnumerable<RecipeDbModel.Recipe>> GetRecipesWithNoPreparation()
        {
            using (var conn = _context.CreateConnection())
            {
                try
                {
                    var result = await conn.QueryAsync<RecipeDbModel.Recipe>("GetRecipesWithNoPreparation", commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

        public async Task<IEnumerable<RecipeDbModel.Recipe>> GetRecipesWithNoPreparationNorIngredients()
        {
            using (var conn = _context.CreateConnection())
            {
                try
                {
                    var result = await conn.QueryAsync<RecipeDbModel.Recipe>("GetRecipesWithNoPreparationAndNoIngredients", commandType: CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
        }

    }
}