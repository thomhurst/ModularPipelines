using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed;

/// <summary>
/// Identifies a capability that a distributed worker can provide.
/// </summary>
[TypeConverter(typeof(CapabilityTypeConverter))]
[JsonConverter(typeof(CapabilityJsonConverter))]
public readonly struct Capability : IEquatable<Capability>
{
    private readonly string? _name;

    /// <summary>
    /// Initializes a capability with its wire-format name.
    /// </summary>
    public Capability(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _name = name;
    }

    /// <summary>
    /// Gets the wire-format capability name.
    /// </summary>
    public string Name => _name ?? string.Empty;

    /// <summary>Windows operating system capability.</summary>
    public static Capability Windows { get; } = new(Names.Windows);

    /// <summary>Linux operating system capability.</summary>
    public static Capability Linux { get; } = new(Names.Linux);

    /// <summary>macOS operating system capability.</summary>
    public static Capability MacOS { get; } = new(Names.MacOS);

    /// <summary>FreeBSD operating system capability.</summary>
    public static Capability FreeBSD { get; } = new(Names.FreeBSD);

    /// <summary>Docker capability.</summary>
    public static Capability Docker { get; } = new(Names.Docker);

    /// <summary>GPU capability.</summary>
    public static Capability Gpu { get; } = new(Names.Gpu);

    /// <summary>
    /// Gets the operating-system capabilities. A worker advertises at most one of them.
    /// </summary>
    internal static IReadOnlyList<Capability> OperatingSystems { get; } = [Windows, Linux, MacOS, FreeBSD];

    /// <summary>
    /// Gets the capability for the operating system this process runs on, when it is known.
    /// </summary>
    internal static Capability? CurrentOperatingSystem
    {
        get
        {
            // Return explicitly: a bare null in a conditional would bind to the implicit string
            // conversion and throw on an unrecognized platform.
            if (OperatingSystem.IsWindows())
            {
                return Windows;
            }

            if (OperatingSystem.IsLinux())
            {
                return Linux;
            }

            if (OperatingSystem.IsMacOS())
            {
                return MacOS;
            }

            return OperatingSystem.IsFreeBSD() ? FreeBSD : (Capability?) null;
        }
    }

    /// <summary>
    /// Gets whether this capability identifies an operating system.
    /// </summary>
    internal bool IsOperatingSystem => OperatingSystems.Contains(this);

    /// <inheritdoc />
    public bool Equals(Capability other) =>
        StringComparer.OrdinalIgnoreCase.Equals(Name, other.Name);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Capability other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);

    /// <inheritdoc />
    public override string ToString() => Name;

    public static bool operator ==(Capability left, Capability right) => left.Equals(right);

    public static bool operator !=(Capability left, Capability right) => !left.Equals(right);

    public static implicit operator Capability(string name) => new(name);

    public static implicit operator string(Capability capability) => capability.Name;

    /// <summary>
    /// Compile-time capability names suitable for attribute arguments.
    /// </summary>
    public static class Names
    {
        public const string Windows = "windows";
        public const string Linux = "linux";
        public const string MacOS = "macos";
        public const string FreeBSD = "freebsd";
        public const string Docker = "docker";
        public const string Gpu = "gpu";
    }
}

internal sealed class CapabilityTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(
        ITypeDescriptorContext? context,
        CultureInfo? culture,
        object value) =>
        value is string name
            ? new Capability(name)
            : base.ConvertFrom(context, culture, value);
}

internal sealed class CapabilityJsonConverter : JsonConverter<Capability>
{
    public override Capability Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String
            ? new Capability(reader.GetString()!)
            : throw new JsonException("Expected a capability string.");

    public override void Write(
        Utf8JsonWriter writer,
        Capability value,
        JsonSerializerOptions options) =>
        WriteValue(writer, value);

    internal static void WriteValue(Utf8JsonWriter writer, Capability value)
    {
        if (string.IsNullOrWhiteSpace(value.Name))
        {
            throw new JsonException("A capability name cannot be empty.");
        }

        writer.WriteStringValue(value.Name);
    }
}
