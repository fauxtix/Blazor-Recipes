using Microsoft.Extensions.Logging;
using Receitas_API.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Receitas_API.Data
{
    public interface ITastyApiRecipesService
    {
        Task<TastyRoot> GetRecipes(string tag, int i, int option);
        Task<RecipeTags> GetRecipesTags();
        Task<string> Translate(string sourcetext);
    }

    public class TastyApiRecipesService : ITastyApiRecipesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TastyApiRecipesService> _logger;

        private const string RapidApiKey = "8229763029msh246a15560c42139p1e1a2djsn1ca0cdd178ee";
        private const string RapidApiHost = "tasty.p.rapidapi.com";

        public TastyApiRecipesService(HttpClient httpClient, ILogger<TastyApiRecipesService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TastyRoot> GetRecipes(string query, int recordCount = 100, int option = 0)
        {
            string queryOption = option == 0 ? "tags" : "q";
            string requestUri = $"https://tasty.p.rapidapi.com/recipes/list?from=0&size={recordCount}&{queryOption}={query}";
            return await SendRequest<TastyRoot>(HttpMethod.Get, requestUri);
        }

        public async Task<RecipeTags> GetRecipesTags()
        {
            string requestUri = "https://tasty.p.rapidapi.com/tags/list";
            return await SendRequest<RecipeTags>(HttpMethod.Get, requestUri);
        }

        public async Task<string> Translate(string sourcetext)
        {
            string requestUri = "https://microsoft-translator-text.p.rapidapi.com/translate?api-version=3.0&to=%3CREQUIRED%3E&textType=plain&profanityAction=NoAction";
            var content = new StringContent(sourcetext);
            return await SendRequest<string>(HttpMethod.Post, requestUri, content);
        }

        private async Task<T> SendRequest<T>(HttpMethod method, string requestUri, HttpContent content = null)
        {
            var request = new HttpRequestMessage
            {
                Method = method,
                RequestUri = new Uri(requestUri),
                Headers =
                {
                    { "x-rapidapi-host", RapidApiHost },
                    { "x-rapidapi-key", RapidApiKey },
                },
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
                throw; // Rethrow the exception for handling at a higher level if necessary
            }
        }
    }
}
