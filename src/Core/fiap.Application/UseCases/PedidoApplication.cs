using fiap.Application.Interfaces;
using fiap.Domain.Entities;
using fiap.Domain.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fiap.Application.UseCases
{
    public class PedidoApplication : IPedidoApplication
    {
        private readonly ILogger _logger;
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoApplication(ILogger logger, IPedidoRepository pedidoRepository)
        {
            _logger = logger;
            _pedidoRepository = pedidoRepository;
        }
        public async Task<List<Pedido>> ObterPedidos()
        {
            try
            {
                _logger.Information("Buscando lista de pedidos.");
                return await _pedidoRepository.ObterPedidos();
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter pedidos. Erro: {ex.Message}");
                throw;
            }
        }
        public async Task<List<Pedido>> ObterPedidosPorStatus(string status1, string status2, string status3)
        {
            try
            {
                _logger.Information($"Buscando lista de pedidos por status na ordem: {status1}, {status2} e {status3}");
                return await _pedidoRepository.ObterPedidosPorStatus(status1, status2, status3);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter pedidos por status {status1}, {status2} e {status3}. Erro: {ex.Message}");
                throw;
            }
        }
        public async Task<Pedido> Inserir(Pedido pedido)
        {
            try
            {
                _logger.Information($"Inserindo novo pedido numero: {pedido.Numero}");
                return await _pedidoRepository.Inserir(pedido);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao inserir pedido {pedido.Numero}. Erro: {ex.Message}");
                throw;
            }
        }
        public async Task<Pedido> ObterPedido(int idPedido)
        {
            try
            {
                _logger.Information($"Buscando pedido por id: {idPedido}.");
                return await _pedidoRepository.ObterPedido(idPedido);

            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao obter pedido por id {idPedido}. Erro: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> Atualizar(Pedido pedido)
        {
            try
            {
                _logger.Information($"Atualizando pedido id: {pedido.IdPedido}.");
                return await _pedidoRepository.Atualizar(pedido);
            }
            catch (Exception ex)
            {
                _logger.Error($"Erro ao atualizar pedido id {pedido.IdPedido}. Erro: {ex.Message}");
                throw;
            }
        }
    }
}
