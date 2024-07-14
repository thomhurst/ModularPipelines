using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Serialization;

namespace ModularPipelines.Models;

public class ModuleResult<T> : ModuleResult
{
    private T? _value;

    internal ModuleResult(Exception exception, ModuleBase module) : base(exception, module)
    {
    }

    internal ModuleResult(T? value, ModuleBase module) : base(module)
    {
        _value = value;
    }

    [ExcludeFromCodeCoverage]
    [JsonConstructor]
    private ModuleResult()
    {
    }

    /// <summary>
    /// Gets the value held in the result.
    /// </summary>
    /// <exception cref="ModuleFailedException">Thrown if the module failed.</exception>
    /// <exception cref="ModuleSkippedException">Thrown if the module was skipped.</exception>
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
                throw new ModuleSkippedException(ModuleName);
            }

            return _value;
        }

        private set
        {
            _value = value;
        }
    }

    /// <summary>
    /// Gets a value indicating whether gets whether the result holds a value.
    /// </summary>
    public bool HasValue => !EqualityComparer<T?>.Default.Equals(_value, default);
}

[JsonConverter(typeof(TypeDiscriminatorConverter<ModuleResult>))]
public class ModuleResult : IModuleResult, ITypeDiscriminator
{
    /// <inheritdoc />
    [JsonInclude]
    public string ModuleName { get; private set; }

    /// <inheritdoc />
    [JsonInclude]
    public TimeSpan ModuleDuration { get; private set; }

    /// <inheritdoc />
    [JsonInclude]
    public DateTimeOffset ModuleStart { get; private set; }

    /// <inheritdoc />
    [JsonInclude]
    public DateTimeOffset ModuleEnd { get; private set; }

    internal ModuleResult(Exception exception, ModuleBase module) : this(module)
    {
        Exception = exception;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="ModuleResult"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
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

    /// <summary>
    /// Initialises a new instance of the <see cref="ModuleResult"/> class.
    /// </summary>
    /// <param name="module">The module from which the result was produced.</param>
    protected ModuleResult(ModuleBase module)
    {
        Module = module;
        ModuleName = module.GetType().Name;
        ModuleStart = module.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : module.StartTime;
        ModuleEnd = module.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : module.EndTime;
        ModuleDuration = ModuleEnd - ModuleStart;
        SkipDecision = module.SkipResult;
        TypeDiscriminator = GetType().FullName!;
        ModuleStatus = module.Status;
    }

    /// <inheritdoc />
    [JsonInclude]
    public Exception? Exception { get; private set; }

    /// <inheritdoc />
    [JsonInclude]
    public SkipDecision SkipDecision { get; private protected set; }

    /// <inheritdoc />
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

    /// <inheritdoc/>
    public Status ModuleStatus { get; private set; }

    /// <summary>
    /// Gets the type information used to aid in serialization.
    /// </summary>
    [JsonInclude]
    public string TypeDiscriminator { get; private set; }

    [JsonIgnore]
    internal ModuleBase? Module { get; set; }
}