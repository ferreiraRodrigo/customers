# Customers API

Essa API permite a criação, login e gerenciamento de uma lista de produtos favoritos (wishlist). 

## **Especificações**
Algumas das especificações levadas em consideração durante o desenvolvimento:
 - Deve ser possível criar, atualizar, visualizar e remover **Clientes**
baseUrl
     - O cadastro dos clientes deve conter apenas seu nome e endereço de
e-mail
     - Um cliente não pode se registrar duas vezes com o mesmo endereço
de e-mail
 - Cada cliente só deverá ter uma única lista de produtos favoritos
 - Em uma lista de produtos favoritos podem existir uma quantidade ilimitada
de produtos
     - Um produto não pode ser adicionado em uma lista caso ele não exista
     - Um produto não pode estar duplicado na lista de produtos favoritos de
um cliente
     - A documentação da API de produtos pode ser visualizada [neste link](https://gist.github.com/Bgouveia/9e043a3eba439489a35e70d1b5ea08ec)
 - O dispositivo que irá renderizar a resposta fornecida por essa nova API irá
apresentar o Título, Imagem, Preço e irá utilizar o ID do produto para formatar
o link que ele irá acessar. Quando existir um review para o produto, o mesmo
será exibido por este dispositivo. Não é necessário criar um frontend para
simular essa renderização (foque no desenvolvimento da API).
 - O acesso à api deve ser aberto ao mundo, porém deve possuir autenticação
e autorização.

## **Principais Tecnologias**
 - ASP.NET 5 (C#)
     - **Entity Framework Core** (Mapeamento de banco de dados)
     - **ProblemDetails** (Padronização de erros)
     - **Serilog** (Melhoria de logs)
     - **Swashbuckle** (Geração de documentação)
     - **Identity Model** (Gerenciamento de tokens, que futuramente será substituido por uma solução mais completa, como o IdentityServer4)
     - **FluentValidation** (Melhoria nas validações realizadas nos contratos da API)
     - **xUnit** (Testes unitários - em desenvolvimento)
 - PostgreSQL
 - Docker

## **Começando a utilizar**

Para utilizar esta aplicação de forma simplificada é necessário possuir o [Docker](https://www.docker.com/) instalado. Com o docker instalado, basta executar o arquivo de docker-compose.yaml que existe no repositório usando o comando:

```
docker-compose up
```

Caso não possua o [Docker](https://www.docker.com/) instalado, será necessário instalar uma instância local do [PostgreSQL](https://www.postgresql.org/) e configurar a aplicação (appsettings.json).

Obs: Caso tenha problemas para executar a aplicação localmente, existe também uma versão dela rodando na AWS (temporariamente): http://ec2-50-16-171-87.compute-1.amazonaws.com:5001 (HTTP apenas).

# **Criando um cliente e adicionando produtos na lista de desejos**

Com a aplicação funcionando localmente em http://localhost:5001 (HTTP apenas), é possível agora executar suas operações. Para auxiliar o consumo da API, existe uma collection do [Postman](https://www.postman.com/) na diretório ./collections/Customers.postman_collection.json. Também é possível mudar o endereço do **baseUrl** nas variáveis de collection e apontar ela para a aplicação presente na AWS (http://ec2-50-16-171-87.compute-1.amazonaws.com:5001)

Como a API possui autenticação e autorização, premeiro precisamos criar nosso cliente e suas credências.

### POST /customers
```json
{
  "name": "Rodrigo",
  "email": "ferreira.rodrigosf@gmail.com",
  "password": "minhaSenha@123"
}
```

Agora com o cliente criado, podemos fazer o login (autenticação). Para fazer isso precisamos passar nossas credênciais e os tipos de permissões que o token gerado terám, separadas por espaço. Por padrão, todo usuário criado tem acesso ao seguintes *scopes* (permissões):
 - read:customers (Operações de leitura de informações do cliente)
 - write: customers (Operações de escrita nas informações do cliente)
 - read:wishlist (Operações de leitura de informações na lista de favoritos do cliente)
 - write:wishlist (Operações de escrita nas informações da lista de favoritos do cliente)

### POST /authentication/login
```json
{
  "email": "ferreira.rodrigosf@gmail.com",
  "password": "minhaSenha@123",
  "scopes": "read:customer write:customer read:wishlist write:wishlist"
}
```

Depois de feito o login, recebemos um token é utilizado para consumir as outras operações da API.

Obs: Na collection presente no repositório, o token é adicionado em uma variável de collection já configurada automaticamente.

Agora que temos um cliente criado e autenticado, é possível adicionar um produto à sua lista de favoritos.

### POST /wishlist/products/**1bf0f365-fbdd-4e21-9786-da459d78dd1f**

Passando na URI o id do produto, caso ele exista, a aplicação nos identifica atráves da identidade contidade dentro do token [JWT](https://jwt.io/) e adiciona o produto em nossa lista de favoritos. 

A documentação utilizando o [Swagger](https://swagger.io/) com todas as operações pode ser obtida em /swagger e também está presente na collection disponibilizada.

## Melhorias
Na versão inicial dessa API diversas implementações foram feitas de forma simplificada e serão alteradas no futuro. Existem diversas delas já mapeadas para melhorias.

 - **Melhoria na autenticação e autorização.** Ele foi feita de forma simplificada, mas existe formas mais completas de realizar essas operações usando o IdentityServer4.
 - **Armazenamento de senha.** As senhas foram guardadas de forma simples no banco, sem nenhum tipo de criptografia.
 - **Estrutura do registro de serviços.** Muitos serviços são registrados na class *Startup.cs* e isso pode ser melhorado extraindo a configuração dos serviços para fora dela.
 - **Registro de logs.** Os logs estão sendo escrito no console e registro em um arquivo não estruturado. Isso pode ser melhorado utilizando o uso do *Serilog* para estruturar o log e até mesmo guardado de forma a utilizar toda stack do [Elasticsearch](https://www.elastic.co/pt/what-is/elasticsearch).
 - **Cache de resultados**. Hoje a aplicação não possui nenhum tipo de cache e isso pode ser melhorado com a implementação de ferramentas que permitem esse tipo de operação, como o Redis.