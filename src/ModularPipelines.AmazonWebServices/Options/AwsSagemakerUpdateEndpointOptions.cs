using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-endpoint")]
public record AwsSagemakerUpdateEndpointOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--endpoint-config-name")] string EndpointConfigName
) : AwsOptions
{
    [CommandSwitch("--exclude-retained-variant-properties")]
    public string[]? ExcludeRetainedVariantProperties { get; set; }

    [CommandSwitch("--deployment-config")]
    public string? DeploymentConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}