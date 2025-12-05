using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetAddPackageOptions : DotNetOptions
{
    public DotNetAddPackageOptions(
        string project,
        string packageName
    )
    {
        CommandParts = ["add", "[<PROJECT>]", "package", "<PACKAGE_NAME>"];

        Project = project;

        PackageName = packageName;
    }

    public DotNetAddPackageOptions(
        string packageName
    )
    {
        CommandParts = ["add", "[<PROJECT>]", "package", "<PACKAGE_NAME>"];

        PackageName = packageName;
    }

    [CliArgument(Name = "[<PROJECT>]")]
    public virtual string? Project { get; set; }

    [CliArgument(Name = "<PACKAGE_NAME>")]
    public virtual string? PackageName { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CliOption("--package-directory")]
    public virtual string? PackageDirectory { get; set; }

    [CliFlag("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }
}
