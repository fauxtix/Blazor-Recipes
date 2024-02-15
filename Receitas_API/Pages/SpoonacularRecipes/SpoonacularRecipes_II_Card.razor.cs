using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Receitas_API.Data;
using Receitas_API.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Receitas_API.Pages.SpoonacularRecipes
{
    public partial class SpoonacularRecipes_II_Card
    {
        [Parameter] public CountriesCuisines.Result SpoonacularRecipe { get; set; }
        [Parameter] public EventCallback<CountriesCuisines.Result> OnSelectedRecipe { get; set; }

        protected int RecipeId { get; set; }
        protected ILogger<App> logger { get; set; }

        //private List<Recipes.MyArray> RecipeDetails { get; set; } = new();
        private RecipeInformation.RecipeInfo RecipeInformation { get; set; } = new();

        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiKey = "871cc9ddc1ea4733830dd2c30e3d691a";

        protected override async Task OnParametersSetAsync()
        {
            RecipeId = SpoonacularRecipe.id;
            RecipeInformation = await GetRecipeInformation(RecipeId);

            //RecipeDetails = await GetRecipeDetails(RecipeId);
        }

        //protected async Task<List<Recipes.MyArray>> GetRecipeDetails(int Id)
        //{
        //    string baseUrl = $"https://api.spoonacular.com/recipes/{Id}/analyzedInstructions";
        //    string url = $"{baseUrl}?apiKey={apiKey}";

        //    try
        //    {
        //        var response = await httpClient.GetAsync(url);
        //        response.EnsureSuccessStatusCode();
        //        var result = await response.Content.ReadAsAsync<List<Recipes.MyArray>>();

        //        return result;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        logger.LogError(ex.Message);
        //        return new List<Recipes.MyArray>();
        //    }
        //}
        private async Task<RecipeInformation.RecipeInfo> GetRecipeInformation(int Id)
        {
            string baseUrl = $"https://api.spoonacular.com/recipes/{Id}/information";
            string nutritionParam = "includeNutrition=false";
            string url = $"{baseUrl}?{nutritionParam}&apiKey={apiKey}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<RecipeInformation.RecipeInfo>();
        }

        private void ShowDetail()
        {
            OnSelectedRecipe.InvokeAsync(SpoonacularRecipe);
        }

    }
}