using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using DotnetAppDemo.API.Extensions;
using DotnetAppDemo.Repository.Data;
using DotnetAppDemo.Service.Interfaces;
using DotnetAppDemo.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

#nullable disable

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer("keycloak", realm: "DotnetAppDemo", options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = "account";
    });

builder.Services.AddAuthorization();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    })
    .AddOData(opt => opt.Select()
        .Expand()
        .SetMaxTop(null)
        .SkipToken()
        .OrderBy()
        .Count()
        .Filter()
        .EnableQueryFeatures(1000)
    )
    .AddODataNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DotnetAppDemoDB"));
});

builder.Services.ConfigureServices();

builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
    policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(builder.Configuration["Keycloak:ClientId"]);
    });
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("users/me", (ClaimsPrincipal claimsPrincipal) =>
{
    var claimsDictionary = claimsPrincipal.Claims
        .GroupBy(c => c.Type)
        .ToDictionary(g => g.Key, g => g.First().Value);
    return claimsDictionary;
}).RequireAuthorization();

app.MapControllers();

app.Run();
