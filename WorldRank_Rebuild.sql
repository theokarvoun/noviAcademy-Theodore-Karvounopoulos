/* =============================================================================
   Rebuild the WorldRank database from scratch.
   Schema matches EF Core migration 20260712200945_InitialCreate (EF Core 10.0.9).

   WARNING: this DROPS the existing WorldRank database and ALL its data.
   Run against a SQL Server where the app's connection string points
   (Server=localhost, Trusted_Connection=True).
   ============================================================================= */

USE master;
GO

/* Drop the existing database (kick out any open connections first). */
IF DB_ID(N'WorldRank') IS NOT NULL
BEGIN
    ALTER DATABASE [WorldRank] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [WorldRank];
END
GO

CREATE DATABASE [WorldRank];
GO

USE [WorldRank];
GO

/* ---------------------------------------------------------------------------
   Players
   --------------------------------------------------------------------------- */
CREATE TABLE [dbo].[Players]
(
    [Id]    uniqueidentifier NOT NULL,
    [Name]  nvarchar(200)    NOT NULL,
    [Score] int              NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([Id])
);
GO

/* ---------------------------------------------------------------------------
   Wallets
   --------------------------------------------------------------------------- */
CREATE TABLE [dbo].[Wallets]
(
    [Id]        uniqueidentifier NOT NULL,
    [PlayerId]  uniqueidentifier NOT NULL,
    [Currency]  nvarchar(3)      NOT NULL,
    [Balance]   decimal(18,2)    NOT NULL,
    [IsBlocked] bit              NOT NULL,
    CONSTRAINT [PK_Wallets] PRIMARY KEY ([Id])
);
GO

/* ---------------------------------------------------------------------------
   EF Core migrations history — records InitialCreate as already applied so
   `dotnet ef database update` / `Update-Database` treats the schema as current.
   --------------------------------------------------------------------------- */
CREATE TABLE [dbo].[__EFMigrationsHistory]
(
    [MigrationId]    nvarchar(150) NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL,
    CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
);
GO

INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260712200945_InitialCreate', N'10.0.9');
GO

PRINT 'WorldRank rebuilt: Players, Wallets, __EFMigrationsHistory created.';
GO
