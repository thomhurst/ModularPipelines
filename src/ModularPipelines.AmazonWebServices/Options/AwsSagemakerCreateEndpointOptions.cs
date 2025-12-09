using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-endpoint")]
public record AwsSagemakerCreateEndpointOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--endpoint-config-name")] string EndpointConfigName
) : AwsOptions
{
    [CliOption("--deployment-config")]
    public string? DeploymentConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}