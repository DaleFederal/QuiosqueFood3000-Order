#language: pt-BR
Funcionalidade: Pedido de Cliente

Como um cliente
Eu quero fazer um pedido
Para que eu possa receber minha comida

Cenário: Um cliente faz um pedido com produtos válidos
    Dado um cliente com um CPF válido
    E uma lista de produtos válidos
    Quando o cliente faz o pedido
    Então o pedido deve ser criado com sucesso