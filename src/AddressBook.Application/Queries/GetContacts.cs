using AddressBook.Domain.Entities;
using AddressBook.Domain.Repositories;
using AutoMapper;
using MediatR;
using NBB.Core.Abstractions.Paging;

namespace AddressBook.Application.Queries;

public class GetContacts
{
    public record Query : IRequest<PagedResult<Model>>
    {
        protected internal const int DefaultPageSize = 100;
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = DefaultPageSize;
    }

    public record Model
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }


    public class QueryHandler : IRequestHandler<Query, PagedResult<Model>>
    {
        private readonly IContactsRepository _contactsRepository;
        private readonly IMapper _mapper;

        public QueryHandler(IContactsRepository contactsRepository, IMapper mapper)
        {
            _contactsRepository = contactsRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<Model>> Handle(Query request, CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest(request.Page <= 0 ? 1 : request.Page, request.PageSize <= 0 ? Query.DefaultPageSize : request.PageSize);
            var paged = await _contactsRepository.GetAllPagedAsync(pageRequest, cancellationToken);
            var result = paged.Map(x => _mapper.Map<Model>(x));
            return result;
        }
    }
}