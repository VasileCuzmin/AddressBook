using AddressBook.Application.Events;
using AddressBook.Domain.Entities;
using AddressBook.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using NBB.Data.Abstractions;

namespace AddressBook.Application.Commands;

public class CreateContact
{
    public record Command(string? FirstName, string? LastName, string? EmailAddress, string? PhoneNumber) : IRequest;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(a => a.FirstName)
                .NotEmpty().WithMessage("FirstName mandatory");
            RuleFor(a => a.LastName)
                .NotEmpty().WithMessage("LastName mandatory");
            RuleFor(a => a.EmailAddress)
                .EmailAddress().WithMessage("Enter valid email");
        }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IContactsRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public Handler(IContactsRepository repository, IMapper mapper, IMediator mediator)
        {
            _repository = repository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var application = _mapper.Map<Contact>(request);
            await _repository.AddAsync(application, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken: cancellationToken);

            //notify other services that a contact has been created
            await _mediator.Publish(new ContactCreated(request.FirstName, request.LastName), cancellationToken);

            return Unit.Value;
        }
    }
}