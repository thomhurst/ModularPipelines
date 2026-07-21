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
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class CliFlagAttribute : Attribute
{
    /// <summary>
    /// Gets the flag name (e.g., "--debug", "--verbose").
    /// </summary>
    public string Name { get; }

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
    /// Initialises a new instance of the <see cref="CliFlagAttribute"/> class.
    /// Initializes a new instance of the <see cref="CliFlagAttribute"/> class.
    /// </summary>
    /// <param name="name">The flag name (e.g., "--debug").</param>
    public CliFlagAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the effective flag name based on <see cref="PreferShortForm"/> setting.
    /// </summary>
    /// <returns></returns>
    public string GetEffectiveName() =>
        PreferShortForm && !string.IsNullOrEmpty(ShortForm) ? ShortForm : Name;
}
