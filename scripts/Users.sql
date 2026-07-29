CREATE TABLE [Users](
    [Id] uniqueidentifier NOT NULL,
    [Active] bit NOT NULL,
    [Name] nvarchar(150) NOT NULL,
    [Email] nvarchar(255) NOT NULL,
    [Password] nvarchar(2000) NOT NULL,
    PRIMARY KEY([Id])
)
GO