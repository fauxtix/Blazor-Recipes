namespace Receitas_API.Models
{
    public class MyRecipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingredients{ get; set; }
        public string Preparation{ get; set; }
        public string OtherInfo { get; set;}
        public string RecipeUrl { get; set; }
        public string ImageUrl { get; set;}
    }
}
