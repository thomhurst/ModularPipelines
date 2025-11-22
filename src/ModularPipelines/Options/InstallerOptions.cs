using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

/// <summary>
/// Options for running an installer executable.
/// </summary>
/// <param name="Path">The path to the installer executable.</param>
public record InstallerOptions([property: PositionalArgument] string Path)
{
    /// <summary>
    /// Gets or sets additional arguments to pass to the installer.
    /// </summary>
    public IEnumerable<string>? Arguments { get; init; }
}
