using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed;

/// <summary>
/// Identifies one distributed worker process for the lifetime of a run.
/// </summary>
/// <remarks>
/// Worker identifiers are opaque. They contain 1-128 ASCII letters, digits, <c>.</c>, <c>_</c>
/// or <c>-</c> so every backend can embed them in keys and channel names. By default each
/// process derives its identifier from its instance index with <see cref="FromInstanceIndex"/>.
/// </remarks>
[TypeConverter(typeof(WorkerIdTypeConverter))]
[JsonConverter(typeof(WorkerIdJsonConverter))]
public readonly struct WorkerId : IEquatable<WorkerId>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a worker identifier.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <exception cref="ArgumentException">The value is empty, too long or contains unsupported characters.</exception>
    public WorkerId(string value)
    {
        DistributedIdentifier.ThrowIfInvalid(value, "Worker identifier");
        _value = value;
    }

    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public string Value => _value ?? string.Empty;

    /// <summary>
    /// Creates the default identifier for the process with the supplied instance index.
    /// </summary>
    /// <param name="instanceIndex">The zero-based distributed instance index.</param>
    /// <returns>The identifier <c>instance-{instanceIndex}</c>.</returns>
    public static WorkerId FromInstanceIndex(int instanceIndex)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(instanceIndex);
        return new WorkerId(string.Create(CultureInfo.InvariantCulture, $"instance-{instanceIndex}"));
    }

    /// <inheritdoc />
    public bool Equals(WorkerId other) => StringComparer.Ordinal.Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is WorkerId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <summary>Compares two identifiers for equality.</summary>
    public static bool operator ==(WorkerId left, WorkerId right) => left.Equals(right);

    /// <summary>Compares two identifiers for inequality.</summary>
    public static bool operator !=(WorkerId left, WorkerId right) => !left.Equals(right);
}

internal sealed class WorkerIdTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value) =>
        value is string id
            ? new WorkerId(id)
            : base.ConvertFrom(context, culture, value);
}

internal sealed class WorkerIdJsonConverter : JsonConverter<WorkerId>
{
    public override WorkerId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String
            ? ReadValue(reader.GetString())
            : throw new JsonException("Expected a worker identifier string.");

    public override WorkerId ReadAsPropertyName(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        ReadValue(reader.GetString());

    public override void Write(Utf8JsonWriter writer, WorkerId value, JsonSerializerOptions options) =>
        writer.WriteStringValue(Validate(value).Value);

    public override void WriteAsPropertyName(Utf8JsonWriter writer, WorkerId value, JsonSerializerOptions options) =>
        writer.WritePropertyName(Validate(value).Value);

    private static WorkerId ReadValue(string? value) =>
        DistributedIdentifier.IsValid(value)
            ? new WorkerId(value!)
            : throw new JsonException($"'{value}' is not a valid worker identifier.");

    private static WorkerId Validate(WorkerId value) =>
        DistributedIdentifier.IsValid(value.Value)
            ? value
            : throw new JsonException("A worker identifier cannot be empty.");
}
