using AddressBook.Api.Models;
using AddressBook.Api.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AddressBook.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Contact>> Get()
    {
        var contacts = _contactService.GetContacts();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public ActionResult<Contact> GetContactById(Guid id)
    {
        var contact = _contactService.GetContact(id);
        if (contact is null)
        {
            return NotFound();
        }

        return Ok(contact);
    }

    [HttpPost]
    public void CreateContact([FromBody] string value)
    {
    }

    [HttpPut("{id}")]
    public void UpdateContact(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}