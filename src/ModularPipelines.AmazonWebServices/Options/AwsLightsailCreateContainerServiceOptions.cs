using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-container-service")]
public record AwsLightsailCreateContainerServiceOptions(
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--power")] string Power,
[property: CliOption("--scale")] int Scale
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--public-domain-names")]
    public IEnumerable<KeyValue>? PublicDomainNames { get; set; }

    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliOption("--private-registry-access")]
    public string? PrivateRegistryAccess { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}