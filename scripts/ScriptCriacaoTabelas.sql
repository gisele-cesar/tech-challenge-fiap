
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PosTechFiap')
  CREATE DATABASE PosTechFiap;
GO

USE PosTechFiap;
GO

IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Pedido')
	DROP TABLE Pedido;
GO 
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'StatusPedido')
	DROP TABLE StatusPedido;
GO 
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Produto')
	DROP TABLE Produto;
GO 
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'CategoriaProduto')
	DROP TABLE CategoriaProduto;
GO 
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ItemPedido')
	DROP TABLE ItemPedido;
GO 
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Cliente')
	DROP TABLE Cliente;
GO 

CREATE TABLE Cliente (
        IdCliente INT IDENTITY PRIMARY KEY,
        NomeCliente VARCHAR(255) NOT NULL,
        Cpf VARCHAR(11) NOT NULL,
        Email VARCHAR(255) NOT NULL,
        DataCriacao DATETIME NOT NULL
);

CREATE TABLE CategoriaProduto (
        IdCategoriaProduto INT IDENTITY PRIMARY KEY,
        NomeCategoriaProduto VARCHAR(255) NOT NULL,
        DataCriacao DATETIME NOT NULL
    );

CREATE TABLE Produto (
        IdProduto INT IDENTITY PRIMARY KEY,
        IdCategoriaProduto INT,
        NomeProduto VARCHAR(255) NOT NULL,
        DescricaoProduto VARCHAR(255) NOT NULL,
        PrecoProduto DECIMAL NOT NULL,
        DataCriacao DATETIME NOT NULL,
        DataAlteracao DATETIME NOT NULL,
        CONSTRAINT FK_CategoriaProduto FOREIGN KEY (IdCategoriaProduto) REFERENCES CategoriaProduto(IdCategoriaProduto)
    );

    CREATE TABLE StatusPedido (
        IdStatus INT IDENTITY PRIMARY KEY,
        DescricaoStatus VARCHAR(50) NOT NULL
    )


    CREATE TABLE Pedido (
        IdPedido INT IDENTITY PRIMARY KEY,
        IdCliente INT NOT NULL,
        NumeroPedido VARCHAR(255) NOT NULL,
        IdStatus INT NOT NULL,
        ValorTotalPedido DECIMAL NOT NULL,
        DataCriacao DATETIME NOT NULL,
        DataAlteracao DATETIME NOT NULL,
        CONSTRAINT FK_Cliente FOREIGN KEY (IdCliente) REFERENCES Cliente(IdCliente),
        CONSTRAINT FK_StatusPedido FOREIGN KEY (IdStatus) REFERENCES StatusPedido(IdStatus)
    );


    CREATE TABLE ItemPedido (
        IdPedido INT NOT NULL,
        IdProduto INT NOT NULL
    );

go

INSERT INTO CategoriaProduto(NomeCategoriaProduto, DataCriacao)
VALUES ('Lanche', getdate()), 
	   ('Acompanhamento', getdate()), 
	   ('Bebida', getdate()), 
	   ('Sobremesa', getdate());

INSERT INTO StatusPedido(DescricaoStatus)
VALUES ('Recebido'), 
	   ('Em preparação'), 
	   ('Pronto'), 
	   ('Finalizado');