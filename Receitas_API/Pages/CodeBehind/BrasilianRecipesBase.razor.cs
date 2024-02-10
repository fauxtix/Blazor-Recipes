using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Receitas_API.Data;
using RestSharp;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.Spinner;
using Syncfusion.Blazor.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Microsoft.Extensions.Hosting;
using Receitas_API.Models;

namespace Receitas_API.Pages.CodeBehind
{
    public class BrasilianRecipesBase : ComponentBase
    {
        [Inject]
        protected IWebHostEnvironment Env { get; set; }

        [Inject]
        protected IBrasilianRecipes_Dapper RecipeService { get; set; }

        protected SfSpinner SpinnerObj;

        protected SfGrid<RecipeDbModel.Recipe> gridObj;
        protected string tabHeight = "540";

        protected SfToast ToastObj;
        public DialogEffect Effects = DialogEffect.Zoom;

        protected string ToastTitle = "";
        protected string ToastContent = "";
        protected string ToastCssClass = "";

        protected string target { get; set; } = "#target";

        protected IEnumerable<RecipeDbModel.Recipe> brasilianRecipes;

        protected RecipeDbModel.Recipe SelectedRecord = new();
        protected bool blnEdit = true;
        public string OperationResult { get; set; }

        protected string sErrorMsgs = "";

        public bool EditDialogVisibility { get; set; } = false;
        public bool DupNamesDialogVisibility { get; set; } = false;
        public bool ShowDetailDialogVisibility { get; set; } = false;
        public bool ValidationErrorsVisibility { get; set; } = false;
        public bool DeleteRecordConfirmVisibility { get; set; } = false;

        public int recipeID { get; set; }
        protected string DeleteCaption;


        protected override async Task OnInitializedAsync()
        {
            DupNamesDialogVisibility = false;
            DeleteCaption = "";
            brasilianRecipes = await RecipeService.GetAllRecipes();
            if (brasilianRecipes.Count() == 0)
            {
                await RecipeService.Update_RecipeTable();
                await RecipeService.CreateDbDataFromApi();
            }

        }

        protected async Task<List<Receita>> GetBrasilianRecipes()
        {
            RestClient client;
            //Env.EnvironmentName = "Production";  // unmarkto "Production"
            try
            {
                if (Env.IsDevelopment())
                {
                    client = new RestClient("https://localhost:44355/api/");  // Recipes API
                }
                else
                {
                    client = new RestClient("localhost:8080/api/");
                }
                var request = new RestRequest("receitas", method: Method.Get);
                request.AddHeader("Accept", "application/json");
                var response = (await client.GetAsync<List<Receita>>(request)).ToList();

                var OrderedRecipes = response.OrderBy(p => p.Nome).ToList();
                return OrderedRecipes;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error reading Api:\r\n{ex.Message}");
            }
        }

        protected async Task InsRecipes()
        {
            try
            {
                await RecipeService.CreateDbDataFromApi();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error reading Api:\r\n{ex.Message}");
            }
        }

        public void OnCommandClicked(CommandClickEventArgs<RecipeDbModel.Recipe> args)
        {
            var data = args.RowData;
            recipeID = data.Id;

            SelectedRecord = RecipeService.GetRecipeById(recipeID);

            if (args.CommandColumn.Type == CommandButtonType.Edit)
            {
                EditDialogVisibility = true;
                blnEdit = true;
                StateHasChanged();
            }

            if (args.CommandColumn.Type == CommandButtonType.Delete)
            {
                DeleteRecordConfirmVisibility = true;
                StateHasChanged();
            }
            if (args.CommandColumn.Type == CommandButtonType.None)
            {
                ShowDetailDialogVisibility = true;
                StateHasChanged();
            }
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

        protected async Task OnBeginHandler(ActionEventArgs<RecipeDbModel.Recipe> Args)
        {
            if (Args.Data == null)
                return;

            var _recipe = Args.Data;
            SelectedRecord = _recipe;

            DeleteCaption = _recipe.Description;

            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                blnEdit = true;
                await AddOrSaveRecord();
            }
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                DeleteRecordConfirmVisibility = true;
            }
        }

        public void RowSelectedHandler(RowSelectEventArgs<RecipeDbModel.Recipe> Args)
        {
            if (Args.Data == null)
                return;

            var _recipe = Args.Data;
            SelectedRecord = _recipe;

        }
        protected async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
        {
            switch (args.Item.Id.ToLower())
            {
                case "clearsearch":
                    await gridObj.SearchAsync("");
                    break;
                case "dupnames":
                    DupNamesDialogVisibility = true;
                    break;
                case "details":
                    ShowDetailDialogVisibility = true;
                    break;
            }
        }

        public async Task AddOrSaveRecord()
        {
            //TODO Validate entries
            if (blnEdit)
            {
                RecipeService.Update_Recipe(SelectedRecord);
                ToastTitle = "Edição de registo";
                ToastCssClass = "e-toast-success";
                ToastContent = "Registo foi gravado com sucesso";
            }
            else
            {
                RecipeService.Insert_Recipe(SelectedRecord);
                ToastTitle = "Criação de registo";
                ToastCssClass = "e-toast-success";
                ToastContent = "Registo foi criado com sucesso";
                SelectedRecord = new RecipeDbModel.Recipe();
            }

            EditDialogVisibility = false;
            StateHasChanged();

            await Task.Delay(200);
            await this.ToastObj.ShowAsync();

            brasilianRecipes = await RecipeService.GetAllRecipes();
            await gridObj.Refresh();
        }

        protected async Task DeleteRecipe()
        {
            DeleteRecordConfirmVisibility = false;
            ToastTitle = "Apagar receita";

            try
            {
                RecipeService.Delete_Recipe(SelectedRecord.Id);

                ToastContent = "Registo removido com sucesso";
                ToastCssClass = "e-toast-success";
                brasilianRecipes = await RecipeService.GetAllRecipes();
                await gridObj.Refresh();


            }
            catch
            {
                ToastContent = "Erro ao remover registo";
                ToastCssClass = "e-toast-danger";
            }

            await ToastObj.ShowAsync();

        }
    }
}



