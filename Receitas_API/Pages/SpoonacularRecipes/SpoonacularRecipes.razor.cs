using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Receitas_API.Data;
using Receitas_API.Models;
using Syncfusion.Blazor.Spinner;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Receitas_API.Pages.SpoonacularRecipes
{
    public partial class SpoonacularRecipes
    {

        protected SfSpinner SpinnerObj;
        protected ILogger<App> logger { get; set; }
        private CountriesCuisines.Root recipesTitles;
        private List<Recipes.MyArray> recipeDetails;
        private CountriesCuisines.Result RecipeDetail;
        private List<string> mainStepNames = new List<string>();
        private List<Recipes.Step> mainStepsDetails = new List<Recipes.Step>();
        //private int recipeId;
        private string RegionName = "French";
        private string spinnerTarget { get; set; } = "#spinnerContainer";

        private readonly HttpClient httpClient = new HttpClient();
        private readonly string apiKey = "871cc9ddc1ea4733830dd2c30e3d691a";

        protected bool RecipeDetailVisibility { get; set; }

        protected override async Task OnInitializedAsync()
        {
            recipesTitles = await GetRecipeTitles();
        }

        private async Task<CountriesCuisines.Root> GetRecipeTitles()
        {
            string baseUrl = "https://api.spoonacular.com/recipes/complexSearch";
            string cuisineParam = $"cuisine={RegionName}";
            string url = $"{baseUrl}?apiKey={apiKey}&{cuisineParam}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<CountriesCuisines.Root>();
        }

        private async void GoTop()
        {
            await JS.InvokeVoidAsync("OnScrollEvent"); // OnScrollEvent
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

        private async Task<RecipeInformation.RecipeInfo> GetRecipeInformation(int Id)
        {
            string baseUrl = $"https://api.spoonacular.com/recipes/{Id}/information";
            string nutritionParam = "includeNutrition=false";
            string url = $"{baseUrl}?{nutritionParam}&apiKey={apiKey}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<RecipeInformation.RecipeInfo>();
        }
        private class CuisineRegion
        {
            public string ID { get; set; }
            public string RegionName { get; set; }
        }

        private List<CuisineRegion> RegionsData = new List<CuisineRegion> {
        new CuisineRegion() { ID= "German", RegionName = "Alemã" },
        new CuisineRegion() { ID= "American", RegionName= "Americana" },
        new CuisineRegion() { ID= "Latin American", RegionName = "América Latina" },
        new CuisineRegion() { ID= "British", RegionName= "Britânica" },
        new CuisineRegion() { ID= "Cajun", RegionName= "Cajun" },
        new CuisineRegion() { ID= "Caribbean", RegionName= "Caraíbas" },
        new CuisineRegion() { ID= "Korean", RegionName = "Coreana" },
        new CuisineRegion() { ID= "Spanish", RegionName = "Espanhola"},
        new CuisineRegion() { ID= "Eastern European", RegionName= "Europa de Leste" },
        new CuisineRegion() { ID= "French", RegionName = "Francesa" },
        new CuisineRegion() { ID= "Greek", RegionName = "Grega"},
        new CuisineRegion() { ID= "Indian", RegionName = "Indiana" },
        new CuisineRegion() { ID= "Irish", RegionName = "Irlandesa"},
        new CuisineRegion() { ID= "Italian", RegionName = "Italiana" },
        new CuisineRegion() { ID= "Japanese", RegionName = "Japonesa" },
        new CuisineRegion() { ID= "Jewish", RegionName = "Judeia" },
        new CuisineRegion() { ID= "Mediterranean", RegionName = "Mediterrânica" },
        new CuisineRegion() { ID= "Mexican", RegionName = "Mexicana" },
        new CuisineRegion() { ID= "Middle Eastern", RegionName = "Médio Oriente"},
        new CuisineRegion() { ID= "Nordic", RegionName = "Nórdica" },
        new CuisineRegion() { ID= "Southern", RegionName = "Sulista"},
        new CuisineRegion() { ID= "Thai", RegionName = "Thai"},
        new CuisineRegion() { ID= "Vietnamese", RegionName= "Vietnamita"}
    };

        private async void onChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, CuisineRegion> args)
        {
            RegionName = args.Value;
            await Task.Delay(200);
            await SpinnerObj.ShowAsync();
            recipesTitles = await GetRecipeTitles();
            await SpinnerObj.HideAsync();
            StateHasChanged();
        }

        protected void HandleSelectedRecipe(CountriesCuisines.Result recipe)
        {
            RecipeDetailVisibility = true;
            RecipeDetail = recipe;
        }

    }
}