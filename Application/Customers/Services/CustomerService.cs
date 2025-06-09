using Application.Customers.Dto;
using Application.Customers.Interfaces;
using Application.Extensions;
using Application.Models;
using Domain.Customers;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Customers.Services;

public class CustomerService(ApplicationDbContext context) : ICustomerService
{
    public async Task<PaginatedResponse<CustomerDto>> GetListAsync(int page, int take, CancellationToken ct)
    {
        var query = context.Customers.AsNoTracking();
        
        var totalCount = await query.CountAsync(ct);
        var customers = await query
            .ExecutePageFilter(page, take)
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                LastName = c.LastName,
                Address = c.Address,
                PhotoUrl = c.PhotoUrl
            })
            .ToListAsync(ct);
        return new PaginatedResponse<CustomerDto>(customers, totalCount, page, take);
    }

    public async Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Customers
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                LastName = c.LastName,
                Address = c.Address,
                PhotoUrl = c.PhotoUrl
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<int> CreateAsync(CustomerCreateDto dto, CancellationToken ct)
    {
        var customer = new Customer {
            Name = dto.Name,
            LastName = dto.LastName,
            Address = dto.Address,
            PhotoUrl = dto.PhotoUrl
        };
        context.Customers.Add(customer);
        await context.SaveChangesAsync(ct);
        return customer.Id;
    }

    public async Task<CustomerDto> UpdateAsync(CustomerDto dto, CancellationToken ct)
    {
        var customer = await context.Customers.FirstOrDefaultAsync(c => c.Id == dto.Id, cancellationToken: ct)
                       ?? throw new Exception("Customer not found");

        customer.Name = dto.Name;
        customer.LastName = dto.LastName;
        customer.Address = dto.Address;
        customer.PhotoUrl = dto.PhotoUrl;

        await context.SaveChangesAsync(ct);
        return dto;
    }
}