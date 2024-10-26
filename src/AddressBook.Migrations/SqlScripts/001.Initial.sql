CREATE TABLE [Contacts]
	(
	Id [uniqueidentifier] NOT NULL,
	[FirstName] nvarchar(450) NOT NULL,
	[LastName] nvarchar(450) NOT NULL,
	[EmailAddress] nvarchar(450) NOT NULL,
	[PhoneNumber] nvarchar(450) NOT NULL,	

	CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED (Id ASC),
	)  ON [PRIMARY]

GO
