using Microsoft.AspNetCore.Components;
using Receitas_API.Models;

namespace Receitas_API.Pages.TastyRecipes
{
    public partial class RecipeDetail
    {
        [Parameter] public Result item { get; set; }

        private bool Visibility { get; set; } = false;

        string video_url;
        string video_title;
        protected bool VideoVisibility { get; set; }


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