CREATE TABLE [dbo].[EntityAttribute]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [EntityId] INT NOT NULL, 
    [AttributeId] INT NOT NULL, 
    [Value] NVARCHAR(MAX) NOT NULL, 
    CONSTRAINT [FK_EntityAttribute_ToUser] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_EntityAttribute_ToEntity] FOREIGN KEY ([EntityId]) REFERENCES [Entity]([Id]), 
    CONSTRAINT [FK_EntityAttribute_ToAttribute] FOREIGN KEY ([AttributeId]) REFERENCES [Attribute]([Id])
)
