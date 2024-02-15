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
        private readonly string apiKey = "871cc9ddc1ea4733830dd2c30e3d691a";

        private readonly List<CuisineRegion> RegionsData = new List<CuisineRegion>
        {
            new() { ID= "German", RegionName = "Alemã" },
            new() { ID= "American", RegionName= "Americana" },
            new() { ID= "Latin American", RegionName = "América Latina" },
            new() { ID= "British", RegionName= "Britânica" },
            new() { ID= "Cajun", RegionName= "Cajun" },
            new() { ID= "Caribbean", RegionName= "Caraíbas" },
            new() { ID= "Korean", RegionName = "Coreana" },
            new() { ID= "Spanish", RegionName = "Espanhola"},
            new() { ID= "Eastern European", RegionName= "Europa de Leste" },
            new() { ID= "French", RegionName = "Francesa" },
            new() { ID= "Greek", RegionName = "Grega"},
            new() { ID= "Indian", RegionName = "Indiana" },
            new() { ID= "Irish", RegionName = "Irlandesa"},
            new() { ID= "Italian", RegionName = "Italiana" },
            new() { ID= "Japanese", RegionName = "Japonesa" },
            new() { ID= "Jewish", RegionName = "Judeia" },
            new() { ID= "Mediterranean", RegionName = "Mediterrânica" },
            new() { ID= "Mexican", RegionName = "Mexicana" },
            new() { ID= "Middle Eastern", RegionName = "Médio Oriente"},
            new() { ID= "Nordic", RegionName = "Nórdica" },
            new() { ID= "Southern", RegionName = "Sulista"},
            new() { ID= "Thai", RegionName = "Thai"},
            new() { ID= "Vietnamese", RegionName= "Vietnamita"}
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
                    $"https://api.spoonacular.com/recipes/complexSearch?apiKey={apiKey}&cuisine={RegionName}");

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
                $"https://api.spoonacular.com/recipes/{id}/information?apiKey={apiKey}&includeNutrition=false");
        }

        private async void OnChangeRegion(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, CuisineRegion> args)
        {
            recipesTitles = null;
            RegionName = args.Value;
            searchMessage = $"Filtrando dados para região/país '{args.ItemData.RegionName}'. Aguarde, p.f.";
            await GetRecipeTitles();
            AlertVisibility = recipesTitles == null;
            StateHasChanged();
        }
    }
}