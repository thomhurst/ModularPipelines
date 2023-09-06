using ModularPipelines.Modules;

namespace ModularPipelines.Models;

public class ModuleResult<T>
{
    private readonly T? _value;

    public string ModuleName { get; private set; }
    
    public TimeSpan ModuleDuration { get; private set; }
    public DateTimeOffset ModuleStart { get; private set; }
    public DateTimeOffset ModuleEnd { get; private set; }
    
    private ModuleResult(ModuleBase module)
    {
        ModuleName = module.GetType().Name;
        ModuleDuration = module.Duration;
        ModuleStart = module.StartTime;
        ModuleEnd = module.EndTime;
    }
    
    internal ModuleResult(T? value, ModuleBase module) : this(module)
    {
        _value = value;
        ModuleResultType = ModuleResultType.SuccessfulResult;
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
                throw new Exception($"{GetModuleName()} has errored. No Value available");
            }

            if (ModuleResultType == ModuleResultType.Skipped)
            {
                throw new Exception($"{GetModuleName()} was skipped. No Value available");
            }

            return _value;
        }
    }

    private string GetModuleName()
    {
        return ModuleName;
    }

    public bool HasValue => _value != null;

    public Exception? Exception { get; private set; }
    
    public ModuleResultType ModuleResultType { get; private protected set; }
}
