using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines;

/// <summary>
/// Non-generic base class for module execution results.
/// Provides common metadata and result types that don't require a type parameter.
/// </summary>
/// <remarks>
/// <para>
/// This base class exists because <see cref="Failure"/> and <see cref="Skipped"/> results
/// don't carry a typed value - only <see cref="ModuleResult{T}.Success"/> needs the type parameter.
/// By separating the non-generic parts, we avoid creating unnecessary generic type instantiations
/// for failure and skipped cases.
/// </para>
/// <para>
/// Use <see cref="ModuleResult{T}"/> when you need type-safe access to success values.
/// Use <see cref="ModuleResult"/> when working with results generically (e.g., logging, reporting).
/// </para>
/// </remarks>
[JsonConverter(typeof(ModuleResultJsonConverterFactory))]
public abstract record ModuleResult : IModuleResult
{
    // === Metadata (available on all outcomes) ===

    /// <inheritdoc />
    [JsonInclude]
    public required string Name { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required TimeSpan Duration { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required DateTimeOffset StartTime { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required DateTimeOffset EndTime { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required ModuleStatus Status { get; init; }

    /// <summary>
    /// Gets the fully qualified type name of the module that produced this result.
    /// Used for cross-process module identification in distributed mode.
    /// </summary>
    [JsonInclude]
    public string? TypeName { get; init; }

    // === Safe accessors (no exceptions) ===

    /// <inheritdoc />
    [JsonIgnore]
    object? IModuleResult.ValueOrDefault => GetValueOrDefault();

    /// <summary>
    /// Gets the value if successful, or null otherwise. Override in derived classes.
    /// </summary>
    /// <returns>The module value, or <c>null</c>.</returns>
    protected abstract object? GetValueOrDefault();

    /// <inheritdoc />
    [JsonIgnore]
    public Exception? ExceptionOrDefault => GetExceptionCore();

    /// <inheritdoc />
    [JsonIgnore]
    public SkipDecision? SkipDecisionOrDefault => GetSkipDecisionCore();

    /// <summary>
    /// Gets the exception from a failure variant. Override in derived classes.
    /// </summary>
    /// <returns>The exception, or <c>null</c>.</returns>
    protected virtual Exception? GetExceptionCore() => null;

    /// <summary>
    /// Gets the skip decision from a skipped variant. Override in derived classes.
    /// </summary>
    /// <returns>The skip decision, or <c>null</c>.</returns>
    protected virtual SkipDecision? GetSkipDecisionCore() => null;

    // === Internal: Module type tracking ===

    /// <summary>
    /// Gets the type of the module that produced this result.
    /// </summary>
    [JsonIgnore]
    internal Type? ModuleType { get; init; }

    // === Non-generic discriminated variants ===

    /// <summary>
    /// Represents a failed module execution with an exception.
    /// </summary>
    /// <remarks>
    /// This type is non-generic because failure results don't carry a typed value.
    /// It can be implicitly converted to <see cref="ModuleResult{T}"/> for any T.
    /// </remarks>
    /// <param name="Exception">The exception that caused the failure.</param>
    public sealed record Failure(Exception Exception) : ModuleResult
    {
        /// <inheritdoc />
        protected override object? GetValueOrDefault() => null;

        /// <inheritdoc />
        protected override Exception? GetExceptionCore() => Exception;
    }

    /// <summary>
    /// Represents a skipped module execution.
    /// </summary>
    /// <remarks>
    /// This type is non-generic because skipped results don't carry a typed value.
    /// It can be implicitly converted to <see cref="ModuleResult{T}"/> for any T.
    /// </remarks>
    /// <param name="Decision">The skip decision containing the reason.</param>
    public sealed record Skipped(SkipDecision Decision) : ModuleResult
    {
        /// <inheritdoc />
        protected override object? GetValueOrDefault() => null;

        /// <inheritdoc />
        protected override SkipDecision? GetSkipDecisionCore() => Decision;
    }

    // === Internal factory methods for non-generic results ===
    internal static Failure CreateFailure(Exception exception, ModuleExecutionContext ctx)
    {
        var (start, end, duration) = GetTimingInfo(ctx);
        return new(exception)
        {
            Name = ctx.ModuleType.Name,
            TypeName = ctx.ModuleType.FullName,
            Duration = duration,
            StartTime = start,
            EndTime = end,
            Status = ctx.Status,
            ModuleType = ctx.ModuleType,
        };
    }

    internal static Skipped CreateSkipped(SkipDecision decision, ModuleExecutionContext ctx)
    {
        var (start, end, duration) = GetTimingInfo(ctx);
        return new(decision)
        {
            Name = ctx.ModuleType.Name,
            TypeName = ctx.ModuleType.FullName,
            Duration = duration,
            StartTime = start,
            EndTime = end,
            Status = ctx.Status,
            ModuleType = ctx.ModuleType,
        };
    }

    /// <summary>
    /// Gets consistent timing information from the execution context.
    /// If either start or end time is MinValue, returns TimeSpan.Zero for duration
    /// to avoid inconsistent results from calling DateTimeOffset.Now multiple times.
    /// </summary>
    internal static (DateTimeOffset Start, DateTimeOffset End, TimeSpan Duration) GetTimingInfo(ModuleExecutionContext ctx)
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
/// Represents the result of a module execution as a discriminated union.
/// Use pattern matching to handle Success, Failure, and Skipped cases.
/// </summary>
/// <typeparam name="T">The type of value returned on success.</typeparam>
/// <remarks>
/// <para>
/// The nested <see cref="Success"/>, <see cref="Failure"/>, and <see cref="Skipped"/>
/// variants provide the canonical pattern-matching surface for a typed result.
/// Non-generic failure and skipped results are converted to these variants at typed boundaries.
/// </para>
/// </remarks>
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
public abstract record ModuleResult<T> : ModuleResult
{
    /// <summary>
    /// Gets the successful value.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The module failed or was skipped.
    /// </exception>
    [JsonIgnore]
    public T Value => this switch
    {
        Success success => success.Value,
        Failure failure => throw new InvalidOperationException(
            $"{Name} failed: {failure.Exception.Message}",
            failure.Exception),
        Skipped skipped => throw new InvalidOperationException(
            $"{Name} was skipped: {skipped.Decision.Reason ?? "No reason was provided"}"),
        _ => throw new InvalidOperationException($"{Name} has an unknown result type"),
    };

    // === Safe accessors (no exceptions) ===

    /// <summary>
    /// Gets the value if successful, or default(T) otherwise. Does not throw.
    /// </summary>
    [JsonIgnore]
    public T? ValueOrDefault => this is Success s ? s.Value : default;

    /// <summary>
    /// Attempts to get the successful value.
    /// </summary>
    /// <param name="value">
    /// When this method returns <c>true</c>, contains the successful value;
    /// otherwise, contains the default value for <typeparamref name="T"/>.
    /// </param>
    /// <returns><c>true</c> for a successful result; otherwise, <c>false</c>.</returns>
    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        if (this is Success success)
        {
            value = success.Value;
            return true;
        }

        value = default!;
        return false;
    }

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
            _ => throw new InvalidOperationException("Unknown result type"),
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

    // === Generic discriminated variants ===

    /// <summary>
    /// Represents a successful module execution with a value.
    /// </summary>
    [JsonConverter(typeof(ModuleResultJsonConverterFactory))]
    public sealed record Success : ModuleResult<T>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Success"/> class.
        /// </summary>
        /// <param name="value">The value produced by the module.</param>
        public Success(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value produced by the module.
        /// </summary>
        public new T Value { get; init; }

        /// <summary>
        /// Deconstructs the result into its successful value.
        /// </summary>
        /// <param name="value">The value produced by the module.</param>
        public void Deconstruct(out T value)
        {
            value = Value;
        }
    }

    /// <summary>
    /// Represents a failed module execution with an exception.
    /// </summary>
    /// <param name="Exception">The exception that caused the failure.</param>
    [JsonConverter(typeof(ModuleResultJsonConverterFactory))]
    public new sealed record Failure(Exception Exception) : ModuleResult<T>
    {
        /// <inheritdoc />
        protected override Exception? GetExceptionCore() => Exception;
    }

    /// <summary>
    /// Represents a skipped module execution.
    /// </summary>
    /// <param name="Decision">The skip decision containing the reason.</param>
    [JsonConverter(typeof(ModuleResultJsonConverterFactory))]
    public new sealed record Skipped(SkipDecision Decision) : ModuleResult<T>
    {
        /// <inheritdoc />
        protected override SkipDecision? GetSkipDecisionCore() => Decision;
    }

    // === Implicit conversions from non-generic Failure/Skipped ===

    /// <summary>
    /// Implicitly converts a non-generic <see cref="ModuleResult.Failure"/> to <see cref="ModuleResult{T}"/>.
    /// </summary>
    /// <param name="failure">The failure result to convert.</param>
    public static implicit operator ModuleResult<T>(ModuleResult.Failure failure) =>
        new Failure(failure.Exception)
        {
            Name = failure.Name,
            TypeName = failure.TypeName,
            Duration = failure.Duration,
            StartTime = failure.StartTime,
            EndTime = failure.EndTime,
            Status = failure.Status,
            ModuleType = failure.ModuleType,
        };

    /// <summary>
    /// Implicitly converts a non-generic <see cref="ModuleResult.Skipped"/> to <see cref="ModuleResult{T}"/>.
    /// </summary>
    /// <param name="skipped">The skipped result to convert.</param>
    public static implicit operator ModuleResult<T>(ModuleResult.Skipped skipped) =>
        new Skipped(skipped.Decision)
        {
            Name = skipped.Name,
            TypeName = skipped.TypeName,
            Duration = skipped.Duration,
            StartTime = skipped.StartTime,
            EndTime = skipped.EndTime,
            Status = skipped.Status,
            ModuleType = skipped.ModuleType,
        };

    // === Internal factory methods ===
    internal static Success CreateSuccess(T value, ModuleExecutionContext ctx)
    {
        var (start, end, duration) = GetTimingInfo(ctx);
        return new(value)
        {
            Name = ctx.ModuleType.Name,
            TypeName = ctx.ModuleType.FullName,
            Duration = duration,
            StartTime = start,
            EndTime = end,
            Status = ctx.Status,
            ModuleType = ctx.ModuleType,
        };
    }

    internal static new Failure CreateFailure(Exception exception, ModuleExecutionContext ctx)
    {
        var (start, end, duration) = GetTimingInfo(ctx);
        return new Failure(exception)
        {
            Name = ctx.ModuleType.Name,
            TypeName = ctx.ModuleType.FullName,
            Duration = duration,
            StartTime = start,
            EndTime = end,
            Status = ctx.Status,
            ModuleType = ctx.ModuleType,
        };
    }

    internal static new Skipped CreateSkipped(SkipDecision decision, ModuleExecutionContext ctx)
    {
        var (start, end, duration) = GetTimingInfo(ctx);
        return new Skipped(decision)
        {
            Name = ctx.ModuleType.Name,
            TypeName = ctx.ModuleType.FullName,
            Duration = duration,
            StartTime = start,
            EndTime = end,
            Status = ctx.Status,
            ModuleType = ctx.ModuleType,
        };
    }

    /// <inheritdoc />
    protected override object? GetValueOrDefault() => ValueOrDefault;

    /// <inheritdoc />
    protected override bool PrintMembers(StringBuilder builder) => base.PrintMembers(builder);

    // Prevent external inheritance - only Success, Failure, and Skipped are valid
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
    [UnconditionalSuppressMessage("Trimming", "IL2057", Justification = "Exception types are validated against the System namespace before activation.")]
    public override Exception? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        string? typeName = null;
        string? message = null;
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
                    if (Activator.CreateInstance(exceptionType, message) is Exception ex)
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
/// JSON converter factory that creates typed converters for ModuleResult and ModuleResult&lt;T&gt;.
/// </summary>
internal sealed class ModuleResultJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        // Handle non-generic ModuleResult and its subtypes
        if (typeToConvert == typeof(ModuleResult) ||
            typeToConvert == typeof(ModuleResult.Failure) ||
            typeToConvert == typeof(ModuleResult.Skipped))
        {
            return true;
        }

        // Handle generic ModuleResult<T>
        if (typeToConvert.IsGenericType &&
            typeToConvert.GetGenericTypeDefinition() == typeof(ModuleResult<>))
        {
            return true;
        }

        // Handle nested types like ModuleResult<T>.Success
        var declaringType = typeToConvert.DeclaringType;
        if (declaringType != null &&
            declaringType.IsGenericType &&
            declaringType.GetGenericTypeDefinition() == typeof(ModuleResult<>))
        {
            return true;
        }

        return false;
    }

    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Reflection-based ModuleResult JSON conversion is unsupported in Native AOT; use source-generated serialization metadata.")]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        // For non-generic types
        if (typeToConvert == typeof(ModuleResult) ||
            typeToConvert == typeof(ModuleResult.Failure) ||
            typeToConvert == typeof(ModuleResult.Skipped))
        {
            return new ModuleResultNonGenericJsonConverter();
        }

        if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ModuleResult<>))
        {
            var valueType = typeToConvert.GetGenericArguments()[0];
            var converterType = typeof(ModuleResultJsonConverter<>).MakeGenericType(valueType);
            return (JsonConverter?) Activator.CreateInstance(converterType);
        }

        if (typeToConvert.IsGenericType &&
            typeToConvert.DeclaringType?.IsGenericType == true)
        {
            var valueType = typeToConvert.GetGenericArguments()[0];
            var converterType = typeof(ModuleResultVariantJsonConverter<,>)
                .MakeGenericType(valueType, typeToConvert);
            return (JsonConverter?) Activator.CreateInstance(converterType);
        }

        return null;
    }
}

/// <summary>
/// JSON converter for non-generic ModuleResult types (Failure, Skipped).
/// </summary>
internal sealed class ModuleResultNonGenericJsonConverter : JsonConverter<ModuleResult>
{
    private static readonly ExceptionJsonConverter ExceptionConverter = new();

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result serialization requires runtime type metadata.")]
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Reflection-based ModuleResult JSON conversion is unsupported in Native AOT; use source-generated serialization metadata.")]
    public override ModuleResult? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        string? discriminator = null;
        string? name = null;
        string? typeName = null;
        var duration = TimeSpan.Zero;
        var startTime = DateTimeOffset.MinValue;
        var endTime = DateTimeOffset.MinValue;
        var moduleStatus = ModuleStatus.NotStarted;
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
                    case "Name":
                    case "ModuleName":
                        name = reader.GetString();
                        break;
                    case "TypeName":
                    case "ModuleTypeName":
                        typeName = reader.GetString();
                        break;
                    case "Duration":
                    case "ModuleDuration":
                        duration = JsonSerializer.Deserialize<TimeSpan>(ref reader, options);
                        break;
                    case "StartTime":
                    case "ModuleStart":
                        startTime = reader.GetDateTimeOffset();
                        break;
                    case "EndTime":
                    case "ModuleEnd":
                        endTime = reader.GetDateTimeOffset();
                        break;
                    case "Status":
                    case "ModuleStatus":
                        moduleStatus = JsonSerializer.Deserialize<ModuleStatus>(ref reader, options);
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

        if (name is null)
        {
            throw new JsonException("Name is required but was not found in the JSON.");
        }

        return discriminator switch
        {
            "Failure" => exception is not null
                ? new ModuleResult.Failure(exception)
                {
                    Name = name,
                    TypeName = typeName,
                    Duration = duration,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = moduleStatus,
                }
                : throw new JsonException("Failure result requires an Exception property in the JSON."),
            "Skipped" => skipDecision is not null
                ? new ModuleResult.Skipped(skipDecision)
                {
                    Name = name,
                    TypeName = typeName,
                    Duration = duration,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = moduleStatus,
                }
                : throw new JsonException("Skipped result requires a Decision property in the JSON."),
            _ => throw new JsonException($"Unknown or unsupported discriminator for non-generic ModuleResult: {discriminator}"),
        };
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result serialization requires runtime type metadata.")]
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Reflection-based ModuleResult JSON conversion is unsupported in Native AOT; use source-generated serialization metadata.")]
    public override void Write(Utf8JsonWriter writer, ModuleResult value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Write discriminator
        var discriminator = value switch
        {
            ModuleResult.Failure => "Failure",
            ModuleResult.Skipped => "Skipped",
            _ => throw new JsonException($"Cannot serialize non-generic ModuleResult of type {value.GetType()}"),
        };
        writer.WriteString("$type", discriminator);

        // Write common properties
        writer.WriteString("Name", value.Name);
        if (value.TypeName is not null)
        {
            writer.WriteString("TypeName", value.TypeName);
        }

        writer.WritePropertyName("Duration");
        JsonSerializer.Serialize(writer, value.Duration, options);
        writer.WriteString("StartTime", value.StartTime);
        writer.WriteString("EndTime", value.EndTime);
        writer.WritePropertyName("Status");
        JsonSerializer.Serialize(writer, value.Status, options);

        // Write variant-specific properties
        switch (value)
        {
            case ModuleResult.Failure failure:
                writer.WritePropertyName("Exception");
                ExceptionConverter.Write(writer, failure.Exception, options);
                break;
            case ModuleResult.Skipped skipped:
                writer.WritePropertyName("Decision");
                JsonSerializer.Serialize(writer, skipped.Decision, options);
                break;
        }

        writer.WriteEndObject();
    }
}

/// <summary>
/// JSON converter for ModuleResult&lt;T&gt; that handles polymorphic serialization/deserialization.
/// </summary>
internal sealed class ModuleResultJsonConverter<T> : JsonConverter<ModuleResult<T>>
{
    private static readonly Type DeclaredValueType =
        Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
    private static readonly ExceptionJsonConverter ExceptionConverter = new();

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result serialization requires runtime type metadata.")]
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Reflection-based ModuleResult JSON conversion is unsupported in Native AOT; use source-generated serialization metadata.")]
    public override ModuleResult<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        string? discriminator = null;
        string? valueTypeName = null;
        string? name = null;
        string? typeName = null;
        var duration = TimeSpan.Zero;
        var startTime = DateTimeOffset.MinValue;
        var endTime = DateTimeOffset.MinValue;
        var moduleStatus = ModuleStatus.NotStarted;
        JsonElement? valueElement = null;
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
                    case "$valueType":
                        valueTypeName = reader.GetString();
                        break;
                    case "Name":
                    case "ModuleName":
                        name = reader.GetString();
                        break;
                    case "TypeName":
                    case "ModuleTypeName":
                        typeName = reader.GetString();
                        break;
                    case "Duration":
                    case "ModuleDuration":
                        duration = JsonSerializer.Deserialize<TimeSpan>(ref reader, options);
                        break;
                    case "StartTime":
                    case "ModuleStart":
                        startTime = reader.GetDateTimeOffset();
                        break;
                    case "EndTime":
                    case "ModuleEnd":
                        endTime = reader.GetDateTimeOffset();
                        break;
                    case "Status":
                    case "ModuleStatus":
                        moduleStatus = JsonSerializer.Deserialize<ModuleStatus>(ref reader, options);
                        break;
                    case "Value":
                        valueElement = JsonElement.ParseValue(ref reader);
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

        if (name is null)
        {
            throw new JsonException("Name is required but was not found in the JSON.");
        }

        return discriminator switch
        {
            "Success" when valueElement is null => throw new JsonException(
                "Success result requires a Value property in the JSON."),
            "Success" => new ModuleResult<T>.Success(
                DeserializeSuccessValue(valueElement, valueTypeName, options)!)
            {
                Name = name,
                TypeName = typeName,
                Duration = duration,
                StartTime = startTime,
                EndTime = endTime,
                Status = moduleStatus,
            },
            "Failure" => exception is not null
                ? new ModuleResult<T>.Failure(exception)
                {
                    Name = name,
                    TypeName = typeName,
                    Duration = duration,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = moduleStatus,
                }
                : throw new JsonException("Failure result requires an Exception property in the JSON."),
            "Skipped" => skipDecision is not null
                ? new ModuleResult<T>.Skipped(skipDecision)
                {
                    Name = name,
                    TypeName = typeName,
                    Duration = duration,
                    StartTime = startTime,
                    EndTime = endTime,
                    Status = moduleStatus,
                }
                : throw new JsonException("Skipped result requires a Decision property in the JSON."),
            _ => throw new JsonException($"Unknown discriminator: {discriminator}"),
        };
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result serialization requires runtime type metadata.")]
    [UnconditionalSuppressMessage("Trimming", "IL2057", Justification = "The serialized runtime value type is validated against the declared module result type.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Reflection-based ModuleResult JSON conversion is unsupported in Native AOT; use source-generated serialization metadata.")]
    private static T? DeserializeSuccessValue(
        JsonElement? valueElement,
        string? valueTypeName,
        JsonSerializerOptions options)
    {
        if (valueElement is null)
        {
            return default;
        }

        var valueType = valueTypeName is null
            ? typeof(T)
            : Type.GetType(valueTypeName, throwOnError: false)
              ?? throw new JsonException($"Unknown module result value type '{valueTypeName}'.");
        if (valueTypeName is not null && !DeclaredValueType.IsAssignableFrom(valueType))
        {
            throw new JsonException(
                $"Module result value type '{valueType}' is not assignable to '{typeof(T)}'.");
        }

        return (T?) valueElement.Value.Deserialize(valueType, options);
    }

    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Module result serialization requires runtime type metadata.")]
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Reflection-based ModuleResult JSON conversion is unsupported in Native AOT; use source-generated serialization metadata.")]
    public override void Write(Utf8JsonWriter writer, ModuleResult<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        // Write discriminator
        var discriminator = value switch
        {
            ModuleResult<T>.Success => "Success",
            ModuleResult<T>.Failure => "Failure",
            ModuleResult<T>.Skipped => "Skipped",
            _ => throw new JsonException("Unknown ModuleResult type"),
        };
        writer.WriteString("$type", discriminator);

        // Write common properties
        writer.WriteString("Name", value.Name);
        if (value.TypeName is not null)
        {
            writer.WriteString("TypeName", value.TypeName);
        }

        writer.WritePropertyName("Duration");
        JsonSerializer.Serialize(writer, value.Duration, options);
        writer.WriteString("StartTime", value.StartTime);
        writer.WriteString("EndTime", value.EndTime);
        writer.WritePropertyName("Status");
        JsonSerializer.Serialize(writer, value.Status, options);

        // Write variant-specific properties
        switch (value)
        {
            case ModuleResult<T>.Success success:
                var runtimeValueType = success.Value?.GetType();
                if (runtimeValueType is not null && runtimeValueType != DeclaredValueType)
                {
                    writer.WriteString(
                        "$valueType",
                        runtimeValueType.AssemblyQualifiedName);
                }

                writer.WritePropertyName("Value");
                JsonSerializer.Serialize(writer, success.Value, runtimeValueType ?? typeof(T), options);
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

/// <summary>
/// Adapts the canonical generic converter to a concrete nested result variant.
/// </summary>
internal sealed class ModuleResultVariantJsonConverter<T, TVariant> : JsonConverter<TVariant>
    where TVariant : ModuleResult<T>
{
    private static readonly ModuleResultJsonConverter<T> Converter = new();

    public override TVariant? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return (TVariant?) Converter.Read(ref reader, typeToConvert, options);
    }

    public override void Write(
        Utf8JsonWriter writer,
        TVariant value,
        JsonSerializerOptions options)
    {
        Converter.Write(writer, value, options);
    }
}
