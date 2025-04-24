using Microsoft.EntityFrameworkCore;
using SothbeysKillerApi.Context;
using SothbeysKillerApi.Infrastructure;
using SothbeysKillerApi.Repository;
using SothbeysKillerApi.Services;
using LoggerNamespace;

var builder = WebApplication.CreateBuilder(args);

// для логування;
builder.Services.AddSingleton<Logger>();

// EF Core з PostgreSQL
builder.Services.AddDbContext<UserDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Реєстрація репозиторіїв і Unit of Work
builder.Services.AddScoped<IUnitOfWork, EFUnitOfWork>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IAuctionRepository, EFAuctionRepository>();
// Додайте IAuctionHistoryRepository, якщо він потрібен
// builder.Services.AddScoped<IAuctionHistoryRepository, EFAuctionHistoryRepository>();

// Реєстрація сервісів
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddScoped<IUserValidationService, UserValidationService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Налаштування HTTP конвеєра
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Глобальна обробки виключень
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();