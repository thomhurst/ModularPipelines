using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-container-service-deployment")]
public record AwsLightsailCreateContainerServiceDeploymentOptions(
[property: CommandSwitch("--service-name")] string ServiceName
) : AwsOptions
{
    [CommandSwitch("--containers")]
    public IEnumerable<KeyValue>? Containers { get; set; }

    [CommandSwitch("--public-endpoint")]
    public string? PublicEndpoint { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}