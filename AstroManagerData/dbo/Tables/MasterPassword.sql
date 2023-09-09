CREATE TABLE [dbo].[MasterPassword]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [HashedPassword] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_MasterPassword_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
