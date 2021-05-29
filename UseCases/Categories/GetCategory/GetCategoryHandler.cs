using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCases.Products.GetProducts.Dtos;

namespace UseCases.Categories.GetCategory
{
    /// <summary>
    /// Handle query for get category.
    /// </summary>
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public GetCategoryHandler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handler of query for get category query.
        /// </summary>
        /// <param name="request">Request for get category.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var requiredCategory = await _context.Categories
                .Include(category => category.Icon)
                .FirstOrDefaultAsync(category => category.RouteName == request.RouteName, cancellationToken);

            if (requiredCategory == default)
            {
                throw new ArgumentException($"Category with route name {request.RouteName} not found");
            }

            return _mapper.Map<CategoryDto>(requiredCategory);
        }
    }
}