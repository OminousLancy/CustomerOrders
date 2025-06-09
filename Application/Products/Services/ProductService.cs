using Application.Extensions;
using Application.Models;
using Application.Products.Dto;
using Application.Products.Interfaces;
using Domain.Products;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Services;

public class ProductService(ApplicationDbContext context) : IProductService
{
    public async Task<PaginatedResponse<ProductDto>> GetListAsync(int page, int take, CancellationToken ct = default)
    {
        var query = context.Products.AsNoTracking();
        
        var totalCount = await query.CountAsync(ct);
        var products = await query
            .ExecutePageFilter(page, take)
            .Select(c => new ProductDto
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price
            })
            .ToListAsync(ct);
        return new PaginatedResponse<ProductDto>(products, totalCount, page, take);
    }

    public async Task<int> CreateAsync(ProductCreateDto dto, CancellationToken ct = default)
    {
        var product = new Product {
            Name = dto.Name,
            Price = dto.Price
        };
        context.Products.Add(product);
        await context.SaveChangesAsync(ct);
        return product.Id;
    }

    public async Task<ProductDto> UpdateAsync(ProductDto dto, CancellationToken ct = default)
    {
        var product = await context.Products.FirstOrDefaultAsync(c => c.Id == dto.Id, cancellationToken: ct)
                       ?? throw new Exception("Product not found");

        product.Name = dto.Name;
        product.Price = dto.Price;

        await context.SaveChangesAsync(ct);
        return dto;
    }
}