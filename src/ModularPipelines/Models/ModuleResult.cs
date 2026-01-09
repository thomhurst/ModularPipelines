using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Represents the result of a module execution as a discriminated union.
/// Use pattern matching to handle Success, Failure, and Skipped cases.
/// </summary>
/// <typeparam name="T">The type of value returned on success.</typeparam>
/// <example>
/// <code>
/// var result = await myModule;
/// switch (result)
/// {
///     case ModuleResult&lt;string&gt;.Success { Value: var value }:
///         Console.WriteLine($"Got: {value}");
///         break;
///     case ModuleResult&lt;string&gt;.Failure { Exception: var ex }:
///         Console.WriteLine($"Failed: {ex.Message}");
///         break;
///     case ModuleResult&lt;string&gt;.Skipped { Decision: var skip }:
///         Console.WriteLine($"Skipped: {skip.Reason}");
///         break;
/// }
/// </code>
/// </example>
[JsonConverter(typeof(ModuleResultJsonConverterFactory))]
public abstract record ModuleResult<T> : IModuleResult
{
    // === Metadata (available on all outcomes) ===

    /// <inheritdoc />
    [JsonInclude]
    public required string ModuleName { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required TimeSpan ModuleDuration { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required DateTimeOffset ModuleStart { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required DateTimeOffset ModuleEnd { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required Status ModuleStatus { get; init; }

    // === Quick checks ===

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsSuccess => this is Success;

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsFailure => this is Failure;

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsSkipped => this is Skipped;

    // === Safe accessors (no exceptions) ===

    /// <summary>
    /// Gets the value if successful, or default(T) otherwise. Does not throw.
    /// </summary>
    [JsonIgnore]
    public T? ValueOrDefault => this is Success s ? s.Value : default;

    /// <inheritdoc />
    [JsonIgnore]
    object? IModuleResult.ValueOrDefault => ValueOrDefault;

    /// <inheritdoc />
    [JsonIgnore]
    public Exception? ExceptionOrDefault => this is Failure f ? f.Exception : null;

    /// <inheritdoc />
    [JsonIgnore]
    public SkipDecision? SkipDecisionOrDefault => this is Skipped sk ? sk.Decision : null;

    // === Computed for compatibility ===

    /// <inheritdoc />
    [JsonIgnore]
    public ModuleResultType ModuleResultType => this switch
    {
        Success => ModuleResultType.Success,
        Failure => ModuleResultType.Failure,
        Skipped => ModuleResultType.Skipped,
        _ => throw new InvalidOperationException("Unknown result type")
    };

    // === Internal: Module type tracking ===

    /// <summary>
    /// Gets or sets the type of the module that produced this result.
    /// </summary>
    [JsonIgnore]
    internal Type? ModuleType { get; init; }

    // === Pattern matching helpers ===

    /// <summary>
    /// Matches the result to one of three functions based on the outcome.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match functions.</typeparam>
    /// <param name="onSuccess">Function to call if successful.</param>
    /// <param name="onFailure">Function to call if failed.</param>
    /// <param name="onSkipped">Function to call if skipped.</param>
    /// <returns>The result of the matched function.</returns>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure,
        Func<SkipDecision, TResult> onSkipped) => this switch
    {
        Success s => onSuccess(s.Value),
        Failure f => onFailure(f.Exception),
        Skipped sk => onSkipped(sk.Decision),
        _ => throw new InvalidOperationException("Unknown result type")
    };

    /// <summary>
    /// Executes one of three actions based on the outcome.
    /// </summary>
    /// <param name="onSuccess">Action to call if successful.</param>
    /// <param name="onFailure">Action to call if failed.</param>
    /// <param name="onSkipped">Action to call if skipped.</param>
    public void Switch(
        Action<T> onSuccess,
        Action<Exception> onFailure,
        Action<SkipDecision> onSkipped)
    {
        switch (this)
        {
            case Success s:
                onSuccess(s.Value);
                break;
            case Failure f:
                onFailure(f.Exception);
                break;
            case Skipped sk:
                onSkipped(sk.Decision);
                break;
        }
    }

    // === Discriminated variants ===

    /// <summary>
    /// Represents a successful module execution with a value.
    /// </summary>
    /// <param name="Value">The value produced by the module.</param>
    public sealed record Success(T Value) : ModuleResult<T>;

    /// <summary>
    /// Represents a failed module execution with an exception.
    /// </summary>
    /// <param name="Exception">The exception that caused the failure.</param>
    public sealed record Failure(Exception Exception) : ModuleResult<T>;

    /// <summary>
    /// Represents a skipped module execution.
    /// </summary>
    /// <param name="Decision">The skip decision containing the reason.</param>
    public sealed record Skipped(SkipDecision Decision) : ModuleResult<T>;

    // === Internal factory methods ===

    internal static Success CreateSuccess(T value, ModuleExecutionContext ctx)
    {
        var (start, end, duration) = GetTimingInfo(ctx);
        return new(value)
        {
            ModuleName = ctx.ModuleType.Name,
            ModuleDuration = duration,
            ModuleStart = start,
            ModuleEnd = end,
            ModuleStatus = ctx.Status,
            ModuleType = ctx.ModuleType
        };
    }

    internal static Failure CreateFailure(Exception exception, ModuleExecutionContext ctx)
    {
        var (start, end, duration) = GetTimingInfo(ctx);
        return new(exception)
        {
            ModuleName = ctx.ModuleType.Name,
            ModuleDuration = duration,
            ModuleStart = start,
            ModuleEnd = end,
            ModuleStatus = ctx.Status,
            ModuleType = ctx.ModuleType
        };
    }

    internal static Skipped CreateSkipped(SkipDecision decision, ModuleExecutionContext ctx)
    {
        var (start, end, duration) = GetTimingInfo(ctx);
        return new(decision)
        {
            ModuleName = ctx.ModuleType.Name,
            ModuleDuration = duration,
            ModuleStart = start,
            ModuleEnd = end,
            ModuleStatus = ctx.Status,
            ModuleType = ctx.ModuleType
        };
    }

    /// <summary>
    /// Gets consistent timing information from the execution context.
    /// If either start or end time is MinValue, returns TimeSpan.Zero for duration
    /// to avoid inconsistent results from calling DateTimeOffset.Now multiple times.
    /// </summary>
    private static (DateTimeOffset Start, DateTimeOffset End, TimeSpan Duration) GetTimingInfo(ModuleExecutionContext ctx)
    {
        var now = DateTimeOffset.Now;
        var start = ctx.StartTime == DateTimeOffset.MinValue ? now : ctx.StartTime;
        var end = ctx.EndTime == DateTimeOffset.MinValue ? now : ctx.EndTime;

        // If either time was originally MinValue, duration is unreliable - use Zero
        var duration = (ctx.StartTime == DateTimeOffset.MinValue || ctx.EndTime == DateTimeOffset.MinValue)
            ? TimeSpan.Zero
            : end - start;

        return (start, end, duration);
    }

    // Prevent external inheritance - only Success, Failure, Skipped are valid
    private protected ModuleResult()
    {
    }
}

/// <summary>
/// JSON converter for Exception objects. Serializes essential exception data
/// and deserializes to a wrapper exception preserving the message.
/// </summary>
/// <remarks>
/// <para><strong>Security Considerations:</strong></para>
/// <para>
/// This converter intentionally serializes limited exception information:
/// </para>
/// <list type="bullet">
/// <item><description>
/// <strong>Type:</strong> Only the full type name (not AssemblyQualifiedName) is serialized
/// to avoid leaking internal assembly version and culture information.
/// </description></item>
/// <item><description>
/// <strong>Message:</strong> Exception messages may contain sensitive data (file paths,
/// user input, etc.). Consumers should sanitize exception messages before serialization
/// if they may contain sensitive information.
/// </description></item>
/// <item><description>
/// <strong>StackTrace:</strong> Stack traces may reveal internal file paths and code structure.
/// Consider whether this is acceptable for your use case. For production logging to external
/// systems, you may want to omit or truncate stack traces.
/// </description></item>
/// </list>
/// <para>
/// On deserialization, only well-known exception types from the System namespace are
/// reconstructed. Unknown types fall back to a generic Exception to prevent type injection.
/// </para>
/// </remarks>
internal sealed class ExceptionJsonConverter : JsonConverter<Exception>
{
    public override Exception? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        string? typeName = null;
        string? message = null;
        string? stackTrace = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "Type":
                        typeName = reader.GetString();
                        break;
                    case "Message":
                        message = reader.GetString();
                        break;
                    case "StackTrace":
                        stackTrace = reader.GetString();
                        break;
                }
            }
        }

        // Try to reconstruct the original exception type if possible
        // Security: Only allow well-known exception types from System namespace
        if (typeName != null)
        {
            var exceptionType = Type.GetType(typeName);
            if (exceptionType != null &&
                typeof(Exception).IsAssignableFrom(exceptionType) &&
                (exceptionType.Namespace?.StartsWith("System", StringComparison.Ordinal) == true))
            {
                try
                {
                    var ex = Activator.CreateInstance(exceptionType, message) as Exception;
                    if (ex != null)
                    {
                        return ex;
                    }
                }
                catch
                {
                    // Fall through to default
                }
            }
        }

        // NOTE: StackTrace is intentionally not restored during deserialization.
        // Setting the StackTrace property via reflection is fragile and can cause issues.
        // The original stack trace is preserved in the JSON for diagnostic purposes,
        // but deserialized exceptions will have a new stack trace from deserialization.
        return new Exception(message ?? "Deserialized exception");
    }

    public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        // Security: Use FullName instead of AssemblyQualifiedName to avoid leaking
        // assembly version, culture, and public key token information
        writer.WriteString("Type", value.GetType().FullName);
        writer.WriteString("Message", value.Message);
        writer.WriteString("StackTrace", value.StackTrace);
        writer.WriteEndObject();
    }
}

/// <summary>
/// JSON converter factory that creates typed converters for ModuleResult&lt;T&gt;.
/// </summary>
internal sealed class ModuleResultJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(ModuleResult<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var valueType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(ModuleResultJsonConverter<>).MakeGenericType(valueType);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
}

/// <summary>
/// JSON converter for ModuleResult&lt;T&gt; that handles polymorphic serialization/deserialization.
/// </summary>
internal sealed class ModuleResultJsonConverter<T> : JsonConverter<ModuleResult<T>>
{
    private static readonly ExceptionJsonConverter ExceptionConverter = new();

    public override ModuleResult<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        string? discriminator = null;
        string? moduleName = null;
        TimeSpan moduleDuration = TimeSpan.Zero;
        DateTimeOffset moduleStart = DateTimeOffset.MinValue;
        DateTimeOffset moduleEnd = DateTimeOffset.MinValue;
        Status moduleStatus = Status.NotYetStarted;
        T? value = default;
        Exception? exception = null;
        SkipDecision? skipDecision = null;

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token");
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "$type":
                        discriminator = reader.GetString();
                        break;
                    case "ModuleName":
                        moduleName = reader.GetString();
                        break;
                    case "ModuleDuration":
                        moduleDuration = JsonSerializer.Deserialize<TimeSpan>(ref reader, options);
                        break;
                    case "ModuleStart":
                        moduleStart = reader.GetDateTimeOffset();
                        break;
                    case "ModuleEnd":
                        moduleEnd = reader.GetDateTimeOffset();
                        break;
                    case "ModuleStatus":
                        moduleStatus = JsonSerializer.Deserialize<Status>(ref reader, options);
                        break;
                    case "Value":
                        value = JsonSerializer.Deserialize<T>(ref reader, options);
                        break;
                    case "Exception":
                        exception = ExceptionConverter.Read(ref reader, typeof(Exception), options);
                        break;
                    case "Decision":
                        skipDecision = JsonSerializer.Deserialize<SkipDecision>(ref reader, options);
                        break;
                }
            }
        }

        if (moduleName is null)
        {
            throw new JsonException("ModuleName is required but was not found in the JSON.");
        }

        return discriminator switch
        {
            "Success" => new ModuleResult<T>.Success(value!)
            {
                ModuleName = moduleName,
                ModuleDuration = moduleDuration,
                ModuleStart = moduleStart,
                ModuleEnd = moduleEnd,
                ModuleStatus = moduleStatus
            },
            "Failure" => exception is not null
                ? new ModuleResult<T>.Failure(exception)
                {
                    ModuleName = moduleName,
                    ModuleDuration = moduleDuration,
                    ModuleStart = moduleStart,
                    ModuleEnd = moduleEnd,
                    ModuleStatus = moduleStatus
                }
                : throw new JsonException("Failure result requires an Exception property in the JSON."),
            "Skipped" => skipDecision is not null
                ? new ModuleResult<T>.Skipped(skipDecision)
                {
                    ModuleName = moduleName,
                    ModuleDuration = moduleDuration,
                    ModuleStart = moduleStart,
                    ModuleEnd = moduleEnd,
                    ModuleStatus = moduleStatus
                }
                : throw new JsonException("Skipped result requires a Decision property in the JSON."),
            _ => throw new JsonException($"Unknown discriminator: {discriminator}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ModuleResult<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Write discriminator
        var discriminator = value switch
        {
            ModuleResult<T>.Success => "Success",
            ModuleResult<T>.Failure => "Failure",
            ModuleResult<T>.Skipped => "Skipped",
            _ => throw new JsonException("Unknown ModuleResult type")
        };
        writer.WriteString("$type", discriminator);

        // Write common properties
        writer.WriteString("ModuleName", value.ModuleName);
        writer.WritePropertyName("ModuleDuration");
        JsonSerializer.Serialize(writer, value.ModuleDuration, options);
        writer.WriteString("ModuleStart", value.ModuleStart);
        writer.WriteString("ModuleEnd", value.ModuleEnd);
        writer.WritePropertyName("ModuleStatus");
        JsonSerializer.Serialize(writer, value.ModuleStatus, options);

        // Write variant-specific properties
        switch (value)
        {
            case ModuleResult<T>.Success success:
                writer.WritePropertyName("Value");
                JsonSerializer.Serialize(writer, success.Value, options);
                break;
            case ModuleResult<T>.Failure failure:
                writer.WritePropertyName("Exception");
                ExceptionConverter.Write(writer, failure.Exception, options);
                break;
            case ModuleResult<T>.Skipped skipped:
                writer.WritePropertyName("Decision");
                JsonSerializer.Serialize(writer, skipped.Decision, options);
                break;
        }

        writer.WriteEndObject();
    }
}
