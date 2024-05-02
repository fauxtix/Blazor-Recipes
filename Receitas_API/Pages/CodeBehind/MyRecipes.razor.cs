using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Receitas_API.Models;
using Receitas_API.Services.Interfaces;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.Spinner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Receitas_API.Pages.CodeBehind
{
    public class MyRecipesBase : ComponentBase
    {
        [Inject] public IMyRecipesService MyRecipesService { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Inject] public ILogger<App> Logger { get; set; }

        protected IEnumerable<MyRecipe> PersonalRecipes { get; set; }

        protected MyRecipe SelectedRecipe { get; set; }
        protected bool EditMode = false;

        protected bool EditDialogVisibility { get; set; } = false;
        protected bool ShowDetailDialogVisibility { get; set; } = false;
        protected bool ValidationErrorsVisibility { get; set; } = false;
        protected bool DeleteVisibility { get; set; } = false;
        protected int RecipeId { get; set; }
        protected string DeleteCaption;

        protected SfSpinner SpinnerObj;

        protected SfGrid<MyRecipe> gridObj;

        protected SfToast ToastObj;
        protected DialogEffect Effects = DialogEffect.Zoom;

        protected string ToastTitle = "";
        protected string ToastContent = "";
        protected string ToastCssClass = "";

        protected bool WarningVisibility = false;


        protected override async Task OnInitializedAsync()
        {
            DeleteCaption = "";
            PersonalRecipes = await GetAllRecipes();
        }

        protected async Task<IEnumerable<MyRecipe>> GetAllRecipes()
        {
            WarningVisibility = false;
            var res =  (await MyRecipesService.GetAllRecipes()).ToList();
            if (res.Any())
                return res;
            else
            {
                WarningVisibility = true;
                return Enumerable.Empty<MyRecipe>();
            }
        }

        protected void AddRecipe()
        {
            SelectedRecipe = new();
            EditMode = false;
            EditDialogVisibility = true;
        }
        protected async void OnCommandClicked(CommandClickEventArgs<MyRecipe> args)
        {
            RecipeId = args.RowData.Id;

            SelectedRecipe = await MyRecipesService.GetRecipeById(RecipeId);

            if (args.CommandColumn.Type == CommandButtonType.Edit)
            {
                EditDialogVisibility = true;
                EditMode = true;
                StateHasChanged();
            }

            if (args.CommandColumn.Type == CommandButtonType.Delete)
            {
                DeleteCaption = "Confirma operação?";
                DeleteVisibility = true;
                StateHasChanged();
            }
            if (args.CommandColumn.Type == CommandButtonType.None)
            {
                var fileName = args.RowData.RecipeUrl;
                if (fileName!.ToLower().StartsWith("http"))
                {
                    try
                    {
                        await JSRuntime!.InvokeAsync<object>("open", fileName, "_blank");

                    }
                    catch (System.Exception ex)
                    {
                        ToastTitle = "Pesquisar url da receita";
                        ToastCssClass = "e-toast-error";
                        ToastContent = $"Erro ao pesquisar ({ex.Message})";

                        StateHasChanged();
                        await Task.Delay(200);
                        await this.ToastObj.ShowAsync();

                        Logger.LogError(ex.Message);
                    }
                }
            }
        }

        protected void RecordDoubleClickHandler(RecordDoubleClickEventArgs<MyRecipe> args)
        {
            ShowDetailDialogVisibility = true;
        }

        protected void RowSelectedHandler(RowSelectEventArgs<MyRecipe> args)
        {
            if (args.Data == null)
                return;

            SelectedRecipe = args.Data;
        }


        protected async Task Voltar()
        {
            ToastTitle = "Edição de Receita";
            ToastCssClass = "e-toast-warning";
            ToastContent = "Operação de gravação cancelada";

            EditDialogVisibility = false;
            StateHasChanged();

            await Task.Delay(200);
            await this.ToastObj.ShowAsync();
        }

        protected async Task AddOrSaveRecord()
        {
            //TODO Validate entries
            if (EditMode)
            {
                await MyRecipesService.Update_Recipe(SelectedRecipe);
                ToastTitle = "Edição de registo";
                ToastCssClass = "e-toast-success";
                ToastContent = "Registo foi gravado com sucesso";
            }
            else
            {
                await MyRecipesService.Insert_Recipe(SelectedRecipe);
                ToastTitle = "Criação de registo";
                ToastCssClass = "e-toast-success";
                ToastContent = "Registo foi criado com sucesso";
                SelectedRecipe = new MyRecipe();
            }

            EditDialogVisibility = false;
            StateHasChanged();

            await Task.Delay(200);
            await ToastObj.ShowAsync();

            PersonalRecipes = await GetAllRecipes();
            await gridObj.Refresh();
        }

        protected async Task DeleteRecipe()
        {
            DeleteVisibility = false;
            ToastTitle = "Apagar receita";

            try
            {
                await MyRecipesService.Delete_Recipe(SelectedRecipe.Id);

                ToastContent = "Registo removido com sucesso";
                ToastCssClass = "e-toast-success";
                PersonalRecipes = await GetAllRecipes();
            }
            catch (Exception ex)
            {
                ToastContent = $"Erro ao remover registo ({ex.Message})";
                ToastCssClass = "e-toast-danger";
            }
            StateHasChanged();
            await Task.Delay(200);
            await ToastObj.ShowAsync();
        }
        protected void ConfirmDeleteNo()
        {
            DeleteVisibility = false;
        }
    }
}