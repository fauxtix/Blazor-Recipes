using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Receitas_API.Models;
using Receitas_API.Services.Interfaces;
using Syncfusion.Blazor.Spinner;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace Receitas_API.Pages.SpoonacularRecipes
{
    public partial class SpoonacularRecipes_II
    {
        [Inject] public ISpoonacularService service { get; set; }
        protected SfSpinner SpinnerObj;
        protected ILogger<App> logger { get; set; }
        private CountriesCuisines.Root recipesTitles;
        private List<Recipes.MyArray> recipeDetails;
        private CountriesCuisines.Result RecipeDetail;
        private List<Recipes.Step> mainStepsDetails = new List<Recipes.Step>();
        private string RegionName = "French";
        private string spinnerTarget { get; set; } = "#spinnerContainer";


        protected bool RecipeDetailVisibility { get; set; }

        protected override async Task OnInitializedAsync()
        {
            recipesTitles = await GetRecipeTitles();
        }

        private async Task<CountriesCuisines.Root> GetRecipeTitles()
        {
            var output = await service.GetRecipeTitles(RegionName);
            return output;
        }

        private async void GoTop()
        {
            await JS.InvokeVoidAsync("OnScrollEvent");
        }

        protected async Task<List<Recipes.MyArray>> GetRecipeDetails(int Id)
        {

            var output = await service.GetRecipeDetails(Id);
            return output;
        }

        private async Task<RecipeInformation.RecipeInfo> GetRecipeInformation(int Id)
        {
            var output = await service.GetRecipeInformation(Id);
            return output;
        }


        private List<CuisineRegion> RegionsData = new List<CuisineRegion>
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