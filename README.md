Required steps before running the api:

- Create a DB called AddressBook(take a look at appsettings.json file)
- Run AddressBook.Migrations console to migrate seeding data in the DB

Observations: 
- I'm using  mediatr for queries. I could've used commands as well for creating new contacts..
- I'm using  clean architecture principles and clean layers even tough the business is quite simple.
