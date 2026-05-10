IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(max) NOT NULL,
    [Login] nvarchar(max) NOT NULL,
    [Senha] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);

CREATE TABLE [Enderecos] (
    [Id] int NOT NULL IDENTITY,
    [Cep] nvarchar(max) NOT NULL,
    [Logradouro] nvarchar(max) NOT NULL,
    [Numero] nvarchar(max) NOT NULL,
    [Complemento] nvarchar(max) NULL,
    [Bairro] nvarchar(max) NOT NULL,
    [Cidade] nvarchar(max) NOT NULL,
    [UF] nvarchar(max) NOT NULL,
    [UsuarioId] int NOT NULL,
    CONSTRAINT [PK_Enderecos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Enderecos_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Enderecos_UsuarioId] ON [Enderecos] ([UsuarioId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260508024018_CriaçãoInicial', N'10.0.7');

COMMIT;
GO

