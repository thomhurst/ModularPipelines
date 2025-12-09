using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "update-location-nfs")]
public record AwsDatasyncUpdateLocationNfsOptions(
[property: CliOption("--location-arn")] string LocationArn
) : AwsOptions
{
    [CliOption("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CliOption("--on-prem-config")]
    public string? OnPremConfig { get; set; }

    [CliOption("--mount-options")]
    public string? MountOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}