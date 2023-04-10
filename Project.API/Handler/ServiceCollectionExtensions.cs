using MediatR;
using Scrutor;
using System.Reflection;

namespace UMS.API.Handler
{
    public static class ServiceCollectionExtensions
    { //This extension method takes a list of types that implement the IRequestHandler
      //interface and registers them with transient lifetime using the AddTransient method
        /* public static void AddHandlersWithTransientLifetime(this IServiceCollection services, params Type[] handlerTypes)
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
             }*/
        public static void AddMyServices(this IServiceCollection services, Type[] handlerTypes)
        {
            var handlerTypeList = handlerTypes.ToList();

            foreach (var handlerType in handlerTypeList)
            {
                var interfaceType = handlerType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

                if (interfaceType != null)
                {
                    services.AddTransient(interfaceType, handlerType);
                }
            }
        }
    }
}
