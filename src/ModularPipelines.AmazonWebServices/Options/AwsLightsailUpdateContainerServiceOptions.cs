using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-container-service")]
public record AwsLightsailUpdateContainerServiceOptions(
[property: CliOption("--service-name")] string ServiceName
) : AwsOptions
{
    [CliOption("--power")]
    public string? Power { get; set; }

    [CliOption("--scale")]
    public int? Scale { get; set; }

    [CliOption("--public-domain-names")]
    public IEnumerable<KeyValue>? PublicDomainNames { get; set; }

    [CliOption("--private-registry-access")]
    public string? PrivateRegistryAccess { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}