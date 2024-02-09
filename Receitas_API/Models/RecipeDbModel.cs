namespace Receitas_API.Models
{
    public class RecipeDbModel
    {
        public class Recipe
        {
            public int Id { get; set; }
            public string Description { get; set; }
        }

        public class Ingredient
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int RecipeId { get; set; }
        }

        public class Preparation
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int RecipeId { get; set; }
        }

        public class Other
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int RecipeId { get; set; }
        }

    }
}
