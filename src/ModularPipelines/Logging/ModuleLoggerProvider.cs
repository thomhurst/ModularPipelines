using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Logging;

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
        var module = stackFrames.Select(GetNonCompilerGeneratedType).FirstOrDefault(IsModule);

        if (module == null)
        {
            var getLoggerFrame = stackFrames.FirstOrDefault(sf => sf.GetMethod()?.Name == "get_Logger");

            if (getLoggerFrame == null)
            {
                return MakeLogger(GetCallingClassType(stackFrames));
            }

            var getLoggerFrameIndex = stackFrames.IndexOf(getLoggerFrame);
            var nextFrame = stackFrames[getLoggerFrameIndex + 1];
            var type = nextFrame.GetMethod()?.ReflectedType;

            if (type != null)
            {
                return MakeLogger(type);
            }

            return MakeLogger(GetCallingClassType(stackFrames));
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
    
    private static Type GetCallingClassType(List<StackFrame> stackFrames)
    {
        var entryAssemblyFirstCallingClass = stackFrames
            .Select(GetNonCompilerGeneratedType)
            .OfType<Type>()
            .Where(t => t != typeof(ModuleLoggerProvider))
            .Where(x => x.Assembly == Assembly.GetEntryAssembly())
            .FirstOrDefault(x => x is { IsAbstract: false, IsGenericTypeDefinition: false });

        if (entryAssemblyFirstCallingClass != null)
        {
            return entryAssemblyFirstCallingClass;
        }

        return stackFrames
            .Select(GetNonCompilerGeneratedType)
            .OfType<Type>()
            .Where(t => t != typeof(ModuleLoggerProvider))
            .First(x => x is { IsAbstract: false, IsGenericTypeDefinition: false });;
    }

    private static Type? GetNonCompilerGeneratedType(StackFrame stackFrame)
    {
        var type = stackFrame.GetMethod()?.ReflectedType;

        while (type?.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
        {
            type = type.DeclaringType;
        }

        return type;
    }


    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}
