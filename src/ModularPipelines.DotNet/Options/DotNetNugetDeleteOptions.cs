using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetDeleteOptions : DotNetOptions
{
    public DotNetNugetDeleteOptions(
        string packageName,
        string packageVersion
    )
    {
        CommandParts = ["nuget", "delete", "<PACKAGE_NAME>", "<PACKAGE_VERSION>"];

        PackageName = packageName;

        PackageVersion = packageVersion;
    }

    public DotNetNugetDeleteOptions()
    {
        CommandParts = ["nuget", "delete", "<PACKAGE_NAME>", "<PACKAGE_VERSION>"];
    }

    [PositionalArgument(PlaceholderName = "<PACKAGE_NAME>")]
    public string? PackageName { get; set; }

    [PositionalArgument(PlaceholderName = "<PACKAGE_VERSION>")]
    public string? PackageVersion { get; set; }

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
