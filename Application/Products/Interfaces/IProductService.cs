using Application.Models;
using Application.Products.Dto;

namespace Application.Products.Interfaces;

public interface IProductService
{
    Task<PaginatedResponse<ProductDto>> GetListAsync(int page, int take, CancellationToken ct = default);
    Task<int> CreateAsync(ProductCreateDto dto, CancellationToken ct = default);
    Task<ProductDto> UpdateAsync(ProductDto dto, CancellationToken ct = default);
}