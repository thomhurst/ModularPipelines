using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetAddPackageOptions : DotNetOptions
{
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
