using AddressBook.Application.Commands;
using AddressBook.Application.Queries;
using AddressBook.Domain.Entities;
using AutoMapper;

namespace AddressBook.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<CreateContact.Command, Contact>();
        CreateMap<Contact, GetContacts.Query>();
    }
}