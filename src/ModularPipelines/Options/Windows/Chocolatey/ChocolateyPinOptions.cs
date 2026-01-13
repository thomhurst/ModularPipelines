using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Options for the 'choco pin' command.
/// Pins or unpins a package to prevent upgrades.
/// </summary>
[ExcludeFromCodeCoverage]
public record ChocolateyPinOptions : ChocolateyOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChocolateyPinOptions"/> class
    /// with the specified action and package name.
    /// </summary>
    /// <param name="action">The pin action: "add", "remove", or "list".</param>
    /// <param name="package">The name of the package to pin or unpin (optional for "list").</param>
    public ChocolateyPinOptions(string action, string? package = null)
    {
        Action = action;
        Package = package;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "pin";

    /// <summary>
    /// Gets the pin action: "add", "remove", or "list".
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Action { get; }

    /// <summary>
    /// Gets or sets the package name to pin or unpin.
    /// </summary>
    [CliFlag("--name")]
    public virtual string? Package { get; set; }

    /// <summary>
    /// Gets or sets the version to pin.
    /// </summary>
    [CliFlag("--version")]
    public new virtual string? Version { get; set; }
}
