using AddressBook.Migrations;

var migrator = new DatabaseMigrator();
await migrator.MigrateToLatestVersion();