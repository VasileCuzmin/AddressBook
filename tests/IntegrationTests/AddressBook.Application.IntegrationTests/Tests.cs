using AddressBook.Application.Queries;
using AddressBook.Domain.Entities;
using AddressBook.Domain.Repositories;
using AddressBook.Migrations;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NBB.Data.Abstractions;
using System.Data.SqlClient;
using Xunit;
using Xunit.Abstractions;

namespace AddressBook.Application.IntegrationTests;

public class Tests : IClassFixture<Fixture>, IDisposable
{
    private readonly Fixture _fixture;
    private readonly IMediator _mediator;
    private readonly IServiceScope _scope;
    private bool _isDisposed;
    private readonly IContactsRepository _contactsRepository;
    public Tests(Fixture fixture, ITestOutputHelper testOutputHelper)
    {
        _fixture = fixture;

        _scope = fixture.Container.CreateScope();
        _mediator = _scope.ServiceProvider.GetService<IMediator>();
        var logger = _scope.ServiceProvider.GetService<ILogger<Tests>>();
        _contactsRepository = _fixture.Container.GetService<IContactsRepository>();

        logger.LogInformation($"{testOutputHelper.GetTestName()}");
        //Task.Run(PrepareDbAsync).Wait();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            // free managed resources
            _scope.Dispose();
        }

        // free native resources if there are any.

        _isDisposed = true;
    }

    private async Task PrepareDbAsync()
    {
        var configuration = _scope.ServiceProvider.GetService<IConfiguration>();

        if (!_fixture.ShouldCleanDb(configuration))
            return;

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        await using (var cnx = new SqlConnection(connectionString))
        {
            cnx.Open();

            List<string> tbls = new List<string>()
                {"Contacts"};

            foreach (var tbl in tbls)
            {
                var cmd = new SqlCommand($"DROP TABLE [dbo].[{tbl}]", cnx);
                await cmd.ExecuteNonQueryAsync();
            }

            var resetCommand = new SqlCommand("UPDATE [dbo].[__AddressBookMigration] SET ScriptsVersion = 0", cnx);
            await resetCommand.ExecuteNonQueryAsync();
        }

        var migrator = new DatabaseMigrator("DefaultConnection");
        await migrator.MigrateToLatestVersion();
    }

    private async Task InsertContact(Contact contact)
    {
        await _contactsRepository.AddAsync(contact);
        await _contactsRepository.SaveChangesAsync();
    }

    private async Task<int> GetContactsCount()
    {
        return (await _contactsRepository.GetAllAsync(CancellationToken.None)).Count();
    }

    [Fact]
    public async void GetContactsTest()
    {
        var prevCount = await GetContactsCount();
        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            FirstName = "Joe",
            LastName = "Smith",
            EmailAddress = "joesmith@gmail.com",
            PhoneNumber = "111-111-1111"
        };

        //act
        await InsertContact(contact);

        var currentCount = await GetContactsCount();

        var result = await _mediator.Send(new GetContacts.Query { Page = 1, PageSize = 10 });

        //assert 
        Assert.Equal(currentCount, prevCount + 1);
        result.TotalCount.Should().Be(currentCount);
        result.Values.Should().Contain(x => x.Id == contact.Id);
    }


    [Fact]
    public async void GetContactByIdTest()
    {
        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            FirstName = "Joe",
            LastName = "Smith",
            EmailAddress = "joesmith@gmail.com",
            PhoneNumber = "111-111-1111"
        };

        //act
        await InsertContact(contact);

        var result = await _mediator.Send(new GetContactById.Query { Id = contact.Id });

        //assert 
        Assert.Equal(result.Id, contact.Id);
    }
}