IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE [name] = '__AddressBookMigration') 
	CREATE TABLE dbo.[__AddressBookMigration] (
		ScriptsVersion [int] NOT NULL
	)

INSERT INTO dbo.[__AddressBookMigration] (ScriptsVersion) 
	SELECT 0 
	WHERE NOT EXISTS(SELECT 1 FROM dbo.[__AddressBookMigration]) 