using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-endpoint")]
public record AwsSagemakerDescribeEndpointOptions(
[property: CliOption("--endpoint-name")] string EndpointName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}