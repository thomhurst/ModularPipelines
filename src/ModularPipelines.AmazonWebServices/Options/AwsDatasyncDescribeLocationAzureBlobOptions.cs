using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "describe-location-azure-blob")]
public record AwsDatasyncDescribeLocationAzureBlobOptions(
[property: CliOption("--location-arn")] string LocationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}