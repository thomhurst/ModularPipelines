using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Helpers;

internal class ModuleLoggerProvider : IModuleLoggerProvider, IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    private ILogger? _logger;
    private readonly IServiceScope _serviceScope;

    public ModuleLoggerProvider(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScope = serviceScopeFactory.CreateScope();
        _serviceProvider = _serviceScope.ServiceProvider;
    }

    public ILogger GetLogger(Type type) => _logger ??= MakeLogger(type);

    public ILogger GetLogger()
    {
        if (_logger != null)
        {
            return _logger;
        }

        var stackFrames = new StackTrace().GetFrames().ToList();
        var module = stackFrames.Select(x => x.GetMethod()?.ReflectedType?.ReflectedType).FirstOrDefault(IsModule);

        if (module == null)
        {
            var getLoggerFrame = stackFrames.FirstOrDefault(sf => sf.GetMethod()?.Name == "get_Logger");

            if (getLoggerFrame == null)
            {
                return _serviceProvider.GetRequiredService<ModuleLogger<ModuleBase>>();
            }

            var getLoggerFrameIndex = stackFrames.IndexOf(getLoggerFrame);
            var nextFrame = stackFrames[getLoggerFrameIndex + 1];
            var type = nextFrame.GetMethod()?.ReflectedType;

            if (type != null)
            {
                return MakeLogger(type);
            }

            return _serviceProvider.GetRequiredService<ModuleLogger<ModuleBase>>();
        }

        return MakeLogger(module);
    }

    private ILogger MakeLogger(Type module)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(module);

        var logger = (ILogger) _serviceProvider.GetRequiredService(loggerType);

        _logger = logger;

        return logger;
    }

    private bool IsModule(Type? type)
    {
        if (type is null)
        {
            return false;
        }

        return !type.IsAbstract && type.IsAssignableTo(typeof(ModuleBase));
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
