using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Internal representation of a command line part tied to a property.
/// This type is in the Internal namespace and is not intended for public use.
/// </summary>
public abstract record PropertyCommandLinePart(PropertyInfo Property);

/// <summary>
/// Internal representation of a positional argument.
/// This type is in the Internal namespace and is not intended for public use.
/// </summary>
public sealed record ArgumentPart(PropertyInfo Property, CliArgumentAttribute Attribute) : PropertyCommandLinePart(Property);

/// <summary>
/// Internal representation of a boolean flag.
/// This type is in the Internal namespace and is not intended for public use.
/// </summary>
public sealed record FlagPart(PropertyInfo Property, CliFlagAttribute Attribute) : PropertyCommandLinePart(Property);

/// <summary>
/// Internal representation of an option with a value.
/// This type is in the Internal namespace and is not intended for public use.
/// </summary>
public sealed record OptionPart(PropertyInfo Property, CliOptionAttribute Attribute) : PropertyCommandLinePart(Property);
