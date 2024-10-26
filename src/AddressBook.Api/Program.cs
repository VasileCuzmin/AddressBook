using AddressBook.Api.Services;
using AddressBook.Application;
using AddressBook.Application.Commands;
using AddressBook.Data;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDataAccess();
builder.Services.AddSingleton<IContactService, ContactService>();
builder.Services.AddMediatR(typeof(CreateContact).Assembly);

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
builder.Services.Scan(scan => scan.FromAssemblyOf<CreateContact>()
    .AddClasses(classes => classes.AssignableTo<IValidator>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(cors =>
{
    cors
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddressBook Api");
});

app.MapControllers();

app.Run();
