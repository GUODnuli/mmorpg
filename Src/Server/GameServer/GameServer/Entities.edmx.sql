
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/05/2024 16:08:03
-- Generated from EDMX file: D:\Projects\UnityProjects\My_mmorpg\Src\Server\GameServer\GameServer\Entities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ExtremeWorld];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerCharacter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Characters] DROP CONSTRAINT [FK_PlayerCharacter];
GO
IF OBJECT_ID(N'[dbo].[FK_CharacterItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TCharacterItems] DROP CONSTRAINT [FK_CharacterItem];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[Characters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Characters];
GO
IF OBJECT_ID(N'[dbo].[TCharacterItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TCharacterItems];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [RegisterDate] datetime  NULL,
    [Player_ID] int  NOT NULL
);
GO

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [ID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Characters'
CREATE TABLE [dbo].[Characters] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TID] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Class] int  NOT NULL,
    [MapID] int  NOT NULL,
    [MapPosX] int  NOT NULL,
    [MapPosY] int  NOT NULL,
    [MapPosZ] int  NOT NULL,
    [Player_ID] int  NOT NULL,
    [Bag_ID] int  NOT NULL
);
GO

-- Creating table 'TCharacterItems'
CREATE TABLE [dbo].[TCharacterItems] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ItemID] int  NOT NULL,
    [ItemCount] int  NOT NULL,
    [CharacterID] int  NOT NULL
);
GO

-- Creating table 'TCharacterBags'
CREATE TABLE [dbo].[TCharacterBags] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Items] varbinary(max)  NOT NULL,
    [Unlocked] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [PK_Characters]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TCharacterItems'
ALTER TABLE [dbo].[TCharacterItems]
ADD CONSTRAINT [PK_TCharacterItems]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TCharacterBags'
ALTER TABLE [dbo].[TCharacterBags]
ADD CONSTRAINT [PK_TCharacterBags]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Player_ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserPlayer]
    FOREIGN KEY ([Player_ID])
    REFERENCES [dbo].[Players]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPlayer'
CREATE INDEX [IX_FK_UserPlayer]
ON [dbo].[Users]
    ([Player_ID]);
GO

-- Creating foreign key on [Player_ID] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [FK_PlayerCharacter]
    FOREIGN KEY ([Player_ID])
    REFERENCES [dbo].[Players]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerCharacter'
CREATE INDEX [IX_FK_PlayerCharacter]
ON [dbo].[Characters]
    ([Player_ID]);
GO

-- Creating foreign key on [CharacterID] in table 'TCharacterItems'
ALTER TABLE [dbo].[TCharacterItems]
ADD CONSTRAINT [FK_CharacterItem]
    FOREIGN KEY ([CharacterID])
    REFERENCES [dbo].[Characters]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CharacterItem'
CREATE INDEX [IX_FK_CharacterItem]
ON [dbo].[TCharacterItems]
    ([CharacterID]);
GO

-- Creating foreign key on [Bag_ID] in table 'Characters'
ALTER TABLE [dbo].[Characters]
ADD CONSTRAINT [FK_CharacterBag]
    FOREIGN KEY ([Bag_ID])
    REFERENCES [dbo].[TCharacterBags]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CharacterBag'
CREATE INDEX [IX_FK_CharacterBag]
ON [dbo].[Characters]
    ([Bag_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------