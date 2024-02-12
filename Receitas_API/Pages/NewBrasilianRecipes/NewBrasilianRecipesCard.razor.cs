using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Receitas_API.Data;
using Receitas_API.Data.BrasilianRecipesService;
using Receitas_API.Models;
using Receitas_API.Pages.SpoonacularRecipes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Receitas_API.Pages.NewBrasilianRecipes
{
    public partial class NewBrasilianRecipesCard
    {
        [Inject ] IRecipeService recipeService { get; set; }
        [Parameter] public BrasilianRecipe.Receita Recipe { get; set; }
        [Parameter] public EventCallback<BrasilianRecipe.Receita> OnSelectedRecipe { get; set; }

        List<string> stepsList = new List<string>();
        protected int RecipeId { get; set; }
        protected ILogger<App> logger { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            RecipeId = Recipe.id;
            Recipe = await recipeService.GetById(RecipeId);

            string[] stepsArray = Recipe.modo_preparo.Split(new string[] { ". " }, StringSplitOptions.RemoveEmptyEntries);

            stepsList = new List<string>(stepsArray);
        }

        private void ShowDetail()
        {
            OnSelectedRecipe.InvokeAsync(Recipe);
        }

    }
}