using System.ComponentModel;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Representation of a command line part tied to a property.
/// </summary>
public abstract record PropertyCommandLinePart(string PropertyName, Func<object, object?> Getter)
{
    /// <summary>
    /// Gets whether the property's type is supported by its CLI attribute, when known.
    /// </summary>
    public bool? IsSupportedPropertyType { get; init; }

    /// <summary>
    /// Gets a value indicating whether the property belongs before command parts.
    /// </summary>
    public bool IsGlobalOption { get; init; }

    /// <summary>
    /// Gets the semantic rendering phase.
    /// </summary>
    public abstract CommandLinePhase Phase { get; }
}

/// <summary>
/// Representation of a positional argument.
/// </summary>
public sealed record ArgumentPart(
    string PropertyName,
    Func<object, object?> Getter,
    CliArgumentAttribute Attribute) : PropertyCommandLinePart(PropertyName, Getter)
{
    /// <summary>
    /// Gets a value indicating whether the argument position was explicitly supplied.
    /// </summary>
    public bool HasExplicitPosition { get; init; }

    /// <inheritdoc />
    public override CommandLinePhase Phase => Attribute.Phase;
}

/// <summary>
/// Representation of a boolean flag.
/// </summary>
public sealed record FlagPart(
    string PropertyName,
    Func<object, object?> Getter,
    CliFlagAttribute Attribute) : PropertyCommandLinePart(PropertyName, Getter)
{
    /// <inheritdoc />
    public override CommandLinePhase Phase => Attribute.Phase;
}

/// <summary>
/// Representation of an option with a value.
/// </summary>
public sealed record OptionPart(
    string PropertyName,
    Func<object, object?> Getter,
    CliOptionAttribute Attribute) : PropertyCommandLinePart(PropertyName, Getter)
{
    /// <summary>
    /// Gets the legacy optional-value compatibility marker.
    /// Retained for binary compatibility with schema-2 generated metadata; ignored by validation.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool AllowsLegacyOptionalValues { get; init; }

    /// <summary>
    /// Gets the number of separate operands consumed by one manual occurrence of this option.
    /// </summary>
    public int ManualOperandCount { get; init; } = -1;

    /// <inheritdoc />
    public override CommandLinePhase Phase => Attribute.Phase;
}
