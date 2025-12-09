using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-container-service-deployment")]
public record AwsLightsailCreateContainerServiceDeploymentOptions(
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--containers")]
    public IEnumerable<KeyValue>? Containers { get; set; }

    [CliOption("--public-endpoint")]
    public string? PublicEndpoint { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}