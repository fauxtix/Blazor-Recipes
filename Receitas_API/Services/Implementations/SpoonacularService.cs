using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging;
using Receitas_API.Models;
using Receitas_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Receitas_API.Services.Implementations
{
    public class SpoonacularService : ISpoonacularService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<SpoonacularService> _logger;
        private readonly string apiKey = "871cc9ddc1ea4733830dd2c30e3d691a";

        public SpoonacularService(HttpClient httpClient, ILogger<SpoonacularService> logger)
        {
            this.httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CountriesCuisines.Root> GetRecipeTitles(string regionName)
        {
            string baseUrl = "https://api.spoonacular.com/recipes/complexSearch";
            string cuisineParam = $"cuisine={regionName}";
            string url = $"{baseUrl}?apiKey={apiKey}&{cuisineParam}";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<CountriesCuisines.Root>();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new CountriesCuisines.Root();
            }
        }


        public async Task<List<Recipes.MyArray>> GetRecipeDetails(int Id)
        {
            string baseUrl = $"https://api.spoonacular.com/recipes/{Id}/analyzedInstructions";
            string url = $"{baseUrl}?apiKey={apiKey}";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<List<Recipes.MyArray>>();

                return result;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                return new List<Recipes.MyArray>();
            }
        }

        public async Task<RecipeInformation.RecipeInfo> GetRecipeInformation(int Id)
        {
            string baseUrl = $"https://api.spoonacular.com/recipes/{Id}/information";
            string nutritionParam = "includeNutrition=false";
            string url = $"{baseUrl}?{nutritionParam}&apiKey={apiKey}";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<RecipeInformation.RecipeInfo>();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new();
            }
        }


    }
}
