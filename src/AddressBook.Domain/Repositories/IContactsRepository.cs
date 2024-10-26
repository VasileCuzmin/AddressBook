using AddressBook.Domain.Entities;
using NBB.Data.Abstractions;

namespace AddressBook.Domain.Repositories;

public interface IContactsRepository : ICrudRepository<Contact>
{
    Task<Contact?> GetByEmail(string email);
}