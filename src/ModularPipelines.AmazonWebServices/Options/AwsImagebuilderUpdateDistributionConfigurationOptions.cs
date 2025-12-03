using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "update-distribution-configuration")]
public record AwsImagebuilderUpdateDistributionConfigurationOptions(
[property: CliOption("--distribution-configuration-arn")] string DistributionConfigurationArn,
[property: CliOption("--distributions")] string[] Distributions
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}