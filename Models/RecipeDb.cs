using Microsoft.EntityFrameworkCore;
using Recipes.Models;

class RecipeDb : DbContext
{
    public RecipeDb(DbContextOptions options) : base(options) { }
    public DbSet<Recipe> Recipes { get; set; } = null!;
}