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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TestDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TestDBContext")));

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
