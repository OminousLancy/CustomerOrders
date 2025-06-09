using Application.Customers.Dto;
using Application.Models;

namespace Application.Customers.Interfaces;

public interface ICustomerService
{
    Task<PaginatedResponse<CustomerDto>> GetListAsync(int page, int take, CancellationToken ct = default);
    Task<CustomerDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(CustomerCreateDto dto, CancellationToken ct = default);
    Task<CustomerDto> UpdateAsync(CustomerDto dto, CancellationToken ct = default);
}