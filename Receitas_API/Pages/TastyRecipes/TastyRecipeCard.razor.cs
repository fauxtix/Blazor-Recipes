using Microsoft.AspNetCore.Components;
using Receitas_API.Models;
using Syncfusion.Blazor.Popups;

namespace Receitas_API.Pages.TastyRecipes
{
    public partial class TastyRecipeCard
    {
        [Parameter, EditorRequired] public Result Recipe { get; set; } = default!;
        [Parameter] public EventCallback<Result> OnSelectedRecipe { get; set; }

        public bool RecipeVisibility { get; set; }
        string video_url;
        string video_title;
        protected bool VideoVisibility { get; set; }
        protected DialogEffect Effects = DialogEffect.SlideTop;

        protected override void OnInitialized()
        {
            RecipeVisibility = false;
        }

        private void ShowDetail()
        {
            OnSelectedRecipe.InvokeAsync(Recipe);
        }

        private void ShowVideo(string url, string title)
        {
            if (!string.IsNullOrEmpty(url))
            {
                video_url = url;
                video_title = title;
                VideoVisibility = true;

            }
        }

        private void CloseVideoDialog()
        {
            video_url = "";
            VideoVisibility = false;
        }
    }
}