using Azure.Core;
using Microsoft.Extensions.Logging;
using Receitas_API.Models;
using Syncfusion.Blazor.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Receitas_API.Data.BrasilianRecipesService
{
    public class RecipeService : IRecipeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RecipeService> _logger;
        private readonly string urlSite = "https://gold-anemone-wig.cyclic.app/receitas";

        public RecipeService(HttpClient httpClient, ILogger<RecipeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<IEnumerable<BrasilianRecipe.Receita>> GetAllRecipes()
        {

            string requestUri = $"{urlSite}/todas";
            return await SendRequest<IEnumerable<BrasilianRecipe.Receita>>(HttpMethod.Get, requestUri);
        }
        public async Task<BrasilianRecipe.Receita> GetById(int Id)
        {

            string requestUri = $"{urlSite}/{Id}";
            try
            {
                using (var response = await _httpClient.GetAsync(requestUri))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<BrasilianRecipe.Receita>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(BrasilianRecipe.Receita);
            }
        }

        public async Task<BrasilianRecipe.Receita> GetRecipes(int Id = 0, int option = 0)
        {

            string queryOption = option == 0 ? "todas" : option == 1 ? "" : "";
            switch (option)
            {
                case 0:
                    queryOption = "todas";
                    break;
                case 1:
                    queryOption = Id.ToString();
                    break;
                case 2:
                    queryOption = $"tipo/{Id}";
                    break;
                case 3:
                    queryOption = "ingredientes";
                    break;
            }
            string requestUri = $"{urlSite}/{queryOption}";
            return await SendRequest<BrasilianRecipe.Receita>(HttpMethod.Get, requestUri);
        }

        private async Task<T> SendRequest<T>(HttpMethod method, string requestUri, HttpContent content = null)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(requestUri),
                Content = content
            };

            try
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<T>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return default(T);
            }
        }

    }
}
