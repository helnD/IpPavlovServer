using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Infrastructure.Abstractions;
using Infrastructure.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCases.Products.GetProducts.Dtos;

namespace UseCases.Products.GetProducts;

/// <summary>
/// Handle get products query.
/// </summary>
public class GetProductsHandler : IRequestHandler<GetProductsQuery, Page<ProductDto>>
{
    private readonly IDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsHandler(IDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Get products by specific name, category and producer.
    /// </summary>
    /// <param name="request"><see cref="GetProductsQuery"/> request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Page with products.</returns>
    public async Task<Page<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Product> allProducts = _context.Products
            .Include(product => product.Category)
            .ThenInclude(category => category.Icon)
            .Include(product => product.Image)
            .Include(product => product.Producer)
            .ThenInclude(producer => producer.Image);

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var upperName = request.Name.ToUpper();
            allProducts = allProducts.Where(product => product.Description.ToUpper().Contains(upperName));
        }

        if (!string.IsNullOrWhiteSpace(request.ProducerIds))
        {
            var ids = ParseIds(request.ProducerIds);
            allProducts = allProducts.Where(product => ids.Contains(product.Producer.Id));
        }

        if (!string.IsNullOrWhiteSpace(request.CategoryIds))
        {
            var ids = ParseIds(request.CategoryIds);
            allProducts = allProducts.Where(product => ids.Contains(product.Category.Id));
        }

        var orderedProducts = allProducts.OrderBy(product => product.Description);

        var products = await Page<ProductDto>.CreatePageAsync(orderedProducts, request.PageNumber, request.PageSize, cancellationToken,
            product => _mapper.Map<ProductDto>((Product)product));

        foreach (var product in products.Content)
        {
            product.Image ??= product.Category.Icon;
        }

        return products;
    }

    private int[] ParseIds(string ids)
    {
        return ids.Split(',').Select(id =>
        {
            if (int.TryParse(id, out var number))
            {
                return number;
            }

            throw new ArgumentException($"argument {ids} is incorrect");
        }).ToArray();
    }
}