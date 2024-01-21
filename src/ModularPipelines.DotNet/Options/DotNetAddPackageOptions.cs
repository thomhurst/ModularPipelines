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
    public string? Framework { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }

    [CommandSwitch("--package-directory")]
    public string? PackageDirectory { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public bool? Prerelease { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}
