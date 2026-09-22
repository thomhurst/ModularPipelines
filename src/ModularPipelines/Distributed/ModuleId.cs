using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Attributes;
using ModularPipelines.Serialization;

namespace ModularPipelines.Distributed;

/// <summary>
/// Identifies a module in distributed messages, artifacts, and cache fingerprints.
/// </summary>
[TypeConverter(typeof(ModuleIdTypeConverter))]
[JsonConverter(typeof(ModuleIdJsonConverter))]
public readonly struct ModuleId : IEquatable<ModuleId>
{
    private readonly string? _value;

    /// <summary>
    /// Initializes a module identifier with its stable wire-format value.
    /// </summary>
    public ModuleId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        _value = value;
    }

    /// <summary>
    /// Gets the stable wire-format value.
    /// </summary>
    public string Value => _value ?? string.Empty;

    /// <summary>
    /// Derives the stable identifier for a module type.
    /// </summary>
    public static ModuleId FromType(Type moduleType)
    {
        ArgumentNullException.ThrowIfNull(moduleType);
        var configuredId = moduleType.GetCustomAttribute<ModuleIdAttribute>(inherit: false)?.Id;
        return new ModuleId(configuredId ?? (moduleType.IsGenericType
            ? StableTypeName.Get(moduleType)
            : moduleType.FullName ?? moduleType.Name));
    }

    /// <inheritdoc />
    public bool Equals(ModuleId other) =>
        StringComparer.Ordinal.Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ModuleId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

    /// <inheritdoc />
    public override string ToString() => Value;

    public static bool operator ==(ModuleId left, ModuleId right) => left.Equals(right);

    public static bool operator !=(ModuleId left, ModuleId right) => !left.Equals(right);

    public static implicit operator ModuleId(string value) => new(value);

    public static implicit operator string(ModuleId moduleId) => moduleId.Value;
}

internal sealed class ModuleIdTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value) =>
        value is string id
            ? new ModuleId(id)
            : base.ConvertFrom(context, culture, value);
}

internal sealed class ModuleIdJsonConverter : JsonConverter<ModuleId>
{
    public override ModuleId ReadAsPropertyName(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        ReadValue(reader.GetString());

    public override ModuleId Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String
            ? ReadValue(reader.GetString())
            : throw new JsonException("Expected a module identifier string.");

    private static ModuleId ReadValue(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? throw new JsonException("A module identifier cannot be empty.")
            : new ModuleId(value);

    public override void Write(
        Utf8JsonWriter writer,
        ModuleId value,
        JsonSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(value.Value))
        {
            throw new JsonException("A module identifier cannot be empty.");
        }

        writer.WriteStringValue(value.Value);
    }

    public override void WriteAsPropertyName(
        Utf8JsonWriter writer,
        ModuleId value,
        JsonSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(value.Value))
        {
            throw new JsonException("A module identifier cannot be empty.");
        }

        writer.WritePropertyName(value.Value);
    }
}
