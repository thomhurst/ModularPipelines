using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-location-nfs")]
public record AwsDatasyncCreateLocationNfsOptions(
[property: CliOption("--subdirectory")] string Subdirectory,
[property: CliOption("--server-hostname")] string ServerHostname,
[property: CliOption("--on-prem-config")] string OnPremConfig
) : AwsOptions
{
    [CliOption("--mount-options")]
    public string? MountOptions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}