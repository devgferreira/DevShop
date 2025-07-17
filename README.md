# DevShop

DevShop é um projeto de microsserviço serverless que simula o processo de um pedido de uma loja, utilizando tecnologias da Amazon Web Services (AWS) e .NET. A arquitetura é projetada para ser escalável, aproveitando os benefícios da computação em nuvem.

A comunicação entre os diferentes serviços do sistema é realizada de forma assíncrona através do Amazon Simple Queue Service (SQS), garantindo que as mensagens sejam processadas de maneira confiável.

---

## Tecnologias Utilizadas

- **Amazon AWS:** Plataforma de serviços de computação em nuvem com um vasto portfólio global de serviços, como computação, armazenamento, bancos de dados, análise, redes, aprendizado de máquina, IA, IoT, segurança e desenvolvimento de aplicações.

- **AWS Lambda:** Serviço de computação serverless que executa código em resposta a eventos, sem necessidade de gerenciar servidores. Escala automaticamente e cobra apenas pelo tempo de execução do código.

- **Amazon DynamoDB:** Banco de dados NoSQL totalmente gerenciado, com desempenho rápido e escalabilidade contínua, ideal para aplicações com latência consistente de milissegundos.

- **Amazon SQS (Simple Queue Service):** Serviço de enfileiramento de mensagens para desacoplar e escalar microsserviços e aplicações serverless, eliminando a complexidade de gerenciar middleware de mensagens.

- **C#/.NET:** Plataforma de desenvolvimento gratuita, de código aberto e multiplataforma, usada para criar serviços backend robustos e aplicações em nuvem.

---
# Como Utilizar o Projeto

## Pré-requisitos
- Conta ativa na Amazon AWS.
- Visual Studio com extensão **AWS Toolkit** instalada.

## Passos para Configuração

1. **Criar uma IAM na AWS:**
   - Criar um usuário (User) com permissão `AdministratorAccess` (para fins de estudo).
   - Criar uma função (Role) com qualquer nome e permissão `AdministratorAccess` (para fins de estudo).

2. **Criar filas no Amazon SQS:**
   - Fila chamada `Pedido`.
   - Fila chamada `Reservado`.

3. **Criar tabelas no Amazon DynamoDB:**
   - Tabela chamada `pedidos`.
   - Tabela chamada `estoque`.

4. **Configurar triggers:**
   - Adicionar trigger do DynamoDB na Lambda do **Coletor**.
   - Adicionar trigger do SQS na fila `Pedido` na Lambda do **Reservador**, configurando para processar 1 mensagem por vez.

5. **Adicionar dados ao DynamoDB:**
   - Popular a tabela `estoque` manualmente.

6. **Desenvolvimento:**
   - Abrir o projeto no Visual Studio.
   - Utilizar C#/.NET para desenvolvimento e codificação.
   - Mudar a URL do SQS no .env, somente até os números. Exemplo: https://sqs.sa-east-1.amazonaws.com/156718323891/

---
## Publicando as Lambdas do Visual Studio para a AWS

### Pré-requisitos
- AWS Toolkit instalado no Visual Studio.
- Perfil AWS configurado com suas credenciais no Visual Studio.
- Projeto AWS Lambda (.NET Core) aberto no Visual Studio.

### Passos para publicar

1. **Configurar credenciais no Visual Studio:**
   - Abra o painel extensão e seleciona o AWS Toolkit e depois clica em getting starded.
   - Clique no `enable` do AWS Toolkit para adicionar seu perfil AWS com Access Key e Secret Key.
2. **Preparar o projeto Lambda:**
   - Confirme que o método handler está correto.

3. **Publicar a função:**
   - Clique com o botão direito no projeto Lambda na **Solution Explorer**.
   - Selecione **Publish to AWS Lambda...**

4. **Configurar a publicação:**
   - Escolha a região AWS.
   - Selecione uma função existente para atualizar ou crie uma nova.
   - Defina o nome da função Lambda.
   - Escolha a role do IAM para a função.
   - Configure memória e timeout conforme necessário.

5. **Publicar:**
   - Clique em **Publish** para enviar seu código para a AWS.
---


## Json de exemplos:

Link do arquivo de exemplo do pedido: [pedido](https://github.com/devgferreira/DevShop/blob/main/DevShop/pedido-examplo.json)

Link do arquivo de exemplo do estoque: [estoque](https://github.com/devgferreira/DevShop/blob/main/DevShop/estoque-exemplo.json)

---

## Arquitetura e Serviços

O fluxo de um pedido no DevShop é dividido entre um microsserviço principal (API) e duas funções Lambda que processam o pedido em etapas.

### APIs

#### Cadastrador

- Porta de entrada do sistema.
- Responsável por receber e criar pedidos.
- Expõe o endpoint: /api/pedido
- Recebe dados do cliente, produtos desejados e informações de pagamento para iniciar o processamento do pedido.

### Funções Lambda

#### Coletor

- Acionado automaticamente toda vez que um novo pedido é inserido na tabela do DynamoDB pelo serviço Cadastrador.
- Responsabilidades:
  - Capturar o evento do novo pedido.
  - Validar se os produtos listados existem no estoque.
  - Validar se o valor total está correto.
  - Enviar o pedido para a fila do SQS chamada `pedido` caso as validações sejam aprovadas.

#### Reservador

- Consome mensagens da fila `pedido` do SQS.
- Responsável pela lógica de estoque:
  - Verificar disponibilidade da quantidade de cada produto solicitado.
  - Reservar os itens caso haja estoque suficiente.
  - Enviar o pedido para a fila `reservado` do SQS para próximas etapas..
  - Se algum item estiver sem estoque, abortar a operação, devolver os produtos já reservados ao estoque e cancelar o pedido para manter a consistência dos dados.

---
## Biblioteca de Classes

Uma biblioteca de classes para serem usadas e compartilhadas por outros processos.

### Compartilhador

- Contém os objetos das entidades.
- Enums.
- Utilitários (utilities).



## Considerações Finais

Este projeto exemplifica o uso de uma arquitetura serverless desacoplada, utilizando os principais serviços da AWS e o ecossistema .NET para implementar um fluxo de pedido em microsserviços serveless.


