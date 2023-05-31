using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Recipes.Models;
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
//CORS

//Add connection string:
var connectionString = builder.Configuration.GetConnectionString("Recipes") ?? "Data Source=Recipes.db";
builder.Services.AddDbContext<RecipeDb>(
  options => options.UseSqlite(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "Recipes API", Description = "Weekly recipes", Version ="v1"});
});


//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
      builder =>
      {
          builder
          .AllowAnyHeader()
          .AllowAnyMethod()
          .WithOrigins("*");
      });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI( c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipes API v1");
});

//CORS
app.UseCors(MyAllowSpecificOrigins);

//ROUTES
app.MapGet("/", () => "Hello World!");
// app.MapGet("/recipes", () => RecipeDB.GetRecipes());
// app.MapGet("/recipe/{id}", (int id) => RecipeDB.GetRecipe(id));
// app.MapPost("/recipes", (Recipe recipe) => RecipeDB.CreateRecipe(recipe));
// app.MapPut("/recipes", (Recipe recipe) => RecipeDB.UpdateRecipe(recipe));
// app.MapDelete("/recipes/{id}", (int id) => RecipeDB.RemoveRecipe(id));

app.MapGet("/recipes", async (RecipeDb db) => await db.Recipes.ToListAsync());

app.MapGet("/recipe/{id}", async (RecipeDb db, int id) => await db.Recipes.FindAsync(id));

app.MapPost("/recipe", async (RecipeDb db, Recipe recipe) =>
{
    await db.Recipes.AddAsync(recipe);
    await db.SaveChangesAsync();
    return Results.Created($"/recipe/{recipe.Id}", recipe);
});

app.MapPut("/recipe/{id}", async (RecipeDb db, Recipe updateRecipe, int id) =>
{
      var recipe = await db.Recipes.FindAsync(id);
      if (recipe is null) return Results.NotFound();
      recipe.Title = updateRecipe.Title;
      recipe.Description = updateRecipe.Description;
      recipe.Complete = updateRecipe.Complete;
      await db.SaveChangesAsync();
      return Results.NoContent();
});

app.MapDelete("/recipe/{id}", async (RecipeDb db, int id) =>
{
   var recipe = await db.Recipes.FindAsync(id);
   if (recipe is null)
   {
      return Results.NotFound();
   }
   db.Recipes.Remove(recipe);
   await db.SaveChangesAsync();
   return Results.Ok();
});
app.Run();
