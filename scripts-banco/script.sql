CREATE DATABASE posTechTeste
GO

USE posTechTeste
GO

CREATE TABLE dbo.Cliente(
 Id int NOT NULL IDENTITY(1,1),
 Nome varchar(200) NOT NULL,
 Email varchar(100) NOT NULL,
 CONSTRAINT PK_Client PRIMARY KEY (Id)
)
GO