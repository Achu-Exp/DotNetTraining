CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL IDENTITY ,
    [Name] VARCHAR(100) NULL,
    [Email] NVARCHAR(150)  NULL,
    [Address] NVARCHAR(355)  NULL,
    [Password] NVARCHAR(MAX) NULL,
    [DepartmentId] INT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT FK_Users_Department FOREIGN KEY ([DepartmentId]) REFERENCES Departments(Id) ON DELETE NO ACTION

)
