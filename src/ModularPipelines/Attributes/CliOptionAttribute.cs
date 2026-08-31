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
/// // Collections automatically repeat the option for each value
/// [CliOption("--values", ShortForm = "-f")]
/// public string[]? Values { get; set; }
/// </code>
/// </example>
/// <param name="name">The option name (e.g., "--namespace").</param>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class CliOptionAttribute(string name) : Attribute
{
    /// <summary>
    /// Gets the option name (e.g., "--namespace", "--output").
    /// </summary>
    public string Name { get; } = name;

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
    /// Gets or sets whether this option requires a value.
    /// For <see cref="CliOptionValueArity.Optional"/>, a null property omits the option,
    /// <see cref="ModularPipelines.Models.CliOptionValue.Bare"/> renders the bare option, and a value renders
    /// the option with that value. Non-null string values are rendered literally, including empty and whitespace values.
    /// </summary>
    public CliOptionValueArity ValueArity { get; set; } = CliOptionValueArity.Required;

    /// <summary>
    /// Gets or sets a value indicating whether collection values share one option occurrence.
    /// By default, collections repeat the option for every value.
    /// Grouped values require <see cref="OptionFormat.SpaceSeparated"/>.
    /// </summary>
    /// <example><c>--arguments first=value second=value</c></example>
    public bool GroupValues { get; set; }

    /// <summary>
    /// Gets or sets the separator used to join collection elements into one option value.
    /// A <see langword="null"/> value renders collection elements independently.
    /// </summary>
    /// <example><c>--environment key1=value1,key2=value2</c></example>
    public string? CollectionSeparator { get; set; }

    /// <summary>
    /// Gets or sets the semantic phase used to order this option.
    /// </summary>
    public CommandLinePhase Phase { get; set; } = CommandLinePhase.Normal;
}
