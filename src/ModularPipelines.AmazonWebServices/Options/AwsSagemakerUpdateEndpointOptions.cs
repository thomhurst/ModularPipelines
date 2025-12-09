using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-endpoint")]
public record AwsSagemakerUpdateEndpointOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--endpoint-config-name")] string EndpointConfigName
) : AwsOptions
{
    [CliOption("--exclude-retained-variant-properties")]
    public string[]? ExcludeRetainedVariantProperties { get; set; }

    [CliOption("--deployment-config")]
    public string? DeploymentConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}