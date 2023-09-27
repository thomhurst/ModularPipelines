using System.Text.Json.Serialization;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Serialization;

namespace ModularPipelines.Models;

public class ModuleResult<T> : ModuleResult
{
    private T? _value;

    [JsonConstructor]
    private ModuleResult() : base()
    {
    }
    
    internal ModuleResult(Exception exception, ModuleBase module) : base(exception, module)
    {
    }
    
    internal ModuleResult(T? value, ModuleBase module) : base(module)
    {
        _value = value;
    }
    
    [JsonInclude]
    public T? Value
    {
        get
        {
            if (ModuleResultType == ModuleResultType.Failure)
            {
                throw new ModuleFailedException(Module!, Exception!);
            }

            if (ModuleResultType == ModuleResultType.Skipped)
            {
                throw new ModuleSkippedException(GetModuleName());
            }

            return _value;
        }

        private set
        {
            _value = value;
        }
    }
    
    public bool HasValue => !EqualityComparer<T?>.Default.Equals(_value, default);
}

[JsonConverter(typeof(TypeDiscriminatorConverter<ModuleResult>))]
public class ModuleResult : IJsonTypeDiscriminator
{
    [JsonIgnore]
    internal ModuleBase? Module { get; set; }

    [JsonInclude]
    public string ModuleName { get; private set; }

    [JsonInclude]
    public TimeSpan ModuleDuration { get; private set; }

    [JsonInclude]
    public DateTimeOffset ModuleStart { get; private set; }

    [JsonInclude]
    public DateTimeOffset ModuleEnd { get; private set; }

    internal ModuleResult(Exception exception, ModuleBase module) : this(module)
    {
        Exception = exception;
    }
    
    [JsonConstructor]
    protected ModuleResult()
    {
        // Late initialisation via deserialisation
        ModuleName = null!;
        ModuleDuration = TimeSpan.Zero!;
        ModuleStart = DateTimeOffset.MinValue!;
        ModuleEnd = DateTimeOffset.MinValue;
        SkipDecision = null!;
        TypeDiscriminator = GetType().FullName!;
    }
    
    protected ModuleResult(ModuleBase module)
    {
        Module = module;
        ModuleName = module.GetType().Name;
        ModuleDuration = module.Duration;
        ModuleStart = module.StartTime;
        ModuleEnd = module.EndTime;
        SkipDecision = module.SkipResult;
        TypeDiscriminator = GetType().FullName!;
    }

    protected string GetModuleName()
    {
        return ModuleName;
    }
    
    [JsonInclude]
    public Exception? Exception { get; private set; }
    
    [JsonInclude]
    public SkipDecision SkipDecision { get; private protected set; }

    public ModuleResultType ModuleResultType
    {
        get
        {
            if (Exception != null)
            {
                return ModuleResultType.Failure;
            }

            if (SkipDecision?.ShouldSkip == true)
            {
                return ModuleResultType.Skipped;
            }

            return ModuleResultType.Success;
        }
    }

    [JsonInclude]
    public string TypeDiscriminator { get; private set; }
}
