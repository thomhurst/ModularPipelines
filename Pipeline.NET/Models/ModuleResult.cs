namespace Pipeline.NET.Models;

public class ModuleResult<T>
{
    public ModuleResult(T? value)
    {
        Value = value;
    }

    public ModuleResult(Exception exception)
    {
        Exception = exception;
    }


    public T? Value { get; set; }
    public Exception? Exception { get; }
    public bool IsErrored => Exception != null;

    public static implicit operator ModuleResult<T>(T t) => ModuleResult.From(t);
    public static implicit operator ModuleResult<T>(Exception exception) => ModuleResult.FromException<T>(exception);
}

public class ModuleResult
{
    public static ModuleResult<T> Empty<T>() => new(default(T));
    public static ModuleResult<T> From<T>(T t)
    {
        return t == null ? Empty<T>() : new ModuleResult<T>(t);
    }
    
    public static ModuleResult<T> FromException<T>(Exception exception) => new(exception);
}