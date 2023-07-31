
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/24/2023 16:42:35
-- Generated from EDMX file: C:\Users\pc\Desktop\NodlaysProjects\Car_System\car_system\Models\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CarDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CarDetail];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CarDetails'
CREATE TABLE [dbo].[CarDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LotYear] nvarchar(20)  NULL,
    [LotMake] nvarchar(255)  NULL,
    [LotModel] nvarchar(255)  NULL,
    [LotRunCondition] nvarchar(255)  NULL,
    [DamageTypeDescription] nvarchar(255)  NULL,
    [CopartFacilityName] nvarchar(255)  NULL,
    [SaleTitleState] nvarchar(255)  NULL,
    [SaleTitleType] nvarchar(255)  NULL,
    [DamageType] nvarchar(255)  NULL,
    [LotColor] nvarchar(255)  NULL,
    [HasKey] nvarchar(255)  NULL,
    [OdometerReading] nvarchar(20)  NULL,
    [SalePrice] nvarchar(20)  NULL,
    [RepairCost] nvarchar(20)  NULL,
    [IsActive] int  NULL,
    [CreatedAt] datetime  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NULL,
    [Contact] nvarchar(255)  NULL,
    [Gender] int  NULL,
    [Email] nvarchar(255)  NULL,
    [Password] nvarchar(max)  NULL,
    [Role] int  NULL,
    [IsActive] int  NULL,
    [CreatedAt] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CarDetails'
ALTER TABLE [dbo].[CarDetails]
ADD CONSTRAINT [PK_CarDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------