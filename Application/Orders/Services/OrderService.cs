using Application.Extensions;
using Application.Models;
using Application.Orders.Dto;
using Application.Orders.Interfaces;
using Application.Products.Dto;
using Domain.Orders;
using Domain.Products;
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
                CustomerId = o.Customer.Id,
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
        if(dto.OrderLines.Count == 0) throw new Exception("Order line count can't be zero.");
        
        var order = new Order {
            CustomerId = dto.CustomerId,
            Created = DateTime.UtcNow,
            Status = OrderStatus.Created,
            OrderLines = dto.OrderLines.Select(l => new OrderLine {
                ProductId = l.Product.Id,
                Count = l.Count,
                Price = l.Product.Price * l.Count
            }).ToList()
        };
        context.Orders.Add(order);
        await context.SaveChangesAsync(ct);
        return order.Id;
    }

    public async Task<OrderDto> UpdateAsync(OrderDto dto, CancellationToken ct)
    {
        var order = await context.Orders
                        .Include(o => o.OrderLines)
                        .FirstOrDefaultAsync(c => c.Id == dto.Id, cancellationToken: ct)
                       ?? throw new Exception("Order not found");
        
        if (order.Status is OrderStatus.Completed or OrderStatus.Cancelled)
            throw new Exception("Cannot modify completed or cancelled order.");
        
        var newOrderLines = dto.OrderLines.Select(l => new OrderLine
        {
            Id = l.Id,
            ProductId = l.Product.Id,
            Count = l.Count,
            Price = l.Product.Price * l.Count,
            OrderId = l.OrderId,
            Product = new Product
            {
                Id = l.Product.Id,
                Name = l.Product.Name,
                Price = l.Product.Price,
            }
        }).ToList();
        order.OrderLines = newOrderLines;
        order.Status = dto.Status;
        order.Updated = DateTime.UtcNow;
        await context.SaveChangesAsync(ct);
        return dto;
    }
    public async Task AddOrderLineAsync(OrderLineCreateDto dto, CancellationToken ct = default) 
    {
        var order = await context.Orders.Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == dto.OrderId, ct)
            ?? throw new Exception("Order not found");
        
        if (order.Status is OrderStatus.Completed or OrderStatus.Cancelled)
            throw new Exception("Cannot modify completed or cancelled order.");

        order.OrderLines.Add(new OrderLine {
            OrderId = order.Id,
            ProductId = dto.Product.Id,
            Count = dto.Count,
            Price = dto.Product.Price * dto.Count
        });
        order.Updated = DateTime.UtcNow;
        await context.SaveChangesAsync(ct);
    }
    public async Task DeleteOrderLineAsync(int orderId, int orderLineId, CancellationToken ct = default) {
        var order = await context.Orders.Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == orderId, ct)
            ?? throw new Exception("Order not found");
        
        if (order.Status is OrderStatus.Completed or OrderStatus.Cancelled)
            throw new Exception("Cannot modify completed or cancelled order.");

        var line = order.OrderLines.FirstOrDefault(l => l.Id == orderLineId);
        if (line != null) {
            order.OrderLines.Remove(line);
            order.Updated = DateTime.UtcNow;
            await context.SaveChangesAsync(ct);
        }
    }
}