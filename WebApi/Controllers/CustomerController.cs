using Application.Customers.Dto;
using Application.Customers.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CustomerController(ICustomerService customerService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<CustomerDto>>> GetList(int page = 1, int take = 10, CancellationToken ct = default)
        => Ok(await customerService.GetListAsync(page, take, ct));

    [HttpGet]
    public async Task<ActionResult<CustomerDto>> GetById(int id, CancellationToken ct = default) {
        var res = await customerService.GetByIdAsync(id, ct);
        return res is null ? NotFound() : Ok(res);
    }
    
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create(CustomerCreateDto dto, CancellationToken ct = default)
        => Ok(await customerService.CreateAsync(dto, ct));
    
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Update(CustomerDto dto, CancellationToken ct = default)
        => Ok(await customerService.UpdateAsync(dto, ct));
}