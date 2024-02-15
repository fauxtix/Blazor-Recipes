using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Receitas_API.Data;
using Receitas_API.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Receitas_API.Pages.SpoonacularRecipes
{
    public partial class RecipeDetail
    {
        [Parameter] public CountriesCuisines.Result Recipe { get; set; }

        private List<Recipes.MyArray> recipeDetails;
        private List<Recipes.Step> mainStepsDetails = new List<Recipes.Step>();

        private int recipeId;

        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiKey = "871cc9ddc1ea4733830dd2c30e3d691a";
        protected ILogger<App> logger { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            recipeId = Recipe.id;
            recipeDetails = await GetRecipeDetails(recipeId);
        }

        protected async Task<List<Recipes.MyArray>> GetRecipeDetails(int Id)
        {
            string baseUrl = $"https://api.spoonacular.com/recipes/{Id}/analyzedInstructions";
            string url = $"{baseUrl}?apiKey={apiKey}";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<List<Recipes.MyArray>>();

                return result;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message);
                return new List<Recipes.MyArray>();
            }
        }

    }
}