
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PosTechFiap')
    CREATE DATABASE PosTechFiap;

USE PosTechFiap;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Cliente')
BEGIN
    CREATE TABLE Cliente (
        IdCliente IDENTITY INT PRIMARY KEY,
        NomeCliente VARCHAR(255) NOT NULL,
        Cpf VARCHAR(11) NOT NULL,
        Email VARCHAR(255) NOT NULL,
        DataCriacao TIMESTAMP NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CategoriaProduto')
BEGIN
    CREATE TABLE CategoriaProduto (
        IdCategoriaProduto IDENTITY INT PRIMARY KEY,
        NomeCategoriaProduto VARCHAR(255) NOT NULL,
        DataCriacao TIMESTAMP NOT NULL
    );
END -- subir script de insert das categorias = Lanche, Acompanhamento, Bebida, Sobremesa 

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Produto')
BEGIN
    CREATE TABLE Produto (
        IdProduto IDENTITY INT PRIMARY KEY,
        IdCategoriaProduto INT,
        NomeProduto VARCHAR(255) NOT NULL,
        DescricaoProduto VARCHAR(255) NOT NULL,
        PrecoProduto DECIMAL NOT NULL,
        DataCriacao TIMESTAMP NOT NULL,
        DataAlteracao TIMESTAMP NOT NULL
        CONSTRAINT FK_CategoriaProduto FOREIGN KEY (IdCategoriaProduto) REFERENCES CategoriaProduto(IdCategoriaProduto)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StatusPedido')
BEGIN
    CREATE TABLE StatusPedido (
        IdStatus IDENTITY INT PRIMARY KEY,
        DescricaoStatus VARCHAR(50) NOT NULL
    )
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pedido')
BEGIN
    CREATE TABLE Pedido (
        IdPedido IDENTITY INT PRIMARY KEY,
        IdCliente INT NOT NULL,
        NumeroPedido VARCHAR(255) NOT NULL,
        IdStatus INT NOT NULL,
        ValorTotalPedido DECIMAL NOT NULL,
        DataCriacao TIMESTAMP NOT NULL,
        DataAlteracao TIMESTAMP NOT NULL
        CONSTRAINT FK_Cliente FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente)
        CONSTRAINT FK_StatusPedido FOREIGN KEY (IdStatus) REFERENCES StatusPedido(IdStatus)
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ItemPedido')
BEGIN
    CREATE TABLE ItemPedido (
        IdPedido INT NOT NULL,
        IdProduto INT NOT NULL
    );
END
