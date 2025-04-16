using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetDeleteOptions : DotNetOptions
{
    public DotNetNugetDeleteOptions(
        string packageNamePackageVersion
    )
    {
        CommandParts = ["nuget", "delete", "[<PACKAGE_NAME> <PACKAGE_VERSION>]"];

        PackageNamePackageVersion = packageNamePackageVersion;
    }

    public DotNetNugetDeleteOptions()
    {
        CommandParts = ["nuget", "delete", "[<PACKAGE_NAME> <PACKAGE_VERSION>]"];
    }

    [PositionalArgument(PlaceholderName = "[<PACKAGE_NAME> <PACKAGE_VERSION>]")]
    public string? PackageNamePackageVersion { get; set; }

    [BooleanCommandSwitch("--force-english-output")]
    public virtual bool? ForceEnglishOutput { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CommandSwitch("--api-key")]
    public virtual string? ApiKey { get; set; }

    [BooleanCommandSwitch("--no-service-endpoint")]
    public virtual bool? NoServiceEndpoint { get; set; }

    [BooleanCommandSwitch("--non-interactive")]
    public virtual bool? NonInteractive { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }
}
