CREATE TABLE [dbo].[RecoveryKey]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [HashedKey] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_RecoveryKey_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
