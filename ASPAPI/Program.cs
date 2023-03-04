using ASPAPI.Abstract.Models;
using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

//select
//var userExtendedCollection = new List<UserExtendedDto> {
//    new UserExtendedDto(1, "Ваcя", 1),
//    new UserExtendedDto(2, "Иван", 2),
//};
//var userCollection = userExtendedCollection.Select(i => new UserDto(i.name, i.roleId));
//------------------------------------------------
//select many
//var productCollection = new Dictionary<int, List<string>> {
//    { 
//        1, new List<string> {  
//            "Хлеб",
//            "Молоко"
//        }
//    },
//    { 
//        2, new List<string> {
//            "Хлеб",
//            "Конфеты",
//            "Суп"
//        }
//    }
//};
//var data = productCollection.SelectMany(i => i.Value).Distinct();
//------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(swagger => {
    swagger.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "JWT Token Authentication API",
        Description = "ASP.NET Core"
    });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

services.AddDbContext<TestDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TestDBContext")));

services.AddScoped<IStoreProductRepository, StoreProductRepository>();
services.AddScoped<IGenericRepository<Product>, ProductRepository>();
services.AddScoped<IGenericRepository<Store>, GenericRepository<Store>>();

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IRoleRepository, RoleRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderItemRepository, OrderItemRepository>();
services.AddScoped<IUserTokeRepository, UserTokeRepository>();

var configuration = builder.Configuration;
services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//to read
//AddSingleton AddScoped AddTransient
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<TestDBContext>();
    db.Database.Migrate();
}

app.Run();
