using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Command.Options;
using ModularPipelines.Context;
using ModularPipelines.Engine;

namespace ModularPipelines.Command.Extensions;

public static class CommandExtensions
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void RegisterCommandContext()
    {
        ServiceContextRegistry.RegisterContext(collection => RegisterCommandContext(collection));
    }
    
    public static IServiceCollection RegisterCommandContext(this IServiceCollection services)
    {
        services.TryAddSingleton(typeof(ICommand<>), typeof(Command<>));
        return services;
    }

    public static CommandLineToolOptions ToCommandLineToolOptions(this CommandEnvironmentOptions options, string tool, IEnumerable<string> arguments)
    {
        return new CommandLineToolOptions(tool)
        {
            Arguments = arguments,
            Credentials = options.Credentials,
            EnvironmentVariables = options.EnvironmentVariables,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput,
            WorkingDirectory = options.WorkingDirectory
        };
    }
    
    public static ICommand Command(this IModuleContext context) => (ICommand) context.ServiceProvider.GetRequiredService(typeof(ICommand<>).MakeGenericType(context.ModuleType));
}