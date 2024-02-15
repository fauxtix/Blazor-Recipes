using Microsoft.JSInterop;
using Receitas_API.Models;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.Spinner;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Receitas_API.Pages.TastyRecipes
{
    public partial class Tasty_Recipes
    {
        public TastyRoot recipesList { get; set; } = new();
        public RecipeTags tagsData { get; set; }
        public List<TagItem> tagsList { get; set; }
        public Result RecipeDetail { get; set; } = new();

        public int MaxResults { get; set; } = 40;
        public string searchQuery { get; set; }
        protected int TagId { get; set; }

        private string spinnerLabel { get; set; }
        private bool VisibleProperty { get; set; } = false;

        private int recipesCount = 0;
        protected SfSpinner SpinnerObj { get; set; }
        protected DialogEffect Effects = DialogEffect.SlideTop;

        SfButton searchBtn;
        SfTextBox searchTxtBox;
        private string text2Search { get; set; }

        string video_url;
        string video_title;
        protected bool VideoVisibility { get; set; }
        protected bool RecipeDetailVisibility { get; set; }
        protected bool AlertMessageVisibility { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tagsData = await GetTags_Data();

            tagsList = await FillTagItems();
            AlertMessageVisibility = false;
            RecipeDetailVisibility = false;
        }

        private async Task onChangeTag(Syncfusion.Blazor.DropDowns.ChangeEventArgs<int, TagItem> args)
        {
            TagId = args.Value;
            if (TagId > 0)
            {
                text2Search = "";
                searchQuery = tagsData.results.SingleOrDefault(p => p.id == TagId).name;

                recipesList = await GetRecipes_Data(0);
            }
        }

        private async Task<TastyRoot> GetRecipes_Data(int opcao, string label = "Lendo receitas...")
        {
            TastyRoot recipesList_Temp = new();
            spinnerLabel = label;
            await Task.Delay(200);
            await SpinnerObj.ShowAsync();

            recipesList_Temp = await recipeService.GetRecipes(searchQuery, MaxResults, opcao);

            recipesCount = recipesList_Temp.results.Count();
            await SpinnerObj.HideAsync();

            await InvokeAsync(StateHasChanged);
            return recipesList_Temp;

        }

        private async Task<RecipeTags> GetTags_Data()
        {
            RecipeTags tagsData_Temp = new();
            tagsData_Temp = await recipeService.GetRecipesTags();
            if(tagsData is null)
            {
                AlertMessageVisibility = true;
                return new RecipeTags();
            }
            StateHasChanged();

            return tagsData_Temp;
        }

        private async Task<List<TagItem>> FillTagItems()
        {
            try
            {
                if (tagsData.count == 0)
                {
                    AlertMessageVisibility = true;
                    return new List<TagItem>();
                }

                var list = tagsData.results.ToList();

                List<TagItem> returnList = new();

                foreach (var item in list)
                {
                    returnList.Add(new TagItem()
                    {
                        Id = item.id,
                        DisplayName = item.display_name
                    });
                }

                List<TagItem> SortedList = returnList.OrderBy(o => o.DisplayName).ToList();
                TagId = SortedList.FirstOrDefault().Id;
                searchQuery = tagsData.results.SingleOrDefault(p => p.id == TagId).name;

                recipesList = await GetRecipes_Data(0);

                return SortedList;

            }
            catch (System.Exception ex)
            {
                var x = ex.Message;
                throw;
            }
        }

        private void ShowVideo(string url, string title)
        {
            if (!string.IsNullOrEmpty(url))
            {
                video_url = url;
                video_title = title;
                VideoVisibility = true;
                AlertMessageVisibility = false;

            }
            else
            {
                AlertMessageVisibility = true;
            }
        }

        private void CloseVideoDialog()
        {
            video_url = "";
            VideoVisibility = false;
        }

        private async Task SearchByRecipeName()
        {
            TagId = 0;
            searchQuery = searchTxtBox.Value;
            VisibleProperty = false;
            await Task.Delay(200);
            recipesList = await GetRecipes_Data(1, "Pesquisando texto nas receitas"); // search option = 'q' (1)
            recipesCount = recipesList.count;
            VisibleProperty = true;

        }

        private async void GoTop()
        {
            await JS.InvokeVoidAsync("OnScrollEvent");
        }
        public class TagItem
        {
            public int Id { get; set; }
            public string DisplayName { get; set; }
        }

        protected void HandleSelectedRecipe(Result recipe)
        {
            RecipeDetailVisibility = true;
            RecipeDetail = recipe;
        }

    }
}