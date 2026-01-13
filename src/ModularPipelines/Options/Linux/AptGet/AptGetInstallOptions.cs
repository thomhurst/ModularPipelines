using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Linux.AptGet;

[ExcludeFromCodeCoverage]
public partial record AptGetInstallOptions : AptGetOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AptGetInstallOptions"/> class
    /// without specifying a package. Use this when running commands like
    /// <c>apt-get install --fix-broken</c> that don't require a package name.
    /// </summary>
    public AptGetInstallOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AptGetInstallOptions"/> class
    /// with the specified package to install.
    /// </summary>
    /// <param name="package">The name of the package to install.</param>
    public AptGetInstallOptions(string package)
    {
        Package = package;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "install";

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Package { get; }
}