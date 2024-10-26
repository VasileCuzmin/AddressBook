using AddressBook.Data.Repositories;
using AddressBook.Domain.Entities;
using AddressBook.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBB.Core.Abstractions;
using NBB.Data.EntityFramework;

namespace AddressBook.Data;

public static class DependencyInjectionExtensions
{
    public static void AddDataAccess(this IServiceCollection services, bool useTestDoubles = false)
    {
        services.AddEntityFrameworkDataAccess();
        services.AddEventSourcingDataAccess((sp, builder) =>
            builder.Options.DefaultSnapshotVersionFrequency = 10);

        if (useTestDoubles)
        {
            services
                .AddDbContextPool<ContactsDbContext>(options => options.UseInMemoryDatabase("ContactTests"));
        }
        else
        {
            services
                .AddDbContextPool<ContactsDbContext>(
                    (serviceProvider, options) =>
                    {
                        var configuration = serviceProvider.GetService<IConfiguration>();
                        var connectionString = configuration.GetConnectionString("AddresBook_Database");

                        options
                            .UseSqlServer(connectionString, builder => { builder.EnableRetryOnFailure(3); })
                            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                    });
        }

        services.AddScoped<IUow<Contact>, EfUow<Contact, ContactsDbContext>>();
        services.AddScoped<IContactsRepository, ContactsRepository>();
    }
}