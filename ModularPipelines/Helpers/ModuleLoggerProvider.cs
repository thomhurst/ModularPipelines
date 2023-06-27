using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Helpers;

internal class ModuleLoggerProvider : IModuleLoggerProvider
{
    private readonly IServiceProvider _serviceProvider;
    
    private ILogger? _logger;

    public ModuleLoggerProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public ILogger Logger => _logger ??= GetLogger();
    
    private bool IsModule(Type? type)
    {
        if (type is null)
        {
            return false;
        }
        
        return !type.IsAbstract && type.IsAssignableTo(typeof(ModuleBase));
    }

    private ILogger GetLogger()
    {
        var module = new StackTrace().GetFrames().Select(x => x.GetMethod()?.ReflectedType?.ReflectedType).FirstOrDefault(IsModule);

        if (module == null)
        {
            return _serviceProvider.GetRequiredService<ModuleLogger<ModuleBase>>();
        }

        var loggerType = typeof(ModuleLogger<>).MakeGenericType(module);

        return (ILogger) _serviceProvider.GetRequiredService(loggerType);
    }
}