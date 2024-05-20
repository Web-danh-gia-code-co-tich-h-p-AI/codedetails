using System.Security.Cryptography;
using CodeData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbConnection");

// Generate the secret key
var key = new byte[32];
using (var rng = new RNGCryptoServiceProvider())
{
    rng.GetBytes(key);
}
var secretKey = Convert.ToBase64String(key);

// Add DbContext and MySqlConnection to services
builder.Services.AddDbContext<ApiDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 23))));

builder.Services.AddTransient<MySqlConnection>(_ =>
    new MySqlConnection(connectionString));

// Add CORS to services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://bewcutoe-001-site1.ctempurl.com/",
            ValidAudience = "http://bewcutoe-001-site1.ctempurl.com/",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization();

// Use CORS policy
app.UseCors("AllowAllOrigins");
app.MapControllers();
app.Run();
