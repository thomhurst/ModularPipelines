namespace ModularPipelines.Models;

public class ModuleResult<T> : ModuleResult
{
    public ModuleResult(T? value)
    {
        _value = value;
        ModuleResultType = ModuleResultType.SuccessfulResult;
    }

    public ModuleResult(Exception exception)
    {
        Exception = exception;
        ModuleResultType = ModuleResultType.Failure;
    }

    private readonly T? _value;
    
    internal string? ModuleName { get; set; }

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
        return ModuleName ?? "This module";
    }

    public Exception? Exception { get; }

    public static implicit operator ModuleResult<T>(T t) => From(t);
    public static implicit operator ModuleResult<T>(Exception exception) => FromException<T>(exception);
}

public class ModuleResult
{
    public static ModuleResult<T> Empty<T>() => new(default(T));
    public static ModuleResult<T> From<T>(T t)
    {
        return t == null ? Empty<T>() : new ModuleResult<T>(t);
    }
    
    public static ModuleResult<T> FromException<T>(Exception exception) => new(exception);
    
    public ModuleResultType ModuleResultType { get; private protected set; }
}