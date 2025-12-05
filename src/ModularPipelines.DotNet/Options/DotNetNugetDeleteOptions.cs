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

    [CliArgument(0, Placement = ArgumentPlacement.BeforeOptions, Name = "<PACKAGE_NAME>")]
    public virtual string? PackageName { get; set; }

    [CliArgument(1, Placement = ArgumentPlacement.BeforeOptions, Name = "<PACKAGE_VERSION>")]
    public virtual string? PackageVersion { get; set; }

    [CliFlag("--force-english-output")]
    public virtual bool? ForceEnglishOutput { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliOption("--api-key")]
    public virtual string? ApiKey { get; set; }

    [CliFlag("--no-service-endpoint")]
    public virtual bool? NoServiceEndpoint { get; set; }

    [CliFlag("--non-interactive")]
    public virtual bool? NonInteractive { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }
}
