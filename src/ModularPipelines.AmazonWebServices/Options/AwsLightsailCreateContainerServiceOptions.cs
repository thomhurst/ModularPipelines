using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "create-container-service")]
public record AwsLightsailCreateContainerServiceOptions(
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--power")] string Power,
[property: CommandSwitch("--scale")] int Scale
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--public-domain-names")]
    public IEnumerable<KeyValue>? PublicDomainNames { get; set; }

    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [CommandSwitch("--private-registry-access")]
    public string? PrivateRegistryAccess { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}