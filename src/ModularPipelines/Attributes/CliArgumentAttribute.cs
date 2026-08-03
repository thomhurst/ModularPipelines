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
/// [CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
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
    /// Gets or sets the argument name for documentation purposes.
    /// This is not used in command building, only for generating help text.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the semantic phase used to order this argument.
    /// Positional arguments default to the pass-through phase after option parsing.
    /// Use <see cref="CommandLinePhase.EarlyOperand"/> for operands that precede regular options.
    /// </summary>
    public CommandLinePhase Phase { get; set; } = CommandLinePhase.Passthrough;

    /// <summary>
    /// Gets or sets a value indicating whether <c>--</c> is emitted immediately
    /// before this argument when it has a value.
    /// </summary>
    public bool PrependOptionTerminator { get; set; }

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
