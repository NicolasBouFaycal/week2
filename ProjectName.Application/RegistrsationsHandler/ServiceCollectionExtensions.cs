using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace UMS.Application.Handler
{
    public static class ServiceCollectionExtensions
    { 
        //searches through the interfaces implemented by handlerType using the GetInterfaces() method
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
