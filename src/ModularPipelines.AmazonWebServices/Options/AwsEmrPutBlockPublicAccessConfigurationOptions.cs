using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "put-block-public-access-configuration")]
public record AwsEmrPutBlockPublicAccessConfigurationOptions(
[property: CliOption("--block-public-access-configuration")] string BlockPublicAccessConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}