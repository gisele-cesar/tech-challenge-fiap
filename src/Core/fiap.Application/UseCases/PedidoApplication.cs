using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Application.UseCases
{
    public class PedidoApplication : IPedidoApplication
    {
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoApplication(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }
        public async Task<List<Pedido>> ObterPedidos()
        {
            return await _pedidoRepository.ObterPedidos();
        }
        public async Task<bool> Inserir(Pedido pedido)
        {
            return await _pedidoRepository.Inserir(pedido);
        }
        public async Task<Pedido> InserirPedido(Pedido pedido)
        {
            return await _pedidoRepository.InserirPedido(pedido);
        }
        public async Task<Pedido> ObterPedido(int idPedido)
        {

            return await _pedidoRepository.ObterPedido(idPedido);
        }
        public async Task<bool> Atualizar(Pedido pedido)
        {
            return await _pedidoRepository.Atualizar(pedido);
        }
    }
}
