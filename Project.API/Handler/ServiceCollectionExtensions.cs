using MediatR;

namespace UMS.API.Handler
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHandlersWithTransientLifetime(this IServiceCollection services, params Type[] handlerTypes)
        {
            foreach (Type handlerType in handlerTypes)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                Type requestType = handlerType.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .Select(i => i.GetGenericArguments()[0])
                    .FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (requestType != null)
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    Type responseType = handlerType.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                        .Select(i => i.GetGenericArguments()[1])
                        .FirstOrDefault();

                    Type requestHandlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

                    services.AddTransient(requestHandlerType, handlerType);
                }
            }
        }
    }
}
