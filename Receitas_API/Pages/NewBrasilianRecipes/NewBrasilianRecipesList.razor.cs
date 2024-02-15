using Microsoft.AspNetCore.Components;
using Receitas_API.Models;
using Receitas_API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Receitas_API.Pages.NewBrasilianRecipes;

public partial class NewBrasilianRecipesList
{
    [Inject] public IBrasilianRecipesIIService RecipeService { get; set;} 
    protected IEnumerable<BrasilianRecipe.Receita> Recipes { get; set; }
    private BrasilianRecipe.Receita RecipeDetail;
    protected bool RecipeDetailVisibility { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Recipes = await RecipeService.GetAllRecipes();
    }

    protected void HandleSelectedRecipe(BrasilianRecipe.Receita recipe)
    {
        RecipeDetailVisibility = true;
        RecipeDetail = recipe;

    }
}