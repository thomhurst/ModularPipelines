﻿using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.DotNet.Extensions;

public static class DotNetExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterDotNetContext()
    {
        ModularPipelinesContextRegistry.RegisterContext(collection => RegisterDotNetContext(collection));
    }

    public static IServiceCollection RegisterDotNetContext(this IServiceCollection services)
    {
        services.TryAddTransient<IDotNet, DotNet>();
        services.TryAddTransient<ITrxParser, TrxParser>();
        return services;
    }

    public static IDotNet DotNet(this IModuleContext context) => context.ServiceProvider.GetRequiredService<IDotNet>();
}
