using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "update-container-service")]
public record AwsLightsailUpdateContainerServiceOptions(
[property: CommandSwitch("--service-name")] string ServiceName
) : AwsOptions
{
    [CommandSwitch("--power")]
    public string? Power { get; set; }

    [CommandSwitch("--scale")]
    public int? Scale { get; set; }

    [CommandSwitch("--public-domain-names")]
    public IEnumerable<KeyValue>? PublicDomainNames { get; set; }

    [CommandSwitch("--private-registry-access")]
    public string? PrivateRegistryAccess { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}