using SothbeysKillerApi.Services;
using LoggerNamespace;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // For logging
        builder.Services.AddSingleton<Logger>();
        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IAuctionService, AuctionService>();
        // Окремий серввіс для валидациї
        builder.Services.AddScoped<IUserValidationService, UserValidationService>();

        // builder.Services.AddDbContext<AuctionContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        // builder.Services.AddScoped<IAuctionRepository, AuctionRepository>();
        builder.Services.AddScoped<IAuctionService, AuctionService>();

        builder.Services.AddTransient<IAuctionService, DbAuctionService>();

        /*
         * Transient
         * Scoped
         * Singleton
         */

        var app = builder.Build();

        // Console.WriteLine("Server running in http://localhost:5086/:api/v1/");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}