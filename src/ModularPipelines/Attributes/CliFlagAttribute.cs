using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Attributes;

/// <summary>
/// Defines a boolean or counted CLI flag. Boolean flags are included when true;
/// counted flags are repeated the specified number of times.
/// </summary>
/// <example>
/// <code>
/// [CliFlag("--debug", ShortForm = "-d")]
/// public bool? Debug { get; set; }
///
/// [CliFlag("--verbose")]
/// public bool? Verbose { get; set; }
///
/// [CliFlag("--verbose", ShortForm = "-v")]
/// public int? Verbosity { get; set; }
/// </code>
/// </example>
/// <param name="name">The flag name (e.g., "--debug").</param>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class CliFlagAttribute(string name) : Attribute
{
    /// <summary>
    /// Gets the flag name (e.g., "--debug", "--verbose").
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets or sets the short form of the flag (e.g., "-d" for "--debug").
    /// When set and <see cref="PreferShortForm"/> is true, the short form will be used.
    /// </summary>
    public string? ShortForm { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether to prefer the short form when building the command.
    /// Defaults to false.
    /// </summary>
    public bool PreferShortForm { get; set; }

    /// <summary>
    /// Gets or sets the semantic phase used to order this flag.
    /// </summary>
    public CommandLinePhase Phase { get; set; } = CommandLinePhase.Normal;
}
