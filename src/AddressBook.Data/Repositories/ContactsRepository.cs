using AddressBook.Domain.Entities;
using AddressBook.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NBB.Core.Abstractions;
using NBB.Data.EntityFramework;
using NBB.Data.EntityFramework.Internal;

namespace AddressBook.Data.Repositories;

public class ContactsRepository : EfCrudRepository<Contact, ContactsDbContext>, IContactsRepository
{
    public readonly ContactsDbContext _dbContext;

    public ContactsRepository(ContactsDbContext c, IExpressionBuilder expressionBuilder, IUow<Contact> uow, ILogger<EfCrudRepository<Contact, ContactsDbContext>> logger, ContactsDbContext dbContext)
        : base(c, expressionBuilder, uow, logger)
    {
        _dbContext = dbContext;
    }

    public async Task<Contact?> GetByEmail(string? email)
    {
        var contact = await _dbContext.Contacts.SingleOrDefaultAsync(a => string.Equals(a.EmailAddress, email, StringComparison.OrdinalIgnoreCase));
        return contact;
    }
}