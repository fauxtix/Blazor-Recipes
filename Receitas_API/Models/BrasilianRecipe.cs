using System;

namespace Receitas_API.Models
{
    public class BrasilianRecipe
    {
        public class Receita
        {
            public int id { get; set; }
            public string receita { get; set; }
            public string ingredientes { get; set; }
            public string modo_preparo { get; set; }
            public string link_imagem { get; set; }
            public string tipo { get; set; }
            public DateTime created_at { get; set; }
            public Ingredientesbase[] IngredientesBase { get; set; }
        }

        public class Ingredientesbase
        {
            public int id { get; set; }
            public string[] nomesIngrediente { get; set; }
            public int receita_id { get; set; }
            public DateTime created_at { get; set; }
        }

    }
}
