using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-aqua-configuration")]
public record AwsRedshiftModifyAquaConfigurationOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--aqua-configuration-status")]
    public string? AquaConfigurationStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}