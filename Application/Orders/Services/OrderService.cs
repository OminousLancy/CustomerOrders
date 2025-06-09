using Application.Extensions;
using Application.Models;
using Application.Orders.Dto;
using Application.Orders.Interfaces;
using Application.Products.Dto;
using Domain.Orders;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Services;

public class OrderService(ApplicationDbContext context) : IOrderService
{
    public async Task<PaginatedResponse<OrderDto>> GetListAsync(int page, int take, CancellationToken ct = default)
    {
        var query = context.Orders
            .Include(o => o.OrderLines)
            .ThenInclude(l => l.Product)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerName = o.Customer.Name,
                Created = o.Created,
                Updated = o.Updated,
                Status = o.Status,
                OrderLines = o.OrderLines.Select(l => new OrderLineDto
                {
                    Id = l.Id,
                    Product = new ProductDto()
                    {
                        Id = l.Product.Id,
                        Name = l.Product.Name,
                        Price = l.Product.Price,
                    },
                    Count = l.Count,
                    Price = l.Price
                }).ToList()
            });
        var count = await query.CountAsync(ct);
        var orders = await query
            .ExecutePageFilter(page, take)
            .ToListAsync(ct);
        return new PaginatedResponse<OrderDto>(orders, count, page, take);
    }

    public async Task<OrderStatus> ChangeStatusAsync(int id, OrderStatus status, CancellationToken ct = default)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id, ct)
            ?? throw new Exception("Order not found");
        
        if (order.Status is OrderStatus.Cancelled or OrderStatus.Completed)
            throw new Exception("Cannot modify completed or cancelled order.");
        
        order.Status = status;
        order.Updated = DateTime.UtcNow;
        await context.SaveChangesAsync(ct);
        
        return order.Status;
    }

    public async Task<int> CreateAsync(OrderCreateDto dto, CancellationToken ct = default)
    {
        var order = new Order {
            CustomerId = dto.CustomerId,
            Created = DateTime.UtcNow,
            Status = OrderStatus.Created,
            OrderLines = dto.OrderLines.Select(l => new OrderLine {
                ProductId = l.Product.Id,
                Count = l.Count,
                Price = l.Price
            }).ToList()
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync(ct);
        return order.Id;
    }
    
    public async Task AddOrderLineAsync(OrderLineCreateDto dto, CancellationToken ct = default) 
    {
        var order = await context.Orders.Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == dto.OrderId, ct)
            ?? throw new Exception("Order not found");
        
        if (order.Status is OrderStatus.Completed or OrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot modify completed or cancelled order.");

        order.OrderLines.Add(new OrderLine {
            OrderId = order.Id,
            ProductId = dto.Product.Id,
            Count = dto.Count,
            Price = dto.Price
        });
        order.Updated = DateTime.UtcNow;
        await context.SaveChangesAsync(ct);
    }
    public async Task DeleteOrderLineAsync(int orderId, int orderLineId, CancellationToken ct = default) {
        var order = await context.Orders.Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct)
            ?? throw new Exception("Order not found");
        
        if (order.Status is OrderStatus.Completed or OrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot modify completed or cancelled order.");

        var line = order.OrderLines.FirstOrDefault(l => l.Id == orderLineId);
        if (line != null) {
            order.OrderLines.Remove(line);
            order.Updated = DateTime.UtcNow;
            await context.SaveChangesAsync(ct);
        }
    }
}