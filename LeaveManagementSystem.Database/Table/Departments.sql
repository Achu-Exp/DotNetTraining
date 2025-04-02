CREATE TABLE [dbo].[Departments]
(
	[Id] INT NOT NULL IDENTITY ,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id]),
)
