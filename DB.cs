namespace Recipes.DB
{
    public record Recipe
    {
        public int Id {get; set;}
        public string? Title {get; set;}
        public string? Description {get; set;}
        public bool Complete {get; set;}
    }

    public class RecipeDB 
    {
        private static List<Recipe> _recipes = new List<Recipe>()
        {
            new Recipe { Id = 1, Title = "Banitza", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Complete=false},
            new Recipe { Id = 2, Title = "Tarator", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Complete=false},
            new Recipe { Id = 3, Title = "Mussaka", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Complete=false},
        };

        public static List<Recipe> GetRecipes() {
            return _recipes;
        }

        public static Recipe? GetRecipe(int id) 
        {
            return _recipes.SingleOrDefault(recipe => recipe.Id == id);
        }

        public static Recipe CreateRecipe(Recipe recipe) 
        {
            _recipes.Add(recipe);
            return recipe;
        }

        public static Recipe UpdateRecipe(Recipe update)
        {
            _recipes = _recipes.Select( recipe => {
                if(recipe.Id == update.Id)
                {
                    recipe.Title = update.Title;
                    recipe.Description = update.Description;
                    recipe.Complete = update.Complete;
                }
                return recipe;
            }).ToList();
            return update;
        }

        public static void RemoveRecipe(int id) 
        {
            _recipes = _recipes.FindAll(recipe=> recipe.Id != id).ToList();
        }
    }
}