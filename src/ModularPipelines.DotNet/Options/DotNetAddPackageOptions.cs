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

    [PositionalArgument(PlaceholderName = "[<PROJECT>]")]
    public string? Project { get; set; }

    [PositionalArgument(PlaceholderName = "<PACKAGE_NAME>")]
    public string? PackageName { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CommandSwitch("--package-directory")]
    public virtual string? PackageDirectory { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }
}
