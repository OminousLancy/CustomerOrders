using Application.Models;
using Application.Orders.Dto;
using Application.Orders.Interfaces;
using Domain.Orders;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class OrderController(IOrderService orderService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<OrderDto>>> GetList(int page = 1, int take = 10, CancellationToken ct = default)
        => Ok(await orderService.GetListAsync(page, take, ct));

    [HttpPost]
    public async Task<ActionResult<OrderStatus>> ChangeStatus(int id, OrderStatus status, CancellationToken ct = default) {
        return Ok(await orderService.ChangeStatusAsync(id, status, ct));
    }
    
    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(OrderCreateDto dto, CancellationToken ct = default)
        => Ok(await orderService.CreateAsync(dto, ct));

    [HttpPost]
    public async Task<ActionResult<string>> AddOrderLineAsync(OrderLineCreateDto dto, CancellationToken ct = default)
    {
        await orderService.AddOrderLineAsync(dto, ct);
        return Ok("Добавлено");
    }
    [HttpDelete]
    public async Task<ActionResult<string>> DeleteOrderLineAsync(int orderId, int orderLineId, CancellationToken ct = default)
    {
        await orderService.DeleteOrderLineAsync(orderId, orderLineId, ct);
        return Ok("Удалено");
    }
}