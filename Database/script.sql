-- TaskFlow API - Script de criação do banco (equivalente à migration InitialCreate)
-- Alternativa recomendada: dotnet ef database update
IF DB_ID('TaskFlowDb') IS NULL
    CREATE DATABASE TaskFlowDb;
GO

USE TaskFlowDb;
GO

CREATE TABLE Categorias (
    Id   INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Tarefas (
    Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Titulo        NVARCHAR(150) NOT NULL,
    Descricao     NVARCHAR(500) NULL,
    Status        NVARCHAR(20)  NOT NULL CONSTRAINT DF_Tarefas_Status DEFAULT 'Pendente',
    DataCriacao   DATETIME2     NOT NULL,
    DataConclusao DATETIME2     NULL,
    CategoriaId   INT           NULL,
    CONSTRAINT FK_Tarefas_Categorias_CategoriaId FOREIGN KEY (CategoriaId)
        REFERENCES Categorias (Id) ON DELETE SET NULL
);
GO

CREATE INDEX IX_Tarefas_CategoriaId ON Tarefas (CategoriaId);
GO

SET IDENTITY_INSERT Categorias ON;
INSERT INTO Categorias (Id, Nome) VALUES (1, 'Desenvolvimento'), (2, 'Design'), (3, 'Infraestrutura');
SET IDENTITY_INSERT Categorias OFF;
GO
