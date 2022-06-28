using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCases.Products.GetProducts.Dtos;

namespace UseCases.Products.GetProductById;

/// <summary>
/// Handle get product by id query.
/// </summary>
public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IDbContext _context;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Handler for <see cref="GetProductByIdQuery"/>.
    /// </summary>
    /// <param name="request"><see cref="GetProductByIdQuery"/> request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(product => product.Category)
            .ThenInclude(category => category.Icon)
            .Include(product => product.Image)
            .Include(product => product.Producer)
            .ThenInclude(producer => producer.Image)
            .FirstOrDefaultAsync(product => product.Id == request.ProductId, cancellationToken);

        if (product == default)
        {
            throw new ArgumentException($"Product with id = {request.ProductId} not found");
        }

        return _mapper.Map<ProductDto>(product);
    }
}