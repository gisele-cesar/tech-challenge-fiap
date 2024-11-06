# Tech Challenge Projeto de Backend com Arquitetura Hexagonal

## Descrição

Este projeto consiste no desenvolvimento de um sistema de backend (monolito) utilizando arquitetura hexagonal.
O projeto contempla a implementação de APIs para cadastro de clientes, produtos, pedidos e checkout e foi desenvolvido utilizando a linguagem C# com .NET8 e banco de dados SQL Server.

## Objetivos

1. Desenvolvimento de uma aplicação backend seguindo os padrões apresentados nas aulas do curso Pos Tech FIAP:
   - Utilizando arquitetura hexagonal
   - Implementação das seguintes APIs:
     - Cadastro do Cliente
     - Identificação do Cliente via CPF
     - Criar, editar e remover produtos
     - Buscar produtos por categoria
     - Fake checkout (enviar produtos escolhidos para a fila)
     - Listar os pedidos

2. Configuração do Docker e docker compose para execução do ambiente completo:
   - Dockerfile para a aplicação
   - docker-compose.yml para subir o ambiente

## Como Iniciar o Projeto Localmente

### Pré-requisitos

- Possuir o Docker instalado. Veja mais informações aqui: https://docs.docker.com/get-started/

### Passos para executar o projeto

1. Clone o repositório e acesse a pasta do projeto:

```bash
git clone https://github.com/gisele-cesar/tech-challenge-fiap.git
cd tech-challenge-fiap
```

2. Construa e execute os contêineres usando Docker Compose:

   Obs.: Inicialize o docker para executar o comando a seguir:

```bash
docker-compose up
```
3. Aguarde até a finalização da criação dos contêineres e execução do script sqlserver.

4. Após a execução dos comandos, acesse a documentação das APIs (Swagger) em http://localhost:8080/swagger/index.html.

## Como testar as API's do Projeto

### Pré-requisitos

- Containeres rodando no Docker
- Acessar o swagger em http://localhost:8080/swagger/index.html

### Passos para executar o projeto

1. Acessar a API Cliente (POST /Cliente) para realizar cadastro de um ou mais clientes.
2. Acessar a API Produto (POST /Produto) para realizar cadastro de um ou mais produtos (na documentação das API's /Produto consta lista de Id's de categoria do produto para o cadastro).
3. Acessar a API de Pedido (POST /Pedido) para realizar cadastro de um mais pedidos.

- Obs.: Exemplo de requisição consta na documentação do swagger para cada uma das API's acima.
- Obs.2: é necessário que seja realizado o cadastro de cliente e de produto antes de realizar o cadastro de pedidos.

## Estrutura do Repositório
src/: Código-fonte do projeto

Dockerfile: Configuração do Docker para a aplicação

docker-compose.yml: Configuração do Docker Compose para o ambiente completo
