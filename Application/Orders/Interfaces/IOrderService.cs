using Application.Models;
using Application.Orders.Dto;
using Domain.Orders;

namespace Application.Orders.Interfaces;

public interface IOrderService
{
    Task<PaginatedResponse<OrderDto>> GetListAsync(int page, int take, CancellationToken ct = default);
    Task<OrderStatus> ChangeStatusAsync(int id, OrderStatus status, CancellationToken ct = default);
    Task<int> CreateAsync(OrderCreateDto dto, CancellationToken ct = default);
    Task<OrderDto> UpdateAsync(OrderDto dto, CancellationToken ct = default);
    Task AddOrderLineAsync(OrderLineCreateDto dto, CancellationToken ct = default);
    Task DeleteOrderLineAsync(int orderId, int orderLineId, CancellationToken ct = default);
}