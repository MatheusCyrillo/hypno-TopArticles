using TopArticles.Application;
using TopArticles.Application.Interfaces;
using TopArticles.CustomException;
using TopArticles.Services;
using TopArticles.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHypnoService, HypnoService>();
builder.Services.AddScoped<IArticleApplication, ArticleApplication>();

builder.Services.AddHttpClient("hypnobox", h =>
{
    h.BaseAddress = new Uri($"http://hypnocore.api.hypnobox.com.br");
    h.Timeout = TimeSpan.FromSeconds(20);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/toparticles/{top}", async (int top, IArticleApplication articleApplication) =>
{
    try
    {
        var topArticles = await articleApplication.GetTopArticles(top);

        return Results.Ok(topArticles);
    }
    catch (BusinessRuleException ex)
    {
        //LOG
        return Results.Problem(ex.Message);
    }
    catch (Exception ex)
    {
        //LOG
        return Results.Problem("Ocorreu um erro inesperado, tenta novamente!");
    }
})
.WithName("TopArticles")
.WithOpenApi();


app.Run();

