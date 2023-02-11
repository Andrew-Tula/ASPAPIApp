using ASPAPI.Abstract.Models;
using ASPAPI.Abstract.Repositories;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using Microsoft.EntityFrameworkCore;

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
services.AddSwaggerGen();

services.AddDbContext<TestDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TestDBContext")));

//services.AddScoped<IGenericRepositories<Role>, GenericRepository<Role>>();
services.AddScoped<IGenericRepositories<Product>, ProductRepository>();
services.AddScoped<IGenericRepositories<Store>, GenericRepository<Store>>();
//services.AddScoped<IGenericRepositories<User>, GenericRepository<User>>();
//services.AddScoped<IGenericRepositories<Order>, OrderRepository>();
//services.AddScoped<IGenericRepositories<OrderItem>, OrderItemRepository>();
//services.AddScoped<IGenericRepositories<Store>, StoreRepository>();

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IRoleRepository, RoleRepository>();
//services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderItemRepository, OrderItemRepository>();

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
