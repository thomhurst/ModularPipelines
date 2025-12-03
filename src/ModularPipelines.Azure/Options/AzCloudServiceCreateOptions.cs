using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud-service", "create")]
public record AzCloudServiceCreateOptions(
[property: CliOption("--cloud-service-name")] string CloudServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--configuration-url")]
    public string? ConfigurationUrl { get; set; }

    [CliOption("--extensions")]
    public string? Extensions { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--lb")]
    public string? Lb { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--package-url")]
    public string? PackageUrl { get; set; }

    [CliOption("--roles")]
    public string? Roles { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }

    [CliFlag("--start-cloud-service")]
    public bool? StartCloudService { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--upgrade-mode")]
    public string? UpgradeMode { get; set; }
}