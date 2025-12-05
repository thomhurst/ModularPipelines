using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-endpoint-config")]
public record AwsSagemakerDescribeEndpointConfigOptions(
[property: CliOption("--endpoint-config-name")] string EndpointConfigName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}