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

## Estrutura do Repositório
src/: Código-fonte do projeto

Dockerfile: Configuração do Docker para a aplicação

docker-compose.yml: Configuração do Docker Compose para o ambiente completo
