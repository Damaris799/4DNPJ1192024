using _4DNPJ1192024.Auth;
using _4DNPJ1192024.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication ",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Ingresar el token de JWT Authentication",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }

    };
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoggedInPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

var key = "key.JWTAPIMinimal2023.API";

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;

    x.SaveToken = true;

    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),

        ValidateAudience = false,

        ValidateIssuerSigningKey = true,

        ValidateIssuer = false
    };
});

builder.Services.AddSingleton<IJwAuthenticationService>(new JwtAuthenticationService (key));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddAccountEndpoints();
app.AddBodegaEndpoints();
app.AddCategoriaProductoEndpoints();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();