using API.SERVICE.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//builder.Services.AddOpenApi();
builder.Services.AddApiService(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}
await app.Services.UseDataSeeder();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();