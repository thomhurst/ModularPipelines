using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-hsm-configuration")]
public record AwsRedshiftDeleteHsmConfigurationOptions(
[property: CliOption("--hsm-configuration-identifier")] string HsmConfigurationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}