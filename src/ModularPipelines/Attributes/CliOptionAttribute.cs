using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Attributes;

/// <summary>
/// Defines a CLI option that takes a value.
/// </summary>
/// <example>
/// <code>
/// // Space-separated: --namespace myns
/// [CliOption("--namespace", ShortForm = "-n")]
/// public string? Namespace { get; set; }
///
/// // Equals-separated: --set=key=value
/// [CliOption("--set", Format = OptionFormat.EqualsSeparated)]
/// public string[]? Set { get; set; }
///
/// // Multiple values allowed
/// [CliOption("--values", ShortForm = "-f", AllowMultiple = true)]
/// public string[]? Values { get; set; }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class CliOptionAttribute : Attribute
{
    /// <summary>
    /// Gets the option name (e.g., "--namespace", "--output").
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the short form of the option (e.g., "-n" for "--namespace").
    /// When set and <see cref="PreferShortForm"/> is true, the short form will be used.
    /// </summary>
    public string? ShortForm { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether to prefer the short form when building the command.
    /// Defaults to false.
    /// </summary>
    public bool PreferShortForm { get; set; }

    /// <summary>
    /// Gets or sets the format for separating the option name from its value.
    /// Defaults to <see cref="OptionFormat.SpaceSeparated"/>.
    /// </summary>
    public OptionFormat Format { get; set; } = OptionFormat.SpaceSeparated;

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether this option can be specified multiple times with different values.
    /// When true and the property is an array/collection, each value generates a separate option instance.
    /// Defaults to false.
    /// </summary>
    public bool AllowMultiple { get; set; }

    /// <summary>
    /// Gets or sets a custom separator string. When set, this overrides <see cref="Format"/>.
    /// Useful for non-standard separators.
    /// </summary>
    public string? CustomSeparator { get; set; }

    /// <summary>
    /// Initialises a new instance of the <see cref="CliOptionAttribute"/> class.
    /// Initializes a new instance of the <see cref="CliOptionAttribute"/> class.
    /// </summary>
    /// <param name="name">The option name (e.g., "--namespace").</param>
    public CliOptionAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the effective option name based on <see cref="PreferShortForm"/> setting.
    /// </summary>
    /// <returns></returns>
    public string GetEffectiveName() =>
        PreferShortForm && !string.IsNullOrEmpty(ShortForm) ? ShortForm : Name;

    /// <summary>
    /// Gets the separator string to use between option name and value.
    /// </summary>
    /// <returns></returns>
    public string GetSeparator()
    {
        if (!string.IsNullOrEmpty(CustomSeparator))
        {
            return CustomSeparator;
        }

        return Format switch
        {
            OptionFormat.SpaceSeparated => " ",
            OptionFormat.EqualsSeparated => "=",
            OptionFormat.ColonSeparated => ":",
            OptionFormat.NoSeparator => string.Empty,
            _ => " ",
        };
    }
}
