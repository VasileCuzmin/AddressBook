namespace AddressBook.Api.Swagger;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "AddressBook Api",
                    Version = "v1"
                }
            );

            c.CustomSchemaIds(type => type.ToString());
        });

        return services;
    }
}