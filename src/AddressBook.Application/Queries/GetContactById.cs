using AddressBook.Domain.Entities;
using AddressBook.Domain.Repositories;
using MediatR;

namespace AddressBook.Application.Queries;

public class GetContactById
{
    public record Query : IRequest<Contact>
    {
        public Guid Id { get; init; }
    }

    public class QueryHandler : IRequestHandler<Query, Contact>
    {
        private readonly IContactsRepository _repository;

        public QueryHandler(IContactsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Contact> Handle(Query request, CancellationToken cancellationToken)
        {
            var application = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return application;
        }
    }
}