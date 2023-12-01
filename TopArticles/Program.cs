using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TopArticles.Models;
using TopArticles.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/toparticles", async ([FromQuery] int top) =>
{
    //Injeção de dependencia 
    HypnoService hypnoService = new HypnoService();

    var article = await hypnoService.GetArticle(1);
    var allArticles = article.Data;

    //Deixar as chamadas assincronas, e paralelas 
    for (int i = 2; i <= article.TotalPages; i++)
    {
       var result = await hypnoService.GetArticle(i);
       allArticles.AddRange(result.Data);

    }

    //Colocar toda lógica em uma Application 
    var filteredArticles = allArticles.Where(a => !(string.IsNullOrEmpty(a.Title) && string.IsNullOrEmpty(a.StoryTitle)));

    filteredArticles = allArticles.Where(a => a.NumComments != null);

    var orderedArticles = filteredArticles.OrderByDescending(a => a.NumComments).ThenBy(a => a.ArticleTitle, StringComparer.OrdinalIgnoreCase);

    return orderedArticles.Select(o =>new { o.ArticleTitle, o.NumComments }).Take(top);

})
.WithName("TopArticles")
.WithOpenApi();


app.Run();

