using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Storage.Application.CQRS.Command.Folders;
using Storage.Application.CQRS.Query;
using Storage.Application.Services;
using Storage.Domain.Repositories;
using Storage.Infrastructure.Repositories;
using Storage.Infrastructure.Storage.Local;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() 
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(CreateFolderCommand).Assembly,
    typeof(GetFoldersByUserIdQuery).Assembly));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });



var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
