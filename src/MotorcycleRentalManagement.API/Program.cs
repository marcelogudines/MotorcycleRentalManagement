using MassTransit;
using MongoDB.Driver;
using MotorcycleRentalManagement.API.Filters.Middleware;
using MotorcycleRentalManagement.API.Middleware;
using MotorcycleRentalManagement.Domain.Entities;
using MotorcycleRentalManagement.Domain.Interfaces;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;
using MotorcycleRentalManagement.Domain.Services;
using MotorcycleRentalManagement.Domain.Strategies.PenaltyStrategies;
using MotorcycleRentalManagement.Domain.Usercases.DeliveryPersonCase;
using MotorcycleRentalManagement.Domain.Usercases.MotorcycleCase;
using MotorcycleRentalManagement.Domain.Usercases.RentalCase;
using MotorcycleRentalManagement.Infrastructure;
using MotorcycleRentalManagement.Infrastructure.Messaging;
using MotorcycleRentalManagement.Infrastructure.Messaging.Consumer;
using MotorcycleRentalManagement.Infrastructure.Repositories;
using MotorcycleRentalManagement.Infrastructure.Storages;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);


ConfigureLogging(builder);

ConfigureMessaging(builder);

ConfigureMongoDb(builder);

RegisterServices(builder.Services);

ConfigureSwagger(builder.Services);

builder.Services.AddControllers();

var app = builder.Build();

// Seed the database
await SeedDatabase(app);

ConfigurePipeline(app);

app.Run();


void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .WriteTo.Console()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Information();
    });
}

void ConfigureMessaging(WebApplicationBuilder builder)
{
    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<MotorcycleRegisteredConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration["RabbitMqSettings:Host"], h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            cfg.ReceiveEndpoint("motorcycle-registered-event", e =>
            {
                e.ConfigureConsumer<MotorcycleRegisteredConsumer>(context);
            });
        });
    });
}

void ConfigureMongoDb(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<IMongoClient>(sp =>
    {
        var settings = MongoClientSettings.FromConnectionString(builder.Configuration["MongoDbSettings:ConnectionString"]);
        return new MongoClient(settings);
    });

    builder.Services.AddScoped(sp =>
    {
        var mongoClient = sp.GetRequiredService<IMongoClient>();
        return mongoClient.GetDatabase(builder.Configuration["MongoDbSettings:DatabaseName"]);
    });
}

void RegisterServices(IServiceCollection services)
{
   
    services.AddScoped<IFileStorageService, LocalFileStorageService>();

    
    services.AddScoped<IRegisterDeliveryPersonUseCase, RegisterDeliveryPersonUseCase>();
    services.AddScoped<IUpdateDeliveryPersonCnhImageUseCase, UpdateDeliveryPersonCnhImageUseCase>();
    services.AddScoped<IDeliveryPersonRepository, DeliveryPersonRepository>();
  
    services.AddScoped<IRegisterMotorcycleMotorcycleUseCase, RegisterMotorcycleMotorcycleUseCase>();
    services.AddScoped<IUpdateMotorcycleLicensePlateUseCase, UpdateMotorcycleLicensePlateUseCase>();
    services.AddScoped<IDeleteMotorcycleUseCase, DeleteMotorcycleUseCase>();
    services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
    services.AddScoped<IMotorcycleUseCase, MotorcycleUseCase>();
    
    services.AddScoped<IRegisterRentalUseCase, RegisterRentalUseCase>();
    services.AddScoped<IReturnRentalUseCase, ReturnRentalUseCase>();
    services.AddScoped<IGetRentalByIdUseCase, GetRentalByIdUseCase>();
    services.AddScoped<IRentalRepository, RentalRepository>();
   
    services.AddScoped<IMessagePublisher, MessagePublisher>();
    services.AddScoped<INotifiable, Notifiable>();
    services.AddScoped<INotificationEventRepository, NotificationEventRepository>();
   
    services.AddSingleton<ImageValidator>();
    services.AddSingleton<RentalPriceCalculator>();
    services.AddScoped<IRentalPenaltyStrategy, EarlyReturnPenaltyStrategy>();
    services.AddScoped<IRentalPenaltyStrategy, LateReturnPenaltyStrategy>();
    services.AddScoped<IPenaltyStrategyContext, PenaltyStrategyContext>();
}

void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.ExampleFilters();
    });

    
    services.AddSwaggerExamplesFromAssemblyOf<Program>();
}

async Task SeedDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        var seeder = new DatabaseSeeder(mongoDatabase);
        await seeder.SeedAsync();
    }
}

void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotorcycleRentalManagement API V1");
    });

    app.UseMiddleware<LoggingMiddleware>();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}
