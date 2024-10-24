using AddressBook.Api.Models;

namespace AddressBook.Api.Services;

public interface IContactService
{
    IEnumerable<Contact> GetContacts();
    Contact? GetContact(Guid id);
}

public class ContactService : IContactService
{
    private readonly List<Contact> _contacts =
    [
        new Contact
        {
            Id = Guid.NewGuid(), FirstName = "John", LastName = "Smith", PhoneNumber = "1111-111",
            EmailAddress = "js@gmail.com"
        },
        new Contact
        {
            Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", PhoneNumber = "1111-222",
            EmailAddress = "js@gmail.com"
        }
    ];
    public IEnumerable<Contact> GetContacts()
    {
        return _contacts;
    }

    public Contact? GetContact(Guid id)
    {
        return _contacts.FirstOrDefault(c => c.Id == id);
    }
}