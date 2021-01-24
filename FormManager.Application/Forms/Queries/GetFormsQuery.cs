using AutoMapper;
using FormManager.Application.Common.Interfaces;
using FormManager.Application.Common.Models;
using FormManager.Application.Forms.Mappings.ViewModels;
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
    public class GetFormsQuery : RequestQuery<Pagination<FormVM>> {}

    public class GetFormsQueryHandler : IRequestHandler<GetFormsQuery, Pagination<FormVM>>
    {
        private readonly IAppDbContext context;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public GetFormsQueryHandler(IAppDbContext context, IMapper mapper, IUserRepository userRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }

        async Task<Pagination<FormVM>> IRequestHandler<GetFormsQuery, Pagination<FormVM>>.Handle(GetFormsQuery request, CancellationToken cancellationToken)
        {
            int page = request.Page ?? 1;
            int limit = request.Limit ?? 10;
            int total = await context.Forms.CountAsync(cancellationToken);
            
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
            List<Form> items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync(cancellationToken);
            List<FormVM> listsVM = mapper.Map<List<FormVM>>(items);
            foreach (var item in listsVM)
                item.Sender.Username = (await userRepository.GetUserById(item.Sender.Id))?.Username;
            
            Pagination<FormVM> pagination = new Pagination<FormVM> {
                TotalItems = total,
                FilteredItems = filtered,
                Page = page,
                Items = listsVM,
                TotalPages = (int)Math.Ceiling((double)filtered / limit)
            };
            pagination.BuildNavigation(request);

            return pagination;
        }
    }
}
