using System;
using Splat;

namespace Clappban.InjectionDependency;

public static class ReadonlyDependencyResolverExtension
{
    public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver resolver)
    {
        var service = resolver.GetService<TService>();
        if (service is null)
        {
            throw new InvalidOperationException($"Failed ot resolve object of type {typeof(TService)}");
        }

        return service;
    }

    public static object GetRequiredService(this IReadonlyDependencyResolver resolver, Type type)
    {
        var service = resolver.GetService(type);
        if (service is null)
        {
            throw new InvalidOperationException($"Failed ot resolve object of type {typeof(object)}");
        }

        return service;
    }
}