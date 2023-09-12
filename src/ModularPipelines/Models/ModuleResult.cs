using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.Models;

public class ModuleResult<T>
{
    private readonly ModuleBase _module;
    private readonly T? _value;

    public string ModuleName { get; private set; }
    
    public TimeSpan ModuleDuration { get; private set; }
    public DateTimeOffset ModuleStart { get; private set; }
    public DateTimeOffset ModuleEnd { get; private set; }
    
    private ModuleResult(ModuleBase module)
    {
        _module = module;
        ModuleName = module.GetType().Name;
        ModuleDuration = module.Duration;
        ModuleStart = module.StartTime;
        ModuleEnd = module.EndTime;
    }
    
    internal ModuleResult(T? value, ModuleBase module) : this(module)
    {
        _value = value;
        ModuleResultType = ModuleResultType.Success;
    }

    internal ModuleResult(Exception exception, ModuleBase module) : this(module)
    {
        Exception = exception;
        ModuleResultType = ModuleResultType.Failure;
    }

    public T? Value
    {
        get
        {
            if (ModuleResultType == ModuleResultType.Failure)
            {
                throw new ModuleFailedException(_module, Exception!);
            }

            if (ModuleResultType == ModuleResultType.Skipped)
            {
                throw new ModuleSkippedException(GetModuleName());
            }

            return _value;
        }
    }

    private string GetModuleName()
    {
        return ModuleName;
    }

    public bool HasValue => !EqualityComparer<T?>.Default.Equals(_value, default);

    public Exception? Exception { get; private set; }
    
    public ModuleResultType ModuleResultType { get; private protected init; }
}
