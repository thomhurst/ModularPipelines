using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Attributes;

/// <summary>
/// Defines a positional CLI argument (not prefixed with -- or -).
/// </summary>
/// <example>
/// <code>
/// // Required positional argument
/// [CliArgument(0)]
/// public required string ReleaseName { get; set; }
///
/// // Optional positional argument (nullable = optional)
/// [CliArgument(1)]
/// public string? ChartReference { get; set; }
///
/// // Argument placed before options
/// [CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)]
/// public string? WorkingDirectory { get; set; }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class CliArgumentAttribute : Attribute
{
    /// <summary>
    /// Gets the zero-based position of this argument relative to other positional arguments.
    /// </summary>
    public int Position { get; }

    /// <summary>
    /// Gets or sets where this argument should be placed relative to options/flags.
    /// Defaults to <see cref="ArgumentPlacement.AfterOptions"/>.
    /// </summary>
    public ArgumentPlacement Placement { get; set; } = ArgumentPlacement.AfterOptions;

    /// <summary>
    /// Gets or sets the argument name for documentation purposes.
    /// This is not used in command building, only for generating help text.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Initialises a new instance of the <see cref="CliArgumentAttribute"/> class.
    /// Initializes a new instance of the <see cref="CliArgumentAttribute"/> class with default position 0.
    /// </summary>
    public CliArgumentAttribute() : this(0)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="CliArgumentAttribute"/> class.
    /// Initializes a new instance of the <see cref="CliArgumentAttribute"/> class.
    /// </summary>
    /// <param name="position">The zero-based position of this argument.</param>
    public CliArgumentAttribute(int position)
    {
        Position = position;
    }
}
