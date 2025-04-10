using System.Text;
using API.SERVICE.Extensions;
using API.SERVICE.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#region JwtBearer
var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();

var key = Encoding.ASCII.GetBytes(jwtConfig?.Secret!);
var tokenValidationParams = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidIssuer = jwtConfig?.Issuer!,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidAudience = jwtConfig?.Audience!,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.SaveToken = false;
    jwt.RequireHttpsMetadata = false;
    jwt.TokenValidationParameters = tokenValidationParams;
});
builder.Services.AddAuthorization();
#endregion

builder.Services.AddApiService(builder.Configuration);
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddSingleton(tokenValidationParams);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    //app.MapOpenApi();
}
await app.Services.UseDataSeeder();

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();