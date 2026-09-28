using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Distributed;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Serialization;

namespace ModularPipelines;

/// <summary>
/// JSON converter for Exception objects in distributed module results.
/// </summary>
/// <remarks>
/// <para>
/// The wire format carries the type name (never the assembly-qualified name, so versions and
/// public key tokens are not leaked), the message, the stack trace, the inner exception chain and,
/// for <see cref="AggregateException"/>, every inner exception. Messages and stack traces can contain
/// sensitive data such as file paths; sanitize them before they are thrown if that matters.
/// </para>
/// <para>
/// Deserialization only activates exception types from the <c>System</c> namespaces and an
/// allowlist of ModularPipelines exceptions whose typed data is carried explicitly. Every other
/// type becomes a <see cref="RemoteModuleException"/> that keeps the original diagnostics, which
/// prevents type injection.
/// </para>
/// </remarks>
internal sealed class ExceptionJsonConverter : JsonConverter<Exception>
{
    private const int MaximumDepth = 16;

    private static readonly IReadOnlyDictionary<string, PortableExceptionCodec> Codecs =
        new Dictionary<string, PortableExceptionCodec>(StringComparer.Ordinal)
        {
            [typeof(ModuleTimeoutException).FullName!] = new(
                static exception => exception is ModuleTimeoutException timeout
                    ? new Dictionary<string, string>
                    {
                        ["ModuleType"] = StableTypeName.Get(timeout.ModuleType),
                        ["ConfiguredTimeout"] = timeout.ConfiguredTimeout.Ticks.ToString(CultureInfo.InvariantCulture),
                        ["ElapsedTime"] = timeout.ElapsedTime.Ticks.ToString(CultureInfo.InvariantCulture),
                        ["WasCancellationTokenRespected"] = timeout.WasCancellationTokenRespected ? "true" : "false",
                    }
                    : null,
                static (node, _) =>
                    TryResolveModuleType(node, out var moduleType)
                    && TryGetTicks(node, "ConfiguredTimeout", out var configured)
                    && TryGetTicks(node, "ElapsedTime", out var elapsed)
                        ? new ModuleTimeoutException(
                            moduleType,
                            configured,
                            elapsed,
                            node.Properties.GetValueOrDefault("WasCancellationTokenRespected") == "true")
                        : null),
            [typeof(ModuleFailedException).FullName!] = new(
                static exception => exception is ModuleFailedException failed
                    ? new Dictionary<string, string> { ["ModuleType"] = StableTypeName.Get(failed.ModuleType) }
                    : null,
                static (node, inner) => TryResolveModuleType(node, out var moduleType)
                    ? new ModuleFailedException(moduleType, inner ?? new Exception(node.Message))
                    : null),
            [typeof(PipelineException).FullName!] = new(
                static _ => null,
                static (node, inner) => new PipelineException(node.Message, inner)),
            [typeof(RequirementNotMetException).FullName!] = new(
                static _ => null,
                static (node, _) => new RequirementNotMetException(node.Message)),
            [typeof(PipelineCanceledException).FullName!] = new(
                static _ => null,
                static (node, _) => new PipelineCanceledException(node.Message)),
        };

    public override Exception? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return CreateException(ReadNode(ref reader, depth: 0));
    }

    public override void Write(Utf8JsonWriter writer, Exception value, JsonSerializerOptions options) =>
        WriteNode(writer, value, depth: 0);

    private static void WriteNode(Utf8JsonWriter writer, Exception value, int depth)
    {
        writer.WriteStartObject();

        var remoteException = value as RemoteModuleException;
        var typeName = remoteException?.OriginalExceptionType ?? value.GetType().FullName;
        var stackTrace = remoteException?.RemoteStackTrace ?? value.StackTrace;
        var aggregate = value as AggregateException;
        var message = remoteException?.OriginalMessage
                      ?? (aggregate is null ? value.Message : GetAggregateBaseMessage(aggregate));

        writer.WriteString("Type", typeName);
        writer.WriteString("Message", message);
        writer.WriteString("StackTrace", stackTrace);

        if (remoteException is null
            && typeName is not null
            && Codecs.TryGetValue(typeName, out var codec)
            && codec.GetProperties(value) is { Count: > 0 } properties)
        {
            writer.WriteStartObject("Properties");
            foreach (var (name, propertyValue) in properties)
            {
                writer.WriteString(name, propertyValue);
            }

            writer.WriteEndObject();
        }

        if (depth < MaximumDepth)
        {
            if (aggregate is not null)
            {
                writer.WriteStartArray("InnerExceptions");
                foreach (var inner in aggregate.InnerExceptions)
                {
                    WriteNode(writer, inner, depth + 1);
                }

                writer.WriteEndArray();
            }
            else if (value.InnerException is { } innerException)
            {
                writer.WritePropertyName("InnerException");
                WriteNode(writer, innerException, depth + 1);
            }
        }

        writer.WriteEndObject();
    }

    private static string GetAggregateBaseMessage(AggregateException aggregate)
    {
        // AggregateException.Message appends every inner message; recreating it from the full
        // message would repeat them.
        var suffix = string.Concat(aggregate.InnerExceptions.Select(static inner => $" ({inner.Message})"));
        var message = aggregate.Message;
        return suffix.Length > 0 && message.EndsWith(suffix, StringComparison.Ordinal)
            ? message[..^suffix.Length]
            : message;
    }

    private static ExceptionNode ReadNode(ref Utf8JsonReader reader, int depth)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected an exception object.");
        }

        if (depth > MaximumDepth)
        {
            throw new JsonException("The exception chain is nested too deeply.");
        }

        var node = new ExceptionNode();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return node;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                continue;
            }

            var propertyName = reader.GetString();
            reader.Read();
            switch (propertyName)
            {
                case "Type":
                    node.TypeName = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                    break;
                case "Message":
                    node.Message = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                    break;
                case "StackTrace":
                    node.StackTrace = reader.TokenType == JsonTokenType.Null ? null : reader.GetString();
                    break;
                case "Properties" when reader.TokenType == JsonTokenType.StartObject:
                    while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var name = reader.GetString()!;
                        reader.Read();
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            node.Properties[name] = reader.GetString()!;
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }

                    break;
                case "InnerException" when reader.TokenType == JsonTokenType.StartObject:
                    node.Inner = ReadNode(ref reader, depth + 1);
                    break;
                case "InnerExceptions" when reader.TokenType == JsonTokenType.StartArray:
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        node.InnerExceptions.Add(ReadNode(ref reader, depth + 1));
                    }

                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        throw new JsonException("Unexpected end of exception data.");
    }

    private static Exception CreateException(ExceptionNode node)
    {
        var inner = node.Inner is null ? null : CreateException(node.Inner);
        var inners = node.InnerExceptions.Select(CreateException).ToArray();
        var message = node.Message ?? "Deserialized exception";
        var exception = node.TypeName is null
            ? new Exception(message, inner)
            : TryCreateAllowlistedException(node, inner)
              ?? TryCreateSystemException(node.TypeName, message, inner, inners)
              ?? new RemoteModuleException(node.TypeName, message, node.StackTrace, workerId: null, inner);

        if (exception is not RemoteModuleException && !string.IsNullOrEmpty(node.StackTrace))
        {
            ExceptionDispatchInfo.SetRemoteStackTrace(exception, node.StackTrace);
        }

        return exception;
    }

    private static Exception? TryCreateAllowlistedException(ExceptionNode node, Exception? inner)
    {
        if (!Codecs.TryGetValue(node.TypeName!, out var codec))
        {
            return null;
        }

        try
        {
            return codec.Create(node, inner);
        }
        catch (Exception exception) when (exception is ArgumentException or FormatException)
        {
            return null;
        }
    }

    [UnconditionalSuppressMessage("Trimming", "IL2057", Justification = "Exception types are restricted to the System namespaces before activation.")]
    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Exception types are restricted to the System namespaces before activation.")]
    private static Exception? TryCreateSystemException(
        string typeName,
        string message,
        Exception? inner,
        Exception[] inners)
    {
        var exceptionType = Type.GetType(typeName, throwOnError: false);
        if (exceptionType is null
            || !typeof(Exception).IsAssignableFrom(exceptionType)
            || exceptionType.IsAbstract
            || !IsSystemNamespace(exceptionType.Namespace))
        {
            return null;
        }

        try
        {
            if (exceptionType == typeof(AggregateException))
            {
                return new AggregateException(message, inners);
            }

            // Prefer (message, innerException): single-string constructors of types such as
            // ArgumentNullException and ObjectDisposedException take a parameter name, not a message.
            if (exceptionType.GetConstructor([typeof(string), typeof(Exception)]) is { } messageAndInner)
            {
                return (Exception) messageAndInner.Invoke([message, inner]);
            }

            if (exceptionType.GetConstructor([typeof(string)]) is { } messageOnly)
            {
                return (Exception) messageOnly.Invoke([message]);
            }

            return exceptionType.GetConstructor(Type.EmptyTypes)?.Invoke([]) as Exception;
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            return null;
        }
    }

    private static bool IsSystemNamespace(string? typeNamespace) =>
        typeNamespace is not null
        && (typeNamespace == "System" || typeNamespace.StartsWith("System.", StringComparison.Ordinal));

    [UnconditionalSuppressMessage("Trimming", "IL2057", Justification = "Resolved types must implement IModule; unresolvable types fall back to RemoteModuleException.")]
    private static bool TryResolveModuleType(ExceptionNode node, [NotNullWhen(true)] out Type? moduleType)
    {
        moduleType = node.Properties.TryGetValue("ModuleType", out var name)
            ? Type.GetType(name, throwOnError: false)
            : null;
        if (moduleType is not null && typeof(IModule).IsAssignableFrom(moduleType))
        {
            return true;
        }

        moduleType = null;
        return false;
    }

    private static bool TryGetTicks(ExceptionNode node, string name, out TimeSpan value)
    {
        if (node.Properties.TryGetValue(name, out var text)
            && long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ticks))
        {
            value = TimeSpan.FromTicks(ticks);
            return true;
        }

        value = default;
        return false;
    }

    private sealed class ExceptionNode
    {
        public string? TypeName { get; set; }

        public string? Message { get; set; }

        public string? StackTrace { get; set; }

        public Dictionary<string, string> Properties { get; } = [with(StringComparer.Ordinal)];

        public ExceptionNode? Inner { get; set; }

        public List<ExceptionNode> InnerExceptions { get; } = [];
    }

    private sealed record PortableExceptionCodec(
        Func<Exception, IReadOnlyDictionary<string, string>?> GetProperties,
        Func<ExceptionNode, Exception?, Exception?> Create);
}
