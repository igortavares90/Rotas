CREATE DATABASE RotaViagemDb;
GO

USE RotaViagemDb;
GO

-- Criação da tabela
CREATE TABLE Rotas (
    Id INT PRIMARY KEY IDENTITY,
    Origem VARCHAR(3) NOT NULL,
    Destino VARCHAR(3) NOT NULL,
    Valor DECIMAL(10,2) NOT NULL
);
GO