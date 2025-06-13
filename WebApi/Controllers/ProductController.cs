using Application.Models;
using Application.Products.Dto;
using Application.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ProductController(IProductService productService) : BaseController
{
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<ProductDto>>> GetList(int page = 1, int take = 10, CancellationToken ct = default)
        => Ok(await productService.GetListAsync(page, take, ct));
    
    [HttpPost]
    public async Task<ActionResult<int>> Create(ProductCreateDto dto, CancellationToken ct = default)
        => Ok(await productService.CreateAsync(dto, ct));
    
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Update(ProductDto dto, CancellationToken ct = default)
        => Ok(await productService.UpdateAsync(dto, ct));
}