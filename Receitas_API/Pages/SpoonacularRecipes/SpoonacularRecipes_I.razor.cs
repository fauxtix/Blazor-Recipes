using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Receitas_API.Data;
using Receitas_API.Models;
using Syncfusion.Blazor.Spinner;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Receitas_API.Pages.SpoonacularRecipes
{
    public partial class SpoonacularRecipes_I
    {

        protected SfSpinner SpinnerObj;

        private CountriesCuisines.Root recipesTitles;
        private RecipeInformation.RecipeInfo recipeInformation;
        private string RegionName = "French";
        private string spinnerTarget { get; set; } = "#spinnerContainer";

        protected bool AlertVisibility { get; set; }
        protected string searchMessage;

        private readonly HttpClient httpClient = new HttpClient();


        private readonly List<CuisineRegion> RegionsData = new List<CuisineRegion> {
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

        protected override async Task OnInitializedAsync()
        {
            AlertVisibility = false;
            searchMessage = "";
            await GetRecipeTitles();
        }

        private async Task GetRecipeTitles()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<CountriesCuisines.Root>(
                    $"https://api.spoonacular.com/recipes/complexSearch?apiKey=871cc9ddc1ea4733830dd2c30e3d691a&cuisine={RegionName}");

                if (response != null)
                {
                    recipesTitles = response;
                }
                else
                {
                    recipesTitles = null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                recipesTitles = null;
            }
        }

        private async void GoTop()
        {
            await JS.InvokeVoidAsync("OnScrollEvent"); // OnScrollEvent
        }

        private async Task<RecipeInformation.RecipeInfo> GetRecipeInformation(int id)
        {
            return await httpClient.GetFromJsonAsync<RecipeInformation.RecipeInfo>(
                $"https://api.spoonacular.com/recipes/{id}/information?apiKey=871cc9ddc1ea4733830dd2c30e3d691a&includeNutrition=false");
        }

        private async void onChangeRegion(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, CuisineRegion> args)
        {
            recipesTitles = null;
            RegionName = args.Value;
            searchMessage = $"Filtrando dados para região/país '{args.ItemData.RegionName}'. Aguarde, p.f.";
            await GetRecipeTitles();
            AlertVisibility = recipesTitles == null;
            StateHasChanged();
        }

        // private async Task GetRecipeTitles()
        // {
        //     try
        //     {
        //         var client = new RestClient();
        //         var request = new RestRequest("https://api.spoonacular.com/recipes/complexSearch", Method.Get);
        //         request.AddParameter("apiKey", "871cc9ddc1ea4733830dd2c30e3d691a");
        //         request.AddParameter("cuisine", RegionName);
        //         request.AddHeader("Accept", "application/json");
        //         //request.AddHeader("content-type", "application/json");
        //         var response = await client.GetAsync<CountriesCuisines.Root>(request);

        //         if (response != null)
        //         {
        //             var recipeId = response.results.Select(o => o.id).FirstOrDefault();
        //             recipeInformation = await GetRecipeInformation(recipeId);
        //             recipesTitles = response;
        //         }
        //         else
        //         {
        //             recipesTitles = null;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         logger.LogError(ex.Message);
        //         recipesTitles = null;
        //     }
        // }

        // private async Task<RecipeInformation.RecipeInfo> GetRecipeInformation(int Id)
        // {
        //     var client = new RestClient($"https://api.spoonacular.com/");
        //     var request = new RestRequest($"recipes/{Id}/information?includeNutrition=false", Method.Get);
        //     request.AddParameter("apiKey", "871cc9ddc1ea4733830dd2c30e3d691a");
        //     request.AddHeader("Accept", "application/json");
        //     var response = await client.GetAsync<RecipeInformation.RecipeInfo>(request);
        //     return response;
        // }

        // private async void onChangeRegion(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, CuisineRegion> args)
        // {
        //     recipesTitles = null;
        //     RegionName = args.Value;
        //     searchMessage = $"Filtrando dados para região/país '{args.ItemData.RegionName}'. Aguarde, p.f.";
        //     await GetRecipeTitles();
        //     AlertVisibility = recipesTitles == null;
        //     StateHasChanged();
        // }

        private class CuisineRegion
        {
            public string ID { get; set; }
            public string RegionName { get; set; }
        }
    }
}