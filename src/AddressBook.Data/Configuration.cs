using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.Data;

internal class Configuration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts").HasKey(x => new { x.Id });
        builder.Property(c => c.FirstName);
        builder.Property(c => c.LastName);
        builder.Property(c => c.EmailAddress);
        builder.Property(c => c.PhoneNumber);
    }
}