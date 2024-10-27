USE PosTechFiap;

INSERT INTO CategoriaProduto(NomeCategoriaProduto, DataCriacao)
VALUES (('Lanche', CURRENT_TIMESTAMP), ('Acompanhamento', CURRENT_TIMESTAMP), ('Bebida', CURRENT_TIMESTAMP), ('Sobremesa', CURRENT_TIMESTAMP),)

INSERT INTO StatusPedido(DescricaoStatus)
VALUES (('Recebido'), ('Em preparação'), ('Pronto'), ('Finalizado'))