using AddressBook.Application.Commands;
using AddressBook.Data;
using AddressBook.Migrations;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AddressBook.Application.IntegrationTests;

public class Fixture : IDisposable
{
    protected internal readonly IServiceProvider Container;

    public Fixture()
    {
        Container = BuildServiceProvider();
        //Task.Run(PrepareDbAsync).Wait();
    }

    private async Task PrepareDbAsync()
    {
        var configuration = Container.GetService<IConfiguration>();
        if (!ShouldCleanDb(configuration))
            return;

        //var logConnectionString = configuration.GetConnectionString("Logs");
        //await using (var cnx = new SqlConnection(logConnectionString))
        //{
        //    cnx.Open();

        //    var cmd = new SqlCommand("TRUNCATE TABLE [dbo].[__Logs]", cnx);
        //    cmd.ExecuteNonQuery();
        //}

        var migrator = new DatabaseMigrator("AddressBook_Database");
        await migrator.MigrateToLatestVersion();
    }

    public void Dispose()
    {
        Log.CloseAndFlush();
    }

    private IServiceProvider BuildServiceProvider()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = configurationBuilder.Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging();

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.Scan(scan => scan.FromAssemblyOf<CreateContact>()
            .AddClasses(classes => classes.AssignableTo<IValidator>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddDataAccess(true);

        services.AddMediatR(typeof(CreateContact).Assembly);
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        var container = services.BuildServiceProvider();
        return container;
    }

    protected internal bool ShouldCleanDb(IConfiguration configuration)
    {
        var result = configuration.GetSection("App").GetValue<bool>("CleanupDb");
        return result;
    }
}