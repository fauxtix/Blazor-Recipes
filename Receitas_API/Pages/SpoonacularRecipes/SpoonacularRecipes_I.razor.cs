using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Receitas_API.Data;
using Receitas_API.Models;
using Receitas_API.Services.Interfaces;
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

        [Inject] public ISpoonacularService service { get; set; }


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
            recipesTitles = await GetRecipeTitles();
        }

        private async Task<CountriesCuisines.Root> GetRecipeTitles()
        {
            var output = await service.GetRecipeTitles(RegionName);
            return output;
        }

        private async void GoTop()
        {
            await JS.InvokeVoidAsync("OnScrollEvent"); // OnScrollEvent
        }

        private async Task<RecipeInformation.RecipeInfo> GetRecipeInformation(int Id)
        {

            var output = await service.GetRecipeInformation(Id);
            return output;
        }

        private async void OnChangeRegion(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, CuisineRegion> args)
        {
            recipesTitles = null;
            RegionName = args.Value;
            searchMessage = $"Filtrando dados para região/país '{args.ItemData.RegionName}'. Aguarde, p.f.";
            recipesTitles= await GetRecipeTitles();
            AlertVisibility = recipesTitles == null;
            StateHasChanged();
        }
    }
}