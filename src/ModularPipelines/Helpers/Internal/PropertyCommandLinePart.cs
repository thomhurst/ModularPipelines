using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Representation of a command line part tied to a property.
/// </summary>
public abstract record PropertyCommandLinePart(PropertyInfo Property);

/// <summary>
/// Representation of a positional argument.
/// </summary>
public sealed record ArgumentPart(PropertyInfo Property, CliArgumentAttribute Attribute) : PropertyCommandLinePart(Property);

/// <summary>
/// Representation of a boolean flag.
/// </summary>
public sealed record FlagPart(PropertyInfo Property, CliFlagAttribute Attribute) : PropertyCommandLinePart(Property);

/// <summary>
/// Representation of an option with a value.
/// </summary>
public sealed record OptionPart(PropertyInfo Property, CliOptionAttribute Attribute) : PropertyCommandLinePart(Property);
