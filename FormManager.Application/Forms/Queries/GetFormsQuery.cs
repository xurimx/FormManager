using FormManager.Application.Common.Interfaces;
using FormManager.Application.Common.Models;
using FormManager.Domain.Entities;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Forms.Queries
{
    public class GetFormsQuery : RequestQuery<Pagination<Form>> {}

    public class GetFormsQueryHandler : IRequestHandler<GetFormsQuery, Pagination<Form>>
    {
        private readonly IAppDbContext context;

        public GetFormsQueryHandler(IAppDbContext context)
        {
            this.context = context;
        }

        async Task<Pagination<Form>> IRequestHandler<GetFormsQuery, Pagination<Form>>.Handle(GetFormsQuery request, CancellationToken cancellationToken)
        {
            int page = request.Page ?? 1;
            int limit = request.Limit ?? 10;
            int total = await context.Forms.CountAsync();
            
            var query = context.Forms.AsQueryable();
            if (!string.IsNullOrEmpty(request.SearchInput))
            {
                if (request.SearchColumns.Length != 0)
                {
                    var predicate = PredicateBuilder.New<Form>();                    
                    foreach (var column in request.SearchColumns)
                    {
                        ParameterExpression param = Expression.Parameter(typeof(Form), "form");
                        MemberExpression property = Expression.Property(param, column);
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        ConstantExpression inputValue = Expression.Constant(request.SearchInput, typeof(string));
                        MethodCallExpression containsMethodExpression = Expression.Call(property, method, inputValue);                        
                        predicate = predicate.Or(Expression.Lambda<Func<Form, bool>>(containsMethodExpression, param));
                    }
                    query = query.Where(predicate);
                }
                else
                {
                    query = query.Where(x =>
                            x.Name.Contains(request.SearchInput) ||
                            x.Company.Contains(request.SearchInput) ||
                            x.Telephone.Contains(request.SearchInput));
                }
            }
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                ParameterExpression param = Expression.Parameter(typeof(Form), "x");
                MemberExpression property = Expression.Property(param, request.OrderBy);
                var lambda = Expression.Lambda(property, param);

                var orderBy = Expression.Call(typeof(Queryable),
                                request.OrderDirection != "desc" ? "OrderBy" : "OrderByDescending",
                                new Type[] { typeof(Form), property.Type },
                                query.Expression,
                                Expression.Quote(lambda));

                query = query.Provider.CreateQuery<Form>(orderBy);                
            }
            int filtered = query.Count();
            List<Form> items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            Pagination<Form> pagination = new Pagination<Form> {
                TotalItems = await context.Forms.CountAsync(),
                FilteredItems = filtered,
                Page = page,
                Items = items,
                TotalPages = (int)Math.Ceiling((double)filtered / limit)
            };
            pagination.BuildNavigation(request);

            return pagination;
        }
    }
}
