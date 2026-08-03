using ModularPipelines.Attributes;

namespace ModularPipelines.OptionsGenerator.Models;

/// <summary>
/// Represents a single CLI option or flag.
/// </summary>
public record CliOptionDefinition
{
    /// <summary>
    /// The CLI switch name (e.g., "--output", "-o").
    /// </summary>
    public required string SwitchName { get; init; }

    /// <summary>
    /// Short form if available (e.g., "-o" for "--output").
    /// </summary>
    public string? ShortForm { get; init; }

    /// <summary>
    /// Whether generated attributes should render the short form when available.
    /// </summary>
    public bool PreferShortForm { get; init; }

    /// <summary>
    /// Generated C# property name.
    /// </summary>
    public required string PropertyName { get; init; }

    /// <summary>
    /// C# type (e.g., "string?", "bool?", "int?").
    /// </summary>
    public required string CSharpType { get; init; }

    /// <summary>
    /// C# type emitted for the generated property.
    /// </summary>
    public string PropertyType => ValueArity == CliOptionValueArity.Optional
        ? "CliOptionValue?"
        : CSharpType;

    /// <summary>
    /// Description for XML documentation.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Documentation URL for options whose metadata does not come from CLI help.
    /// </summary>
    public string? DocumentationUrl { get; init; }

    /// <summary>
    /// Optional edition, license, plugin, or environment availability note.
    /// </summary>
    public string? Availability { get; init; }

    /// <summary>
    /// Whether this is a boolean flag (no value).
    /// </summary>
    public bool IsFlag { get; init; }

    /// <summary>
    /// Whether a value is required, optional, or absent.
    /// </summary>
    public CliOptionValueArity ValueArity { get; init; } = CliOptionValueArity.Required;

    /// <summary>
    /// Semantic rendering phase for this option.
    /// </summary>
    public CommandLinePhase Phase { get; init; } = CommandLinePhase.Normal;

    /// <summary>
    /// Whether the option is required.
    /// </summary>
    public bool IsRequired { get; init; }

    /// <summary>
    /// Whether the option can be specified multiple times.
    /// </summary>
    public bool AcceptsMultipleValues { get; init; }

    /// <summary>
    /// Whether this is a key-value pair option.
    /// </summary>
    public bool IsKeyValue { get; init; }

    /// <summary>
    /// Whether generated code needs the ModularPipelines.Models namespace for this option type.
    /// </summary>
    public bool RequiresModelsNamespace
        => IsKeyValue
           || ValueArity == CliOptionValueArity.Optional
           || CSharpType.Contains("CliValuePair", StringComparison.Ordinal);

    /// <summary>
    /// Whether the value is numeric.
    /// </summary>
    public bool IsNumeric { get; init; }

    /// <summary>
    /// Whether this option contains a secret value that should be obfuscated in logs.
    /// Automatically detected for options with "Secret", "Password", "Token", "Key", or "Credential" in the name.
    /// </summary>
    public bool IsSecret { get; init; }

    /// <summary>
    /// For key-value options, the keys whose values contain secrets.
    /// </summary>
    public IReadOnlyList<string> SecretValueKeys { get; init; } = [];

    /// <summary>
    /// The value separator (space, equals, etc.).
    /// </summary>
    public string ValueSeparator { get; init; } = " ";

    /// <summary>
    /// If this option has constrained values, the enum definition for it.
    /// </summary>
    public CliEnumDefinition? EnumDefinition { get; init; }

    /// <summary>
    /// Validation constraints (e.g., min/max for numeric values).
    /// </summary>
    public CliValidationConstraints? ValidationConstraints { get; init; }

    /// <summary>
    /// Determines the appropriate attribute type.
    /// </summary>
    public OptionAttributeType AttributeType => IsFlag
        ? OptionAttributeType.BooleanCommandSwitch
        : ValueSeparator == "="
            ? OptionAttributeType.CommandEqualsSeparatorSwitch
            : OptionAttributeType.CommandSwitch;
}

/// <summary>
/// The type of attribute to use for this option.
/// </summary>
public enum OptionAttributeType
{
    CommandSwitch,
    BooleanCommandSwitch,
    CommandEqualsSeparatorSwitch
}

/// <summary>
/// Validation constraints for an option.
/// </summary>
public record CliValidationConstraints
{
    /// <summary>
    /// Minimum value for numeric options.
    /// </summary>
    public int? MinValue { get; init; }

    /// <summary>
    /// Maximum value for numeric options.
    /// </summary>
    public int? MaxValue { get; init; }

    /// <summary>
    /// Regex pattern for string validation.
    /// </summary>
    public string? Pattern { get; init; }
}
