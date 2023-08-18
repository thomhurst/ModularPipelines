using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Helpers;

internal class ModuleLoggerProvider : IModuleLoggerProvider, IDisposable
{
    private readonly IModuleLoggerContainer _moduleLoggerContainer;
    private readonly IServiceProvider _serviceProvider;

    private ILogger? _logger;
    private readonly IServiceScope _serviceScope;

    public ModuleLoggerProvider(IServiceScopeFactory serviceScopeFactory, IModuleLoggerContainer moduleLoggerContainer)
    {
        _moduleLoggerContainer = moduleLoggerContainer;
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

            Type? nonAbstractType;
            
            if (getLoggerFrame == null)
            {
                nonAbstractType = stackFrames.Skip(2).Select(x => x.GetMethod()?.ReflectedType?.ReflectedType)
                    .OfType<Type>()
                    .First(x => x is { IsAbstract: false, IsGenericTypeDefinition: false });

                return MakeLogger(nonAbstractType);
            }

            var getLoggerFrameIndex = stackFrames.IndexOf(getLoggerFrame);
            var nextFrame = stackFrames[getLoggerFrameIndex + 1];
            var type = nextFrame.GetMethod()?.ReflectedType;

            if (type != null)
            {
                return MakeLogger(type);
            }

            nonAbstractType = stackFrames.Skip(2).Select(x => x.GetMethod()?.ReflectedType?.ReflectedType)
                .OfType<Type>()
                .First(x => x is { IsAbstract: false, IsGenericTypeDefinition: false });

            return MakeLogger(nonAbstractType);
        }

        return MakeLogger(module);
    }

    private ILogger MakeLogger(Type module)
    {
        var loggerType = typeof(ModuleLogger<>).MakeGenericType(module);

        if (_moduleLoggerContainer.TryGetModuleLogger(loggerType, out var moduleLogger))
        {
            return moduleLogger;
        }

        return _logger = (ILogger) _serviceProvider.GetRequiredService(loggerType);
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
