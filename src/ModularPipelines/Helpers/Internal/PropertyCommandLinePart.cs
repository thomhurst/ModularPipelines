using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Internal representation of a command line part tied to a property.
/// </summary>
internal abstract record PropertyCommandLinePart(PropertyInfo Property);

/// <summary>
/// Internal representation of a positional argument.
/// </summary>
internal sealed record ArgumentPart(PropertyInfo Property, CliArgumentAttribute Attribute) : PropertyCommandLinePart(Property);

/// <summary>
/// Internal representation of a boolean flag.
/// </summary>
internal sealed record FlagPart(PropertyInfo Property, CliFlagAttribute Attribute) : PropertyCommandLinePart(Property);

/// <summary>
/// Internal representation of an option with a value.
/// </summary>
internal sealed record OptionPart(PropertyInfo Property, CliOptionAttribute Attribute) : PropertyCommandLinePart(Property);
