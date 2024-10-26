using AddressBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Data;

public class ContactsDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new Configuration());
    }
}